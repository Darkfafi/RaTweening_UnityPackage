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
		protected override void Init(Type tweenType)
		{
			_tween = (RaTweenCore)Activator.CreateInstance(tweenType);
			_tween.SetDefaultValuesInternal();
		}

		protected override RaTweenCore CreateTweenCore()
		{
			return _tween.Clone();
		}

		protected override string GetElementName()
		{
			return _tween == null ? nameof(RaTweenerSerializableElement) : _tween.GetType().Name;
		}

		#endregion
	}
}