using Assigment2.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Assigment2.Models.interfaces
{
    public interface ICourseRepository
    {
        Task<List<CourseViewModel>> GetAll();
        List< Course> GetAll2();

        Task<PaginatedList<Course>> GetPaginatedCoursesAsync(
               int pageNumber,
               int pageSize,
               string sortOrder = "name");

        Task<int> GetCourseount();

        Course GetById(int id);

        // CRUD 
        void Add(Course NewCourse);

        void Update(int Id, Course NewCourse);

        void Delete(int id);
        
    }
}
