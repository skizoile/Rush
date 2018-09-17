///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 14/05/2018 21:32
///-----------------------------------------------------------------

using Fr.Sebastien.Rush.Rush.Scripts.Other;
using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scripts.Manager {
    public class MeteoresUIManager : MonoBehaviour {
        private static MeteoresUIManager _instance;
        public static MeteoresUIManager Instance { get { return _instance; } }

        private void Awake(){
            if (_instance){
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

		[SerializeField] protected GameObject meteoresUI;
		[SerializeField] protected Transform[] environnementBlock;
		[SerializeField] protected float minTimeSpawn = 3f;
		[SerializeField] protected float maxTimeSpawn = 10f;
		protected float timeSpawn;

		protected float elapsedTime = 0;


        private void Start () {
			if (environnementBlock.Length == 0)
				Debug.LogError("pas de bloc pour l'arrivé des metéores");
        }

        private void Update () {
			elapsedTime += Time.deltaTime;

			if (elapsedTime >= timeSpawn)
			{
				elapsedTime = 0;
				NewTimeSpawn();
				GameObject lMeteores = Instantiate(meteoresUI);
				lMeteores.GetComponent<MeteoresUI>().SetEndPos(environnementBlock[Random.Range(0, environnementBlock.Length - 1)].position);
			}
        }

		protected void NewTimeSpawn()
		{
			timeSpawn = Random.Range(minTimeSpawn, maxTimeSpawn);
		}

        private void OnDestroy(){
            if (this == _instance)
                _instance = null;
        }
    }
}



