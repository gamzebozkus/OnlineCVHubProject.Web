using App.Common.Entities.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Presantation.Controllers
{
    [Authorize]
    public class MyAccountController : Controller
    {
        private readonly UserManager<tblUser> _userManager;

        public MyAccountController(UserManager<tblUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            tblUser user=new tblUser();   
            user.FirstName = values.FirstName;
            user.LastName = values.LastName;
            return View(user);
        }
    }
}
