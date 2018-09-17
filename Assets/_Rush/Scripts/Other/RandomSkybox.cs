///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 14/05/2018 23:51
///-----------------------------------------------------------------

using Fr.Sebastien.Rush.Rush.Scripts.UI;
using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scripts.Other {
    public class RandomSkybox : MonoBehaviour {

		[SerializeField] protected Material[] listSkyboxs;
		protected int currentSkybox = 0;

        private void Start () {
			currentSkybox = Random.Range(0, listSkyboxs.Length - 1);
			RenderSettings.skybox = listSkyboxs[currentSkybox];

			ResumeAndRetryScreen.OnBackHome += ResumeAndRetryScreen_OnBackHome;
		}

		private void ResumeAndRetryScreen_OnBackHome()
		{
			ChangeSkybox();
		}

		private void ChangeSkybox () {
			if (Random.Range(0f, 1f) < 0.5f)
				return;

			currentSkybox++;
			if (currentSkybox == listSkyboxs.Length)
				currentSkybox = 0;

			RenderSettings.skybox = listSkyboxs[currentSkybox];
		}

		private void OnDestroy()
		{
			ResumeAndRetryScreen.OnBackHome -= ResumeAndRetryScreen_OnBackHome;
		}
	}
}



