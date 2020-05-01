using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Akuma.Awakened
{
    public class AkumaABoom : ModProjectile
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

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DragonFire"), 200);
        }

        public override void Kill(int timeLeft)
        {
            projectile.timeLeft = 0;
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            int shader = Terraria.Graphics.Shaders.GameShaders.Armor.GetShaderIdFromItemId(Terraria.ID.ItemID.LivingOceanDye);
            Vector2 Drawpos = projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY);

            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 5, 0, 2);

            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], shader, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 5, frame, Color.White, true);
            return false;
        }


    }
}
