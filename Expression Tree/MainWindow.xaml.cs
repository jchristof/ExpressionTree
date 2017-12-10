
using System.Windows.Controls;
using Expression_Tree.ViewModels;

namespace Expression_Tree {

    public partial class MainWindow {
        public MainWindow() {

            DataContext = new ExpressionVisualizerViewModel();
            InitializeComponent();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "Expression") {
                Nodes.Items.Clear();
                Nodes.Items.Add(ViewModel.expressionTree.Root);
            }
        }

        ExpressionVisualizerViewModel ViewModel => DataContext as ExpressionVisualizerViewModel;

        private void DataGrid_OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e) {
            if (!(sender is DataGrid dataGrid))
                return;

            if (dataGrid.SelectedItem == null)
                return;

            dataGrid.RowEditEnding -= DataGrid_OnRowEditEnding;
            dataGrid.CommitEdit();
            dataGrid.Items.Refresh();
            dataGrid.RowEditEnding += DataGrid_OnRowEditEnding;
            
            ViewModel.RowAdded(dataGrid.SelectedItem);         
            
        }
    }
}
