using Assigment2.Filters;
using Assigment2.Models;
using Assigment2.Models.interfaces;
using Assigment2.Models.Repositroy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Threading.Tasks;


namespace Assigment2.Controllers
{
    // if we put the filter here will apply in the whole controller
    // 1 bultin Filter
    // 2 Custome Filter 
    //[Authorize][
   // [MyFilter]
    
    public class CourseController : Controller
    {
        // when applly IOC Using the dependency injection we remove the new keyword 
     private readonly   ICourseRepository _courseRepoistory; // new ICourseRepoistory();
       private readonly IDepartmentRepository _DepartmentRepoistory; //new IDepartmentRepoistory();
                                                                     //   CourseRepoistory courseRepoistory = new CourseRepoistory();

        // action can return  view - JSON  - RediercFile -PartialView - Content - StatusCode
     
        //filter will excute
        //[Authorize]
        //[ResponseCache(Duration =10,Location =)]
        public CourseController(ICourseRepository courseRepoistory, IDepartmentRepository DepartmentRepoistory)
        {
           _courseRepoistory= courseRepoistory;
           _DepartmentRepoistory = DepartmentRepoistory;
            
        }
        public IActionResult Index2(int pageNumber = 1, int pageSize = 10, string searchTerm = "")
        {
            //// Input validation
            //pageNumber = Math.Max(1, pageNumber);
            //pageSize = Math.Clamp(pageSize, 5, 100);

            //// Get paged results from repository
            //var model = _courseRepoistory.GetPagedCourses(pageNumber, pageSize, searchTerm);

            //ViewBag.SearchTerm = searchTerm;
            return View( );
        }
        public IActionResult Index(int pg = 1)
        {
            const int pageSize = 8;
            var query = _courseRepoistory.GetAll2(); // Returns IQueryable<Course>

            int recsCount = query.Count(); // Executes SQL COUNT()
            int totalPages = (int)Math.Ceiling(recsCount / (double)pageSize);

            // Ensure pg is within valid range
            pg = Math.Clamp(pg, 1, totalPages > 0 ? totalPages : 1);

            // Apply pagination at the database level
            var data = query.Skip((pg - 1) * pageSize).Take(pageSize).ToList();

            // Ensure totalPages is at least 1 in PagedResult
            var pager = new PagedResult(recsCount, pg, pageSize);
            ViewBag.Pager = pager;

            return View(data);
        }

        public IActionResult DetailsUsingPartialView(int id)
        {
            Course course = _courseRepoistory.GetById(id);                
            return PartialView("_CourseCardPartial", course);
        }
         public IActionResult Detials(int Id)//get data from DB
        {
            return View(_courseRepoistory.GetById(Id));
        }
        // anchor tag
        public async Task<IActionResult> Edit(int Id)
        {
            var courseModel = _courseRepoistory.GetById(Id) ?? new Course();

            var departments = await _DepartmentRepoistory.GetAll();
            var courses = await _courseRepoistory.GetAll();

            // ✅ Convert lists to SelectList before sending them to the view
            ViewBag.DeptList = new SelectList(departments, "DepartmentId", "Name");
            ViewBag.CourseList = new SelectList(courses, "CourseId", "Name");

            return View("AddNew", courseModel);
        }



        public async Task<IActionResult> AddNew()
        {
      // Using ViewBag for easy access in Razor
            ViewBag.DeptList = new SelectList(await _DepartmentRepoistory.GetAll(), "DepartmentId", "Name");

            ViewBag.CourseList = _courseRepoistory.GetAll();

            return View(new Course()); // Return an empty course object for new entry
        }

     
        [HttpPost]
        public IActionResult SaveEdit(Course UpdatedCourse)
        {
            if (!ModelState.IsValid)
            {
                return View("AddNew", UpdatedCourse);
            }

            if (UpdatedCourse.CourseId == 0) // Adding a new course
            {
                var newCourse = new Course
                {
                    Name = UpdatedCourse.Name,
                    Degree = UpdatedCourse.Degree,
                    MinDegree = UpdatedCourse.MinDegree,
                    DepartmentId = UpdatedCourse.DepartmentId,
                };

                _courseRepoistory.Add(newCourse);
            }
            else
            {
                // Get the existing course from the database
                var existingCourse = _courseRepoistory.GetById(UpdatedCourse.CourseId); // 🔄 Use UpdatedCourse.Id instead of Id
                if (existingCourse == null)
                {
                    return NotFound(); // Course not found
                }

                // Update existing course properties
                existingCourse.Name = UpdatedCourse.Name;
                existingCourse.Degree = UpdatedCourse.Degree;
                existingCourse.MinDegree = UpdatedCourse.MinDegree;
                existingCourse.DepartmentId = UpdatedCourse.DepartmentId;

                _courseRepoistory.Update(UpdatedCourse.CourseId, existingCourse); // ✅ Use UpdatedCourse.Id
            }

            return RedirectToAction("Index");
        }
    

        [HttpPost]
        public IActionResult Delete(int Id) { 
        
            if (Id==0)
            {
            return NotFound();
                }

            var DeletedID = _courseRepoistory.GetById(Id);

            _courseRepoistory.Delete(DeletedID.CourseId);
            return Ok();



        }
    }
   
}
