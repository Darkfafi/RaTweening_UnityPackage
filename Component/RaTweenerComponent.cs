using System.Collections;
using UnityEngine;

namespace RaTweening
{
	public class RaTweenerComponent : MonoBehaviour
	{
		#region Editor Variables

		[SerializeReference]
		private RaTweenBase _raTween = null;

		#endregion

		#region Variables

		private RaTweenBase _tween = null;

		#endregion

		#region Lifecycle

		protected void OnEnable()
		{
			if(_tween != null)
			{
				_tween.Kill();
				_tween = null;
			}

			_tween = _raTween.Clone().Play();
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