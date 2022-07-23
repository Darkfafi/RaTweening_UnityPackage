using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public class RaTweenPosition : RaTweenDynamic<Transform, Vector3>
	{
		#region Editor Variables

		[Header("RaTweenPosition")]
		[SerializeField]
		private AxisOption _excludeAxis = AxisOption.None;

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

		internal void SetExcludeAxisAPIInternal(AxisOption excludeAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = excludeAxis;
			}
		}

		internal void OnlyInclideAxisAPIInternal(AxisOption inclAxis)
		{
			if(CanBeModified())
			{
				if(!inclAxis.HasFlag(AxisOption.X))
				{
					_excludeAxis |= AxisOption.X;
				}

				if(!inclAxis.HasFlag(AxisOption.Y))
				{
					_excludeAxis |= AxisOption.Y;
				}

				if(!inclAxis.HasFlag(AxisOption.Z))
				{
					_excludeAxis |= AxisOption.Z;
				}
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
			Vector3 finalValue = start + (delta * normalizedValue);

			if(_excludeAxis.HasFlag(AxisOption.X))
			{
				finalValue.x = target.position.x;
			}

			if(_excludeAxis.HasFlag(AxisOption.Y))
			{
				finalValue.y = target.position.y;
			}

			if(_excludeAxis.HasFlag(AxisOption.Z))
			{
				finalValue.z = target.position.z;
			}

			target.position = finalValue;
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

		#region Nested

		[Flags]
		public enum AxisOption
		{
			None	= 0, 
			X		= 1, 
			Y		= 2, 
			Z		= 4
		}

		#endregion
	}

	#region Extensions

	public static class RaTweenPositionExtensions
	{
		public static RaTweenPosition TweenMoveX(this Transform self, float posX, float duration)
		{
			return new RaTweenPosition(self, Vector3.one * posX, duration)
				.OnlyIncludeAxis(RaTweenPosition.AxisOption.X).Play();
		}

		public static RaTweenPosition TweenMoveY(this Transform self, float posY, float duration)
		{
			return new RaTweenPosition(self, Vector3.one * posY, duration)
				.OnlyIncludeAxis(RaTweenPosition.AxisOption.Y).Play();
		}

		public static RaTweenPosition TweenMoveZ(this Transform self, float posZ, float duration)
		{
			return new RaTweenPosition(self, Vector3.one * posZ, duration)
				.OnlyIncludeAxis(RaTweenPosition.AxisOption.Z).Play();
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

		public static RaTweenPosition SetExcludeAxis(this RaTweenPosition self, RaTweenPosition.AxisOption excludeAxis)
		{
			self.SetExcludeAxisAPIInternal(excludeAxis);
			return self;
		}

		public static RaTweenPosition OnlyIncludeAxis(this RaTweenPosition self, RaTweenPosition.AxisOption includeAxis)
		{
			self.OnlyInclideAxisAPIInternal(includeAxis);
			return self;
		}
	}

	#endregion
}