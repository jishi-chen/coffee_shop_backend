﻿using coffee_shop_backend.Enums;
using coffee_shop_backend.Models;
using coffee_shop_backend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class QuestionnaireFrontController : BaseController
    {
        private HttpContext? _context;

        public QuestionnaireFrontController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            IEnumerable<Application> model = _db.Applications.Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now).Where(x => x.IsEnabled == true).ToList();
            return View(model);
        }

        [Route("Detail")]
        public IActionResult Detail(string id)
        {
            Application application = _db.Applications.Find(Guid.Parse(id))!;
            IEnumerable<ApplicationField> fields = _db.ApplicationFields.Where(x => x.ApplicationId.ToString() == id).OrderBy(x => x.Sort).ToList();
            QuestionDetailViewModel model = new QuestionDetailViewModel()
            {
                ID = application.Id,
                Caption = application.Caption,
                HeadText = application.HeadText,
                FooterText = application.FooterText,
            };
            model.Questions = _db.ApplicationFields
                                .Where(x => x.ApplicationId.ToString() == id)
                                .OrderBy(x => x.Sort)
                                .Select(x => new ApplicationFieldViewModel
                                {
                                    ID = x.Id.ToString(),
                                    FieldName = x.FieldName,
                                    Note = x.Note,
                                    FieldType = (AnswerTypeEnum)x.FieldType,
                                    WordLimit = x.WordLimit,
                                    RowLimit = x.RowLimit,
                                    FileSizeLimit = x.FileSizeLimit,
                                    IsRequired = x.IsRequired,
                                    IsFixed = x.IsFixed,
                                    Sort = x.Sort,
                                }).ToList();
            foreach (var item in model.Questions)
            {
                if (item.FieldType == AnswerTypeEnum.SingleChoice || item.FieldType == AnswerTypeEnum.MultipleChoice || item.FieldType == AnswerTypeEnum.DropDownList)
                {
                    item.Options = _db.ApplicationFieldOptions
                                    .Where(x => x.ApplicationFieldId.ToString() == item.ID)
                                    .OrderBy(x => x.Sort)
                                    .Select(x => new ApplicationFieldOptionViewModel
                                    {
                                        ID = x.Id.ToString(),
                                        OptionName = x.OptionName,
                                        Sort = x.Sort,
                                        MemoType = (MemoTypeEnum)x.MemoType,
                                        Checked = false,
                                    }).ToList();
                }
            }
            return View(model);
        }

        [HttpPost]
        [Route("Form")]
        [ValidateAntiForgeryToken]
        public IActionResult Form(FormCollection collection)
        {

            return View();
        }
    }
}