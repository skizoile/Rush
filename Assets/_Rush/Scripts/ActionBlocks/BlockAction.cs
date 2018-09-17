///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 12/05/2018 22:02
///-----------------------------------------------------------------

using Fr.Sebastien.Rush.Rush.Scripts.Other;
using UnityEngine;
using System.Collections;

namespace Fr.Sebastien.Rush.Rush.Scripts.ActionBlocks
{
    public class BlockAction : Block {

		[SerializeField] protected bool onDropMeteore = false;
		protected float elapsedTime = 0;
		protected float timeCreate = 0.7f;
		protected float minPosY = 0;
		protected Vector3 realColliderSize;

		virtual protected void Awake()
		{
			transform.GetChild(0).localScale = new Vector3(0, 1, 0);
		}

		override protected void Start()
		{
			base.Start();
			Meteores.OnExplode += Meteores_OnExplode;
			if (onDropMeteore)
			{
				Meteores.OnExplode -= Meteores_OnExplode;
				minPosY = transform.GetChild(0).position.y;
				StartCoroutine(CreateObjectAnimation());
			}

		}

		protected IEnumerator CreateObjectAnimation()
		{
			float ratio = 0;
			float startRotation = transform.GetChild(0).eulerAngles.y;
			float endRotation = startRotation + 1440.0f;

			while (ratio < 1)
			{
				elapsedTime += Time.deltaTime;
				ratio = elapsedTime / timeCreate;
				float yRotation = Mathf.Lerp(startRotation, endRotation, ratio) % 1440.0f;
				Transform child;
				for (int i = 0; i < transform.childCount; i++)
				{
					child = transform.GetChild(i);
					child.localScale = new Vector3(Mathf.Min(ratio, 1), 1, Mathf.Min(ratio, 1));
					child.position = new Vector3(child.position.x, Mathf.Max((minPosY + (0.041f * i)), (minPosY + (0.041f * i)) + Mathf.Sin(ratio * Mathf.PI)), child.position.z);
					child.eulerAngles = new Vector3(child.eulerAngles.x, yRotation, child.eulerAngles.z);
				}

				yield return new WaitForEndOfFrame();
			}
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).localScale = new Vector3(1, 1, 1);
			}
		}

		private void Meteores_OnExplode()
		{
			if (onDropMeteore)
				return;
			onDropMeteore = true;
			minPosY = transform.GetChild(0).position.y;
			StartCoroutine(CreateObjectAnimation());
		}

		override protected void OnDestroy()
		{
			base.OnDestroy();
			Meteores.OnExplode -= Meteores_OnExplode;
		}
	}
}



