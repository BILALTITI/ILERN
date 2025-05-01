using Assigment2.ViewModel;

namespace Assigment2.Models.interfaces
{
    public interface IinstructorRepository
    {
        Task<List<InstructorViewModel>> GetAll();
          List<Instructor> GetAll2();

        Task<int> GetInstructorCount();
        Instructor GetById(int id);


          void Add(Instructor NewInstructor);


            void Update(int Id, Instructor NewInstructor);

        void Delete(int id);
    }
}
