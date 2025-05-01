using Assigment2.ViewModel;

namespace Assigment2.Models.interfaces
{
    public interface ICourseResultReipoistory
    {

        Task<List<CourseResultViewModel>> GetAll();
        List<CourseResult> GetAll2();
        Task<int> GetCourseResultCount();
        CourseResult GetById(int id);

        void Add(CourseResult NewCourseResult);

        void Update(int Id, CourseResult NewCourseResult);

        void Delete(int id);
        
    }
}
