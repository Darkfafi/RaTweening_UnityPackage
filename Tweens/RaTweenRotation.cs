using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public class RaTweenRotation : RaTween
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
			: this(null, Vector3.zero, Vector3.zero, default)
		{

		}

		public RaTweenRotation(Transform target, Vector3 startRot, Vector3 endRot, AnimationCurve easing)
			: base(easing)
		{
			_target = target;
			_startRot = startRot;
			_endRot = endRot;
			_dynamicStartRotation = false;
		}

		public RaTweenRotation(Transform target, Vector3 endRot, AnimationCurve easing)
			: this(target, target.position, endRot, easing)
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

		protected override RaTweenCore CloneSelf()
		{
			RaTweenRotation tween = new RaTweenRotation(_target, _startRot, _endRot, Easing);
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


	#region Extensions

	public static class RaTweenRotationExtensions
	{
		public static RaTweenCore TweenRotateX(this Transform self, float rotX, AnimationCurve easing)
		{
			return new RaTweenRotation(self, Vector3.right * rotX, easing).Play();
		}

		public static RaTweenCore TweenRotateY(this Transform self, float rotY, AnimationCurve easing)
		{
			return new RaTweenRotation(self, Vector3.up * rotY, easing).Play();
		}

		public static RaTweenCore TweenRotateZ(this Transform self, float rotZ, AnimationCurve easing)
		{
			return new RaTweenRotation(self, Vector3.forward * rotZ, easing).Play();
		}

		public static RaTweenCore TweenRotate(this Transform self, Vector3 rot, AnimationCurve easing)
		{
			return new RaTweenRotation(self, rot, easing).Play();
		}

		public static RaTweenCore TweenRotate(this Transform self, Vector3 startRot, Vector3 endRot, AnimationCurve easing)
		{
			return new RaTweenRotation(self, startRot, endRot, easing).Play();
		}
	}

	#endregion
}