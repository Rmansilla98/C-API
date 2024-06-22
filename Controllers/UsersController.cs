using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthKalumManagement.DbContext;
using AuthKalumManagement.DTOs;
using AuthKalumManagement.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthKalumManagement.Controllers
{
    [ApiController]
    [Route("authkalum-management/v1/users")]
    public class UsersController : ControllerBase
    {

        private readonly AuthKalumManagementContext AuthKalumManagementContext;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IMapper Mapper;

        public UsersController(AuthKalumManagementContext _AuthKalumManagementContext, UserManager<ApplicationUser> _UserManager, IMapper _Mapper)
        {
            this.AuthKalumManagementContext = _AuthKalumManagementContext;
            this.UserManager = _UserManager;
            this.Mapper = _Mapper;
        }

        //Evento para crear un Usuario
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> Post([FromBody] UserDTO userDTO)
        {
            var userInfo = new ApplicationUser()
            {
                UserName = userDTO.UserName,
                NormalizedUserName = userDTO.NormalizedUserName,
                Email = userDTO.Email
            };
            var newUser = await this.UserManager.CreateAsync(userInfo, userDTO.Password);
            if (newUser.Succeeded)
            {
                await this.UserManager.AddToRoleAsync(userInfo, "ROLE_USER");
                return Ok(newUser);
            }
            else
            {
                return BadRequest("La información enviada no es correcta");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<UserListDTO>>> Get()
        {
            List<ApplicationUser> users = await this.UserManager.Users.ToListAsync();
            if (users == null || users.Count == 0)
            {
                return NoContent();
            }
            List<UserListDTO> lista = new List<UserListDTO>();

            foreach (var item in users)
            {
                var roles = await this.UserManager.GetRolesAsync(item); // esto sirve para poder traer los roles a la lista 
                lista.Add(new UserListDTO()
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    NormalizedUserName = item.NormalizedUserName,
                    Email = item.Email,
                    Roles = roles
                });
            }
            return Ok(lista);
        }


        [HttpGet("search", Name = "GetUserByEmail")]

        public async Task<ActionResult<UserListDTO>> GetUserByEmail([FromQuery] string email)
        {
            var user = await this.UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NoContent();
            }
            var userRole = this.Mapper.Map<UserListDTO>(user);//esto sirve para convertir el objeto user a un UserListDTO
            userRole.Roles = await this.UserManager.GetRolesAsync(user);
            return Ok(userRole);

        }

        [HttpGet("{id}", Name = "GetUserById")]

        public async Task<ActionResult<UserListDTO>> GetUserById(string id)
        {
            var user = await this.UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NoContent();
            }
            var userRole = this.Mapper.Map<UserListDTO>(user);//esto sirve para convertir el objeto user a un UserListDTO
            userRole.Roles = await this.UserManager.GetRolesAsync(user);
            return Ok(userRole);
        }

        [HttpPost("add-role")]
        public async Task<ActionResult> UserAddRole([FromBody] UserAddRoleDTO userAddRoleDTO)
        {
            var user = await this.UserManager.FindByIdAsync(userAddRoleDTO.IdUser);// esto sirve para ir a traer el id del usario que agragaremos el role
            if (user == null)
            {
                return NoContent();
            }
            await this.UserManager.AddToRoleAsync(user, userAddRoleDTO.Role);//aca le agregamos el role al usario
            await this.UserManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, userAddRoleDTO.Role));//duda
            return Ok();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(string id)
        {
            var user = await this.UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NoContent();
            }
            await this.UserManager.DeleteAsync(user);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            var user = await this.UserManager.FindByIdAsync(id);// buscar el usuario 
            if (user == null)
            {
                return NoContent();
            }
            user.UserName = userUpdateDTO.UserName;
            user.NormalizedUserName = userUpdateDTO.NormalizedUserName;
            user.Email = userUpdateDTO.Email;
            var userUpdate = await this.UserManager.UpdateAsync(user);// se crea un objeto para poder guardar los cambios de los tatos actualizados
            if (userUpdate.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("remove-role")]
        public async Task<ActionResult> UserRemoveRole([FromBody] UserAddRoleDTO userAddRoleDTO)
        {
            var user = await this.UserManager.FindByIdAsync(userAddRoleDTO.IdUser);// esto sirve para ir a traer el id del usario que agragaremos el role
            if (user == null)
            {
                return NoContent();
            }
            await this.UserManager.RemoveFromRoleAsync(user, userAddRoleDTO.Role);//aca le agregamos el role al usario
            await this.UserManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, userAddRoleDTO.Role));
            return Ok();
        }

    }
}