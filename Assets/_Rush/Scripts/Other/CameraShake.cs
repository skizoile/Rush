///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 12/05/2018 21:01
///-----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scripts.Other {
    public class CameraShake : MonoBehaviour {

		[SerializeField] protected float duration = .15f;
		[SerializeField] protected float magnitude = .4f;

		void Start()
		{
			Meteores.OnExplode += Meteores_OnExplode;
		}

		private void Meteores_OnExplode()
		{
			StartCoroutine(Shake());
		}

		IEnumerator Shake()
		{
			Vector3 originalPos = transform.localPosition;
			float elasped = 0.0f;
			while(elasped < duration)
			{
				float x = Random.Range(-1f, 1f) * magnitude;
				float y = Random.Range(-1f, 1f) * magnitude;

				transform.localPosition = new Vector3(x, y, originalPos.z);

				elasped += Time.deltaTime;

				yield return null;
			}

			transform.localPosition = originalPos;
		}

		private void OnDestroy()
		{
			Meteores.OnExplode -= Meteores_OnExplode;
		}
	}
}



