using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabManagerProject
{
    public class Cache
    {
        private List<string> tabNameList = new List<string>();

        public List<string> TabNameList
        {
            get { return this.tabNameList; }
            set { this.tabNameList = value; }
        }


        //private List<bool> tabValueList;

        //public List<bool> TabValueList
        //{
        //    get { return tabValueList; }
        //    set { tabValueList = value; }
        //}



    }
}
