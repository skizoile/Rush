///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 23/04/2018 20:13
///-----------------------------------------------------------------

using UnityEngine;
using System;

namespace Fr.Sebastien.Rush.Rush.Scripts.ActionBlocks
{
    public class ConvoyorBlock : BlockAction
	{

		public override void DoAction()
		{
			base.DoAction();

			if (firstTime)
			{
				SetArg();
				firstTime = false;
			}

			target.transform.position = Vector3.Lerp(target.StartPos, target.FinalPos, (target.TickCount / target.TickAction) + target.ElapsedTime / target.TimeMovement);
		}

		protected void SetArg()
		{
			target.StartPos = target.transform.position;
			target.FinalPos = target.StartPos + transform.forward;
			target.CallBack = target.SetModeVerify;
		}
	}
}



