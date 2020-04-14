using System;
using System.Collections.Generic;
using System.Windows;
using BIM2VR.Models.Materials;
using BIM2VR.Models.View3D;
using BIM2VR.ViewModels;

namespace BIM2VR.Views
{
    public partial class MaterialDispatcherWindow : Window
    {
        public MaterialDispatcherWindowViewModel Manager => DataContext as MaterialDispatcherWindowViewModel;

        public MaterialDispatcherWindow(IMaterialManager materialManager, IView3DManager view3DManager)
        {
            InitializeComponent();
            Manager.Initialize(materialManager, view3DManager);
        }
    }
}
