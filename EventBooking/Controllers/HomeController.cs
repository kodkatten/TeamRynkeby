using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EventBooking.Data;
using WebMatrix.WebData;

namespace EventBooking.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                WebSecurity.CreateUserAndAccount(model.Email, model.Password, new {Created = DateTime.Now});
                WebSecurity.Login(model.Email, model.Password, model.RememberMe);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            return View();
        }
    }

    public class SignUpModel
    {
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Du måste ange en korrekt epostadress")]
        [Display(Name="Epostadress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [MinLength(6, ErrorMessage = "6 tecken eller fler")]
        [Display(Name="Lösenord")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Lösenorden stämmer inte med varandra.")]
        [Display(Name="Bekräfta lösenord")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name="Kom ihåg mi")]
        public bool RememberMe { get; set; }
    }

    public class ProfileModel
    {
        [Required]
        [Display(Name="Namn")]
        public string Name { get; set; }

        [Display(Name="Gatuadress")]
        public string StreetAddress { get; set; }
        
        [Display(Name="Postnummer")]
        public string ZipCode { get; set; }
        
        [Display(Name = "Ort")]
        public string City { get; set; }
        
        [Required]
        [Display(Name = "Mobiltelefon")]
        public string Cellphone { get; set; }
        
        [Display(Name = "Telefon")]
        public string Phone { get; set; }
        
        [Required]
        [Display(Name = "Epostadress")]
        public string Epost { get; set; }
        
        [Display(Name = "Födelsedatum")]
        public DateTime Birthdate { get; set; }
        
        [Required]
        [Display(Name = "Team")]
        public Team Team { get; set; }
    }
}