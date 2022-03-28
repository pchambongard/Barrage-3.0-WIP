using System.DirectoryServices;
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
		public AuthLDAP_ADReturnObject GetAuthLDAP_AD(string login, string password)
		{
			try
			{
				using DirectoryEntry de = new(Path, login, password);
				using DirectorySearcher searcher = new(de);
				searcher.Filter = $"(&(objectClass=user)(sAMAccountName={login}))";

				try
				{
					SearchResultCollection res = searcher.FindAll();
					Console.WriteLine(searcher.Filter);
					return new AuthLDAP_ADReturnObject(true, "User credentials are correct");
				}
				catch (Exception ex)
				{
					return new AuthLDAP_ADReturnObject(false, ex.Message);
				}
				finally
				{
					de.Close();
				}
			}
			catch (Exception e)
			{
				return new AuthLDAP_ADReturnObject(false, "\r\nUnexpected exception occured:\r\n\t" + e.GetType() + ":" + e.Message);
			}
		}
		public AuthLDAP_ADReturnObject GetAuthLDAP_AD_Filter(string login, string password, string filter)
		{
			try
			{
				using DirectoryEntry de = new(Path, login, password);
				using DirectorySearcher searcher = new(de);
				searcher.Filter = $"(&(objectClass=user)(sAMAccountName={login})({filter}))";

				try
				{
					SearchResultCollection res = searcher.FindAll();
					Console.WriteLine(searcher.Filter);

					if (res == null || res.Count == 0)
					{
						return new AuthLDAP_ADReturnObject(false, "This user is *NOT* member of that group");
					}
					else
					{
						return new AuthLDAP_ADReturnObject(true, "This user is INDEED a member of that group");
					}
				}
				catch (Exception ex)
				{
					return new AuthLDAP_ADReturnObject(false, ex.Message);
				}
				finally
				{
					de.Close();
				}
			}
			catch (Exception e)
			{
				return new AuthLDAP_ADReturnObject(false, "\r\nUnexpected exception occured:\r\n\t" + e.GetType() + ":" + e.Message);
			}
		}
	}
}