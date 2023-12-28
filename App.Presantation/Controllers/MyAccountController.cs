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

        [HttpGet]
        public async Task<IActionResult> CvEdit()
        {   
        
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);

            var userInfo = _context.UserInfos.FirstOrDefault(i => i.UserId == values.Id); 
            var emptyModel = new VM_CvAdd
            {
                EducationInfos = new List<EducationInfo>(), // Boş bir eğitim listesi ekleyin veya varsayılan değerler atayın
                Experiences = new List<Experience>(), // Boş bir eğitim listesi ekleyin veya varsayılan değerler atayın
                Skills = new List<Skill>(), // Boş bir eğitim listesi ekleyin veya varsayılan değerler atayın
                Hobbies = new List<Hobby>(), // Boş bir eğitim listesi ekleyin veya varsayılan değerler atayın
                                             // Diğer alanları da varsayılan değerlerle doldurabilirsiniz
            };
            ViewData["FirstName"] = userInfo.FirstName;
            ViewData["LastName"] = userInfo.LastName;

        
            

            return View(emptyModel);
         
        }

       

        [HttpPost]
        public async Task<IActionResult> CvEdit(VM_CvAdd cvAdd)
        {
          
            
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var userInfo = _context.UserInfos.FirstOrDefault(i => i.UserId == values.Id);
            
            var userCv = new UserCv
            {
                CvNameSurname=cvAdd.CvNameSurname,
                Title = cvAdd.Title,
                Summary = cvAdd.Summary,
                CreationDate = cvAdd.CreationDate,
                Address = cvAdd.Address,
                tblUser = cvAdd.tblUser,
                UserId=values.Id,
               //EducationInfos=cvAdd.EducationInfos,              
            };

            _context.UserCVs.Add(userCv);
            await _context.SaveChangesAsync();

            foreach (var educationInfo in cvAdd.EducationInfos)
            {
                var edu = new EducationInfo
                {
                    CVId = userCv.CVId,
                    Major = educationInfo.Major,
                    School = educationInfo.School,
                    Degree = educationInfo.Degree,
                    StartDate = educationInfo.StartDate,
                    EndDate = educationInfo.EndDate,
                };

                _context.EducationInfos.Add(edu);
                await _context.SaveChangesAsync();
            }

           
            foreach (var experience in cvAdd.Experiences)
            {

                var exp = new Experience
                {
                    CVId = userCv.CVId,
                    Company = experience.Company,
                    Position = experience.Position,
                    Responsibilities= experience.Responsibilities,
                    StartDate =experience.StartDate,
                    EndDate = experience.EndDate,

                };
                _context.Experiences.Add(exp); 
                await _context.SaveChangesAsync();
            }
           
            foreach (var skills in cvAdd.Skills)
            {

                var skill = new Skill
                {
                    CVId = userCv.CVId,
                    SkillName = skills.SkillName,
                    Level = skills.Level,
                };

                _context.Skills.Add(skill);
                await _context.SaveChangesAsync();
            }
           
            foreach (var hobbies in cvAdd.Hobbies)
            {

                var hobby = new Hobby
                {
                    CVId = userCv.CVId,
                    HobbyName = hobbies.HobbyName,

                };

                _context.Hobbies.Add(hobby);
                await _context.SaveChangesAsync();
            }

            int newCvId = userCv.CVId;

            // Template1 eylemine yönlendirme
            return RedirectToAction("Template1", "MyAccount", new { cvId = newCvId });
        }

        public async Task<IActionResult> Cvs()
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);

            var userInfo = _context.UserInfos.FirstOrDefault(i => i.UserId == values.Id);
           
            ViewData["FirstName"] = userInfo.FirstName;
            ViewData["LastName"] = userInfo.LastName;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Template1(int cvId)
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);

            var userInfo = _context.UserInfos.FirstOrDefault(i => i.UserId == values.Id);
            var cv = await _context.UserCVs.FirstOrDefaultAsync(c => c.CVId == cvId);
            var info = new VM_CvAdd
            {
                CvNameSurname = cv.CvNameSurname,
                Title = cv.Title,
                Summary = cv.Summary,
                CreationDate = cv.CreationDate,
                Address = cv.Address,
                tblUser = cv.tblUser,


            };
            return View(info);
        }
    }
}
