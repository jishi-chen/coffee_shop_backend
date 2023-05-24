﻿using coffee_shop_backend.Enums;
using coffee_shop_backend.Interface;
using coffee_shop_backend.Models;
using coffee_shop_backend.Utility;
using coffee_shop_backend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class DocumentController : BaseController
    {
        private IUnitOfWork _unitOfWork;
        private HttpContext? _context;
        private string sessionName = "DocumentViewModel";

        public DocumentController(IUnitOfWork unitOfWork, IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
            _unitOfWork = unitOfWork;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            var model = _unitOfWork.DocumentRepository.GetAdminList();
            _unitOfWork.Dispose();
            return View(model);
        }

        [Route("Form")]
        public IActionResult Form(string id, string parentId, string tab)
        {
            DocumentViewModel model = new DocumentViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                model.InfoPage = _unitOfWork.DocumentRepository.GetInfoPage(id);
                model.QuestionPage.FieldList = _unitOfWork.DocumentRepository.GetQuestionFieldList(id).OrderBy(x => x.Sort).ToList();
                var parentList = model.QuestionPage.FieldList.Where(x => x.FieldType == (int)AnswerTypeEnum.Panel).Select(x => new SelectListItem
                {
                    Text = x.FieldName,
                    Value = x.Id,
                }).ToList();
                model.QuestionPage.ParentFieldList.Add(new SelectListItem { Text = "---請選擇---", Value = null });
                model.QuestionPage.ParentFieldList.AddRange(parentList);
                _unitOfWork.Dispose();
            }
            GetAnswerTypeSettings(model.QuestionPage.AnswerTypeList);
            GetMemoTypeSettings(model.QuestionPage.MemoTypeList);
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

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Error";
                return View(model);
            }
            else
            {
                Document document = new Document()
                {
                    Id = model.InfoPage.Id!,
                    CsId = model.InfoPage.CsId,
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
                    _unitOfWork.DocumentRepository.InsertDocument(document);
                    _unitOfWork.Complete();
                }
                //修改
                else
                {
                    document.Updator = "Admin";
                    document.UpdateDate = DateTime.Now;
                    _unitOfWork.DocumentRepository.UpdateDocument(document);
                    _unitOfWork.Complete();
                }
                return RedirectToAction("Form", new { id = model.InfoPage.Id, tab = "0" } );
            }
        }

        #region 新增/編輯題目

        [HttpPost]
        [Route("InsertField")]
        [ValidateAntiForgeryToken]
        public IActionResult InsertField(DocumentQuestionPage QuestionPage)
        {
            var Question = QuestionPage;
            if (TempData.Peek(sessionName) is string jsonText)
            {
                DocumentViewModel model = JsonConvert.DeserializeObject<DocumentViewModel>(jsonText)!;
                if (!ModelState.IsValid)
                {
                    ViewBag.Tab = "1";
                    return View("Form", model);
                }

                DocumentField field = new DocumentField()
                {
                    Id = model.QuestionPage.DocumentFieldId!,
                    DocumentId = model.InfoPage.Id!,
                    FieldName = Question.Caption,
                    FieldType = (byte)Question.AnswerType,
                    IsRequired = Question.IsRequired,
                    IsIncludedExport = Question.IsIncludedExport,
                    IsEditable = Question.IsEditable,
                    WordLimit = Question.WordLimit,
                    FileSizeLimit = Question.FileSizeLimit,
                    FileExtension = Question.FileExtension,
                    Note = Question.Note,
                };
                //新增
                if (string.IsNullOrEmpty(Question.DocumentFieldId))
                {
                    field.Creator = "Admin";
                    field.CreateDate = DateTime.Now;
                    var list = model.QuestionPage.FieldList.Where(x => x.ParentId == Question.ParentId);
                    if (list.Any())
                        field.Sort = list.OrderByDescending(x => x.Sort).FirstOrDefault()!.Sort +1;
                    else
                        field.Sort = 0;
                    field.Id = _unitOfWork.DocumentRepository.InsertDocumentField(field).ToString();
                }
                //編輯
                else
                {
                    field.Updator = "Admin";
                    field.UpdateDate = DateTime.Now;
                    _unitOfWork.DocumentRepository.UpdateDocumentField(field);

                }
                if (field.FieldType == (int)AnswerTypeEnum.SingleChoice || field.FieldType == (int)AnswerTypeEnum.MultipleChoice)
                {
                    _unitOfWork.DocumentRepository.DeleteFieldOptions(field.Id);
                    if (model.QuestionPage.OptionList.Any())
                    {
                        foreach (var item in model.QuestionPage.OptionList)
                        {
                            DocumentFieldOption option = new DocumentFieldOption()
                            {
                                DocumentFieldId = field.Id,
                                OptionName = item.Text,
                                Sort = (short)(item.Sort.HasValue ? item.Sort.Value : 0),
                                MemoType = (byte)item.MemoType,
                            };
                            _unitOfWork.DocumentRepository.InsertFieldOption(option);
                        }
                    }
                }
                _unitOfWork.Complete();
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
                DocumentField field = _unitOfWork.DocumentRepository.GetDocumentField(fieldId);
                if (field != null)
                {
                    model.QuestionPage.DocumentFieldId = field.Id;
                    model.QuestionPage.Caption = field.FieldName;
                    model.QuestionPage.WordLimit = field.WordLimit;
                    model.QuestionPage.FileSizeLimit = field.FileSizeLimit;
                    model.QuestionPage.FileExtension = field.FileExtension;
                    model.QuestionPage.IsIncludedExport = field.IsIncludedExport;
                    model.QuestionPage.IsRequired = field.IsRequired;
                    model.QuestionPage.IsEditable = field.IsEditable;
                    model.QuestionPage.Note = field.Note;
                    model.QuestionPage.AnswerType = field.FieldType;
                    model.QuestionPage.OptionList.Clear();
                    if (model.QuestionPage.AnswerType == (int)AnswerTypeEnum.SingleChoice || model.QuestionPage.AnswerType == (int)AnswerTypeEnum.MultipleChoice)
                    {
                        IEnumerable<DocumentFieldOption> optionList = _unitOfWork.DocumentRepository.GetFieldOption(fieldId);
                        if (optionList.Any())
                        {
                            foreach (var item in optionList)
                            {
                                model.QuestionPage.OptionList.Add(new AnswerOption()
                                {
                                    Text = item.OptionName,
                                    MemoType = item.MemoType,
                                    Sort = item.Sort,
                                    MemoText = EnumHelper.GetDescription((MemoTypeEnum)item.MemoType),
                                });
                            }
                        }
                    }
                }
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
            DocumentField field = _unitOfWork.DocumentRepository.GetDocumentField(fieldId);
            _unitOfWork.DocumentRepository.DeleteField(fieldId);
            _unitOfWork.DocumentRepository.DeleteFieldOptions(fieldId);
            IEnumerable<DocumentField> fields = _unitOfWork.DocumentRepository.GetQuestionFieldList(field.DocumentId);
            int q = 0;
            foreach (var item in fields)
            {
                item.Sort = q;
                q++;
            }
            _unitOfWork.DocumentRepository.UpdateFieldSort(fields);
            _unitOfWork.Complete();
            return RedirectToAction("Form", new { id = field.DocumentId, tab = "1" });
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
            int d = direction ? -1 : 1;
            if (TempData.Peek(sessionName) is string jsonText)
            {
                DocumentViewModel model = JsonConvert.DeserializeObject<DocumentViewModel>(jsonText)!;
                DocumentField field = _unitOfWork.DocumentRepository.GetDocumentField(fieldId);
                if (field != null)
                {
                    field.Sort += d;
                    model.QuestionPage.FieldList.FirstOrDefault(x => x.Id == field.Id)!.Sort += d;
                    DocumentField changedField = _unitOfWork.DocumentRepository.GetDocumentField(field.DocumentId, field.Sort)!;
                    changedField.Sort -= d;
                    model.QuestionPage.FieldList.FirstOrDefault(x => x.Id == changedField.Id)!.Sort -= d;
                    List<DocumentField> updateFields = new List<DocumentField>();
                    updateFields.Add(field);
                    updateFields.Add(changedField);
                    _unitOfWork.DocumentRepository.UpdateFieldSort(updateFields);
                }
                model.QuestionPage.FieldList = model.QuestionPage.FieldList.OrderBy(x => x.Sort).ToList();
                _unitOfWork.Complete();
                TempData[sessionName] = JsonConvert.SerializeObject(model);
                ViewBag.Tab = "1";
                return View("Form", model);
            }
            return RedirectToAction("Index");
        }

        #endregion


        public void GetAnswerTypeSettings(List<SelectListItem> list)
        {
            foreach (var item in Enum.GetValues(typeof(AnswerTypeEnum)))
            {
                list.Add(new SelectListItem()
                {
                    Text = EnumHelper.GetDescription((AnswerTypeEnum)item),
                    Value = ((int)item).ToString(),
                });
            }
        }
        public void GetMemoTypeSettings(List<SelectListItem> list)
        {
            foreach (var item in Enum.GetValues(typeof(MemoTypeEnum)))
            {
                list.Add(new SelectListItem()
                {
                    Text = EnumHelper.GetDescription((MemoTypeEnum)item),
                    Value = ((int)item).ToString(),
                });
            }
        }
    }
}