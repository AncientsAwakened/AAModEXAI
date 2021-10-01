using Microsoft.Xna.Framework;
using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Rajah.Supreme.RoyalRabbit
{
    public class BlazingTerra : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            DisplayName.SetDefault("Blazing Terra");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.scale = 1.1f;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.alpha = 60;
            projectile.timeLeft = 360;
            projectile.tileCollide = false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            if (projectile.timeLeft > 0)
            {
                projectile.timeLeft--;
            }
            if (projectile.timeLeft == 0)
            {
                projectile.Kill();
            }

            projectile.frameCounter++;
            projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
            if (projectile.frameCounter > 6)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
            for (int num189 = 0; num189 < 1; num189++)
            {
                int num190 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.CarrotDust>(), 0f, 0f, 0);

                Main.dust[num190].scale *= 1.3f;
                Main.dust[num190].fadeIn = 1f;
                Main.dust[num190].noGravity = true;
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("InfinityOverload"), 600);
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 4, 0, 0);
            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 4, frame, Color.White, true);
            return false;
        }

        public override void Kill(int timeLeft)
        {
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, 1, ModContent.DustType<Dusts.CarrotDust>(), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
            }
			for (int k = 0; k < 8; k++)
			{
				Vector2 vel = new Vector2(0, -1);
				float rand = Main.rand.NextFloat() * 6.283f;
				vel = vel.RotatedBy(rand);
				vel *= 8f;
				int i = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ModContent.ProjectileType<FireTentacle>(), projectile.damage/3, 0, Main.myPlayer);
				Main.projectile[i].usesLocalNPCImmunity = true;
				Main.projectile[i].localNPCHitCooldown = 6;
				Main.projectile[i].penetrate = -1;
			}
        }
    }

    public class FireTentacle : ModProjectile
    {
        public override string Texture => "AAModEXAI/BlankTex";
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rabbit Flares");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.MaxUpdates = 3;
            projectile.melee = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
        	if (projectile.velocity.X != projectile.velocity.X)
			{
				if (Math.Abs(projectile.velocity.X) < 1f)
				{
					projectile.velocity.X = -projectile.velocity.X;
				}
				else
				{
					projectile.Kill();
				}
			}
			if (projectile.velocity.Y != projectile.velocity.Y)
			{
				if (Math.Abs(projectile.velocity.Y) < 1f)
				{
					projectile.velocity.Y = -projectile.velocity.Y;
				}
				else
				{
					projectile.Kill();
				}
			}
        	Vector2 center10 = projectile.Center;
			projectile.scale = 1f - projectile.localAI[0];
			projectile.width = (int)(20f * projectile.scale);
			projectile.height = projectile.width;
			projectile.position.X = center10.X - projectile.width / 2;
			projectile.position.Y = center10.Y - projectile.height / 2;
			if (projectile.localAI[0] < 0.1)
			{
				projectile.localAI[0] += 0.01f;
			}
			else
			{
				projectile.localAI[0] += 0.025f;
			}
			if (projectile.localAI[0] >= 0.95f)
			{
				projectile.Kill();
			}
			projectile.velocity.X = projectile.velocity.X + projectile.ai[0] * 2f;
			projectile.velocity.Y = projectile.velocity.Y + projectile.ai[1] * 2f;
			if (projectile.velocity.Length() > 16f)
			{
				projectile.velocity.Normalize();
				projectile.velocity *= 16f;
			}
			projectile.ai[0] *= 1.05f;
			projectile.ai[1] *= 1.05f;
			if (projectile.scale < 1f)
			{
				int num897 = 0;
				while (num897 < projectile.scale * 10f)
				{
					int num898 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.CarrotDust>(), projectile.velocity.X, projectile.velocity.Y, 100);
					Main.dust[num898].position = (Main.dust[num898].position + projectile.Center) / 2f;
					Main.dust[num898].noGravity = true;
					Main.dust[num898].velocity *= 0.1f;
					Main.dust[num898].velocity -= projectile.velocity * (1.3f - projectile.scale);
					//Main.dust[num898].fadeIn = (float)(100 + projectile.owner);
					Main.dust[num898].scale += projectile.scale * 0.75f;
					num897++;
				}
				return;
			}
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("InfinityOverload"), 300);
        }
    }
}