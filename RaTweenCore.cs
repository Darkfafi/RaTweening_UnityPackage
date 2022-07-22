using RaTweening.Core;
using System;
using UnityEngine;

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

		#region Editor Variables

		[SerializeField, HideInInspector]
		private int _loopsSerialized = 0;

		[SerializeField, HideInInspector]
		private float _delaySerialized = 0f;

		#endregion

		#region Variables

		private RaTweenProgressor _process;
		private RaTweenProgressor _delay;

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
		public bool IsInfinite => _process.IsInfinite;
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

		public int Loops => _loopsSerialized;

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
				if(Loops >= 0)
				{
					return _loopCount >= Loops;
				}

				return false;
			}
		}

		// Core
		public bool IsEmpty => _delay.IsEmpty && _process.IsEmpty;
		
		public State TweenState
		{
			get; private set;
		}

		public bool IsPlaying
		{
			get
			{
				switch(TweenState)
				{
					case State.InDelay:
					case State.InProgress:
						return true;
					default:
						return false;
				}
			}
		}

		public bool IsPaused
		{
			get
			{
				return TweenState == State.IsPaused;
			}
		}

		public bool IsCompleted
		{
			get; private set;
		}

		#endregion

		public RaTweenCore(float duration)
		{
			_delay = new RaTweenProgressor(0f);
			_process = new RaTweenProgressor(0f);

			TweenState = State.None;

			SetDuration(duration);
		}

		#region Public Methods

		public bool Pause()
		{
			if(CanUseAPI() && IsPlaying)
			{
				SetStateInternal(State.IsPaused);
				return true;
			}
			return false;
		}

		public bool Resume()
		{
			if(CanUseAPI() && IsPaused)
			{
				SetStateInternal(State.ToResume);
				return true;
			}
			return false;
		}

		public bool Kill()
		{
			if(CanUseAPI())
			{
				SetStateInternal(State.Dead);
				return true;
			}
			return false;
		}

		public RaTweenCore Clone()
		{
			RaTweenCore tween = CloneSelf();
			tween.SetDelayAPIInternal(_delaySerialized);
			tween.SetLoopingAPIInternal(_loopsSerialized);
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

		internal RaTweenCore SetDelayAPIInternal(float delay)
		{
			if(CanBeModified())
			{
				_delaySerialized = delay;
				_delay.SetDuration(delay);
			}
			return this;
		}

		internal RaTweenCore SetLoopingAPIInternal(int loopAmount)
		{
			if(CanBeModified())
			{
				_loopsSerialized = loopAmount;
			}
			return this;
		}

		internal RaTweenCore OnSetupAPIInternal(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onSetupEvent += callback;
			}
			return this;
		}

		public RaTweenCore OnStartAPIInternal(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onStartEvent += callback;
			}
			return this;
		}

		internal RaTweenCore OnLoopAPIInternal(LoopCallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onLoopEvent += callback;
			}
			return this;
		}

		internal RaTweenCore OnCompleteAPIInternal(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onCompletedEvent += callback;
			}
			return this;
		}

		internal RaTweenCore OnKillAPIInternal(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onKillEvent += callback;
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected abstract void SetDefaultValues();

		protected RaTweenCore SetInfiniteDuration()
		{
			if(CanBeModified())
			{
				_process.SetInfiniteDuration();
			}
			return this;
		}

		protected RaTweenCore SetDuration(float duration)
		{
			if(CanBeModified())
			{
				_process.SetDuration(duration);
				OnSetDuration(duration);
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

		protected virtual void OnSetDuration(float duration)
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
		protected abstract float CalculateEvaluation();

		#endregion

		#region Private Methods

		private void PerformEvaluation()
		{
			Evaluate(CalculateEvaluation());
		}

		private bool CanUseAPI()
		{
			switch(TweenState)
			{
				case State.Data:
				case State.Dead:
				case State.Completed:
					return false;
			}

			return !IsCompleted;
		}

		#endregion

		#region Internal

		public enum State
		{
			// Idle
			None		= 0,

			// Intent
			ToStart		= 101,
			ToResume	= 102,
			
			// Lifecycle
			InDelay		= 201,
			Started		= 202,
			InProgress	= 203,
			IsPaused	= 204,
			Completed	= 205,
			Dead		= 206,

			// Type Lock
			Data		= 301,
		}

		#endregion
	}

	public static class RaTweenCoreExtensions
	{
		public static TweenT Play<TweenT>(this TweenT self)
			where TweenT : RaTweenCore
		{
			return RaTweeningCore.Instance.RegisterTween(self);
		}

		public static TweenT OnSetup<TweenT>(this TweenT self, RaTweenCore.CallbackHandler callback)
			where TweenT : RaTweenCore
		{
			self.OnSetupAPIInternal(callback);
			return self;
		}

		public static TweenT OnStart<TweenT>(this TweenT self, RaTweenCore.CallbackHandler callback)
			where TweenT : RaTweenCore
		{
			self.OnStartAPIInternal(callback);
			return self;
		}

		public static TweenT OnLoop<TweenT>(this TweenT self, RaTweenCore.LoopCallbackHandler callback)
			where TweenT : RaTweenCore
		{
			self.OnLoopAPIInternal(callback);
			return self;
		}

		public static TweenT OnComplete<TweenT>(this TweenT self, RaTweenCore.CallbackHandler callback)
			where TweenT : RaTweenCore
		{
			self.OnCompleteAPIInternal(callback);
			return self;
		}

		public static TweenT OnKill<TweenT>(this TweenT self, RaTweenCore.CallbackHandler callback)
			where TweenT : RaTweenCore
		{
			self.OnKillAPIInternal(callback);
			return self;
		}

		public static TweenT SetLooping<TweenT>(this TweenT self, int loopAmount)
			where TweenT : RaTweenCore
		{
			self.SetLoopingAPIInternal(loopAmount);
			return self;
		}

		public static TweenT SetInfiniteLooping<TweenT>(this TweenT self)
			where TweenT : RaTweenCore
		{
			self.SetLoopingAPIInternal(RaTweenCore.InfiniteLoopingValue);
			return self;
		}

		public static TweenT DisableLooping<TweenT>(this TweenT self)
			where TweenT : RaTweenCore
		{
			self.SetLoopingAPIInternal(0);
			return self;
		}

		public static TweenT SetDelay<TweenT>(this TweenT self, float delay)
			where TweenT : RaTweenCore
		{
			self.SetDelayAPIInternal(delay);
			return self;
		}
	}
}