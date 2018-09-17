///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 23/04/2018 20:14
///-----------------------------------------------------------------

using UnityEngine;
using System;
using Fr.Sebastien.Rush.Rush.Scripts.UI;

namespace Fr.Sebastien.Rush.Rush.Scripts.ActionBlocks
{

	[RequireComponent(typeof(BoxCollider))]
	public class Block : MonoBehaviour
	{

		protected static string tagCollid = "Cube";
		protected CubeMove target;
		protected bool firstTime = true;

		virtual protected void Start()
		{
			GetComponent<BoxCollider>().isTrigger = true;
			PauseCard.OnRetryGame += PauseCard_OnRetryGame;
		}

		protected void PauseCard_OnRetryGame()
		{
			firstTime = true;
		}

		virtual protected void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag(tagCollid))
				return;

			firstTime = true;
			target = other.GetComponent<CubeMove>();
		}

		virtual public void DoAction()
		{

		}

		virtual protected void OnDestroy()
		{
			PauseCard.OnRetryGame -= PauseCard_OnRetryGame;
		}
	}
}



