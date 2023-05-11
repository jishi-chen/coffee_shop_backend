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
            IEnumerable<Examine> model = _db.Examines.Where(x => x.StartDate < DateTime.Now && (x.EndDate.HasValue ? x.EndDate.Value : DateTime.MaxValue) > DateTime.Now)
                .Where(x => x.IsEnabled == true).ToList();
            return View(model);
        }

        [Route("Form")]
        public IActionResult Form(string id)
        {
            QuestionnaireViewModel? model = new QuestionnaireViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                model = _db?.Examines.Select(x => new QuestionnaireViewModel
                {
                    Id = x.Id,
                    Caption = x.Caption,
                    IsEnabled = x.IsEnabled,
                    Sort = x.Sort,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).FirstOrDefault(x => x.Id.ToString() == id);
            }
            

            return View(model);
        }

        [HttpPost]
        [Route("Form")]
        [ValidateAntiForgeryToken]
        public IActionResult Form(QuestionnaireViewModel model)
        {
            if (string.IsNullOrEmpty(model.Caption))
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
                if (model.Id == Guid.Empty)
                {
                    Examine examine = new Examine()
                    {
                        Id = Guid.NewGuid(),
                        Sort = model.Sort,
                        Caption = model.Caption,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        IsEnabled = model.IsEnabled,
                        ExamineNo = "",
                        Hits = 0,
                        Creator = "Admin",
                        CreateDate = DateTime.Now,
                    };
                    _db?.Examines.Add(examine);
                }
                //修改
                else
                {
                    Examine? examine = _db?.Examines.FirstOrDefault(x => x.Id == model.Id);
                    if (examine != null)
                    {
                        examine.Caption = model.Caption;
                        examine.HeadText = model.HeadText;
                        examine.FooterText = model.FooterText;
                        examine.IsEnabled = model.IsEnabled;
                        examine.StartDate = model.StartDate;
                        examine.EndDate = model.EndDate;
                        examine.Sort = model.Sort;
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
