///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 09/05/2018 18:09
///-----------------------------------------------------------------

using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scripts.Other {
    public class FlyingEndPoint : MonoBehaviour {

		protected float elaspedTime;
		protected float time = 1;

		public float speed = 1;
		protected Vector3 startPos;
		protected float factorSpeed;

		protected new Camera camera;
		protected MeshRenderer render;

        private void Start () {
			camera = Camera.main;
			render = GetComponent<MeshRenderer>();
			startPos = transform.position;
			factorSpeed = Random.Range(0.1f, 1f);
			speed *= factorSpeed;
		}

        private void Update () {
			RaycastHit hit;
			if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 25f))
			{
				if (hit.collider.gameObject == gameObject)
					render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, 0.25f);
				else
					render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, 1f);
			}

			elaspedTime += Time.deltaTime * speed;

			float sin = Mathf.Sin(elaspedTime / Mathf.PI) / 2;

			transform.position = new Vector3(startPos.x, startPos.y + sin, startPos.z);
			transform.Rotate(Vector3.up, factorSpeed * 2);
        }
    }
}



