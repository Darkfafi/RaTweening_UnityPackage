using System.Collections.Generic;
using UnityEngine;

namespace RaTweening
{
	public class RaTweeningCore : MonoBehaviour
	{
		#region Consts

		private const string Name = "<" + nameof(RaTweeningCore) + ">";

		#endregion

		#region Variables

		private readonly List<RaTweenBase> _tweens = new List<RaTweenBase>();
		private readonly List<RaTweenBase> _killedTweens = new List<RaTweenBase>();

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
				RaTweenBase tween = _tweens[i];

				if(tween.IsValid)
				{
					// Stepping
					switch(tween.TweenState)
					{
						case RaTweenBase.State.ToStart:
							tween.SetupInternal();
							tween.SetStateInternal(RaTweenBase.State.InDelay);
							goto case RaTweenBase.State.InDelay;

						case RaTweenBase.State.InDelay:
							tween.StepDelayInternal(Time.deltaTime);
							if(tween.HasNoDelayRemaining)
							{
								tween.SetStateInternal(RaTweenBase.State.Started);
							}
							break;

						case RaTweenBase.State.InProgress:
							tween.StepTweenInternal(Time.deltaTime);
							if(tween.IsCompleted)
							{
								tween.SetStateInternal(RaTweenBase.State.Completed);
							}
							break;
					}
				}
				else
				{
					tween.SetStateInternal(RaTweenBase.State.Dead);
				}

				// State Switching
				switch(tween.TweenState)
				{
					case RaTweenBase.State.Started:
						tween.StartInternal();
						if(tween.IsCompleted)
						{
							tween.SetStateInternal(RaTweenBase.State.Completed);
						}
						else
						{
							tween.SetStateInternal(RaTweenBase.State.InProgress);
						}
						break;
					case RaTweenBase.State.Completed:
						tween.CompleteInternal();
						tween.SetStateInternal(RaTweenBase.State.Dead);
						break;
					case RaTweenBase.State.Dead:
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

		internal RaTweenBase RegisterTween(RaTweenBase tween)
		{
			if(tween.TweenState == RaTweenBase.State.None)
			{
				_tweens.Add(tween);
				tween.SetStateInternal(RaTweenBase.State.ToStart);
#if UNITY_EDITOR
				name = string.Concat(Name, "(", _tweens.Count, ")");
#endif
			}

			return tween;
		}

		internal RaTweenBase UnregisterTween(RaTweenBase tween)
		{
			if(tween.TweenState != RaTweenBase.State.None)
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