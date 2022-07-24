using RaTweening.UI.RaImage;
using System;
using UnityEngine.UI;

namespace RaTweening.UI.RaImage
{
	[Serializable]
	public class RaTweenFill : RaTweenDynamic<Image, float>
	{
		public RaTweenFill()
			: base()
		{

		}

		public RaTweenFill(Image target, float startFill, float endFill, float duration)
			: base(target, startFill, endFill, duration)
		{

		}

		public RaTweenFill(Image target, float endFill, float duration)
			: base(target, endFill, duration)
		{

		}

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(1f);
			SetEndValue(0f);
		}

		protected override void DynamicEvaluation(float normalizedValue, Image target, float start, float end)
		{
			float delta = end - start;
			target.fillAmount = start + (delta * normalizedValue);
		}

		protected override RaTweenDynamic<Image, float> DynamicClone()
		{
			RaTweenFill tween = new RaTweenFill();
			return tween;
		}

		protected override float ReadValue(Image reference)
		{
			return reference.fillAmount;
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
		public static RaTweenFill TweenFill(this Image self, float alpha, float duration)
		{
			return new RaTweenFill(self, alpha, duration).Play();
		}
	}

	#endregion
}
