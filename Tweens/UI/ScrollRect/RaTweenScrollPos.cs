using RaTweening.UI.RaScrollRect;
using UnityEngine;
using static RaTweening.RaVector2Options;
using UnityEngine.UI;

namespace RaTweening.UI.RaScrollRect
{
	public class RaTweenScrollPos : RaTweenDynamic<ScrollRect, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenScrollPos")]
		[SerializeField]
		private Axis _excludeAxis = Axis.None;

		#endregion

		public RaTweenScrollPos()
			: base()
		{

		}

		public RaTweenScrollPos(ScrollRect target, Vector2 startNormalizedPos, Vector2 endNormalizedPos, float duration)
			: base(target, startNormalizedPos, endNormalizedPos, duration)
		{

		}

		public RaTweenScrollPos(ScrollRect target, Vector2 endNormalizedPos, float duration)
			: base(target, endNormalizedPos, duration)
		{

		}

		public RaTweenScrollPos(ScrollRect target, Vector2 startNormalizedPos, ScrollRect endNormalizedPos, float duration)
			: base(target, startNormalizedPos, default, duration)
		{
			SetEndRef(endNormalizedPos);
		}

		#region Public Methods

		public RaTweenScrollPos SetExcludeAxis(Axis excludeAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = excludeAxis;
			}
			return this;
		}

		public RaTweenScrollPos OnlyIncludeAxis(Axis inclAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = GetOnlyIncludeAxes(inclAxis);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenDynamic<ScrollRect, Vector2> DynamicClone()
		{
			RaTweenScrollPos tween = new RaTweenScrollPos();
			tween._excludeAxis = _excludeAxis;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, ScrollRect target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.normalizedPosition = ApplyExcludeAxes(target.normalizedPosition, start + (delta * normalizedValue), _excludeAxis);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(ScrollRect reference)
		{
			return reference.normalizedPosition;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		public static RaTweenScrollPos TweenScrollPosX(this ScrollRect self, float normalizedPosX, float duration)
		{
			return new RaTweenScrollPos(self, Vector2.one * normalizedPosX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		public static RaTweenScrollPos TweenScrollPosY(this ScrollRect self, float normalizedPosY, float duration)
		{
			return new RaTweenScrollPos(self, Vector2.one * normalizedPosY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		public static RaTweenScrollPos TweenScrollPos(this ScrollRect self, Vector2 normalizedPos, float duration)
		{
			return new RaTweenScrollPos(self, normalizedPos, duration).Play();
		}

		public static RaTweenScrollPos TweenScrollPos(this ScrollRect self, Vector2 startNormalizedPos, Vector2 endNormalizedPos, float duration)
		{
			return new RaTweenScrollPos(self, startNormalizedPos, endNormalizedPos, duration).Play();
		}

		public static RaTweenScrollPos TweenScrollPos(this ScrollRect self, Vector2 startNormalizedPos, ScrollRect endTarget, float duration)
		{
			return new RaTweenScrollPos(self, startNormalizedPos, endTarget, duration).Play();
		}
	}

	#endregion
}