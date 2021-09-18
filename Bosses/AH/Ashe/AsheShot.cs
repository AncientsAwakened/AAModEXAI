using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.AH.Ashe
{
    public class AsheShot : ModProjectile
    {
    	
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dayfire");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 25;
            projectile.height = 25;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if(Main.npc[(int)projectile.ai[0]].type == ModContent.NPCType<AsheRune>())
            {
                if(projectile.ai[1] ++ < 30)
                {
                    projectile.alpha += 8;
                    projectile.scale = 0.8f;
                    projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
                    projectile.velocity = Vector2.Normalize(projectile.velocity) * .1f;
                }
                else if(projectile.ai[1] > 60)
                {
                    projectile.scale = 0.8f;
                    projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
                    projectile.velocity = Vector2.Normalize(projectile.velocity) * 10f;
                }
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
            }
            
            if(projectile.alpha > 255) projectile.alpha = 255;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Color.White.R, Color.White.G, Color.White.B, projectile.alpha);
        }

        public override void Kill(int timeLeft)
        {
            if(projectile.ai[0] == 0 && projectile.ai[1] == 0) Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 124, Terraria.Audio.SoundType.Sound));
            float spread = 45f * 0.0174f;
            double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - (spread / 2);
            double deltaAngle = spread / 8f;
            Projectile.NewProjectile(projectile.Center, new Vector2(0, 0), ModContent.ProjectileType<AsheBoom>(), projectile.damage, 2);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<DeBuffs.DragonFire>(), 200);
        }
    }
}