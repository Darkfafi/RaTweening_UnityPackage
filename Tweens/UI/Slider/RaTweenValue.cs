using RaTweening.UI.RaSlider;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RaTweening.UI.RaSlider
{
	[Serializable]
	public class RaTweenValue : RaTweenDynamic<Slider, float>
	{
		public RaTweenValue()
			: base()
		{

		}

		public RaTweenValue(Slider target, float startValue, float endValue, float duration)
			: base(target, startValue, endValue, duration)
		{

		}

		public RaTweenValue(Slider target, float endValue, float duration)
			: base(target, endValue, duration)
		{

		}

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(0f);
			SetEndValue(1f);
		}

		protected override void DynamicEvaluation(float normalizedValue, Slider target, float start, float end)
		{
			float delta = end - start;
			target.value = start + (delta * normalizedValue);
		}

		protected override RaTweenDynamic<Slider, float> DynamicClone()
		{
			RaTweenValue tween = new RaTweenValue();
			return tween;
		}

		protected override float ReadValue(Slider reference)
		{
			return reference.value;
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
		public static RaTweenValue TweenValue(this Slider self, float value, float duration)
		{
			return new RaTweenValue(self, value, duration).Play();
		}
	}

	#endregion
}