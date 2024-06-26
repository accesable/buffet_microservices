﻿using AuthenticationServices.Dtos.Account;
using AuthenticationServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace AuthenticationServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized(new { message = "Invalid Username" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.IsEmailConfirmedAsync(user);

            return Ok(
                new NewUserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    AccessToken = _tokenService.CreateToken(user, userRoles),
                    Roles = userRoles
                }
            );
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }
                var appUser = new AppUser
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    Contact = registerDto.Contact,
                    Address = registerDto.Address,
                    UserName = registerDto.Email
                };
                var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createUser.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(appUser);
                    return Ok(new NewUserDto
                    {
                        Email = appUser.Email,
                        UserName = appUser.UserName,
                        AccessToken = _tokenService.CreateToken(appUser, userRoles),
                    });
                }
                else
                {
                    return StatusCode(500, createUser.Errors.ToList());
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }

        }
    }
}
