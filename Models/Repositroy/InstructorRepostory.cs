using Assigment2.Models.interfaces;
using Assigment2.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Assigment2.Models.Repositroy
{
    public class instructorRepostory : IinstructorRepository
    {
        AppDbContext context;// = new AppDbContext();
      public instructorRepostory(AppDbContext DB)
        {
            context = DB;
        }   
        public async Task<List<InstructorViewModel>> GetAll()
        {
           var  instructors =await context.Instructors.Include(D=>D.Department).Include(C=>C.Course).Select(IVM=>new InstructorViewModel
           {
               Address = IVM.Address,
               CourseName = IVM.Course.Name,
               DepartmentName = IVM.Department.Name,
               InstructorID = IVM.InstructorId,
               Image = IVM.Image,
               Name = IVM.Name,
               Salary = IVM.Salary
           }).ToListAsync();
            return instructors; ;
        }

        public  List<Instructor> GetAll2()
        {
            List<Instructor> instructors = context.Instructors
                .Include(D => D.Department)
                .Include(C => C.Course)
                .Select(I => new Instructor
                {
                    InstructorId = I.InstructorId,
                    Name = I.Name,
                    Address = I.Address,
                    Salary = I.Salary,
                    CourseId = I.CourseId,
                    DepartmentId = I.DepartmentId,
                    Image = I.Image,
                    Course = I.Course,
                    Department = I.Department,
                })
                .ToList();
            return instructors;
        }       
        public async Task<int> GetInstructorCount()
        {
            return await context.Instructors.CountAsync(); // Use CountAsync() on IQueryable
        }
        public Instructor GetById(int id)
        {
            Instructor instructor = context.Instructors.FirstOrDefault(d => d.InstructorId == id);
            return instructor;
        }
        public void Add(Instructor NewInstructor)
        {
            context.Instructors.Add(NewInstructor);
            context.SaveChanges();
        }
        public void Update(int Id, Instructor NewInstructor)
        {
            // old referance
            Instructor oldInstructor = GetById(Id);
            // Set New Values
            oldInstructor.Name = NewInstructor.Name;
            oldInstructor.Address = NewInstructor.Address;
            oldInstructor.DepartmentId = NewInstructor.DepartmentId;
            oldInstructor.CourseId = NewInstructor.CourseId;
            oldInstructor.Image = NewInstructor.Image;
            oldInstructor.Salary = NewInstructor.Salary;
            oldInstructor.Salary = NewInstructor.Salary;

            //Save
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            Instructor instructor = GetById(id);
            context.Instructors.Remove(instructor);
            context.SaveChanges();
        }

       
    }
}
