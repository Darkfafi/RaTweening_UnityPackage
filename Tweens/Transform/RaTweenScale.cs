using System;
using UnityEngine;

namespace RaTweening
{
	public class RaTweenScale : RaTweenDynamic<Transform, Transform, Vector3>
	{
		public RaTweenScale()
			: base()
		{

		}

		public RaTweenScale(Transform target, Vector3 startScale, Vector3 endScale, float duration)
			: base(target, startScale, endScale, duration)
		{

		}

		public RaTweenScale(Transform target, Vector3 endScale, float duration)
			: base(target, endScale, duration)
		{

		}

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(Target != null ? GetDynamicStart(Target) : Vector3.one);
			SetEndValue(Vector3.one);
		}

		protected override void DynamicEvaluation(float normalizedValue, Transform target, Vector3 start, Vector3 end)
		{
			Vector3 delta = end - start;
			target.localScale = start + (delta * normalizedValue);
		}

		protected override RaTweenDynamic<Transform, Transform, Vector3> DynamicClone()
		{
			return new RaTweenScale();
		}

		protected override Vector3 GetStartFromRef(Transform reference)
		{
			return reference.localScale;
		}

		protected override Vector3 GetDynamicStart(Transform target)
		{
			return target.localScale;
		}

		protected override Vector3 GetEndFromRef(Transform reference)
		{
			return reference.localScale;
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
		{
			return start + delta;
		}

		#endregion
	}


	#region Extensions

	public static class RaTweenScaleExtensions
	{
		public static RaTweenScale TweenScaleX(this Transform self, float scaleX, float duration)
		{
			return new RaTweenScale(self, Vector3.right * scaleX, duration).Play();
		}

		public static RaTweenScale TweenScaleY(this Transform self, float scaleY, float duration)
		{
			return new RaTweenScale(self, Vector3.up * scaleY, duration).Play();
		}

		public static RaTweenScale TweenScaleZ(this Transform self, float scaleX, float duration)
		{
			return new RaTweenScale(self, Vector3.forward * scaleX, duration).Play();
		}

		public static RaTweenScale TweenScale(this Transform self, float scale, float duration)
		{
			return new RaTweenScale(self, Vector3.one * scale, duration).Play();
		}

		public static RaTweenScale TweenScale(this Transform self, float startScale, float endScale, float duration)
		{
			return new RaTweenScale(self, Vector3.one * startScale, Vector3.one * endScale, duration).Play();
		}

		public static RaTweenScale TweenScale(this Transform self, Vector3 scale, float duration)
		{
			return new RaTweenScale(self, scale, duration).Play();
		}

		public static RaTweenScale TweenScale(this Transform self, Vector3 startScale, Vector3 endScale, float duration)
		{
			return new RaTweenScale(self, startScale, endScale, duration).Play();
		}
	}

	#endregion
}