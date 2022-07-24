using UnityEngine;
using RaTweening.Core;
using RaTweening.Tools;

namespace RaTweening
{
	[RaTweenerElement(typeof(RaTweenerSerializableElement))]
	public abstract class RaTween : RaTweenCore
	{
		#region Editor Variables

		[Header("Tween Settings")]
		[SerializeField]
		[ModifierField(nameof(_useCurveEasing), true, ModifierFieldAttribute.ModType.DontDraw)]
		private RaEasingType _easingType = RaEasingType.Linear;
		
		[SerializeField]
		[ModifierField(nameof(_useCurveEasing), false, ModifierFieldAttribute.ModType.DontDraw)]
		private AnimationCurve _curveEasing = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField]
		[ModifierField(nameof(_useCurveEasingDuration), true, ModifierFieldAttribute.ModType.DontDraw)]
		private float _duration = 1f;

		[SerializeField]
		[ModifierField(nameof(_useCurveEasing), false, ModifierFieldAttribute.ModType.DontDraw)]
		private bool _useCurveEasingDuration = false;

		[SerializeField]
		private bool _useCurveEasing = false;

		[Header("Modifier Settings")]
		[SerializeField]
		[ModifierField(nameof(_useCurveModifier), true, ModifierFieldAttribute.ModType.DontDraw)]
		private RaModifierType _modifierType = RaModifierType.None;

		[SerializeField]
		[ModifierField(nameof(_useCurveModifier), false, ModifierFieldAttribute.ModType.DontDraw)]
		private AnimationCurve _curveModifier = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField]
		private bool _useCurveModifier = false;

		#endregion

		public RaTween()
			: base(0f)
		{
			SetDuration(_duration);

			if(_useCurveEasing)
			{
				SetEasingAPIInternal(_curveEasing, _useCurveEasingDuration);
			}
			else
			{
				SetEasingAPIInternal(_easingType);
			}

			if(_useCurveModifier)
			{
				SetModifierAPIInternal(_curveModifier);
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
			_useCurveEasing = false;
			_useCurveEasingDuration = false;
		}

		#region Internal Methods

		internal void SetEasingAPIInternal(RaEasingType easing)
		{
			if(CanBeModified())
			{
				_easingType = easing;
				_useCurveEasing = false;
				_useCurveEasingDuration = false;
			}
		}

		internal void SetEasingAPIInternal(AnimationCurve easing, bool inclDuration)
		{
			if(CanBeModified())
			{
				_curveEasing = easing ?? AnimationCurve.Linear(0f, 0f, 0f, 0f);
				_useCurveEasing = true;

				if(_useCurveEasingDuration = inclDuration)
				{
					_duration = 0f;
					if(_curveEasing.keys.Length > 0)
					{
						_duration = _curveEasing.keys[_curveEasing.keys.Length - 1].time;
					}

					SetDuration(_duration);
				}
			}
		}

		internal void AddModifierAPIInternal(RaModifierType modifier)
		{
			SetModifierAPIInternal(_modifierType | modifier);
		}

		internal void SetModifierAPIInternal(RaModifierType modifier)
		{
			if(CanBeModified())
			{
				_modifierType = modifier;
				_useCurveModifier = false;
			}
		}

		internal void SetModifierAPIInternal(AnimationCurve modifier)
		{
			if(CanBeModified())
			{
				modifier = modifier ?? AnimationCurve.Linear(0f, 0f, 0f, 0f);
				_curveModifier = modifier;
				_useCurveModifier = true;
			}
		}

		#endregion

		#region Protected Methods

		protected abstract RaTween RaTweenClone();

		protected override RaTweenCore CloneSelf()
		{
			RaTween tween = RaTweenClone();

			// Easing
			tween._easingType = _easingType;
			tween._curveEasing = _curveEasing;
			tween._useCurveEasing = _useCurveEasing;
			tween._useCurveEasingDuration = _useCurveEasingDuration;

			// Modifier
			tween._modifierType = _modifierType;
			tween._curveModifier = _curveModifier;
			tween._useCurveModifier = _useCurveModifier;

			// Generic
			tween.SetDuration(_duration);

			// Post Easing
			if(tween._useCurveEasing)
			{
				tween.SetEasingAPIInternal(_curveEasing, _useCurveEasingDuration);
			}

			return tween;
		}

		protected override void OnSetDuration(float duration)
		{
			_duration = duration;
		}

		protected override void SetDefaultValues()
		{
			_curveModifier = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			_easingType = RaEasingType.Linear;
			_duration = 1f;
		}

		protected override float CalculateEvaluation()
		{
			float modifiedProgress = ApplyEvaluationModifier(Progress);
			if(_useCurveEasing)
			{
				return _curveEasing.Evaluate(_useCurveEasingDuration ? modifiedProgress * Duration : modifiedProgress);
			}
			else
			{
				return RaTweenEasing.Evaluate(_easingType, modifiedProgress);
			}
		}

		protected float ApplyEvaluationModifier(float value)
		{
			if(_useCurveModifier)
			{
				return _curveModifier.Evaluate(value);
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