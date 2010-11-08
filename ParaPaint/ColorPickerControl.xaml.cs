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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ParaPaint
{
    public class PaletteColor : INotifyPropertyChanged
    {
        // INotifyPropertyChanged member
        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string propName) { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propName)); } }

        private Color color;
        private Color complementColor;
        private Brush brush;
        private Brush complementBrush;
        private int index;
        private string name;

        public int Index
        {
            get { return index; }
            set { index = value; Notify("Index"); }
        }
        public string Name
        {
            get { return name; }
            set { name = value; Notify("Name"); }
        }
        public Color Color
        {
            get { return color; }
            set { color = value; Notify("Color"); }
        }
        public Color ComplementColor
        {
            get { return complementColor; }
            set { complementColor = value; Notify("ComplementColor"); }
        }
        public Brush Brush
        {
            get { return brush; }
            set { brush = value; Notify("Brush"); }
        }
        public Brush ComplementBrush
        {
            get { return complementBrush; }
            set { complementBrush = value; Notify("ComplementBrush"); }
        }
    }

    public class PaletteColorVector : ObservableCollection<PaletteColor> { }

    /// <summary>
    /// Interaction logic for ColorPickerControl.xaml
    /// </summary>
    public partial class ColorPickerControl : UserControl
    {
        public static PaletteColorVector PaletteColors = new PaletteColorVector();

        static ColorPickerControl()
        {
            List<Color> colorVector = new List<Color>();
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FF000000"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FF3E31A2"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FF574200"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FF8C3E34"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FF545454"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FF8D47B3"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FF905F25"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FF7C70DA"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FF808080"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FF68A941"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FFBB776D"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FF7ABFC7"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FFABABAB"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FFD0DC71"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FFACEA88"));
            colorVector.Add((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));

            for (int i = 0; i<16; i++) {
                PaletteColor pc = new PaletteColor();
                pc.Color = colorVector[i];
                pc.ComplementColor = i == 0 ? Colors.White : Colors.Black;
                pc.Brush = new SolidColorBrush(pc.Color);
                pc.ComplementBrush = i == 0 ? Brushes.White : Brushes.Black;
                pc.Index = i;
                pc.Name = String.Format("{0,2:X0}", i);
                PaletteColors.Add(pc);
            }
        }

        public delegate void ChangedColorEventHandler(object sender, PaletteColor paletteColor);
        public event ChangedColorEventHandler ForegroundChanged;
        public event ChangedColorEventHandler BackgroundChanged;

        public ColorPickerControl()
        {
            InitializeComponent();

            foreach (PaletteColor pc in PaletteColors)
            {
                Button b = new Button();
                b.UseLayoutRounding = true;
                b.Foreground = pc.ComplementBrush;
                b.Background = pc.Brush;
                b.Content = pc.Name;
                b.Height = 32;
                b.Width = 32;
                b.Margin = new System.Windows.Thickness(1);
                PaletteColor pcc = pc;
                b.Click += (s, e) => { if (ForegroundChanged!=null) ForegroundChanged(this, pcc); };
                b.MouseRightButtonUp += (s, e) => { if (BackgroundChanged != null) BackgroundChanged(this, pcc); };
                uniformGrid.Children.Add(b);
            }
        }
    }
}

