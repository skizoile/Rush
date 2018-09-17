///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 14/05/2018 21:28
///-----------------------------------------------------------------

using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scripts.Other {
    public class MeteoresUI : MonoBehaviour {

		[SerializeField] protected Vector3 startPos = new Vector3(-50, 50, 0);
		[SerializeField] protected Vector3 endPos;
		[SerializeField] protected float speed = 5;
		[SerializeField] protected GameObject explodParticles;
		[SerializeField] protected AudioClip explodeAudio;
		protected Vector3 direction;
		protected bool hasExplod = false;

		protected AudioSource audioSource;

		private void Start()
		{
			audioSource = GetComponent<AudioSource>();
			audioSource.playOnAwake = false;

			transform.position = startPos;
			direction = (new Vector3(endPos.x, endPos.y + 0.5f, endPos.z) - startPos).normalized;
		}

		public void SetEndPos(Vector3 pEndPos)
		{
			endPos = pEndPos;
		}

		private void Update()
		{

			if (hasExplod)
				return;

			if (Vector3.Distance(transform.position, new Vector3(endPos.x, endPos.y + 0.5f, endPos.z)) <= 0.8f)
			{
				hasExplod = true;
				GameObject lPart = Instantiate(explodParticles);
				lPart.transform.position = new Vector3(transform.position.x,
														transform.position.y + 0.5f,
														transform.position.z);
				lPart.transform.SetParent(transform.parent);

				audioSource.PlayOneShot(explodeAudio);

				Destroy(lPart, 1f);
				gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
				Destroy(gameObject, 0.5f);
				return;
			}

			transform.position = Vector3.MoveTowards(transform.position, new Vector3(endPos.x, endPos.y + 0.5f, endPos.z), Time.deltaTime * speed);
		}
	}
}



