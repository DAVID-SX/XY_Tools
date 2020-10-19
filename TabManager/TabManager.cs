using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabManager
{
    [Transaction(TransactionMode.Manual)]
    class TabManager : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // 初始化窗体
            MainWindow wpf = new MainWindow();

            // 获取功能页信息并将其存放至列表中
            IList<RibbonTab> tabList = ComponentManager.Ribbon.Tabs;
            foreach (RibbonTab tab in tabList)
            {
                if (!tab.IsContextualTab && !tab.IsMergedContextualTab && tab.KeyTip == null)
                {
                    //tab.IsVisible = !tab.IsVisible;
                    //Autodesk.Revit.UI.TaskDialog.Show("result", tab.Name);
                    //wpf.tabNameListBox.ItemsSource = ribbonControl.Tabs.ToList();
                    //tabNameList.Add(tab.Name.ToString());
                    Autodesk.Revit.UI.TaskDialog.Show("Result", tab.Name);
                }
            }

            // 为wpf窗体的ListBox指定源文件
            //wpf.tabNameListBox.ItemsSource = tabNameList;
            //wpf.ShowDialog();
            return Result.Succeeded;
        }
    }
}
