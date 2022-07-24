using RaTweening.UI.RaRectTransform;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.UI.RaRectTransform
{
	public class RaTweenAnchorMin : RaTweenDynamic<RectTransform, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenAnchorMin")]
		[SerializeField]
		private Axis _excludeAxis = Axis.None;

		#endregion

		public RaTweenAnchorMin()
			: base()
		{

		}

		public RaTweenAnchorMin(RectTransform target, Vector2 startMin, Vector2 endMin, float duration)
			: base(target, startMin, endMin, duration)
		{

		}

		public RaTweenAnchorMin(RectTransform target, Vector2 endMin, float duration)
			: base(target, endMin, duration)
		{

		}

		public RaTweenAnchorMin(RectTransform target, Vector2 startMin, RectTransform endMin, float duration)
			: base(target, startMin, default, duration)
		{
			SetEndRef(endMin);
		}

		#region Public Methods

		public RaTweenAnchorMin SetExcludeAxis(Axis excludeAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = excludeAxis;
			}
			return this;
		}

		public RaTweenAnchorMin OnlyIncludeAxis(Axis inclAxis)
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
			RaTweenAnchorMin tween = new RaTweenAnchorMin();
			tween._excludeAxis = _excludeAxis;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, RectTransform target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.anchorMin = ApplyExcludeAxes(target.anchorMin, start + (delta * normalizedValue), _excludeAxis);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(RectTransform reference)
		{
			return reference.anchorMin;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		public static RaTweenAnchorMin TweenAnchorMinX(this RectTransform self, float minX, float duration)
		{
			return new RaTweenAnchorMin(self, Vector2.one * minX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		public static RaTweenAnchorMin TweenAnchorMinY(this RectTransform self, float minY, float duration)
		{
			return new RaTweenAnchorMin(self, Vector2.one * minY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		public static RaTweenAnchorMin TweenAnchorMin(this RectTransform self, Vector2 min, float duration)
		{
			return new RaTweenAnchorMin(self, min, duration).Play();
		}

		public static RaTweenAnchorMin TweenAnchorMin(this RectTransform self, Vector2 startMin, Vector2 endMin, float duration)
		{
			return new RaTweenAnchorMin(self, startMin, endMin, duration).Play();
		}

		public static RaTweenAnchorMin TweenAnchorMin(this RectTransform self, Vector2 startMin, RectTransform endTarget, float duration)
		{
			return new RaTweenAnchorMin(self, startMin, endTarget, duration).Play();
		}
	}

	#endregion
}