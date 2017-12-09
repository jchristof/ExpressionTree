
using Expression_Tree.ViewModels;

namespace Expression_Tree {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
    }
}
