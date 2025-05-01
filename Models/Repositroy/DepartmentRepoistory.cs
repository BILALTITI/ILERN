using Assigment2.Models.interfaces;
using Assigment2.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Assigment2.Models.Repositroy
{

    //crud operations C=Create, R=Read, U=Update, D=Delete
    public class DepartmentRepoistory : IDepartmentRepository
    {
        AppDbContext context;// = new AppDbContext();
        public DepartmentRepoistory(AppDbContext DB)
        {
            context = DB;
        }
        public async Task<List<DepartmentViewModel>> GetAll()
        {
                 var departments = await context.Departments.Select(DMV=>new DepartmentViewModel{

                DepartmentId = DMV.DepartmentId,

                    Name = DMV.Name,
                    Manager = DMV.Manager,

            }).ToListAsync();


            return departments;
        }
        public  List<Department> GetAll2()
        {
            return context.Departments
                       .ToList();
         ;

        }
        public async Task<int> GetDepartmentCount()
        {
            var count = await context.Departments.CountAsync();
            return count;
        }

        public List<Department> GetAllWithTraineeName()
        {
            List<Department> departments = context.Departments.ToList();
            return departments;
        }
        public Department GetById(int id)
        {
            Department department = context.Departments.FirstOrDefault(d => d.DepartmentId == id);
            return department;
        }
        public void Add(Department Newdepartment)
        {
            context.Departments.Add(Newdepartment);
            context.SaveChanges();
        }
        public void Update(int Id,Department Newdepartment)
        {
            // old referance
            Department oldDepartment = GetById(Id);
            // Set New Values
            oldDepartment.Name = Newdepartment.Name;
            oldDepartment.Manager = Newdepartment.Manager;
             //Save 
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            Department department = GetById(id);
            context.Departments.Remove(department);
            context.SaveChanges();
        }
        public void Add(DepartmentViewModel newDepartment)
        {
            var department = new Department
            {
                Name = newDepartment.Name,
                Manager = newDepartment.Manager
            };
            context.Departments.Add(department);
            context.SaveChanges();
        }
    }
}
