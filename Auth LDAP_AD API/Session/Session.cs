using Novell.Directory.Ldap;
using AuthLDAP_AD_Model;

namespace Auth_LDAP_AD_API.Session
{
	public class Session
	{
		public static  AuthLDAP_ADReturnObject GetAuthLDAP_AD_Filter(string login, string password, string filter)
		{
			try
			{
				string ldapHost = "auth.intra.cg30.fr";
				int ldapPort = 389;
				string	searchBase = "DC=intra,DC=cg30,DC=fr";
				string	searchFilter = $"(&(objectClass=user)(sAMAccountName={login})({filter}))";
				
				string[] requiredAttributes = { "cn" };
				LdapConnection con = new();

				LdapSearchConstraints cons = con.SearchConstraints;
				cons.ReferralFollowing = true;
				con.Constraints = cons;

				con.Connect(ldapHost, ldapPort);
				con.Bind(login + "@intra.cg30.fr", password);
				Console.WriteLine(searchFilter);
				var results = con.Search(searchBase, LdapConnection.ScopeBase, searchFilter, requiredAttributes, false);
				con.Disconnect();
				if (results == null || results.Count <= 0)
				{
					return new AuthLDAP_ADReturnObject(false, "L'utilisateur n'est pas autorisé.");
				}
				else
				{
					Console.WriteLine(results.Next());
					return new AuthLDAP_ADReturnObject(true, "This user is INDEED a member of that group");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return new AuthLDAP_ADReturnObject(false, "\r\nUnexpected exception occured:\r\n\t" + e.GetType() + ":" + e.Message);
			}
		}
	}
}