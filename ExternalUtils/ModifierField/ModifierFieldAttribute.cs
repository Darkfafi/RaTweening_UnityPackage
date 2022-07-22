using UnityEngine;
using System;

namespace RaTweening.Tools
{
	/// <summary>
	/// Draws the field/property ONLY if the compared property compared by the comparison type with the value of comparedValue returns true.
	/// Based on: https://forum.unity.com/threads/draw-a-field-only-if-a-condition-is-met.448855/
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public class ModifierFieldAttribute : PropertyAttribute
	{
		#region Properties

		public string ComparedPropertyName
		{
			get; private set;
		}

		public object ComparedValue
		{
			get; private set;
		}

		public DisableType DisablingType
		{
			get; private set;
		}

		public string Rename
		{
			get; private set;
		}

		public bool ReverseCondition
		{
			get; private set;
		}

		/// <summary>
		/// Types of comperisons.
		/// </summary>
		public enum DisableType
		{
			ReadOnly = 2,
			DontDraw = 3,
			Rename = 4,
		}

		#endregion

		public ModifierFieldAttribute(string comparedPropertyName, object comparedValue, DisableType disablingType = DisableType.DontDraw, bool reverseCondition = false)
		{
			ComparedPropertyName = comparedPropertyName;
			ComparedValue = comparedValue;
			DisablingType = disablingType;
			ReverseCondition = reverseCondition;
		}

		public ModifierFieldAttribute(string comparedPropertyName, object comparedValue, string rename, bool reverseCondition = false)
		{
			ComparedPropertyName = comparedPropertyName;
			ComparedValue = comparedValue;
			DisablingType = DisableType.Rename;
			Rename = rename;
			ReverseCondition = reverseCondition;
		}
	}
}