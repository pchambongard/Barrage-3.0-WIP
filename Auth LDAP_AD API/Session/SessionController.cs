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
				// if (infos.Count == 3)
				// {
				// 	Session auth = new(infos[0]);
				// 	return Ok(auth.GetAuthLDAP_AD(infos[1], infos[2]));
				// }
				if (infos.Count == 4)
				{
					Session auth = new(infos[0]);
					return Ok(auth.GetAuthLDAP_AD_Filter(infos[1], infos[2], infos[3]));
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
