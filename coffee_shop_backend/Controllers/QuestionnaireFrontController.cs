using coffee_shop_backend.Models;
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

            return View();
        }
    }
}
