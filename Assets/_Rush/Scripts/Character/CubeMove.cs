///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 24/04/2018 10:51
///-----------------------------------------------------------------

using UnityEngine;
using System;
using Fr.Sebastien.Rush.Rush.Scripts.ActionBlocks;
using Fr.Sebastien.Rush.Rush.Scripts.Other;

namespace Fr.Sebastien.Rush.Rush.Scripts {

	public class CubeMove : MonoBehaviour {

		[SerializeField] protected GameObject deathIndicatorPrefab;
		[SerializeField] protected AudioClip hitGroundAudio;
		protected AudioSource audioSource;

		private float _tickCount = 0;
		private float _tickAction = 0;

		protected float _elapsedTime = 0;
		protected float _timeMovement;
		protected float _timeMovementBase = 1f;

		protected Vector3 _startPos;
		protected Vector3 _finalPos;
		protected Vector3 offset = new Vector3(0, 0.5f, 0);

		protected Quaternion startRot;
		protected Quaternion finalRot;

		protected float posY;

		public Vector3 _rotDirection;
		public Vector3 _moveDirection;

		protected bool blockEnvironnementAndBlock = false;



		public Vector3 MoveDirection
		{
			get
			{
				return _moveDirection;
			}
			set
			{
				_moveDirection = value;
				_rotDirection = Quaternion.AngleAxis(90, Vector3.up) * _moveDirection;
			}
		}
		public float ElapsedTime
		{
			get
			{
				return _elapsedTime;
			}
		}
		public float TimeMovement
		{
			get
			{
				return _timeMovement;
			}
			set
			{
				_timeMovement = value;
			}
		}
		public Vector3 StartPos
		{
			get
			{
				return _startPos;
			}
			set
			{
				_startPos = value;
			}
		}
		public Vector3 FinalPos
		{
			get
			{
				return _finalPos;
			}
			set
			{
				_finalPos = value;
			}
		}

		public float TickCount
		{
			get
			{
				return _tickCount;
			}
			set
			{
				_tickCount = value;
			}
		}

		public float TickAction
		{
			get
			{
				return _tickAction;
			}

			set
			{
				_tickAction = value;
			}
		}

		public Action CallBack
		{
			get
			{
				return _callback;
			}

			set
			{
				_callback = value;
			}
		}

		public float TimeMovementBase
		{
			get
			{
				return _timeMovementBase;
			}
		}

		[SerializeField] protected LayerMask maskBlock;
		[SerializeField] protected LayerMask maskDeadZone;
		[SerializeField] protected LayerMask environnementBlock;
		[SerializeField] protected float lenghtRayCast = 0.7f;

		private Action _doAction;
		private Action _callback;

		public static event Action OnFinish;
		public static event Action OnPause;

		protected GameObject _spawner;

		public GameObject Spawner
		{
			get
			{
				return _spawner;
			}
			set
			{
				_spawner = value;
			}
		}

		public Action DoAction
		{
			get
			{
				return _doAction;
			}

			set
			{
				_doAction = value;
			}
		}

		protected void Start()
		{
			audioSource = GetComponent<AudioSource>();
			audioSource.playOnAwake = false;

			MoveDirection = transform.rotation * Vector3.forward;
			_timeMovement = _tickAction;
			ResetNextPosition(MoveDirection, _rotDirection);
			transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

			Metronome. OnUpdateTimeRatio += Metronome_OnUpdateTimeRatio;
			Metronome.OnTick += Metronome_OnTick;
			SetModeStart();
		}

		private void Metronome_OnTick()
		{
			_tickCount++;

			if (_tickCount >= _tickAction)
			{
				_tickCount = 0;
				_callback();
			}
		}

		private void Metronome_OnUpdateTimeRatio(float pTime)
		{
			_elapsedTime = pTime;
			DoAction();
		}

		protected void SetModeStart()
		{
			CallBack = SetModeVerify;
			_tickAction = 3;
			posY = transform.position.y;
			_timeMovement = _timeMovementBase * _tickAction;
			DoAction = DoActionStart;
		}

		protected void DoActionStart()
		{
			float ratio = (_tickCount / _tickAction) + ElapsedTime / TimeMovement;

			transform.position = new Vector3(transform.position.x, (posY + 1) + Mathf.Sin(ratio * Mathf.PI), transform.position.z);
			if (_tickCount >= _tickAction / 2)
			{
				float size = Mathf.Abs(Mathf.Cos(ratio * Mathf.PI));
				transform.localScale = new Vector3(size, size, size);
			}
		}

		public void SetModeVerify()
		{
			audioSource.PlayOneShot(hitGroundAudio);
			CallBack = SetModeVerify;
			RaycastHit hit;

			if (Physics.Raycast(transform.position, Vector3.down, 1f, maskDeadZone))
			{
				SetModeDeath();
				return;
			}
			if (!Physics.Raycast(transform.position, Vector3.down, lenghtRayCast))
			{
				SetModeFall();
				return;
			}
			else if (Physics.Raycast(transform.position, Vector3.down, out hit, lenghtRayCast, maskBlock) && !blockEnvironnementAndBlock)
			{
				if (Physics.Raycast(transform.position, Vector3.Cross(MoveDirection.normalized, Vector3.up), lenghtRayCast, environnementBlock))
				{
					SetModeCollidEnvironnement();
					return;
				}

				blockEnvironnementAndBlock = false;
				DoAction = hit.collider.GetComponent<Block>().DoAction;
				return;
			}
			else if (Physics.Raycast(transform.position, MoveDirection.normalized, out hit, lenghtRayCast, environnementBlock))
			{
				MoveDirection = -Vector3.Cross(MoveDirection.normalized, Vector3.up);
				blockEnvironnementAndBlock = Physics.Raycast(transform.position, Vector3.down, out hit, lenghtRayCast, maskBlock);
				SetModeCollidEnvironnement();
				return;
			}
			else
			{
				blockEnvironnementAndBlock = false;
				SetModeMove();
				return;
			}
		}

		public void SetModeVerifyWait()
		{
			if (!Physics.Raycast(transform.position, Vector3.down, lenghtRayCast))
			{
				SetModeVerify();
			}
			else
			{
				SetModeWait(2);
			}
		}

		public void SetModeMove()
		{
			CallBack = SetModeVerify;
			_tickAction = 2;
			_timeMovement = _timeMovementBase * _tickAction;
			ResetNextPosition(MoveDirection, _rotDirection);
			DoAction = DoActionMove;
		}

		protected void DoActionMove()
		{
			float ratio = (_tickCount / _tickAction) + ElapsedTime / TimeMovement;

			Vector3 fakePos = Vector3.Slerp(offset, MoveDirection + offset, ratio);
			fakePos -= offset;

			transform.position = StartPos + fakePos;
			transform.rotation = Quaternion.Lerp(startRot, finalRot, ratio);
		}

		protected void SetModeFall()
		{
			_tickAction = 1;
			_timeMovement = _timeMovementBase * _tickAction;
			ResetNextPosition(Vector3.down, Vector3.zero);
			DoAction = DoActionFall;
		}

		protected void DoActionFall()
		{
			float ratio = (_tickCount / _tickAction) + ElapsedTime / TimeMovement;
			transform.position = Vector3.Lerp(StartPos, FinalPos, ratio);
		}

		protected void ResetNextPosition(Vector3 pDirectionMove, Vector3 pDirectionRot)
		{
			_startPos = transform.position;
			_finalPos = StartPos + pDirectionMove;

			startRot = transform.rotation;
			finalRot = Quaternion.AngleAxis(90, pDirectionRot) * startRot;
		}

		public void SetModeWait(int pTime)
		{
			CallBack = SetModeMove;
			_tickAction = pTime;
			_timeMovement = _timeMovementBase * _tickAction;
			DoAction = DoActionWait;
		}

		protected void DoActionWait()
		{

		}

		protected void SetModeCollidEnvironnement()
		{
			_tickAction = 2;
			_timeMovement = _timeMovementBase * _tickAction;
			ResetNextPosition(MoveDirection, _rotDirection);
			DoAction = DoActionCollidEnvironnement;
		}

		protected void DoActionCollidEnvironnement()
		{
			
		}

		private void SetModeDeath()
		{
			GameObject indicatorDeath = Instantiate(deathIndicatorPrefab);
			indicatorDeath.GetComponent<DeathIndicator>().SetTargetPos(transform);
			
			if (OnPause != null)
			{
				OnPause();
				Time.timeScale = 0;
			}
			transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);

		}

		private void OnTriggerEnter(Collider other)
		{

			if (!other.CompareTag("Cube"))
				return;

			Debug.Log("Collid", gameObject);
			SetModeDeath();
		}

		public void SetModeEnd()
		{
			_tickAction = 4;
			posY = transform.position.y;
			foreach (var boxCollider in GetComponents<BoxCollider>())
			{
				boxCollider.enabled = false;
			}
			DoAction = DoActionEnd;
		}

		protected void DoActionEnd()
		{
			float ratio = (_tickCount / (_tickAction / 2)) + ElapsedTime / TimeMovement;

			if (_tickCount <= _tickAction / 8)
			{
				transform.position = new Vector3(transform.position.x, (posY + 1) + Mathf.Sin(ratio * Mathf.PI), transform.position.z);
				transform.localScale = new Vector3(0.1f + Mathf.Cos(ratio * Mathf.PI), 0.1f + Mathf.Cos(ratio * Mathf.PI), 0.1f + Mathf.Cos(ratio * Mathf.PI));
			}

			if (_tickCount >= _tickAction / 2)
			{
				Destroy(gameObject);

				Metronome.OnUpdateTimeRatio -= Metronome_OnUpdateTimeRatio;
				Metronome.OnTick -= Metronome_OnTick;

				if (OnFinish != null)
					OnFinish();
				return;
			}
		}

		private void OnDestroy()
		{
			Metronome.OnUpdateTimeRatio -= Metronome_OnUpdateTimeRatio;
			Metronome.OnTick -= Metronome_OnTick;
		}
	}
}



