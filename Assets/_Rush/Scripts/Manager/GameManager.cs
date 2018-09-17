///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 24/04/2018 18:54
///-----------------------------------------------------------------

using Fr.Sebastien.Rush.Rush.Scripts.ActionBlocks;
using Fr.Sebastien.Rush.Rush.Scripts.UI;
using System;
using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scripts {
    public class GameManager : MonoBehaviour {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }

		protected int cubeSpawning = 0;
		protected int cubeDestroying = 0;
		public bool isStart = false;

		public static event Action OnBuildGame;
		public static event Action OnStartingGame;
		public static event Action OnWinGame;

		private void Awake(){
            if (_instance){
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void Start () {
			Spawner.OnCreateCube += Spawner_OnCreateCube;
			CubeMove.OnFinish += CubeMove_OnFinish;

			Hud.OnStartingGame += Hud_OnStartingGame;
			ResumeAndRetryScreen.OnRetryGame += ResumeAndRetryScreen_OnRetryGame;
			ResumeAndRetryScreen.OnBackHome += ResumeAndRetryScreen_OnBackHome;
		}

		public void StartLevel()
		{
			ResetGame();
		}

		private void ResumeAndRetryScreen_OnRetryGame()
		{
			ResetGame();
		}

		private void ResumeAndRetryScreen_OnBackHome()
		{
			ResetGame();
		}

		private void Hud_OnStartingGame()
		{
			StartGame();
		}

		private void Spawner_OnCreateCube()
		{
			cubeSpawning++;
		}

		private void CubeMove_OnFinish()
		{
			cubeDestroying++;

			if (cubeDestroying >= cubeSpawning)
				Victory();
		}

		protected void Update()
		{
			if (Input.GetKeyUp(KeyCode.Space) && !isStart)
				StartGame();
		}

		public void StartGame()
		{
			isStart = true;
			if (OnStartingGame != null)
				OnStartingGame();
		}

		protected void Victory()
		{
			if (OnWinGame != null)
				OnWinGame();
		}

		protected void ResetGame()
		{
			Time.timeScale = 1;
			cubeSpawning = 0;
			cubeDestroying = 0;
			isStart = false;
			
			foreach (var cube in GameObject.FindGameObjectsWithTag("Cube"))
			{
				Destroy(cube);
			}
			foreach (var block in GameObject.FindGameObjectsWithTag("Block"))
			{
				Destroy(block);
			}

			if (OnBuildGame != null)
				OnBuildGame();
		}

        private void OnDestroy(){
            if (this == _instance)
                _instance = null;

			Spawner.OnCreateCube -= Spawner_OnCreateCube;
			CubeMove.OnFinish -= CubeMove_OnFinish;

			Hud.OnStartingGame -= Hud_OnStartingGame;
			ResumeAndRetryScreen.OnRetryGame -= ResumeAndRetryScreen_OnRetryGame;
			ResumeAndRetryScreen.OnBackHome -= ResumeAndRetryScreen_OnBackHome;
		}
    }
}



