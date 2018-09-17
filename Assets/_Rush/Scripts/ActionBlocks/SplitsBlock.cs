///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 23/04/2018 20:12
///-----------------------------------------------------------------

using UnityEngine;
using System;

namespace Fr.Sebastien.Rush.Rush.Scripts.ActionBlocks
{
    public class SplitsBlock : BlockAction
	{
		[SerializeField] protected GameObject logoSplits;
		[SerializeField] protected float speedRotate = 3;
		protected int multiplicator = 1;


		protected override void Awake()
		{
			base.Awake();
			transform.GetChild(1).localScale = new Vector3(0, 1, 0);
		}

		override protected void Start()
		{
			base.Start();
		}

		protected void Update()
		{
			logoSplits.transform.Rotate(Vector3.up, speedRotate);
		}

		public override void DoAction()
		{
			base.DoAction();

			SetArg();
			target.SetModeMove();
		}

		protected void SetArg()
		{
			target.MoveDirection = -Vector3.Cross(target.MoveDirection.normalized, Vector3.up);
			target.MoveDirection *= multiplicator;
			multiplicator *= -1;
			logoSplits.transform.Rotate(Vector3.right, 180);
			target.StartPos = target.transform.position;
			target.FinalPos = target.StartPos + transform.forward;
		}

	}
}



