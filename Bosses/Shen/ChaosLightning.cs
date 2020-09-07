using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.Graphics.Shaders;

using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Shen
{
    public class ChaosLightning : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Lightning");
		}

		public int DustShader = 0;

		public Color OuterColor = Color.Magenta;

		public Color InnerColor = Color.White;
    	
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 4;
            projectile.timeLeft = 120 * (projectile.extraUpdates + 1);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                projHitbox.X = (int)projectile.oldPos[i].X;
                projHitbox.Y = (int)projectile.oldPos[i].Y;
                if (projHitbox.Intersects(targetHitbox))
                {
                    return true;
                }
            }
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.localAI[1] < 1f)
            {
                projectile.localAI[1] += 2f;
                projectile.position += projectile.velocity;
                projectile.velocity = Vector2.Zero;
            }
            return false;
        }

        public override void AI()
		{
			Lighting.AddLight(projectile.Center, OuterColor.R / 255, OuterColor.G / 255, OuterColor.B / 255);

			if (projectile.localAI[1] == 0f && projectile.ai[0] >= 900f)
			{
				projectile.ai[0] -= 1000f;
				projectile.localAI[1] = -1f;
			}
			projectile.frameCounter++;
			Lighting.AddLight(projectile.Center, 0.3f, 0.45f, 0.5f);
			if (projectile.velocity == Vector2.Zero)
			{
				if (projectile.frameCounter >= projectile.extraUpdates * 2)
				{
					projectile.frameCounter = 0;
					bool flag32 = true;
					for (int num751 = 1; num751 < projectile.oldPos.Length; num751++)
					{
						if (projectile.oldPos[num751] != projectile.oldPos[0])
						{
							flag32 = false;
						}
					}
					if (flag32)
					{
						projectile.Kill();
						return;
					}
				}
				if (Main.rand.Next(projectile.extraUpdates) == 0 && (projectile.velocity != Vector2.Zero || Main.rand.Next((projectile.localAI[1] == 2f) ? 2 : 6) == 0))
				{
					for (int num752 = 0; num752 < 2; num752++)
					{
						float num753 = projectile.rotation + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
						float num754 = (float)Main.rand.NextDouble() * 0.8f + 1f;
						Vector2 vector56 = new Vector2((float)Math.Cos(num753) * num754, (float)Math.Sin(num753) * num754);
						int num755 = Dust.NewDust(projectile.Center, 30, 30, 226, vector56.X, vector56.Y, 0, new Color(255, 255, 255), 1f);
						if (DustShader != 0)
						{
							Main.dust[num755].shader = GameShaders.Armor.GetSecondaryShader(DustShader, Main.LocalPlayer);
						}
						Main.dust[num755].noGravity = true;
						Main.dust[num755].scale = 1.2f;
					}
					if (Main.rand.Next(5) == 0)
					{
						Vector2 value43 = projectile.velocity.RotatedBy(1.5707963705062866) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
						int num756 = Dust.NewDust(projectile.Center + value43 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default, 1.5f);
						Dust dust = Main.dust[num756];
						dust.velocity *= 0.5f;
						Main.dust[num756].velocity.Y = 0f - Math.Abs(Main.dust[num756].velocity.Y);
					}
				}
			}
			else
			{
				if (projectile.frameCounter < projectile.extraUpdates * 2)
				{
					return;
				}
				projectile.frameCounter = 0;
				float num757 = projectile.velocity.Length();
				UnifiedRandom unifiedRandom2 = new UnifiedRandom((int)projectile.ai[1]);
				int num758 = 0;
				Vector2 spinningpoint15 = -Vector2.UnitY;
				while (true)
				{
					int num759 = unifiedRandom2.Next();
					projectile.ai[1] = num759;
					num759 %= 100;
					float f2 = num759 / 100f * ((float)Math.PI * 2f);
					Vector2 vector57 = f2.ToRotationVector2();
					if (vector57.Y > 0f)
					{
						vector57.Y *= -1f;
					}
					bool flag33 = false;
					if (vector57.Y > -0.02f)
					{
						flag33 = true;
					}
					if (vector57.X * (projectile.extraUpdates + 1) * 2f * num757 + projectile.localAI[0] > 40f)
					{
						flag33 = true;
					}
					if (vector57.X * (projectile.extraUpdates + 1) * 2f * num757 + projectile.localAI[0] < -40f)
					{
						flag33 = true;
					}
					if (flag33)
					{
						if (num758++ >= 100)
						{
							projectile.velocity = Vector2.Zero;
							if (projectile.localAI[1] < 1f)
							{
								projectile.localAI[1] += 2f;
							}
							break;
						}
						continue;
					}
					spinningpoint15 = vector57;
					break;
				}
				if (!(projectile.velocity != Vector2.Zero))
				{
					return;
				}
				projectile.localAI[0] += spinningpoint15.X * (projectile.extraUpdates + 1) * 2f * num757;
				projectile.velocity = spinningpoint15.RotatedBy(projectile.ai[0] + (float)Math.PI / 2f) * num757;
				projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
				if (Main.rand.Next(4) == 0 && Main.netMode != NetmodeID.MultiplayerClient && projectile.localAI[1] == 0f)
				{
					float num760 = Main.rand.Next(-3, 4) * ((float)Math.PI / 3f) / 3f;
					Vector2 vector58 = projectile.ai[0].ToRotationVector2().RotatedBy(num760) * projectile.velocity.Length();
					if (!Collision.CanHitLine(projectile.Center, 0, 0, projectile.Center + vector58 * 50f, 0, 0))
					{
						Projectile.NewProjectile(projectile.Center.X - vector58.X, projectile.Center.Y - vector58.Y, vector58.X, vector58.Y, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, vector58.ToRotation() + 1000f, projectile.ai[1]);
					}
				}
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Color color25 = Lighting.GetColor((int)(projectile.position.X + projectile.width * 0.5) / 16, (int)((projectile.position.Y + projectile.height * 0.5) / 16.0));
            Vector2 end = projectile.position + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            Texture2D tex3 = Main.extraTexture[33];
            projectile.GetAlpha(color25);
            Vector2 scale16 = new Vector2(projectile.scale) / 2f;
            for (int num291 = 0; num291 < 3; num291++)
            {
                if (num291 == 0)
                {
                    scale16 = new Vector2(projectile.scale) * 0.6f;
                    DelegateMethods.c_1 = Color.DarkMagenta * 0.5f;
                }
                else if (num291 == 1)
                {
                    scale16 = new Vector2(projectile.scale) * 0.4f;
                    DelegateMethods.c_1 = Color.Magenta * 0.5f;
                }
                else
                {
                    scale16 = new Vector2(projectile.scale) * 0.2f;
                    DelegateMethods.c_1 = new Color(255, 255, 255, 0) * 0.5f;
                }
                DelegateMethods.f_1 = 1f;
                for (int num292 = projectile.oldPos.Length - 1; num292 > 0; num292--)
                {
                    if (!(projectile.oldPos[num292] == Vector2.Zero))
                    {
                        Vector2 start = projectile.oldPos[num292] + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
                        Vector2 end2 = projectile.oldPos[num292 - 1] + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
                        Utils.DrawLaser(Main.spriteBatch, tex3, start, end2, scale16, new Utils.LaserLineFraming(DelegateMethods.LightningLaserDraw));
                    }
                }
                if (projectile.oldPos[0] != Vector2.Zero)
                {
                    DelegateMethods.f_1 = 1f;
                    Vector2 start2 = projectile.oldPos[0] + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
                    Utils.DrawLaser(Main.spriteBatch, tex3, start2, end, scale16, new Utils.LaserLineFraming(DelegateMethods.LightningLaserDraw));
                }
            }
            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DiscordInferno"), 180);
        }
    }
}
 