using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceTracker.Helpers;
using ServiceTracker.Models;
using System.Text.Encodings.Web;
using ServiceTracker.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using GSS.Authentication.CAS;

namespace ServiceTracker.Controllers
{
   
    [AllowAnonymous]
    public class AccountController: Controller
    {
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        
        public async Task Login(string returnUrl)
        {            
            var props = new AuthenticationProperties { RedirectUri = returnUrl };
            await HttpContext.ChallengeAsync(CasDefaults.AuthenticationType, props);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }



    }
}