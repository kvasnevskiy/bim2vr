using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using BIM2VR.Models.BIM;
using BIM2VR.Models.Materials;
using BIM2VR.Models.Visualizer.Data.ConstructiveMesh;
using BIM2VR.Models.Visualizer.Data.GeometrySource;
using BIM2VR.Models.Visualizer.Data.MaterialSource;
using BIM2VR.Models.Visualizer.Data.MaterialSource.Attributes;
using BIM2VR.Models.Visualizer.Data.Mesh;
using BIM2VR.Models.Visualizer.Data.TextureSource;
using Newtonsoft.Json;

namespace BIM2VR.Models.Visualizer.Data
{
    public class SceneData
    {
        #region Serializable Data

        public int Version { get; set; } = 0;

        public List<GeometrySourceData> GeometrySources { get; set; }

        public List<ConstructiveMeshData> ConstructiveMeshes { get; set; }

        public List<MaterialSourceData> MaterialSources { get; set; }

        public List<TextureSourceData> TextureSources { get; set; }

        #endregion

        #region Inner Data

        private List<MeshData> Meshes { get; set; }

        #endregion

        public SceneData(BimStore3DModel store, IMaterialManager materialManager)
        {
            Meshes = new List<MeshData>();
            GeometrySources = new List<GeometrySourceData>();
            ConstructiveMeshes = new List<ConstructiveMeshData>();
            MaterialSources = new List<MaterialSourceData>();

            //Materials and Textures
            var uniqueTextures = new Dictionary<Guid, TextureSourceData>();

            foreach (var material in materialManager.Materials)
            {
                MaterialSources.Add(new MaterialSourceData(material.Value));

                //Get textures
                var propertyInfos = material.Value.GetType().GetProperties();

                foreach (var propertyInfo in propertyInfos)
                {
                    var attributes = propertyInfo.GetCustomAttributes(typeof(VisualizerTextureMaterialParameter), true);

                    if (attributes.Length > 0)
                    {
                        var texture = (TextureModelEx)propertyInfo.GetValue(material.Value);

                        if (texture != null)
                        {
                            if (!uniqueTextures.ContainsKey(texture.Id))
                            {
                                uniqueTextures.Add(texture.Id, new TextureSourceData(texture));
                            }
                        }
                    }
                }
            }

            TextureSources = uniqueTextures.Values.ToList();

            //Meshes + Geometries + ConstructiveMeshes
            foreach (var item in store.Items)
            {
                foreach (var geometry in item.Geometries)
                {
                    var meshData = new MeshData(geometry);
                    Meshes.Add(meshData);
                    GeometrySources.Add(new GeometrySourceData(meshData, @"GeometrySources"));
                    ConstructiveMeshes.Add(new ConstructiveMeshData(geometry, meshData.Id, geometry.Material.Id));
                }
            }
        }

        public void SaveGeometrySources(string sourceFolder)
        {
            if (!Directory.Exists(sourceFolder))
            {
                Directory.CreateDirectory(sourceFolder);
            }

            foreach (var mesh in Meshes)
            {
                var meshJson = JsonConvert.SerializeObject(mesh, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    TypeNameHandling = TypeNameHandling.None,
                    Formatting = Formatting.Indented,
                    Context = new StreamingContext(StreamingContextStates.All)
                });

                using (var streamWriter = new StreamWriter(Path.Combine(sourceFolder, $"{mesh.Id}.json")))
                {
                    streamWriter.Write(meshJson);
                }
            }
        }

        //public void SaveTextureSources(string sourceFolder)
        //{
        //    if (!Directory.Exists(sourceFolder))
        //    {
        //        Directory.CreateDirectory(sourceFolder);
        //    }

        //    //Clear directory
        //    var directory = new DirectoryInfo(sourceFolder);

        //    foreach (var file in directory.GetFiles())
        //    {
        //        file.Delete();
        //    }

        //    foreach (var textureSource in TextureSources)
        //    {

        //        File.Copy(textureSource.Path, );
        //    }
        //}
    }
}
