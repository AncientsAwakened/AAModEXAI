
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    public class ForsakenSun : ModNPC
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forsaken Sun");
        }

		public override void SetDefaults()
		{
            npc.width = 32;
            npc.height = 32;
            npc.aiStyle = -1;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.damage = 50;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle SunFrame = new Rectangle(0, 0, 64, 64);
            BaseDrawing.DrawTexture(spriteBatch, mod.GetTexture("Bosses/Anubis/Forsaken/ForsakenSun1"), 0, npc.position + new Vector2(0, npc.gfxOffY), npc.width, npc.height, npc.scale, -npc.rotation, npc.spriteDirection, 1, SunFrame, npc.GetAlpha(AAColor.COLOR_WHITEFADE1), true);
            BaseDrawing.DrawTexture(spriteBatch, mod.GetTexture("Bosses/Anubis/Forsaken/ForsakenSun"), 0, npc.position + new Vector2(0, npc.gfxOffY), npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 1, SunFrame, npc.GetAlpha(AAColor.COLOR_WHITEFADE1), true);
            return false;
        }

        public override void AI()
        {
            npc.TargetClosest();

            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
            else
            {
                npc.alpha -= 5;
            }
            
            npc.velocity = Vector2.Zero;
            npc.rotation -= npc.direction * 6.28318548f / 120f;
            npc.scale = npc.Opacity;
            Lighting.AddLight(npc.Center, new Vector3(0.9f, 0.6f, 0f) * npc.Opacity);
            if (Main.rand.Next(2) == 0)
            {
                Vector2 vector135 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                Dust dust31 = Main.dust[Dust.NewDust(npc.Center - vector135 * 30f, 0, 0, ModContent.DustType<AkumaDust>(), 0f, 0f, 0, default, 1f)];
                dust31.noGravity = true;
                dust31.position = npc.Center - vector135 * Main.rand.Next(10, 21);
                dust31.velocity = vector135.RotatedBy(1.5707963705062866, default) * 6f;
                dust31.scale = 0.5f + Main.rand.NextFloat();
                dust31.fadeIn = 0.5f;
                dust31.customData = npc.Center;
            }
            if (Main.rand.Next(2) == 0)
            {
                Vector2 vector136 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                Dust dust32 = Main.dust[Dust.NewDust(npc.Center - vector136 * 30f, 0, 0, ModContent.DustType<AkumaDust>(), 0f, 0f, 0, default, 1f)];
                dust32.noGravity = true;
                dust32.position = npc.Center - vector136 * 30f;
                dust32.velocity = vector136.RotatedBy(-1.5707963705062866, default) * 3f;
                dust32.scale = 0.5f + Main.rand.NextFloat();
                dust32.fadeIn = 0.5f;
                dust32.customData = npc.Center;
            }
            if (npc.ai[0] < 0f)
            {
                Vector2 center15 = npc.Center;
                int num1059 = Dust.NewDust(center15 - Vector2.One * 8f, 16, 16, ModContent.DustType<AkumaDust>(), npc.velocity.X / 2f, npc.velocity.Y / 2f, 0);
                Main.dust[num1059].velocity *= 2f;
                Main.dust[num1059].noGravity = true;
                Main.dust[num1059].scale = Utils.SelectRandom(Main.rand, new float[]
                {
                    0.8f,
                    1.65f
                });
                Main.dust[num1059].customData = this;
            }

            npc.ai[1]++;

            if (npc.ai[1] == 120)
            {
                Projectile.NewProjectile(npc.Center, new Vector2(12, 12), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-12, 12), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(12, -12), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-12, -12), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[1] == 240)
            {
                Projectile.NewProjectile(npc.Center, new Vector2(12, 0), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-12, 0), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(0, 12), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(0, -12), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[1] == 360)
            {
                Projectile.NewProjectile(npc.Center, new Vector2(12, 12), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-12, 12), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(12, -12), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-12, -12), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[1] == 480)
            {
                Projectile.NewProjectile(npc.Center, new Vector2(12, 0), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-12, 0), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(0, 12), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(0, -12), mod.ProjectileType("ForsakenBlast"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }

            if (npc.ai[1] > 600)
            {
                Main.PlaySound(SoundID.Item14, npc.position);
                Vector2 position = npc.Center + (Vector2.One * -20f);
                int num84 = 40;
                int height3 = num84;
                for (int num85 = 0; num85 < 3; num85++)
                {
                    int num86 = Dust.NewDust(position, num84, height3, 240, 0f, 0f, 100, default, 1.5f);
                    Main.dust[num86].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                }
                for (int num87 = 0; num87 < 15; num87++)
                {
                    int num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<ForsakenDust>(), 0f, 0f, 200, default, 3.7f);
                    Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                    Main.dust[num88].noGravity = true;
                    Main.dust[num88].noLight = true;
                    Main.dust[num88].velocity *= 3f;
                    Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                    num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<ForsakenDust>(), 0f, 0f, 100, default, 1.5f);
                    Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                    Main.dust[num88].velocity *= 2f;
                    Main.dust[num88].noGravity = true;
                    Main.dust[num88].fadeIn = 1f;
                    Main.dust[num88].color = Color.Crimson * 0.5f;
                    Main.dust[num88].noLight = true;
                    Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * 8f;
                }
                for (int num89 = 0; num89 < 10; num89++)
                {
                    int num90 = Dust.NewDust(position, num84, height3, ModContent.DustType<ForsakenDust>(), 0f, 0f, 0, default, 2.7f);
                    Main.dust[num90].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                    Main.dust[num90].noGravity = true;
                    Main.dust[num90].noLight = true;
                    Main.dust[num90].velocity *= 3f;
                    Main.dust[num90].velocity += npc.DirectionTo(Main.dust[num90].position) * 2f;
                }
                for (int num91 = 0; num91 < 30; num91++)
                {
                    int num92 = Dust.NewDust(position, num84, height3, ModContent.DustType<ForsakenDust>(), 0f, 0f, 0, default, 1.5f);
                    Main.dust[num92].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                    Main.dust[num92].noGravity = true;
                    Main.dust[num92].velocity *= 3f;
                    Main.dust[num92].velocity += npc.DirectionTo(Main.dust[num92].position) * 3f;
                }
                npc.active = false;
                npc.netUpdate = true;
            }
        }
    }
}
