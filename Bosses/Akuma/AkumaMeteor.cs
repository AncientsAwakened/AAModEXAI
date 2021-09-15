using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Akuma
{
    public class AkumaMeteor : ModProjectile
    {
    	
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dayfire");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            projectile.velocity.Y += .01f;
            if (projectile.position.Y > Main.player[projectile.owner].position.Y - 300f)
            {
                projectile.tileCollide = true;
            }
            if (projectile.position.Y < Main.worldSurface * 16.0)
            {
                projectile.tileCollide = true;
            }
            projectile.scale = projectile.ai[1];
            projectile.rotation = projectile.velocity.ToRotation() - 1.57079637f;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 124, Terraria.Audio.SoundType.Sound));
            float spread = 45f * 0.0174f;
            double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - (spread / 2);
            double deltaAngle = spread / 8f;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int proj = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("AkumaBoom"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                Main.projectile[proj].netUpdate = true;
            }
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModContent.DustType<AkumaDust>(), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 0);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModContent.DustType<AkumaDust>(), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 0);
                Main.dust[num469].velocity *= 2f;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<DeBuffs.DragonFire>(), 200);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            Vector2 Drawpos = projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY);

            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], shader, projectile, Color.White, true);
            return false;
        }
    }
}