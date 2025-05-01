using Microsoft.AspNetCore.Mvc.Filters;

namespace Assigment2.Filters
{
    public class MyFilter : Attribute, IActionFilter
    {

        // this action will excute when the action excute then will excute this 
        public void OnActionExecuted(ActionExecutedContext context)
        {
              Console.WriteLine("OnActionExecuted");
        }
        // this will exucte within the action excuteing
        public void OnActionExecuting(ActionExecutingContext context)
        {
                Console.WriteLine("OnActionExecuting");
        }
    }
}
