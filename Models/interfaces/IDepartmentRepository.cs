using Assigment2.ViewModel;

namespace Assigment2.Models.interfaces
{
    public interface IDepartmentRepository
    {
        Task <List<DepartmentViewModel>> GetAll();
     List <Department> GetAll2();

        Task<int> GetDepartmentCount();
        List<Department> GetAllWithTraineeName();
 
        Department GetById(int id);

        void Add(Department Newdepartment);

        void Update(int Id, Department Newdepartment);

          void Delete(int id);
     }
}
