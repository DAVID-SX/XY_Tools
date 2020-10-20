using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TabManager
{
    [Transaction(TransactionMode.Manual)]
    class TabManager : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {


            // 获取功能页信息并将其存放至列表中
            IList<RibbonTab> tabList = ComponentManager.Ribbon.Tabs;
            List<string> tabNameList = new List<string>();
            List<bool> tabBoolList = new List<bool>();
            foreach (RibbonTab tab in tabList)
            {
                if (!tab.IsContextualTab && !tab.IsMergedContextualTab && tab.KeyTip == null)
                {
                    //tab.IsVisible = !tab.IsVisible;
                    //Autodesk.Revit.UI.TaskDialog.Show("result", tab.Name);
                    //wpf.tabNameListBox.ItemsSource = ribbonControl.Tabs.ToList();
                    //tabNameList.Add(tab.Name.ToString());
                    tabNameList.Add(tab.Name);
                    tabBoolList.Add(true);
                    //Autodesk.Revit.UI.TaskDialog.Show("Result", tab.Name);
                }
            }
            // 初始化窗体
            MainWindow wpf = new MainWindow();
            //为wpf窗体的Canvas指定源文件
            for (int i = 0; i < tabNameList.Count; i++)
            {
                CheckBox checkBox = new CheckBox
                {
                    Height = 14, Content = tabNameList[i],
                    checkBox.Checked +=
                };
                Canvas.SetTop(checkBox, 18 * i);
                Canvas.SetLeft(checkBox, 10);
                wpf.tabNameCanvas.Children.Add(checkBox);

            }
            wpf.ShowDialog();
            return Result.Succeeded;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
