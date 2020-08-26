using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
    public class AbyssalThunder : ModProjectile
	{
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Thunder");
            Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.width = 70;
			projectile.height = 70;
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
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("HydraToxin"), 600);
        }

        public override void PostAI()
        {
            if (projectile.frameCounter++ > 6)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeleft)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 89, Terraria.Audio.SoundType.Sound));
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("YamataADust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y - 2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("YamataADust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y - 4f, 100, default);
                Main.dust[num469].velocity *= 2f;
            }
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 101 + 8, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("Shockwave2"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
        }
    }
}
