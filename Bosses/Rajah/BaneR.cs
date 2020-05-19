using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Rajah
{
    public class BaneR: ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bane of the Bunny");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.hostile = true;
            projectile.aiStyle = -1;
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

        public override void AI()
        {
            int num972 = 25;
            if (projectile.alpha > 0)
            {
                projectile.alpha -= num972;
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
                projectile.damage = 0;
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
                else if (Main.player[num978].active && !Main.player[num978].dead)
                {
                    projectile.Center = Main.player[num978].Center - projectile.velocity * 2f;
                    projectile.gfxOffY = Main.player[num978].gfxOffY;
                    Main.player[num978].AddBuff(BuffID.Bleeding, 2);
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
    }
}
