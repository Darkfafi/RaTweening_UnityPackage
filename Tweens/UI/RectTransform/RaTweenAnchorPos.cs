using RaTweening.UI.RaRectTransform;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.UI.RaRectTransform
{
	public class RaTweenAnchorPos : RaTweenDynamic<RectTransform, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenAnchorPos")]
		[SerializeField]
		private Axis _excludeAxis = Axis.None;

		#endregion

		public RaTweenAnchorPos()
			: base()
		{

		}

		public RaTweenAnchorPos(RectTransform target, Vector2 startPos, Vector2 endPos, float duration)
			: base(target, startPos, endPos, duration)
		{

		}

		public RaTweenAnchorPos(RectTransform target, Vector2 endPos, float duration)
			: base(target, endPos, duration)
		{

		}

		public RaTweenAnchorPos(RectTransform target, Vector2 startPos, RectTransform endPos, float duration)
			: base(target, startPos, default, duration)
		{
			SetEndRef(endPos);
		}

		#region Public Methods

		public RaTweenAnchorPos SetExcludeAxis(Axis excludeAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = excludeAxis;
			}
			return this;
		}

		public RaTweenAnchorPos OnlyIncludeAxis(Axis inclAxis)
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
			RaTweenAnchorPos tween = new RaTweenAnchorPos();
			tween._excludeAxis = _excludeAxis;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, RectTransform target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.anchoredPosition = ApplyExcludeAxes(target.anchoredPosition, start + (delta * normalizedValue), _excludeAxis);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(RectTransform reference)
		{
			return reference.anchoredPosition;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		public static RaTweenAnchorPos TweenAnchorPosX(this RectTransform self, float posX, float duration)
		{
			return new RaTweenAnchorPos(self, Vector2.one * posX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		public static RaTweenAnchorPos TweenAnchorPosY(this RectTransform self, float posY, float duration)
		{
			return new RaTweenAnchorPos(self, Vector2.one * posY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		public static RaTweenAnchorPos TweenAnchorPos(this RectTransform self, Vector2 pos, float duration)
		{
			return new RaTweenAnchorPos(self, pos, duration).Play();
		}

		public static RaTweenAnchorPos TweenAnchorPos(this RectTransform self, Vector2 startPos, Vector2 endPos, float duration)
		{
			return new RaTweenAnchorPos(self, startPos, endPos, duration).Play();
		}

		public static RaTweenAnchorPos TweenAnchorPos(this RectTransform self, Vector2 startPos, RectTransform endTarget, float duration)
		{
			return new RaTweenAnchorPos(self, startPos, endTarget, duration).Play();
		}
	}

	#endregion
}