using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Zero
{
    public class ZeroBeam1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Beam");
        }
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.penetrate = 4;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;                 //this make that the projectile does not go thru walls
            projectile.ignoreWater = false;
            projectile.alpha = 0;
            projectile.extraUpdates = 1;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return AAColor.ZeroShield;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.7f / 255f, (255 - projectile.alpha) * 0.3f / 255f, (255 - projectile.alpha) * 0f / 255f);
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            projectile.localAI[0] += 1f;
            projectile.alpha += 4;
            if (projectile.alpha >= 255)
            {
                projectile.Kill();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}