using CoffeeShop.Model.Entities;
using CoffeeShop.Model.Enum;
using CoffeeShop.Model.ViewModels;
using CoffeeShop.Repository.Interface;
using CoffeeShop.Service.Implement;
using CoffeeShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class DocumentFrontController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        private string sessionName = "DocumentFrontViewModel";
        private IDocumentService _documentService;
        public DocumentFrontController(IUnitOfWork unitOfWork, IHttpContextAccessor accessor, IDocumentService documentService) : base(accessor)
        {
            _unitOfWork = unitOfWork;
            _documentService = documentService;
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
            DocumentFormViewModel model = _documentService.GetFrontFormData(id, recordId);
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
                string recordId = collection["RecordId"];
                _documentService.FieldDataCheck(collection, model.Fields, model.ValidResults, recordId);
                if (model.ValidResults.Count == 0)
                {
                    bool isEdit = !string.IsNullOrEmpty(recordId);
                    //新增
                    if (!isEdit)
                    {
                        var record = _unitOfWork.DocumentRepository.GetDocumentRecord().OrderByDescending(x => x.DocumentRecordId).FirstOrDefault();
                        recordId = record == null ? "1" : (record.DocumentRecordId + 1).ToString();
                    }
                    _documentService.Create(model, recordId, collection.Files, isEdit);
                    _unitOfWork.Complete();
                    return RedirectToAction("Index");
                }
                _unitOfWork.Complete();
                return View("Form", model);
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
            List<DocumentRecordListViewModel> model = _documentService.GetRecodList();
            return View(model);
        }
    }
}
