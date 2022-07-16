using UnityEngine;

namespace RaTweening
{
	internal class RaTweenProgressor
	{
		#region Consts

		public const float InfinityValue = -1f;

		#endregion

		#region Properties

		public float Time
		{
			get; private set;
		}

		public float Duration
		{
			get; private set;
		}

		public float Progress
		{
			get
			{
				if(IsEmpty)
				{
					return 0f;
				}

				if(IsInfinite)
				{
					return InfinityValue;
				}

				return Time / Duration;
			}
		}

		public bool IsCompleted => Mathf.Approximately(Time, Duration);

		public bool IsEmpty => Mathf.Approximately(Duration, 0f);

		public bool IsInfinite => Mathf.Approximately(Duration, InfinityValue);

		#endregion

		public RaTweenProgressor(float duration)
		{
			Time = 0f;
			SetDuration(duration);
		}

		#region Public Methods

		public void SetInfiniteDuration()
		{
			Duration = InfinityValue;
		}

		public void SetDuration(float duration)
		{
			Duration = Mathf.Max(0f, duration);
		}

		public void Reset()
		{
			Time = 0f;
		}

		public void Step(float delta)
		{
			if(IsInfinite)
			{
				Time += delta;
			}
			else
			{
				Time = Mathf.Clamp(Time + delta, 0f, Duration);
			}
		}

		public void Complete()
		{
			if(IsInfinite)
			{
				return;
			}

			Time = Duration;
		}

		#endregion
	}
}