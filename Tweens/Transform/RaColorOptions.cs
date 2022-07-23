using System;
using UnityEngine;

namespace RaTweening
{
	public static class RaColorOptions
	{
		#region Public Methods

		public static Channel GetOnlyIncludeChannels(Channel inclChannels)
		{
			Channel returnValue = Channel.None;

			if(!inclChannels.HasFlag(Channel.R))
			{
				returnValue |= Channel.R;
			}

			if(!inclChannels.HasFlag(Channel.G))
			{
				returnValue |= Channel.G;
			}

			if(!inclChannels.HasFlag(Channel.B))
			{
				returnValue |= Channel.B;
			}

			if(!inclChannels.HasFlag(Channel.A))
			{
				returnValue |= Channel.A;
			}

			return returnValue;
		}

		public static Color ApplyExcludeChannels(Color original, Color final, Channel excludeChannels)
		{
			if(excludeChannels.HasFlag(Channel.R))
			{
				final.r = original.r;
			}

			if(excludeChannels.HasFlag(Channel.G))
			{
				final.g = original.g;
			}

			if(excludeChannels.HasFlag(Channel.B))
			{
				final.b = original.b;
			}

			if(excludeChannels.HasFlag(Channel.A))
			{
				final.a = original.a;
			}

			return final;
		}

		#endregion


		#region Nested

		[Flags]
		public enum Channel
		{
			None = 0,
			R = 1,
			G = 2,
			B = 4,
			A = 8,
			All = R | G | B | A
		}

		#endregion
	}
}