using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public class RaTweenSRSprites : RaTween, IRaTweenTarget
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

		public RaTweenSRSprites()
			: base()
		{
		
		}

		public RaTweenSRSprites(SpriteRenderer target, Sprite[] sprites, float duration)
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

		public RaTweenSRSprites SetTarget(SpriteRenderer target)
		{
			if(CanBeModified())
			{
				_target = target;
			}
			return this;
		}

		public RaTweenSRSprites SetSprites(Sprite[] sprites)
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
			RaTweenSRSprites tween = new RaTweenSRSprites();
			tween._sprites = _sprites;
			tween._target = _target;
			return tween;
		}

		#endregion
	}

	#region Extensions

	public static class RaTweenSRSpritesExtensions
	{
		public static RaTweenSRSprites TweenSprites(this SpriteRenderer self, Sprite[] sprites, float duration)
		{
			return new RaTweenSRSprites(self, sprites, duration).Play();
		}
	}

	#endregion
}