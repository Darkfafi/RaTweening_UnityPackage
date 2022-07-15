using System;
using System.Collections.Generic;
using UnityEngine;


namespace RaTweening
{
	public class RaTweenerSequenceElement : RaTweenerElementBase
	{
		#region Editor Variables

		[SerializeField, HideInInspector, SerializeReference]
		private List<RaTweenerElementBase> _sequenceElements = new List<RaTweenerElementBase>();

		#endregion

		#region Lifecycle

		protected void OnDestroy()
		{
			if(_sequenceElements != null && _sequenceElements.Count > 0)
			{
				for(int i = _sequenceElements.Count - 1; i >= 0; i--)
				{
					RaTweenerElementBase tweenElement = _sequenceElements[i];
					if(tweenElement)
					{
#if UNITY_EDITOR
						if(RaTweenerComponentEditor.TryRemoveTween(tweenElement))
						{
							_sequenceElements.RemoveAt(i);
						}
#else
						if(Application.isPlaying)
						{
							Destroy(tweenElement);
							_sequenceElements.RemoveAt(i);
						}
#endif
					}
				}
				_sequenceElements.Clear();
			}
		}

		#endregion

		#region Protected Methods

		protected override void Init(Type tweenType)
		{
			_sequenceElements = new List<RaTweenerElementBase>();
		}

		protected override RaTweenCore CreateTweenCore()
		{
			RaTweenCore[] sequence = new RaTweenCore[_sequenceElements.Count];
			for(int i = 0; i < sequence.Length; i++)
			{
				sequence[i] = _sequenceElements[i].CreateTween();
			}
			return new RaTweenSequence(sequence);
		}

		protected override string GetElementName()
		{
			return nameof(RaTweenSequence);
		}

		#endregion

		#region Internal Methods

		internal bool RegisterTweenElement(RaTweenerElementBase element)
		{
			if(!_sequenceElements.Contains(element))
			{
				_sequenceElements.Add(element);
				return true;
			}
			return false;
		}

		internal bool UnregisterTweenElement(RaTweenerElementBase element)
		{
			return _sequenceElements.Remove(element);
		}

		#endregion
	}
}