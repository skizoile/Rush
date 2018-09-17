///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 19/05/2018 21:30
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using System;
using UnityEngine.Networking;

namespace Fr.ElasticTower.ElasticTower.Scripts.Localization {

    public class LocalizationManager : MonoBehaviour {

		public static LocalizationManager instance;

		public event Action OnLoadLocalizedText;

		private Dictionary<string, string> localizedText;
		private bool isReady = false;
		private string missingTextString = "Localized text not found";

		protected string filePath = "";
		protected string dataAsJson = "";

		private void Awake () {
            if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy(gameObject);

			DontDestroyOnLoad(gameObject);
			PlayerPrefs.DeleteAll();
			if (PlayerPrefs.HasKey("langue"))
				LoadLocalizedText(PlayerPrefs.GetString("langue"));
			else
				LoadLocalizedText("localizedText_en.json");

        }
		
		public void LoadLocalizedText(string fileName)
		{
			localizedText = new Dictionary<string, string>();
			filePath = Path.Combine(Application.streamingAssetsPath, fileName);

			if (File.Exists(filePath) || Application.platform == RuntimePlatform.Android)
			{
				StartCoroutine(FindFile());
			}
			else{
				Debug.LogError("Cannot find file ! Path = " + filePath);
			}

			isReady = true;
			
		}

		protected IEnumerator FindFile()
		{

			if (filePath.Contains("jar:file:/"))
			{
				UnityWebRequest www = UnityWebRequest.Get(filePath);
				yield return www.SendWebRequest();
				dataAsJson = www.downloadHandler.text;
			}
			else
			{
				dataAsJson = File.ReadAllText(filePath);
			}

			LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

			for (int i = 0; i < loadedData.items.Length; i++)
			{
				localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
			}

			if (OnLoadLocalizedText != null)
				OnLoadLocalizedText();
		}


		public string GetLocalizedValue(string key)
		{
			string result = missingTextString;
			if (localizedText.ContainsKey(key))
				result = localizedText[key];

			return result;
		}

		public bool GetIsReady()
		{
			return isReady;
		}


		protected void OnDestroy()
		{
			instance = null;
		}
	}
}



