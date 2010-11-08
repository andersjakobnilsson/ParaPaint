using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ParaPaint
{
    public struct LevelTile
    {
        public int tileIndex; // Character... perhaps have as a Tile-pointer so we can more easily rearrange tile palette?
        public int color; // Color RAM
    }

    public class Level : INotifyPropertyChanged
    {
        // INotifyPropertyChanged member
        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string propName) { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propName)); }}

        private Project owner;
        private string name = "Unnamed";

        public string Name {
            get { return name; }
            set { name = value; Notify("Name"); }
           }

        private LevelTile[,] tileData;
        private int width, height;

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public LevelTile[,] TileData
        {
            get { return tileData; }
        }

        public Level(Project _owner)
        {
            owner = _owner;
        }

        public void Resize(int _width, int _height, bool keep) 
        {
            width = _width;
            height = _height;
            LevelTile[,] n = new LevelTile[width, height];

            // Copy from tileData?
            tileData = n;
        }

        static Random rnd = new Random();

        public void FillRandom(int maxTileIndex)
        {
            for (int y=0; y<height; y++) {
                for (int x = 0; x < width; x++)
                {
                    tileData[x, y].tileIndex = rnd.Next(0, maxTileIndex);
                    tileData[x, y].color = rnd.Next(0, 16);
                }
            }
        }
    }

    public class LevelVector : ObservableCollection<Level> { }
}
