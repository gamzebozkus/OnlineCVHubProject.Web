using App.Common.DataAccess.Context.Concrete;
using App.Common.Entities.DataModels;
using App.Common.Entities.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.Design;
using System.Globalization;

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

            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);
            var departmentNames = _context.CompanyDepartments
                            .Where(d => d.CompanyId == values.Id)
                            .Select(d => d.DepartmentName)
                            .ToList();
            var info = new VM_Request_CompanyRegister
            {
                CompanyName = companyInfo.CompanyName,
                CompanyMail = values.Email,
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
                CompanyMail = values.Email,

            };
            // Veritabanından verileri sorgula (Bu kısım projenize göre değişiklik gösterecektir)
            var gorusmeVerileri = _context.CompanySavedCvs
    .Where(c => c.GorusmeTarihi != null) // Görüşme tarihi null olmayan kayıtlar
    .Select(c => new
    {
        Tarih = c.GorusmeTarihi.Value,
        Durum = c.Gorusme.HasValue && c.Gorusme.Value // Görüşme durumu (true/false)
    })
    .ToList();


            // Verileri işle
            var aylikGorusmeler = gorusmeVerileri.GroupBy(g => new { g.Tarih.Year, g.Tarih.Month })
                                                .Select(group => new
                                                {
                                                    Yil = group.Key.Year,
                                                    Ay = group.Key.Month,
                                                    Olumlu = group.Count(g => g.Durum == true),
                                                    Olumsuz = group.Count(g => g.Durum == false)
                                                })
                                                .ToList();

            // Verileri view'a aktar
            ViewBag.AylikGorusmeler = aylikGorusmeler;

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
            var companyInfo = await _context.CompanyInfos.FirstOrDefaultAsync(i => i.CompanyId == values.Id);

            // Toplantı verilerini çekiyoruz ve MeetingViewModel'ine dolduruyoruz
            var meetingViewModels = _context.CompanySavedCvs.Select(cv => new VM_Meeting
            {
                MeetingSubject = cv.MeetingSubject,
                MeetingDate = cv.MeetingDate,
                MeetingTime = cv.MeetingTime
            }).ToList();

            var info = new VM_Request_CompanyRegister
            {
                CompanyName = companyInfo?.CompanyName,
                CompanyMail = values.Email,
                Meetings = meetingViewModels // Toplantı verilerini VM_Request_CompanyRegister içinde kullanmak için atıyoruz
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
        public async Task<IActionResult> CvAra(VM_Request_CompanyRegister model, string[] skills, string[] languages, string departmentName, string experienceLevel, string phoneNum, string[] ozellik)
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);

            model.CompanyName = companyInfo.CompanyName;
            model.CompanyMail = values.Email;
            model.DepartmentName = new List<string> { departmentName };

            var companyDepartment = await _context.CompanyDepartments.FirstOrDefaultAsync(d => d.DepartmentName == departmentName);
            int departmentId = companyDepartment?.Id ?? 0;

            // skills ve languages değerlerini birleştir
            string aranacakSkills = string.Join(",", skills);
            string aranacakLanguages = string.Join(",", languages);

            // Seçilen özellikleri birleştir
            string aranacakOzellik = string.Join(",", ozellik);

            string aranacakTags = $"{aranacakSkills},{aranacakLanguages},{experienceLevel},{aranacakOzellik}";

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://localhost:5000/tavsiye?aranacakTag={aranacakTags}&title={departmentName}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var sonuclar = JsonConvert.DeserializeObject<List<Recommendation>>(content);
                model.Sonuclar = sonuclar.Where(s => s.Similarity > 0).ToList();
            }
            TempData["DepartmanId"] = departmentId;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCvDetails(int cvId)
        {
            // cvId'ye göre ilgili CV detaylarını veritabanından alın
            var cv = await _context.UserCVs.FirstOrDefaultAsync(c => c.CVId == cvId);
            var experiences = await _context.Experiences.Where(e => e.CVId == cvId).ToListAsync();
            var educations = await _context.EducationInfos.Where(e => e.CVId == cvId).ToListAsync();
            var skill = await _context.Skills.Where(e => e.CVId == cvId).ToListAsync();
            var hobi = await _context.Hobbies.Where(e => e.CVId == cvId).ToListAsync();
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);
            var info = new VM_CvAdd
            {
                CvNameSurname = cv.CvNameSurname,
                Title = cv.Title,
                Summary = cv.Summary,
                CreationDate = cv.CreationDate,
                Address = cv.Address,
                Email = cv.Email,
                PhoneNum = cv.PhoneNum,
                tblUser = cv.tblUser,
                Experiences = experiences,
                EducationInfos = educations,
                Skills = skill,
                Hobbies = hobi,


            };
            TempData["CvId"] = cvId;
            TempData["CompanyId"] = companyInfo.CompanyId;

            //if (cvDetails == null)
            //{
            //    return NotFound();
            //}

            return PartialView("_GetCvDetails", info); // CV detaylarını bir kısmi görünüm olarak döndürün
        }
        [HttpGet]
        public async Task<IActionResult> CvDetails(int cvId)
        {
            // cvId'ye göre ilgili CV detaylarını veritabanından alın
            var cv = await _context.UserCVs.FirstOrDefaultAsync(c => c.CVId == cvId);
            var experiences = await _context.Experiences.Where(e => e.CVId == cvId).ToListAsync();
            var educations = await _context.EducationInfos.Where(e => e.CVId == cvId).ToListAsync();
            var skill = await _context.Skills.Where(e => e.CVId == cvId).ToListAsync();
            var hobi = await _context.Hobbies.Where(e => e.CVId == cvId).ToListAsync();
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);
            var info = new VM_CvAdd
            {
                CvNameSurname = cv.CvNameSurname,
                Title = cv.Title,
                Summary = cv.Summary,
                CreationDate = cv.CreationDate,
                Address = cv.Address,
                Email = cv.Email,
                PhoneNum = cv.PhoneNum,
                tblUser = cv.tblUser,
                Experiences = experiences,
                EducationInfos = educations,
                Skills = skill,
                Hobbies = hobi,


            };
            TempData["CvId"] = cvId;
            TempData["CompanyId"] = companyInfo.CompanyId;

            //if (cvDetails == null)
            //{
            //    return NotFound();
            //}

            return View(info); // CV detaylarını bir kısmi görünüm olarak döndürün
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
        [HttpGet]
        public async Task<IActionResult> DeleteDepartment(string departmentName)
        {
            var department = _context.CompanyDepartments.FirstOrDefault(d => d.DepartmentName == departmentName);
            if (department != null)
            {
                _context.CompanyDepartments.Remove(department);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "CompanyAccount");
        }

        [HttpPost]
        public IActionResult Kaydet(int CvId, string CompanyId, int DepartmentId)
        {
            var existingCv = _context.CompanySavedCvs.FirstOrDefault(c => c.CvId == CvId);
            if (existingCv == null)
            {
                var companySavedCv = new CompanySavedCv
                {
                    CvId = CvId,
                    CompanyId = CompanyId,
                    DepartmentId = DepartmentId,
                    GorusmeTarihi = DateTime.Now,

                };

                _context.CompanySavedCvs.Add(companySavedCv);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "CV basariyla kaydedildi.";
            }
            else
            {
                TempData["WarningMessage"] = "Bu CV mevcut.Tekrardan kaydedilemez.";
            }

            // Aynı sayfayı tekrar yükle
            return RedirectToAction("GetCvDetails", new { cvId = CvId }); // 'model' sayfanın modelidir.
        }


        [HttpGet]
        public async Task<IActionResult> KaydedilenCvs(string departmentName)
        {
            var user = await _userManager.GetUserAsync(User);
            var companyInfo = await _context.CompanyInfos.FirstOrDefaultAsync(i => i.CompanyId == user.Id);

            var department = await _context.CompanyDepartments.FirstOrDefaultAsync(d => d.DepartmentName == departmentName);
            if (department == null)
            {
                return NotFound();
            }

            var savedCvs = await _context.CompanySavedCvs
                                 .Where(c => c.CompanyId == companyInfo.CompanyId && c.DepartmentId == department.Id)
                                 .OrderByDescending(c => c.Id) // En son kaydedilen CV'leri en başta getir
                                 .ToListAsync();

            var sonuclar = new List<Recommendation>();
            foreach (var savedCv in savedCvs)
            {
                var cv = await _context.UserCVs.FirstOrDefaultAsync(c => c.CVId == savedCv.CvId);
                if (cv != null)
                {
                    sonuclar.Add(new Recommendation
                    {
                        CvId = cv.CVId.ToString(),
                        CvNameSurname = cv.CvNameSurname,
                        Title = cv.Title,
                        Image = cv.Image,
                        PhoneNum = cv.PhoneNum,
                        Email = cv.Email,
                        DepartmentId = department.Id,

                    });
                }
            }

            var info = new VM_Request_CompanyRegister
            {
                CompanyName = companyInfo.CompanyName,
                CompanyMail = user.Email,
                Sonuclar = sonuclar
            };

            return View(info);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCvState(string cvId, bool state, int departmentId)
        {
            if (string.IsNullOrEmpty(cvId))
            {
                return BadRequest("CV ID boş olamaz.");
            }

            // İlgili CV'yi bulun
            var cvSaved = await _context.CompanySavedCvs.FirstOrDefaultAsync(c => c.CvId.ToString() == cvId);
            if (cvSaved == null)
            {
                return NotFound("Belirtilen CV bulunamadı.");
            }

            // Durumu güncelleyin
            cvSaved.Gorusme = state;

            // Veritabanında değişiklikleri kaydedin
            await _context.SaveChangesAsync();

            // DepartmanId'yi DepartmanName'e çevirin
            //var department = await _context.CompanySavedCvs.FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
            var qq = await _context.CompanyDepartments.FirstOrDefaultAsync(a => a.Id == departmentId);
            var departmentName = qq.DepartmentName;                         // kullanabilirsiniz

            // İlgili departmana yönlendirin
            return RedirectToAction("KaydedilenCvs", new { departmentName });
        }



        [HttpGet]
        public async Task<IActionResult> SavedDepartments()
        {
            var savedCvs = await _context.CompanySavedCvs.ToListAsync();
            var uniqueDepartments = new HashSet<string>();

            foreach (var savedCv in savedCvs)
            {
                var department = await _context.CompanyDepartments.FirstOrDefaultAsync(d => d.Id == savedCv.DepartmentId);
                if (department != null)
                {
                    uniqueDepartments.Add(department.DepartmentName);
                }
            }

            var viewModel = new VM_Request_CompanyRegister
            {
                UniqueDepartments = uniqueDepartments.ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> GorusmeGrafik()
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            var companyInfo = _context.CompanyInfos.FirstOrDefault(i => i.CompanyId == values.Id);


            var info = new VM_Request_CompanyRegister
            {
                CompanyName = companyInfo.CompanyName,
                CompanyMail = values.Email,

            };
            // Veritabanından verileri sorgula (Bu kısım projenize göre değişiklik gösterecektir)
            var gorusmeVerileri = _context.CompanySavedCvs
    .Where(c => c.GorusmeTarihi != null) // Görüşme tarihi null olmayan kayıtlar
    .Select(c => new
    {
        Tarih = c.GorusmeTarihi.Value,
        Durum = c.Gorusme.HasValue && c.Gorusme.Value // Görüşme durumu (true/false)
    })
    .ToList();


            // Verileri işle
            var aylikGorusmeler = gorusmeVerileri.GroupBy(g => new { g.Tarih.Year, g.Tarih.Month })
                                                .Select(group => new
                                                {
                                                    Yil = group.Key.Year,
                                                    Ay = group.Key.Month,
                                                    Olumlu = group.Count(g => g.Durum == true),
                                                    Olumsuz = group.Count(g => g.Durum == false)
                                                })
                                                .ToList();

            // Verileri view'a aktar
            ViewBag.AylikGorusmeler = aylikGorusmeler;

            return View(info);
        }
        [HttpPost]
        public ActionResult AddNote(int cvId, string note)
        {
            // Veritabanı bağlantınızı ve context'inizi burada kullanın
            // Örneğin, DbContext'iniz 'dbContext' adında olsun
            var cv = _context.CompanySavedCvs.FirstOrDefault(c => c.CvId == cvId);
            if (cv != null)
            {
                cv.Notes = note; // Not alanını güncelle
                _context.SaveChanges(); // Değişiklikleri kaydet
                return Json(new { success = true, message = "Not başarıyla eklendi." });
            }
            else
            {
                return Json(new { success = false, message = "CV bulunamadı." });
            }
        }

        [HttpPost]
        public ActionResult AddMeetingDate(int cvId, DateTime meetingDate, TimeSpan meetingTime, string meetingSubject)
        {
            var existingMeeting = _context.CompanySavedCvs.FirstOrDefault(c => c.MeetingDate == meetingDate.Date && c.MeetingTime == meetingTime && c.CvId == cvId);

            if (existingMeeting != null)
            {
                return Json(new { success = false, message = "Aynı tarih ve saatte zaten bir toplantı var." });
            }

            var cv = _context.CompanySavedCvs.FirstOrDefault(c=>c.CvId==cvId);
            if (cv != null)
            {
                cv.MeetingDate = meetingDate;
                cv.MeetingTime = meetingTime;
                cv.MeetingSubject = meetingSubject;
                _context.SaveChanges();
                return Json(new { success = true, message = "Toplantı başarıyla eklendi." });
            }
            else
            {
                return Json(new { success = false, message = "CV bulunamadı." });
            }
        }
        [HttpGet]
        public JsonResult GetMeetings()
        {
            var meetings = _context.CompanySavedCvs
                .Where(cv => cv.MeetingDate != null) // Sadece toplantı tarihi olan kayıtları seç
                .Select(cv => new
                {
                    id = cv.CvId, // Her toplantının benzersiz bir ID'si olmalı
                    title = cv.MeetingSubject,
                    start = cv.MeetingDate.Value.ToString("yyyy-MM-dd") + "T" + (cv.MeetingTime != null ? cv.MeetingTime.Value.ToString(@"hh\:mm\:ss") : ""),
                    allDay = false // Bu örnekte tüm gün süren etkinlikler yok
                })
                .ToList();

            return Json(meetings);
        }


    }
}
