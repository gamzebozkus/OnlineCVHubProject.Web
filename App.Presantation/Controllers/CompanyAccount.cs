using App.Common.DataAccess.Context.Concrete;
using App.Common.Entities.DataModels;
using App.Common.Entities.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public async Task<IActionResult> CvAra(string departmentName)
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);
            

            var info = new VM_Request_CompanyRegister
            {
                CompanyName = companyInfo.CompanyName,
                CompanyMail = values.Email,
                DepartmentName = new List<string> { departmentName },
            };
           
            return View(info);
        }
        [HttpPost]
        public async Task<IActionResult> CvAra(VM_Request_CompanyRegister model, string[] skills, string[] languages, string departmentName, string experienceLevel,string ozellik)
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);

            model.CompanyName = companyInfo.CompanyName;
            model.CompanyMail = values.Email;
            model.DepartmentName = new List<string> { departmentName };

            // skills ve languages değerlerini birleştir
            string aranacakSkills = string.Join(",", skills);
            string aranacakLanguages = string.Join(",", languages);
            string aranacakTags = $"{aranacakSkills},{aranacakLanguages},{experienceLevel},{ozellik}";

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://localhost:5000/tavsiye?aranacakTag={aranacakTags}&title={departmentName}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var sonuclar = JsonConvert.DeserializeObject<List<Recommendation>>(content);
                model.Sonuclar = sonuclar;
               
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult GetCvDetails(int cvId)
        {
            // cvId'ye göre ilgili CV detaylarını veritabanından alın
            var cvDetails = _context.UserCVs.FirstOrDefault(cv => cv.CVId == cvId);

            if (cvDetails == null)
            {
                return NotFound();
            }

            return PartialView("_GetCvDetails", cvDetails); // CV detaylarını bir kısmi görünüm olarak döndürün
        }


        [HttpGet]
        public async Task<IActionResult> CvAraSonuc()
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
    }
}
