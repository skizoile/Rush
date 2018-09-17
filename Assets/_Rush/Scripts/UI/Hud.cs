///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 24/04/2018 19:57
///-----------------------------------------------------------------

using UnityEngine.UI;
using UnityEngine;
using System;
using Fr.Sebastien.Rush.Rush.Scripts.Manager;
using System.Collections.Generic;
using Fr.Sebastien.Rush.Rush.Scripts.Utils;
using Fr.Sebastien.Rush.Rush.Scripts.UI;
using UnityEngine.EventSystems;

namespace Fr.Sebastien.Rush.Rush.Scripts {
    public class Hud : MonoBehaviour {
        private static Hud _instance;
        public static Hud Instance { get { return _instance; } }

		[SerializeField] protected Button playMode;
		[SerializeField] protected Button pauseMode;
		[SerializeField] protected Slider timeScale;
		[SerializeField] protected Button inventoryImage;
		[SerializeField] protected Transform container;
		//[SerializeField] protected GameObject indicator;

		public static event Action OnStartingGame;
		public static event Action OnPauseGame;
		public static event Action<float> OnChangeTimeScale;

		public static event Action OnMoveSlider;
		public static event Action OnDontMoveSlider;

		protected float baseValue;

		protected List<Button> listButton = new List<Button>();

		protected EventSystem eventSystem;

        private void Awake(){
            if (_instance){
                Destroy(gameObject);
                return;
            }
			baseValue = timeScale.value;
			_instance = this;

			eventSystem = EventSystem.current;

			EditorMode.OnInventoryChange += EditorMode_OnInventoryChange;
			SateliteCamera.OnMoveCamera += SateliteCamera_OnMoveCamera;
			ResumeAndRetryScreen.OnRetryGame += ResumeAndRetryScreen_OnRetryGame;
			ResumeAndRetryScreen.OnBackHome += ResumeAndRetryScreen_OnRetryGame;
        }

		private void SateliteCamera_OnMoveCamera(float pPhi)
		{
			foreach (var icon in listButton)
			{
				icon.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, pPhi - 90);
			}
		}

		private void ResumeAndRetryScreen_OnRetryGame()
		{
			timeScale.value = baseValue;
		}

		private void Start () {

			if (OnChangeTimeScale != null)
				OnChangeTimeScale(timeScale.value);

			timeScale.onValueChanged.AddListener(delegate {
				if (OnChangeTimeScale != null)
					OnChangeTimeScale(timeScale.value);
			});

			playMode.onClick.AddListener(delegate {
				if (OnStartingGame != null)
					OnStartingGame();
			});

			pauseMode.onClick.AddListener(delegate {
				if (OnPauseGame != null)
					OnPauseGame();
			});

		}

		private void EditorMode_OnInventoryChange(Dictionary<BlockList, ActionBlock> pList, BlockList pCurrentBlock)
		{
			foreach (var button in listButton)
			{
				Destroy(button.gameObject);
			}

			listButton.Clear();

			Dictionary<Button, ActionBlock> lList = new Dictionary<Button, ActionBlock>();

			foreach (var item in pList)
			{
				Button lButtonInventory = Instantiate(inventoryImage);
				lButtonInventory.transform.SetParent(container, false);
				lButtonInventory.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.Value.icon;
				lButtonInventory.transform.GetChild(1).GetComponent<Text>().text = item.Value.nbLeft.ToString();
				lButtonInventory.onClick.AddListener(() => Inventory_OnClick(lButtonInventory.transform, item.Value));
				lButtonInventory.transform.GetComponentInChildren<Outline>().effectDistance = Vector2.zero;

				if (item.Value.nbLeft >= 1)
					lList.Add(lButtonInventory, item.Value);

				listButton.Add(lButtonInventory);
			}

			if (lList.Count == 0)
			{
				for (int i = 0; i < container.childCount; i++)
				{
					container.GetChild(i).GetComponentInChildren<Outline>().effectDistance = Vector2.zero;
				}

				return;
			}

			foreach (var item in lList.Keys)
			{
				Debug.Log(lList[item].type + " :: " + pCurrentBlock);
				if (lList[item].type == pCurrentBlock)
				{
					for (int i = 0; i < container.childCount; i++)
					{
						container.GetChild(i).GetComponentInChildren<Outline>().effectDistance = Vector2.zero;
					}
					item.GetComponentInChildren<Outline>().effectDistance = new Vector2(8, 8);
					return;
				}
			}
		}

		protected void Inventory_OnClick(Transform pPos, ActionBlock pValue)
		{
			if (pValue.nbLeft == 0)
				return;
			
			for (int i = 0; i < container.childCount; i++)
			{
				container.GetChild(i).GetComponentInChildren<Outline>().effectDistance = Vector2.zero;
			}
			pPos.GetComponentInChildren<Outline>().effectDistance = new Vector2(8, 8);

			EditorMode.instance.ChangeItem(pValue.type);
		}

		protected void Update()
		{
#if UNITY_ANDROID
			 if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
			{
				// Check if the mouse was clicked over a UI element
				if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
				{
					if (OnMoveSlider != null)
						OnMoveSlider();
				}else{
					if (OnDontMoveSlider != null)
						OnDontMoveSlider();
				}
			}
#endif
		}

		private void OnDestroy(){
            if (this == _instance)
                _instance = null;

			EditorMode.OnInventoryChange -= EditorMode_OnInventoryChange;
			SateliteCamera.OnMoveCamera -= SateliteCamera_OnMoveCamera;
			ResumeAndRetryScreen.OnRetryGame -= ResumeAndRetryScreen_OnRetryGame;
			ResumeAndRetryScreen.OnBackHome -= ResumeAndRetryScreen_OnRetryGame;
		}
    }
}



