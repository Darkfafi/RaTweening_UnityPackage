using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public class RaTweenPosition : RaTweenBase
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

		protected override RaTweenBase CloneSelf()
		{
			return new RaTweenPosition(_target, _startPos, _endPos, Easing, Delay);
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
}