using Assigment2.Models;
using Assigment2.Models.interfaces;
using Assigment2.Models.Repositroy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assigment2.Controllers
{
    public class TraineeResultController : Controller
    {
        ITraineeRepository traneeReposetory;
        public TraineeResultController(TraineeRepoistory traneeReposetory)
        {
            this.traneeReposetory = traneeReposetory;
        }   
        public IActionResult Result(int Id)



        {
           
                var CourseModel1 =traneeReposetory.GetById(Id);



                return View(CourseModel1);
            
         }
    }
}
