using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{

    [Authorize(Roles ="Admin")]
    public class AdminController : ApiController
    {
        private IUserRepository _userRepository;

        /// <summary>
        /// Controlador para Administradores
        /// </summary>
        /// <remarks>
        /// Acceso solo para Administradores con el Rol Admin.
        /// </remarks>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>               
        /// <response code="403">Forbidden. Token JWT de acceso es correcto pero no tiene autorización para el recurso.</response> 
        public AdminController()
        {
            _userRepository = new UserRepository();
        }

        /// <summary>
        /// Obtiene una lista de todos los usuarios.
        /// </summary>
        /// <response code="200">Devuelve una lista de usuarios.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>               
        /// <response code="403">Forbidden. Token JWT de acceso es correcto pero no tiene autorización para el recurso.</response>               
        /// <response code="500">InternalServerError.Ha habido un error al procesar la peticion</response>
        /// <returns>Una lista de todos los usuarios</returns>
        [HttpGet]
        [Route("api/Admin/Users")]
        [ResponseType(typeof(List<ApplicationUser>))]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_userRepository.GetAllUsers());
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Obtiene informacion del Usuario con UserName.
        /// </summary>
        /// <param name="userName">Nombre Unico del Usuario a devolver.</param>
        /// <response code="200">Devuelve el usuario.</response>
        /// <response code="400">BadRequest. La petición es incorrecta</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>               
        /// <response code="403">Forbidden. Token JWT de acceso es correcto pero no tiene autorización para el recurso.</response>               
        /// <response code="404">NotFound. No se ha encontrado el usuario actual.</response>
        /// <response code="500">InternalServerError. Ha habido un error al procesar la peticion</response>
        [HttpGet]
        [Route("api/Admin/Users/{userName}")]
        [ResponseType(typeof(ApplicationUser))]
        public IHttpActionResult Get(string userName)
        {
            if (userName == null) return BadRequest("UserName cannot be null");

            try
            {
                return Ok(_userRepository.GetUser(userName));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Create a new User.
        /// </summary>
        /// <param name="user"></param>
        /// <response code="201"></response>
        /// <response code="400">BadRequest. La petición es incorrecta</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>               
        /// <response code="403">Forbidden. Token JWT de acceso es correcto pero no tiene autorización para el recurso.</response>
        /// <response code="500">InternalServerError.Ha habido un error al procesar la peticion</response>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Admin/Users")]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> Post([FromBody] RegisterModel user)
        {
            if (user == null) return BadRequest("User cannot be null");

            try
            { 
                return Created("Created new User", await _userRepository.AddUserAsync(user.UserName, user.Email, user.Password, user.InitialBalance));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Edita un usuario.
        /// </summary>
        /// <param name="user"></param>
        /// <response code="200"></response>
        /// <response code="400">BadRequest. La petición es incorrecta</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>               
        /// <response code="403">Forbidden. Token JWT de acceso es correcto pero no tiene autorización para el recurso.</response>
        /// <response code="404">NotFound. No se ha encontrado el usuario actual.</response>
        /// <response code="500">InternalServerError.Ha habido un error al procesar la peticion</response>
        /// <returns>Devuelve el Usuario Editado.</returns>
        [HttpPut]
        [Route("api/Admin/Users")]
        [ResponseType(typeof(ApplicationUser))]
        public IHttpActionResult Put([FromBody] ApplicationUser user)
        {
            if (user == null) return BadRequest("User Cannot be null");

            try
            {   
                return Ok(_userRepository.EditUser(user));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Añade una cantidad al Balance del Usuario.
        /// </summary>
        /// <param name="amount">Cantidad a añadir</param>
        /// <param name="userName">Destinatario</param>
        /// <response code="200"></response>        
        /// <response code="400">BadRequest. La petición es incorrecta</response>            
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>               
        /// <response code="403">Forbidden. Token JWT de acceso es correcto pero no tiene autorización para el recurso.</response>
        /// <response code="404">NotFound. No se ha encontrado el usuario actual.</response>
        /// <response code="500">InternalServerError.Ha habido un error al procesar la peticion</response>
        /// <returns>Devuelve el nuevo Balance.</returns>
        [HttpPut]
        [Route("api/Admin/Balance/{userName}/Add")]
        [ResponseType(typeof(string))]
        public IHttpActionResult AddBalance([FromBody] int amount, string userName)
        {
            if (userName == null) return BadRequest("User cannot be null");
            if (amount < 0) return BadRequest("Amount cannot be negative");

            try
            {
                return Ok(_userRepository.AddBalance(userName, amount));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Descuenta una cantidad al Balance del Usuario.
        /// </summary>
        /// <param name="amount">Cantidad a descontar</param>
        /// <param name="userName">Destinatario</param>
        /// <response code="200"></response>
        /// <response code="400">BadRequest. La petición es incorrecta</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>               
        /// <response code="403">Forbidden. Token JWT de acceso es correcto pero no tiene autorización para el recurso.</response>
        /// <response code="404">NotFound. No se ha encontrado el usuario actual.</response>
        /// <response code="500">InternalServerError. Ha habido un error al procesar la peticion</response>
        /// <returns>Devuelve el nuevo Balance.</returns>
        [HttpPut]
        [Route("api/Admin/Balance/{userName}/Remove")]
        [ResponseType(typeof(string))]
        public IHttpActionResult RemoveBalance([FromBody] int amount, string userName)
        {
            if (userName == null) return BadRequest("User cannot be null");
            if (amount < 0) return BadRequest("Amount cannot be negative");

            try
            {   
                return Ok(_userRepository.RemoveBalance(userName, amount));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Borra un usuario.
        /// </summary>
        /// <param name="userName">Nombre Unico del Usuario a borrar.</param>
        /// <response code="200"></response>
        /// <response code="400">BadRequest. La petición es incorrecta</response>  
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>               
        /// <response code="403">Forbidden. Token JWT de acceso es correcto pero no tiene autorización para el recurso.</response>               
        /// <response code="404">NotFound. No se ha encontrado el usuario actual.</response>
        /// <response code="500">InternalServerError.Ha habido un error al procesar la peticion</response>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Admin/Users/{userName}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult Delete(string userName)
        {
            if (userName == null) return BadRequest("User cannot be null");

            try
            {
                _userRepository.DeleteUser(userName);
                return Ok("User delete Succes");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

    }

}
