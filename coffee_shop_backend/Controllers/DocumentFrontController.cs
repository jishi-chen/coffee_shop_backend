using coffee_shop_backend.Enums;
using coffee_shop_backend.Interface;
using coffee_shop_backend.Models;
using coffee_shop_backend.Services;
using coffee_shop_backend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class DocumentFrontController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        private HttpContext? _context;
        private string sessionName = "DocumentFrontViewModel";

        public DocumentFrontController(IUnitOfWork unitOfWork, IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
            _unitOfWork = unitOfWork;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            var model = _unitOfWork.DocumentRepository.GetAdminList(true);
            _unitOfWork.Dispose();
            return View(model);
        }

        [Route("Form")]
        public IActionResult Form(string id, string recordId)
        {
            Document document = _unitOfWork.DocumentRepository.GetDocument(id);
            IEnumerable<DocumentField> fields = _unitOfWork.DocumentRepository.GetFieldList(id).ToList();
            IEnumerable<DocumentRecord> record = _unitOfWork.DocumentRepository.GetDocumentRecord(recordId);
            DocumentFormViewModel model = new DocumentFormViewModel()
            {
                Id = document.Id!,
                Caption = document.Caption,
                HeadText = document.HeadText,
                FooterText = document.FooterText,
            };
            foreach (DocumentField field in fields)
            {
                DocumentFieldViewModel dfvm = new DocumentFieldViewModel()
                {
                    Id = field.Id,
                    ParentId = field.ParentId,
                    FieldName = field.FieldName,
                    Note = field.Note,
                    FieldType = (AnswerTypeEnum)field.FieldType,
                    WordLimit = field.WordLimit,
                    RowLimit = field.RowLimit,
                    FileSizeLimit = field.FileSizeLimit,
                    FileExtension = field.FileExtension,
                    Sort = field.Sort,
                    IsRequired = field.IsRequired,
                    IsIncludedExport = field.IsIncludedExport,
                    IsEditable = field.IsEditable,
                };
                if (field.FieldType == (int)AnswerTypeEnum.SingleChoice || field.FieldType == (int)AnswerTypeEnum.MultipleChoice || field.FieldType == (int)AnswerTypeEnum.DropDownList)
                {
                    IEnumerable<DocumentFieldOption> options = _unitOfWork.DocumentRepository.GetFieldOption(field.Id).ToList();
                    foreach (DocumentFieldOption option in options)
                    {
                        DocumentFieldOptionViewModel dfovm = new DocumentFieldOptionViewModel()
                        {
                            Id = option.Id,
                            OptionName = option.OptionName,
                            Sort = option.Sort,
                            MemoType = (MemoTypeEnum)option.MemoType,
                        };
                        dfvm.Options.Add(dfovm);
                    }
                }
                //修改
                if (record.Count() > 0)
                {
                    var doc = record.FirstOrDefault(x => x.DocumentFieldId == field.Id);
                    if (doc != null)
                    {
                        dfvm.Value = doc.FilledText;
                        dfvm.MemoValue = doc.MemoText;
                        dfvm.Remark = doc.Remark;
                    }
                }
                model.Fields.Add(dfvm);
            }
            _unitOfWork.Dispose();

            model.Fields = model.Fields.OrderBy(x => x.Sort).ThenBy(x => x.ParentId).ToList();
            string modelString = JsonConvert.SerializeObject(model);
            TempData[sessionName] = modelString;
            return View(model);
        }

        [HttpPost]
        [Route("Form")]
        [ValidateAntiForgeryToken]
        public IActionResult Form(IFormCollection collection)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                DocumentFormViewModel model = JsonConvert.DeserializeObject<DocumentFormViewModel>(jsonText)!;
                new DocumentAPI(_unitOfWork).FieldDataCheck(collection, model.Fields, model.ValidResults, string.Empty);
                if (model.ValidResults.Count == 0)
                {
                    //取得暫時的recordId
                    string recordId = "0";
                    var record = _unitOfWork.DocumentRepository.GetDocumentRecord("").OrderByDescending(x => x.RegId).FirstOrDefault();
                    if (record != null)
                        recordId = (int.Parse(record.RegId) + 1).ToString();
                    //
                    new DocumentAPI(_unitOfWork).Create(model, recordId, collection.Files);
                }
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("RecordList")]
        public IActionResult RecordList()
        {
            List<DocumentRecordListViewModel> model = new List<DocumentRecordListViewModel>();
            var record = _unitOfWork.DocumentRepository.GetDocumentRecord("");
            if (record.Count() > 0)
            {
                model = record.Select(x => new DocumentRecordListViewModel()
                {
                    RegId = x.RegId,
                    DocumentId = x.DocumentId,
                    DocumentName = _unitOfWork.DocumentRepository.GetDocument(x.DocumentId).Caption, 
                }).ToList();
            }
            _unitOfWork.Dispose();
            return View(model);
        }

        [HttpGet]
        [Route("Record")]
        public IActionResult Record(string id, string documentId)
        {
            List<DocumentRecordViewModel> model = new List<DocumentRecordViewModel>();
            model = new DocumentAPI(_unitOfWork).GetRecordData(id, documentId);
            _unitOfWork.Dispose();
            return View(model);
        }
    }
}
