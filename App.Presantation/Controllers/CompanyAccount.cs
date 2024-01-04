﻿using App.Common.DataAccess.Context.Concrete;
using App.Common.Entities.DataModels;
using App.Common.Entities.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Presantation.Controllers
{
	public class CompanyAccount : Controller
	{
        private readonly SignInManager<tblUser> _signInManager;
        private readonly UserManager<tblUser> _userManager;
        private readonly IUserStore<tblUser> _userStore;
        private readonly IUserEmailStore<tblUser> _emailStore;
        private readonly AppContextDB _context;
        public CompanyAccount(
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
        public async Task<IActionResult> Index()
        {

            var values =  await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);
            var departmentNames = _context.CompanyDepartments
                            .Where(d => d.CompanyId == values.Id)
                            .Select(d => d.DepartmentName)
                            .ToList();
            var info = new VM_Request_CompanyRegister
            {
                CompanyName = companyInfo.CompanyName,
                CompanyMail=values.Email,
                DepartmentName = departmentNames,
            };

            return View(info);
        }

        public async Task<IActionResult> Chart()
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);

            var info = new VM_Request_CompanyRegister
            {
                CompanyName = companyInfo.CompanyName,
                CompanyMail = values.Email
            };

            return View(info);
        }
        public async Task<IActionResult> GeneralNotes()
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);

            var info = new VM_Request_CompanyRegister
            {
                CompanyName = companyInfo.CompanyName,
                CompanyMail = values.Email
            };

            return View(info);
        }
        public async Task<IActionResult> Calender()
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);

            var info = new VM_Request_CompanyRegister
            {
                CompanyName = companyInfo.CompanyName,
                CompanyMail = values.Email
            };

            return View(info);
        }

        [HttpGet]
        public async Task<IActionResult> DepartmenAdd()
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);

            var info = new VM_Request_CompanyRegister
            {
                CompanyName = companyInfo.CompanyName,
                CompanyMail = values.Email
            };

            return View(info);
        }

        [HttpPost]
        public async Task<IActionResult> DepartmentAdd(VM_Request_CompanyRegister cDepartment)
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);

            if (cDepartment.DepartmentName != null && cDepartment.DepartmentName.Any())
            {
                foreach (var departmentName in cDepartment.DepartmentName)
                {
                    var companyDep = new CompanyDepartment
                    {
                        DepartmentName = departmentName,
                        CompanyId = values.Id,
                    };

                    _context.CompanyDepartments.Add(companyDep);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "CompanyAccount");
        }

        [HttpGet]
        public async Task<IActionResult> CvAra()
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);
            

            var info = new VM_Request_CompanyRegister
            {
                CompanyName = companyInfo.CompanyName,
                CompanyMail = values.Email,
             
            };
           
            return View(info);
        }

        //[HttpPost]
        //public async Task<IActionResult> CvAra()
        //{
        //    var values = await _userManager.FindByEmailAsync(User.Identity.Name);
        //    var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);

        //    return View();
        //}
    }
}
