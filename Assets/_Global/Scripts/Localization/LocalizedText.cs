///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 19/05/2018 22:53
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Fr.ElasticTower.ElasticTower.Scripts.Localization {
    public class LocalizedText : MonoBehaviour {

		public string key;
		protected Text text;
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID
		private void Start () {
			LocalizationManager.instance.OnLoadLocalizedText += Instance_OnLoadLocalizedText;
			text = GetComponent<Text>();
			Instance_OnLoadLocalizedText();
		}

		private void Instance_OnLoadLocalizedText()
		{
			text.text = LocalizationManager.instance.GetLocalizedValue(key);
		}
#endif
	}
}



