using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Web.Models;
using Services.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Mail;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ISecurityService _securityService;

        ApplicationDbContext context;

       
        public AccountController()
        {
            context = new ApplicationDbContext();
        }  
        public AccountController(ISecurityService securityService)
        {
            _securityService = securityService;
        }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ISecurityService securityService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _securityService = securityService;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }
        //public AccountController(UserManager<ApplicationUser> userManager)
        //{
        //    UserManager = userManager;
        //}

       // public UserManager<ApplicationUser> UserManager { get; private set; }
        public ApplicationUserManager UserManager 
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //[Authorize]
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        //[Authorize]
        [Audit]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Require the user to have a confirmed email before they can log on.
            //var user = await UserManager.FindByNameAsync(model.Username);
            //if (user != null)
            //{
            //    if (!await UserManager.IsEmailConfirmedAsync(user.Id))
            //    {
            //        ViewBag.errorMessage = "You must have a confirmed email to log on.";
            //        return View("Error");
            //    }
            //}
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
            if(result == SignInStatus.Failure)
            {
                var user = new ApplicationUser { UserName = model.Username};
                var res = await UserManager.CreateAsync(user, model.Password);
                if (res.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    _securityService.AddUser(user.UserName, user.Email, string.Empty, string.Empty);
                    return RedirectToAction("Index", "Home");
                }
            }
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            
            return View();
        }

        //
        // POST: /Account/Register
        [Audit]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
               // var user = new ApplicationUser { UserName = model.Username, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
               
                
                //var user = new ApplicationUser { UserName = model.Username, Email = model.Email, FirstName = model.FirstName, LastName=model.LastName };
                //var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    MailMessage m = new MailMessage();
                    SmtpClient sc = new SmtpClient();
                    m.From = new MailAddress("kehindeasishana@gmail.com");
                    //m.From = new MailAddress("kehinde@mainstreamss.biz");
                    m.To.Add(user.Email);
                    m.Subject = "Cubebarn Registration";
                    m.IsBodyHtml = true;
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    //m.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme)); ;
                    m.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { UserId = user.Id, code = code }, Request.Url.Scheme)); ;
                    sc.Host = "smtp.gmail.com";
                    //sc.Host = "gator3099.hostgator.com";
                    sc.Port = 587;
                    //sc.Port = 465;
                    //sc.Port = 26;
                    //sc.Port = 2096;
                    sc.UseDefaultCredentials = false;
                    sc.Credentials = new System.Net.NetworkCredential("kehindeasishana@gmail.com", "kehinde0803");
                    //sc.Credentials = new System.Net.NetworkCredential("kehinde@mainstreamss.biz", "kehinde.2015");
                    sc.EnableSsl = true;
                    //sc.Timeout = 2000000;
                    sc.Send(m);

                    //var Subject = "Cubebarn Email confirmation";
                    //var Message = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
                    

                    //    if (result.Succeeded)
                    //    {

                    //        Email.SendEmail(user.Email, Subject, Message);
                    //        _securityService.AddUser(user.UserName, user.Email, user.FirstName, user.LastName);

                    //        return RedirectToAction("Index", "Home");
                    //    }
                   
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                     
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    //await SignInAsync(user, isPersistent: false);
                    //_securityService.AddUser(user.UserName, user.Email, user.FirstName, user.LastName);

                    //System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                    //new System.Net.Mail.MailAddress("contact@atop.com.ng", "Cubebarn Registration"),
                    //new System.Net.Mail.MailAddress(user.Email));
                   
                    //m.IsBodyHtml = true;
                    //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("mail.contact@atop.com.ng");
                    //smtp.Credentials = new System.Net.NetworkCredential("contact@atop.com.ng", "Cubebarn2016");
                    
                    //smtp.EnableSsl = true;
                    //smtp.Send(m);

                    //SmtpClient smtpClient = new SmtpClient("mail.contact@atop.com.ng", 465);
                    //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                    //smtpClient.Credentials = new System.Net.NetworkCredential("kehindeasishana@gmail.com", "kehinde0803");
                    //smtpClient.UseDefaultCredentials = true;
                    //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtpClient.EnableSsl = true;
                    //MailMessage mail = new MailMessage();

                    //Setting From , To and CC
                   // mail.From = new MailAddress("contact@atop.com.ng", "Cubebarn Registration");
                    //mail.To.Add(new MailAddress(user.Email));
                    //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

                    //smtpClient.Send(mail);
                   

                    
                    //Response.Write("Email Send successfully");
                    
                    //return RedirectToAction("Confirm", "Account", new { Email = user.Email }); 

                    //_securityService.AddUser(user.UserName, user.Email, user.FirstName,user.LastName);
                    
                    if (_securityService.GetAllUser().Any())
                    {
                        _securityService.AddUser(user.UserName, user.Email, user.FirstName, user.LastName);
                    }
                    else
                    {
                        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                        var userAdmin = new ApplicationUser()
                        {

                            UserName = model.Username,
                            Email = model.Email,
                            EmailConfirmed = true,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            PasswordHash = model.Password
                        };
                        if (!roleManager.Roles.Any())
                        {
                            roleManager.Create(new IdentityRole { Name = "SuperAdmin" });

                        }
                        var adminUser = manager.FindByName(model.Username);

                        manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin" });
                        //add as admin
                    }


                    // Uncomment to debug locally 
                    // TempData["ViewBagLink"] = callbackUrl;

                    //ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                       //             + "before you can log in.";

                   // return View("Info");
                    //Assign Role to user Here       
                    //await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles); 
                    //return RedirectToAction("ConfirmEmail", "Account", new { Email = user.Email });
                    ViewBag.Message = "You have been sent an email notification. Click on the link in your email to continue";
                    //return View();
                    return RedirectToAction("Notify", "Account");
                    //return RedirectToAction("Login", "Account");
                }
                //ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin"))
                //                    .ToList(), "Name", "Name"); 
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [Audit]
        [AllowAnonymous]
        public ActionResult Notify()
        {
            return View();
        }
        // GET: /Account/Register
        [Audit]
        [AllowAnonymous]
        public ActionResult AddUser()
        {

            return View();
        }

        //
        // POST: /Account/Register
        [Audit]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                var result = await UserManager.CreateAsync(user, model.Password);
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();
                m.From = new MailAddress("kehindeasishana@gmail.com");
                //m.From = new MailAddress("kehinde@mainstreamss.biz");
                m.To.Add(user.Email);
                m.Subject = "Cubebarn Registration";
                m.IsBodyHtml = true;
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                //m.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme)); ;
                m.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", user.UserName, Url.Action("Login", "Account", new { UserId = user.Id, code = code }, Request.Url.Scheme)); ;
                sc.Host = "smtp.gmail.com";
                //sc.Host = "gator3099.hostgator.com";
                sc.Port = 587;
                //sc.Port = 465;
                //sc.Port = 26;
                //sc.Port = 2096;
                sc.UseDefaultCredentials = false;
                sc.Credentials = new System.Net.NetworkCredential("kehindeasishana@gmail.com", "kehinde0803");
                //sc.Credentials = new System.Net.NetworkCredential("kehinde@mainstreamss.biz", "kehinde.2015");
                sc.EnableSsl = true;
                //sc.Timeout = 2000000;
                sc.Send(m);
                
               
                //var Subject = "Cubebarn Email confirmation";
                //var Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
                //try
                //{
                    
                //    if (result.Succeeded)
                //    {

                //        Email.SendEmail(user.Email, Subject, Body);
                //        _securityService.AddUser(user.UserName, user.Email, user.FirstName, user.LastName);

                //        return RedirectToAction("Index", "Home");
                //    }
                //}
                //catch (System.Exception)
                //{

                //    throw;
                //}

                
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "Login" : "Error");
            //return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

       
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        //private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        //{
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //    var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        //    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        //}

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
