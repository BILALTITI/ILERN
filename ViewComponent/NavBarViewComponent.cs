using Assigment2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class NavBarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var userImage = User.Identity.IsAuthenticated
            ? UserClaimsPrincipal.Claims.FirstOrDefault(c => c.Type == "ProfilePicture")?.Value
            : "/images/default-user.png";

        var model = new ProfilePictureViewModel
        {
            ProfilePictureUrl = userImage
        };

        return View(model); // Ensure the correct view is used
    }
}
