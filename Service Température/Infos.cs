namespace Service_Temp
{
	public static class Infos
	{
		//public const string barrageAPI = "http://BarrageAPI:8080/api";
		public const string barrageAPI = "http://localhost:5254/api";
		public const string thermListRequest = "https://www.thermotrack-webserve.com/API/get_api.php?method=equip_list&api_key=532c-16a171e&format=xml";
		public const string thermDataRequest = "https://www.thermotrack-webserve.com/API/get_api.php?method=equip_data&api_key=532c-16a171e&equip_id=";
	}
}
