using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BIM2VR.Controls;
using BIM2VR.Models.Import;
using BIM2VR.Models.Materials;
using BIM2VR.Models.View3D;
using BIM2VR.Models.Visualizer;
using BIM2VR.ViewModels.BIM;
using BIM2VR.ViewModels.Busy;
using BIM2VR.ViewModels.MRU;
using BIM2VR.Views;
using Fluent;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model;
using Microsoft.Win32;
using Prism.Mvvm;
using SharpDX;

namespace BIM2VR.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Backstage

        private Backstage backstage;
        private void CloseBackstage()
        {
            backstage.IsOpen = false;
        }

        #endregion

        #region Title

        public string Title => "BIM2VR";

        #endregion

        #region Model

        private async void LoadModel(string fileName)
        {
            CloseBackstage();
            BusyManager.SetBusy(0, 100, "LoadModel");
            var bimStore3D = await ImportManager.LoadAsync(fileName);
            var model = new BimStore3DViewModel(bimStore3D, MaterialManager);
            View3DManager.SetModel(model);
            BusyManager.SetFree();
        }

        #endregion

        #region Assistants

        public IMaterialManager MaterialManager { get; }
        public IImportManager ImportManager { get; }
        public IView3DManager View3DManager { get; }
        public IBimModelsMostRecentlyUsedManager BimModelsMostRecentlyUsedManager { get; }
        public IBusyManager BusyManager { get; }
        public IVisualizerManager VisualizerManager { get; }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            MaterialManager = new MaterialManagerModel();
            ImportManager = new ImportManagerModel((i, o) =>
            {
                BusyManager.SetValue(i);
                BusyManager.SetMessage(o as string);
            }, MaterialManager);
            View3DManager = new View3DManagerModel();
            BimModelsMostRecentlyUsedManager = new BimModelsMostRecentlyUsedManagerModel();
            BusyManager = new BusyManagerModel();
            VisualizerManager = new VisualizerManagerModel(MaterialManager);
        }

        #endregion

        #region Event Handlers Menu

        public void RecentModelClickHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != null && ItemsControl.ContainerFromElement((ListBox)sender, e.OriginalSource as DependencyObject ?? throw new InvalidOperationException()) is ListBoxItem item)
            {
                var bimModelPath = item.DataContext as string;

                if (File.Exists(bimModelPath))
                {
                    LoadModel(bimModelPath);
                }
                else
                {
                    BimModelsMostRecentlyUsedManager.Remove(bimModelPath);
                }
            }
        }

        public void BackstageLoadedHandler(object sender, RoutedEventArgs e)
        {
            backstage = sender as Backstage;
        }

        public void ViewportLoadedHandler(object sender, RoutedEventArgs e)
        {
            View3DManager.Initialize(sender as Viewport3DX);
        }

        public void ImportHandler()
        {
            var dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                LoadModel(dlg.FileName);
                BimModelsMostRecentlyUsedManager.Add(dlg.FileName);
            }
        }

        public void ShowWireframeHandler()
        {
            View3DManager.EnableWireframe();
        }

        public void HideWireframeHandler()
        {
            View3DManager.DisableWireframe();
        }

        public void ShowVisualizer3DHandler()
        {
            if (View3DManager.Model != null)
            {
                VisualizerManager.Run(View3DManager.Model);
            }
        }

        public void OnStructureElementSelected(object sender, TargetSelectedEventArgs e)
        {
            var element = View3DManager.Model.Items.FirstOrDefault(item => item.IfcEntity.Equals(e.IfcEntity));

            if (element != null)
            {
                View3DManager.HighlightItem(element);
            }
            else
            {
                View3DManager.ClearItemsHighlight();
            }
        }

        public void OpenMaterialDispatcherHandler()
        {
            var materialDispatcherWindow = new MaterialDispatcherWindow(MaterialManager, View3DManager);
            materialDispatcherWindow.Show();
        }

        #endregion
    }
}
