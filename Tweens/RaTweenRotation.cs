using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public class RaTweenRotation : RaTweenBase
	{
		#region Editor Variables

		[SerializeField]
		private Transform _target = default;

		[SerializeField]
		private Vector3 _startRot = Vector3.zero;

		[SerializeField]
		private Vector3 _endRot = Vector3.zero;

		[SerializeField]
		private bool _dynamicStartRotation = false;

		#endregion

		#region Properties

		public override bool IsValid => _target != null;

		#endregion

		public RaTweenRotation()
			: this(null, Vector3.zero, Vector3.zero, default, 0f)
		{

		}

		public RaTweenRotation(Transform target, Vector3 startRot, Vector3 endRot, AnimationCurve easing, float delay)
			: base(easing, delay)
		{
			_target = target;
			_startRot = startRot;
			_endRot = endRot;
			_dynamicStartRotation = false;
		}

		public RaTweenRotation(Transform target, Vector3 endRot, AnimationCurve easing, float delay)
			: this(target, target.position, endRot, easing, delay)
		{
			_dynamicStartRotation = true;
		}

		#region Protected

		protected override void OnStart()
		{
			base.OnStart();

			if(_dynamicStartRotation)
			{
				_startRot = _target.rotation.eulerAngles;
			}
		}

		protected override RaTweenBase CloneSelf()
		{
			RaTweenRotation tween = new RaTweenRotation(_target, _startRot, _endRot, Easing, Delay);
			tween._dynamicStartRotation = _dynamicStartRotation;
			return tween;
		}

		protected override void Evaluate(float normalizedValue)
		{
			if(_target != null)
			{
				_target.rotation = Quaternion.SlerpUnclamped(Quaternion.Euler(_startRot), Quaternion.Euler(_endRot), normalizedValue);
			}
		}

		protected override void Dispose()
		{
			_target = null;
		}

		#endregion
	}
}