using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    /// <summary>
    /// BeatItemsListView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatItemsListView : UserControl, ISelectItemsContainer, IDragSelect
    {
        private readonly SelectedItemsCollection _selectedItemsCollection;
        private readonly DragSelectAction _dragSelectAction;
        public DragSelectAction DragSelectAction => _dragSelectAction;
        public SelectedItemsCollection SelectedItemsCollection => _selectedItemsCollection;
        public ItemCollection Items => PART_ItemsControl.Items;
        public event EventHandler<ItemsControlSelectionChangedEventArgs> ItemsControlSelectionChanged;

        public BeatItemsListView()
        {
            _selectedItemsCollection = new SelectedItemsCollection(this);
            _dragSelectAction = new DragSelectAction(this);
            InitializeComponent();
            Unloaded += BeatItemsListView_Unloaded;
        }

        public void OnItemsControlSelectionChanged(ItemsControlSelectionChangedEventArgs e)
        {
            ItemsControlSelectionChanged?.Invoke(this, e);
        }

        public void RenderDragSelectMask(GeometryDrawing geometryDrawing)
        {
            PART_SelectMask.DrawingHandler((drawingContext) =>
            {
                if(geometryDrawing != null)
                {
                    drawingContext.DrawDrawing(geometryDrawing);
                }
            });
        }

        private void BeatItemsListView_Unloaded(object sender, RoutedEventArgs e)
        {
            _dragSelectAction.Dispose();
            Unloaded -= BeatItemsListView_Unloaded;
        }
    }
}
