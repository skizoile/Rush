///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 23/04/2018 23:06
///   
/// https://pastebin.com/ii49PhhV
///-----------------------------------------------------------------

using UnityEngine;
using System;

namespace Fr.Sebastien.Rush.Rush.Scripts.ActionBlocks
{
    public class TeleportBlock : Block {

		[SerializeField] protected Transform endTeleport;
		[SerializeField] protected Color colorGizmos;
		[SerializeField] protected Color colorTeleport;
		[SerializeField] protected bool principalTeleport = false;

		protected bool waitTeleport = true;
		protected float posY;
		protected float ratio;


		protected void Awake()
		{
			transform.localScale = new Vector3(1, 1, 1);
			posY = transform.position.y + 1;
			if (principalTeleport)
			{
				transform.GetChild(0).GetComponent<MeshRenderer>().material.color = colorTeleport;
				endTeleport.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = colorTeleport;
			}
		}

		override protected void Start()
		{
			GetComponent<BoxCollider>().isTrigger = true;
		}

		override protected void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag(tagCollid))
				return;
			firstTime = true;
			posY = transform.position.y + 1;
			target = other.GetComponent<CubeMove>();
		}

		public override void DoAction()
		{
			base.DoAction();

			if (firstTime)
			{
				firstTime = false;
				target.TickAction = 2;
				target.TimeMovement = target.TimeMovementBase * target.TickAction;
				target.CallBack = Teleport;
			}

			ratio = (target.TickCount / target.TickAction) + target.ElapsedTime / target.TimeMovement;
			target.transform.position = new Vector3(target.transform.position.x, posY + Mathf.Sin(ratio * Mathf.PI), target.transform.position.z);
			if (Mathf.Cos(ratio * Mathf.PI) > 0.1f)
				target.transform.localScale = new Vector3(Mathf.Cos(ratio * Mathf.PI), Mathf.Cos(ratio * Mathf.PI), Mathf.Cos(ratio * Mathf.PI));
		}

		protected void Teleport()
		{
			posY = endTeleport.position.y + 1;
			target.transform.position = new Vector3(endTeleport.position.x, posY, endTeleport.position.z);
			target.TickAction = 2;
			target.TimeMovement = target.TimeMovementBase * target.TickAction;
			target.CallBack = target.SetModeMove;
			target.DoAction = Out;
			endTeleport.GetComponent<TeleportBlock>().firstTime = false;
		}

		private void Out()
		{
			ratio = (target.TickCount / target.TickAction) + target.ElapsedTime / target.TimeMovement;
			target.transform.position = new Vector3(target.transform.position.x, posY + Mathf.Sin(ratio * Mathf.PI), target.transform.position.z);
			if (Mathf.Cos(ratio * Mathf.PI) < 0f)
				target.transform.localScale = new Vector3(
					Mathf.Abs(Mathf.Cos(ratio * Mathf.PI)),
					Mathf.Abs(Mathf.Cos(ratio * Mathf.PI)),
					Mathf.Abs(Mathf.Cos(ratio * Mathf.PI)));
		}

		private void OnDrawGizmos()
		{
			if (endTeleport == null)
				return;

			colorGizmos.a = 1;

			Vector3 startPos = transform.position;
			Vector3 endPos = endTeleport.position;
			startPos.y += 1;
			endPos.y += 1;

			Vector3 direction = (startPos - endPos).normalized;

			Vector3 endArrowMiddle = startPos + (endPos - startPos).normalized;
			Vector3 endArrowTop = endArrowMiddle - ((endPos / 8 - startPos / 8));
			Vector3 endArrowBottom = endArrowMiddle - ((endPos / 8 - startPos / 8));
			endArrowTop.y += 0.2f;
			endArrowBottom.y += -0.2f;

			Gizmos.color = colorGizmos;
			Gizmos.DrawLine(transform.position, startPos);
			Gizmos.DrawLine(startPos, endPos);
			Gizmos.DrawLine(endPos, endTeleport.position);

			Gizmos.DrawLine(endArrowTop, endArrowMiddle);
			Gizmos.DrawLine(endArrowMiddle, endArrowBottom);
		}
	}
}



