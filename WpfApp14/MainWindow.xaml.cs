using System.Windows;
using System.Windows.Controls;

namespace WpfApp14
{
    public partial class MainWindow : Window
    {
        private LibraryViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new LibraryViewModel();
            DataContext = viewModel;
        }

        private void AuthorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AuthorsComboBox.SelectedValue != null)
            {
                int authorId = (int)AuthorsComboBox.SelectedValue;
                viewModel.LoadBooksByAuthor(authorId);
            }
        }
    }
}