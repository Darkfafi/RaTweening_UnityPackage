using RaTweening.Core;
using System;

namespace RaTweening
{
	public abstract class RaTweenCore
	{
		#region Consts

		public const int InfiniteLoopingValue = -1;

		#endregion

		#region Events

		public delegate void CallbackHandler();
		public delegate void LoopCallbackHandler(int loopCount);

		private CallbackHandler _onSetupEvent;
		private CallbackHandler _onStartEvent;
		private CallbackHandler _onCompletedEvent;
		private CallbackHandler _onKillEvent;
		private LoopCallbackHandler _onLoopEvent;

		#endregion

		#region Variables

		private RaTweenTimeline _process;
		private RaTweenTimeline _delay;

		// Tracker
		private int _loopCount = 0;

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

		// Progress And Delay
		public float TotalTime => _delay.Time + Time;
		public float TotalDuration => Delay + Duration;
		public float TotalProgress => IsEmpty ? 0f : TotalTime / TotalDuration;
		public bool IsTotalCompleted => _process.IsCompleted && HasNoDelayRemaining;

		// Looping
		public bool IsLoop => Loops != 0;
		public bool IsInfiniteLoop => Loops == InfiniteLoopingValue;

		public int Loops
		{
			get; private set;
		}

		public float TotalLoopingTime
		{
			get
			{
				if(IsInfiniteLoop)
				{
					return InfiniteLoopingValue;
				}

				return TotalTime + (_loopCount * TotalDuration);
			}
		}

		public float TotalLoopingDuration
		{
			get
			{
				if(IsInfiniteLoop)
				{
					return InfiniteLoopingValue;
				}

				return TotalDuration + (Loops * TotalDuration);
			}
		}

		public float TotalLoopingProgress
		{
			get
			{
				if(IsInfiniteLoop)
				{
					return InfiniteLoopingValue;
				}

				return IsEmpty ? 0f : TotalLoopingTime / TotalLoopingDuration;
			}
		}

		public bool HasReachedLoopEnd
		{
			get
			{
				if(IsFinite)
				{
					return _loopCount >= Loops;
				}

				return false;
			}
		}

		// Core
		public bool IsFinite => Loops >= 0;
		public bool IsEmpty => _delay.IsEmpty && _process.IsEmpty;
		public State TweenState
		{
			get; private set;
		}

		public bool IsCompleted
		{
			get; private set;
		}

		#endregion

		public RaTweenCore(float duration)
		{
			_delay = new RaTweenTimeline(0f);
			_process = new RaTweenTimeline(0f);

			TweenState = State.None;

			SetDuration(duration);
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

		public RaTweenCore ListenToSetup(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onSetupEvent += callback;
			}
			return this;
		}

		public RaTweenCore ListenToStart(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onStartEvent += callback;
			}
			return this;
		}

		public RaTweenCore ListenToLoop(LoopCallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onLoopEvent += callback;
			}
			return this;
		}

		public RaTweenCore ListenToComplete(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onCompletedEvent += callback;
			}
			return this;
		}

		public RaTweenCore ListenToKill(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onKillEvent += callback;
			}
			return this;
		}

		public RaTweenCore SetLooping(int loopAmount)
		{
			if(CanBeModified())
			{
				Loops = loopAmount;
			}
			return this;
		}

		public RaTweenCore SetInfiniteLooping()
		{
			return SetLooping(InfiniteLoopingValue);
		}

		public RaTweenCore DisableLooping()
		{
			return SetLooping(0);
		}

		public RaTweenCore SetDelay(float delay)
		{
			if(CanBeModified())
			{
				_delay.SetDuration(delay);
			}
			return this;
		}

		public virtual RaTweenCore Clone()
		{
			RaTweenCore tween = CloneSelf();
			tween.SetDelay(Delay);
			tween.SetLooping(Loops);
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
			IsCompleted = false;

			_loopCount = 0;
			
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

		internal void LoopInternal()
		{
			_loopCount++;
			_delay.Reset();
			_process.Reset();
			
			OnLoop();
			_onLoopEvent?.Invoke(_loopCount);
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
			_onLoopEvent = null;
		}

		internal void DisposeInternal()
		{
			Dispose();
		}

		internal void CompleteInternal()
		{
			IsCompleted = true;

			_delay.Complete();
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

		protected virtual void OnLoop()
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