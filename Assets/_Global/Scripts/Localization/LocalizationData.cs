///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 19/05/2018 21:21
///-----------------------------------------------------------------

namespace Fr.ElasticTower.ElasticTower.Scripts.Localization {
	[System.Serializable]
	public class LocalizationData
	{
		public LocalizationItem[] items;
	}

	[System.Serializable]
	public class LocalizationItem
	{
		public string key;
		public string value;
	}
}



