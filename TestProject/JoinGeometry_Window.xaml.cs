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
using System.Windows.Shapes;

namespace XY_Tools_Project
{

    public partial class JoinGeometry_Window : Window
    {
        Cache _cache;
        #region
        /// <summary>
        /// 定义需要使用到的相关属性和字段
        /// </summary>
        // 结构基础扣减字段
        private bool jGJC_JGQ;
        public bool JGJC_JGQ
        {
            get { return jGJC_JGQ; }
            set { jGJC_JGQ = value; }
        }
        private bool jGJC_JGZ;
        public bool JGJC_JGZ
        {
            get { return jGJC_JGZ; }
            set { jGJC_JGZ = value; }
        }
        private bool jGJC_JGL;
        public bool JGJC_JGL
        {
            get { return jGJC_JGL; }
            set { jGJC_JGL = value; }
        }
        private bool jGJC_JGB;
        public bool JGJC_JGB
        {
            get { return jGJC_JGB; }
            set { jGJC_JGB = value; }
        }
        private bool jGJC_JGT;
        public bool JGJC_JGT
        {
            get { return jGJC_JGT; }
            set { jGJC_JGT = value; }
        }
        private bool jGJC_CGMX;
        public bool JGJC_CGMX
        {
            get { return jGJC_CGMX; }
            set { jGJC_CGMX = value; }
        }
        // 结构墙扣减属性
        private bool jGQ_JGJC;
        public bool JGQ_JGJC
        {
            get { return jGQ_JGJC; }
            set { jGQ_JGJC = value; }
        }
        private bool jGQ_JGZ;
        public bool JGQ_JGZ
        {
            get { return jGQ_JGZ; }
            set { jGQ_JGZ = value; }
        }
        private bool jGQ_JGL;
        public bool JGQ_JGL
        {
            get { return jGQ_JGL; }
            set { jGQ_JGL = value; }
        }
        private bool jGQ_JGB;
        public bool JGQ_JGB
        {
            get { return jGQ_JGB; }
            set { jGQ_JGB = value; }
        }
        private bool jGQ_JGT;
        public bool JGQ_JGT
        {
            get { return jGQ_JGT; }
            set { jGQ_JGT = value; }
        }
        private bool jGQ_CGMX;
        public bool JGQ_CGMX
        {
            get { return jGQ_CGMX; }
            set { jGQ_CGMX = value; }
        }
        // 结构柱扣减属性
        private bool jGZ_JGJC;
        public bool JGZ_JGJC
        {
            get { return jGZ_JGJC; }
            set { jGZ_JGJC = value; }
        }
        private bool jGZ_JGQ;
        public bool JGZ_JGQ
        {
            get { return jGZ_JGQ; }
            set { jGZ_JGQ = value; }
        }
        private bool jGZ_JGL;
        public bool JGZ_JGL
        {
            get { return jGZ_JGL; }
            set { jGZ_JGL = value; }
        }
        private bool jGZ_JGB;
        public bool JGZ_JGB
        {
            get { return jGZ_JGB; }
            set { jGZ_JGB = value; }
        }
        private bool jGZ_JGT;
        public bool JGZ_JGT
        {
            get { return jGZ_JGT; }
            set { jGZ_JGT = value; }
        }
        private bool jGZ_CGMX;
        public bool JGZ_CGMX
        {
            get { return jGZ_CGMX; }
            set { jGZ_CGMX = value; }
        }
        //结构梁相关字段
        private bool jGL_JGJC;
        public bool JGL_JGJC
        {
            get { return jGL_JGJC; }
            set { jGL_JGJC = value; }
        }
        private bool jGL_JGQ;
        public bool JGL_JGQ
        {
            get { return jGL_JGQ; }
            set { jGL_JGQ = value; }
        }
        private bool jGL_JGZ;
        public bool JGL_JGZ
        {
            get { return jGL_JGZ; }
            set { jGL_JGZ = value; }
        }
        private bool jGL_JGB;
        public bool JGL_JGB
        {
            get { return jGL_JGB; }
            set { jGL_JGB = value; }
        }
        private bool jGL_JGT;
        public bool JGL_JGT
        {
            get { return jGL_JGT; }
            set { jGL_JGT = value; }
        }
        private bool jGL_CGMX;
        public bool JGL_CGMX
        {
            get { return jGL_CGMX; }
            set { jGL_CGMX = value; }
        }
        //结构板相关字段
        private bool jGB_JGJC;
        public bool JGB_JGJC
        {
            get { return jGB_JGJC; }
            set { jGB_JGJC = value; }
        }
        private bool jGB_JGQ;
        public bool JGB_JGQ
        {
            get { return jGB_JGQ; }
            set { jGB_JGQ = value; }
        }
        private bool jGB_JGZ;
        public bool JGB_JGZ
        {
            get { return jGB_JGZ; }
            set { jGB_JGZ = value; }
        }
        private bool jGB_JGL;
        public bool JGB_JGL
        {
            get { return jGB_JGL; }
            set { jGB_JGL = value; }
        }
        private bool jGB_JGT;
        public bool JGB_JGT
        {
            get { return jGB_JGT; }
            set { jGB_JGT = value; }
        }
        private bool jGB_CGMX;
        public bool JGB_CGMX
        {
            get { return jGB_CGMX; }
            set { jGB_CGMX = value; }
        }
        //结构梯相关字段
        private bool jGT_JGJC;
        public bool JGT_JGJC
        {
            get { return jGT_JGJC; }
            set { jGT_JGJC = value; }
        }
        private bool jGT_JGQ;
        public bool JGT_JGQ
        {
            get { return jGT_JGQ; }
            set { jGT_JGQ = value; }
        }
        private bool jGT_JGZ;
        public bool JGT_JGZ
        {
            get { return jGT_JGZ; }
            set { jGT_JGZ = value; }
        }
        private bool jGT_JGL;
        public bool JGT_JGL
        {
            get { return jGT_JGL; }
            set { jGT_JGL = value; }
        }
        private bool jGT_JGB;
        public bool JGT_JGB
        {
            get { return jGT_JGB; }
            set { jGT_JGB = value; }
        }
        private bool jGT_CGMX;
        public bool JGT_CGMX
        {
            get { return jGT_CGMX; }
            set { jGT_CGMX = value; }
        }
        //常规模型相关字段
        private bool cGMX_JGJC;
        public bool CGMX_JGJC
        {
            get { return cGMX_JGJC; }
            set { cGMX_JGJC = value; }
        }
        private bool cGMX_JGQ;
        public bool CGMX_JGQ
        {
            get { return cGMX_JGQ; }
            set { cGMX_JGQ = value; }
        }
        private bool cGMX_JGZ;
        public bool CGMX_JGZ
        {
            get { return cGMX_JGZ; }
            set { cGMX_JGZ = value; }
        }
        private bool cGMX_JGL;
        public bool CGMX_JGL
        {
            get { return cGMX_JGL; }
            set { cGMX_JGL = value; }
        }
        private bool cGMX_JGB;
        public bool CGMX_JGB
        {
            get { return cGMX_JGB; }
            set { cGMX_JGB = value; }
        }
        private bool cGMX_JGT;
        public bool CGMX_JGT
        {
            get { return cGMX_JGT; }
            set { cGMX_JGT = value; }
        }
        private bool cGMX_JZQ;
        public bool CGMX_JZQ
        {
            get { return cGMX_JZQ; }
            set { cGMX_JZQ = value; }
        }
        private bool cGMX_JZZ;
        public bool CGMX_JZZ
        {
            get { return cGMX_JZZ; }
            set { cGMX_JZZ = value; }
        }
        private bool cGMX_JZB;
        public bool CGMX_JZB
        {
            get { return cGMX_JZB; }
            set { cGMX_JZB = value; }
        }
        //建筑墙相关字段
        private bool jJQ_JZZ;
        public bool JZQ_JZZ
        {
            get { return jJQ_JZZ; }
            set { jJQ_JZZ = value; }
        }
        private bool jJQ_JZB;
        public bool JZQ_JZB
        {
            get { return jJQ_JZB; }
            set { jJQ_JZB = value; }
        }
        private bool jJQ_CGMX;
        public bool JZQ_CGMX
        {
            get { return jJQ_CGMX; }
            set { jJQ_CGMX = value; }
        }
        //建筑柱相关字段
        private bool jJZ_JZQ;
        public bool JZZ_JZQ
        {
            get { return jJZ_JZQ; }
            set { jJZ_JZQ = value; }
        }
        private bool jJZ_JZB;
        public bool JZZ_JZB
        {
            get { return jJZ_JZB; }
            set { jJZ_JZB = value; }
        }
        private bool jJZ_CGMX;
        public bool JZZ_CGMX
        {
            get { return jJZ_CGMX; }
            set { jJZ_CGMX = value; }
        }
        //建筑板相关字段
        private bool jJB_JZQ;
        public bool JZB_JZQ
        {
            get { return jJB_JZQ; }
            set { jJB_JZQ = value; }
        }
        private bool jJB_JZZ;
        public bool JZB_JZZ
        {
            get { return jJB_JZZ; }
            set { jJB_JZZ = value; }
        }
        private bool jJB_CGMX;
        public bool JZB_CGMX
        {
            get { return jJB_CGMX; }
            set { jJB_CGMX = value; }
        }
        #endregion
        #region
        /// <summary>
        /// 定义相关复选框的点击事件
        /// </summary>
        private void JGJC_JGQCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGJC_JGQ = true;
            JGQ_JGJC = false;
            JGQ_JGJCCheckBox.IsChecked = false;
        }

        private void JGJC_JGZCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGJC_JGZ = true;
            JGZ_JGJC = false;
            JGZ_JGJCCheckBox.IsChecked = false;
        }

        private void JGJC_JGLCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGJC_JGL = true;
            JGL_JGJC = false;
            JGL_JGJCCheckBox.IsChecked = false;
        }

        private void JGJC_JGBCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGJC_JGB = true;
            JGB_JGJC = false;
            JGB_JGJCCheckBox.IsChecked = false;
        }

        private void JGJC_JGTCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGJC_JGT = true;
            JGT_JGJC = false;
            JGT_JGJCCheckBox.IsChecked = false;
        }

        private void JGJC_CGMXCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGJC_CGMX = true;
            CGMX_JGJC = false;
            CGMX_JGJCCheckBox.IsChecked = false;
        }

        private void JGQ_JGJCCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGQ_JGJC = true;
            JGJC_JGQ = false;
            JGJC_JGQCheckBox.IsChecked = false;
        }

        private void JGQ_JGZCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGQ_JGZ = true;
            JGZ_JGQ = false;
            JGZ_JGQCheckBox.IsChecked = false;
        }

        private void JGQ_JGLCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGQ_JGL = true;
            JGL_JGQ = false;
            JGL_JGQCheckBox.IsChecked = false;
        }

        private void JGQ_JGBCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGQ_JGB = true;
            JGB_JGQ = false;
            JGB_JGQCheckBox.IsChecked = false;
        }

        private void JGQ_JGTCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGQ_JGT = true;
            JGT_JGQ = false;
            JGT_JGQCheckBox.IsChecked = false;
        }

        private void JGQ_CGMXCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGQ_CGMX = true;
            CGMX_JGQ = false;
            CGMX_JGQCheckBox.IsChecked = false;
        }

        private void JGZ_JGJCCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGZ_JGJC = true;
            JGJC_JGZ = false;
            JGJC_JGZCheckBox.IsChecked = false;
        }

        private void JGZ_JGQCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGZ_JGQ = true;
            JGQ_JGZ = false;
            JGQ_JGZCheckBox.IsChecked = false;
        }

        private void JGZ_JGLCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGZ_JGL = true;
            JGL_JGZ = false;
            JGL_JGZCheckBox.IsChecked = false;
        }

        private void JGZ_JGBCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGZ_JGB = true;
            JGB_JGZ = false;
            JGB_JGZCheckBox.IsChecked = false;
        }

        private void JGZ_JGTCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGZ_JGT = true;
            JGT_JGZ = false;
            JGT_JGZCheckBox.IsChecked = false;
        }

        private void JGZ_CGMXCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGZ_CGMX = true;
            CGMX_JGZ = false;
            CGMX_JGZCheckBox.IsChecked = false;
        }

        private void JGL_JGJCCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGL_JGJC = true;
            JGJC_JGL = false;
            JGJC_JGLCheckBox.IsChecked = false;
        }

        private void JGL_JGQCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGL_JGQ = true;
            JGQ_JGL = false;
            JGQ_JGLCheckBox.IsChecked = false;
        }

        private void JGL_JGZCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGL_JGZ = true;
            JGZ_JGL = false;
            JGZ_JGLCheckBox.IsChecked = false;
        }

        private void JGL_JGBCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGL_JGB = true;
            JGB_JGL = false;
            JGB_JGLCheckBox.IsChecked = false;
        }

        private void JGL_JGTCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGL_JGT = true;
            JGT_JGL = false;
            JGT_JGLCheckBox.IsChecked = false;
        }

        private void JGL_CGMXCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGL_CGMX = true;
            CGMX_JGL = false;
            CGMX_JGLCheckBox.IsChecked = false;
        }

        private void JGB_JGJCCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGB_JGJC = true;
            JGJC_JGB = false;
            JGJC_JGBCheckBox.IsChecked = false;
        }

        private void JGB_JGQCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGB_JGQ = true;
            JGQ_JGB = false;
            JGQ_JGBCheckBox.IsChecked = false;
        }

        private void JGB_JGZCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGB_JGZ = true;
            JGZ_JGB = false;
            JGZ_JGBCheckBox.IsChecked = false;
        }

        private void JGB_JGLCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGB_JGL = true;
            JGL_JGB = false;
            JGL_JGBCheckBox.IsChecked = false;
        }

        private void JGB_JGTCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGB_JGT = true;
            JGT_JGB = false;
            JGT_JGBCheckBox.IsChecked = false;
        }

        private void JGB_CGMXCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGB_CGMX = true;
            CGMX_JGB = false;
            CGMX_JGBCheckBox.IsChecked = false;
        }

        private void JGT_JGJCCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGT_JGJC = true;
            JGJC_JGT = false;
            JGJC_JGTCheckBox.IsChecked = false;
        }

        private void JGT_JGQCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGT_JGQ = true;
            JGQ_JGT = false;
            JGQ_JGTCheckBox.IsChecked = false;
        }

        private void JGT_JGZCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGT_JGZ = true;
            JGZ_JGT = false;
            JGZ_JGTCheckBox.IsChecked = false;
        }

        private void JGT_JGLCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGT_JGL = true;
            JGL_JGT = false;
            JGL_JGTCheckBox.IsChecked = false;
        }

        private void JGT_JGBCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGT_JGB = true;
            JGB_JGT = false;
            JGB_JGTCheckBox.IsChecked = false;
        }

        private void JGT_CGMXCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JGT_CGMX = true;
            CGMX_JGT = false;
            CGMX_JGTCheckBox.IsChecked = false;
        }

        private void CGMX_JGJCCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CGMX_JGJC = true;
            JGJC_CGMX = false;
            JGJC_CGMXCheckBox.IsChecked = false;
        }

        private void CGMX_JGQCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CGMX_JGQ = true;
            JGQ_CGMX = false;
            JGQ_CGMXCheckBox.IsChecked = false;
        }

        private void CGMX_JGZCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CGMX_JGZ = true;
            JGZ_CGMX = false;
            JGZ_CGMXCheckBox.IsChecked = false;
        }

        private void CGMX_JGLCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CGMX_JGL = true;
            JGL_CGMX = false;
            JGL_CGMXCheckBox.IsChecked = false;
        }

        private void CGMX_JGBCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CGMX_JGB = true;
            JGB_CGMX = false;
            JGB_CGMXCheckBox.IsChecked = false;
        }

        private void CGMX_JGTCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CGMX_JGT = true;
            JGT_CGMX = false;
            JGT_CGMXCheckBox.IsChecked = false;
        }

        private void CGMX_JZQCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CGMX_JZQ = true;
            JZQ_CGMX = false;
            JZQ_CGMXCheckBox.IsChecked = false;
        }

        private void CGMX_JZZCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CGMX_JZZ = true;
            JZZ_CGMX = false;
            JZZ_CGMXCheckBox.IsChecked = false;

        }

        private void CGMX_JZBCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CGMX_JZB = true;
            JZB_CGMX = false;
            JZB_CGMXCheckBox.IsChecked = false;
        }

        private void JZQ_JZZCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JZQ_JZZ = true;
            JZZ_JZQ = false;
            JZZ_JZQCheckBox.IsChecked = false;
        }

        private void JZQ_JZBCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JZQ_JZB = true;
            JZB_JZQ = false;
            JZB_JZQCheckBox.IsChecked = false;
        }

        private void JZQ_CGMXCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JZQ_CGMX = true;
            CGMX_JZQ = false;
            CGMX_JZQCheckBox.IsChecked = false;
        }

        private void JZZ_JZQCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JZZ_JZQ = true;
            JZQ_JZZ = false;
            JZQ_JZZCheckBox.IsChecked = false;
        }

        private void JZZ_JZBCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JZZ_JZB = true;
            JZB_JZZ = false;
            JZB_JZZCheckBox.IsChecked = false;
        }

        private void JZZ_CGMXCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JZZ_CGMX = true;
            CGMX_JZZ = false;
            CGMX_JZZCheckBox.IsChecked = false;
        }

        private void JZB_JZQCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JZB_JZQ = true;
            JZQ_JZB = false;
            JZQ_JZBCheckBox.IsChecked = false;
        }

        private void JZB_JZZCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JZB_JZZ = true;
            JZZ_JZB = false;
            JZZ_JZBCheckBox.IsChecked = false;
        }

        private void JZB_CGMXCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JZB_CGMX = true;
            CGMX_JZB = false;
            CGMX_JZBCheckBox.IsChecked = false;
        }
        #endregion
        public JoinGeometry_Window(Cache cache)
        {
            InitializeComponent();
            _cache = cache;
        }
        CheckBox checkbox;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _cache.LevelNameList.Count; i++)
            {
                checkbox = new CheckBox();
                checkbox.Content = _cache.LevelNameList[i].Replace("_","__");
                Thickness thickness = new Thickness(3);
                checkbox.Margin = thickness;
                this.levelListStackPanel.Children.Add(checkbox);
            }
            levelListStackPanel.IsEnabled = false;
            selectAllModeleRadioButton.IsChecked = true;
        }

        private void useModelRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            levelListStackPanel.IsEnabled = true;
        }

        private void selectAllModeleRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            levelListStackPanel.IsEnabled = false;
        }

        private void selectModelRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            levelListStackPanel.IsEnabled = false;
        }
    }
}
