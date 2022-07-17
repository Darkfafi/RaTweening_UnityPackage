using System;
using System.Collections.Generic;

namespace RaTweening.Core
{
	public class RaTweeningProcessor : IDisposable
	{
		#region Variables

		private readonly List<RaTweenCore> _tweens = new List<RaTweenCore>();
		private readonly List<RaTweenCore> _killedTweens = new List<RaTweenCore>();

		#endregion

		#region Public Methods

		public void Step(float deltaTime)
		{
			for(int i = 0, c = _tweens.Count; i < c; i++)
			{
				RaTweenCore tween = _tweens[i];

				if(tween.IsValid)
				{
					// Stepping
					switch(tween.TweenState)
					{
						case RaTweenCore.State.ToResume:
							if(!tween.HasNoDelayRemaining)
							{
								tween.SetStateInternal(RaTweenCore.State.InDelay);
								goto case RaTweenCore.State.InDelay;
							}
							else if(!tween.IsCompleted)
							{
								tween.SetStateInternal(RaTweenCore.State.InProgress);
								goto case RaTweenCore.State.InProgress;
							}
							else
							{
								tween.SetStateInternal(RaTweenCore.State.Dead);
							}
							break;
						case RaTweenCore.State.ToStart:
							tween.SetupInternal();
							tween.SetStateInternal(RaTweenCore.State.InDelay);
							goto case RaTweenCore.State.InDelay;

						case RaTweenCore.State.InDelay:
							tween.StepDelayInternal(deltaTime);
							if(tween.HasNoDelayRemaining)
							{
								tween.SetStateInternal(RaTweenCore.State.Started);
							}
							break;

						case RaTweenCore.State.InProgress:
							tween.StepTweenInternal(deltaTime);
							if(tween.IsTotalCompleted)
							{
								if(tween.HasReachedLoopEnd)
								{
									tween.SetStateInternal(RaTweenCore.State.Completed);
								}
								else
								{
									tween.SetStateInternal(RaTweenCore.State.InDelay);
									tween.LoopInternal();
								}
							}
							break;
					}
				}
				else
				{
					tween.SetStateInternal(RaTweenCore.State.Dead);
				}

				// State Switching
				switch(tween.TweenState)
				{
					case RaTweenCore.State.Started:
						tween.StartInternal();
						if(tween.IsTotalCompleted)
						{
							tween.SetStateInternal(RaTweenCore.State.Completed);
						}
						else
						{
							tween.SetStateInternal(RaTweenCore.State.InProgress);
						}
						break;
					case RaTweenCore.State.Completed:
						tween.CompleteInternal();
						tween.SetStateInternal(RaTweenCore.State.Dead);
						break;
					case RaTweenCore.State.Dead:
						tween.KillInternal();
						_killedTweens.Add(tween);
						break;
				}
			}

			for(int i = _killedTweens.Count - 1; i >= 0; i--)
			{
				UnregisterTween(_killedTweens[i]);
			}

			_killedTweens.Clear();
		}

		public void Dispose()
		{
			for(int i = _killedTweens.Count - 1; i >= 0; i--)
			{
				_killedTweens[i].DisposeInternal();
			}

			for(int i = _tweens.Count - 1; i >= 0; i--)
			{
				_tweens[i].DisposeInternal();
			}

			_killedTweens.Clear();
			_tweens.Clear();
		}

		#endregion

		#region Internal Methods

		internal RaTweenCore RegisterTween(RaTweenCore tween)
		{
			if(tween.TweenState == RaTweenCore.State.None)
			{
				_tweens.Add(tween);
				tween.SetStateInternal(RaTweenCore.State.ToStart);
			}

			return tween;
		}

		internal RaTweenCore UnregisterTween(RaTweenCore tween)
		{
			if(tween.TweenState != RaTweenCore.State.None)
			{
				_tweens.Remove(tween);
			}

			return tween;
		}

		#endregion
	}
}