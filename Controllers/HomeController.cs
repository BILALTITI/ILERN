using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Assigment2.Models;
using Assigment2.Models.interfaces;
using Microsoft.AspNetCore.Identity;
using Assigment2.Models.Repositroy;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Assigment2.ViewModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
 
namespace Assigment2.Controllers;

 public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICourseRepository _courseRepoistory; 
    private readonly IDepartmentRepository _DepartmentRepoistory;
     private readonly ITraineeRepository _TraineeRepoistory;
    private readonly ICourseResultReipoistory _CourseResultRepostory;
    private readonly IinstructorRepository _instructorRepostory;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, ICourseRepository courseRepoistory, IDepartmentRepository departmentRepository,
        ITraineeRepository traineeRepository ,ICourseResultReipoistory CourseResultReipoistory, IinstructorRepository iinstructorRepository , UserManager<ApplicationUser> userManager )
    {
        _logger = logger;
        _courseRepoistory = courseRepoistory;
        _DepartmentRepoistory = departmentRepository;
        _TraineeRepoistory= traineeRepository;
        _CourseResultRepostory = CourseResultReipoistory;
        _instructorRepostory = iinstructorRepository;
        _userManager = userManager;

    }
    public IActionResult GetNavBar()
    {
        var userImage = User.Identity.IsAuthenticated ? User.Claims.FirstOrDefault(c => c.Type == "ProfilePicture")?.Value : "/images/default-user.png";

        var model = new ProfilePictureViewModel
        {
            ProfilePictureUrl = userImage
        };

        return PartialView("_NavPartial", model);
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new AuthViewModel
        {
            LoginModel = new LoginUserViewModel(),
            RegisterModel = new RegisterUserViewModel()
        };

        // Only show real user data if authenticated
        if (User.Identity.IsAuthenticated)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user != null)
            {
                // Show only actual user data
                ViewBag.UserName = user.UserName;
                ViewBag.ProfilePicture = user.ProfilePicture; // Could be null
                ViewBag.Email = user.Email;
                ViewBag.PhoneNumber = user.PhoneNumber;
            }
        }

        return View(viewModel);
    }

    [Authorize]
    public async Task<IActionResult> Dashboard()
    {
        // Get real user data without defaults
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToAction("Login", "Account");
        }

        var currentUser = await _userManager.FindByEmailAsync(userEmail);
        if (currentUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var model = new DashboardViewModel
        {
            UserName = currentUser.UserName,

            InstructorCounter = await _instructorRepostory.GetInstructorCount(),
            CourseCounter = await _courseRepoistory.GetCourseount(),
            DepartmentCounter = await _DepartmentRepoistory.GetDepartmentCount(),
            TraineeCounter = await _TraineeRepoistory.GetTraineesCount(),
            CourseResultCounter = await _CourseResultRepostory.GetCourseResultCount(),
            UsersCounter = await _userManager.Users.CountAsync()
        };

        // Dynamic Data for Charts (real data only)
        ViewBag.CategoryData = new Dictionary<string, int>
    {
        { "Course Results", model.CourseResultCounter },
        { "Courses", model.CourseCounter },
        { "Departments", model.DepartmentCounter },
        { "Instructors", model.InstructorCounter },
        { "Trainees", model.TraineeCounter },
        { "Users", model.UsersCounter }
    };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
