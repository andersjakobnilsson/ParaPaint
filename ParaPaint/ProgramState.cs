using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ParaPaint
{
    public class ProgramState : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string propName) { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propName)); } }

        #endregion

        public ProgramState()
        {
            activeColors.Add(ColorPickerControl.PaletteColors[0]);
            activeColors.Add(ColorPickerControl.PaletteColors[1]);
            activeColors.Add(ColorPickerControl.PaletteColors[2]);
            activeColors.Add(ColorPickerControl.PaletteColors[3]);
        }

        #region Members

        private ProjectVector projects = new ProjectVector();

        public ProjectVector Projects
        {
            get { return projects; }
        }

        private ActionHandler activeUndoStack;            
        private Project activeProject;
        private PaletteColorVector activeColors = new PaletteColorVector();
        private Tile activeTile;
        private Level activeLevel;

        #endregion

        #region Accessors

        public Project ActiveProject
        {
            get { return activeProject; }
            set
            {
                // Check order here
                activeProject = value;
                activeTile  = activeProject.Tiles .Count == 0 ? null : activeProject.Tiles[0];
                activeLevel = activeProject.Levels.Count == 0 ? null : activeProject.Levels[0];
                ActiveUndoStack = activeProject.UndoStack;
                Notify("ActiveProject");
                Notify("ActiveTile");
                Notify("ActiveLevel");
            }
        }
        public ActionHandler ActiveUndoStack
        {
            get { return activeUndoStack; }
            set { activeUndoStack = value; Notify("ActiveUndoStack"); }
        }            
        public Level ActiveLevel
        {
            get { return activeLevel; }
            set { activeLevel = value; Notify("ActiveLevel"); }
        }

        public Tile ActiveTile
        {
            get { return activeTile; }
            set { activeTile = value; Notify("ActiveTile"); }
        }
        public PaletteColorVector ActiveColors
        {
            get { return activeColors; }
        }

        #endregion
    }
}
