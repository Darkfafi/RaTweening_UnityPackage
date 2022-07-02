using UnityEngine;

namespace RaTweening
{
	internal class RaTweenTimeline
	{
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

				return Time / Duration;
			}
		}

		public bool IsCompleted => Mathf.Approximately(Time, Duration);

		public bool IsEmpty => Mathf.Approximately(Duration, 0f);

		#endregion

		public RaTweenTimeline(float duration)
		{
			Time = 0f;
			SetDuration(duration);
		}

		#region Public Methods

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
			Time = Mathf.Clamp(Time + delta, 0f, Duration);
		}

		public void Complete()
		{
			Time = Duration;
		}

		#endregion
	}
}