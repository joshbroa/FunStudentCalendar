using FunStudentCalendar.Models;
using FunStudentCalendar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FunStudentCalendar.Controllers
{
    [Authorize]
    public class Calendar : Controller
    {
        private UserManager<ApplicationUser> _userManager;

        private CalendarApiService _CAS;

        public Calendar(CalendarApiService CAS, UserManager<ApplicationUser> userManager)
        {
            _CAS = CAS;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(long StartTime, long EndTime)
        {
            var user = await _userManager.GetUserAsync(User);
            var userEmail = user.Email;
            string jsonString = "";

            jsonString = _CAS.GetEvents(1727049600, 1727654400, userEmail);

            /*
            using (var scope = _serviceProvider.CreateScope())
            {
                // Resolve the credentials from configuration
                var credentials = _CS.GetCreds();

                // Create a new instance of CalendarApiService
                var calendarApiService = new CalendarApiService(credentials);

                // Use the calendarApiService instance as needed
                // var events = calendarApiService.GetEvents(...);
                jsonString = calendarApiService.GetEvents(1727049600, 1727654400, userEmail);
            }
            */

            ViewBag.JsonString = jsonString;
            return View();
        }
    }
}
