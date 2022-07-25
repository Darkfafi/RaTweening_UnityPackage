using RaTweening.UI.RaShadow;
using UnityEngine;
using UnityEngine.UI;
using static RaTweening.RaVector2Options;

namespace RaTweening.UI.RaShadow
{
	public class RaTweenDistance : RaTweenDynamic<Shadow, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenDistance")]
		[SerializeField]
		private Axis _excludeAxis = Axis.None;

		#endregion

		public RaTweenDistance()
			: base()
		{

		}

		public RaTweenDistance(Shadow target, Vector2 startDistance, Vector2 endDistance, float duration)
			: base(target, startDistance, endDistance, duration)
		{

		}

		public RaTweenDistance(Shadow target, Vector2 endDistance, float duration)
			: base(target, endDistance, duration)
		{

		}

		public RaTweenDistance(Shadow target, Vector2 startDistance, Shadow endDistance, float duration)
			: base(target, startDistance, default, duration)
		{
			SetEndRef(endDistance);
		}

		#region Public Methods

		public RaTweenDistance SetExcludeAxis(Axis excludeAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = excludeAxis;
			}
			return this;
		}

		public RaTweenDistance OnlyIncludeAxis(Axis inclAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = GetOnlyIncludeAxes(inclAxis);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenDynamic<Shadow, Vector2> DynamicClone()
		{
			RaTweenDistance tween = new RaTweenDistance();
			tween._excludeAxis = _excludeAxis;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, Shadow target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.effectDistance = ApplyExcludeAxes(target.effectDistance, start + (delta * normalizedValue), _excludeAxis);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(Shadow reference)
		{
			return reference.effectDistance;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		public static RaTweenDistance TweenDistanceX(this Shadow self, float distanceX, float duration)
		{
			return new RaTweenDistance(self, Vector2.one * distanceX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		public static RaTweenDistance TweenDistanceY(this Shadow self, float distanceY, float duration)
		{
			return new RaTweenDistance(self, Vector2.one * distanceY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		public static RaTweenDistance TweenDistance(this Shadow self, Vector2 distance, float duration)
		{
			return new RaTweenDistance(self, distance, duration).Play();
		}

		public static RaTweenDistance TweenDistance(this Shadow self, Vector2 startDistance, Vector2 endDistance, float duration)
		{
			return new RaTweenDistance(self, startDistance, endDistance, duration).Play();
		}

		public static RaTweenDistance TweenDistance(this Shadow self, Vector2 startDistance, Shadow endTarget, float duration)
		{
			return new RaTweenDistance(self, startDistance, endTarget, duration).Play();
		}
	}

	#endregion
}