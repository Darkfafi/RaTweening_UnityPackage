using System;
using UnityEngine;

namespace RaTweening
{
	public static class RaVector3Options
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

			if(!inclAxis.HasFlag(Axis.Z))
			{
				returnValue |= Axis.Z;
			}

			return returnValue;
		}

		public static Vector3 ApplyExcludeAxes(Vector3 original, Vector3 final, Axis excludeAxes)
		{
			if(excludeAxes.HasFlag(Axis.X))
			{
				final.x = original.x;
			}

			if(excludeAxes.HasFlag(Axis.Y))
			{
				final.y = original.y;
			}

			if(excludeAxes.HasFlag(Axis.Z))
			{
				final.z = original.z;
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
			Z = 4,
			All = X | Y | Z
		}

		#endregion
	}
}