using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
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

namespace TestProject
{
    public partial class TabManagerWindow : Window
    {
        private ExternalCommandData _commandData;
        public Cache _cache;
        public TabManagerWindow(ExternalCommandData commandData, Cache cache)
        {
            InitializeComponent();
            this._commandData = commandData;
            this._cache = cache;
        }


        private CheckBox checkbox;
        private void TabManagerWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _cache.TabNameList.Count; i++)
            {
                checkbox = new CheckBox();
                checkbox.Content = _cache.TabNameList[i];
                this.tabNameStackPanel.Children.Add(checkbox);
            }
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.tabNameStackPanel.Children.Count; i++)
            {
                CheckBox checkBox = (CheckBox)this.tabNameStackPanel.Children[i];
                if (checkBox.IsChecked != true)
                {
                    this._cache.TabValueList[i] = false;
                }
            }
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
