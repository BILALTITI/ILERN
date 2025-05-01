using Assigment2.ViewModel;

namespace Assigment2.Models.interfaces
{
    public interface ITraineeRepository
    {
          Task<List<TraineeViewModel>> GetAll();
       List<Trainee> GetAll2();

        Task<int> GetTraineesCount();

        List<Trainee> GetTraneeByDepartmentId(int id);
        
        Trainee GetById(int id);
        
        Task Add(Trainee NewTrainee);


        Task Update(int Id, Trainee NewTrainee);


        void Delete(int id);
         
    }
}
