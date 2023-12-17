using App.Common.DataAccess.Context.Concrete;
using App.Common.Entities.DataModels;
using App.Common.Entities.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace App.Presantation.Controllers
{
    [Authorize]
    public class MyAccountController : Controller
    {
        private readonly SignInManager<tblUser> _signInManager;
        private readonly UserManager<tblUser> _userManager;
        private readonly IUserStore<tblUser> _userStore;
        private readonly IUserEmailStore<tblUser> _emailStore;
        private readonly AppContextDB _context;
        public MyAccountController(
          UserManager<tblUser> userManager,
          SignInManager<tblUser> signInManager,
            IUserStore<tblUser> userStore,
            AppContextDB context
          //IEmailSender emailSender
          )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
            //_emailSender = emailSender;
            _emailStore = GetEmailStore();
            _context = context;
        }

        private tblUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<tblUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(tblUser)}'. " +
                    $"Ensure that '{nameof(tblUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<tblUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<tblUser>)_userStore;
        }
       
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
          
            var userInfo=_context.UserInfos.FirstOrDefault(i=>i.UserId==values.Id);

            var info = new VM_Request_Register
            {
                FirstName=userInfo.FirstName,
                LastName=userInfo.LastName
            };
          
            return View(info);
        }
        
        public async Task<IActionResult> CvEditAsync()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            tblUser user = new tblUser();
            //user.FirstName = values.FirstName;
            //user.LastName = values.LastName;
            return View(user);
        }
    }
}
