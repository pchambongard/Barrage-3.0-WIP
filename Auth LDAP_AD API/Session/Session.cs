using LdapForNet;
using AuthLDAP_AD_Model;

namespace Auth_LDAP_AD_API.Session
{
	public class Session
	{
		private readonly string Path;
		public Session(string path)
		{
			Path = path;
		}
		// public AuthLDAP_ADReturnObject GetAuthLDAP_AD(string login, string password)
		// {
		// 	try
		// 	{
		// 		using var cn = new LdapConnection();
		// 		cn.Connect(new Uri(Path));
		// 		cn.Bind(LdapForNet.Native.Native.LdapAuthType.Digest, new LdapCredential { UserName = login, Password = password });
		// 		return new AuthLDAP_ADReturnObject(true, "User authenticated");
		// 	}
		// 	catch (Exception e)
		// 	{
		// 		return new AuthLDAP_ADReturnObject(false, "\r\nUnexpected exception occured:\r\n\t" + e.GetType() + ":" + e.Message);
		// 	}
		// }
		public AuthLDAP_ADReturnObject GetAuthLDAP_AD_Filter(string login, string password, string filter)
		{
			try
			{
				using var cn = new LdapConnection();
				cn.Connect(new Uri(Path));
				cn.Bind(LdapForNet.Native.Native.LdapAuthType.Digest, new LdapCredential { UserName = login, Password = password });
				var entries = cn.Search("DC=intra,DC=cg30,DC=fr", $"(&(objectClass=user)(sAMAccountName={login})({filter}))");
				if (entries == null || entries.Count == 0)
				{
					return new AuthLDAP_ADReturnObject(false, "This user is *NOT* member of that group");
				}
				return new AuthLDAP_ADReturnObject(true, "This user is INDEED a member of that group");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return new AuthLDAP_ADReturnObject(false, "\r\nUnexpected exception occured:\r\n\t" + e.GetType() + ":" + e.Message);
			}
		}
	}
}