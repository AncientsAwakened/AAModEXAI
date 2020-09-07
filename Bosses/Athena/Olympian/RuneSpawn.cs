
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Athena.Olympian
{
    public class RuneSpawn : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Varian Burst");
            Main.projFrames[projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 36;
			projectile.friendly = false; 
			projectile.hostile = true;
			projectile.melee = true;
			projectile.penetrate = 1;
			projectile.alpha = 20;
            projectile.tileCollide = false;
			projectile.ignoreWater = true;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (projectile.frameCounter++ > 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 2)
                {
                    projectile.frame = 0;
                }
            }
            for(int i = 0; i < 200; i++)
            {
                if(Main.player[i].active && Main.player[i].statLife > 0 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.player[i].position, Main.player[i].width, Main.player[i].height))
                {
                    projectile.Kill();
                    return;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 3, 0, 0);
            BaseDrawing.DrawAfterimage(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile, .5f, 1f, 10, false, 0f, 0f, Color.White, frame, 3);
            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 3, frame, Color.White, false);
            return false;
        }

        public override void Kill(int timeleft)
        {
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
            int p = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, mod.NPCType("OwlRuneCharged"));
            Main.npc[p].Center = projectile.Center;
            for (int num468 = 0; num468 < 10; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.Electric, -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
            }
            
        }
    }
}
