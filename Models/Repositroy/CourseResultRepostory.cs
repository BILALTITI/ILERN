using Assigment2.Models.interfaces;
using Assigment2.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Assigment2.Models.Repositroy
{
    public class CourseResultRepostory:ICourseResultReipoistory
    {
        AppDbContext context;// = new AppDbContext();

        public CourseResultRepostory(AppDbContext DB)
        {
            context = DB;
        }
        public async Task <List<CourseResultViewModel>> GetAll()
        {
           var courseResults =await context.CourseResults.Include(C=>C.Course).Include(T=>T.Trainee).Select(CVM=>new CourseResultViewModel
           {
               CourseResultID = CVM.CourseResultId,
               CourseName = CVM.Course.Name,
               TraineeName = CVM.Trainee.Name,
               Degree = CVM.Degree

           }).ToListAsync();
            return courseResults;
        }
        
        public  List<CourseResult > GetAll2()
        {
           var courseResults = context.CourseResults.Include(C=>C.Course).Include(T=>T.Trainee).Select(CVM=>new CourseResult 
           {
               CourseResultId = CVM.CourseResultId,
               Course = CVM.Course ,
               Trainee = CVM.Trainee ,
               Degree = CVM.Degree

           }).ToList ();
            return courseResults;
        }
        public async Task<int> GetCourseResultCount()
        {
            return await context.CourseResults.CountAsync(); // Use CountAsync() on IQueryable
        }

        public CourseResult GetById(int id)
        {
            CourseResult courseResult = context.CourseResults.FirstOrDefault(d => d.CourseResultId == id);
            return courseResult;
        }
        public void Add(CourseResult NewCourseResult)
        {
            context.CourseResults.Add(NewCourseResult);
            context.SaveChanges();
        }
        public void Update(int Id, CourseResult NewCourseResult)
        {
            // old referance
            CourseResult oldCourseResult = GetById(Id);
            // Set New Values
            oldCourseResult.TraineeId = NewCourseResult.TraineeId;
            oldCourseResult.CourseId = NewCourseResult.CourseId;
            oldCourseResult.Degree = NewCourseResult.Degree;
            //Save
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            CourseResult courseResult = GetById(id);
            context.CourseResults.Remove(courseResult);
            context.SaveChanges();
        }
    }
}
