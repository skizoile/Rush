///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 29/04/2018 23:49
///-----------------------------------------------------------------

using Fr.Sebastien.Rush.Rush.Scripts.Manager;
using Fr.Sebastien.Rush.Rush.Scripts.Other;
using Fr.Sebastien.Rush.Rush.Scripts.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scripts {
    public class SateliteCamera : MonoBehaviour {

		[SerializeField] protected float movementSpeed = 3f;
		protected float movementSpeedMobile = 0.3f;
		[SerializeField] protected Transform origin;
		protected Vector3 originPos;
		[SerializeField] protected float distanceOrigin = 15;
		protected float distance;

		protected Quaternion baseRotation;

		protected float angleTheta = 0;
		protected float anglePhi = 360;

		protected float lastTheta = 0;
		protected float lastPhi = 0;

		public static event Action<float> OnMoveCamera;
		public static event Action OnDontMoveCamera;
		
		[SerializeField] protected bool isStart;

		private void Start () {
			baseRotation = transform.rotation;
			distance = distanceOrigin;
			isStart = false;
			originPos = (origin == null) ? new Vector3() : origin.transform.position;
			transform.position = SetPos(angleTheta, anglePhi);

			GameManager.OnBuildGame += GameManager_OnBuildGame;
			Hud.OnPauseGame += Hud_OnPauseGame;
			PauseCard.OnResumeGame += PauseCard_OnResumeGame;
			GameManager.OnWinGame += GameManager_OnWinGame;
			DeathIndicator.OnDeath += DeathIndicator_OnDeath;
			EditorMode.OnActiveCamera += EditorMode_OnActiveCamera;
			EditorMode.OnInventoryChange += EditorMode_OnInventoryChange;
			ResumeAndRetryScreen.OnBackHome += ResumeAndRetryScreen_OnBackHome;
			ResumeAndRetryScreen.OnRetryGame += ResumeAndRetryScreen_OnRetryGame;
#if UNITY_ANDROID
			Hud.OnMoveSlider += Hud_OnMoveSlider;
			Hud.OnDontMoveSlider += Hud_OnDontMoveSlider;
#endif
		}

		private void EditorMode_OnInventoryChange(Dictionary<BlockList, Utils.ActionBlock> obj, BlockList pCurrentBlock)
		{
			if (OnMoveCamera != null)
				OnMoveCamera(-anglePhi);
		}

		private void Hud_OnMoveSlider()
		{
			isStart = false;
		}

		private void Hud_OnDontMoveSlider()
		{
			isStart = true;
		}

		private void EditorMode_OnActiveCamera()
		{
			if (OnMoveCamera != null)
				OnMoveCamera(-anglePhi);
		}

		private void ResumeAndRetryScreen_OnRetryGame()
		{
			ResetCamera(true);
		}

		private void ResumeAndRetryScreen_OnBackHome()
		{
			ResetCamera(false);
		}

		private void DeathIndicator_OnDeath()
		{
			isStart = false;
		}

		private void GameManager_OnWinGame()
		{
			isStart = false;
		}

		private void PauseCard_OnResumeGame()
		{
			isStart = true;
		}

		private void Hud_OnPauseGame()
		{
			isStart = false;
		}

		private void GameManager_OnBuildGame()
		{
			isStart = true;
		}

		private Vector3 SetPos(float pThetaDeg, float pPhiDeg)
		{
			Vector3 newPos;

			float thetaRad = Mathf.Deg2Rad * pThetaDeg;
			float phiRad = Mathf.Deg2Rad * pPhiDeg;

			float posX = distance * Mathf.Cos(thetaRad) * Mathf.Cos(phiRad);
			float posY = distance * Mathf.Cos(thetaRad) * Mathf.Sin(phiRad);
			float posZ = distance * Mathf.Sin(thetaRad);

			newPos = originPos;
			newPos.x += posX;
			newPos.y += posZ;
			newPos.z += posY;

			return newPos;
		}

		private void Update () {

			if (!isStart)
				return;

#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL
			if (Input.GetAxis("Mouse ScrollWheel") != 0f)
			{
				distance -= Input.GetAxis("Mouse ScrollWheel") * (movementSpeed * 1.5f);
				distance = Mathf.Clamp(distance, 3, 20);
			}

			anglePhi -= movementSpeed * ((Input.GetMouseButton(1)) ? Input.GetAxis("Mouse X") : -Input.GetAxis("Horizontal"));
			anglePhi = Mathf.Clamp(anglePhi, 0, 361);
			if (anglePhi == 0)
				anglePhi = 360;
			else if (anglePhi == 361)
				anglePhi = 0;

			angleTheta += movementSpeed * ((Input.GetMouseButton(1)) ? Input.GetAxis("Mouse Y") : Input.GetAxis("Vertical"));
			angleTheta = Mathf.Clamp(angleTheta, -10, 89);


			if (lastTheta != angleTheta || lastPhi != anglePhi)
			{
				if (OnMoveCamera != null)
					OnMoveCamera(-anglePhi);

				lastTheta = angleTheta;
				lastPhi = anglePhi;
			}
			else
			{
				if (OnDontMoveCamera != null)
					OnDontMoveCamera();
			}
#endif

#if UNITY_ANDROID
			if (Input.touchCount == 2)
			{
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);

				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

				distance += -deltaMagnitudeDiff * (movementSpeedMobile / 10);
				distance = Mathf.Clamp(distance, 3, 20);
			}
			else if (Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
				anglePhi += movementSpeedMobile * -touchDeltaPosition.x;
				anglePhi = Mathf.Clamp(anglePhi, 0, 360);
				if (anglePhi == 0)
					anglePhi = 360;
				else if (anglePhi == 360)
					anglePhi = 0;

				angleTheta += movementSpeedMobile * -touchDeltaPosition.y;
				angleTheta = Mathf.Clamp(angleTheta, -10, 89);

				if (lastTheta != angleTheta || lastPhi != anglePhi)
				{
					if (OnMoveCamera != null)
						OnMoveCamera(-anglePhi);

					lastTheta = angleTheta;
					lastPhi = anglePhi;
				}
				else
				{
					if (OnDontMoveCamera != null)
						OnDontMoveCamera();
				}
			}
#endif

			transform.position = SetPos(angleTheta, anglePhi);
			transform.rotation = Quaternion.LookRotation(originPos - transform.position);
		}

		protected void ResetCamera(bool pRetry)
		{
			isStart = pRetry;
			distance = distanceOrigin;
			angleTheta = 0;
			anglePhi = -90;
			transform.position = SetPos(angleTheta, anglePhi);
			transform.rotation = baseRotation;

			if (OnMoveCamera != null)
				OnMoveCamera(-anglePhi);
		}

		private void OnDestroy()
		{
			GameManager.OnBuildGame -= GameManager_OnBuildGame;
			Hud.OnPauseGame -= Hud_OnPauseGame;
			PauseCard.OnResumeGame -= PauseCard_OnResumeGame;
			GameManager.OnWinGame -= GameManager_OnWinGame;
			DeathIndicator.OnDeath -= DeathIndicator_OnDeath;
			EditorMode.OnActiveCamera -= EditorMode_OnActiveCamera;
			EditorMode.OnInventoryChange -= EditorMode_OnInventoryChange;
			ResumeAndRetryScreen.OnBackHome -= ResumeAndRetryScreen_OnBackHome;
			ResumeAndRetryScreen.OnRetryGame -= ResumeAndRetryScreen_OnRetryGame;
#if UNITY_ANDROID
			Hud.OnMoveSlider -= Hud_OnMoveSlider;
			Hud.OnDontMoveSlider -= Hud_OnDontMoveSlider;
#endif
		}
	}
}



