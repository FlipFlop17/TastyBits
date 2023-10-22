﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TastyBits.Services
{
    public class LoggedUserService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly UserManager<IdentityUser> _userManager;
        public IdentityUser UserStore { get; private set; }


        public LoggedUserService(AuthenticationStateProvider authState ,UserManager<IdentityUser> userManager)
        {
            _authStateProvider = authState;
            _userManager = userManager;
        }

        public async Task<IdentityUser> GetUserDataAsync()
        {
            AuthenticationState authState= await _authStateProvider.GetAuthenticationStateAsync();
            ClaimsPrincipal user = authState.User;

            if (user.Identity.IsAuthenticated) {
                UserStore= await _userManager.GetUserAsync(user);
                return UserStore;
            }
            return null;
        }
    }
}