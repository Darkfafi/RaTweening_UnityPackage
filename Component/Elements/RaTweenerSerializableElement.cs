using System;
using UnityEngine;

namespace RaTweening
{
	public class RaTweenerSerializableElement : RaTweenerElementBase
	{
		#region Editor Variables

		[Header("Serialized Tween")]
		[SerializeField, SerializeReference]
		private RaTweenCore _tween = null;

		#endregion

		#region Protected Methods

		protected override RaTweenCore CreateTween()
		{
			return _tween.Clone();
		}

		#endregion

		#region Internal Methods

		internal override void Init(Type tweenType)
		{
			_tween = (RaTweenCore)Activator.CreateInstance(tweenType);
			_tween.SetDefaultValuesInternal();
		}

		internal override string GetElementName()
		{
			return _tween == null ? nameof(RaTweenerSerializableElement) : _tween.GetType().Name;
		}

		#endregion
	}
}