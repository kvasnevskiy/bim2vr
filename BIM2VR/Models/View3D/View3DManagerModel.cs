using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using BIM2VR.Models.Materials;
using BIM2VR.ViewModels.BIM;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model;
using Prism.Mvvm;
using SharpDX;
using Camera = HelixToolkit.Wpf.SharpDX.Camera;
using Color = System.Windows.Media.Color;
using Material = HelixToolkit.Wpf.SharpDX.Material;

namespace BIM2VR.Models.View3D
{
    public class View3DManagerModel : BindableBase, IView3DManager
    {
        private Viewport3DX viewport;

        private bool isWireframeEnabled;
        public bool IsWireframeEnabled
        {
            get => isWireframeEnabled;
            set => SetProperty(ref isWireframeEnabled, value);
        }

        public void UpdateMaterials(IEnumerable<MaterialModel> materials)
        {
            foreach (var material in materials)
            {
                foreach (var owner in material.Owners)
                {
                    foreach (var item in Model.Items)
                    {
                        foreach (var geometry in item.Geometries)
                        {
                            if (geometry.Id == owner)
                            {
                                geometry.Material = material.ToPBR();
                            }
                        }
                    }
                }
            }
        }

        public void HighlightMaterialOwners(IEnumerable<Guid> owners)
        {
            SelectedGeometries = new ObservableCollection<BimGeometry3DViewModel>();

            foreach (var owner in owners)
            {
                foreach (var item in Model.Items)
                {
                    foreach (var geometry in item.Geometries)
                    {
                        if (geometry.Id == owner)
                        {
                            SelectedGeometries.Add(geometry);
                        }
                    }
                }
            }
        }

        #region Highlighting

        private ObservableCollection<BimStoreItem3DViewModel> selectedItems;
        public ObservableCollection<BimStoreItem3DViewModel> SelectedItems
        {
            get => selectedItems;
            set => SetProperty(ref selectedItems, value);
        }

        private ObservableCollection<BimGeometry3DViewModel> selectedGeometries;
        public ObservableCollection<BimGeometry3DViewModel> SelectedGeometries
        {
            get => selectedGeometries;
            set => SetProperty(ref selectedGeometries, value);
        }

        public Material HighlightMaterial { get; }

        #endregion

        #region Camera

        public Camera Camera { get; private set; }
        public Vector3D UpDirection => new Vector3D(0, 0, 1);
        private double cameraFOV = 90.0;
        private double nearPlaneDistance = 5.0;
        private double farPlaneDistance = 500000.0;

        private void InitCamera()
        {
            Camera = new HelixToolkit.Wpf.SharpDX.PerspectiveCamera()
            {
                FieldOfView = cameraFOV,
                LookDirection = new Vector3D(0, -10, -10),
                Position = new Point3D(0, 10, 10),
                UpDirection = UpDirection,
                NearPlaneDistance = nearPlaneDistance,
                FarPlaneDistance = farPlaneDistance
            };
        }

        #endregion

        #region Effects Manager

        public IEffectsManager EffectsManager { get; private set; }

        protected void InitEffectsManager()
        {
            EffectsManager = new DefaultEffectsManager();
        }

        #endregion

        #region Lights

        public Color DirectionalLightColor { get; private set; }
        public Color AmbientLightColor { get; private set; }

        protected void InitLights()
        {
            AmbientLightColor = Color.FromRgb(0x20, 0x20, 0x20);
            DirectionalLightColor = Colors.White;
        }

        #endregion

        #region Environment

        public Stream BackgroundTexture { get; private set; }

        private static MemoryStream LoadFileToMemory(string filePath)
        {
            try
            {
                using (var file = new FileStream(filePath, FileMode.Open))
                {
                    var memory = new MemoryStream();
                    file.CopyTo(memory);
                    return memory;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        protected void InitEnvironment()
        {
            BackgroundTexture = BitmapExtensions.CreateLinearGradientBitmapStream(EffectsManager, 128, 128, Direct2DImageFormat.Bmp,
                new Vector2(0, 0), new Vector2(0, 128), new SharpDX.Direct2D1.GradientStop[]
                {
                    new SharpDX.Direct2D1.GradientStop(){ Color = Colors.White.ToColor4(), Position = 0f },
                    new SharpDX.Direct2D1.GradientStop(){ Color = Colors.DarkGray.ToColor4(), Position = 1f }
                });
        }

        #endregion

        #region Model

        private BimStore3DViewModel model;
        public BimStore3DViewModel Model
        {
            get => model;
            set => SetProperty(ref model, value);
        }

       
        public void SetModel(BimStore3DViewModel inputModel)
        {
            ClearItemsHighlight();
            Model = inputModel;
            ZoomToAll();
        }

        public void ZoomToAll()
        {
            var viewportDispatcher = this.viewport.Dispatcher;
            viewportDispatcher?.BeginInvoke(DispatcherPriority.ContextIdle,
                new Action(delegate()
                {
                    this.viewport.ZoomExtents(200.0);
                }));
        }

        public void EnableWireframe()
        {
            IsWireframeEnabled = true;
        }

        public void DisableWireframe()
        {
            IsWireframeEnabled = false;
        }

        public void HighlightItem(BimStoreItem3DViewModel item)
        {
            SelectedItems = new ObservableCollection<BimStoreItem3DViewModel>
            {
                item
            };
        }

        public void ClearMaterialOwnersHighlight()
        {
            SelectedGeometries?.Clear();
            SelectedGeometries = null;
        }

        public void ClearItemsHighlight()
        {
            SelectedItems?.Clear();
            SelectedItems = null;
        }

        #endregion

        #region Initialize


        public void Initialize(Viewport3DX inputViewport)
        {
            this.viewport = inputViewport;
        }

        #endregion

        #region Constructors


        public View3DManagerModel()
        {
            InitCamera();
            InitLights();
            InitEffectsManager();
            InitEnvironment();

            HighlightMaterial = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial(new DiffuseMaterialCore
            {
                DiffuseColor = Colors.Red.ToColor4()
            });
        }

        #endregion
    }
}
