using UnityEngine;

namespace RaTweening
{
	public abstract class RaTween : RaTweenCore
	{
		#region Editor Variables

		[SerializeField]
		private AnimationCurve _easing = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		#endregion

		#region Properties

		public AnimationCurve Easing => _easing;

		#endregion

		public RaTween()
			: this(AnimationCurve.Linear(0f, 0f, 1f, 1f), 0f)
		{ 
		}

		public RaTween(AnimationCurve curve, float delay)
			: base(0f, delay)
		{
			SetEasing(curve);
		}

		#region Public Methods

		public RaTweenCore SetEasing(AnimationCurve easing)
		{
			if(CanBeModified())
			{
				easing = easing ?? AnimationCurve.Linear(0f, 0f, 0f, 0f);

				float duration = 0f;
				if(easing.keys.Length > 0)
				{
					duration = easing.keys[easing.keys.Length - 1].time;
				}

				SetDuration(duration);
				_easing = easing;
			}
			return this;
		}

		#endregion

		#region Protected Methods
		protected override void SetDefaultValues()
		{
			SetEasing(AnimationCurve.Linear(0f, 0f, 1f, 1f));
		}

		protected override void PerformEvaluation()
		{
			Evaluate(_easing.Evaluate(Time));
		}

		#endregion
	}
}