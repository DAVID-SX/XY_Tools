using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XY_Tools_Project
{
    [Transaction(TransactionMode.Manual)]
    public class NameChecker : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            // 墙体命名检查
            string wallRegexPattern = @"^[A-Z]{0,3}_[0-9a-zA-Z\u4e00-\u9fa5]*(_[0-9a-zA-Z\u4e00-\u9fa5×]*){1,2}$";
            List<Element> wallToRenameList = CheckName(doc, wallRegexPattern, BuiltInCategory.OST_Walls, typeof(WallType));
            TaskDialog.Show("result", wallToRenameList.Count.ToString());

            // 结构柱命名检查
            //string structuralColumnRegexPattern = @"^[A-Z]{0,3}_[0-9a-zA-Z\u4e00-\u9fa5]*(_[0-9a-zA-Z\u4e00-\u9fa5×]*){1,2}$";
            //List<Element> stucturalColumnToRenameList = NameChecker(doc, structuralColumnRegexPattern, BuiltInCategory.OST_StructuralColumns, typeof(FamilySymbol));

            // 结构框架命名检查
            //string structuralFramingRegexPattern = @"^[A-Z]{0,3}_[0-9a-zA-Z\u4e00-\u9fa5]*(_[0-9a-zA-Z\u4e00-\u9fa5×]*){1,2}$";
            //List<Element> structuralFramingToRenameList = NameChecker(doc, structuralFramingRegexPattern, BuiltInCategory.OST_StructuralFraming, typeof(FamilySymbol));

            // 楼板命名检查
            //string floorRegexPattern = @"^[A-Z]{0,3}_[0-9a-zA-Z\u4e00-\u9fa5]*(_[0-9a-zA-Z\u4e00-\u9fa5×]*){1,2}$";
            //List<Element> floorToRenameList = NameChecker(doc, floorRegexPattern, BuiltInCategory.OST_Floors, typeof(FloorType));

            // 天花板命名检查
            //string ceilingRegexPattern = @"^[A-Z]{0,3}_[0-9a-zA-Z\u4e00-\u9fa5]*(_[0-9a-zA-Z\u4e00-\u9fa5×]*){1,2}$";
            //List<Element> ceilingToRenameList = NameChecker(doc, ceilingRegexPattern, BuiltInCategory.OST_Ceilings, typeof(CeilingType));

            // 屋顶命名检查
            //string ceilingRegexPattern = @"^[A-Z]{0,3}_[0-9a-zA-Z\u4e00-\u9fa5]*(_[0-9a-zA-Z\u4e00-\u9fa5×]*){1,2}$";
            //List<Element> ceilingToRenameList = NameChecker(doc, ceilingRegexPattern, BuiltInCategory.OST_Ceilings, typeof(CeilingType));

            // 栏杆扶手命名检查
            //string ceilingRegexPattern = @"^[A-Z]{0,3}_[0-9a-zA-Z\u4e00-\u9fa5]*(_[0-9a-zA-Z\u4e00-\u9fa5×]*){1,2}$";
            //List<Element> ceilingToRenameList = NameChecker(doc, ceilingRegexPattern, BuiltInCategory.OST_Ceilings, typeof(CeilingType));

            // 楼板命名检查
            //string ceilingRegexPattern = @"^[A-Z]{0,3}_[0-9a-zA-Z\u4e00-\u9fa5]*(_[0-9a-zA-Z\u4e00-\u9fa5×]*){1,2}$";
            //List<Element> ceilingToRenameList = NameChecker(doc, ceilingRegexPattern, BuiltInCategory.OST_Ceilings, typeof(CeilingType));

            // 楼梯命名检查
            //string ceilingRegexPattern = @"^[A-Z]{0,3}_[0-9a-zA-Z\u4e00-\u9fa5]*(_[0-9a-zA-Z\u4e00-\u9fa5×]*){1,2}$";
            //List<Element> ceilingToRenameList = NameChecker(doc, ceilingRegexPattern, BuiltInCategory.OST_Ceilings, typeof(CeilingType));

            return Result.Succeeded;
        }
        public List<Element> CheckName(Document doc, string regexType, BuiltInCategory builtInCategory, Type type)
        {
            string message = "请检查以下构件是否满足命名规范：\n\n";
            List<Element> outList = new List<Element>();
            FilteredElementCollector elemCollector = new FilteredElementCollector(doc).OfCategory(builtInCategory).OfClass(type);
            foreach (Element elem in elemCollector)
            {
                bool match = Regex.IsMatch(elem.Name, regexType);
                if (!match)
                {
                    message += elem.Name + "\n";
                    outList.Add(elem);
                }
            }

            if (outList != null)
            {
                TaskDialog.Show("请注意以下墙体的命名是否满足命名规范", message);
            }
            return outList;
        }
    }
}
