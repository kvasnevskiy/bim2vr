using BIM2VR.Models.BIM;
using BIM2VR.ViewModels.BIM;

namespace BIM2VR.Models.Visualizer
{
    public interface IVisualizerManager
    {
        void GenerateScene(BimStore3DViewModel store);
        void Run(BimStore3DViewModel store);
    }
}