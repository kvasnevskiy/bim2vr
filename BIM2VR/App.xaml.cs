using BIM2VR.Models.Import;
using BIM2VR.Models.View3D;
using BIM2VR.Views;
using Prism.Ioc;
using System.Windows;
using BIM2VR.Models.Materials;
using BIM2VR.ViewModels.MRU;
using Prism.DryIoc;
using DryIoc;

namespace BIM2VR
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
