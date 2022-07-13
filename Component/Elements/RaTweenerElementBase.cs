using System;
using UnityEngine;

namespace RaTweening
{
	public abstract class RaTweenerElementBase : MonoBehaviour
	{
		#region Public Methods

		public abstract RaTweenCore CreateTween();

		#endregion

		#region Internal Methods

		internal abstract string GetElementName();
		internal abstract void Init(Type tweenType);

		#endregion
	}
}