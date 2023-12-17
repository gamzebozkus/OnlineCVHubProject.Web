using App.Common.DataAccess.Context.Concrete;
using App.Common.Entities.DataModels;
using App.Common.Entities.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Presantation.Controllers
{
    public class CompanyRegister : Controller
    {
        private readonly SignInManager<tblUser> _signInManager;
        private readonly UserManager<tblUser> _userManager;
        private readonly IUserStore<tblUser> _userStore;
        private readonly IUserEmailStore<tblUser> _emailStore;
        private readonly AppContextDB _context;
        //private readonly IEmailSender _emailSender;

        public CompanyRegister(
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(VM_Request_CompanyRegister requestModel)
        {
            tblUser user = CreateUser();
            var companyInfo = new CompanyInfo();
            await _userStore.SetUserNameAsync(user, requestModel.CompanyMail, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, requestModel.CompanyMail, CancellationToken.None);
            IdentityResult result = await _userManager.CreateAsync(user, requestModel.CompanyPassword);

            companyInfo.CompanyId = user.Id;
            companyInfo.CompanyName = requestModel.CompanyName;

            _context.CompanyInfos.Add(companyInfo);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VM_Request_CompanyRegister loginModel)
        {
            
                var user = await _userManager.FindByEmailAsync(loginModel.CompanyMail);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginModel.CompanyPassword, loginModel.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "CompanyAccount");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "E-posta adresiniz veya şifreniz yanlıştır.";
                        return PartialView("Login");
                    }

                }
                else
                {
                    TempData["ErrorMessage"] = "E-posta adresiniz veya şifreniz yanlıştır.";
                    return PartialView("Login");
                }
            
           
            

        }
    }
}


