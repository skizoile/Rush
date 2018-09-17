///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 24/05/2018 14:34
///-----------------------------------------------------------------

using Fr.ElasticTower.ElasticTower.Scripts.Localization;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Fr.Sebastien.Rush.Rush.UI {
    public class OptionScreen : MonoBehaviour {
        private static OptionScreen _instance;
        public static OptionScreen Instance { get { return _instance; } }

		[SerializeField] protected Button validBtn;

		[SerializeField] protected Button btnFR;
		[SerializeField] protected Button btnEN;

		[SerializeField] protected Slider sliderMaster;
		[SerializeField] protected Slider sliderMusic;
		[SerializeField] protected Slider sliderSFX;

		[SerializeField] protected string masterVolumeName;
		[SerializeField] protected string musicVolumeName;
		[SerializeField] protected string sfxVolumeName;

		[SerializeField] protected AudioMixer audioMixer;

		[SerializeField] protected string nameFileLocalizeFr;
		[SerializeField] protected string nameFileLocalizeEn;

        private void Awake(){
            if (_instance){
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void Start () {
			validBtn.onClick.AddListener(Validation_OnClick);
			btnFR.onClick.AddListener(BtnFrench_OnClick);
			btnFR.onClick.AddListener(BtnEnglish_OnClick);
			sliderMaster.onValueChanged.AddListener(SliderMaster_OnValueChanged);
			sliderMusic.onValueChanged.AddListener(SliderMusic_OnValueChanged);
			sliderSFX.onValueChanged.AddListener(SliderSFX_OnValueChanged);

			if (PlayerPrefs.HasKey(masterVolumeName))
				sliderMaster.value = PlayerPrefs.GetFloat(masterVolumeName);

			if (PlayerPrefs.HasKey(musicVolumeName))
				sliderMusic.value = PlayerPrefs.GetFloat(musicVolumeName);

			if (PlayerPrefs.HasKey(sfxVolumeName))
				sliderSFX.value = PlayerPrefs.GetFloat(sfxVolumeName);
		}

		private void Validation_OnClick()
		{
			PlayerPrefs.SetFloat(masterVolumeName, sliderMaster.value);

			PlayerPrefs.SetFloat(musicVolumeName, sliderMusic.value);

			PlayerPrefs.SetFloat(sfxVolumeName, sliderSFX.value);
		}

		private void BtnFrench_OnClick()
		{
			PlayerPrefs.SetString("langue", nameFileLocalizeFr);
			LocalizationManager.instance.LoadLocalizedText(nameFileLocalizeFr);
		}

		private void BtnEnglish_OnClick()
		{
			PlayerPrefs.SetString("langue", nameFileLocalizeEn);
			LocalizationManager.instance.LoadLocalizedText(nameFileLocalizeEn);
		}

		private void SliderMaster_OnValueChanged(float pValue)
		{
			audioMixer.SetFloat(masterVolumeName, pValue);
		}

		private void SliderMusic_OnValueChanged(float pValue)
		{
			audioMixer.SetFloat(musicVolumeName, pValue);
		}

		private void SliderSFX_OnValueChanged(float pValue)
		{
			audioMixer.SetFloat(sfxVolumeName, pValue);
		}

        private void OnDestroy(){
            if (this == _instance)
                _instance = null;
        }
    }
}



