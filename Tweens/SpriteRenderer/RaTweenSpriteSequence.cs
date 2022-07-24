using System;
using UnityEngine;
using RaTweening.RaSpriteRenderer;

namespace RaTweening.RaSpriteRenderer
{
	[Serializable]
	public class RaTweenSpriteSequence : RaTween, IRaTweenTarget
	{
		#region Editor Variables

		[Header("Properties")]
		[SerializeField]
		private SpriteRenderer _target = default;

		[SerializeField]
		private Sprite[] _sprites = null;

		#endregion

		#region Properties

		public override bool IsValid => _target != null;

		#endregion

		public RaTweenSpriteSequence()
			: base()
		{
		
		}

		public RaTweenSpriteSequence(SpriteRenderer target, Sprite[] sprites, float duration)
			: base(duration)
		{
			_target = target;
			_sprites = sprites;
		}

		#region Public Methods

		public Type GetTargetTypeRaw() => typeof(SpriteRenderer);

		public void SetTargetRaw(object value)
		{
			if(value is SpriteRenderer renderer)
			{
				_target = renderer;
			}
		}

		public RaTweenSpriteSequence SetTarget(SpriteRenderer target)
		{
			if(CanBeModified())
			{
				_target = target;
			}
			return this;
		}

		public RaTweenSpriteSequence SetSprites(Sprite[] sprites)
		{
			if(CanBeModified())
			{
				_sprites = sprites;
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void Dispose()
		{
			_target = default;
			_sprites = default;
		}

		protected override void Evaluate(float normalizedValue)
		{
			if(_sprites == null || _sprites.Length == 0)
			{
				return;
			}

			int index = Mathf.FloorToInt(_sprites.Length * normalizedValue) % _sprites.Length;
			Sprite currentSprite = _sprites[index];
			_target.sprite = currentSprite;
		}

		protected override RaTween RaTweenClone()
		{
			RaTweenSpriteSequence tween = new RaTweenSpriteSequence();
			tween._sprites = _sprites;
			tween._target = _target;
			return tween;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		public static RaTweenSpriteSequence TweenSpriteSequence(this SpriteRenderer self, Sprite[] sprites, float duration)
		{
			return new RaTweenSpriteSequence(self, sprites, duration).Play();
		}
	}

	#endregion
}