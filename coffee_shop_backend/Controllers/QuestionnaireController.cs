using coffee_shop_backend.Enums;
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
    public class QuestionnaireController : BaseController
    {
        private HttpContext? _context;
        private string sessionName = "ViewModel";

        public QuestionnaireController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            IEnumerable<Application> model = _db.Applications.ToList();
            return View(model);
        }

        [Route("Form")]
        public IActionResult Form(string id, string parentId, string tab)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                model.Info = _db?.Applications.Select(x => new BasicInformation
                {
                    Id = x.Id,
                    Caption = x.Caption,
                    IsEnabled = x.IsEnabled,
                    Sort = x.Sort,
                    StartDate = x.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                    HeadText = x.HeadText,
                    FooterText = x.FooterText,
                }).FirstOrDefault(x => x.Id.ToString() == id)!;

                model.Question.QuestionList = _db.ApplicationFields.Where(x => x.ApplicationId.ToString() == id).ToList();
            }
            GetAnswerTypeSettings(model.Question.AnswerTypes);
            GetMemoTypeSettings(model.Question.MemoTypes);
            
            string modelString = JsonConvert.SerializeObject(model);
            TempData[sessionName] = modelString;
            ViewBag.Tab = tab;
            return View(model);
        }

        [HttpPost]
        [Route("Form")]
        [ValidateAntiForgeryToken]
        public IActionResult Form(QuestionnaireViewModel model)
        {
            if (string.IsNullOrEmpty(model.Info.Caption))
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
                //新增
                if (model.Info.Id == Guid.Empty)
                {
                    Application Application = new Application()
                    {
                        Id = Guid.NewGuid(),
                        Sort = model.Info.Sort,
                        Caption = model.Info.Caption,
                        StartDate = DateTime.Parse(model.Info.StartDate),
                        EndDate = DateTime.Parse(model.Info.EndDate),
                        IsEnabled = model.Info.IsEnabled,
                        Hits = 0,
                        HeadText = model.Info.HeadText,
                        FooterText = model.Info.FooterText,
                        Creator = "Admin",
                        CreateDate = DateTime.Now,
                    };
                    _db?.Applications.Add(Application);
                }
                //修改
                else
                {
                    Application? Application = _db?.Applications.FirstOrDefault(x => x.Id == model.Info.Id);
                    if (Application != null)
                    {
                        Application.Caption = model.Info.Caption;
                        Application.HeadText = model.Info.HeadText;
                        Application.FooterText = model.Info.FooterText;
                        Application.IsEnabled = model.Info.IsEnabled;
                        Application.StartDate = DateTime.Parse(model.Info.StartDate);
                        Application.EndDate = DateTime.Parse(model.Info.EndDate);
                        Application.Sort = model.Info.Sort;
                        Application.Updator = "Admin";
                        Application.UpdateDate = DateTime.Now;
                    }
                }
                _db?.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("FormQuestion")]
        [ValidateAntiForgeryToken]
        public IActionResult FormQuestion(QuestionContent Question)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                QuestionnaireViewModel model = JsonConvert.DeserializeObject<QuestionnaireViewModel>(jsonText)!;
                if (!ModelState.IsValid)
                {
                    ViewBag.Tab = "1";
                    return View("Form", model);
                }
                    
                ApplicationField question = new ApplicationField();
                //新增
                if (Question.QuestionId == Guid.Empty)
                {
                    question = new ApplicationField()
                    {
                        Id = Guid.NewGuid(),
                        ApplicationId = model.Info.Id,
                        FieldName = Question.Caption,
                        FieldType = (byte)Question.AnswerType,
                        IsRequired = Question.IsNeedAnswer,
                        IsIncludedExport = Question.IsIncludedExport,
                        WordLimit = (short)Question.WordLimit,
                        FileSizeLimit = (short)Question.FileSizeLimit,
                        Note = Question.Note,
                        Creator = "Admin",
                        CreateDate = DateTime.Now,
                    };
                    IEnumerable<ApplicationField> list = _db.ApplicationFields.Where(x => x.ApplicationId == model.Info.Id);
                    if (list.Any())
                    {
                        question.Sort = list.OrderByDescending(x => x.Sort).FirstOrDefault()!.Sort;
                        question.Sort++;
                    }
                    else
                        question.Sort = 0;
                    _db.ApplicationFields.Add(question);
                }
                //編輯
                else
                {
                    question = _db.ApplicationFields.FirstOrDefault(x => x.Id == Question.QuestionId)!;
                    question.FieldName = Question.Caption;
                    question.FieldType = (byte)Question.AnswerType;
                    question.IsRequired = Question.IsNeedAnswer;
                    question.IsIncludedExport = Question.IsIncludedExport;
                    question.WordLimit = (short)Question.WordLimit;
                    question.FileSizeLimit = (short)Question.FileSizeLimit;
                    question.Note = Question.Note;
                    question.Updator = "Admin";
                    question.UpdateDate = DateTime.Now;
                }

                
                if (question.FieldType == (int)AnswerTypeEnum.SingleChoice || question.FieldType == (int)AnswerTypeEnum.MultipleChoice)
                {
                    IEnumerable<ApplicationFieldOption> oldAnswer = _db.ApplicationFieldOptions.Where(x => x.ApplicationFieldId == question.Id);
                    _db.ApplicationFieldOptions.RemoveRange(oldAnswer);
                    if (model.Question.AnswerOptions.Any())
                    {
                        foreach (var item in model.Question.AnswerOptions)
                        {
                            ApplicationFieldOption answer = new ApplicationFieldOption()
                            {
                                Id = Guid.NewGuid(),
                                ApplicationFieldId = question.Id,
                                OptionName = item.Text,
                                Sort = (short)(item.Sort.HasValue ? item.Sort.Value : 0),
                                MemoType = (byte)item.MemoType,
                            };
                            _db.ApplicationFieldOptions.Add(answer);
                        }
                    }
                }
                _db.SaveChanges();
                model.Question = new QuestionContent();
                GetAnswerTypeSettings(model.Question.AnswerTypes);
                GetMemoTypeSettings(model.Question.MemoTypes);
                model.Question.QuestionList = _db.ApplicationFields.Where(x => x.ApplicationId == model.Info.Id).ToList();
                ModelState.Clear();
                TempData[sessionName] = JsonConvert.SerializeObject(model);
                ViewBag.Tab = "1";
                return View("Form", model);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("EditQuestion")]
        public IActionResult EditQuestion(string questionId)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                QuestionnaireViewModel model = JsonConvert.DeserializeObject<QuestionnaireViewModel>(jsonText)!;
                ApplicationField question = _db.ApplicationFields.Find(Guid.Parse(questionId))!;
                if (question != null)
                {
                    model.Question.QuestionId = question.Id;
                    model.Question.Caption = question.FieldName;
                    model.Question.WordLimit = question.WordLimit;
                    model.Question.FileSizeLimit = question.FileSizeLimit;
                    model.Question.IsIncludedExport = question.IsIncludedExport;
                    model.Question.IsNeedAnswer = question.IsRequired;
                    model.Question.Note = question.Note;
                    model.Question.AnswerType = question.FieldType;
                    model.Question.AnswerOptions.Clear();
                    if (model.Question.AnswerType == (int)AnswerTypeEnum.SingleChoice || model.Question.AnswerType == (int)AnswerTypeEnum.MultipleChoice)
                    {
                        IEnumerable<ApplicationFieldOption> answerList = _db.ApplicationFieldOptions.Where(x => x.ApplicationFieldId == question.Id);
                        if (answerList.Any())
                        {
                            foreach (var item in answerList)
                            {
                                model.Question.AnswerOptions.Add(new AnswerOption()
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
        [Route("DeleteQuestion")]
        public IActionResult DeleteQuestion(string questionId)
        {
            ApplicationField question = _db.ApplicationFields.Find(Guid.Parse(questionId))!;
            IEnumerable<ApplicationFieldOption> options = _db.ApplicationFieldOptions.Where(x => x.ApplicationFieldId == question.Id);
            _db.ApplicationFields.Remove(question);
            _db.ApplicationFieldOptions.RemoveRange(options);
            _db.SaveChanges();
            IEnumerable<ApplicationField> questions = _db.ApplicationFields.Where(x => x.ApplicationId == question.ApplicationId).OrderBy(x => x.Sort).ToList();
            int q = 0;
            foreach (var item in questions)
            {
                item.Sort = q;
                q++;
            }
            _db.SaveChanges();
            return RedirectToAction("Form", new { id = question.ApplicationId, tab = "1" });
        }

        [HttpGet]
        [Route("SetSortUp")]
        public IActionResult SetSortUp(string questionId)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                QuestionnaireViewModel model = JsonConvert.DeserializeObject<QuestionnaireViewModel>(jsonText)!;
                ApplicationField question = _db.ApplicationFields.Find(Guid.Parse(questionId))!;
                if (question != null)
                {
                    question.Sort--;
                    model.Question.QuestionList.Find(x => x.Id == question.Id)!.Sort -= 1;
                    ApplicationField before = _db.ApplicationFields.FirstOrDefault(x => x.ApplicationId == question.ApplicationId && x.Sort == question.Sort)!;
                    before.Sort++;
                    model.Question.QuestionList.Find(x => x.Id == before.Id)!.Sort += 1;
                }
                _db.SaveChanges();
                TempData[sessionName] = JsonConvert.SerializeObject(model);
                ViewBag.Tab = "1";
                return View("Form", model);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("SetSortDown")]
        public IActionResult SetSortDown(string questionId)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                QuestionnaireViewModel model = JsonConvert.DeserializeObject<QuestionnaireViewModel>(jsonText)!;
                ApplicationField question = _db.ApplicationFields.Find(Guid.Parse(questionId))!;
                if (question != null)
                {
                    question.Sort++;
                    model.Question.QuestionList.Find(x => x.Id == question.Id)!.Sort += 1;
                    ApplicationField before = _db.ApplicationFields.FirstOrDefault(x => x.ApplicationId == question.ApplicationId && x.Sort == question.Sort)!;
                    before.Sort--;
                    model.Question.QuestionList.Find(x => x.Id == before.Id)!.Sort -= 1;
                }
                _db.SaveChanges();
                TempData[sessionName] = JsonConvert.SerializeObject(model);
                ViewBag.Tab = "1";
                return View("Form", model);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("AddToList")]
        public JsonResult AddToList(AnswerOption Question)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                QuestionnaireViewModel model = JsonConvert.DeserializeObject<QuestionnaireViewModel>(jsonText)!;
                if (Question.Sort == null) //新增
                {
                    Question.Sort = model.Question.AnswerOptions.Any() ? model.Question.AnswerOptions.OrderByDescending(x => x.Sort).FirstOrDefault()!.Sort + 1 : 0;
                    Question.MemoText = EnumHelper.GetDescription((MemoTypeEnum)Question.MemoType);
                    model.Question.AnswerOptions.Add(Question);
                }
                else //編輯
                {
                    AnswerOption option = model.Question.AnswerOptions.FirstOrDefault(x => x.Sort == Question.Sort)!;
                    option.Text = Question.Text;
                    option.MemoType = Question.MemoType;
                    option.MemoText = EnumHelper.GetDescription((MemoTypeEnum)option.MemoType);
                    model.Question.NewOption = new AnswerOption();
                }
                ModelState.Clear();
                TempData[sessionName] = JsonConvert.SerializeObject(model);
                return this.Json(model.Question.AnswerOptions);
            }
            return Json("Insert Error");
        }

        [HttpGet]
        [Route("EditSelectOption")]
        public JsonResult EditSelectOption(int index)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                QuestionnaireViewModel model = JsonConvert.DeserializeObject<QuestionnaireViewModel>(jsonText)!;
                model.Question.NewOption = model.Question.AnswerOptions.FirstOrDefault(x => x.Sort == index)!;
                TempData[sessionName] = JsonConvert.SerializeObject(model);
                return this.Json(model.Question.NewOption);
            }
            return Json("Read Error");
        }

        [HttpGet]
        [Route("DeleteSelectOption")]
        public JsonResult DeleteSelectOption(int index)
        {
            if (TempData.Peek(sessionName) is string jsonText)
            {
                QuestionnaireViewModel model = JsonConvert.DeserializeObject<QuestionnaireViewModel>(jsonText)!;
                AnswerOption option = model.Question.AnswerOptions.FirstOrDefault(x => x.Sort == index)!;
                model.Question.AnswerOptions.Remove(option);
                model.Question.AnswerOptions = model.Question.AnswerOptions.OrderBy(x => x.Sort).ToList();
                int q = 0;
                foreach (var item in model.Question.AnswerOptions)
                {
                    item.Sort = q;
                    q++;
                }
                model.Question.NewOption = new AnswerOption();
                ModelState.Clear();
                TempData[sessionName] = JsonConvert.SerializeObject(model);
                return this.Json(model.Question.AnswerOptions);
            }
            return Json("Delete Error");
        }

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
