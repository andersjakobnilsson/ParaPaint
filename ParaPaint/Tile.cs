using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Globalization;

namespace ParaPaint
{
    public class Tile : INotifyPropertyChanged
    {
        public Tile(int index)
        {
            Image = new Image64(8, 8);
            TileIndex = index;
            Image.FillRandom();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string propName) { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propName)); } }

        #endregion

        #region Members

        private int tileIndex;
        private Image64 image;

        #endregion

        #region Acessors

        public int TileIndex 
        {
            get { return tileIndex; }
            set { tileIndex = value; Notify("TileIndex"); }
        }

        public Image64 Image
        {
            get { return image; }
            set { image = value; Notify("Image"); }
        }

        #endregion
    }

    public class TileVector : ObservableCollection<Tile> { }
}
