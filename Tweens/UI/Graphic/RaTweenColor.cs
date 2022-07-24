using RaTweening.UI.RaGraphic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static RaTweening.RaColorOptions;

namespace RaTweening.UI.RaGraphic
{
	[Serializable]
	public class RaTweenColor : RaTweenDynamic<Graphic, Color>
	{
		#region Editor Variables

		[Header("RaTweenColor")]
		[SerializeField]
		private Channel _excludeChannels = Channel.None;

		#endregion

		public RaTweenColor()
			: base()
		{

		}

		public RaTweenColor(Graphic target, Color startColor, Color endColor, float duration)
			: base(target, startColor, endColor, duration)
		{

		}

		public RaTweenColor(Graphic target, Color endColor, float duration)
			: base(target, endColor, duration)
		{

		}

		#region Public Methods

		public RaTweenColor SetExcludeChannels(Channel excludeChannels)
		{
			if(CanBeModified())
			{
				_excludeChannels = excludeChannels;
			}
			return this;
		}

		public RaTweenColor OnlyIncludeChannels(Channel inclChannels)
		{
			if(CanBeModified())
			{
				_excludeChannels = GetOnlyIncludeChannels(inclChannels);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(Color.white);
			SetEndValue(Color.white);
		}

		protected override void DynamicEvaluation(float normalizedValue, Graphic target, Color start, Color end)
		{
			Color delta = end - start;
			target.color = ApplyExcludeChannels(target.color, start + (delta * normalizedValue), _excludeChannels);
		}

		protected override RaTweenDynamic<Graphic, Color> DynamicClone()
		{
			RaTweenColor tween = new RaTweenColor();
			tween._excludeChannels = _excludeChannels;
			return tween;
		}

		protected override Color ReadValue(Graphic reference)
		{
			return reference.color;
		}

		protected override Color GetEndByDelta(Color start, Color delta)
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
		public static RaTweenColor TweenColorR(this Graphic self, float red, float duration)
		{
			return new RaTweenColor(self, Color.white * red, duration)
				.OnlyIncludeChannels(Channel.R)
				.Play();
		}

		public static RaTweenColor TweenColorG(this Graphic self, float green, float duration)
		{
			return new RaTweenColor(self, Color.white * green, duration)
				.OnlyIncludeChannels(Channel.G)
				.Play();
		}

		public static RaTweenColor TweenColorB(this Graphic self, float blue, float duration)
		{
			return new RaTweenColor(self, Color.white * blue, duration)
				.OnlyIncludeChannels(Channel.B)
				.Play();
		}

		public static RaTweenColor TweenColorA(this Graphic self, float alpha, float duration)
		{
			return new RaTweenColor(self, Color.white * alpha, duration)
				.OnlyIncludeChannels(Channel.A)
				.Play();
		}

		public static RaTweenColor TweenColor(this Graphic self, Color color, float duration)
		{
			return new RaTweenColor(self, color, duration).Play();
		}
	}

	#endregion
}