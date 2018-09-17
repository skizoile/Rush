///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 30/04/2018 15:10
///-----------------------------------------------------------------

using Fr.Sebastien.Rush.Rush.Scripts.Other;
using Fr.Sebastien.Rush.Rush.Scripts.UI;
using Fr.Sebastien.Rush.Rush.Scripts.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scripts.Manager {

	public enum BlockList
	{
		NONE,
		ARROW_0,
		ARROW_90,
		ARROW_180,
		ARROW_270,
		CONVOYOR_0,
		CONVOYOR_90,
		CONVOYOR_180,
		CONVOYOR_270,
		STOP,
		SPLITS
	}

    public class EditorMode : MonoBehaviour {

		public static EditorMode instance;

		[SerializeField] protected List<ActionBlock> listAction = new List<ActionBlock>();
		[SerializeField] private GameObject prefabMeteore;
		[SerializeField] private LayerMask layerEnvironnement;
		[SerializeField] private LayerMask layerBlock;
		protected Dictionary<BlockList, ActionBlock> dictionaryBlock = new Dictionary<BlockList, ActionBlock>();
		protected Dictionary<GameObject, ActionBlock> blockUse = new Dictionary<GameObject, ActionBlock>();
		protected new Camera camera;
		protected BlockList currentBlock;
		protected GameObject selector;
		protected int idBlock;
		protected List<BlockList> listBlockType = new List<BlockList>();
		protected List<BlockList> listBlockEmpty = new List<BlockList>();
		protected bool cameraMoved = false;

		public static event Action<Dictionary<BlockList, ActionBlock>, BlockList> OnInventoryChange;
		public static event Action OnActiveCamera;

		protected bool isStart;

		private void Awake()
		{
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy(gameObject);
		}

		private void Start () {
			camera = Camera.main;

			ResetEditorMode();

			Hud.OnStartingGame += Hud_OnStartingGame;
			Hud.OnPauseGame += Hud_OnPauseGame;
			GameManager.OnStartingGame += Hud_OnStartingGame;
			SateliteCamera.OnMoveCamera += SateliteCamera_OnMoveCamera;
			SateliteCamera.OnDontMoveCamera += SateliteCamera_OnDontMoveCamera;
			PauseCard.OnResumeGame += PauseCard_OnResumeGame;
			ResumeAndRetryScreen.OnBackHome += ResumeAndRetryScreen_OnRetryGame;
			ResumeAndRetryScreen.OnRetryGame += ResumeAndRetryScreen_OnRetryGame;
        }

		private void PauseCard_OnResumeGame()
		{
			isStart = false;
		}

		private void SateliteCamera_OnDontMoveCamera()
		{
			cameraMoved = false;
		}

		private void SateliteCamera_OnMoveCamera(float obj2)
		{
			cameraMoved = true;
		}

		private void ResumeAndRetryScreen_OnRetryGame()
		{
			ResetEditorMode();
		}

		private void Hud_OnPauseGame()
		{
			isStart = true;
		}

		private void Hud_OnStartingGame()
		{
			isStart = true;
		}

		private void Hud_OnChangeItem(BlockList newItem)
		{
			currentBlock = newItem;
			SelectBlock();
		}

		private void SelectNewTypeBlock()
		{
			listBlockType.Clear();

			foreach (BlockList value in dictionaryBlock.Keys)
			{
				if (!listBlockEmpty.Contains(value))
					listBlockType.Add(value);
			}

			if (listBlockType.Count != 0)
			{
				currentBlock = listBlockType[0];
				SelectBlock();
			}
			else
			{
				Destroy(selector);
			}
		}

		private void SelectBlock()
		{
			if (currentBlock == BlockList.NONE && listAction.Count != 0)
				currentBlock = listAction[0].type;

			if (selector != null)
				Destroy(selector);

			ActionBlock actionBlock = dictionaryBlock[currentBlock];

			if (actionBlock.nbLeft > 0)
				selector = Instantiate(actionBlock.prefabSelector);
			else
			{
				listBlockEmpty.Add(actionBlock.type);
				SelectNewTypeBlock();
			}
		}
		
		public void ChangeItem(BlockList pBlockList)
		{
			currentBlock = pBlockList;
			SelectBlock();
		}

		private void PlaceBlock()
		{
			ActionBlock actionBlock = dictionaryBlock[currentBlock];

			actionBlock.nbLeft--;
			GameObject newBlock = Instantiate(actionBlock.prefabGame);
			newBlock.transform.position = selector.transform.position;
			GameObject meteore = Instantiate(prefabMeteore);
			meteore.GetComponent<Meteores>().SetEndPos(selector.transform.position);

			blockUse.Add(newBlock, actionBlock);

			SelectBlock();

			if (OnInventoryChange != null)
				OnInventoryChange(dictionaryBlock, currentBlock);
		}

		private void RemoveBlock(GameObject pGameObject)
		{
			if (!blockUse.ContainsKey(pGameObject))
				return;

			ActionBlock actionBlock = dictionaryBlock[blockUse[pGameObject].type];


			if (actionBlock.prefabGame == blockUse[pGameObject].prefabGame)
			{
				actionBlock.nbLeft++;
				blockUse.Remove(pGameObject);
				Destroy(pGameObject);
				Destroy(selector);

				if (listBlockEmpty.Contains(currentBlock))
					listBlockEmpty.Remove(currentBlock);

				currentBlock = actionBlock.type;

				if (OnInventoryChange != null)
					OnInventoryChange(dictionaryBlock, currentBlock);

				SelectBlock();
			}
		}

		private void Update()
		{
			if (isStart)
				return;

			if (Input.GetKeyDown(KeyCode.R))
				ResetEditorMode();

			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			Debug.DrawRay(camera.transform.position, ray.direction * 1000);

			if (Physics.Raycast(ray, out hit, 20f, layerBlock) && Input.GetMouseButtonUp(0))
			{
				RemoveBlock(hit.collider.gameObject);
				return;
			}

			if (Physics.Raycast(ray, out hit, 20f, layerEnvironnement) && !Physics.Raycast(ray, 20f, layerBlock) && selector != null)
			{
				selector.transform.GetChild(0).gameObject.SetActive(true);
				if (selector.transform.childCount == 2)
					selector.transform.GetChild(1).gameObject.SetActive(true);

				selector.transform.position = hit.collider.transform.position;

				if (Input.GetMouseButtonUp(0))
					PlaceBlock();
			}
			else
			{
				if (selector != null)
				{
					selector.transform.GetChild(0).gameObject.SetActive(false);
					if (selector.transform.childCount == 2)
						selector.transform.GetChild(1).gameObject.SetActive(false);
				}
			}

#if UNITY_ANDROID
			if (Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				if (Physics.Raycast(ray, out hit, 20f, layerEnvironnement) && !Physics.Raycast(ray, 20f, layerBlock) && selector != null)
				{
					selector.transform.GetChild(0).gameObject.SetActive(true);
					if (selector.transform.childCount == 2)
							selector.transform.GetChild(1).gameObject.SetActive(true);

					selector.transform.position = hit.collider.transform.position;

					if (Input.GetTouch(0).phase == TouchPhase.Ended)
					{
						PlaceBlock();
						return;
					}
				}
				else
				{
					if (selector != null)
					{
						selector.transform.GetChild(0).gameObject.SetActive(false);
						if (selector.transform.childCount == 2)
							selector.transform.GetChild(1).gameObject.SetActive(false);
					}
				}
			}
#endif
		}

		private void ResetEditorMode()
		{
			if (selector != null)
				Destroy(selector);

			isStart = false;
			currentBlock = BlockList.NONE;

			dictionaryBlock.Clear();
			listBlockEmpty.Clear();
			foreach (var block in blockUse.Keys)
			{
				Destroy(block);
			}

			foreach (var actionBlock in listAction)
			{
				if (!dictionaryBlock.ContainsKey(actionBlock.type))
					dictionaryBlock.Add(actionBlock.type, actionBlock);

				actionBlock.nbLeft = actionBlock.nbMax;
			}

			SelectBlock();

			if (OnInventoryChange != null)
				OnInventoryChange(dictionaryBlock, currentBlock);

			if (OnActiveCamera != null)
				OnActiveCamera();

		}

		protected void OnDestroy()
		{
			Hud.OnStartingGame -= Hud_OnStartingGame;
			Hud.OnPauseGame -= Hud_OnPauseGame;
			GameManager.OnStartingGame -= Hud_OnStartingGame;
			SateliteCamera.OnMoveCamera -= SateliteCamera_OnMoveCamera;
			SateliteCamera.OnDontMoveCamera -= SateliteCamera_OnDontMoveCamera;
			PauseCard.OnResumeGame -= PauseCard_OnResumeGame;
			ResumeAndRetryScreen.OnBackHome -= ResumeAndRetryScreen_OnRetryGame;
			ResumeAndRetryScreen.OnRetryGame -= ResumeAndRetryScreen_OnRetryGame;
			if (selector != null)
				Destroy(selector);

			instance = null;
		}
	}
}