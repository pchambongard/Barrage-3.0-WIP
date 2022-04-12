namespace WebSite
{
	public static class Infos
	{
		public const string barrageAPI = "http://BarrageAPI:8080/api";
		//public const string barrageAPI = "http://localhost:5254/api";
		public const string authldapadAPI = "http://authldapadAPI:8080/api/Session/";
		//public const string authldapadAPI = "http://localhost:5114/api";
		public const string authSearchBase = "LDAP://auth.intra.cg30.fr:389/DC=intra,DC=cg30,DC=fr";
		public const string authFilter = "memberOf=CN=BARRAGE,OU=Globaux-Fonctionnels,OU=Groupes-globaux,OU=CG30-Groupes,OU=CG30-Usr-Grp,DC=intra,DC=cg30,DC=fr";
	}
}
