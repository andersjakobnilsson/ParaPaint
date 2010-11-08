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

namespace ParaPaint
{
    /// <summary>
    /// Interaction logic for TileDrawerControl.xaml
    /// </summary>
    public partial class TileDrawerControl : UserControl
    {
        public TileDrawerControl()
        {
            InitializeComponent();
            Image64Convertor a = (Image64Convertor)this.FindResource("image64Converter");
            a.palette = 0x1234;
            a.multicolor = false;
        }
    }
}
