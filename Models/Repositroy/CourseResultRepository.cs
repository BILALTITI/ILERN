using Assigment2.Models.interfaces;
using Assigment2.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Assigment2.Models.Repositroy
{
    public class CourseResultRepository: ICourseResultReipoistory
    {
        AppDbContext context
            ;//= new AppDbContext();
        //private readonly IConfiguration _configuration;

        public CourseResultRepository(AppDbContext DB)
        {
            context = DB;
        }

        public void Add(CourseResult NewCourseResult)
        {
            context.Add(NewCourseResult);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            var CourseResult = context.CourseResults.Find(id);
            context.CourseResults.Remove(CourseResult);
            context.SaveChanges();
        }
        public async Task <List<CourseResultViewModel>> GetAll()
        {
            var CourseResults = await context.CourseResults.Include(C => C.Course).Include(C => C.Trainee)
                .Select(c => new CourseResultViewModel
                {
                    CourseResultID = c.CourseResultId,
                    CourseName = c.Course.Name,
                    TraineeName = c.Trainee.Name,
                    Degree = c.Degree
                })
                .ToListAsync();
            return CourseResults;
        }
        public List<CourseResult> GetAll2()
        {
            var Result = context.CourseResults.Include(C => C.Course).Include(C => C.Trainee)
                .Select(c => new CourseResult
                {
                    CourseResultId = c.CourseResultId,
                    Course = c.Course,
                    Trainee = c.Trainee,
                    Degree = c.Degree
                }).ToList();

            return Result;
              
        }
        public async Task<int> GetCourseResultCount()
        {
            return await context.CourseResults.CountAsync(); // Use CountAsync() on IQueryable
        }
        public CourseResult GetById(int id)
        {
            return context.CourseResults.Find(id);
        }
        public void Update(int Id, CourseResult NewCourseResult)
        {
            var CourseResult = context.CourseResults.Find(Id);
            CourseResult.CourseId = NewCourseResult.CourseId;
            CourseResult.TraineeId = NewCourseResult.TraineeId;
            CourseResult.Degree = NewCourseResult.Degree;
            context.SaveChanges();
        }
    }
}
