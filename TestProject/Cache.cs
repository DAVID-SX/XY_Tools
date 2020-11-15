using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XY_Tools_Project
{
    public class Cache
    {
        #region
        /// <summary>
        /// TabManager需要使用到的相关参数
        /// </summary>
        //用于存放插件名称的列表
        private List<string> tabNameList = new List<string>();
        public List<string> TabNameList
        {
            get { return this.tabNameList; }
            set { tabNameList = value; }
        }
        //用于存放插件是否需要关闭的列表
        private List<bool> tabValueList = new List<bool>();
        public List<bool> TabValueList
        {
            get { return this.tabValueList; }
            set { tabValueList = value; }
        }
        #endregion

        private List<string> levelNameList = new List<string>();

        public List<string> LevelNameList
        {
            get { return levelNameList; }
            set { levelNameList = value; }
        }

    }
}
