using System;
using UnityEngine;

namespace RaTweening
{
	public class RaTweenerSerializableElement : RaTweenerElementBase
	{
		#region Editor Variables

		[Header("RaTweenerElement")]
		[SerializeField, SerializeReference]
		private RaTweenCore _tween = null;

		#endregion

		#region Protected Methods
		protected override void Init(Type tweenType)
		{
			_tween = (RaTweenCore)Activator.CreateInstance(tweenType);
			_tween.SetDefaultValuesInternal();

			// Dynamic Tween Auto Targeting
			if(_tween is IRaTweenDynamic dynamic)
			{
				Type type = dynamic.GetTargetType();

				if(IsOfType<GameObject>(type))
				{
					dynamic.SetTarget(gameObject);
				}
				else if(IsOfType<Component>(type))
				{
					dynamic.SetTarget(gameObject.GetComponent(type));
				}
			}
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

		#region Private Methods

		private bool IsOfType<T>(Type t)
		{
			return typeof(T).IsAssignableFrom(t);
		}

		#endregion
	}
}