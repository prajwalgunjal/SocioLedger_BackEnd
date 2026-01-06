using Businesslayer.Interface;
using CommonLayer.Models.RequestModels;
using CommonLayer.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace SocioLedger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private IUserBusiness iUserBusiness;
        private readonly ILogger<CustomerController> logger;
        public CustomerController(IUserBusiness iUserBusiness, ILogger<CustomerController> logger)
        {
            this.iUserBusiness = iUserBusiness;
            this.logger = logger;
        }
        [HttpPost]
        // request url:-  localhost/Controller_name/MethodRoute
        [Route("RegisterEmployee")]
        public ActionResult Registeration(OwnerModel registrationModel)
        {
            try
            {
                var result = iUserBusiness.RegisterEmployee(registrationModel);
                if (result != null)
                {
                    logger.LogInformation("Registered information");
                    return Ok(new ResponseModel<OwnerEntity> { Success = true, Message = "Registred Successfull", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<OwnerEntity> { Success = false, Message = "Not Registred", Data = null });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw ex;
            }
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                var result = iUserBusiness.Login(loginModel);
                if (result != null)
                {
                    return Ok(new ResponseModel<string> { Success = true, Message = "Login Successfull", Data = result });
                }

                else
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Email Not Found", Data = null });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
