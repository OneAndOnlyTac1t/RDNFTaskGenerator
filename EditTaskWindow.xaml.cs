using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TDNFGenerator.Model;
using TDNFGenerator.ViewModel;

namespace TDNFGenerator.Views
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        public EditTaskWindow(SingleTestTask singleTestTask, MainWindowViewModel mwvm)
        {
            InitializeComponent();
            DataContext = new EditTaskWindowViewModel(singleTestTask, mwvm);
        }
    }
}
