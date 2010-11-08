using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ParaPaint.Actions
{
    public class ActionUpdateLevelTiles : IAction
    {
        #region INotifyPropertyChanged member

        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string propName) { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propName)); } }

        #endregion

        virtual public void Perform()
        {
        }

        virtual public void Undo()
        {
        }

        public string Description { get { return "Level tiles updated"; } }
        public string DetailedDescription { get { return "Level tiles really updated"; } }
    }
}
