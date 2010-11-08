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

namespace ParaPaint
{
    public partial class Editor : Window
    {
        private ProgramState program = new ProgramState();

        private Window undoStackWindow;
        private UndoStackControl undoStackControl;

        public Editor()
        {
            InitializeComponent();
            projectsMenu.DataContext = program.Projects;

            undoStackWindow = new Window();
            undoStackWindow.ShowInTaskbar = false;
            undoStackWindow.Title = "Undo Stack";
            undoStackWindow.Topmost = true;
            undoStackWindow.Width = 200;
            undoStackWindow.Height = 300;
            undoStackWindow.WindowStyle = System.Windows.WindowStyle.ToolWindow;
            undoStackControl = new UndoStackControl();
            undoStackWindow.Content = undoStackControl;

            // Make sure our components interact properly
            colorPicker.ForegroundChanged += (s, paletteColor) => { program.ActiveColors[0] = paletteColor; };
            colorPicker.BackgroundChanged += (s, paletteColor) => { program.ActiveColors[1] = paletteColor; };
            tileSelector.TileSelected += (s, tile) => { program.ActiveTile = tile; };
            program.PropertyChanged += (s, e) =>
            {
                // TODO: Should we consider all the active properties to be updated in group? We make many assumptions now...
                if (e.PropertyName == "ActiveUndoStack") { undoStackControl.SetUndoStack(program.ActiveUndoStack); }
                if (e.PropertyName == "ActiveProject") { levelsMenu.DataContext = program.ActiveProject.Levels; tileSelector.SetTiles(program.ActiveProject.Tiles); }
                if (e.PropertyName == "ActiveLevel") { level0.SetLevel(program.ActiveLevel, program.ActiveProject.Tiles, program.ActiveUndoStack); }
            };

            DataContext = program;
        }

        private Nullable<bool> SetupAndShowProjectDialog(Microsoft.Win32.FileDialog fd)
        {
            fd.FileName = "Project"; // Default file name
            fd.DefaultExt = ".ted"; // Default file extension
            fd.Filter = "Tile Editor Project (.tep)|*.tep"; // Filter files by extension
            return fd.ShowDialog();
        }

        private void Application_HasActiveProject(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = program.ActiveProject != null;
        }

        private void Application_SaveProject(object sender, ExecutedRoutedEventArgs e)
        {
            string filename = program.ActiveProject.Path;

            if (filename == null)
            {
                // Configure open file dialog box
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                if (SetupAndShowProjectDialog(dlg) == true)
                {
                    filename = dlg.FileName;
                }
                else
                {
                    return;
                }
            }

            program.ActiveProject.SaveProject(filename);
        }

        private void Application_SaveProjectAs(object sender, ExecutedRoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = program.ActiveProject.Path;
            if (SetupAndShowProjectDialog(dlg) == true)
            {
                program.ActiveProject.SaveProject(dlg.FileName);
            }
        }

        private void Application_OpenProject(object sender, ExecutedRoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (SetupAndShowProjectDialog(dlg) == true)
            {
                Project project = new Project(program);
                if (project.LoadProject(dlg.FileName))
                {
                    program.Projects.Add(project);
                }
            }
        }

        private void Action_ExportProject(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You clicked 'export project...'");
        }

        private void Application_Close(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Application_NewProject(object sender, ExecutedRoutedEventArgs e)
        {
            Project p = new Project(program);
            program.Projects.Add(p);
            program.ActiveProject = p;
        }

        private void selectedProject(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)e.OriginalSource;
            Project p = (Project)mi.Header;
            program.ActiveProject = p;
        }

        private void selectedLevel(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)e.OriginalSource;
            Level p = (Level)mi.Header;
            program.ActiveLevel = p;
        }

        private void Application_CanUndo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = program.ActiveUndoStack!= null && !program.ActiveUndoStack.Empty();
        }

        private void Application_Undo(object sender, ExecutedRoutedEventArgs e)
        {
            program.ActiveUndoStack.Undo();
        }

        private void undoStackVisible_Checked(object sender, RoutedEventArgs e)
        {
            undoStackWindow.Show();
            this.Focus();
        }

        private void undoStackVisible_Unchecked(object sender, RoutedEventArgs e)
        {
            undoStackWindow.Hide();
        }    
    }
}
