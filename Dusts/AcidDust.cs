using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace AAModEXAI.Dusts
{
    public class AcidDust : ModDust
	{
        public override void OnSpawn(Dust dust)
        {
            dust.scale *= 1.3f;
            dust.alpha = 20;
        }

        public override bool Update(Dust dust)
        {
            bool flag5 = WorldGen.SolidTile(Framing.GetTileSafely((int)dust.position.X / 16, (int)dust.position.Y / 16));
            if (flag5)
            {
                dust.noLight = true;
            }
            return true;
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
            }
            if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f)
            {
                dust.scale *= 0.9f;
                dust.velocity *= 0.10f;
            }
            return false;
        }
    }
}