using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Hydra
{
    public class HydraBomb : ModProjectile
	{
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Bomb");     
            Main.projFrames[projectile.type] = 5;     
		}

		public override void SetDefaults()
		{
			projectile.width = 14;               
			projectile.height = 14;              
			projectile.aiStyle = 1;             
			projectile.friendly = false;         
			projectile.hostile = true;        
			projectile.penetrate = 1;           
			projectile.timeLeft = 600;          
			projectile.alpha = 20;              
			projectile.ignoreWater = true;
			projectile.tileCollide = true;        
			aiType = ProjectileID.WoodenArrowFriendly;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 600);
        }

        public override void Kill(int timeleft)
        {
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("AcidDust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("AcidDust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default);
                Main.dust[num469].velocity *= 2f;
            }
            Gore.NewGore(projectile.Center, -projectile.oldVelocity * 0.2f, 704, 1f);
            Gore.NewGore(projectile.Center, -projectile.oldVelocity * 0.2f, 705, 1f);
            if (projectile.owner == Main.myPlayer)
            {
                int num319 = Main.rand.Next(20, 31);
                for (int num320 = 0; num320 < num319; num320++)
                {
                    Vector2 value21 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    value21.Normalize();
                    value21 *= Main.rand.Next(10, 201) * 0.01f;
                    int a = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, value21.X, value21.Y, mod.ProjectileType("HydraMist"), projectile.damage, 1f, projectile.owner, 0f, Main.rand.Next(-45, 1));
                    Main.projectile[a].localAI[0] = Main.rand.Next(3);
                }
            }
        }
    }
}
