using UnityEngine;

namespace RaTweening
{
	public static partial class RaTweenEasing
	{
		#region Consts

		private const float C1 = 1.70158f;
		private const float C2 = C1 * 1.525f;
		private const float C3 = C1 + 1f;
		private const float C4 = (2f * Mathf.PI) / 3f;
		private const float C5 = (2f * Mathf.PI) / 4.5f;

		private const float N1 = 7.5625f;
		private const float D1 = 2.75f;

		#endregion

		#region Public Methods

		public static float Evaluate(EasingType easing, float x)
		{
			switch(easing)
			{
				// Sine
				case EasingType.InSine:
					return 1f - Mathf.Cos((x * Mathf.PI) / 2f);
				case EasingType.OutSine:
					return Mathf.Sin((x * Mathf.PI) / 2f);
				case EasingType.InOutSine:
					return -(Mathf.Cos(Mathf.PI * x) - 1f) / 2f;

				// Quad
				case EasingType.InQuad:
					return x * x;
				case EasingType.OutQuad:
					return 1f - (1f - x) * (1f - x);
				case EasingType.InOutQuad:
					return x < 0.5f ? 2f * x * x : 1f - Mathf.Pow(-2f * x + 2f, 2f) / 2f;

				// Cubic
				case EasingType.InCubic:
					return x * x * x;
				case EasingType.OutCubic:
					return 1f - Mathf.Pow(1f - x, 3f);
				case EasingType.InOutCubic:
					return x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;

				// Quart
				case EasingType.InQuart:
					return x * x * x * x;
				case EasingType.OutQuart:
					return 1f - Mathf.Pow(1f - x, 4f);
				case EasingType.InOutQuart:
					return x < 0.5f ? 8f * x * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 4f) / 2f;

				// Quint
				case EasingType.InQuint:
					return x * x * x * x * x;
				case EasingType.OutQuint:
					return 1f - Mathf.Pow(1f - x, 5f);
				case EasingType.InOutQuint:
					return x < 0.5f ? 16f * x * x * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 6f) / 2f;

				// Expo
				case EasingType.InExpo:
					return Mathf.Approximately(x, 0f) ? 0f : Mathf.Pow(2f, 10f * x - 10f);
				case EasingType.OutExpo:
					return Mathf.Approximately(x, 1f) ? 1f : 1f - Mathf.Pow(2f, -10f * x);
				case EasingType.InOutExpo:
					return Mathf.Approximately(x, 0f)
						  ? 0f
						  : Mathf.Approximately(x, 1f)
						  ? 1f
						  : x < 0.5f ? Mathf.Pow(2f, 20f * x - 10f) / 2f
						  : (2f - Mathf.Pow(2f, -20f * x + 10f)) / 2f;

				// Circ
				case EasingType.InCirc:
					return 1f - Mathf.Sqrt(1f - Mathf.Pow(x, 2f));
				case EasingType.OutCirc:
					return Mathf.Sqrt(1f - Mathf.Pow(x - 1f, 2f));
				case EasingType.InOutCirc:
					return x < 0.5f
						  ? (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * x, 2f))) / 2f
						  : (Mathf.Sqrt(1f - Mathf.Pow(-2f * x + 2f, 2f)) + 1f) / 2f;
				// Back
				case EasingType.InBack:
					return C3 * x * x * x - C1 * x * x;
				case EasingType.OutBack:
					return 1f + C3 * Mathf.Pow(x - 1f, 3f) + C1 * Mathf.Pow(x - 1f, 2f);
				case EasingType.InOutBack:
					return x < 0.5f
						  ? (Mathf.Pow(2f * x, 2f) * ((C2 + 1f) * 2f * x - C2)) / 2f
						  : (Mathf.Pow(2f * x - 2f, 2f) * ((C2 + 1f) * (x * 2f - 2f) + C2) + 2f) / 2f;

				// Elastic
				case EasingType.InElastic:
					return Mathf.Approximately(x, 0f)
						  ? 0f
						  : Mathf.Approximately(x, 1f)
						  ? 1f
						  : -Mathf.Pow(2f, 10f * x - 10f) * Mathf.Sin((x * 10f - 10.75f) * C4);
				case EasingType.OutElastic:
					return Mathf.Approximately(x, 0f)
						  ? 0f
						  : Mathf.Approximately(x, 1f)
						  ? 1f
						  : Mathf.Pow(2f, -10f * x) * Mathf.Sin((x * 10f - 0.75f) * C4) + 1f;
				case EasingType.InOutElastic:
					return Mathf.Approximately(x, 0f)
						  ? 0f
						  : Mathf.Approximately(x, 1f)
						  ? 1f
						  : x < 0.5f
						  ? -(Mathf.Pow(2f, 20f * x - 10f) * Mathf.Sin((20f * x - 11.125f) * C5)) / 2f
						  : (Mathf.Pow(2f, -20f * x + 10f) * Mathf.Sin((20f * x - 11.125f) * C5)) / 2f + 1f;

				// Bounce
				case EasingType.InBounce:
					return 1f - OutBounce(1f - x);
				case EasingType.OutBounce:
					return OutBounce(x);
				case EasingType.InOutBounce:
					return x < 0.5f
						  ? (1f - OutBounce(1f - 2f * x)) / 2f
						  : (1f + OutBounce(2f * x - 1f)) / 2f;

				// Default
				case EasingType.Linear:
				default:
					return x;
			}

			float OutBounce(float v)
			{
				if(v < 1f / D1)
				{
					return N1 * v * v;
				}
				else if(v < 2f / D1)
				{
					return N1 * (v -= 1.5f / D1) * v + 0.75f;
				}
				else if(v < 2.5f / D1)
				{
					return N1 * (v -= 2.25f / D1) * v + 0.9375f;
				}
				else
				{
					return N1 * (v -= 2.625f / D1) * v + 0.984375f;
				}
			}
		}

#endregion
#region Nested

		#endregion
	}
}