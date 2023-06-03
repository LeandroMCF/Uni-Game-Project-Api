using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uni.Project.WebApi.Domain;
using Uni.Project.WebApi.Enum;
using Uni.Project.WebApi.Interface;
using Uni.Project.WebApi.Repository;

namespace Uni.Project.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserInterface _user { get; set; }

        public UserController()
        {
            _user = new UserRepository();
        }

        [HttpPost("cad/{name}/{email}/{password}")]
        public IActionResult CadUser(string name, string email, string password)
        {
            try
            {
                UserDomain user = new UserDomain(name, email, password, DateTime.Now, DateTime.Now, StatusEnum.allowed, UserEnum.comum);
                return Ok(_user.CadUser(user));
            }
            catch (Exception erro)
            {

                return BadRequest(erro);
            }

        }

        [HttpGet("server")]
        public IActionResult Server()
        {
            return Ok("Work!");
        }

        [HttpPost("login/{email}/{password}")]
        public IActionResult Login(string email, string password)
        {
            try
            {
                return Ok(_user.Login(email, password));
            }
            catch (Exception erro)
            {

                return BadRequest(erro);
            }

        }

        [HttpPut("download/{id}")]
        public IActionResult Download(string id)
        {
            try
            {
                return Ok(_user.Download(id));
            }
            catch (Exception erro)
            {

                return BadRequest(erro);
            }

        }
    }
}
