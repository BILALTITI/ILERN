using Assigment2.Models;
using Assigment2.Models.interfaces;
using Assigment2.Models.Repositroy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Assigment2.ViewModel;

namespace Assigment2.Controllers
{
    [Route("Trainee/[action]/{id?}")]
    public class TraineeController : Controller
    {
        private readonly ITraineeRepository _traineeRepoistory;
        private readonly IDepartmentRepository _DepartmentRepoistory;
        private readonly ICourseRepository _courseRepository;

        public TraineeController(ITraineeRepository traineeRepoistory, IDepartmentRepository DepartmentRepoistory, ICourseRepository courseRepository)
        {
            _traineeRepoistory = traineeRepoistory;
            _DepartmentRepoistory = DepartmentRepoistory;
            _courseRepository = courseRepository;
        }
        [HttpGet]
        public IActionResult Index(int pg = 1)
        {
            const int pageSize = 8;
            var query = _traineeRepoistory.GetAll2(); // Returns IQueryable<Course>

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
        [HttpGet]
        public async Task<IActionResult> AddNew()
        {
            ViewBag.DeptList = new SelectList( _DepartmentRepoistory.GetAll2(), "DepartmentId", "Name");
           ViewBag.CourseList = new SelectList( _courseRepository.GetAll2(), "CourseId", "Name");
            return View("Edit",new Trainee());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var trainee = _traineeRepoistory.GetById(id);
            if (trainee == null)
            {
                return NotFound();
            }

            // Ensure image is not null, assign a default image if needed
            trainee.Image ??= "/images/1.png"; // Default image

            // Populate department and course lists
            var departments =   _DepartmentRepoistory.GetAll2();
            var courses =   _courseRepository.GetAll2();

            // Set ViewBag with selected department and course
            ViewBag.DeptList = new SelectList(departments, "DepartmentId", "Name", trainee.DepartmentId);
            ViewBag.CourseList = new SelectList(courses, "CourseId", "Name", trainee.courseId);

            return View(trainee);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var trainee = _traineeRepoistory.GetById(id);
            if (trainee == null)
            {
                return NotFound();
            }
            _traineeRepoistory.Delete(id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(Trainee UpdatedTrainee, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Handle image upload
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                UpdatedTrainee.Image = "/images/" + fileName;  // Store the relative path in the database
            }
            //else if (UpdatedTrainee.TraineeId == 0) // If image not uploaded and it's a new trainee
            //{
            //    UpdatedTrainee.Image = "/images/default.jpg";  // Set a default image
            //}

            if (!ModelState.IsValid)
            {
                // Return the form view if validation fails
                ViewBag.DeptList = new SelectList(_DepartmentRepoistory.GetAll2(), "DepartmentId", "Name");
                ViewBag.CourseList = new SelectList(_courseRepository.GetAll2(), "CourseId", "Name");
                return View("Edit", UpdatedTrainee);
            }

            var existingTrainee = UpdatedTrainee.TraineeId == 0
                ? new Trainee()  // Add new trainee
                : _traineeRepoistory.GetById(UpdatedTrainee.TraineeId);

            if (existingTrainee == null)
            {
                return NotFound();
            }

            // Update trainee details
            existingTrainee.Name = UpdatedTrainee.Name;
            existingTrainee.Address = UpdatedTrainee.Address;
            existingTrainee.Grade = UpdatedTrainee.Grade;
            existingTrainee.DepartmentId = UpdatedTrainee.DepartmentId;
            existingTrainee.courseId = UpdatedTrainee.courseId;
            existingTrainee.Image = UpdatedTrainee.Image;
            if (string.IsNullOrEmpty(UpdatedTrainee.Image))
            {
                existingTrainee.Image = UpdatedTrainee.Image;
            }
            // Save or update trainee
            if (existingTrainee.TraineeId == 0)
            {
                await _traineeRepoistory.Add(existingTrainee);
            }
            else
            {
                await _traineeRepoistory.Update(existingTrainee.TraineeId, existingTrainee);
            }

            return RedirectToAction("Index");
        }


    }
}
