using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public class RaTweenRotation : RaTweenDynamic<Transform, Vector3>
	{
		public RaTweenRotation()
			: base()
		{

		}

		public RaTweenRotation(Transform target, Vector3 startRot, Vector3 endRot, AnimationCurve easing, bool endIsDelta = false)
			: base(target, startRot, endRot, easing, endIsDelta)
		{

		}

		public RaTweenRotation(Transform target, Vector3 endRot, AnimationCurve easing, bool endIsDelta = false)
			: base(target, endRot, endRot, easing, endIsDelta)
		{

		}

		#region Protected Methods

		protected override Vector3 GetDynamicStart()
		{
			return Target.rotation.eulerAngles;
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
		{
			return start + delta;
		}

		protected override void DynamicEvaluation(float normalizedValue, Transform target, Vector3 start, Vector3 end)
		{
			target.rotation = Quaternion.SlerpUnclamped(Quaternion.Euler(start), Quaternion.Euler(end), normalizedValue);
		}

		protected override RaTweenDynamic<Transform, Vector3> DynamicClone()
		{
			return new RaTweenRotation();
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