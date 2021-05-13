using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// Permite realizar consultas sobre el usuario.
    /// </summary>
    [Authorize]
    public class UserController : ApiController
    {
        private IUserRepository _userRepository;

        public UserController()
        {
            _userRepository = new UserRepository();
        }

        /// <summary>
        /// Obtiene informacion del Usuario asociado al Token JWT.
        /// </summary>
        /// <response code="200">Devuelve el usuario.</response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>               
        /// <response code="403">Forbidden. Token JWT de acceso es correcto pero no tiene autorización para el recurso.</response>               
        /// <response code="404">NotFound. No se ha encontrado el usuario actual.</response> 
        [HttpGet]
        [ResponseType(typeof(ApplicationUser))]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_userRepository.GetUser(User.Identity.Name));
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
        /// Realiza una tranferencia de Balance entre el Usuario asociado al Token JWT y uno de elección.
        /// </summary>
        /// <param name="userName">Usuario a quien se tranfiere el balance.</param>
        /// <param name="amount">Cantidad a tranferir.</param>
        /// <response code="201"></response>        
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>               
        /// <response code="403">Forbidden. Token JWT de acceso es correcto pero no tiene autorización para el recurso.</response>               
        /// <response code="404">NotFound. No se ha encontrado el usuario actual.</response>
        [HttpPost]
        [Route("api/User/{userName}")]
        public IHttpActionResult Post([FromBody]int amount, string userName)
        {
            try
            {
                _userRepository.TransferToUser(User.Identity.Name, userName, amount);
                return Ok("Balance tranfer succes");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("User balance cannot become negative");
            }
        }


    }
}
