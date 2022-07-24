using System;
using UnityEngine;
using static RaTweening.RaColorOptions;
using RaTweening.RaSpriteRenderer;

namespace RaTweening.RaSpriteRenderer
{
	[Serializable]
	public class RaTweenColor : RaTweenDynamic<SpriteRenderer, Color>
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

		public RaTweenColor(SpriteRenderer target, Color startColor, Color endColor, float duration)
			: base(target, startColor, endColor, duration)
		{

		}

		public RaTweenColor(SpriteRenderer target, Color endColor, float duration)
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

		protected override void DynamicEvaluation(float normalizedValue, SpriteRenderer target, Color start, Color end)
		{
			Color delta = end - start;
			target.color = ApplyExcludeChannels(target.color, start + (delta * normalizedValue), _excludeChannels);
		}

		protected override RaTweenDynamic<SpriteRenderer, Color> DynamicClone()
		{
			RaTweenColor tween = new RaTweenColor();
			tween._excludeChannels = _excludeChannels;
			return tween;
		}

		protected override Color ReadValue(SpriteRenderer reference)
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
		public static RaTweenColor TweenColorR(this SpriteRenderer self, float red, float duration)
		{
			return new RaTweenColor(self, Color.white * red, duration)
				.OnlyIncludeChannels(Channel.R)
				.Play();
		}

		public static RaTweenColor TweenColorG(this SpriteRenderer self, float green, float duration)
		{
			return new RaTweenColor(self, Color.white * green, duration)
				.OnlyIncludeChannels(Channel.G)
				.Play();
		}

		public static RaTweenColor TweenColorB(this SpriteRenderer self, float blue, float duration)
		{
			return new RaTweenColor(self, Color.white * blue, duration)
				.OnlyIncludeChannels(Channel.B)
				.Play();
		}

		public static RaTweenColor TweenColorA(this SpriteRenderer self, float alpha, float duration)
		{
			return new RaTweenColor(self, Color.white * alpha, duration)
				.OnlyIncludeChannels(Channel.A)
				.Play();
		}

		public static RaTweenColor TweenColor(this SpriteRenderer self, Color color, float duration)
		{
			return new RaTweenColor(self, color, duration).Play();
		}
	}

	#endregion
}