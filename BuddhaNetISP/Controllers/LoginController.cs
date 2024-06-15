using BuddhaNetISP.DTO;
using BuddhaNetISP.Interface;
using Humanizer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BuddhaNetISP.Controllers
{
    public class LoginController : Controller
    {
        private readonly IuserRepo _userRepo;
        public LoginController(IuserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                var response = _userRepo.GetUser(userDTO);

                if (response.IsSuccess)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,userDTO.Username)
                    };
                    var identity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principle = new ClaimsPrincipal(identity);
                    var props = new AuthenticationProperties();
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, props).Wait();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Authentication failed then it will give error 
                    UserDTO errorresp = new UserDTO();
                    errorresp.Errors = response.Message;
                    errorresp.Username = userDTO.Username;
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(errorresp);
                }
            }
            return View(userDTO);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return View("Index");
        }

    }
}

