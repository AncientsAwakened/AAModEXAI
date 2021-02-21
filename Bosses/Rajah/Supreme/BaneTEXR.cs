using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Rajah.Supreme
{
    public class BaneTEXR : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bane of the Bunny");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.aiStyle = -1;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.extraUpdates = 1;
        }

        public bool StuckInEnemy = false;
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Rectangle myRect = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
            bool flag3 = projectile.Colliding(myRect, target.getRect());
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("SpearStuck"), 2);
            if (flag3 && !StuckInEnemy)
            {
                StuckInEnemy = true;
                projectile.ai[0] = 1f;
                projectile.ai[1] = target.whoAmI;
                projectile.velocity = (target.Center - projectile.Center) * 0.75f;
                projectile.netUpdate = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.ai[0] == 1f)
            {
               ;
            }
        }

        public override void AI()
        {
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 25;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (projectile.ai[0] == 0f)
            {
                projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
            }
            if (projectile.ai[0] == 1f)
            {
                projectile.ignoreWater = true;
                projectile.tileCollide = false;
                int num977 = 15;
                bool flag53 = false;
                projectile.localAI[0] += 1f;
                int num978 = (int)projectile.ai[1];
                if (projectile.localAI[0] >= 60 * num977)
                {
                    flag53 = true;
                }
                else if (num978 < 0 || num978 >= 200)
                {
                    flag53 = true;
                }
                else if (Main.player[num978].active || !Main.player[num978].dead)
                {
                    projectile.Center = Main.player[num978].Center - projectile.velocity * 2f;
                    projectile.gfxOffY = Main.player[num978].gfxOffY;
                }
                else
                {
                    flag53 = true;
                }
                if (flag53)
                {
                    projectile.Kill();
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height, 0, 2);
            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 1, frame, lightColor, true);
            return false;
        }
    }
}
