
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Zero
{
    class RiftSlashZ : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50;
            projectile.aiStyle = 27;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 240;
        }

        public override void PostAI()
        {
            projectile.timeLeft--;
            if (projectile.timeLeft <= 0)
            {
                projectile.Kill();
            }
            Lighting.AddLight(projectile.Center, .5f, 0f, .1f);
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color lightColor)
        {
            BaseDrawing.DrawAfterimage(spritebatch, Main.projectileTexture[projectile.type], 0, projectile, 1.5f, 1f, 5, false, 0f, 0f, projectile.GetAlpha(AAColor.ZeroShield));
            return true;
        }
    }
}
