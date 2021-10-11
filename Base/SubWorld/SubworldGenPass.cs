using System;
using Terraria.World.Generation;

namespace AAModEXAI
{
	public class SubworldGenPass : GenPass
	{
		public SubworldGenPass(Action<GenerationProgress> method) : base("", 1f)
		{
			this.method = method;
		}

		public SubworldGenPass(float weight, Action<GenerationProgress> method) : base("", weight)
		{
			this.method = method;
		}

		public override void Apply(GenerationProgress progress)
		{
			this.method(progress);
		}

		private Action<GenerationProgress> method;
	}
}
