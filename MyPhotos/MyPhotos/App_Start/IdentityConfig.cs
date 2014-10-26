using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MyPhotos.Models;

namespace MyPhotos
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public const string AdminRoleName = "admins";
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            
            // Verify that the admin role exists and the bootstrap users are in the
            // admin role. Whenever the website runs we make sure bootstrap users
            // are populated.
            var adminRole = FindOrCreateAdminRole(context);
            VerifyAdminUsers(adminRole, manager);

            return manager;
        }

        private static IdentityRole FindOrCreateAdminRole(IOwinContext context)
        {
            var ac = context.Get<ApplicationDbContext>();
            var roles = from r in ac.Roles where r.Name.Equals(AdminRoleName, StringComparison.OrdinalIgnoreCase) select r;
            if (!roles.Any())
            {
                ac.Roles.Add(new IdentityRole(AdminRoleName));
                var dbContext = (DbContext)ac;
                dbContext.SaveChanges();
            }

            ac = context.Get<ApplicationDbContext>();

            roles = from r in ac.Roles where r.Name.Equals(AdminRoleName, StringComparison.OrdinalIgnoreCase) select r;
            return roles.First();
        }

        private static void VerifyAdminUsers(IdentityRole adminRole, ApplicationUserManager manager)
        {
            var settings = (NameValueCollection)ConfigurationManager.GetSection("MyPhotosSettings");
            var bootstrapAdminsCsv = settings["BootstrapAdmins"];
            if (string.IsNullOrEmpty(bootstrapAdminsCsv))
            {
                return;
            }

            var bootstrapAdmins = bootstrapAdminsCsv.Split(new char[] { ',' });

            foreach(var admin in bootstrapAdmins)
            {
                var selection = from u in manager.Users where u.UserName.Equals(admin, StringComparison.OrdinalIgnoreCase) select u;
                if (selection.Any())
                {
                    var user = selection.First();
                    if (!manager.IsInRole(user.Id, adminRole.Name))
                    {
                        manager.AddToRoles(user.Id, new string[] { adminRole.Name });
                    }
                }
            }
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
