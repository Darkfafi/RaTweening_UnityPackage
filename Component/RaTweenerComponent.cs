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

		[Header("Generic Settings")]
		[SerializeField]
		private float _delay = 0f;

		[Header("Callbacks")]
		[SerializeField]
		private UnityEvent _onSetup = null;

		[SerializeField]
		private UnityEvent _onStart = null;

		[SerializeField]
		private UnityEvent _onComplete = null;

		[SerializeField]
		private UnityEvent _onEnd = null;

		#endregion

		#region Variables

		private RaTweenCore _tween = null;

		#endregion

		#region Lifecycle

		protected void OnEnable()
		{
			if(Application.isPlaying)
			{
				if(_tween != null)
				{
					_tween.Kill();
					_tween = null;
				}

				if(_tweenElement != null)
				{
					_tween = _tweenElement
						.CreateTween()
						.ListenToSetup(() => _onSetup?.Invoke())
						.ListenToStart(() => _onStart?.Invoke())
						.ListenToComplete(() => _onComplete?.Invoke())
						.ListenToKill(() => _onEnd?.Invoke())
						.SetDelay(_delay)
						.Play();
				}
			}
		}

		protected void OnDisable()
		{
			if(Application.isPlaying)
			{
				if(_tween != null)
				{
					_tween.Kill();
					_tween = null;
				}
			}
		}

		protected void OnDestroy()
		{
			if(_tweenElement)
			{
#if UNITY_EDITOR
				UnityEditor.EditorApplication.delayCall += ClearElement;
#else
				ClearElement();
#endif
			}
		}

		#endregion

		#region Private Methods

		private void ClearElement()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.delayCall -= ClearElement;
#endif
			if(_tweenElement)
			{
				if(Application.isPlaying)
				{
					Destroy(_tweenElement);
				}
				else
				{
					DestroyImmediate(_tweenElement);
				}
			}
		}

		#endregion
	}
}