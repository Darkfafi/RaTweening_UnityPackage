using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public class RaTweenPosition : RaTween
	{
		#region Editor Variables

		[SerializeField]
		private Transform _target = default;

		[SerializeField]
		private Vector3 _startPos = Vector3.zero;

		[SerializeField]
		private Vector3 _endPos = Vector3.zero;

		[SerializeField]
		private bool _dynamicStartPosition = false;

		#endregion

		#region Properties

		public override bool IsValid => _target != null;

		#endregion

		public RaTweenPosition()
			: this(null, Vector3.zero, Vector3.zero, default, 0f)
		{

		}

		public RaTweenPosition(Transform target, Vector3 startPos, Vector3 endPos, AnimationCurve easing, float delay)
			: base(easing, delay)
		{
			_target = target;
			_startPos = startPos;
			_endPos = endPos;
			_dynamicStartPosition = false;
		}

		public RaTweenPosition(Transform target, Vector3 endPos, AnimationCurve easing, float delay)
			: this(target, target.position, endPos, easing, delay)
		{
			_dynamicStartPosition = true;
		}

		#region Protected

		protected override void OnStart()
		{
			base.OnStart();

			if(_dynamicStartPosition)
			{
				_startPos = _target.position;
			}
		}

		protected override RaTweenCore CloneSelf()
		{
			RaTweenPosition tween = new RaTweenPosition(_target, _startPos, _endPos, Easing, Delay);
			tween._dynamicStartPosition = _dynamicStartPosition;
			return tween;
		}

		protected override void Evaluate(float normalizedValue)
		{
			if(_target != null)
			{
				Vector3 delta = _endPos - _startPos;
				_target.position = _startPos + (delta * normalizedValue);
			}
		}

		protected override void Dispose()
		{
			_target = null;
		}

		#endregion
	}

	#region Extensions

	public static class RaTweenPositionExtensions
	{
		public static RaTweenCore TweenMoveX(this Transform self, float posX, AnimationCurve easing, float delay = 0f)
		{
			return new RaTweenPosition(self, Vector3.right * posX, easing, delay).Play();
		}

		public static RaTweenCore TweenMoveY(this Transform self, float posY, AnimationCurve easing, float delay = 0f)
		{
			return new RaTweenPosition(self, Vector3.up * posY, easing, delay).Play();
		}

		public static RaTweenCore TweenMoveZ(this Transform self, float posZ, AnimationCurve easing, float delay = 0f)
		{
			return new RaTweenPosition(self, Vector3.forward * posZ, easing, delay).Play();
		}

		public static RaTweenCore TweenMove(this Transform self, Vector3 pos, AnimationCurve easing, float delay = 0f)
		{
			return new RaTweenPosition(self, pos, easing, delay).Play();
		}

		public static RaTweenCore TweenMove(this Transform self, Vector3 startPos, Vector3 endPos, AnimationCurve easing, float delay = 0f)
		{
			return new RaTweenPosition(self, startPos, endPos, easing, delay).Play();
		}
	}

	#endregion
}