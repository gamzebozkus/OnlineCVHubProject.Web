using App.Common.DataAccess.Context.Concrete;
using App.Common.Entities.DataModels;
using App.Common.Entities.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
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
                Languages = new List<Language>(), // Boş bir eğitim listesi ekleyin veya varsayılan değerler atayın
                                             // Diğer alanları da varsayılan değerlerle doldurabilirsiniz
            };
            ViewData["FirstName"] = userInfo.FirstName;
            ViewData["LastName"] = userInfo.LastName;

        
            

            return View(emptyModel);
         
        }

       

        [HttpPost]
        public async Task<IActionResult> CvEdit(VM_CvAdd cvAdd,bool state, IFormFile formFile, string personalityTraits)
        {
          
            
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var userInfo = _context.UserInfos.FirstOrDefault(i => i.UserId == values.Id);
            string randomName = string.Empty;
            if (formFile != null)
            {
                var extent = Path.GetExtension(formFile.FileName);
                randomName = ($"{Guid.NewGuid()}{extent}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }
            //deneyim zamanı hesaplama
            int totalExperienceMonths = cvAdd.Experiences
     .Where(e => e.StartDate.HasValue && e.EndDate.HasValue) // Null kontrolü
     .Sum(e =>
     {
         var startDate = e.StartDate.Value;
         var endDate = e.EndDate.Value;
         return ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month;
     });

            // Toplam süreye göre etiket oluştur
            string experienceLevel = totalExperienceMonths switch
            {
                < 12 => "0-1",
                < 24 => "1-2",
                < 36 => "2-3",
                _ => "4+"
            };
            var userCv = new UserCv
            {
                CvNameSurname=cvAdd.CvNameSurname,
                Email=cvAdd.Email,
                PhoneNum=cvAdd.PhoneNum,
                Title = cvAdd.Title,
                Summary = cvAdd.Summary,
                CreationDate = cvAdd.CreationDate,
                Address = cvAdd.Address,
                PersonalityTraits = personalityTraits,
                
               
                Image = formFile != null ? "/img/" + randomName : null, // Dosya varsa yolu ata, yoksa null olarak bırak
                Tags = String.Join(", ", new List<string>
                {
                cvAdd.Title,
                String.Join(", ", cvAdd.Skills.Select(s => s.SkillName)),
                String.Join(", ", cvAdd.Languages.Select(s => s.LanguageName)),
                String.Join(", ", cvAdd.EducationInfos.SelectMany(e => new string[] { e.Major, e.School })),
                personalityTraits,
                experienceLevel,
                }),
                
                UserId =values.Id,
               
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
            foreach (var languages in cvAdd.Languages)
            {

                var language= new Language
                {
                    CVId = userCv.CVId,
                    LanguageName = languages.LanguageName,
                    LanguageLevel=languages.LanguageLevel,

                };

                _context.Languages.Add(language);
                await _context.SaveChangesAsync();
            }

           int newCvId = userCv.CVId;
            if (state)
            {
                // Eğer state true ise, CvPool tablosuna ekleyelim
                var cvPool = new CvPool
                {
                    CVId = userCv.CVId // CV'nin ID'si CvPool tablosuna ekleniyor
                };

                _context.CvPools.Add(cvPool);
                await _context.SaveChangesAsync();
            }
 
            // Template1 eylemine yönlendirme
            return RedirectToAction("CVTemplates", "MyAccount", new { cvId = newCvId });
        }

        public async Task<IActionResult> Cvs()
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);

            var userInfo = _context.UserInfos.FirstOrDefault(i => i.UserId == values.Id);
           
            ViewData["FirstName"] = userInfo.FirstName;
            ViewData["LastName"] = userInfo.LastName;
            return View();
        }


        //CV şablonlarını seçmesini sağlayan controller
        [HttpGet]
        public async Task<IActionResult> CVTemplates(int cvId)
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);

            var userInfo = _context.UserInfos.FirstOrDefault(i => i.UserId == values.Id);
          
             var userCv = new UserCv();
            //int newCvId = userCv.CVId;
            //ViewBag.cvId = newCvId;

            ViewBag.cvId = cvId;
            return View();

        }


        [HttpGet]
        public async Task<IActionResult> Template1(int cvId)
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);

            var userInfo = _context.UserInfos.FirstOrDefault(i => i.UserId == values.Id);
            var cv = await _context.UserCVs.FirstOrDefaultAsync(c => c.CVId == cvId);
            var experiences = await _context.Experiences.Where(e => e.CVId == cvId).ToListAsync();
            var educations = await _context.EducationInfos.Where(e => e.CVId == cvId).ToListAsync();
            var skill = await _context.Skills.Where(e => e.CVId == cvId).ToListAsync();
            var hobi = await _context.Hobbies.Where(e => e.CVId == cvId).ToListAsync();
            
            var info = new VM_CvAdd
            {
                CvNameSurname = cv.CvNameSurname,
                Title = cv.Title,
                Summary = cv.Summary,
                CreationDate = cv.CreationDate,
                Address = cv.Address,
                Email=cv.Email,
                PhoneNum=cv.PhoneNum,
                tblUser = cv.tblUser,
                Experiences=experiences,
                EducationInfos=educations,
                Skills=skill,
                Hobbies=hobi,

            };


           
            return View(info);
        }
        [HttpGet]
        public async Task<IActionResult> Template2(int cvId)
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);

            var userInfo = _context.UserInfos.FirstOrDefault(i => i.UserId == values.Id);
            var cv = await _context.UserCVs.FirstOrDefaultAsync(c => c.CVId == cvId);
            var experiences = await _context.Experiences.Where(e => e.CVId == cvId).ToListAsync();
            var educations = await _context.EducationInfos.Where(e => e.CVId == cvId).ToListAsync();
            var skill = await _context.Skills.Where(e => e.CVId == cvId).ToListAsync();
            var language = await _context.Languages.Where(e => e.CVId == cvId).ToListAsync();
            var hobi = await _context.Hobbies.Where(e => e.CVId == cvId).ToListAsync();

            var info = new VM_CvAdd
            {
                CvNameSurname = cv.CvNameSurname,
                Title = cv.Title,
                Summary = cv.Summary,
                Image=cv.Image,
                CreationDate = cv.CreationDate,
                Address = cv.Address,
                Email = cv.Email,
                PhoneNum = cv.PhoneNum,
                tblUser = cv.tblUser,
                Experiences = experiences,
                EducationInfos = educations,
                Languages=language,
                Skills = skill,
                Hobbies = hobi,

            };



            return View(info);
        }

        [HttpGet]
        public async Task<IActionResult> Template3(int cvId)
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);

            var userInfo = _context.UserInfos.FirstOrDefault(i => i.UserId == values.Id);
            var cv = await _context.UserCVs.FirstOrDefaultAsync(c => c.CVId == cvId);
            var experiences = await _context.Experiences.Where(e => e.CVId == cvId).ToListAsync();
            var educations = await _context.EducationInfos.Where(e => e.CVId == cvId).ToListAsync();
            var language = await _context.Languages.Where(e => e.CVId == cvId).ToListAsync();
            var skill = await _context.Skills.Where(e => e.CVId == cvId).ToListAsync();
            var hobi = await _context.Hobbies.Where(e => e.CVId == cvId).ToListAsync();

            var info = new VM_CvAdd
            {
                CvNameSurname = cv.CvNameSurname,
                Title = cv.Title,
                Summary = cv.Summary,
                Image = cv.Image,
                CreationDate = cv.CreationDate,
                Address = cv.Address,
                Email = cv.Email,
                PhoneNum = cv.PhoneNum,
                tblUser = cv.tblUser,
                Experiences = experiences,
                EducationInfos = educations,
                Languages = language,
                Skills = skill,
                Hobbies = hobi,

            };



            return View(info);
        }


    }
}
