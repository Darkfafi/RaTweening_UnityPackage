using UnityEngine;

namespace RaTweening.Core
{
	public static class RaTweenModifier
	{
		public static float ApplyModifier(RaModifierType modifierType, float x)
		{
			switch(modifierType)
			{
				// Modifiers
				case RaModifierType.AbsSin:
					return Mathf.Abs(Mathf.Sin(x * Mathf.PI));

				case RaModifierType.Yoyo:
					if(x <= 0.5f)
					{
						x *= 2f;
					}
					else
					{
						x *= -2f;
						x += 2f;
					}
					return x;

				case RaModifierType.Reverse:
					return 1f - x;

				// Default
				case RaModifierType.None:
					return x;

				// Not Implemented Exception
				default:
					throw new System.NotImplementedException($"{nameof(RaModifierType)} {modifierType} not implemented");
			}
		}
	}
}