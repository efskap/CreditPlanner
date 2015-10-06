using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
namespace CreditPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Course> mainList { get; set; }
        public List<Requirement> requirementList { get; set; }

        public MainWindow()
        {
            if (File.Exists("config"))
            {
                SaveFile sf = JsonConvert.DeserializeObject<SaveFile>(File.ReadAllText("config"));
                mainList = sf.mainList;
                requirementList = sf.requirementList;
            }
            else
            {
                mainList = new List<Course>(new Course[] { new Course() { year = 1, name = "CPSC 110", credits = 4, flags = new string[] { "science" } } });
                requirementList = new List<Requirement>();
            }

            InitializeComponent();
            DataContext = this;
        }


        private void requirementGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Requirement a = (Requirement)e.Row.Item;
            a.execute(mainList);


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveFile sf = new SaveFile();
            sf.mainList = mainList;
            sf.requirementList = requirementList;
            File.WriteAllText("config", JsonConvert.SerializeObject(sf));
        }
        void refresh()
        {
            foreach (Requirement r in requirementList)
                r.execute(mainList);
        }
        private void mainGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            refresh();
        }

        private void requirementGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            foreach (var x in e.AddedCells)
            {
                try
                {
                    ((Course)x.Item).selected = true;
                }
                catch { }

            }
            foreach (var x in e.RemovedCells)
            {
                try
                {
                    ((Course)x.Item).selected = false;
                }
                catch { }
            }
            refresh();
        }

        private void mainGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (mainGrid.SelectedIndex != -1 && mainGrid.SelectedIndex < mainList.Count - 1)
                    mainList.Remove((Course)mainGrid.SelectedItem);
            }
            if (e.Key == Key.PageDown)
            {
          
                e.Handled = true;
                
                for (int i = mainList.Count - 1; i >= 0; i--)
                {

                    bool sel = mainList[i].selected;
                    if (sel && i == mainList.Count - 1)
                        break;
                    if (sel)
                    {
                        var tmp = mainList[i + 1];
                        mainList[i + 1] = mainList[i];
                        mainList[i] = tmp;

                    }

            


                }
      
                mainGrid.Items.Refresh();
         
            }
            if (e.Key == Key.PageUp)
            {
                e.Handled = true;
                for (int i = 0; i < mainList.Count; i++)
                {
                    bool sel = mainList[i].selected;
                    if (sel && i == 0)
                        break;
                    if (sel)
                    {
                        var tmp = mainList[i - 1];
                        mainList[i - 1] = mainList[i];
                        mainList[i] = tmp;

                    }
                }
                mainGrid.Items.Refresh();
            }
        }

        private void requirementGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (requirementGrid.SelectedIndex != -1 && requirementGrid.SelectedIndex < requirementList.Count - 1)
                    requirementList.Remove((Requirement)requirementGrid.SelectedItem);
            }


        }




    }
    class SaveFile
    {
        public List<Requirement> requirementList;
        public List<Course> mainList;
    }

}
