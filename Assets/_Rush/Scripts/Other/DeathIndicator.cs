///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 23/05/2018 09:36
///-----------------------------------------------------------------

using Fr.Sebastien.Rush.Rush.Scripts.UI;
using System;
using System.Collections;
using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scripts.Other {
    public class DeathIndicator : MonoBehaviour {

		private static DeathIndicator _instance;
		public static DeathIndicator Instance { get { return _instance; } }

		[SerializeField] protected float speed = 1;
		protected Transform target;
		protected Vector3 startPos;
		protected Coroutine coroutine;

		public static event Action OnDeath;
		public static event Action<Vector3> OnPlaced;

		protected int marge = 8;

		private void Awake()
		{
			if (_instance)
			{
				Destroy(gameObject);
				return;
			}

			_instance = this;
			ResumeAndRetryScreen.OnBackHome += ResumeAndRetryScreen_OnBackHome;
			ResumeAndRetryScreen.OnRetryGame += ResumeAndRetryScreen_OnRetryGame;
		}

		private void ResumeAndRetryScreen_OnRetryGame()
		{
			Destroy(gameObject);
		}

		private void ResumeAndRetryScreen_OnBackHome()
		{
			Destroy(gameObject);
		}

		protected void Start()
		{
			if (OnPlaced != null)
			{
				OnPlaced(target.position);
			}
		}

		public void SetTargetPos(Transform posTarget)
		{
			target = posTarget;
			startPos = new Vector3(target.position.x, target.position.y + marge, target.position.z);
			transform.position = startPos;
			coroutine = StartCoroutine(Move());
		}

		private IEnumerator Move()
		{
			while (transform.position != target.position)
			{
				transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
				yield return null;
			}
			if (OnDeath != null)
				OnDeath();
			StopCoroutine(coroutine);
		}

		protected void OnDestroy()
		{
			ResumeAndRetryScreen.OnBackHome -= ResumeAndRetryScreen_OnBackHome;
			ResumeAndRetryScreen.OnRetryGame -= ResumeAndRetryScreen_OnRetryGame;
			StopCoroutine(coroutine);
			_instance = null;
		}
	}
}



