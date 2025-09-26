using Microsoft.AspNetCore.Identity;
namespace AOL_Portal.Data
{
    public class TestUserSeed
    {
        private readonly UserManager<AolApplicationUser> _userManager;
        public TestUserSeed(UserManager<AolApplicationUser> userManager)
        {

            _userManager = userManager;
        }
        public async Task CreateDefaultUser()
        {


            var tessaExists = await _userManager.FindByEmailAsync("tessa@shad.ie").ConfigureAwait(false);
            if (tessaExists == null)
            {
                AolApplicationUser user = new AolApplicationUser()
                {
                    Email = "tessa@shad.ie",
                    UserName = "tessa@shad.ie",
                    FirstName = "Tessa",
                    Surname = "Marais",
                    IsSiteAdmin = true,
                    StatusId = 2,
                    CreatedDtm = DateTime.Now,
                    LastUpdateDtm = DateTime.Now,
                    LastUpdateUserId = string.Empty
                };
                var newuser = _userManager.CreateAsync(user, "TestuMFASUmfa123$").Result;
            }
            else if (tessaExists.EmailConfirmed == false)
            {
                tessaExists.EmailConfirmPasswordChanged = true;
                tessaExists.EmailConfirmed = true;
                await _userManager.UpdateAsync(tessaExists).ConfigureAwait(false);
            }

            var niciExists = await _userManager.FindByEmailAsync("nici@umfa.co.za").ConfigureAwait(false);
            if (niciExists == null)
            {
                AolApplicationUser user = new AolApplicationUser()
                {
                    Email = "nici@umfa.co.za",
                    UserName = "nici@umfa.co.za",
                    FirstName = "Nici",
                    Surname = "Marais",
                    IsSiteAdmin = true,
                    StatusId = 2,
                    CreatedDtm = DateTime.Now,
                    LastUpdateDtm = DateTime.Now,
                    LastUpdateUserId = string.Empty
                };
                var newuser = _userManager.CreateAsync(user, "TestuMFASUmfa123$").Result;
            }
            else if (niciExists.EmailConfirmed == false)
            {
                niciExists.EmailConfirmPasswordChanged = true;
                niciExists.EmailConfirmed = true;
                await _userManager.UpdateAsync(niciExists).ConfigureAwait(false);
            }
        }
    }
}
