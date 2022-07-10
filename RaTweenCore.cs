using System;
using RaTweening.Core;

namespace RaTweening
{

	public abstract class RaTweenCore
	{
		#region Events

		private Action _onSetupEvent;
		private Action _onStartEvent;
		private Action _onCompletedEvent;
		private Action _onKillEvent; 

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
		public float Time => _process.Time;

		// Total
		public float TotalTime => _delay.Time + Time;
		public bool IsEmpty => _delay.IsEmpty && _process.IsEmpty;
		public float TotalDuration => Delay + Duration;
		public float TotalProgress => IsEmpty ? 0f : (_delay.Time + _process.Time) / TotalDuration;
		public bool IsCompleted => _process.IsCompleted && HasNoDelayRemaining;

		// Core
		public State TweenState
		{
			get; private set;
		}

		#endregion

		public RaTweenCore(float duration, float delay)
		{
			_delay = new RaTweenTimeline(0f);
			_process = new RaTweenTimeline(0f);
			
			TweenState = State.None;

			SetDuration(duration);
			SetDelay(delay);
		}

		#region Public Methods

		public RaTweenCore Play()
		{
			return RaTweeningCore.Instance.RegisterTween(this);
		}

		public RaTweenCore Kill()
		{
			SetStateInternal(State.Dead);
			return this;
		}

		public RaTweenCore ListenToSetup(Action callback)
		{
			if(CanBeModified())
			{
				_onSetupEvent += callback;
			}
			return this;
		}

		public RaTweenCore ListenToStart(Action callback)
		{
			if(CanBeModified())
			{
				_onStartEvent += callback;
			}
			return this;
		}

		public RaTweenCore ListenToComplete(Action callback)
		{
			if(CanBeModified())
			{
				_onCompletedEvent += callback;
			}
			return this;
		}

		public RaTweenCore ListenToKill(Action callback)
		{
			if(CanBeModified())
			{
				_onKillEvent += callback;
			}
			return this;
		}

		public RaTweenCore SetDelay(float delay)
		{
			if(CanBeModified())
			{
				_delay.SetDuration(delay);
			}
			return this;
		}

		public RaTweenCore Clone()
		{
			RaTweenCore tween = CloneSelf();
			return tween;
		}

		public bool CanBeModified()
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

		#endregion

		#region Internal Methods

		internal void SetDefaultValuesInternal()
		{
			SetDefaultValues();
		}

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

			DisposeInternal();

			_onKillEvent = null;
			_onCompletedEvent = null;
			_onStartEvent = null;
			_onSetupEvent = null;
		}

		internal void DisposeInternal()
		{
			Dispose();
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

		protected abstract void SetDefaultValues();

		protected RaTweenCore SetDuration(float duration)
		{
			if(CanBeModified())
			{
				_process.SetDuration(duration);
			}
			return this;
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

		protected abstract RaTweenCore CloneSelf();
		protected abstract void Evaluate(float normalizedValue);
		protected abstract void Dispose();
		protected abstract void PerformEvaluation();

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
			Data,
		}

		#endregion
	}
}