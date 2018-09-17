///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 30/04/2018 21:58
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System;

namespace Fr.Sebastien.Rush.Rush.Scripts.UI {
    public class PauseCard : ResumeAndRetryScreen
	{
		[SerializeField] protected Button resume;

		public static event Action OnResumeGame;

		override protected void Start ()
		{
			base.Start();

			resume.onClick.AddListener(delegate {
				if (OnResumeGame != null)
					OnResumeGame();
			});
		}
    }
}



