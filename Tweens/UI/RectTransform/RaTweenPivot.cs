using RaTweening.UI.RaRectTransform;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.UI.RaRectTransform
{
	public class RaTweenPivot : RaTweenDynamic<RectTransform, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenPivot")]
		[SerializeField]
		private Axis _excludeAxis = Axis.None;

		#endregion

		public RaTweenPivot()
			: base()
		{

		}

		public RaTweenPivot(RectTransform target, Vector2 startPivot, Vector2 endPivot, float duration)
			: base(target, startPivot, endPivot, duration)
		{

		}

		public RaTweenPivot(RectTransform target, Vector2 endPivot, float duration)
			: base(target, endPivot, duration)
		{

		}

		public RaTweenPivot(RectTransform target, Vector2 startPivot, RectTransform endPivot, float duration)
			: base(target, startPivot, default, duration)
		{
			SetEndRef(endPivot);
		}

		#region Public Methods

		public RaTweenPivot SetExcludeAxis(Axis excludeAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = excludeAxis;
			}
			return this;
		}

		public RaTweenPivot OnlyIncludeAxis(Axis inclAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = GetOnlyIncludeAxes(inclAxis);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(Target != null ? ReadValue(Target) : Vector2.one * 0.5f);
			SetEndValue(Vector2.one * 0.5f);
		}

		protected override RaTweenDynamic<RectTransform, Vector2> DynamicClone()
		{
			RaTweenPivot tween = new RaTweenPivot();
			tween._excludeAxis = _excludeAxis;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, RectTransform target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.pivot = ApplyExcludeAxes(target.pivot, start + (delta * normalizedValue), _excludeAxis);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(RectTransform reference)
		{
			return reference.pivot;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		public static RaTweenPivot TweenPivotX(this RectTransform self, float pivotX, float duration)
		{
			return new RaTweenPivot(self, Vector2.one * pivotX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		public static RaTweenPivot TweenPivotY(this RectTransform self, float pivotY, float duration)
		{
			return new RaTweenPivot(self, Vector2.one * pivotY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		public static RaTweenPivot TweenPivot(this RectTransform self, Vector2 pivot, float duration)
		{
			return new RaTweenPivot(self, pivot, duration).Play();
		}

		public static RaTweenPivot TweenPivot(this RectTransform self, Vector2 startPivot, Vector2 endPivot, float duration)
		{
			return new RaTweenPivot(self, startPivot, endPivot, duration).Play();
		}

		public static RaTweenPivot TweenPivot(this RectTransform self, Vector2 startPivot, RectTransform endPivot, float duration)
		{
			return new RaTweenPivot(self, startPivot, endPivot, duration).Play();
		}
	}

	#endregion
}