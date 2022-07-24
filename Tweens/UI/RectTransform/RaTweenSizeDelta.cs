using RaTweening.UI.RaRectTransform;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.UI.RaRectTransform
{
	public class RaTweenSizeDelta : RaTweenDynamic<RectTransform, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenSizeDelta")]
		[SerializeField]
		private Axis _excludeAxis = Axis.None;

		#endregion

		public RaTweenSizeDelta()
			: base()
		{

		}

		public RaTweenSizeDelta(RectTransform target, Vector2 startSize, Vector2 endSize, float duration)
			: base(target, startSize, endSize, duration)
		{

		}

		public RaTweenSizeDelta(RectTransform target, Vector2 endSize, float duration)
			: base(target, endSize, duration)
		{

		}

		public RaTweenSizeDelta(RectTransform target, Vector2 startSize, RectTransform endSize, float duration)
			: base(target, startSize, default, duration)
		{
			SetEndRef(endSize);
		}

		#region Public Methods

		public RaTweenSizeDelta SetExcludeAxis(Axis excludeAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = excludeAxis;
			}
			return this;
		}

		public RaTweenSizeDelta OnlyIncludeAxis(Axis inclAxis)
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
			RaTweenSizeDelta tween = new RaTweenSizeDelta();
			tween._excludeAxis = _excludeAxis;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, RectTransform target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.sizeDelta = ApplyExcludeAxes(target.sizeDelta, start + (delta * normalizedValue), _excludeAxis);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(RectTransform reference)
		{
			return reference.sizeDelta;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		public static RaTweenSizeDelta TweenSizeWidth(this RectTransform self, float width, float duration)
		{
			return new RaTweenSizeDelta(self, Vector2.one * width, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		public static RaTweenSizeDelta TweenSizeHeight(this RectTransform self, float height, float duration)
		{
			return new RaTweenSizeDelta(self, Vector2.one * height, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		public static RaTweenSizeDelta TweenSize(this RectTransform self, Vector2 size, float duration)
		{
			return new RaTweenSizeDelta(self, size, duration).Play();
		}

		public static RaTweenSizeDelta TweenSize(this RectTransform self, Vector2 startSize, Vector2 endSize, float duration)
		{
			return new RaTweenSizeDelta(self, startSize, endSize, duration).Play();
		}

		public static RaTweenSizeDelta TweenSize(this RectTransform self, Vector2 startSize, RectTransform endTarget, float duration)
		{
			return new RaTweenSizeDelta(self, startSize, endTarget, duration).Play();
		}
	}

	#endregion
}