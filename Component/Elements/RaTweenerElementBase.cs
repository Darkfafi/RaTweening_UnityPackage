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

		#endregion

		#region Public Methods

		public RaTweenCore CreateTweenCore()
		{
			return CreateTween()
				.ListenToSetup(() => _onSetup?.Invoke())
				.ListenToStart(() => _onStart?.Invoke())
				.ListenToComplete(() => _onComplete?.Invoke())
				.ListenToKill(() => _onEnd?.Invoke())
				.SetDelay(_delay);
		}

		#endregion

		#region Internal Methods

		internal abstract string GetElementName();
		internal abstract void Init(Type tweenType);

		#endregion

		#region Protected Methods

		protected abstract RaTweenCore CreateTween();

		#endregion
	}
}