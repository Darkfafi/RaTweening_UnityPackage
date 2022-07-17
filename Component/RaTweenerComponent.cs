using UnityEngine;
using UnityEngine.Events;

namespace RaTweening
{
	[ExecuteAlways]
	public class RaTweenerComponent : MonoBehaviour
	{
		#region Editor Variables

		[SerializeReference, HideInInspector]
		private RaTweenerElementBase _tweenElement = null;

		[Header("Options")]
		[SerializeField]
		private PlayOption _playOption = PlayOption.OnEnable;

		[SerializeField]
		private StopOption _stopOption = StopOption.OnDisable;

		[SerializeField]
		private Progressor _optionProgressor = Progressor.Default;

		#endregion

		#region Variables

		private RaTweenCore _tween = null;

		#endregion

		#region Properties

		public bool IsPlaying => _tween != null && _tween.IsPlaying;
		public bool IsPaused => _tween != null && _tween.IsPaused;
		public bool IsCompleted => _tween != null && _tween.IsCompleted;

		#endregion

		#region Lifecycle

		protected void Awake()
		{
			TryPerformPlay(PlayOption.OnAwake);
		}

		protected void Start()
		{
			TryPerformPlay(PlayOption.OnStart);
		}

		protected void OnEnable()
		{
			TryPerformPlay(PlayOption.OnEnable);
		}

		protected void OnDisable()
		{
			TryPerformStop(StopOption.OnDisable);
		}

		protected void OnDestroy()
		{
			if(_tweenElement)
			{
#if UNITY_EDITOR
				if(RaTweenerComponentEditor.TryRemoveTween(_tweenElement))
				{
					_tweenElement = null;
				}
#else
				if(Application.isPlaying)
				{
					Destroy(_tweenElement);
					_tweenElement = null;
				}
#endif
			}
		}

		#endregion

		#region Public Methods

		public RaTweenCore CreateTweenInstance()
		{
			if(_tweenElement)
			{
				return _tweenElement.CreateTween();
			}
			return null;
		}

		public RaTweenCore Play()
		{
			Stop();

			if(_tweenElement)
			{
				return _tween = _tweenElement
					.CreateTween()
					.Play();
			}

			return null;
		}

		public bool Pause()
		{
			if(_tween != null)
			{
				return _tween.Pause();
			}
			return false;
		}

		public RaTweenCore ResumeOrPlay()
		{
			if(!Resume())
			{
				return Play();
			}
			return _tween;
		}

		public bool Resume()
		{
			if(_tween != null)
			{
				return _tween.Resume();
			}
			return false;
		}

		public bool Stop()
		{
			if(_tween != null)
			{
				_tween.Kill();
				_tween = null;
				return true;
			}
			return false;
		}

		#endregion

		#region Private Methods

		private void TryPerformPlay(PlayOption option)
		{
			if(option != _playOption || !Application.isPlaying)
			{
				return;
			}

			switch(_optionProgressor)
			{
				case Progressor.Default:
					Play();
					break;
				case Progressor.ResumeOrDefault:
					ResumeOrPlay();
					break;
				case Progressor.Resume:
					if(_tween == null)
					{
						Play();
					}
					else
					{
						Resume();
					}
					break;
			}
		}

		private void TryPerformStop(StopOption option)
		{
			if(option != _stopOption || !Application.isPlaying)
			{
				return;
			}

			switch(_optionProgressor)
			{
				case Progressor.Default:
					Stop();
					break;
				case Progressor.ResumeOrDefault:
					if(!Pause())
					{
						Stop();
					}
					break;
				case Progressor.Resume:
					Pause();
					break;
			}
		}

		#endregion

		#region Nested

		public enum PlayOption
		{
			None,
			OnAwake,
			OnStart,
			OnEnable
		}

		public enum StopOption
		{
			None,
			OnDisable
		}

		public enum Progressor
		{
			Default,
			ResumeOrDefault,
			Resume,
		}

		#endregion
	}
}