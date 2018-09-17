///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 07/06/2018 12:35
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Fr.Sebastien.Rush.Rush.UI {
	[RequireComponent(typeof(Button))]
    public class PlaySoundOnClickButton : MonoBehaviour {

		[SerializeField] protected AudioClip clickAudio;
		protected Button button;
		[SerializeField] protected AudioSource audioSource;

        private void Start () {
			button = GetComponent<Button>();
			audioSource.playOnAwake = false;
			button.onClick.AddListener(Button_OnClick);
        }

        private void Button_OnClick() {
			audioSource.PlayOneShot(clickAudio);
		}
    }
}



