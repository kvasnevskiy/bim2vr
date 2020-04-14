using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BIM2VR.Models.BIM;
using PropertyTools.Wpf;
using Xbim.Common;
using Xbim.Ifc.ViewModels;
using Xbim.Ifc4.Interfaces;

namespace BIM2VR.Controls
{
    public class TargetSelectedEventArgs : RoutedEventArgs
    {
        public IPersistEntity IfcEntity { get; }

        public TargetSelectedEventArgs(RoutedEvent routedEvent, IPersistEntity ifcEntity)
            : base(routedEvent)
        {
            IfcEntity = ifcEntity;
        }
    }

    public class BimTreeView : TreeListBox
    {
        public BimTreeView()
        {
            SelectionMode = SelectionMode.Single;
        }

        public BimStore3DModel Model
        {
            get => (BimStore3DModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        public static readonly RoutedEvent TargetSelectedEvent = EventManager.RegisterRoutedEvent("TargetSelected", RoutingStrategy.Bubble, typeof(EventHandler<TargetSelectedEventArgs>), typeof(BimTreeView));
        public event EventHandler<TargetSelectedEventArgs> TargetSelected
        {
            add => AddHandler(TargetSelectedEvent, value);
            remove => RemoveHandler(TargetSelectedEvent, value);
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(BimStore3DModel), typeof(BimTreeView), new UIPropertyMetadata(null, OnModelChanged));

        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tv = d as BimTreeView;

            if (tv != null && e.NewValue is BimStore3DModel)
            {
                tv.ViewModel();
            }
            else
            {
                if (tv != null) //unbind
                {
                    tv.UnselectAll();
                    tv.HierarchySource = Enumerable.Empty<XbimModelViewModel>();
                }
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            foreach (var addedItem in e.AddedItems)
            {
                RaiseEvent(new TargetSelectedEventArgs(TargetSelectedEvent, ((IXbimViewModel)(addedItem)).Entity));
            }
        }

        private void ViewModel()
        {
            var project = Model.Ifc.Instances.OfType<IIfcProject>().FirstOrDefault();
            if (project != null)
            {
                ChildrenPath = "Children";
                var svList = new ObservableCollection<XbimModelViewModel>
                {
                    new XbimModelViewModel(project, null)
                };
                HierarchySource = svList;
            }
        }
    }
}
