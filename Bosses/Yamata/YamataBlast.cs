using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Yamata
{
    public class YamataBlast : ModProjectile
	{
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Venom");     
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
            
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("HydraToxin"), 300);
        }

        public override void Kill(int timeleft)
        {
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("YamataDust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, new Color(86, 191, 188));
                Main.dust[num469].velocity *= 2f;
            }
            Main.PlaySound(new LegacySoundStyle(2, 89, Terraria.Audio.SoundType.Sound));
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 51 + 8, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("Shock"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
        }
    }
}
