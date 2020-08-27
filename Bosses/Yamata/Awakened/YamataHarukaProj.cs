using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
    public class YamataHarukaProj : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haruka Yamata");
            Main.projFrames[projectile.type] = 11;
		}

        public override void SetDefaults()
        {
            projectile.width = 82;
            projectile.height = 74;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 480;
            projectile.penetrate = -1;
            projectile.aiStyle = -1;
        }

        const float dashTime = 90;
        public override void AI()
        {
            if (!NPC.AnyNPCs(mod.NPCType("YamataA")) || NPC.AnyNPCs(mod.NPCType("HarukaY")))
            {
                projectile.Kill();
                return;
            }
            int ai0 = (int)projectile.ai[0];
            if (ai0 < 0 || ai0 >= Main.maxPlayers)
            {
                projectile.Kill();
                return;
            }

            Player player = Main.player[ai0];
            if (projectile.Center.X > player.Center.X) projectile.direction = 1;
            else projectile.direction = -1;
            projectile.spriteDirection = projectile.direction;
            if (++projectile.ai[1] <= dashTime) //move beside player
            {
                Vector2 target = player.Center;
                target.X += projectile.Center.X < player.Center.X ? -400 : 400;
                MoveToPoint(target);
                if (projectile.ai[1] == dashTime) //dash
                {
                    projectile.velocity = projectile.DirectionTo(player.Center) * 22;
                    projectile.netUpdate = true;
                }
            }
            else
            {
                projectile.velocity *= 0.98f;
                if (projectile.ai[1] > dashTime + 60)
                {
                    projectile.ai[1] = 0;
                    projectile.netUpdate = true;
                }
            }
        }

        public override void PostAI()
        {
            if (projectile.frameCounter++ > 5)
            {
              projectile.frameCounter = 0;
              projectile.frame++;
            }

            if (projectile.ai[1] <= dashTime)
            {
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            else
            {
                if (projectile.frame < 4)
                {
                    projectile.frame = 4;
                }
                if (projectile.frame >= 11)
                {
                    projectile.frame = 7;
                }
            }
        }

        public void MoveToPoint(Vector2 point)
        {
            float moveSpeed = 16f;
            if (Vector2.Distance(projectile.Center, point) > 500)
                moveSpeed = 25f;
            float velMultiplier = 1f;
            Vector2 dist = point - projectile.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < moveSpeed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
            }
            if (length < 200f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 100f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 50f)
            {
                moveSpeed *= 0.5f;
            }
            projectile.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            projectile.velocity *= moveSpeed;
            projectile.velocity *= velMultiplier;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, projectile.position);
            Vector2 position = projectile.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, 240, 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 15; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("AbyssDust"), 0f, 0f, 200, default, 3.7f);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("YamataDust"), 0f, 0f, 100, default, 1.5f);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Crimson * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * 8f;
            }
            for (int num89 = 0; num89 < 10; num89++)
            {
                int num90 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("AbyssDust"), 0f, 0f, 0, default, 2.7f);
                Main.dust[num90].position = projectile.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(projectile.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += projectile.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 30; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("YamataDust"), 0f, 0f, 0, default, 1.5f);
                Main.dust[num92].position = projectile.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(projectile.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += projectile.DirectionTo(Main.dust[num92].position) * 3f;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("HydraToxin"), 300);
        }
    }
}
