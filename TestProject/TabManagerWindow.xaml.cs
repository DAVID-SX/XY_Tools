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

namespace XY_Tools_Project
{
    public partial class TabManagerWindow : Window
    {
        private ExternalCommandData _commandData;
        public Cache _cache;
        XY_Tools uiDemo = new XY_Tools();
        //通过构造函数将commandData和缓存类传入
        public TabManagerWindow(ExternalCommandData commandData, Cache cache)
        {
            InitializeComponent();
            this._commandData = commandData;
            this._cache = cache;
        }
        //窗体加载时根据缓存类中的内容为StackPanel添加复选框
        private CheckBox checkbox;
        private void TabManagerWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _cache.TabNameList.Count; i++)
            {
                checkbox = new CheckBox();
                checkbox.Content = _cache.TabNameList[i];
                Thickness thickness = new Thickness(3);
                checkbox.Margin = thickness;
                this.tabNameStackPanel.Children.Add(checkbox);
            }
        }
        //定义属性来判断窗体是如何关闭的
        public bool IsClickCanceled { set; get; } = false;
        //单击确定时返回插件面板是否关闭的列表
        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.tabNameStackPanel.Children.Count; i++)
            {
                CheckBox checkBox = (CheckBox)this.tabNameStackPanel.Children[i];
                if (checkBox.Content.ToString() == uiDemo.TabName)
                {
                    this._cache.TabValueList.Add(true);
                }
                else if (checkBox.IsChecked != true)
                {
                    this._cache.TabValueList.Add(false);
                }
                else
                {
                    this._cache.TabValueList.Add(true);
                }
            }
            IsClickCanceled = true;
            this.Close();
        }


        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void checkBox_SelectAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in tabNameStackPanel.Children)
            {
                CheckBox checkBox = (CheckBox)item;
                checkBox.IsChecked = true;
            }
        }

        private void checkBox_SelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in tabNameStackPanel.Children)
            {
                CheckBox checkBox = (CheckBox)item;
                checkBox.IsChecked = false;
            }
        }
    }
}
