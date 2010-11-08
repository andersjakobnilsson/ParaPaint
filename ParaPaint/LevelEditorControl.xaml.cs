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
using ParaPaint.Actions;

namespace ParaPaint
{
    /// <summary>
    /// Interaction logic for LevelEditor.xaml
    /// </summary>
    public partial class LevelEditorControl : UserControl
    {
        private Level level;
        private TileVector tiles;
        private ActionHandler undoStack;
        private UInt16 palette;
        private bool multicolor;

        public LevelEditorControl()
        {
            InitializeComponent();
            palette = 0x1234;
            multicolor = false;
        }
    
        private void Resize()
        {
            if (level == null)
            {
                _wb = null;
                _colorArray = null;
                return;
            }

            _width = level.Width * 8;
            _height = level.Height * 8;

            if (_wb==null || _wb.Width != _width || _wb.Height != _height)
            {
                _wb = new WriteableBitmap(_width, _height, 96, 96, PixelFormats.Bgra32, null);

                int bytesPerPixel = (_wb.Format.BitsPerPixel + 7) / 8;
                int stride = _wb.PixelWidth * bytesPerPixel;

                _colorArray = new byte[stride * _wb.PixelHeight];

                levelImage.Width = _width * _zoomFactor;
                levelImage.Height = _height * _zoomFactor;

                DrawLevel();
            }
        }

        public void DrawLevel()
        {
            int bytesPerPixel = (_wb.Format.BitsPerPixel + 7) / 8;
            int stride = _wb.PixelWidth * bytesPerPixel;

            int w = level.Width;
            int h = level.Height;

            for (int ty = 0; ty < h; ty++)
            {
                for (int tx = 0; tx < w; tx++)
                {
                    LevelTile lt = level.TileData[tx, ty];
                    UInt16 tilePalette = (UInt16)(((palette & 0xff0f)) | ((UInt16)lt.color << 4));
                    Tile t = tiles[lt.tileIndex];
                    t.Image.toPixels(tilePalette, multicolor, _colorArray, tx*8*4+ty*8*stride, stride );
                }
            }

            _wb.WritePixels(new Int32Rect(0,0,w*8, h*8), _colorArray, stride, 0);

            levelImage.Source = _wb;
        }

        public void SetLevel(Level _level, TileVector _tiles, ActionHandler _undoStack) 
        {
            level = _level;
            tiles = _tiles;
            undoStack = _undoStack;            
            Resize();
        }

        private double _zoomFactor = 1;
        private int _width, _height;
        private WriteableBitmap _wb;
        private byte[] _colorArray;

        Random rnd = new Random();

        private void changeZoomLevel(object sender, MouseWheelEventArgs e)
        {
            _zoomFactor *= (1.0 + 10.0/e.Delta);
            if (_zoomFactor<0.1) {
                _zoomFactor =0.1;
            }
            levelImage.Width = _width * _zoomFactor;
            levelImage.Height = _height * _zoomFactor;
        }

        private Point getTilePos(MouseEventArgs e)
        {
            Point p = e.GetPosition(levelImage);

            double x = (p.X * _width / levelImage.Width ) / 8.0;
            double y = (p.Y * _height / levelImage.Height) / 8.0;

            return new Point(x, y);            
        }

        private bool isValid(Point p)
        {
            if (p.X < 0 || p.Y < 0 || p.X >= level.Width || p.Y >= level.Height)
            {
                return false;
            }
            return true;
        }

        private void updateTile(int tx, int ty, int ti, int tc) 
        {
            int bytesPerPixel = (_wb.Format.BitsPerPixel + 7) / 8;
            int stride = _wb.PixelWidth * bytesPerPixel;

            int w = level.Width;
            int h = level.Height;

            LevelTile lt = level.TileData[tx, ty];
            UInt16 tilePalette = (UInt16)tc; // (UInt16)(((palette & 0xf00f)) + ((UInt16)tc << 8));
            Tile t = tiles[ti];

            byte[] cache = new byte[8 * 8 * 4];
            t.Image.toPixels(tilePalette, multicolor, cache, 0, 8*4);
            _wb.WritePixels(new Int32Rect(tx*8, ty*8, 8, 8), cache, 8*4, 0);

            levelImage.Source = _wb;
        }

        private void levelImage_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = getTilePos(e);

            

            //updateTile(tx, ty, rnd.Next(0,tiles.Count), rnd.Next(0,16));
        }

        private void levelImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = getTilePos(e);
            if (isValid(p))
            {
                int tx = (int)p.X;
                int ty = (int)p.Y;
            }
        }

        private void levelImage_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
