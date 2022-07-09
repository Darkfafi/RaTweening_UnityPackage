using System.Collections.Generic;
using UnityEngine;

namespace RaTweening.Core
{
	public class RaTweeningCore : MonoBehaviour
	{
		#region Consts

		private const string Name = "<" + nameof(RaTweeningCore) + ">";

		#endregion

		#region Variables

		private readonly List<RaTweenCore> _tweens = new List<RaTweenCore>();
		private readonly List<RaTweenCore> _killedTweens = new List<RaTweenCore>();

		#endregion

		#region Properties

		public static RaTweeningCore Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new GameObject("<"+nameof(RaTweeningCore) +">").AddComponent<RaTweeningCore>();
					DontDestroyOnLoad(_instance);
				}
				return _instance;
			}
		}

		private static RaTweeningCore _instance = null;

		#endregion

		#region Lifecycle

		protected void LateUpdate()
		{
			for(int i = 0, c = _tweens.Count; i < c; i++)
			{
				RaTweenCore tween = _tweens[i];

				if(tween.IsValid)
				{
					// Stepping
					switch(tween.TweenState)
					{
						case RaTweenCore.State.ToStart:
							tween.SetupInternal();
							tween.SetStateInternal(RaTweenCore.State.InDelay);
							goto case RaTweenCore.State.InDelay;

						case RaTweenCore.State.InDelay:
							tween.StepDelayInternal(Time.deltaTime);
							if(tween.HasNoDelayRemaining)
							{
								tween.SetStateInternal(RaTweenCore.State.Started);
							}
							break;

						case RaTweenCore.State.InProgress:
							tween.StepTweenInternal(Time.deltaTime);
							if(tween.IsCompleted)
							{
								tween.SetStateInternal(RaTweenCore.State.Completed);
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
						if(tween.IsCompleted)
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

		#endregion

		#region Internal Methods

		internal RaTweenCore RegisterTween(RaTweenCore tween)
		{
			if(tween.TweenState == RaTweenCore.State.None)
			{
				_tweens.Add(tween);
				tween.SetStateInternal(RaTweenCore.State.ToStart);
#if UNITY_EDITOR
				name = string.Concat(Name, "(", _tweens.Count, ")");
#endif
			}

			return tween;
		}

		internal RaTweenCore UnregisterTween(RaTweenCore tween)
		{
			if(tween.TweenState != RaTweenCore.State.None)
			{
				_tweens.Remove(tween);
#if UNITY_EDITOR
				name = string.Concat(Name, "(", _tweens.Count, ")");
#endif
			}

			return tween;
		}

		#endregion
	}
}