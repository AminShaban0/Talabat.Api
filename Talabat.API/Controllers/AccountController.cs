using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.API.Dtos;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.API.Controllers
{
    
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _auth;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager , IAuthService auth ,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _auth = auth;
            _mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            var checkpass =await _signInManager.CheckPasswordSignInAsync(user, model.Password , false);
            if (checkpass.Succeeded is false)
                return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token =await _auth.CreateTokenAsync(user ,_userManager)
            });

        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if(CheckemailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors =new string[]{ "This Email Is Already In User !" } });
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };
            var result =await _userManager.CreateAsync(user,model.Password);
            if (result.Succeeded is false) return BadRequest(new ApiResponse(400));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "this is Token"
            });

        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            
            var buyeremail = User.FindFirstValue(ClaimTypes.Email);
            var user =await _userManager.FindByEmailAsync(buyeremail);
            return Ok(new UserDto()
            {
                DisplayName =user.DisplayName,
                Email = user.Email,
                Token = await _auth.CreateTokenAsync(user , _userManager)
            });
        }
        [HttpGet("address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user =await _userManager.FindUserWithAddressAsync(User);
            var useraddress = _mapper.Map<AddressDto>(user.Adress);
            return Ok(useraddress);

        }
        [HttpPut("address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var addressmap =_mapper.Map<Adress>(address);
            addressmap.Id = user.Adress.Id;
            user.Adress = addressmap;
            var result =await _userManager.UpdateAsync(user);
            if(result.Succeeded is false) return BadRequest(new ApiResponse(404));
            return Ok(address);

        }
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckemailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
            
        }

    }   
}
