using System;
using UnityEngine;
using static RaTweening.RaVector3Options;
using RaTweening.RaTransform;

namespace RaTweening.RaTransform
{
	[Serializable]
	public class RaTweenRotation : RaTweenDynamic<Transform, Vector3>
	{
		#region Editor Variables

		[Header("RaTweenRotation")]
		[SerializeField]
		private Axis _excludeAxis = Axis.None;

		[SerializeField]
		private bool _localRotation = false;

		#endregion

		public RaTweenRotation()
			: base()
		{

		}

		public RaTweenRotation(Transform target, Vector3 startRot, Vector3 endRot, float duration)
			: base(target, startRot, endRot, duration)
		{

		}

		public RaTweenRotation(Transform target, Vector3 endRot, float duration)
			: base(target, endRot, endRot, duration)
		{

		}

		#region Public Methods

		public RaTweenRotation SetLocalRotation(bool isLocal = true)
		{
			if(CanBeModified())
			{
				_localRotation = isLocal;
			}
			return this;
		}

		public RaTweenRotation SetExcludeAxis(Axis excludeAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = excludeAxis;
			}
			return this;
		}

		public RaTweenRotation OnlyIncludeAxis(Axis inclAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = GetOnlyIncludeAxes(inclAxis);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void DynamicEvaluation(float normalizedValue, Transform target, Vector3 start, Vector3 end)
		{
			target.rotation = Quaternion.SlerpUnclamped
			(
				Quaternion.Euler(start), 
				Quaternion.Euler(ApplyExcludeAxes(target.rotation.eulerAngles, end, _excludeAxis)), 
				normalizedValue
			);
		}

		protected override RaTweenDynamic<Transform, Vector3> DynamicClone()
		{
			RaTweenRotation tween = new RaTweenRotation();
			tween._excludeAxis = _excludeAxis;
			return tween;
		}

		protected override Vector3 ReadValue(Transform reference)
		{
			return reference.rotation.eulerAngles;
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
		{
			return start + delta;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		public static RaTweenRotation TweenRotateX(this Transform self, float rotX, float duration)
		{
			return new RaTweenRotation(self, Vector3.one * rotX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		public static RaTweenRotation TweenRotateY(this Transform self, float rotY, float duration)
		{
			return new RaTweenRotation(self, Vector3.one * rotY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		public static RaTweenRotation TweenRotateZ(this Transform self, float rotZ, float duration)
		{
			return new RaTweenRotation(self, Vector3.one * rotZ, duration)
				.OnlyIncludeAxis(Axis.Z)
				.Play();
		}

		public static RaTweenRotation TweenRotate(this Transform self, Vector3 rot, float duration)
		{
			return new RaTweenRotation(self, rot, duration).Play();
		}

		public static RaTweenRotation TweenRotate(this Transform self, Vector3 startRot, Vector3 endRot, float duration)
		{
			return new RaTweenRotation(self, startRot, endRot, duration).Play();
		}
	}

	#endregion
}