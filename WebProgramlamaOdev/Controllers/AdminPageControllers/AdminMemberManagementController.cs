using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.DTOs;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Controllers.AdminPageControllers
{
    [Authorize(Roles = "Gym")]
    public class AdminMemberManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminMemberManagementController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // ============================================
        // SHOW MEMBERS - Üyeleri Listele
        // ============================================
        public async Task<IActionResult> ShowMembers()
        {
            try
            {
                var members = await _unitOfWork.Members.GetAllAsync();
                
                var memberViewModels = members.Select(m => new MemberListViewModel
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    FirstName = m.User?.FirstName ?? string.Empty,
                    LastName = m.User?.LastName ?? string.Empty,
                    Email = m.User?.Email ?? "Belirtilmemiş",
                    PhoneNumber = m.User?.PhoneNumber ?? "Belirtilmemiş",
                    CreatedDate = m.User?.CreatedDate ?? DateTime.Now,
                    DateOfBirth = m.DateOfBirth,
                    Gender = m.Gender ?? "Belirtilmemiş",
                    Height = m.Height,
                    Weight = m.Weight,
                    AppointmentCount = m.Appointments?.Count ?? 0,
                    AIRecommendationCount = m.AIRecommendations?.Count ?? 0
                }).ToList();

                return View(memberViewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Üyeler yüklenirken hata: " + ex.Message;
                return View(new List<MemberListViewModel>());
            }
        }

        // ============================================
        // CREATE MEMBER (GET)
        // ============================================
        public IActionResult CreateMember()
        {
            return View();
        }

        
        // ============================================
        // EDIT MEMBER (GET)
        // ============================================
        [HttpGet]
        public async Task<IActionResult> EditMember(int id)
        {
            try
            {
                var member = await _unitOfWork.Members.GetByIdAsync(id);
                
                if (member == null)
                {
                    TempData["ErrorMessage"] = "Üye bulunamadı.";
                    return RedirectToAction(nameof(ShowMembers));
                }

                var dto = new UpdateMemberRequestDto
                {
                    Id = member.Id,
                    UserId = member.UserId,
                    FirstName = member.User?.FirstName ?? string.Empty,
                    LastName = member.User?.LastName ?? string.Empty,
                    Email = member.User?.Email ?? string.Empty,
                    PhoneNumber = member.User?.PhoneNumber ?? string.Empty,
                    DateOfBirth = member.DateOfBirth ?? DateTime.Now,
                    Gender = member.Gender ?? "Male",
                    Height = member.Height ?? 170,
                    Weight = member.Weight ?? 70
                };
                
                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Hata: " + ex.Message;
                return RedirectToAction(nameof(ShowMembers));
            }
        }

        // ============================================
        // EDIT MEMBER (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMember(UpdateMemberRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var member = await _unitOfWork.Members.GetByIdAsync(model.Id);
                
                if (member == null)
                {
                    TempData["ErrorMessage"] = "Üye bulunamadı.";
                    return RedirectToAction(nameof(ShowMembers));
                }

                // Member güncelle
                member.DateOfBirth = model.DateOfBirth;
                member.Gender = model.Gender;
                member.Height = model.Height;
                member.Weight = model.Weight;

                // User güncelle
                if (member.User != null)
                {
                    member.User.FirstName = model.FirstName;
                    member.User.LastName = model.LastName;
                    member.User.Email = model.Email;
                    member.User.UserName = model.Email;
                    member.User.PhoneNumber = model.PhoneNumber;

                    var updateResult = await _userManager.UpdateAsync(member.User);
                    
                    if (!updateResult.Succeeded)
                    {
                        foreach (var error in updateResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }

                await _unitOfWork.Members.UpdateAsync(member);
                await _unitOfWork.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"✅ {model.FirstName} {model.LastName} güncellendi!";
                return RedirectToAction(nameof(ShowMembers));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "❌ Hata: " + ex.Message +
                    (ex.InnerException != null ? " | " + ex.InnerException.Message : "");
                return View(model);
            }
        }

        // ============================================
        // DELETE MEMBER (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                var member = await _unitOfWork.Members.GetByIdAsync(id);
                
                if (member == null)
                {
                    TempData["ErrorMessage"] = "❌ Üye bulunamadı.";
                    return RedirectToAction(nameof(ShowMembers));
                }

                string memberName = $"{member.User?.FirstName} {member.User?.LastName}";
                string userId = member.UserId;

                // ============================================
                // ADIM 1: Appointments kayıtlarını SİL/GÜNCELLE
                // ============================================
                if (member.Appointments != null && member.Appointments.Any())
                {
                    int appointmentCount = member.Appointments.Count;
                    
                    // SEÇENEK A: Tamamen sil
                    foreach (var appointment in member.Appointments.ToList())
                    {
                        await _unitOfWork.Appointments.DeleteAsync(appointment);
                    }
                    
                    await _unitOfWork.SaveChangesAsync();
                    
                    Console.WriteLine($"✅ {appointmentCount} Appointment silindi.");
                }

                // ============================================
                // ADIM 2: AIRecommendations kayıtlarını SİL
                // ============================================
                if (member.AIRecommendations != null && member.AIRecommendations.Any())
                {
                    int aiCount = member.AIRecommendations.Count;
                    
                    foreach (var aiRec in member.AIRecommendations.ToList())
                    {
                        // AIRecommendation silme için UnitOfWork'e repository eklenmeli
                        // Geçici olarak context üzerinden silebilirsin
                    }
                    
                    await _unitOfWork.SaveChangesAsync();
                }

                // ============================================
                // ADIM 3: Member'ı SİL
                // ============================================
                await _unitOfWork.Members.DeleteAsync(member);
                await _unitOfWork.SaveChangesAsync();

                // ============================================
                // ADIM 4: User'ı SİL
                // ============================================
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var deleteUserResult = await _userManager.DeleteAsync(user);
                    
                    if (!deleteUserResult.Succeeded)
                    {
                        TempData["ErrorMessage"] = $"⚠️ {memberName} silindi ama kullanıcı hesabı silinemedi.";
                        return RedirectToAction(nameof(ShowMembers));
                    }
                }
                
                TempData["SuccessMessage"] = $"✅ {memberName} başarıyla silindi!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"❌ Hata: {ex.Message}" +
                    (ex.InnerException != null ? $" | {ex.InnerException.Message}" : "");
            }

            return RedirectToAction(nameof(ShowMembers));
        }
    }
}
