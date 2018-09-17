///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 27/04/2018 12:46
///-----------------------------------------------------------------

using Fr.Sebastien.Rush.Rush.Scripts.UI;
using System;
using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scripts {
    public class Metronome : MonoBehaviour {
        private static Metronome _instance;
        public static Metronome Instance { get { return _instance; } }

		public static event Action<float> OnUpdateTimeRatio;
		public static event Action OnTick;
		protected float elapsedTime = 0;
		protected float timeRatio = 0;
		public float timeRef = 0.5f;

		[SerializeField] protected bool isStart;

		private void Awake(){
            if (_instance){
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void Start () {
			ResumeAndRetryScreen.OnBackHome += ResumeAndRetryScreen_OnBackHome;
            Hud.OnChangeTimeScale += Hud_OnChangeTimeScale;
			Hud.OnStartingGame += Hud_OnStartingGame;
			GameManager.OnStartingGame += Hud_OnStartingGame;
			CubeMove.OnPause += CubeMove_OnPause;

			Hud.OnPauseGame += Hud_OnPauseGame;
			PauseCard.OnRetryGame += PauseCard_OnRetryGame;
			isStart = false;

		}

		private void ResumeAndRetryScreen_OnBackHome()
		{
			isStart = false;
		}

		private void CubeMove_OnPause()
		{
			isStart = false;
		}

		private void CubeMove_OnDeath()
		{
			isStart = false;
		}

		private void PauseCard_OnRetryGame()
		{
			elapsedTime = 0;
			timeRatio = 0;
			isStart = false;
		}

		private void Hud_OnPauseGame()
		{
			isStart = false;
		}

		private void Hud_OnStartingGame()
		{
			isStart = true;
		}

		private void Hud_OnChangeTimeScale(float pValue)
		{
			timeRef = pValue;
		}

		private void Update()
		{
			if (!isStart)
				return;

			elapsedTime += Time.deltaTime;
			timeRatio = elapsedTime / timeRef;

			if (OnUpdateTimeRatio != null)
				OnUpdateTimeRatio(timeRatio);

			if (elapsedTime >= timeRef)
			{
				elapsedTime = elapsedTime % timeRef;
				if (OnTick != null)
					OnTick();
			}
		}

		private void OnDestroy(){
            if (this == _instance)
                _instance = null;

			ResumeAndRetryScreen.OnBackHome -= ResumeAndRetryScreen_OnBackHome;
			Hud.OnChangeTimeScale -= Hud_OnChangeTimeScale;
			Hud.OnStartingGame -= Hud_OnStartingGame;
			GameManager.OnStartingGame -= Hud_OnStartingGame;
			CubeMove.OnPause -= CubeMove_OnPause;

			Hud.OnPauseGame -= Hud_OnPauseGame;
			PauseCard.OnRetryGame -= PauseCard_OnRetryGame;
		}
    }
}



