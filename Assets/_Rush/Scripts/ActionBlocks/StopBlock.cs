///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 23/04/2018 20:13
///-----------------------------------------------------------------

using UnityEngine;
using System;

namespace Fr.Sebastien.Rush.Rush.Scripts.ActionBlocks
{
    public class StopBlock : BlockAction
	{

		public override void DoAction()
		{
			base.DoAction();
			target.SetModeWait(4);
		}
	}
}



