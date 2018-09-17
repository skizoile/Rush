///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 29/04/2018 22:27
///-----------------------------------------------------------------

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Fr.Sebastien.Rush.Rush.Scenes;
using System;
using Fr.Sebastien.Rush.Rush.Scripts.UI;
using Fr.Sebastien.Rush.Rush.Scripts.Other;

namespace Fr.Sebastien.Rush.Rush.Scripts {
    public class UIManager : MonoBehaviour {

		[SerializeField] protected GameObject decors;

		[SerializeField] protected Canvas titleCard;
		[SerializeField] protected Canvas optionCard;
		[SerializeField] protected Canvas creditCard;
		[SerializeField] protected Canvas easyCard;
		[SerializeField] protected Canvas mediumCard;
		[SerializeField] protected Canvas hardCard;
		[SerializeField] protected Canvas pauseCard;
		[SerializeField] protected Canvas hud;
		[SerializeField] protected Canvas gameOverCard;
		[SerializeField] protected Canvas victoryCard;

		[SerializeField] protected Button buttonLevel;
		[SerializeField] protected Transform easyCardContainer;
		[SerializeField] protected Transform mediumCardContainer;
		[SerializeField] protected Transform hardCardContainer;

		[SerializeField] protected List<Levels> listLevel = new List<Levels>();

		protected GameObject levelDesign;

		private void Start () {
			Hud.OnPauseGame += Hud_OnPauseGame;
			ResumeAndRetryScreen.OnBackHome += ResumeAndRetryScreen_OnBackHome;
			PauseCard.OnResumeGame += PauseCard_OnResumeGame;
			ResumeAndRetryScreen.OnRetryGame += ResumeAndRetryScreen_OnRetryGame;
			DeathIndicator.OnDeath += DeathIndicator_OnDeath;
			GameManager.OnWinGame += GameManager_OnWinGame;

			InitEasyCard(DifficultyLevel.EASY, easyCardContainer);
			InitEasyCard(DifficultyLevel.MEDIUM, mediumCardContainer);
			InitEasyCard(DifficultyLevel.HARD, hardCardContainer);
			ActiveTitleCard();
		}

		private void GameManager_OnWinGame()
		{
			ActiveVictoryCard();
		}

		private void DeathIndicator_OnDeath()
		{
			ActiveGameOverCard();
		}

		private void ResumeAndRetryScreen_OnRetryGame()
		{
			ActiveHUD();
		}

		private void ResumeAndRetryScreen_OnBackHome()
		{
			Destroy(levelDesign);
			ActiveTitleCard();
		}

		private void PauseCard_OnResumeGame()
		{
			ActiveHUD();
		}

		private void Hud_OnPauseGame()
		{
			ActivePauseCard();
		}

		private void InitEasyCard(DifficultyLevel pDifficulty, Transform pContainer)
		{
			foreach (var level in listLevel)
			{
				if (level.difficultyLevel == pDifficulty)
				{
					Button lButton = Instantiate(buttonLevel);
					lButton.transform.GetChild(0).GetComponent<Text>().text = level.nameLevel;
					lButton.transform.SetParent(pContainer, false);
					lButton.onClick.AddListener(() => OnSelectLevel(level));
				}
			}
		}

		private void OnSelectLevel(Levels level)
		{
			levelDesign = Instantiate(level.gameObjectLevel);
			DisableAll();
			ActiveHUD();
			GameManager.Instance.StartLevel();
		}

		public void ActiveTitleCard()
		{
			DisableAll();
			decors.SetActive(true);
			titleCard.gameObject.SetActive(true);
		}

		public void ActiveOptionCard()
		{
			DisableAll();
			decors.SetActive(true);
			optionCard.gameObject.SetActive(true);
		}

		public void ActiveCreditCard()
		{
			DisableAll();
			decors.SetActive(true);
			creditCard.gameObject.SetActive(true);
		}

		public void ActiveEasyCard()
		{
			DisableAll();
			decors.SetActive(true);
			easyCard.gameObject.SetActive(true);
		}

		public void ActiveMediumCard()
		{
			DisableAll();
			decors.SetActive(true);
			mediumCard.gameObject.SetActive(true);
		}

		public void ActiveHardCard()
		{
			DisableAll();
			decors.SetActive(true);
			hardCard.gameObject.SetActive(true);
		}

		public void ActivePauseCard()
		{
			DisableAll();
			pauseCard.gameObject.SetActive(true);
		}

		public void DisablePauseCard()
		{
			pauseCard.gameObject.SetActive(false);
		}

		public void ActiveHUD()
		{
			DisableAll();
			hud.gameObject.SetActive(true);
		}

		public void DisableHUD()
		{
			hud.gameObject.SetActive(false);
		}

		private void ActiveGameOverCard()
		{
			DisableAll();
			gameOverCard.gameObject.SetActive(true);
		}

		private void ActiveVictoryCard()
		{
			DisableAll();
			victoryCard.gameObject.SetActive(true);
		}

		public void DisableAll()
		{
			decors.SetActive(false);
			titleCard.gameObject.SetActive(false);
			optionCard.gameObject.SetActive(false);
			creditCard.gameObject.SetActive(false);
			easyCard.gameObject.SetActive(false);
			mediumCard.gameObject.SetActive(false);
			hardCard.gameObject.SetActive(false);
			gameOverCard.gameObject.SetActive(false);
			victoryCard.gameObject.SetActive(false);
			DisablePauseCard();
			DisableHUD();
		}

		protected void OnDestroy()
		{
			Hud.OnPauseGame -= Hud_OnPauseGame;
			ResumeAndRetryScreen.OnBackHome -= ResumeAndRetryScreen_OnBackHome;
			PauseCard.OnResumeGame -= PauseCard_OnResumeGame;
			ResumeAndRetryScreen.OnRetryGame -= ResumeAndRetryScreen_OnRetryGame;
			DeathIndicator.OnDeath -= DeathIndicator_OnDeath;
			GameManager.OnWinGame -= GameManager_OnWinGame;
		}
	}
}



