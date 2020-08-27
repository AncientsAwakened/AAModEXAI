using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
    public class YamataShot : ModProjectile
	{
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Blast");
            Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
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
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 51 + 8, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("Shockwave"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
        }
    }
}
