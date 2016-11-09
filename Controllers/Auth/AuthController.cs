
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;


namespace TheWorld.Controllers
{
    using Models;
    using System.Threading.Tasks;
    using ViewModels;

    public class AuthController : Controller
    {
        private SignInManager<WorldUser> _signInManager;

        public AuthController(SignInManager<WorldUser> signInManager)
        {
            _signInManager = signInManager;
        }


        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Trips", "App");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);

                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Trips, App");
                    }

                    return Redirect(returnUrl);
                }

                ModelState.AddModelError("", "Username or password incorrect");
            }

            return View();
        }


        public async Task<IActionResult> Logout(LoginViewModel vm, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Removes cookie collection and signs out user.
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "App");
        }


    }
}
