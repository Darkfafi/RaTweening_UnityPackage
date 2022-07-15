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
	}
}