using RaTweening.Tools;
using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public abstract class RaTweenDynamic<TargetT, ValueT> : RaTween, IRaTweenTarget
	{
		#region Editor Variables

		[Header("Properties")]
		[SerializeField]
		private TargetT _target = default;

		// Start
		[SerializeField]
		[ModifierField(new string[] { nameof(_useStartRef), nameof(_dynamicStart) }, new object[] { false, true }, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.Any)]
		private TargetT _startRef = default;

		[SerializeField]
		[ModifierField(new string[] { nameof(_useStartRef), nameof(_dynamicStart) }, new object[] { true, true }, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.Any)]
		private ValueT _startValue = default;

		[SerializeField]
		private bool _useStartRef = false;

		[SerializeField]
		[Tooltip("Dynamic Start will calculate the Start value at runtime duing the defined DynamicSetupStep")]
		private bool _dynamicStart = false;

		// End
		[SerializeField]
		[ModifierField(nameof(_useEndRef), false, ModifierFieldAttribute.ModType.DontDraw)]
		private TargetT _endRef = default;
		
		[SerializeField]
		[ModifierField(nameof(_useEndRef), true, ModifierFieldAttribute.ModType.DontDraw)]
		private ValueT _endValue = default;

		[SerializeField]
		private bool _useEndRef = false;

		[SerializeField]
		[Tooltip("End Is Delta will calculate the End value at runtime duing the defined DynamicSetupStep, relative to the Start Value")]
		private bool _endIsDelta = false;

		[SerializeField]
		[Tooltip("The Step when to calculate the dynamic values")]
		private RaTweenDynamicSetupStep _dynamicSetupStep = RaTweenDynamicSetupStep.Setup;

		#endregion

		#region Properties

		protected TargetT Target => _target;

		protected ValueT Start
		{
			get; private set;
		}

		protected ValueT End
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
			SetStartValue(start);
			SetEndValue(end);
			_dynamicSetupStep = RaTweenDynamicSetupStep.Setup;
		}

		public RaTweenDynamic(TargetT target, ValueT end, float duration)
			: this(target, default, end, duration)
		{
			SetStartDynamic(true);
		}

		#region Public Methods

		// Raw
		Type IRaTweenTarget.GetTargetTypeRaw()
		{
			return typeof(TargetT);
		}

		void IRaTweenTarget.SetTargetRaw(object value)
		{
			if(value is TargetT target)
			{
				_target = target;
			}
		}

		// API
		public RaTweenDynamic<TargetT, ValueT> SetStartRef(TargetT start)
		{
			if(CanBeModified())
			{
				_useStartRef = true;
				_startRef = start;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueT> SetStartValue(ValueT start)
		{
			if(CanBeModified())
			{
				_useStartRef = false;
				_startValue = start;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueT> SetEndRef(TargetT end)
		{
			if(CanBeModified())
			{
				_useEndRef = true;
				_endRef = end;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueT> SetEndValue(ValueT end)
		{
			if(CanBeModified())
			{
				_useEndRef = false;
				_endValue = end;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueT> SetTarget(TargetT target)
		{
			if(CanBeModified())
			{
				_target = target;
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

		public RaTweenDynamic<TargetT, ValueT> SetDynamicSetupStep(RaTweenDynamicSetupStep step)
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

		protected abstract ValueT GetEndByDelta(ValueT start, ValueT delta);

		protected abstract ValueT ReadValue(TargetT reference);

		protected override RaTween RaTweenClone()
		{
			RaTweenDynamic<TargetT, ValueT> clone = DynamicClone();

			// Generic
			clone._target = _target;
			clone._dynamicSetupStep = _dynamicSetupStep;

			// Start
			clone._startValue = _startValue;
			clone._startRef = _startRef;
			clone._useStartRef = _useStartRef;
			clone._dynamicStart = _dynamicStart;

			// End
			clone._endValue = _endValue;
			clone._endRef = _endRef;
			clone._useEndRef = _useEndRef;
			clone._endIsDelta = _endIsDelta;

			return clone;
		}

		protected override void OnSetup()
		{
			base.OnSetup();

			if(_dynamicSetupStep == RaTweenDynamicSetupStep.Setup)
			{
				PerformDynamicSetup();
			}
		}

		protected override void OnStart()
		{
			base.OnStart();

			if(_dynamicSetupStep == RaTweenDynamicSetupStep.Start)
			{
				PerformDynamicSetup();
			}
		}

		protected override void Evaluate(float normalizedValue)
		{
			if(_target != null)
			{
				DynamicEvaluation(normalizedValue, _target, Start, End);
			}
		}

		protected override void Dispose()
		{
			_target = default;

			_endRef = default;
			_endValue = default;

			_startRef = default;
			_startValue = default;
		}

		protected abstract void DynamicEvaluation(float normalizedValue, TargetT target, ValueT start, ValueT end);

		#endregion

		#region Private Methods

		private void PerformDynamicSetup()
		{
			if(_dynamicStart)
			{
				Start = ReadValue(Target);
			}
			else
			{
				Start = _useStartRef ? ReadValue(_startRef) : _startValue;
			}

			if(_useEndRef)
			{
				End = _endIsDelta ? GetEndByDelta(Start, ReadValue(_endRef)) : ReadValue(_endRef);
			}
			else
			{
				End = _endIsDelta ? GetEndByDelta(Start, _endValue) : _endValue;
			}
		}

		#endregion
	}

	public enum RaTweenDynamicSetupStep
	{
		Setup,
		Start
	}
}