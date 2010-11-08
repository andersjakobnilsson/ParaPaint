using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace ParaPaint
{
    public class Image64 : INotifyPropertyChanged
    {
        public Image64(int w, int h)
        {
            imageData = new bool[w * h];
            width = w;
            height = h;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string propName) { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propName)); } }

        #endregion

        #region Members

        private bool[] imageData;
        private int width;
        private int height;
        private int revision = 0;

        #endregion

        #region Acessors

        public bool[] ImageData
        {
            get { return imageData; }
        }
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }
        public int Revision
        {
            get { return revision; }
            set { revision = value; Notify("ImageData"); Notify("Revision"); }
        }

        #endregion

        private static Random rnd = new Random();

        public void FillRandom()
        {
            for (int i = 0; i < width * height; i++)
            {
                imageData[i] = rnd.Next(0, 2) == 0;
            }
            Revision++;
        }

        public void toPixels(UInt16 palette, bool multicolor, byte[] pixelBuffer, int baseOffset, int stride)
        {
            Color[] pc = new Color[4];
            for (int k = 0; k < 4; k++)
            {
                pc[k] = ColorPickerControl.PaletteColors[(palette >> (k * 4)) & 0xf].Color;
            }

            int destOfs = baseOffset;
           
            for (int y = 0, ofs = 0; y < height; y++)
            {
                if (multicolor)
                {
                    for (int x = 0; x < width; x += 2, ofs += 2, destOfs+=8)
                    {
                        int color = 0;
                        if (imageData[ofs + 0]) color += 1;
                        if (imageData[ofs + 1]) color += 2;
                        Color c = pc[color];
                        pixelBuffer[destOfs + 0] = c.B;
                        pixelBuffer[destOfs + 1] = c.G;
                        pixelBuffer[destOfs + 2] = c.R;
                        pixelBuffer[destOfs + 3] = 255;
                        pixelBuffer[destOfs + 4] = c.B;
                        pixelBuffer[destOfs + 5] = c.G;
                        pixelBuffer[destOfs + 6] = c.R;
                        pixelBuffer[destOfs + 7] = 255;
                    }
                } 
                else 
                {
                    for (int x = 0; x < width; x++, ofs ++, destOfs+=4)
                    {
                        Color c = pc[imageData[ofs + 0] ? 1 : 0];
                        pixelBuffer[destOfs + 0] = c.B;
                        pixelBuffer[destOfs + 1] = c.G;
                        pixelBuffer[destOfs + 2] = c.R;
                        pixelBuffer[destOfs + 3] = 255;
                    }
                }
                destOfs += stride - width * 4;
            }
        }

        public byte[] toPixels(UInt16 palette, bool multicolor) 
        {
            var pixelBuffer = new byte[width * height * 4];
            toPixels(palette, multicolor, pixelBuffer, 0, 8*4);
            return pixelBuffer; // Cache these? Add revision
        }
    }

    [ValueConversion(typeof(Image64), typeof(ImageSource))]
    public sealed class Image64Convertor : IValueConverter
    {
        public UInt16 palette = 0;
        public bool multicolor = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Image64 t = (Image64)value;
                WriteableBitmap wb = new WriteableBitmap(t.Width, t.Height, 96, 96, PixelFormats.Bgra32, null);
                wb.WritePixels(new Int32Rect(0, 0, t.Width, t.Height), t.toPixels(palette, multicolor), t.Width * 4, 0);
                return wb;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
