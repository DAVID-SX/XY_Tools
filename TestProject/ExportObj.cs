using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XY_Tools_Project
{
    [Transaction(TransactionMode.Manual)]
    class ExportObj : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            View3D view3D = doc.ActiveView as View3D;
            ExportObjContext exportObjContext = new ExportObjContext(doc);

            using (CustomExporter exporter = new CustomExporter(doc, exportObjContext))
            {
                exporter.ShouldStopOnError = false;
                exporter.Export(view3D as View);
            }

            return Result.Succeeded;
        }
    }
}
