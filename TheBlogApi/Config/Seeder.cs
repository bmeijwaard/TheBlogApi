using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Data.Context.Providers.Contracts;
using TheBlogApi.Models.Domain;
using TheBlogApi.Models.DTO;
using TheBlogApi.Data.Identity.Contracts;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Models.Types;
using AutoMapper;
using System.Net;

namespace TheBlogApi.Config
{
    public class RoleSeeder
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleSeeder(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new Role(role));
                }
            }
        }
    }

    public class UserSeeder
    {
        private readonly IDbContextProvider _context;
        private readonly IUserService _userService;

        public UserSeeder(IDbContextProvider context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task Seed()
        {
            UserDTO user = null;
            if (!_context.Context.Users.Any())
            {
                using (var contextTransaction = _context.Context.BeginTransaction())
                {
                    try
                    {
                        // admin user
                        var admin = new UserDTO
                        {
                            Email = "admin@bobdebouwer.net",
                            UserName = "Bobmans",
                            EmailConfirmed = true,
                            AccountConfirmedDateUtc = DateTime.UtcNow
                        };

                        await _userService.CreateAsync(admin, "Aasd123!");
                        await _userService.AddToRoleAsync(admin.Email, Roles.Administrator);

                        // test user
                        user = new UserDTO
                        {
                            Email = "bmeijwaard@gmail.com",
                            UserName = "testuser",
                            FirstName = "Bob",
                            LastName = "Meijwaard"
                        };
                        await _userService.CreateAsync(user, "Aasd123!");

                        await _context.Context.SaveChangesAsync();
                        contextTransaction.Commit();
                    }
                    catch
                    {
                        contextTransaction.Rollback();
                        throw;
                    }
                }
            }

            if (user != null && !_context.Context.Tenants.Any())
            {
                var token = await _userService.GenerateEmailConfirmationTokenAsync(user.Email);
                await _userService.ActivateAccountAsync(user.Email, WebUtility.UrlEncode(token));

            }
        }
    }
}

