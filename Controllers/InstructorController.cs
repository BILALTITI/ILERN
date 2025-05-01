using Assigment2.Models;
using Assigment2.Models.interfaces;
using Assigment2.Models.Repositroy;
using Assigment2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Assigment2.Controllers
{
    public class InstructorController : Controller
    {

        private readonly IinstructorRepository _instructorRepostory;
        private readonly IDepartmentRepository _departmentRepostory;
        private readonly ICourseRepository _courseRepostory;

        public InstructorController(
            IinstructorRepository instructorRepostory,
            IDepartmentRepository departmentRepostory,
            ICourseRepository courseRepostory)
        {
            _instructorRepostory = instructorRepostory;
            _departmentRepostory = departmentRepostory;
            _courseRepostory = courseRepostory;
        }

        public IActionResult Index(int pg = 1)
        {
            const int pageSize = 8;
            var query = _instructorRepostory.GetAll2(); // Returns IQueryable<Course>

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
        // this function Will Call From Instructor Model (Remote)in Salary Property
        public IActionResult CheckSalary(int Salry)
        {
            if (Salry > 2000)
            {

                return Json(true);
            }
            else
            {
                return Json(false);

            }

        }
        // anchor tag
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Instructor InstructorModel1 = _instructorRepostory.GetById(Id);
            if (InstructorModel1 == null)
            {
                return NotFound();
            }
            _instructorRepostory.Delete(Id);
            return Ok();
        }
        public async Task<IActionResult> Edit(int Id) // Get data from DB
        {
            var instructor = _instructorRepostory.GetById(Id) ?? new Instructor();
            if (instructor == null)
            {
                return NotFound();
            }

            instructor.Image ??= "/images/1.png"; // Default image

            ViewBag.DeptList = new SelectList(_departmentRepostory.GetAll2(), "DepartmentId", "Name", instructor.DepartmentId);
            ViewBag.CourseList = new SelectList(_courseRepostory.GetAll2(), "CourseId", "Name", instructor.CourseId);

            return View("AddNew", instructor);
        }

        public async Task<IActionResult> AddNew()
        {
            ViewBag.DeptList = new SelectList(_departmentRepostory.GetAll2(), "DepartmentId", "Name");
            ViewBag.CourseList = new SelectList(_courseRepostory.GetAll2(), "CourseId", "Name");

            return View(new Instructor());
        }

        [HttpPost]
         
        public async Task<IActionResult> SaveEdit(Instructor UpdatedInstructor, IFormFile? ImageFile)
        {
            // Preserve the existing image if no new file is uploaded
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                UpdatedInstructor.Image = "/images/" + fileName;
            }
            //else if (string.IsNullOrEmpty(UpdatedInstructor.Image))  // Retain old image if none uploaded
            //{
            //    UpdatedInstructor.Image = "/images/default.jpg";  // Ensure default image is used for new records
            //}

            if (!ModelState.IsValid)
            {
                // Retain dropdown selections
                ViewBag.DeptList = new SelectList(_departmentRepostory.GetAll2(), "DepartmentId", "Name", UpdatedInstructor.DepartmentId);
                ViewBag.CourseList = new SelectList(_courseRepostory.GetAll2(), "CourseId", "Name", UpdatedInstructor.CourseId);

                return View("AddNew", UpdatedInstructor);
            }

            var existingInstructor = UpdatedInstructor.InstructorId == 0
                ? new Instructor()
                : _instructorRepostory.GetById(UpdatedInstructor.InstructorId);

            if (existingInstructor == null)
            {
                return NotFound();
            }

            existingInstructor.Name = UpdatedInstructor.Name;
            existingInstructor.Address = UpdatedInstructor.Address;
            existingInstructor.Salary = UpdatedInstructor.Salary;
            existingInstructor.DepartmentId = UpdatedInstructor.DepartmentId;
            existingInstructor.CourseId = UpdatedInstructor.CourseId;

            // Preserve existing image if no new one was uploaded
            if (string.IsNullOrEmpty(UpdatedInstructor.Image))
            {
                existingInstructor.Image = UpdatedInstructor.Image;
            }

            if (existingInstructor.InstructorId == 0)
            {
                _instructorRepostory.Add(existingInstructor);
            }
            else
            {
                _instructorRepostory.Update(existingInstructor.InstructorId, existingInstructor);
            }

            return RedirectToAction("Index");
        }

    }
}
