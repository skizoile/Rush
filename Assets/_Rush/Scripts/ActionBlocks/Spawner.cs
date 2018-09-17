///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 24/04/2018 13:00
///-----------------------------------------------------------------

using UnityEngine;
using System;
using Fr.Sebastien.Rush.Rush.Scripts.UI;
using System.Collections;

namespace Fr.Sebastien.Rush.Rush.Scripts.ActionBlocks
{
    public class Spawner : Block
	{

		[SerializeField] protected GameObject prefabCube;
		[SerializeField] protected int nbCubeAtSpawn;
		protected float tickCount = 0;
		[SerializeField] protected float tickSpawn = 8;
		[SerializeField] protected float tickFirstSpawn = 3;

		protected bool firstSpawn;

		private float currentCubeSpawn;

		public static event Action OnCreateCube;

		[SerializeField] private Color colorSpawn;
		[SerializeField] private MeshRenderer directionalSign;
		[SerializeField] private MeshRenderer endBlockRenderer;
		[SerializeField] private MeshRenderer endBlockSecondaryRenderer;
		[SerializeField] private MeshRenderer flyingEndBlock;

		[SerializeField] protected bool isStart = false;

		protected void Awake()
		{
			transform.localScale = new Vector3(1, 1, 1);
		}

		override protected void Start () {
			currentCubeSpawn = 0;
			directionalSign.material.color = colorSpawn;
			endBlockRenderer.material.color = colorSpawn;
			if (endBlockSecondaryRenderer != null)
				endBlockSecondaryRenderer.material.color = colorSpawn;
			firstSpawn = true;

			Metronome.OnTick += Metronome_OnTick;
			Hud.OnStartingGame += Hud_OnStartingGame;
			GameManager.OnStartingGame += Hud_OnStartingGame;

			CubeMove.OnPause += CubeMove_OnPause;

			Hud.OnPauseGame += Hud_OnPauseGame;
			PauseCard.OnResumeGame += PauseCard_OnResumeGame;
			ResumeAndRetryScreen.OnRetryGame += ResumeAndRetryScreen_OnRetryGame;
			ResumeAndRetryScreen.OnBackHome += ResumeAndRetryScreen_OnRetryGame;
		}

		private void CubeMove_OnPause()
		{
			isStart = false;
		}

		protected void ResumeAndRetryScreen_OnRetryGame()
		{
			isStart = false;
			currentCubeSpawn = 0;
			firstSpawn = true;
		}

		private void PauseCard_OnResumeGame()
		{
			isStart = true;
		}

		private void Hud_OnPauseGame()
		{
			isStart = false;
		}

		private void Hud_OnStartingGame()
		{
			isStart = true;
		}

		private void Metronome_OnTick()
		{
			if (currentCubeSpawn >= nbCubeAtSpawn || !isStart)
				return;

			tickCount += 1;

			if (firstSpawn && tickCount >= tickFirstSpawn && isStart)
			{
				tickCount = 0;
				CreateNewCube();
				firstSpawn = false;
			}

			if (!firstSpawn && tickCount >= tickSpawn && isStart)
			{
				tickCount = 0;
				CreateNewCube();
			}
		}

		protected void CreateNewCube()
		{
			Vector3 newPos = transform.position;
			newPos.y += 1;
			GameObject newCube = Instantiate(prefabCube, newPos, transform.rotation);
			MeshRenderer cubeRenderer = newCube.GetComponent<MeshRenderer>();
			cubeRenderer.material.SetColor("_Color", colorSpawn);
			newCube.GetComponent<CubeMove>().Spawner = gameObject;
			currentCubeSpawn++;
			if (OnCreateCube != null)
				OnCreateCube();
		}

		override protected void OnDestroy()
		{
			Metronome.OnTick -= Metronome_OnTick;
			Hud.OnStartingGame -= Hud_OnStartingGame;
			GameManager.OnStartingGame -= Hud_OnStartingGame;

			CubeMove.OnPause -= CubeMove_OnPause;

			Hud.OnPauseGame -= Hud_OnPauseGame;
			PauseCard.OnResumeGame -= PauseCard_OnResumeGame;
			ResumeAndRetryScreen.OnRetryGame -= ResumeAndRetryScreen_OnRetryGame;
			ResumeAndRetryScreen.OnBackHome -= ResumeAndRetryScreen_OnRetryGame;
		}
	}
}



