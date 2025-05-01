using Assigment2.Models;
using Assigment2.Models.interfaces;
using Assigment2.Models.Repositroy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;
using System.Threading.Tasks;

namespace Assigment2.Controllers
{
    public class CourseResultController : Controller
    {
        // when applly IOC Using the dependency injection we remove the new keyword
     private readonly   ICourseResultReipoistory _CourseResultRepostory;

        private readonly ICourseRepository _courseRepository;
        private readonly ITraineeRepository _traineeRepository;
        // apply the dependency injection
        public CourseResultController(ICourseResultReipoistory CourseResultRepostory ,ICourseRepository courseRepository,ITraineeRepository traineeRepository)
        {
           _CourseResultRepostory = CourseResultRepostory;
            _courseRepository = courseRepository;
            _traineeRepository = traineeRepository;
        }
        [HttpGet]
        public IActionResult Index(int pg = 1)
        {
            const int pageSize = 8;
            var query = _CourseResultRepostory.GetAll2(); // Returns IQueryable<Course>

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

        public async Task<IActionResult>  Edit(int Id)
        {
            var courseModel = _CourseResultRepostory.GetById(Id) ?? new CourseResult();

            if (courseModel == null)
            {
                NotFound();
            }    
            // Fetch trainees and courses from repository
            var trainees = _traineeRepository.GetAll2();
            var courses = _courseRepository.GetAll2();

            // Convert to SelectList
            ViewBag.TraineeList = new SelectList(trainees, "TraineeId", "Name");
            ViewBag.CourseList = new SelectList(courses, "CourseId", "Name");

            return View("AddNew", courseModel);
        }



        public async Task<IActionResult> AddNew()
        {
            // Fetch trainees and courses from repository
            var trainees =   _traineeRepository.GetAll2();
            var courses =   _courseRepository.GetAll2();

            // Convert to SelectList
            ViewBag.TraineeList = new SelectList(trainees, "TraineeId", "Name");
            ViewBag.CourseList = new SelectList(courses, "CourseId", "Name");

            return View(new CourseResult()); // Return an empty course object for new entry
        }
        [HttpPost]
        public IActionResult SaveEdit(CourseResult UpdatedCourseResult)
        {
            if (!ModelState.IsValid)
            {
                return View("AddNew", UpdatedCourseResult);
            }

            if (UpdatedCourseResult.CourseResultId == 0) // Adding a new course
            {
                var newCourse = new CourseResult
                {
                    Degree = UpdatedCourseResult.Degree,
                    CourseId = UpdatedCourseResult.CourseId,
                    TraineeId = UpdatedCourseResult.TraineeId,
                };

                _CourseResultRepostory.Add(newCourse);
            }
            else
            {
                // Get the existing course from the database
                var existingCourseResult = _CourseResultRepostory.GetById(UpdatedCourseResult.CourseResultId); // 🔄 Use UpdatedCourseResult.Id instead of Id
                if (existingCourseResult == null)
                {
                    return NotFound(); // Course not found
                }

                // Update existing course properties
                existingCourseResult.Degree = UpdatedCourseResult.Degree;
                existingCourseResult.CourseId = UpdatedCourseResult.CourseId;
                existingCourseResult.TraineeId = UpdatedCourseResult.TraineeId;

                _CourseResultRepostory.Update(UpdatedCourseResult.CourseResultId, existingCourseResult); // ✅ Use UpdatedCourseResult.Id
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            CourseResult CourseResultModel1 = _CourseResultRepostory.GetById(Id);
            if (CourseResultModel1 == null)
            {
                return NotFound();
            }
            _CourseResultRepostory.Delete(Id);
            return Ok();
        }

        // we need only show the data

    }
}
