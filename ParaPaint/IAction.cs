using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ParaPaint
{
    public interface IAction : INotifyPropertyChanged
    {
        void Perform();
        void Undo();

        string Description { get; }
        string DetailedDescription { get; }
    }
}
