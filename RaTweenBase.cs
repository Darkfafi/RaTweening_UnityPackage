using System;
using UnityEngine;

namespace RaTweening
{
	public abstract class RaTweenBase
	{
		#region Events

		private Action _onSetupEvent;
		private Action _onStartEvent;
		private Action _onCompletedEvent;
		private Action _onKillEvent; 

		#endregion

		#region Editor Variables

		[SerializeField]
		private AnimationCurve _easing = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		#endregion

		#region Variables

		private RaTweenTimeline _process;
		private RaTweenTimeline _delay;

		#endregion

		#region Properties

		public abstract bool IsValid
		{
			get;
		}

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
			_delay = new RaTweenTimeline(0f);
			_process = new RaTweenTimeline(0f);
			
			TweenState = State.None;

			SetEasing(easing);
			SetDelay(delay);
		}

		#region Public Methods

		public RaTweenBase Play()
		{
			return RaTweeningCore.Instance.RegisterTween(this);
		}

		public RaTweenBase Kill()
		{
			SetStateInternal(State.Dead);
			return this;
		}

		public RaTweenBase ListenToSetup(Action callback)
		{
			if(CanBeModified())
			{
				_onSetupEvent += callback;
			}
			return this;
		}

		public RaTweenBase ListenToStart(Action callback)
		{
			if(CanBeModified())
			{
				_onStartEvent += callback;
			}
			return this;
		}

		public RaTweenBase ListenToComplete(Action callback)
		{
			if(CanBeModified())
			{
				_onCompletedEvent += callback;
			}
			return this;
		}

		public RaTweenBase ListenToKill(Action callback)
		{
			if(CanBeModified())
			{
				_onKillEvent += callback;
			}
			return this;
		}

		public RaTweenBase SetDelay(float delay)
		{
			if(CanBeModified())
			{
				_delay.SetDuration(delay);
			}
			return this;
		}

		public RaTweenBase SetEasing(AnimationCurve easing)
		{
			if(CanBeModified())
			{
				easing = easing ?? AnimationCurve.Linear(0f, 0f, 0f, 0f);

				float duration = 0f;
				if(easing.keys.Length > 0)
				{
					duration = easing.keys[easing.keys.Length - 1].time;
				}
				_process = new RaTweenTimeline(duration);
				_easing = easing;
			}
			return this;
		}

		public RaTweenBase Clone()
		{
			RaTweenBase tween = CloneSelf();
			return tween;
		}

		#endregion

		#region Internal Methods

		internal void SetupInternal()
		{
			_delay.Reset();
			_process.Reset();
			OnSetup();
			_onSetupEvent?.Invoke();
		}

		internal void StartInternal()
		{
			_process.Reset();
			
			OnStart();
			_onStartEvent?.Invoke();

			PerformEvaluation();
		}

		internal void StepDelayInternal(float deltaTime)
		{
			_delay.Step(deltaTime);
		}

		internal void StepTweenInternal(float deltaTime)
		{
			_process.Step(deltaTime);
			PerformEvaluation();
		}

		internal void KillInternal()
		{
			OnKill();
			_onKillEvent?.Invoke();
			
			Dispose();

			_onKillEvent = null;
			_onCompletedEvent = null;
			_onStartEvent = null;
			_onSetupEvent = null;
		}

		internal void CompleteInternal()
		{
			_process.Complete();
			OnComplete();
			_onCompletedEvent?.Invoke();
			PerformEvaluation();
		}

		internal void SetStateInternal(State state)
		{
			TweenState = state;
		}

		#endregion

		#region Protected Methods

		protected bool CanBeModified()
		{
			switch(TweenState)
			{
				case State.None:
				case State.ToStart:
					return true;
				default:
					return false;
			}
		}

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
			InDelay,
			Started,
			InProgress,
			Completed,
			Dead,
		}

		#endregion
	}
}