///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 24/04/2018 15:16
///-----------------------------------------------------------------

using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scripts.ActionBlocks
{
    public class EndBlock : Block {

		private static int currentEnd;
		[SerializeField] private GameObject spawner;

		protected void Awake()
		{
			transform.localScale = new Vector3(1, 1, 1);
		}

		protected override void Start()
		{
			GetComponent<BoxCollider>().isTrigger = true;
		}

		public override void DoAction()
		{
			base.DoAction();

			if (target.gameObject.GetComponent<CubeMove>().Spawner == spawner)
			{
				currentEnd++;
				target.SetModeEnd();
			}
			else
			{
				target.SetModeMove();
			}

		}
	}
}



