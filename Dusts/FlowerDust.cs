using Terraria;
using Terraria.ModLoader;
namespace AAModEXAI.Dusts
{
    public class FlowerDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity.Y *= 3f;
			dust.velocity.X *= 3f;
			dust.scale *= 2f;
			dust.noGravity = true;
			dust.noLight = true;
			dust.alpha = 100;
		}
	}
}