using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using AAMod;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
	public class HorusSentry : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 60;
            npc.height = 48;
            npc.value = BaseUtility.CalcValue(0, 0, 0, 0);
            npc.npcSlots = 1;
            npc.aiStyle = -1;
            npc.lifeMax = 1200;
            npc.defense = 30;
            npc.damage = 60;
            npc.HitSound = SoundID.NPCHit49;
            npc.DeathSound = SoundID.NPCDeath51;
            npc.knockBackResist = 0.7f;
            npc.alpha = 255;
            npc.noGravity = true;
        }

        public override void AI()
        {
            npc.TargetClosest();
            Player player = Main.player[npc.target];
            int FrameHeight = 48;
            if (npc.localAI[0] == 0f)
            {
                Main.PlaySound(SoundID.Item121, npc.position);
                npc.localAI[0] = 1f;
            }

            if (npc.alpha <= 0)
            {
                npc.alpha = 0;
            }
            else
            {
                npc.alpha -= 5;
            }

            npc.ai[0] += 1f;

            if (!NPC.AnyNPCs(mod.NPCType("ForsakenAnubis")))
            {
                npc.StrikeNPCNoInteraction(99999, 0, 0, false);
            }

            if (npc.ai[0] == 160 || Vector2.Distance(npc.Center, player.Center) > 3000)
            {
                Main.PlaySound(SoundID.Item121, npc.position);
                Teleport();
            }

            if (npc.ai[0] < 180)
            {
                npc.frame.Y = 0;
            }
            if (npc.ai[0] > 180)
            {
                npc.frame.Y = FrameHeight;
            }
            if (npc.ai[0] > 220)
            {
                npc.frame.Y = FrameHeight * 2;
            }
            if (npc.ai[0] > 260)
            {
                npc.frame.Y = FrameHeight * 3;
            }
            if (npc.ai[0] > 300)
            {
                npc.frame.Y = FrameHeight * 3;
            }
            if (npc.ai[0] >= 340 && npc.ai[0] <= 460)
            {
                BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, mod.ProjectileType("CurseFlame"), ref npc.ai[1], 20, npc.damage / 2, 10, false);
            }
            if (npc.ai[0] > 460)
            {
                npc.frame.Y = FrameHeight * 2;
            }
            if (npc.ai[0] > 500)
            {
                npc.frame.Y = FrameHeight * 1;
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color drawColor)
        {
            BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 7, npc.frame, npc.GetAlpha(ColorUtils.COLOR_GLOWPULSE), true);
            return false;
        }

        public void Teleport()
        {
            Player player = Main.player[npc.target];
            Vector2 targetPos = player.Center;
            int posX = Main.rand.Next(3);
            switch (posX)
            {
                case 0:
                    posX = -300;
                    break;
                case 1:
                    posX = 0;
                    break;
                case 2:
                    posX = 300;
                    break;
            }
            int posY = Main.rand.Next(posX == 0 ? 1 : 2);
            switch (posY)
            {
                case 0:
                    posY = -300;
                    break;
                case 1:
                    posY = 0;
                    break;
            }

            npc.position = new Vector2(targetPos.X + posX, targetPos.Y + posY);

            Vector2 position = npc.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("ForsakenDust"), 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 15; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("ForsakenDust"), 0f, 0f, 50, default, 3.7f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("ForsakenDust"), 0f, 0f, 25, default, 1.5f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Black * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * 8f;
            }
            for (int num89 = 0; num89 < 10; num89++)
            {
                int num90 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("ForsakenDust"), 0f, 0f, 0, default, 2.7f);
                Main.dust[num90].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += npc.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 30; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("ForsakenDust"), 0f, 0f, 0, default, 1.5f);
                Main.dust[num92].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += npc.DirectionTo(Main.dust[num92].position) * 3f;
            }
        }

        public override void NPCLoot()
        {
            Vector2 position = npc.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("ForsakenDust"), 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 15; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("ForsakenDust"), 0f, 0f, 50, default, 3.7f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("ForsakenDust"), 0f, 0f, 25, default, 1.5f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Black * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * 8f;
            }
            for (int num89 = 0; num89 < 10; num89++)
            {
                int num90 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("ForsakenDust"), 0f, 0f, 0, default, 2.7f);
                Main.dust[num90].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += npc.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 30; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("ForsakenDust"), 0f, 0f, 0, default, 1.5f);
                Main.dust[num92].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += npc.DirectionTo(Main.dust[num92].position) * 3f;
            }
        }
    }

}