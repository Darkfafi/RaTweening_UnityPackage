using System;
using UnityEngine;
using static RaTweening.RaVector3Options;

namespace RaTweening
{
	[Serializable]
	public class RaTweenPosition : RaTweenDynamic<Transform, Vector3>
	{
		#region Editor Variables

		[Header("RaTweenPosition")]
		[SerializeField]
		private Axis _excludeAxis = Axis.None;

		#endregion

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

		#region Internal Methods

		internal void SetExcludeAxisAPIInternal(Axis excludeAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = excludeAxis;
			}
		}

		internal void OnlyIncludeAxisAPIInternal(Axis inclAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = GetOnlyIncludeAxes(inclAxis);
			}
		}

		#endregion

		#region Protected Methods

		protected override RaTweenDynamic<Transform, Vector3> DynamicClone()
		{
			RaTweenPosition tween = new RaTweenPosition();
			tween._excludeAxis = _excludeAxis;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, Transform target, Vector3 start, Vector3 end)
		{
			Vector3 delta = end - start;
			target.position = ApplyExcludeAxes(target.position, start + (delta * normalizedValue), _excludeAxis);
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
			return new RaTweenPosition(self, Vector3.one * posX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		public static RaTweenPosition TweenMoveY(this Transform self, float posY, float duration)
		{
			return new RaTweenPosition(self, Vector3.one * posY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		public static RaTweenPosition TweenMoveZ(this Transform self, float posZ, float duration)
		{
			return new RaTweenPosition(self, Vector3.one * posZ, duration)
				.OnlyIncludeAxis(Axis.Z)
				.Play();
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

		public static RaTweenPosition SetExcludeAxis(this RaTweenPosition self, Axis excludeAxis)
		{
			self.SetExcludeAxisAPIInternal(excludeAxis);
			return self;
		}

		public static RaTweenPosition OnlyIncludeAxis(this RaTweenPosition self, Axis includeAxis)
		{
			self.OnlyIncludeAxisAPIInternal(includeAxis);
			return self;
		}
	}

	#endregion
}