namespace Model_AuthLDAP_AD
{
	public class AuthLDAP_ADReturnObject
	{
		public bool Auth { get; set; } = false;
		public string State { get; set; } = "";

		public AuthLDAP_ADReturnObject() { }
		public AuthLDAP_ADReturnObject(bool auth, string state)
		{
			Auth = auth;
			State = state;
		}
	}
}
