using System;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;

namespace AAModEXAI
{
    public class RadialDitherTopMiddle : GenAction
	{
		private readonly int _width;
		private readonly float _innerRadius, _outerRadius;

		public RadialDitherTopMiddle(int width, int height, float innerRadius, float outerRadius)
		{
			_width = width;
			_innerRadius = innerRadius;
			_outerRadius = outerRadius;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			Vector2 value = new Vector2((float)origin.X + (_width / 2), origin.Y);
			Vector2 value2 = new Vector2(x, y);
			float num = Vector2.Distance(value2, value);
			float num2 = Math.Max(0f, Math.Min(1f, (num - _innerRadius) / (_outerRadius - _innerRadius)));
			if (_random.NextDouble() > num2)
			{
				return UnitApply(origin, x, y, args);
			}
			return Fail();
		}
	}
	
	#region Custom Modifiers
	
	#endregion
}