///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 01/05/2018 17:09
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System;

namespace Fr.Sebastien.Rush.Rush.Scripts.UI {
    public class ResumeAndRetryScreen : MonoBehaviour {
        private static ResumeAndRetryScreen _instance;

		[SerializeField] protected Button retry;
		[SerializeField] protected Button home;

		public static event Action OnRetryGame;
		public static event Action OnBackHome;


        virtual protected void Start () {
			retry.onClick.AddListener(delegate {
				if (OnRetryGame != null)
					OnRetryGame();
			});

			home.onClick.AddListener(delegate {
				if (OnBackHome != null)
					OnBackHome();
			});
		}

        protected void OnDestroy(){

        }
    }
}



