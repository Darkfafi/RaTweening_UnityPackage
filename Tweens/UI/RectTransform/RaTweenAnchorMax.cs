using RaTweening.UI.RaRectTransform;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.UI.RaRectTransform
{
	public class RaTweenAnchorMax : RaTweenDynamic<RectTransform, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenAnchorMax")]
		[SerializeField]
		private Axis _excludeAxis = Axis.None;

		#endregion

		public RaTweenAnchorMax()
			: base()
		{

		}

		public RaTweenAnchorMax(RectTransform target, Vector2 startMax, Vector2 endMax, float duration)
			: base(target, startMax, endMax, duration)
		{

		}

		public RaTweenAnchorMax(RectTransform target, Vector2 endMax, float duration)
			: base(target, endMax, duration)
		{

		}

		public RaTweenAnchorMax(RectTransform target, Vector2 startMax, RectTransform endMax, float duration)
			: base(target, startMax, default, duration)
		{
			SetEndRef(endMax);
		}

		#region Public Methods

		public RaTweenAnchorMax SetExcludeAxis(Axis excludeAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = excludeAxis;
			}
			return this;
		}

		public RaTweenAnchorMax OnlyIncludeAxis(Axis inclAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = GetOnlyIncludeAxes(inclAxis);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenDynamic<RectTransform, Vector2> DynamicClone()
		{
			RaTweenAnchorMax tween = new RaTweenAnchorMax();
			tween._excludeAxis = _excludeAxis;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, RectTransform target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.anchorMax = ApplyExcludeAxes(target.anchorMax, start + (delta * normalizedValue), _excludeAxis);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(RectTransform reference)
		{
			return reference.anchorMax;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		public static RaTweenAnchorMax TweenAnchorMaxX(this RectTransform self, float minX, float duration)
		{
			return new RaTweenAnchorMax(self, Vector2.one * minX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		public static RaTweenAnchorMax TweenAnchorMaxY(this RectTransform self, float minY, float duration)
		{
			return new RaTweenAnchorMax(self, Vector2.one * minY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		public static RaTweenAnchorMax TweenAnchorMax(this RectTransform self, Vector2 min, float duration)
		{
			return new RaTweenAnchorMax(self, min, duration).Play();
		}

		public static RaTweenAnchorMax TweenAnchorMax(this RectTransform self, Vector2 startMax, Vector2 endMax, float duration)
		{
			return new RaTweenAnchorMax(self, startMax, endMax, duration).Play();
		}

		public static RaTweenAnchorMax TweenAnchorMax(this RectTransform self, Vector2 startMax, RectTransform endTarget, float duration)
		{
			return new RaTweenAnchorMax(self, startMax, endTarget, duration).Play();
		}
	}

	#endregion
}