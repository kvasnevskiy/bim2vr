using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using BIM2VR.Models.BIM;
using BIM2VR.Models.Materials;
using BIM2VR.Models.Visualizer.Data;
using BIM2VR.ViewModels.BIM;
using Newtonsoft.Json;

namespace BIM2VR.Models.Visualizer
{
    public class VisualizerManagerModel : IVisualizerManager
    {
        private readonly IMaterialManager materialManager;

        private static string GeometrySourcesPath => Path.Combine(ConfigurationManager.AppSettings["VisualizerDirectoryPath"], "GeometrySources");
        private static string SceneDataPath => Path.Combine(ConfigurationManager.AppSettings["VisualizerDirectoryPath"], "scene.json");
        private static string VisualizerExecutivePath => Path.Combine(ConfigurationManager.AppSettings["VisualizerDirectoryPath"], ConfigurationManager.AppSettings["VisualizerExecutiveName"]);

        public VisualizerManagerModel(IMaterialManager materialManager)
        {
            this.materialManager = materialManager;
        }

        public void GenerateScene(BimStore3DViewModel store)
        {
            var sceneData = new SceneData(store.Model, materialManager);
            sceneData.SaveGeometrySources(GeometrySourcesPath);

            var sceneDataJson = JsonConvert.SerializeObject(sceneData, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                TypeNameHandling = TypeNameHandling.None,
                Formatting = Formatting.Indented,
                Context = new StreamingContext(StreamingContextStates.All)
            });

            using (var streamWriter = new StreamWriter(SceneDataPath))
            {
                streamWriter.Write(sceneDataJson);
            }
        }

        public void Run(BimStore3DViewModel store)
        {
            GenerateScene(store);

            Process.Start(new ProcessStartInfo(VisualizerExecutivePath)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true,
                Arguments = $"-JsonScenedata=\"{Path.GetFullPath(SceneDataPath)}\""
            });
        }
    }
}
