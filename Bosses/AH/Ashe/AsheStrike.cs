using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.AH.Ashe
{
    public class AsheStrike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dayfire");     
            Main.projFrames[projectile.type] = 5;     
        }

        public override void SetDefaults()
        {
            projectile.width = 98;
            projectile.height = 98;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 600;

            drawOffsetX = -49;
			drawOriginOffsetY = -49;
        }

        public override void AI()
        {
            if (++projectile.frameCounter >= 9)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 5)
                {
                    projectile.Kill();

                }
            }
            projectile.velocity.X *= 0.00f;
            projectile.velocity.Y *= 0.00f;

        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Color.White.R, Color.White.G, Color.White.B, projectile.alpha);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DragonFire"), 200);
        }

        public override void Kill(int timeLeft)
        {
            projectile.timeLeft = 0;
        }
    }
}
