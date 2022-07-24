using RaTweening.UI.RaRectTransform;
using UnityEngine;
using static RaTweening.RaVector3Options;

namespace RaTweening.UI.RaRectTransform
{
	public class RaTweenAnchorPos3D : RaTweenDynamic<RectTransform, Vector3>
	{
		#region Editor Variables

		[Header("RaTweenAnchorPos3D")]
		[SerializeField]
		private Axis _excludeAxis = Axis.None;

		#endregion

		public RaTweenAnchorPos3D()
			: base()
		{

		}

		public RaTweenAnchorPos3D(RectTransform target, Vector3 startPos, Vector3 endPos, float duration)
			: base(target, startPos, endPos, duration)
		{

		}

		public RaTweenAnchorPos3D(RectTransform target, Vector3 endPos, float duration)
			: base(target, endPos, duration)
		{

		}

		public RaTweenAnchorPos3D(RectTransform target, Vector3 startPos, RectTransform endPos, float duration)
			: base(target, startPos, default, duration)
		{
			SetEndRef(endPos);
		}

		#region Public Methods

		public RaTweenAnchorPos3D SetExcludeAxis(Axis excludeAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = excludeAxis;
			}
			return this;
		}

		public RaTweenAnchorPos3D OnlyIncludeAxis(Axis inclAxis)
		{
			if(CanBeModified())
			{
				_excludeAxis = GetOnlyIncludeAxes(inclAxis);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenDynamic<RectTransform, Vector3> DynamicClone()
		{
			RaTweenAnchorPos3D tween = new RaTweenAnchorPos3D();
			tween._excludeAxis = _excludeAxis;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, RectTransform target, Vector3 start, Vector3 end)
		{
			Vector3 delta = end - start;
			target.anchoredPosition3D = ApplyExcludeAxes(target.anchoredPosition3D, start + (delta * normalizedValue), _excludeAxis);
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
		{
			return start + delta;
		}

		protected override Vector3 ReadValue(RectTransform reference)
		{
			return reference.anchoredPosition3D;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		public static RaTweenAnchorPos3D TweenAnchorPos3DX(this RectTransform self, float posX, float duration)
		{
			return new RaTweenAnchorPos3D(self, Vector3.one * posX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		public static RaTweenAnchorPos3D TweenAnchorPos3DY(this RectTransform self, float posY, float duration)
		{
			return new RaTweenAnchorPos3D(self, Vector3.one * posY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		public static RaTweenAnchorPos3D TweenAnchorPos3DZ(this RectTransform self, float posZ, float duration)
		{
			return new RaTweenAnchorPos3D(self, Vector3.one * posZ, duration)
				.OnlyIncludeAxis(Axis.Z)
				.Play();
		}

		public static RaTweenAnchorPos3D TweenAnchorPos3D(this RectTransform self, Vector3 pos, float duration)
		{
			return new RaTweenAnchorPos3D(self, pos, duration).Play();
		}

		public static RaTweenAnchorPos3D TweenAnchorPos3D(this RectTransform self, Vector3 startPos, Vector3 endPos, float duration)
		{
			return new RaTweenAnchorPos3D(self, startPos, endPos, duration).Play();
		}

		public static RaTweenAnchorPos3D TweenAnchorPos3D(this RectTransform self, Vector3 startPos, RectTransform endTarget, float duration)
		{
			return new RaTweenAnchorPos3D(self, startPos, endTarget, duration).Play();
		}
	}

	#endregion
}