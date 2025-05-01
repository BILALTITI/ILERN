using Assigment2.Models.interfaces;
using Assigment2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace Assigment2.Models.Repositroy
{
    public class CourseRepoistory : ICourseRepository
    {
        AppDbContext context;// = new AppDbContext();
        public CourseRepoistory(AppDbContext DB)
        {
            context = DB;

        }

        public async Task<PaginatedList<Course>> GetPaginatedCoursesAsync(
         int pageNumber,
         int pageSize,
         string sortOrder = "name")
        {
            IQueryable<Course> query = context.Courses
                .Include(c => c.Department)
                .AsNoTracking();

            query = sortOrder.ToLower() switch
            {
                "name_desc" => query.OrderByDescending(c => c.Name),
               
                _ => query.OrderBy(c => c.Name),
            };
            return await PaginatedList<Course>.CreateAsync(
                query,
                pageNumber,
                pageSize);
        }
        public async Task<List<CourseViewModel>> GetAll()
        {
           var  courses = await  context.Courses.Include(D=>D.Department).Select(CVM=>new CourseViewModel
           {

               CourseID = CVM.CourseId,
               CourseName = CVM.Name,
               Degree = CVM.Degree,
               MinDegree = CVM.MinDegree,
               DepartmentName = CVM.Department.Name,
           }).ToListAsync(); ;
            return courses;
        }

        public List<Course> GetAll2()
      {
             return  context.Courses
                    .Include(c => c.Department) // Eagerly load Department
                    .ToList();
        
        
        }  
        public async Task<int> GetCourseount()
        {
            int count = await context.Courses.CountAsync();
            return count;
        }   
        public Course GetById(int id)
        {
            Course course = context.Courses.FirstOrDefault(d => d.CourseId == id);
            return course;
        }
        public void Add(Course NewCourse)
        {
            context.Courses.Add(NewCourse);
            context.SaveChanges();
        }
        public void Update(int Id, Course NewCourse)
        {
            // old referance
            Course oldCourse = GetById(Id);
            // Set New Values
            oldCourse.Name = NewCourse.Name;
            oldCourse.MinDegree = NewCourse.MinDegree;
            oldCourse.DepartmentId = NewCourse.DepartmentId;
            //Save 
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            Course course = GetById(id);
            context.Courses.Remove(course);
            context.SaveChanges();
        }
    }
}
