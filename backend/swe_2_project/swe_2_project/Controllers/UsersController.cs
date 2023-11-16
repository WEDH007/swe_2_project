using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swe_2_project.Data;
using swe_2_project.Models;
using static Org.BouncyCastle.Math.EC.ECCurve;
using swe_2_project.Services;
using BC = BCrypt.Net.BCrypt;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace swe_2_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly swe_2_DbContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public UsersController(swe_2_DbContext context, IEmailService emailService, IConfiguration config)
        {
            _context = context;
            _emailService = emailService;
            _config = config;
        }

        // GET: /users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDto dto)
        {
            // Check if the email already exists
            if (await _context.Users.AnyAsync(u => u.email == dto.Email))
            {
                return BadRequest("Email already in use.");
            }

            // Hash the password using BCrypt
            string hashedPassword = BC.HashPassword(dto.Password);

            // Create the user
            var user = new users
            {
                first_name = dto.FirstName,
                last_name = dto.LastName,
                password_hash = hashedPassword,
                email = dto.Email,
                dob = dto.Dob, // Assuming you add this to UserRegisterDto
                verification_token = Guid.NewGuid().ToString(),
                status = user_status.inactive
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                string verificationLink = $"UPDATE LINK?token={user.verification_token}";
                var emailDto = new EmailDto
                {
                    To = user.email,
                    Subject = "Verify your email",
                    Body = $"Please click the link to verify your email: {verificationLink}"
                };
                _emailService.SendEmail(emailDto);

                return Ok(new { Email = user.email });
            }
            catch (Exception ex)
            {
                // Log the exception details for further analysis
                // Consider using a logging framework
                return StatusCode(500, "An error occurred while registering the user.");
            }
        }

        [HttpGet("verify/{token}")]
        public async Task<ActionResult> VerifyEmail(string token)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.verification_token == token);
            if (user == null)
            {
                return BadRequest("Invalid token.");
            }

            user.status = user_status.active;
            user.verified_at = DateTime.UtcNow;
            user.verification_token = null;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Email verified successfully." });

        }

        [HttpPost("login")]
        public async Task<ActionResult<users>> Login(UserLoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == request.email);

            if (user.verification_token != null)
            {
                return BadRequest("Email not verified.");
            }

            if (user.email != request.email)
            {
                return BadRequest("User not found.");
            }

            if (!BC.Verify(request.password, user.password_hash))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);
            var accessToken = new
            {
                AccessToken = token,
            };

            return Ok(accessToken);
        }

        private string CreateToken(users user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddHours(8),
                        signingCredentials: cred
                    );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        // PUT: /users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Update the user properties
            user.first_name = dto.FirstName;
            user.last_name = dto.LastName;
            user.email = dto.Email;
            user.dob = dto.Dob;


            try
            {
                await _context.SaveChangesAsync();
                return Ok(user); // Indicates success but no content to return
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }


        // DELETE: /users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return NoContent(); // Indicates success but no content to return
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }



        // Additional methods like Post, Put, Delete can be added here
    }
}
