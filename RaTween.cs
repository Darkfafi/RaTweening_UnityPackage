using UnityEngine;

namespace RaTweening
{
	[RaTweenerElement(typeof(RaTweenerSerializableElement))]
	public abstract class RaTween : RaTweenCore
	{
		#region Editor Variables

		[Header("Tween Settings")]
		[SerializeField]
		private EasingType _easing = EasingType.Linear;
		[SerializeField]
		private float _duration = 1f;


		[Header("Animation Curve")]
		[SerializeField]
		private bool _useAnimationCurve = false;
		[SerializeField]
		private AnimationCurve _animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		[SerializeField]
		private bool _useAnimationCurveDuration = false;

		#endregion

		public RaTween()
			: base(0f)
		{
			SetDuration(_duration);

			if(_useAnimationCurve)
			{
				SetEasing(_animationCurve, _useAnimationCurveDuration);
			}
			else
			{
				SetEasing(_easing);
			}
		}

		public RaTween(float duration)
			: base(duration)
		{
			_duration = duration;
			_easing = EasingType.Linear;
			_useAnimationCurve = false;
			_useAnimationCurveDuration = false;
		}

		#region Public Methods

		public RaTween SetEasing(EasingType easing)
		{
			if(CanBeModified())
			{
				_easing = easing;
				_useAnimationCurve = false;
				_useAnimationCurveDuration = false;
			}
			return this;
		}

		public RaTween SetEasing(AnimationCurve easing, bool inclDuration)
		{
			if(CanBeModified())
			{
				easing = easing ?? AnimationCurve.Linear(0f, 0f, 0f, 0f);

				if(_useAnimationCurveDuration = inclDuration)
				{
					_duration = 0f;
					if(easing.keys.Length > 0)
					{
						_duration = easing.keys[easing.keys.Length - 1].time;
					}

					SetDuration(_duration);
				}
				_animationCurve = easing;
				_useAnimationCurve = true;
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected abstract RaTween RaTweenClone();

		protected override RaTweenCore CloneSelf()
		{
			RaTween tween = RaTweenClone();

			tween._easing = _easing;
			tween._animationCurve = _animationCurve;
			tween._useAnimationCurve = _useAnimationCurve;
			tween._useAnimationCurveDuration = _useAnimationCurveDuration;

			tween.SetDuration(_duration);

			if(_useAnimationCurve)
			{
				tween.SetEasing(_animationCurve, _useAnimationCurveDuration);
			}

			return tween;
		}

		protected override void SetDefaultValues()
		{
			_animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			_easing = EasingType.Linear;
			_duration = 1f;
		}

		protected override float CalculateEvaluation()
		{
			return _useAnimationCurve ?
				_animationCurve.Evaluate(_useAnimationCurveDuration ? Time : Progress) :
				RaTweenEasing.Evaluate(_easing, Progress);
		}

		#endregion
	}
}