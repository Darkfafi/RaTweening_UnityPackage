using RaTweening.UI.RaCanvasGroup;
using System;
using UnityEngine;

namespace RaTweening.UI.RaCanvasGroup
{
	[Serializable]
	public class RaTweenAlpha : RaTweenDynamic<CanvasGroup, float>
	{
		public RaTweenAlpha()
			: base()
		{

		}

		public RaTweenAlpha(CanvasGroup target, float startAlpha, float endAlpha, float duration)
			: base(target, startAlpha, endAlpha, duration)
		{

		}

		public RaTweenAlpha(CanvasGroup target, float endAlpha, float duration)
			: base(target, endAlpha, duration)
		{

		}

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(1f);
			SetEndValue(0f);
		}

		protected override void DynamicEvaluation(float normalizedValue, CanvasGroup target, float start, float end)
		{
			float delta = end - start;
			target.alpha = start + (delta * normalizedValue);
		}

		protected override RaTweenDynamic<CanvasGroup, float> DynamicClone()
		{
			RaTweenAlpha tween = new RaTweenAlpha();
			return tween;
		}

		protected override float ReadValue(CanvasGroup reference)
		{
			return reference.alpha;
		}

		protected override float GetEndByDelta(float start, float delta)
		{
			return start + delta;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		public static RaTweenAlpha TweenAlpha(this CanvasGroup self, float alpha, float duration)
		{
			return new RaTweenAlpha(self, alpha, duration).Play();
		}
	}

	#endregion
}