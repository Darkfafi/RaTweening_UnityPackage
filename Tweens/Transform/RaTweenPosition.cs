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

		public RaTweenPosition(Transform target, Vector3 startPos, Vector3 endPos, float duration)
			: base(target, startPos, endPos, duration)
		{

		}

		public RaTweenPosition(Transform target, Vector3 endPos, float duration)
			: base(target, endPos, duration)
		{

		}

		public RaTweenPosition(Transform target, Vector3 startPos, Transform endPos, float duration)
		: base(target, startPos, duration)
		{
			SetEndRef(endPos);
		}

		#region Protected Methods

		protected override RaTweenDynamic<Transform, Vector3> DynamicClone()
		{
			return new RaTweenPosition();
		}

		protected override void DynamicEvaluation(float normalizedValue, Transform target, Vector3 start, Vector3 end)
		{
			Vector3 delta = end - start;
			target.position = start + (delta * normalizedValue);
		}

		protected override Vector3 ReadValue(Transform reference)
		{
			return reference.position;
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
		{
			return start + delta;
		}

		#endregion
	}

	#region Extensions

	public static class RaTweenPositionExtensions
	{
		public static RaTweenPosition TweenMoveX(this Transform self, float posX, float duration)
		{
			return new RaTweenPosition(self, Vector3.right * posX, duration).Play();
		}

		public static RaTweenPosition TweenMoveY(this Transform self, float posY, float duration)
		{
			return new RaTweenPosition(self, Vector3.up * posY, duration).Play();
		}

		public static RaTweenPosition TweenMoveZ(this Transform self, float posZ, float duration)
		{
			return new RaTweenPosition(self, Vector3.forward * posZ, duration).Play();
		}

		public static RaTweenPosition TweenMove(this Transform self, Vector3 pos, float duration)
		{
			return new RaTweenPosition(self, pos, duration).Play();
		}

		public static RaTweenPosition TweenMove(this Transform self, Vector3 startPos, Vector3 endPos, float duration)
		{
			return new RaTweenPosition(self, startPos, endPos, duration).Play();
		}

		public static RaTweenPosition TweenMove(this Transform self, Vector3 startPos, Transform endTarget, float duration)
		{
			return new RaTweenPosition(self, startPos, endTarget, duration).Play();
		}
	}

	#endregion
}