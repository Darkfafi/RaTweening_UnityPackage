using RaTweening.Tools;
using System;
using UnityEngine;

namespace RaTweening
{
	[Serializable]
	public abstract class RaTweenDynamic<TargetT, ValueTRef, ValueT> : RaTween, IRaTweenDynamic
	{
		#region Editor Variables

		[Header("Properties")]
		[SerializeField]
		private TargetT _target = default;

		// Start
		[SerializeField]
		[ModifierField(new string[] { nameof(_useStartRef), nameof(_dynamicStart) }, new object[] { false, true }, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.Any)]
		private ValueTRef _startRef = default;

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
		private ValueTRef _endRef = default;
		
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
		private DynamicSetupStep _dynamicSetupStep = DynamicSetupStep.Setup;

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
			_dynamicSetupStep = DynamicSetupStep.Setup;
		}

		public RaTweenDynamic(TargetT target, ValueT end, float duration)
			: this(target, default, end, duration)
		{
			SetStartDynamic(true);
		}

		#region Public Methods

		// Raw
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

		// API
		public RaTweenDynamic<TargetT, ValueTRef, ValueT> SetStartRef(ValueTRef start)
		{
			if(CanBeModified())
			{
				_useStartRef = true;
				_startRef = start;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueTRef, ValueT> SetStartValue(ValueT start)
		{
			if(CanBeModified())
			{
				_useStartRef = false;
				_startValue = start;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueTRef, ValueT> SetEndRef(ValueTRef end)
		{
			if(CanBeModified())
			{
				_useEndRef = true;
				_endRef = end;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueTRef, ValueT> SetEndValue(ValueT end)
		{
			if(CanBeModified())
			{
				_useEndRef = false;
				_endValue = end;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueTRef, ValueT> SetTarget(TargetT target)
		{
			if(CanBeModified())
			{
				_target = target;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueTRef, ValueT> SetEndIsDelta(bool enabled = true)
		{
			if(CanBeModified())
			{
				_endIsDelta = enabled;
			}

			return this;
		}

		public RaTweenDynamic<TargetT, ValueTRef, ValueT> SetStartDynamic(bool enabled = true)
		{
			if(CanBeModified())
			{
				_dynamicStart = enabled;
			}
			return this;
		}

		public RaTweenDynamic<TargetT, ValueTRef, ValueT> SetDynamicSetupStep(DynamicSetupStep step)
		{
			if(CanBeModified())
			{
				_dynamicSetupStep = step;
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected abstract RaTweenDynamic<TargetT, ValueTRef, ValueT> DynamicClone();

		protected abstract ValueT GetStartFromRef(ValueTRef reference);
		protected abstract ValueT GetEndFromRef(ValueTRef reference);
		protected abstract ValueT GetDynamicStart(TargetT target);
		protected abstract ValueT GetEndByDelta(ValueT start, ValueT delta);

		protected override RaTween RaTweenClone()
		{
			RaTweenDynamic<TargetT, ValueTRef, ValueT> clone = DynamicClone();

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
				Start = GetDynamicStart(Target);
			}
			else
			{
				Start = _useStartRef ? GetStartFromRef(_startRef) : _startValue;
			}

			if(_useEndRef)
			{
				End = _endIsDelta ? GetEndByDelta(Start, GetEndFromRef(_endRef)) : GetEndFromRef(_endRef);
			}
			else
			{
				End = _endIsDelta ? GetEndByDelta(Start, _endValue) : _endValue;
			}
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