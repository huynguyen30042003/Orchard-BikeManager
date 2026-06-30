using BikeManagerV3.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Media;
using OrchardCore.Users;
using OrchardCore.Users.Models;
using System.Net.Http.Json; 
namespace BikeManagerV3.Auth.Controllers;

[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IUser> _userManager;

    private readonly HttpClient _httpClient;

    private readonly IMediaFileStore _mediaFileStore;
    public AuthController(
        UserManager<IUser> userManager,
        IHttpClientFactory factory,
        IMediaFileStore mediaFileStore)
    {
        _userManager = userManager;

        _httpClient = factory.CreateClient();

        _mediaFileStore = mediaFileStore;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var exists =
            await _userManager.FindByNameAsync(request.Username);

        if (exists != null)
        {
            return BadRequest(new
            {
                message = "Username already exists"
            });
        }

        var emailExists =
            await _userManager.FindByEmailAsync(request.Email);

        if (emailExists != null)
        {
            return BadRequest(new
            {
                message = "Email already exists"
            });
        }

        var user = new User
        {
            UserName = request.Username,

            Email = request.Email,

            EmailConfirmed = true,

            Properties =
            {
                ["FullName"] = request.FullName,
            }
        };

        var result =
            await _userManager.CreateAsync(
                user,
                request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                errors = result.Errors.Select(e => e.Description)
            });
        }

        return Ok(new
        {
            message = "Register success"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var form =
            new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", request.Username },
                { "password", request.Password },
                { "client_id", "nextjs-client" },
                { "scope", "openid profile offline_access roles email" }
            };

        var response =
            await _httpClient.PostAsync(
                "https://localhost:5001/connect/token",
                new FormUrlEncodedContent(form));

        var errorContent =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return Unauthorized(new
            {
                message = "Invalid username or password",
                error = errorContent
            });
        }

        var result =
            await response.Content
                .ReadFromJsonAsync<TokenResponse>();

        if (result == null)
        {
            return Unauthorized();
        }

        if (!string.IsNullOrEmpty(result?.refresh_token))
        {
             Response.Cookies.Append(
                "refresh_token",
                result.refresh_token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    Path = "/"
                });
        }

        return Ok(new
        {
            message = "Login success",

            access_token = result.access_token,

            expires_in = result.expires_in
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken =
            Request.Cookies["refresh_token"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new
            {
                message = "Refresh token missing"
            });
        }

        var form =
            new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                { "client_id", "nextjs-client" }
            };

        var response =
            await _httpClient.PostAsync(
                "https://localhost:5001/connect/token",
                new FormUrlEncodedContent(form));

        if (!response.IsSuccessStatusCode)
        {
            return Unauthorized(new
            {
                message = "Invalid refresh token"
            });
        }

        var result =
            await response.Content
                .ReadFromJsonAsync<TokenResponse>();

        if (result == null)
        {
            return Unauthorized();
        }

        if (!string.IsNullOrEmpty(result?.refresh_token))
        {
            Response.Cookies.Append(
               "refresh_token",
               result.refresh_token,
               new CookieOptions
               {
                   HttpOnly = true,
                   Secure = true,
                   SameSite = SameSiteMode.None,
                   Expires = DateTimeOffset.UtcNow.AddDays(30),
                   Path = "/"
               });
        }

        return Ok(new
        {
            access_token = result.access_token,
            expires_in = result.expires_in
        });
    }

    [Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId) as User; ;

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            id = userId,
            username = user.UserName,
            email = user.Email,

            fullName = user.Properties["FullName"]?.ToString(),

            phoneNumber =
                user.Properties["PhoneNumber"]?.ToString(),

            avatarUrl =
                user.Properties["AvatarUrl"]?.ToString()
        });
    }

    [Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
    [HttpPatch("update")]
    public async Task<IActionResult> Update(
            [FromForm] UpdateRequest request)
    {
        var userId = User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user =
            await _userManager.FindByIdAsync(userId) as User;

        if (user == null)
        {
            return NotFound(new
            {
                message = "ko tim thay user"
            });
        }

        user.UserName = request.Username;

        user.Properties["FullName"] =
            request.FullName;

        user.Properties["PhoneNumber"] =
            request.PhoneNumber ?? "";

        if (request.Avatar != null)
        {
            var extension =
                Path.GetExtension(request.Avatar.FileName);

            var fileName =
                $"avatars/{Guid.NewGuid()}{extension}";

            await using var stream =
                request.Avatar.OpenReadStream();

            await _mediaFileStore.CreateFileFromStreamAsync(
                fileName,
                stream,
                overwrite: true);

            user.Properties["AvatarUrl"] =
                fileName;
        }

        var result =
            await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new
        {
            message = "Updated successfully",

            avatar =
                user.Properties["AvatarUrl"]?.ToString()
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(
            "refresh_token",
            new CookieOptions
            {
                Path = "/",
                Secure = true,
                SameSite = SameSiteMode.None
            });

        return Ok(new
        {
            message = "Logout success"
        });
    }
}