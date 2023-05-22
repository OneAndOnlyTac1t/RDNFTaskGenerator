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
using TDNFGenerator.ViewModel;

namespace TDNFGenerator
{
    /// <summary>
    /// Interaction logic for DisplayTestTaskWindow.xaml
    /// </summary>
    public partial class DisplayTestTaskWindow : Window
    {
        public DisplayTestTaskWindow(MainWindowViewModel mainWindowViewModel, Model.SingleTestTask task)
        {
            InitializeComponent();
            DataContext = new DisplayTestTaskWindowViewModel(mainWindowViewModel, task);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
