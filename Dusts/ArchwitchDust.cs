using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace AAModEXAI.Dusts
{
    public class ArchwitchDust : ModDust
	{
        public override void OnSpawn(Dust dust)
        {
            dust.scale *= 1.3f;
        }

        public override bool MidUpdate(Dust dust)
        {
            dust.rotation += dust.velocity.X / 3f;
            if (!dust.noLight)
            {
                float strength = dust.scale * 1.4f;
                if (strength > 1f)
                {
                    strength = 1f;
                }
                Lighting.AddLight(dust.position, 0.05f * strength, 0f * strength, 0.1f * strength);
            }
            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return new Color(lightColor.R, lightColor.G, lightColor.B, 25);
        }
    }
}