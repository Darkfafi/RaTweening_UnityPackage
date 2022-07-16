using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public class RaTweenPosition : RaTweenDynamic<Transform, Vector3>
	{
		public RaTweenPosition()
			: base()
		{

		}

		public RaTweenPosition(Transform target, Vector3 startPos, Vector3 endPos, AnimationCurve easing, bool endIsDelta = false)
			: base(target, startPos, endPos, easing, endIsDelta)
		{

		}

		public RaTweenPosition(Transform target, Vector3 endPos, AnimationCurve easing, bool endIsDelta = false)
			: base(target, endPos, easing, endIsDelta)
		{

		}

		#region Protected Methods

		protected override Vector3 GetDynamicStart()
		{
			return Target.position;
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
		{
			return start + delta;
		}

		protected override void DynamicEvaluation(float normalizedValue, Transform target, Vector3 start, Vector3 end)
		{
			Vector3 delta = end - start;
			target.position = start + (delta * normalizedValue);
		}

		protected override RaTweenCore CloneSelf()
		{
			return new RaTweenPosition();
		}

		#endregion
	}

	#region Extensions

	public static class RaTweenPositionExtensions
	{
		public static RaTweenCore TweenMoveX(this Transform self, float posX, AnimationCurve easing)
		{
			return new RaTweenPosition(self, Vector3.right * posX, easing).Play();
		}

		public static RaTweenCore TweenMoveY(this Transform self, float posY, AnimationCurve easing)
		{
			return new RaTweenPosition(self, Vector3.up * posY, easing).Play();
		}

		public static RaTweenCore TweenMoveZ(this Transform self, float posZ, AnimationCurve easing)
		{
			return new RaTweenPosition(self, Vector3.forward * posZ, easing).Play();
		}

		public static RaTweenCore TweenMove(this Transform self, Vector3 pos, AnimationCurve easing)
		{
			return new RaTweenPosition(self, pos, easing).Play();
		}

		public static RaTweenCore TweenMove(this Transform self, Vector3 startPos, Vector3 endPos, AnimationCurve easing)
		{
			return new RaTweenPosition(self, startPos, endPos, easing).Play();
		}
	}

	#endregion
}