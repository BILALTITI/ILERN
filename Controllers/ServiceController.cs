using Assigment2.Filters;
using Assigment2.Models.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Assigment2.Controllers
{
    public class ServiceController : Controller
    {
        // DIP 
        IDepartmentRepository departmentRepository;

        // inject bult in service 
        private readonly IConfiguration Config;
        // Inject Ask (DI)
        public ServiceController(IDepartmentRepository departmentRepository,IConfiguration confi )
        {
            this.departmentRepository = departmentRepository;
this.Config = confi ;
        }

        [MyFilter]
        public IActionResult TestFilter()
        {
            return Content("Hi");
        }
        public IActionResult Index([FromServices]IDepartmentRepository DepRepo )//inject the method 
        {
         var Name= Config.GetSection("ConnectionStrings"); 
            return View(Name);
        }
    }
    
}
