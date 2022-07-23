using UnityEngine;
using static RaTweening.RaVector3Options;

namespace RaTweening
{
	public class RaTweenScale : RaTweenDynamic<Transform, Vector3>
	{
		#region Editor Variables

		[Header("RaTweenScale")]
		[SerializeField]
		private Axis _excludeAxis = Axis.None;

		#endregion

		public RaTweenScale()
			: base()
		{

		}

		public RaTweenScale(Transform target, Vector3 startScale, Vector3 endScale, float duration)
			: base(target, startScale, endScale, duration)
		{

		}

		public RaTweenScale(Transform target, Vector3 endScale, float duration)
			: base(target, endScale, duration)
		{

		}

		public RaTweenScale(Transform target, Vector3 startScale, Transform endScale, float duration)
		   : base(target, startScale, default, duration)
		{
			SetEndRef(endScale);
		}

		#region Internal Methods

		internal void SetExcludeAxisAPIInternal(Axis excludeAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = excludeAxis;
			}
		}

		internal void OnlyIncludeAxisAPIInternal(Axis inclAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = GetOnlyIncludeAxes(inclAxis);
			}
		}

		#endregion

		#region Protected Methods

		protected override RaTweenDynamic<Transform, Vector3> DynamicClone()
		{
			RaTweenScale tween = new RaTweenScale();
			tween._excludeAxis = _excludeAxis;
			return tween;
		}

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(Target != null ? ReadValue(Target) : Vector3.one);
			SetEndValue(Vector3.one);
		}

		protected override void DynamicEvaluation(float normalizedValue, Transform target, Vector3 start, Vector3 end)
		{
			Vector3 delta = end - start;
			target.localScale = ApplyExcludeAxes(target.localScale, start + (delta * normalizedValue), _excludeAxis);
		}

		protected override Vector3 ReadValue(Transform reference)
		{
			return reference.localScale;
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
		{
			return start + delta;
		}

		#endregion
	}


	#region Extensions

	public static class RaTweenScaleExtensions
	{
		public static RaTweenScale TweenScaleX(this Transform self, float scaleX, float duration)
		{
			return new RaTweenScale(self, Vector3.one * scaleX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		public static RaTweenScale TweenScaleY(this Transform self, float scaleY, float duration)
		{
			return new RaTweenScale(self, Vector3.one * scaleY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		public static RaTweenScale TweenScaleZ(this Transform self, float scaleX, float duration)
		{
			return new RaTweenScale(self, Vector3.one * scaleX, duration)
				.OnlyIncludeAxis(Axis.Z)
				.Play();
		}

		public static RaTweenScale TweenScale(this Transform self, float scale, float duration)
		{
			return new RaTweenScale(self, Vector3.one * scale, duration).Play();
		}

		public static RaTweenScale TweenScale(this Transform self, float startScale, float endScale, float duration)
		{
			return new RaTweenScale(self, Vector3.one * startScale, Vector3.one * endScale, duration).Play();
		}

		public static RaTweenScale TweenScale(this Transform self, Vector3 scale, float duration)
		{
			return new RaTweenScale(self, scale, duration).Play();
		}

		public static RaTweenScale TweenScale(this Transform self, Vector3 startScale, Vector3 endScale, float duration)
		{
			return new RaTweenScale(self, startScale, endScale, duration).Play();
		}

		public static RaTweenScale TweenScale(this Transform self, Vector3 startScale, Transform endScale, float duration)
		{
			return new RaTweenScale(self, startScale, endScale, duration).Play();
		}

		public static RaTweenScale SetExcludeAxis(this RaTweenScale self, Axis excludeAxis)
		{
			self.SetExcludeAxisAPIInternal(excludeAxis);
			return self;
		}

		public static RaTweenScale OnlyIncludeAxis(this RaTweenScale self, Axis includeAxis)
		{
			self.OnlyIncludeAxisAPIInternal(includeAxis);
			return self;
		}
	}

	#endregion
}