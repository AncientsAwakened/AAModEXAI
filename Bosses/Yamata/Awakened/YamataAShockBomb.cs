using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
    public class YamataAShockBomb : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyssal Storm");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 90;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = 1;
            projectile.alpha = 0;
            cooldownSlot = 1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 250, 250, 0);
        }

        public override void AI()
        {
        	Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0f / 255f, (255 - projectile.alpha) * 0.4f / 255f);
        	projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
        	if (projectile.ai[1] == 0f)
			{
				projectile.ai[1] = 1f;
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 20);
			}
            projectile.velocity.Y += 0.2f;
            if (projectile.ai[0] > -1 && projectile.ai[0] < 255 && projectile.Center.Y > Main.player[(int)projectile.ai[0]].Center.Y)
                projectile.Kill();
        }
        
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
        	target.AddBuff(mod.BuffType("HydraToxin"), 300);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(2, 89, Terraria.Audio.SoundType.Sound));
	    	if (Main.netMode != NetmodeID.MultiplayerClient)
	    	{
                const float ai0 = 20;
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("Shockwave2"), projectile.damage, projectile.knockBack, projectile.owner, ai0);
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("Shockwave2"), projectile.damage, projectile.knockBack, projectile.owner, -ai0);
            }
        	for (int dust = 0; dust <= 10; dust++)
        	{
        		Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 235, projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
        	}
        }
    }
}