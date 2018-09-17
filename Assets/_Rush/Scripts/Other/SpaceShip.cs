///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 16/05/2018 15:20
///-----------------------------------------------------------------

using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scripts.Other {
    public class SpaceShip : MonoBehaviour {

		[SerializeField] protected float speed;
		[SerializeField] protected float amplitude = 1;
		[SerializeField] protected GameObject ship;
		protected float posY;
		protected float elapsedTime = 0;

		private void Start()
		{
			posY = ship.transform.position.y;
		}
		private void Update () {
			elapsedTime += Time.deltaTime;

			ship.transform.position = new Vector3(ship.transform.position.x, posY + (Mathf.Cos(elapsedTime) * amplitude), ship.transform.position.z);
			transform.Rotate(Vector3.up, speed);
        }
    }
}



