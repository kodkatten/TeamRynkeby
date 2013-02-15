using System.Security;

namespace EventBooking.Controllers.ViewModels
{
    public class LoginModel
    {
        public string ElectronicMailAddress { get; set; }
        public SecureString Password { get; set; }
    }
}