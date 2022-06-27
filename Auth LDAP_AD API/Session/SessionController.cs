using Microsoft.AspNetCore.Mvc;
using AuthLDAP_AD_Model;

namespace Auth_LDAP_AD_API.Session
{
	[ApiController]
	[Route("api/[controller]")]
	public class SessionController : ControllerBase
	{
		private readonly ILogger<SessionController> logger;

		public SessionController(ILogger<SessionController> logger)
		{
			this.logger = logger;
		}

		[HttpGet]
		public IActionResult GetAuth([FromBody] List<string> infos)
		{
			try
			{
				if (infos.Count == 3)
				{
					return Ok(Session.GetAuthLDAP_AD_Filter(infos[0], infos[1], infos[2]));
				}
				else
				{
					return Ok(new AuthLDAP_ADReturnObject(false, "Wrong request body"));
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return Ok(new AuthLDAP_ADReturnObject(false, StatusCodes.Status500InternalServerError + " " + ex.Message));
			}
		}
	}
}
