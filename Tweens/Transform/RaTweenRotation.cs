using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public class RaTweenRotation : RaTweenDynamic<Transform, Transform, Vector3>
	{
		public RaTweenRotation()
			: base()
		{

		}

		public RaTweenRotation(Transform target, Vector3 startRot, Vector3 endRot, float duration)
			: base(target, startRot, endRot, duration)
		{

		}

		public RaTweenRotation(Transform target, Vector3 endRot, float duration)
			: base(target, endRot, endRot, duration)
		{

		}

		#region Protected Methods

		protected override void DynamicEvaluation(float normalizedValue, Transform target, Vector3 start, Vector3 end)
		{
			target.rotation = Quaternion.SlerpUnclamped(Quaternion.Euler(start), Quaternion.Euler(end), normalizedValue);
		}

		protected override RaTweenDynamic<Transform, Transform, Vector3> DynamicClone()
		{
			return new RaTweenRotation();
		}

		protected override Vector3 GetStartFromRef(Transform reference)
		{
			return reference.rotation.eulerAngles;
		}

		protected override Vector3 GetDynamicStart(Transform target)
		{
			return target.rotation.eulerAngles;
		}

		protected override Vector3 GetEndFromRef(Transform reference)
		{
			return reference.rotation.eulerAngles;
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
		{
			return start + delta;
		}

		#endregion
	}


	#region Extensions

	public static class RaTweenRotationExtensions
	{
		public static RaTweenRotation TweenRotateX(this Transform self, float rotX, float duration)
		{
			return new RaTweenRotation(self, Vector3.right * rotX, duration).Play();
		}

		public static RaTweenRotation TweenRotateY(this Transform self, float rotY, float duration)
		{
			return new RaTweenRotation(self, Vector3.up * rotY, duration).Play();
		}

		public static RaTweenRotation TweenRotateZ(this Transform self, float rotZ, float duration)
		{
			return new RaTweenRotation(self, Vector3.forward * rotZ, duration).Play();
		}

		public static RaTweenRotation TweenRotate(this Transform self, Vector3 rot, float duration)
		{
			return new RaTweenRotation(self, rot, duration).Play();
		}

		public static RaTweenRotation TweenRotate(this Transform self, Vector3 startRot, Vector3 endRot, float duration)
		{
			return new RaTweenRotation(self, startRot, endRot, duration).Play();
		}
	}

	#endregion
}