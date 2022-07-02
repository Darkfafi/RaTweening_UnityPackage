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

				// Stepping
				switch(tween.TweenState)
				{
					case RaTweenBase.State.ToStart:
						tween.StepDelay(Time.deltaTime);
						if(tween.HasNoDelayRemaining)
						{
							tween.SetState(RaTweenBase.State.Started);
						}
						break;
					case RaTweenBase.State.InProgress:
						tween.StepTween(Time.deltaTime);
						if(tween.IsCompleted)
						{
							tween.SetState(RaTweenBase.State.Completed);
						}
						break;
				}

				// State Switching
				switch(tween.TweenState)
				{
					case RaTweenBase.State.Started:
						tween.Start();
						if(tween.IsCompleted)
						{
							tween.SetState(RaTweenBase.State.Completed);
						}
						else
						{
							tween.SetState(RaTweenBase.State.InProgress);
						}
						break;
					case RaTweenBase.State.Completed:
						tween.Complete();
						tween.SetState(RaTweenBase.State.Dead);
						break;
					case RaTweenBase.State.Dead:
						tween.Kill();
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

		internal void RegisterTween(RaTweenBase tween)
		{
			if(tween.TweenState == RaTweenBase.State.None)
			{
				_tweens.Add(tween);
				tween.SetState(RaTweenBase.State.ToStart);
				tween.Setup();
#if UNITY_EDITOR
				name = string.Concat(Name, "(", _tweens.Count, ")");
#endif
			}
		}

		internal void UnregisterTween(RaTweenBase tween)
		{
			if(tween.TweenState != RaTweenBase.State.None)
			{
				_tweens.Remove(tween);
#if UNITY_EDITOR
				name = string.Concat(Name, "(", _tweens.Count, ")");
#endif
			}
		}

		#endregion
	}
}