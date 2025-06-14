using Rhino;
using STAG.Constants;
using STAG.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace STAG.Views
{
    /// <summary>
    /// Interaction logic for STAGView.xaml
    /// </summary>
    public partial class STAGView
    {

        public STAGView(uint documentSerialNumber)
        {
            HardCodedData.DocumentSerialNumber = documentSerialNumber;
            DataContext = new STAGPanelViewModel(documentSerialNumber);
            InitializeComponent();
        }

        private STAGPanelViewModel ViewModel => DataContext as STAGPanelViewModel;

        private void Button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Rhino.RhinoApp.RunScript("_Line 0,0,0 5,5,0", false);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            Rhino.RhinoApp.RunScript("_Circle", false);
        }

        private void Control_VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                // Visible code here
                RhinoApp.WriteLine("STAGView now visible");
            }
            else
            {
                // Hidden code here
                RhinoApp.WriteLine("STAGView now hidden");
            }
        }
    }
}
