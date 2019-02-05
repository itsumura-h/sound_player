using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace soundPlayerWPF
{
    public partial class MainWindow : Window
    {
        //public ObservableCollection<SongData> _rowsList { get; set; } →DataGrid.csで宣言
        public bool IsEditing { get; set; }
        public bool IsDragging { get; set; }

        public static readonly DependencyProperty DraggedItemProperty =
        DependencyProperty.Register("DraggedItem", typeof(SongData), typeof(MainWindow));

        public SongData DraggedItem
        {
            get { return (SongData)GetValue(DraggedItemProperty); }
            set { SetValue(DraggedItemProperty, value); }
        }

        //------------------------------------------------------------------------


        private void dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            IsEditing = true;
            if (IsDragging) ResetDragDrop();
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            IsEditing = false;
        }

        private void dataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsEditing) return;

            var row = UIHelpers.TryFindFromPoint<DataGridRow>((UIElement)sender, e.GetPosition(dataGrid));
            if (row == null || row.IsEditing) return;

            IsDragging = true;
            DraggedItem = (SongData)row.Item;
        }

        private void ResetDragDrop()
        {
            IsDragging = false;
            popup1.IsOpen = false;
        }

        private void LayoutRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsDragging || IsEditing)
            {
                return;
            }

            SongData targetItem = (SongData)dataGrid.SelectedItem;

            if (targetItem == null || !ReferenceEquals(DraggedItem, targetItem))
            {
                _rowsList.Remove(DraggedItem);

                var targetIndex = _rowsList.IndexOf(targetItem);

                _rowsList.Insert(targetIndex, DraggedItem);

                dataGrid.SelectedItem = DraggedItem;
            }

            ResetDragDrop();

            //IDを整列
            for(int i = 0; i < _rowsList.Count; i++)
            {
                _rowsList[i].ID = i;
            }
        }

        private void LayoutRoot_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDragging || e.LeftButton != MouseButtonState.Pressed) return;

            if (!popup1.IsOpen)
            {
                dataGrid.IsReadOnly = true;

                popup1.IsOpen = true;
            }


            Size popupSize = new Size(popup1.ActualWidth, popup1.ActualHeight);
            popup1.PlacementRectangle = new Rect(e.GetPosition(this), popupSize);

            Point position = e.GetPosition(dataGrid);
            var row = UIHelpers.TryFindFromPoint<DataGridRow>(dataGrid, position);
            if (row != null) dataGrid.SelectedItem = row.Item;
        }

    }
}
