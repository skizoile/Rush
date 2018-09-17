///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 23/04/2018 20:12
///-----------------------------------------------------------------

using UnityEngine;
using System;

namespace Fr.Sebastien.Rush.Rush.Scripts.ActionBlocks
{
    public class ArrowBlock : BlockAction
	{
		public override void DoAction()
		{
			base.DoAction();
			target.MoveDirection = transform.forward;
			target.SetModeMove();
		}
	}
}



