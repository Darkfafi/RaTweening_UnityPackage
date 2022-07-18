using System;
using UnityEngine;
using UnityEngine.Events;

namespace RaTweening
{
	public abstract class RaTweenerElementBase : MonoBehaviour
	{
		#region Editor Variables

		[SerializeField, HideInInspector]
		private LoopAllowStage _loopingAllowStage = LoopAllowStage.ToInfinity;

		[SerializeField, HideInInspector]
		private int _loops = 0;

		[SerializeField, HideInInspector]
		private float _delay = 0f;

		[SerializeField, HideInInspector]
		private bool _isReverse = false;

		[SerializeField, HideInInspector]
		private UnityEvent _onSetup = null;

		[SerializeField, HideInInspector]
		private UnityEvent _onStart = null;

		[SerializeField, HideInInspector]
		private UnityEvent _onLoop = null;

		[SerializeField, HideInInspector]
		private UnityEvent _onComplete = null;

		[SerializeField, HideInInspector]
		private UnityEvent _onEnd = null;

		[SerializeField, HideInInspector]
		private string _overrideName = null;

		#endregion

		#region Public Methods

		public RaTweenCore CreateTween()
		{
			return CreateTweenCore()
				.OnSetup(() => _onSetup?.Invoke())
				.OnStart(() => _onStart?.Invoke())
				.OnLoop((loopCount) => _onLoop?.Invoke())
				.OnComplete(() => _onComplete?.Invoke())
				.OnKill(() => _onEnd?.Invoke())
				.SetReverse(_isReverse)
				.SetDelay(_delay)
				.SetLooping(_loops);
		}

		#endregion

		#region Internal Methods

		internal LoopAllowStage GetLoopAllowStage()
		{
			return _loopingAllowStage;
		}

		internal void SetLoopingAllowStage(LoopAllowStage stage)
		{
			_loopingAllowStage = stage;
		}

		internal void Initialize(Type tweenType)
		{
			Init(tweenType);
		}

		internal string GetName()
		{
			return string.IsNullOrEmpty(_overrideName) ? GetElementName() : _overrideName;
		}

		internal void SetName(string name)
		{
			_overrideName = name;
		}

		#endregion

		#region Protected Methods

		protected abstract void Init(Type tweenType);
		protected abstract string GetElementName();
		protected abstract RaTweenCore CreateTweenCore();

		#endregion

		#region Enum

		public enum LoopAllowStage
		{
			ToInfinity,
			ToFinite,
			None,
		}

		#endregion
	}
}