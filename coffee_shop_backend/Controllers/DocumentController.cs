using CoffeeShop.Model.Enum;
using CoffeeShop.Model.ViewModels;
using CoffeeShop.Repository.Interface;
using CoffeeShop.Service.Interface;
using CoffeeShop.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;


namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    [Authorize(Policy = "AdminPolicy")]
    public class DocumentController : BaseController
    {
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;
        private HttpContext? _context;
        private string sessionName = "DocumentViewModel";

        public DocumentController(IDocumentService documentService, IUnitOfWork unitOfWork, IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
            _documentService = documentService;
            _unitOfWork = unitOfWork;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            return View(_documentService.GetAdminList());
        }

        [Route("Form")]
        public IActionResult Form(string id, string tab)
        {
            DocumentViewModel model = _documentService.GetFormData(id);
            TempData[sessionName] = JsonConvert.SerializeObject(model);
            ViewBag.Tab = tab;
            return View(model);
        }

        [HttpPost]
        [Route("Form")]
        [ValidateAntiForgeryToken]
        public IActionResult Form(DocumentViewModel model)
        {
            if (string.IsNullOrEmpty(model.InfoPage.Caption))
            {
                ModelState.AddModelError("Caption", "標題不可為空白");
            }

            DocumentDTO document = new DocumentDTO()
            {
                Id = model.InfoPage.Id!,
                Sort = model.InfoPage.Sort,
                Caption = model.InfoPage.Caption,
                StartDate = model.InfoPage.StartDate,
                EndDate = model.InfoPage.EndDate,
                IsEnabled = model.InfoPage.IsEnabled,
                Hits = 0,
                HeadText = model.InfoPage.HeadText,
                FooterText = model.InfoPage.FooterText,
            };
            //新增
            if (string.IsNullOrEmpty(document.Id))
            {
                document.Creator = "Admin";
                document.CreateDate = DateTime.Now;
                _documentService.InsertDocument(document);
            }
            //修改
            else
            {
                document.Updator = "Admin";
                document.UpdateDate = DateTime.Now;
                _documentService.UpdateDocument(document);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("RecordList")]
        public IActionResult RecordList()
        {
            List<DocumentRecordListViewModel> model = _documentService.GetRecodList();
            return View(model);
        }

        [HttpGet]
        [Route("Record")]
        public IActionResult Record(string id, string documentId)
        {
            List<DocumentRecordViewModel> model = _documentService.GetRecordData(id, documentId);
            return View(model);
        }

        [HttpGet]
        [Route("ExportData")]
        public IActionResult ExportData(string id)
        {
            DataTable dt = _unitOfWork.DocumentRepository.GetExportData(id);
            _unitOfWork.Dispose();

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("報名資料");

            //設定樣式
            ICellStyle style = workbook.CreateCellStyle();
            IFont headerfont = workbook.CreateFont();
            style.Alignment = HorizontalAlignment.Center; //水平置中
            style.VerticalAlignment = VerticalAlignment.Center; //垂直置中
            headerfont.FontName = "微軟正黑體";
            style.SetFont(headerfont);

            //標題
            var rowIndex = 0;
            sheet.CreateRow(rowIndex);
            foreach (DataColumn column in dt.Columns)
            {
                sheet.GetRow(rowIndex).CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                sheet.GetRow(rowIndex).GetCell(column.Ordinal).CellStyle = style;
            }
            rowIndex++;

            // 設定欄位寬度
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                int length = dt.Columns[i].ColumnName.Length;
                int width = (int)((length + 15 + 0.72) * 256);
                sheet.SetColumnWidth(i, width);
            }

            //內容
            foreach (DataRow row in dt.Rows)
            {
                sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dt.Columns)
                {
                    sheet.GetRow(rowIndex).CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                rowIndex++;
            }

            var excelDatas = new MemoryStream();
            workbook.Write(excelDatas);
            var filename = $"register_{Guid.NewGuid().ToString().Substring(0, 5)}.xls";

            return File(excelDatas.ToArray(), "application/vnd.ms-excel", filename);
        }

        #region 新增/編輯題目

        [HttpPost]
        [Route("InsertField")]
        [ValidateAntiForgeryToken]
        public IActionResult InsertField(DocumentQuestionPage QuestionPage)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                DocumentViewModel model = JsonConvert.DeserializeObject<DocumentViewModel>(jsonText)!;
                if (!ModelState.IsValid)
                {
                    ViewBag.Tab = "1";
                    return View("Form", model);
                }
                _documentService.InsertField(model, QuestionPage);
                ModelState.Clear();
                return RedirectToAction("Form", new { id = model.InfoPage.Id, tab = "1" });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("EditField")]
        public IActionResult EditField(string fieldId)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                DocumentViewModel model = JsonConvert.DeserializeObject<DocumentViewModel>(jsonText)!;
                _documentService.EditField(model, fieldId);
                TempData[sessionName] = JsonConvert.SerializeObject(model);
                ViewBag.Tab = "1";
                return View("Form", model);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("DeleteField")]
        public IActionResult DeleteField(string fieldId)
        {
            int documentId = _documentService.DeleteField(fieldId);
            return RedirectToAction("Form", new { id = documentId, tab = "1" });
        }


        #endregion

        #region 選擇題選項

        [HttpGet]
        [Route("InsertOption")]
        public JsonResult InsertOption(AnswerOption Question)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                DocumentViewModel model = JsonConvert.DeserializeObject<DocumentViewModel>(jsonText)!;
                if (Question.Sort == null) //新增
                {
                    Question.Sort = model.QuestionPage.OptionList.Any() ? model.QuestionPage.OptionList.OrderByDescending(x => x.Sort).FirstOrDefault()!.Sort + 1 : 0;
                    Question.MemoText = EnumHelper.GetDescription((MemoTypeEnum)Question.MemoType);
                    model.QuestionPage.OptionList.Add(Question);
                }
                else //編輯
                {
                    AnswerOption option = model.QuestionPage.OptionList.FirstOrDefault(x => x.Sort == Question.Sort)!;
                    option.Text = Question.Text;
                    option.MemoType = Question.MemoType;
                    option.MemoText = EnumHelper.GetDescription((MemoTypeEnum)option.MemoType);
                    model.QuestionPage.NewOption = new AnswerOption();
                }
                ModelState.Clear();
                TempData[sessionName] = JsonConvert.SerializeObject(model);
                return this.Json(model.QuestionPage.OptionList);
            }
            return Json("Insert Error");
        }

        [HttpGet]
        [Route("EditOption")]
        public JsonResult EditOption(int index)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                DocumentViewModel model = JsonConvert.DeserializeObject<DocumentViewModel>(jsonText)!;
                model.QuestionPage.NewOption = model.QuestionPage.OptionList.FirstOrDefault(x => x.Sort == index)!;
                TempData[sessionName] = JsonConvert.SerializeObject(model);
                return this.Json(model.QuestionPage.NewOption);
            }
            return Json("Read Error");
        }

        [HttpGet]
        [Route("DeleteOption")]
        public JsonResult DeleteOption(int index)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                DocumentViewModel model = JsonConvert.DeserializeObject<DocumentViewModel>(jsonText)!;
                AnswerOption option = model.QuestionPage.OptionList.FirstOrDefault(x => x.Sort == index)!;
                model.QuestionPage.OptionList.Remove(option);
                model.QuestionPage.OptionList = model.QuestionPage.OptionList.OrderBy(x => x.Sort).ToList();
                int q = 0;
                foreach (var item in model.QuestionPage.OptionList)
                {
                    item.Sort = q;
                    q++;
                }
                model.QuestionPage.NewOption = new AnswerOption();
                ModelState.Clear();
                TempData[sessionName] = JsonConvert.SerializeObject(model);
                return this.Json(model.QuestionPage.OptionList);
            }
            return Json("Delete Error");
        }

        #endregion

        #region 調整排序

        [HttpGet]
        [Route("SetFieldSort")]
        public IActionResult SetFieldSort(string fieldId, bool direction)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                DocumentViewModel model = JsonConvert.DeserializeObject<DocumentViewModel>(jsonText)!;
                _documentService.SetFieldSort(model, fieldId, direction);
                return RedirectToAction("Form", new { id = model.InfoPage.Id, tab = "1" });
            }
            return RedirectToAction("Index");
        }

        #endregion





    }
}
