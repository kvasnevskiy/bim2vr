using System.Threading.Tasks;
using BIM2VR.Models.BIM;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model.Scene;

namespace BIM2VR.Models.Import
{
    public interface IImportManager
    {
        BimStore3DModel Load(string fileName);
        Task<BimStore3DModel> LoadAsync(string fileName);
    }
}