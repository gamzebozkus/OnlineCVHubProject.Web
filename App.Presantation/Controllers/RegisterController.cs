using App.Common.DataAccess.Context.Concrete;
using App.Common.Entities.DataModels;
using App.Common.Entities.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace App.Presantation.Controllers
{
	public class RegisterController : Controller
	{
		private readonly SignInManager<tblUser> _signInManager;
		private readonly UserManager<tblUser> _userManager;
		private readonly IUserStore<tblUser> _userStore;
		private readonly IUserEmailStore<tblUser> _emailStore;
        private readonly AppContextDB _context;
        //private readonly IEmailSender _emailSender;

        public RegisterController(
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
			_context= context;
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
		public async Task<IActionResult> Index(VM_Request_Register requestModel)
		{
			if (ModelState.IsValid)
			{

				tblUser user = CreateUser();

				var userInfo=new UserInfo();
                await _userStore.SetUserNameAsync(user, requestModel.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, requestModel.Email, CancellationToken.None);
                IdentityResult result = await _userManager.CreateAsync(user, requestModel.Password);

                userInfo.UserId = user.Id;
				userInfo.FirstName = requestModel.FirstName;
				userInfo.LastName = requestModel.LastName;

				_context.UserInfos.Add(userInfo);
				await _context.SaveChangesAsync();

				//user.FirstName = requestModel.FirstName;
				//user.LastName = requestModel.LastName;
				

				//var result = await _userManager.CreateAsync(user, requestModel.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("Index", "ConfirmMail");
				}

			}
			return View();

		}


		
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(VM_Request_Register loginModel)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(loginModel.Email);
				if (user != null)
				{
					var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false);
					if (result.Succeeded)
					{
						return RedirectToAction("Index", "MyAccount");
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
			else
			{
                TempData["ErrorMessage"] = "E-posta adresiniz veya şifreniz yanlıştır.";
                return PartialView("Login");
			}

		}
	}
}
