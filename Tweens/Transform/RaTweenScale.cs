using System;
using UnityEngine;

namespace RaTweening
{
	public class RaTweenScale : RaTweenDynamic<Transform, Vector3>
	{
		public RaTweenScale()
			: base()
		{

		}

		public RaTweenScale(Transform target, Vector3 startScale, Vector3 endScale, AnimationCurve easing, bool endIsDelta = false)
			: base(target, startScale, endScale, easing, endIsDelta)
		{

		}

		public RaTweenScale(Transform target, Vector3 endScale, AnimationCurve easing, bool endIsDelta = false)
			: base(target, endScale, easing, endIsDelta)
		{

		}

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStart(Target != null ? GetDynamicStart() : Vector3.one);
			SetEnd(Vector3.one);
		}

		protected override Vector3 GetDynamicStart()
		{
			return Target.localScale;
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
		{
			return start + delta;
		}

		protected override void DynamicEvaluation(float normalizedValue, Transform target, Vector3 start, Vector3 end)
		{
			Vector3 delta = end - start;
			target.localScale = start + (delta * normalizedValue);
		}

		protected override RaTweenDynamic<Transform, Vector3> DynamicClone()
		{
			return new RaTweenScale();
		}

		#endregion
	}


	#region Extensions

	public static class RaTweenScaleExtensions
	{
		public static RaTweenCore TweenScaleX(this Transform self, float posX, AnimationCurve easing)
		{
			return new RaTweenScale(self, Vector3.right * posX, easing).Play();
		}

		public static RaTweenCore TweenScaleY(this Transform self, float posY, AnimationCurve easing)
		{
			return new RaTweenScale(self, Vector3.up * posY, easing).Play();
		}

		public static RaTweenCore TweenScaleZ(this Transform self, float posZ, AnimationCurve easing)
		{
			return new RaTweenScale(self, Vector3.forward * posZ, easing).Play();
		}

		public static RaTweenCore TweenScale(this Transform self, Vector3 pos, AnimationCurve easing)
		{
			return new RaTweenScale(self, pos, easing).Play();
		}

		public static RaTweenCore TweenScale(this Transform self, Vector3 startScale, Vector3 endScale, AnimationCurve easing)
		{
			return new RaTweenScale(self, startScale, endScale, easing).Play();
		}
	}

	#endregion
}