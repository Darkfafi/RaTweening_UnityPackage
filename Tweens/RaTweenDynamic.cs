using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public abstract class RaTweenDynamic<TargetT, ValueT> : RaTween
	{
		#region Editor Variables

		[Header("Properties")]
		[SerializeField]
		private TargetT _target = default;

		[SerializeField]
		private ValueT _start = default;

		[SerializeField]
		private ValueT _end = default;

		[Header("Dynamic")]
		[SerializeField]
		[Tooltip("Dynamic Start will calculate the Start value at runtime duing the defined DynamicSetupStep")]
		private bool _dynamicStart = false;

		[SerializeField]
		[Tooltip("End Is Delta will calculate the End value at runtime duing the defined DynamicSetupStep, relative to the Start Value")]
		private bool _endIsDelta = false;

		[SerializeField]
		[Tooltip("The Step when to calculate the dynamic values")]
		private DynamicSetupStep _dynamicSetupStep = DynamicSetupStep.Setup;

		#endregion

		#region Properties

		protected TargetT Target => _target;

		protected ValueT SetStart
		{
			get; private set;
		}

		protected ValueT SetEnd
		{
			get; private set;
		}

		public override bool IsValid => _target != null;

		#endregion

		public RaTweenDynamic()
			: this(default, default, default, default)
		{

		}

		public RaTweenDynamic(TargetT target, ValueT start, ValueT end, AnimationCurve easing, bool endIsDelta = false)
			: base(easing)
		{
			_target = target;
			_start = start;
			_end = end;
			_dynamicStart = false;
			_endIsDelta = endIsDelta;
			_endIsDelta = false;
		}

		public RaTweenDynamic(TargetT target, ValueT end, AnimationCurve easing, bool endIsDelta = false)
			: this(target, default, end, easing, endIsDelta)
		{
			_dynamicStart = true;
			_start = default;
		}

		#region Public Methods

		public RaTweenDynamic<TargetT, ValueT> SetEndIsDelta(bool enabled = true)
		{
			if(CanBeModified())
			{
				_endIsDelta = enabled;
			}

			return this;
		}

		public RaTweenDynamic<TargetT, ValueT> SetStartDynamic(bool enabled = true)
		{
			if(CanBeModified())
			{
				_dynamicStart = enabled;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueT> SetDynamicSetupStep(DynamicSetupStep step)
		{
			if(CanBeModified())
			{
				_dynamicSetupStep = step;
			}
			return this;
		}

		public override RaTweenCore Clone()
		{
			RaTweenDynamic<TargetT, ValueT> clone = base.Clone() as RaTweenDynamic<TargetT, ValueT>;
			clone._target = _target;
			clone._dynamicStart = _dynamicStart;
			clone._start = _start;
			clone._endIsDelta = _endIsDelta;
			clone._end = _end;
			clone._dynamicSetupStep = _dynamicSetupStep;
			clone.SetEasing(Easing);
			return clone;
		}

		#endregion

		#region Protected Methods

		protected abstract ValueT GetDynamicStart();
		protected abstract ValueT GetEndByDelta(ValueT start, ValueT delta);

		protected override void OnSetup()
		{
			base.OnSetup();

			if(_dynamicSetupStep == DynamicSetupStep.Setup)
			{
				PerformDynamicSetup();
			}
		}

		protected override void OnStart()
		{
			base.OnStart();

			if(_dynamicSetupStep == DynamicSetupStep.Start)
			{
				PerformDynamicSetup();
			}
		}

		protected override void Evaluate(float normalizedValue)
		{
			if(_target != null)
			{
				DynamicEvaluation(normalizedValue, _target, SetStart, SetEnd);
			}
		}

		protected override void Dispose()
		{
			_target = default;
			SetStart = default;
			SetEnd = default;
		}

		protected abstract void DynamicEvaluation(float normalizedValue, TargetT target, ValueT start, ValueT end);

		#endregion

		#region Private Methods

		private void PerformDynamicSetup()
		{
			SetStart = _dynamicStart ? GetDynamicStart() : _start;
			SetEnd = _endIsDelta ? GetEndByDelta(SetStart, _end) : _end;
		}

		#endregion

		#region Nested

		public enum DynamicSetupStep
		{
			Setup,
			Start
		}

		#endregion
	}
}