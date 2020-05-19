using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Toad
{
    public class ToadBomb : ModProjectile
	{
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Acid"); 
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
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("Shroomed"), 200);
        }

        public override void Kill(int timeleft)
        {
            Main.PlaySound(SoundID.Item14, projectile.position);
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("ShroomDust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, new Color(86, 191, 188), 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("ShroomDust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, new Color(86, 191, 188));
                Main.dust[num469].velocity *= 2f;
            }
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("ToadBoom"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
        }
    }
}
