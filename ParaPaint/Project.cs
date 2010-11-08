using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ParaPaint
{
    public class Project : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged member

        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string propName) { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propName)); } }

        #endregion

        #region Members

        private ProgramState owner;
        private TileVector tiles = new TileVector();
        private LevelVector levels = new LevelVector();
        private string name = "Unnamed";
        private string path; // TODO: Move to programState in a project<>program-mapping
        private ActionHandler undoStack = new ActionHandler();

        #endregion

        #region Accessors

        public string Name
        {
            get { return name; }
            set { name = value; Notify("Name"); }
        }
        public string Path
        {
            get { return path; }
            set { path = value; Notify("Path"); }
        }

        public LevelVector Levels { get { return levels; } }
        public TileVector  Tiles  { get { return tiles;  } }

        public ActionHandler UndoStack
        {
            get { return undoStack; }
        }

        #endregion Accessor

        public Project(ProgramState _owner)
        {
           owner = _owner;

           // Add some tiles
           for (int k = 0; k < 8; k++)
           {
               tiles.Add(new Tile(k));
           }

           // Add two levels
           for (int k = 0; k < 2; k++)
           {
               Level l = new Level(this);
               l.Resize(2048, 25, true);
               l.FillRandom(tiles.Count);
               levels.Add(l);
           }
        }

        public void SaveProject(string savePath) 
        {
            System.Windows.MessageBox.Show("Saving to " + savePath);
            this.Path = savePath;
        }

        public bool LoadProject(string loadPath) 
        {
            this.Path = loadPath;
            return true;
        }
    }

    public class ProjectVector : ObservableCollection<Project> { }
}
