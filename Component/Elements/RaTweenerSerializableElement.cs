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

			// Dynamic Tween Auto Targeting
			if(_tween is IRaTweenTarget targetTween)
			{
				Type type = targetTween.GetTargetTypeRaw();

				if(IsOfType<GameObject>(type))
				{
					targetTween.SetTargetRaw(gameObject);
				}
				else if(IsOfType<Component>(type))
				{
					targetTween.SetTargetRaw(gameObject.GetComponent(type));
				}
			}

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

		#region Private Methods

		private bool IsOfType<T>(Type t)
		{
			return typeof(T).IsAssignableFrom(t);
		}

		#endregion
	}
}