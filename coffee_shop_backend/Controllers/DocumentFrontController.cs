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
        public IActionResult Form(DocumentFormViewModel model, IFormCollection collection)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                model = JsonConvert.DeserializeObject<DocumentFormViewModel>(jsonText)!;
                new DocumentAPI(_unitOfWork).FieldDataCheck(collection, model.Fields, model.ValidResults);
                if (model.ValidResults.Count == 0)
                {
                    //new DocumentAPI(_unitOfWork).Create(model);
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
