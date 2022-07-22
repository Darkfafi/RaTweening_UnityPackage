using UnityEngine;
using RaTweening.Core;

namespace RaTweening
{
	[RaTweenerElement(typeof(RaTweenerSerializableElement))]
	public abstract class RaTween : RaTweenCore
	{
		#region Editor Variables

		[Header("Tween Settings")]
		[SerializeField]
		private RaEasingType _easingType = RaEasingType.Linear;
		[SerializeField]
		private float _duration = 1f;

		[Header("Tween Animation Curve")]
		[SerializeField]
		private bool _useAnimationCurveEasing = false;
		[SerializeField]
		private AnimationCurve _animationCurveEasing = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		[SerializeField]
		private bool _useAnimationCurveEasingDuration = false;


		[Header("Modifier Settings")]
		[SerializeField]
		private RaModifierType _modifierType = RaModifierType.None;

		[Header("Tween Modifier Curve")]
		[SerializeField]
		private bool _useAnimationCurveModifier = false;
		[SerializeField]
		private AnimationCurve _animationCurveModifier = AnimationCurve.Linear(0f, 0f, 1f, 1f);


		#endregion

		public RaTween()
			: base(0f)
		{
			SetDuration(_duration);

			if(_useAnimationCurveEasing)
			{
				SetEasingAPIInternal(_animationCurveEasing, _useAnimationCurveEasingDuration);
			}
			else
			{
				SetEasingAPIInternal(_easingType);
			}

			if(_useAnimationCurveModifier)
			{
				SetModifierAPIInternal(_animationCurveModifier);
			}
			else
			{
				SetModifierAPIInternal(_modifierType);
			}
		}

		public RaTween(float duration)
			: base(duration)
		{
			_duration = duration;
			_easingType = RaEasingType.Linear;
			_useAnimationCurveEasing = false;
			_useAnimationCurveEasingDuration = false;
		}

		#region Internal Methods

		internal RaTween SetEasingAPIInternal(RaEasingType easing)
		{
			if(CanBeModified())
			{
				_easingType = easing;
				_useAnimationCurveEasing = false;
				_useAnimationCurveEasingDuration = false;
			}
			return this;
		}

		internal RaTween SetEasingAPIInternal(AnimationCurve easing, bool inclDuration)
		{
			if(CanBeModified())
			{
				easing = easing ?? AnimationCurve.Linear(0f, 0f, 0f, 0f);

				if(_useAnimationCurveEasingDuration = inclDuration)
				{
					_duration = 0f;
					if(easing.keys.Length > 0)
					{
						_duration = easing.keys[easing.keys.Length - 1].time;
					}

					SetDuration(_duration);
				}
				_animationCurveEasing = easing;
				_useAnimationCurveEasing = true;
			}
			return this;
		}

		internal RaTween AddModifierAPIInternal(RaModifierType modifier)
		{
			return SetModifierAPIInternal(_modifierType | modifier);
		}

		internal RaTween SetModifierAPIInternal(RaModifierType modifier)
		{
			if(CanBeModified())
			{
				_modifierType = modifier;
				_useAnimationCurveModifier = false;
			}
			return this;
		}

		internal RaTween SetModifierAPIInternal(AnimationCurve modifier)
		{
			if(CanBeModified())
			{
				modifier = modifier ?? AnimationCurve.Linear(0f, 0f, 0f, 0f);
				_animationCurveModifier = modifier;
				_useAnimationCurveModifier = true;
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected abstract RaTween RaTweenClone();

		protected override RaTweenCore CloneSelf()
		{
			RaTween tween = RaTweenClone();

			// Easing
			tween._easingType = _easingType;
			tween._animationCurveEasing = _animationCurveEasing;
			tween._useAnimationCurveEasing = _useAnimationCurveEasing;
			tween._useAnimationCurveEasingDuration = _useAnimationCurveEasingDuration;

			// Modifier
			tween._modifierType = _modifierType;
			tween._animationCurveModifier = _animationCurveModifier;
			tween._useAnimationCurveModifier = _useAnimationCurveModifier;

			// Generic
			tween.SetDuration(_duration);

			// Post Easing
			if(_useAnimationCurveEasing)
			{
				tween.SetEasingAPIInternal(_animationCurveEasing, _useAnimationCurveEasingDuration);
			}

			return tween;
		}

		protected override void OnSetDuration(float duration)
		{
			_duration = duration;
		}

		protected override void SetDefaultValues()
		{
			_animationCurveModifier = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			_easingType = RaEasingType.Linear;
			_duration = 1f;
		}

		protected override float CalculateEvaluation()
		{
			float modifiedProgress = ApplyEvaluationModifier(Progress);
			if(_useAnimationCurveEasing)
			{
				return _animationCurveModifier.Evaluate(_useAnimationCurveEasingDuration ? modifiedProgress * Duration : modifiedProgress);
			}
			else
			{
				return RaTweenEasing.Evaluate(_easingType, modifiedProgress);
			}
		}

		protected float ApplyEvaluationModifier(float value)
		{
			if(_useAnimationCurveModifier)
			{
				return _animationCurveModifier.Evaluate(value);
			}
			else
			{
				return RaTweenModifier.ApplyModifier(_modifierType, value);
			}
		}

		#endregion
	}

	public static class RaTweenExtensions
	{
		public static TweenT SetEasing<TweenT>(this TweenT self, RaEasingType easing)
			where TweenT : RaTween
		{
			self.SetEasingAPIInternal(easing);
			return self;
		}

		public static TweenT SetEasing<TweenT>(this TweenT self, AnimationCurve easing, bool inclDuration)
			where TweenT : RaTween
		{
			self.SetEasingAPIInternal(easing, inclDuration);
			return self;
		}

		public static TweenT AddModifier<TweenT>(this TweenT self, RaModifierType modifier)
			where TweenT : RaTween
		{
			self.AddModifierAPIInternal(modifier);
			return self;
		}

		public static TweenT SetModifier<TweenT>(this TweenT self, RaModifierType modifier)
			where TweenT : RaTween
		{
			self.SetModifierAPIInternal(modifier);
			return self;
		}

		public static TweenT SetModifier<TweenT>(this TweenT self, AnimationCurve modifier)
			where TweenT : RaTween
		{
			self.SetModifierAPIInternal(modifier);
			return self;
		}
	}
}