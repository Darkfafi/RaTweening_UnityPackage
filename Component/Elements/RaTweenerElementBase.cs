using System;
using UnityEngine;
using UnityEngine.Events;

namespace RaTweening
{
	public abstract class RaTweenerElementBase : MonoBehaviour
	{
		#region Editor Variables

		[SerializeField, HideInInspector]
		private float _delay = 0f;

		[SerializeField, HideInInspector]
		private UnityEvent _onSetup = null;

		[SerializeField, HideInInspector]
		private UnityEvent _onStart = null;

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
				.ListenToSetup(() => _onSetup?.Invoke())
				.ListenToStart(() => _onStart?.Invoke())
				.ListenToComplete(() => _onComplete?.Invoke())
				.ListenToKill(() => _onEnd?.Invoke())
				.SetDelay(_delay);
		}

		#endregion

		#region Internal Methods

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
	}
}