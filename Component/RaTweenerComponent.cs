using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RaTweening
{
	public class RaTweenerComponent : MonoBehaviour
	{
		#region Editor Variables

		[SerializeReference]
		private RaTweenCore _raTween = null;

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
			if(_tween != null)
			{
				_tween.Kill();
				_tween = null;
			}

			_tween = _raTween
				.Clone()
				.ListenToSetup(()=> _onSetup?.Invoke())
				.ListenToStart(() => _onStart?.Invoke())
				.ListenToComplete(()=> _onComplete?.Invoke())
				.ListenToKill(()=> _onEnd?.Invoke())
				.Play();
		}

		protected void OnDisable()
		{
			if(_tween != null)
			{
				_tween.Kill();
				_tween = null;
			}
		}

		#endregion
	}
}