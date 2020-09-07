using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AAMod;using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Greed.OreBomb
{
    public class OreBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BoulderStaffOfEarth);
            projectile.penetrate = 1;  
            projectile.width = 44;
            projectile.height = 44;
			projectile.friendly = false;
			projectile.hostile = true;
            projectile.timeLeft = 300;
        }

		public override void SetStaticDefaults()
		{
		    DisplayName.SetDefault("Ore Cluster");
		}

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return true;
        }

        public override void Kill(int a)
        {
            Main.PlaySound(SoundID.Item14, projectile.position);
            for (int i = 0; i < Main.rand.Next(5, 10); i++)
            {
                int x = Main.rand.Next(-6, 6);
                int y = -Main.rand.Next(3, 5);
                int p = Projectile.NewProjectile(projectile.position, new Vector2(x, y), ModContent.ProjectileType<OreChunkM>(), projectile.damage, projectile.knockBack, Main.myPlayer, 0, Main.rand.Next(28));
                Main.projectile[p].Center = projectile.Center - new Vector2(0, 25);

                if (Main.projectile[p].ai[1] == 10)
                {
                    Main.projectile[p].knockBack *= 1.5f;
                }
                if (Main.projectile[p].ai[1] == 19)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Vector2 perturbedSpeed = new Vector2(x, y).RotatedByRandom(MathHelper.ToRadians(20));
                        int q = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<OreChunkM>(), projectile.damage, projectile.knockBack, Main.myPlayer, 5, 19);
                        Main.projectile[q].Center = projectile.Center - new Vector2(0, 4);
                    }
                }
            }
        }
    }
}
