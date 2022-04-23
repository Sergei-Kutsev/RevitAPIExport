using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingSheets
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            #region "Экспорт в DWG"
            /*
            using (var ts = new Transaction(doc, "Export to DWG"))
            {
                ts.Start();

                ViewPlan viewPlan = new FilteredElementCollector(doc)//что конкретно экспортируем
                    .OfClass(typeof(ViewPlan))
                    .Cast<ViewPlan>()
                    .FirstOrDefault(v => v.ViewType == ViewType.FloorPlan && //собираем план этажа
                    v.Name.Equals("Level 1")); //который должен иметь название Level 1

                var dwgOption = new DWGExportOptions(); //настройки при экспорте в двг будут по умолчанию, но их можно и настроить
                //dwgOption.FileVersion.Equals(ACADVersion.R2007); //версия 2010 года



                doc.Export(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "export.dwg", //указываем путь сохранения
                    new List<ElementId> { viewPlan.Id }, dwgOption); //в модели мб несколько планов этажей
                ts.Commit();
            }
            */
            #endregion "Экспорт в IFC"

            #region "Экспорт в IFC"
            /*
            using (var ts = new Transaction(doc, "Export to IFC"))
            {
                ts.Start();

                ViewPlan viewPlan = new FilteredElementCollector(doc)//что конкретно экспортируем
                    .OfClass(typeof(ViewPlan))
                    .Cast<ViewPlan>()
                    .FirstOrDefault(v => v.ViewType == ViewType.FloorPlan && //собираем план этажа
                    v.Name.Equals("Level 1")); //который должен иметь название Level 1

                var dwgOption = new IFCExportOptions(); //настройки при экспорте в двг будут по умолчанию, но их можно и настроить
                //dwgOption.FileVersion.Equals(ACADVersion.R2007); //версия 2010 года



                doc.Export(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "export.ifc", //указываем путь сохранения
                     dwgOption); //
                ts.Commit();
            }
            */
            #endregion

            #region "Экспорт в Navisworks"
            /*
            NavisworksExportOptions nwcOption = new NavisworksExportOptions();
            nwcOption.ExportScope = NavisworksExportScope.Model;
            nwcOption.ViewId = uidoc.ActiveView.Id;
            doc.Export(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "export.nwc", //указываем путь сохранения
                 nwcOption);
            */
            #endregion

            View view = doc.ActiveView;

            ViewPlan viewPlan = new FilteredElementCollector(doc)//что конкретно экспортируем
                    .OfClass(typeof(ViewPlan))
                    .Cast<ViewPlan>()
                    .FirstOrDefault(v => v.ViewType == ViewType.FloorPlan && //собираем план этажа
                    v.Name.Equals("Level 1")); //который должен иметь название Level 1

            string DeskTopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            using (var ts = new Transaction(doc, "Export current view"))
            {
                ts.Start();

                ImageExportOptions imageExportOptions = new ImageExportOptions
                {
                    ZoomType = ZoomFitType.FitToPage,
                    PixelSize = 1024,

                    FilePath = DeskTopPath + @"\" + view.Name,
                    FitDirection = FitDirectionType.Horizontal,
                    HLRandWFViewsFileType = ImageFileType.PNG,
                    ImageResolution = ImageResolution.DPI_600,
                    ExportRange = ExportRange.SetOfViews,
                    ShadowViewsFileType = ImageFileType.PNG,
                   
                };

                imageExportOptions.SetViewsAndSheets(new List<ElementId> { viewPlan.Id });
                doc.ExportImage(imageExportOptions);

                ts.Commit();
            }
            return Result.Succeeded;
        }
    }
}
