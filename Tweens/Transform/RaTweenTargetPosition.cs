using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public class RaTweenTargetPosition : RaTweenDynamic<Transform, Transform, Vector3>
	{
		public RaTweenTargetPosition()
			: base()
		{

		}

		public RaTweenTargetPosition(Transform target, Transform startPos, Transform endPos, float duration)
			: base(target, startPos, endPos, duration)
		{

		}

		public RaTweenTargetPosition(Transform target, Transform endPos, float duration)
			: base(target, endPos, duration)
		{

		}

		#region Protected Methods

		protected override Vector3 GetStart()
		{
			return GetStartRef().position;
		}

		protected override Vector3 GetEnd()
		{
			return GetEndRef().position;
		}

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

		protected override RaTweenDynamic<Transform, Transform, Vector3> DynamicClone()
		{
			return new RaTweenTargetPosition();
		}

		#endregion
	}

	#region Extensions

	public static class RaTweenPositionRefExtensions
	{
		public static RaTweenTargetPosition TweenMove(this Transform self, Transform endTarget, float duration)
		{
			return new RaTweenTargetPosition(self, endTarget, duration).Play();
		}

		public static RaTweenTargetPosition TweenMove(this Transform self, Transform startTarget, Transform endTarget, float duration)
		{
			return new RaTweenTargetPosition(self, startTarget, endTarget, duration).Play();
		}
	}

	#endregion
}