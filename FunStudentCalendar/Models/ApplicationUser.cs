using Microsoft.AspNetCore.Identity;

namespace FunStudentCalendar.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Points { get; set; }
        public long LastLoginUnix { get; set; }
    }
}
