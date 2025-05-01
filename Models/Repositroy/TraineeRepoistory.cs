using Assigment2.Models.interfaces;
using Assigment2.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Assigment2.Models.Repositroy
{
    public class TraineeRepoistory: ITraineeRepository
    {
        AppDbContext context;// = new AppDbContext();
        public TraineeRepoistory(AppDbContext DB)
        {
            context = DB;
        }
        public async Task<List<TraineeViewModel>> GetAll()
        {
            var trainees = await context.Trainees
                .Include(t => t.Department)
                .Include(t => t.Course)
                .Select(t => new TraineeViewModel
                {
                    TraineeId = t.TraineeId,
                    Name = t.Name,
                    Address = t.Address,
                    Grade = t.Grade,
                    CourseName = t.Course.Name,  // Get Course Name instead of ID
                    DepartmentName = t.Department.Name // Get Department Name instead of ID
                })
                .ToListAsync();

            return trainees;
        }
        public List<Trainee> GetAll2()
        {
            List<Trainee> trainees = context.Trainees
                .Include(D => D.Department)
                .Include(C => C.Course)
                .Select(T => new Trainee
                {
                    TraineeId = T.TraineeId,
                    Name = T.Name,
                    Address = T.Address,
                    Grade = T.Grade,
                    courseId = T.courseId,
                    DepartmentId = T.DepartmentId,
                    Image = T.Image,
                    Course = T.Course,
                    Department = T.Department,
                })
                .ToList();

            return trainees;
        }

        public async Task<int> GetTraineesCount()
        {
            return await context.Trainees.CountAsync(); // Use CountAsync() on IQueryable
        }

        public List<Trainee> GetTraneeByDepartmentId(int id)
        {
            List<Trainee> trainees = context.Trainees.Where(d => d.DepartmentId == id).ToList();
            return trainees;
        }
        public Trainee GetById(int id)
        {
            Trainee trainee = context.Trainees.FirstOrDefault(d => d.TraineeId == id);
            return trainee;
        }
        public async Task Add(Trainee NewTrainee)
        {
              context.Trainees.Add(NewTrainee);
            context.SaveChanges();
        }
        public async  Task   Update(int Id, Trainee NewTrainee)
        {
            // old referance
            Trainee oldTrainee = GetById(Id);
            // Set New Values
            oldTrainee.Name = NewTrainee.Name;
            oldTrainee.Address = NewTrainee.Address;
            oldTrainee.DepartmentId = NewTrainee.DepartmentId;
            oldTrainee.courseId = NewTrainee.courseId;
            oldTrainee.Image = NewTrainee.Image;
            oldTrainee.Grade = NewTrainee.Grade;
            //Save
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            Trainee trainee = GetById(id);
            context.Trainees.Remove(trainee);
            context.SaveChanges();
        }
    }
    }
