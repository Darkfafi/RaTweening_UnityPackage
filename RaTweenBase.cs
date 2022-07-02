using UnityEngine;

namespace RaTweening
{
	public abstract class RaTweenBase
	{
		#region Editor Variables

		[SerializeField]
		private AnimationCurve _easing = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		#endregion

		#region Variables

		private RaTweenTimeline _process;
		private RaTweenTimeline _delay;

		#endregion

		#region Properties

		// Delay
		public float Delay => _delay.Duration;
		public float DelayProgress => _delay.Progress;
		public bool HasNoDelayRemaining => _delay.IsCompleted;

		// Process
		public float Duration => _process.Duration;
		public float Progress => _process.Progress;

		// Total
		public bool IsEmpty => _delay.IsEmpty && _process.IsEmpty;
		public float TotalDuration => Delay + Duration;
		public float TotalProgress => IsEmpty ? 0f : (_delay.Time + _process.Time) / TotalDuration;
		public bool IsCompleted => _process.IsCompleted && HasNoDelayRemaining;

		// Core
		public AnimationCurve Easing => _easing;
		public State TweenState
		{
			get; private set;
		}

		#endregion

		public RaTweenBase(AnimationCurve easing, float delay)
		{
			_delay = new RaTweenTimeline(delay);

			float duration = 0f;
			if(easing.keys.Length > 0)
			{
				duration = easing.keys[easing.keys.Length - 1].time;
			}
			_process = new RaTweenTimeline(duration);
			_easing = easing;

			TweenState = State.None;
		}

		#region Public Methods

		public RaTweenBase Clone()
		{
			RaTweenBase tween = CloneSelf();
			return tween;
		}

		#endregion

		#region Internal Methods

		internal void Setup()
		{
			_delay.Reset();
			_process.Reset();
			OnSetup();
		}

		internal void Start()
		{
			_process.Reset();
			OnStart();
			PerformEvaluation();
		}

		internal void StepDelay(float deltaTime)
		{
			_delay.Step(deltaTime);
		}

		internal void StepTween(float deltaTime)
		{
			_process.Step(deltaTime);
			PerformEvaluation();
		}

		internal void Kill()
		{
			OnKill();
			Dispose();
		}

		internal void Complete()
		{
			_process.Complete();
			OnComplete();
			PerformEvaluation();
		}

		internal void SetState(State state)
		{
			TweenState = state;
		}

		#endregion

		#region Protected Methods

		protected virtual void OnSetup()
		{
		
		}

		protected virtual void OnStart()
		{

		}

		protected virtual void OnComplete()
		{

		}

		protected virtual void OnKill()
		{

		}

		protected abstract RaTweenBase CloneSelf();
		protected abstract void Evaluate(float normalizedValue);
		protected abstract void Dispose();

		#endregion

		#region Private Methods

		private void PerformEvaluation()
		{
			Evaluate(_easing.Evaluate(_process.Time));
		}

		#endregion

		#region Internal

		public enum State
		{
			None,
			ToStart,
			Started,
			InProgress,
			Completed,
			Dead,
		}

		#endregion
	}
}