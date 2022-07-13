using System;

namespace RaTweening
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class RaTweenerElementAttribute : Attribute
	{
		public readonly Type ElementSOType;

		public RaTweenerElementAttribute(Type type)
		{
			ElementSOType = type;
		}
	}
}