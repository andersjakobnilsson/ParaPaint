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
using System.Windows.Shapes;

namespace ParaPaint
{
    /// <summary>
    /// Interaction logic for UndoStackControl.xaml
    /// </summary>
    public partial class UndoStackControl : UserControl
    {
        private ActionHandler undoStack;

        public UndoStackControl()
        {
            InitializeComponent();
        }

        public void SetUndoStack(ActionHandler _undoStack)
        {
            undoStack = _undoStack;
            listbox.ItemsSource = undoStack.UndoStack;
        }
    }
}
