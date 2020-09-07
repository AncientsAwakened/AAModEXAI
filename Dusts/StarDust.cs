using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace AAModEXAI.Dusts
{
    public class StarDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity.Y = Main.rand.Next(-10, 6) * 0.1f;
			dust.velocity.X *= 0.3f;
			dust.scale *= 0.7f;
            dust.noGravity = false;
		}

		public override bool MidUpdate(Dust dust)
		{
			if (!dust.noGravity)
			{
				dust.velocity.Y += 0.05f;
			}
			if (!dust.noLight)
			{
				float strength = dust.scale * 1.4f;
				if (strength > 1f)
				{
					strength = 1f;
				}
				Lighting.AddLight(dust.position, 0.3f * strength, 0.0f * strength, 0.1f * strength);
			}
			return false;
		}

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return new Color(200, 100, 0, 25);
        }
    }
}