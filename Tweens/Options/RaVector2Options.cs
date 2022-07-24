using System;
using UnityEngine;

namespace RaTweening
{
	public static class RaVector2Options
	{
		#region Public Methods

		public static Axis GetOnlyIncludeAxes(Axis inclAxis)
		{
			Axis returnValue = Axis.None;

			if(!inclAxis.HasFlag(Axis.X))
			{
				returnValue |= Axis.X;
			}

			if(!inclAxis.HasFlag(Axis.Y))
			{
				returnValue |= Axis.Y;
			}

			return returnValue;
		}

		public static Vector2 ApplyExcludeAxes(Vector2 original, Vector2 final, Axis excludeAxes)
		{
			if(excludeAxes.HasFlag(Axis.X))
			{
				final.x = original.x;
			}

			if(excludeAxes.HasFlag(Axis.Y))
			{
				final.y = original.y;
			}

			return final;
		}

		#endregion


		#region Nested

		[Flags]
		public enum Axis
		{
			None = 0,
			X = 1,
			Y = 2,
			All = X | Y
		}

		#endregion
	}
}