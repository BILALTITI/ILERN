using Assigment2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assigment2.Controllers
{
    public class DetailsController : Controller
    {
        public IActionResult IndexDetails()
        {
            using (var context = new AppDbContext())
            {
              var CourseModel1=  context.Courses.Include(D=>D.Department).ToList();



            return View(CourseModel1);
             }

             
        }
    }
}
