using coffee_shop_backend.Models;
using coffee_shop_backend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class QuestionnaireController : BaseController
    {
        private HttpContext? _context;

        public QuestionnaireController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            IEnumerable<Examine> model = _db.Examines.Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now).ToList();
            return View(model);
        }

        [Route("Form")]
        public IActionResult Form(string id)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                model.Info = _db?.Examines.Select(x => new BasicInformation
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
            }
            

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
                    Examine examine = new Examine()
                    {
                        Id = Guid.NewGuid(),
                        Sort = model.Info.Sort,
                        Caption = model.Info.Caption,
                        StartDate = DateTime.Parse(model.Info.StartDate),
                        EndDate = DateTime.Parse(model.Info.EndDate),
                        IsEnabled = model.Info.IsEnabled,
                        ExamineNo = "",
                        Hits = 0,
                        HeadText = model.Info.HeadText,
                        FooterText = model.Info.FooterText,
                        Creator = "Admin",
                        CreateDate = DateTime.Now,
                    };
                    _db?.Examines.Add(examine);
                }
                //修改
                else
                {
                    Examine? examine = _db?.Examines.FirstOrDefault(x => x.Id == model.Info.Id);
                    if (examine != null)
                    {
                        examine.Caption = model.Info.Caption;
                        examine.HeadText = model.Info.HeadText;
                        examine.FooterText = model.Info.FooterText;
                        examine.IsEnabled = model.Info.IsEnabled;
                        examine.StartDate = DateTime.Parse(model.Info.StartDate);
                        examine.EndDate = DateTime.Parse(model.Info.EndDate);
                        examine.Sort = model.Info.Sort;
                        examine.Updator = "Admin";
                        examine.UpdateDate = DateTime.Now;
                    }
                }
                _db?.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
