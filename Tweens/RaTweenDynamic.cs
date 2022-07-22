using RaTweening.Tools;
using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public abstract class RaTweenDynamic<TargetT, ValueT> : RaTween, IRaTweenDynamic
	{
		#region Editor Variables

		[Header("Properties")]
		[SerializeField]
		private TargetT _target = default;

		[SerializeField]
		[ModifierField(nameof(_dynamicStart), true, ModifierFieldAttribute.DisableType.DontDraw)]
		private ValueT _start = default;

		[SerializeField]
		[ModifierField(nameof(_endIsDelta), true, "Delta")]
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

		protected ValueT StartValue
		{
			get; private set;
		}

		protected ValueT EndValue
		{
			get; private set;
		}

		public override bool IsValid => _target != null;

		#endregion

		public RaTweenDynamic()
			: base()
		{

		}

		public RaTweenDynamic(TargetT target, ValueT start, ValueT end, float duration)
			: base(duration)
		{
			_target = target;
			_start = start;
			_end = end;
			_dynamicStart = false;
			_endIsDelta = false;
			_dynamicSetupStep = DynamicSetupStep.Setup;
		}

		public RaTweenDynamic(TargetT target, ValueT end, float duration)
			: this(target, default, end, duration)
		{
			_dynamicStart = true;
		}

		#region Public Methods

		Type IRaTweenDynamic.GetTargetTypeRaw()
		{
			return typeof(TargetT);
		}

		void IRaTweenDynamic.SetTargetRaw(object value)
		{
			if(value is TargetT target)
			{
				_target = target;
			}
		}

		public RaTweenDynamic<TargetT, ValueT> SetTarget(TargetT target)
		{
			if(CanBeModified())
			{
				_target = target;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueT> SetStart(ValueT start)
		{
			if(CanBeModified())
			{
				_start = start;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueT> SetEnd(ValueT end)
		{
			if(CanBeModified())
			{
				_end = end;
			}
			return this;
		}

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

		#endregion

		#region Protected Methods

		protected abstract RaTweenDynamic<TargetT, ValueT> DynamicClone();
		protected abstract ValueT GetDynamicStart();
		protected abstract ValueT GetEndByDelta(ValueT start, ValueT delta);

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStart(Target != null ? GetDynamicStart() : default);
		}

		protected override RaTween RaTweenClone()
		{
			RaTweenDynamic<TargetT, ValueT> clone = DynamicClone();
			clone._target = _target;
			clone._dynamicStart = _dynamicStart;
			clone._start = _start;
			clone._endIsDelta = _endIsDelta;
			clone._end = _end;
			clone._dynamicSetupStep = _dynamicSetupStep;
			return clone;
		}

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
				DynamicEvaluation(normalizedValue, _target, StartValue, EndValue);
			}
		}

		protected override void Dispose()
		{
			_target = default;
			StartValue = default;
			EndValue = default;
		}

		protected abstract void DynamicEvaluation(float normalizedValue, TargetT target, ValueT start, ValueT end);

		#endregion

		#region Private Methods

		private void PerformDynamicSetup()
		{
			StartValue = _dynamicStart ? GetDynamicStart() : _start;
			EndValue = _endIsDelta ? GetEndByDelta(StartValue, _end) : _end;
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

	public interface IRaTweenDynamic
	{
		Type GetTargetTypeRaw();
		void SetTargetRaw(object value);
	}
}