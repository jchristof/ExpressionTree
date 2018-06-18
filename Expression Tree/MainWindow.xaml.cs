
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using Expression_Tree.ViewModels;
using Newtonsoft.Json;

namespace Expression_Tree {

    public partial class MainWindow {
        public MainWindow() {
            //Properties.Settings.Default.Reset();

            var settings = Properties.Settings.Default;
            var variables = JsonConvert.DeserializeObject<Dictionary<string, string>>(settings.Variables) ?? new Dictionary<string, string>();

            InitializeComponent();
            DataContext = new ExpressionVisualizerViewModel(variables);        
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            ViewModel.Expression = settings.Expression;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "Expression") {
                Nodes.Items.Clear();
                Nodes.Items.Add(ViewModel.expressionTree._root);
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

        private void MainWindow_OnClosing(object sender, CancelEventArgs e) {
            var settings = Properties.Settings.Default;
            settings.Variables = JsonConvert.SerializeObject(ViewModel.ExpressionVariables);
            settings.Expression = ViewModel.Expression;
            settings.Save();
        }

    }
}
