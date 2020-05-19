using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Broodmother
{
    public class BroodBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Ball");
        }

        public override void SetDefaults()
        {
            projectile.height = 22;
            projectile.width = 22;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.timeLeft = 600;
        }

        public override void AI()
        {
			projectile.rotation += projectile.velocity.Length() * 0.025f;
            projectile.velocity.Y += .15f;

            bool explode = false;
            for(int i = 0; i < 255 && !explode; i++)
            {
                if(Main.player[i].active && Math.Abs(Main.player[i].Center.X - projectile.Center.X) + Math.Abs(Main.player[i].Center.Y - projectile.Center.Y) < 66)
                {
                    explode = true;
                }
            }

            Vector2 tile = new Vector2(projectile.Center.X, projectile.Center.Y + projectile.height / 2);
            bool tileCheck = TileID.Sets.Platforms[Main.tile[(int)(tile.X / 16), (int)(tile.Y / 16)].type];
            if (tileCheck) 
            {
                projectile.velocity.X = 0f;
                projectile.velocity.Y = 0f;
                if(explode) projectile.Kill();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bool explode = false;
            for(int i = 0; i < 255 && !explode; i++)
            {
                if(Main.player[i].active && Math.Abs(Main.player[i].Center.X - projectile.Center.X) + Math.Abs(Main.player[i].Center.Y - projectile.Center.Y) < 66)
                {
                    explode = true;
                }
            }
            if(explode) projectile.Kill();
            return explode;
        }
		
        public override void Kill(int timeLeft)
        {
            for (int num468 = 0; num468 < 30; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, 1, ModLoader.GetMod("AAMod").DustType("BroodmotherDust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("BroodmotherDust"), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default);
                Main.dust[num469].velocity *= 2f;
            }
			if(Main.netMode != NetmodeID.MultiplayerClient)
			{
				int projID = Projectile.NewProjectile(projectile.Top.X, projectile.Top.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("BroodBoom"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[projID].Bottom = projectile.Bottom + new Vector2(0, 10);
				Main.projectile[projID].netUpdate = true;
			}
        }

		public override Color? GetAlpha(Color lightColor)
		{
			return ColorUtils.COLOR_GLOWPULSE;
		}
    }
}