using System;
using UnityEngine;
using static RaTweening.RaColorOptions;

namespace RaTweening
{
	[Serializable]
	public class RaTweenSRColor : RaTweenDynamic<SpriteRenderer, Color>
	{
		#region Editor Variables

		[Header("RaTweenSRColor")]
		[SerializeField]
		private Channel _excludeChannels = Channel.None;

		#endregion

		public RaTweenSRColor()
			: base()
		{

		}

		public RaTweenSRColor(SpriteRenderer target, Color startColor, Color endColor, float duration)
			: base(target, startColor, endColor, duration)
		{

		}

		public RaTweenSRColor(SpriteRenderer target, Color endColor, float duration)
			: base(target, endColor, endColor, duration)
		{

		}

		#region Internal Methods

		internal void SetExcludeChannelsAPIInternal(Channel excludeChannels)
		{
			if(CanBeModified())
			{
				_excludeChannels = excludeChannels;
			}
		}

		internal void OnlyIncludeChannelsAPIInternal(Channel inclChannels)
		{
			if(CanBeModified())
			{
				_excludeChannels = GetOnlyIncludeChannels(inclChannels);
			}
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
			RaTweenSRColor tween = new RaTweenSRColor();
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


	#region Extensions

	public static class RaTweenSRColorExtensions
	{
		public static RaTweenSRColor TweenColorR(this SpriteRenderer self, float red, float duration)
		{
			return new RaTweenSRColor(self, Color.white * red, duration)
				.OnlyIncludeChannels(Channel.R)
				.Play();
		}

		public static RaTweenSRColor TweenColorG(this SpriteRenderer self, float green, float duration)
		{
			return new RaTweenSRColor(self, Color.white * green, duration)
				.OnlyIncludeChannels(Channel.G)
				.Play();
		}

		public static RaTweenSRColor TweenColorB(this SpriteRenderer self, float blue, float duration)
		{
			return new RaTweenSRColor(self, Color.white * blue, duration)
				.OnlyIncludeChannels(Channel.B)
				.Play();
		}

		public static RaTweenSRColor TweenColorA(this SpriteRenderer self, float alpha, float duration)
		{
			return new RaTweenSRColor(self, Color.white * alpha, duration)
				.OnlyIncludeChannels(Channel.A)
				.Play();
		}

		public static RaTweenSRColor TweenColor(this SpriteRenderer self, Color color, float duration)
		{
			return new RaTweenSRColor(self, color, duration).Play();
		}

		public static RaTweenSRColor SetExcludeChannels(this RaTweenSRColor self, Channel excludeChannels)
		{
			self.SetExcludeChannelsAPIInternal(excludeChannels);
			return self;
		}

		public static RaTweenSRColor OnlyIncludeChannels(this RaTweenSRColor self, Channel includeChannels)
		{
			self.OnlyIncludeChannelsAPIInternal(includeChannels);
			return self;
		}
	}

	#endregion
}