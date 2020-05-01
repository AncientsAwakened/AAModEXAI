
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Equinox
{
    public class NCCloud : ModNPC
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightclawer Cloud");
             Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 46;
            npc.height = 46;
            npc.friendly = false;
            npc.damage = 80;
            npc.lifeMax = 1500;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.timeLeft = 10;
            npc.alpha = 255;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(body);
                writer.Write(rotValue);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                body = reader.ReadInt();
                rotValue = reader.ReadFloat();
            }
        }

        public int body = -1;
        public float rotValue = -1f;
        public override void AI()
        {
            if (npc.frameCounter++ > 5)
            {
                npc.frameCounter = 0;
                npc.frame.Y += 46;
                if (npc.frame.Y >= 46 * 4)
                {
                    npc.frame.Y = 0;
                }
            }

            if (npc.alpha > 0)
            {
                npc.alpha -= 10;
            }
            else
            {
                npc.alpha = 0;
            }

            if(npc.alpha == 205)
            {
                SpawnDust();
            }
            npc.noGravity = true;
            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(npc.Center, mod.NPCType("NightcrawlerHead"), 120f, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;

            NPC NC = Main.npc[body];
            if (NC == null || NC.life <= 0 || !NC.active || NC.type != mod.NPCType("NightcrawlerHead")) { npc.active = false; return; }

            for (int m = npc.oldPos.Length - 1; m > 0; m--)
            {
                npc.oldPos[m] = npc.oldPos[m - 1];
            }
            npc.oldPos[0] = npc.position;

            if (rotValue == -1f) rotValue = npc.ai[3];
            rotValue += 0.05f;
            while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;
            npc.Center = BaseUtility.RotateVector(NC.position, NC.position + new Vector2(140f, 0f), rotValue);

            int aiTimerFire = 0;

            npc.ai[1]++;

            if (npc.ai[3] == 1 || npc.ai[3] == 4 || npc.ai[3] == 7 || npc.ai[3] == 10)
            {
                aiTimerFire = 50;
            }
            if (npc.ai[3] == 2 || npc.ai[3] == 5 || npc.ai[3] == 8 || npc.ai[3] == 11)
            {
                aiTimerFire = 100;
            }
            if (npc.ai[3] == 3 || npc.ai[3] == 6 || npc.ai[3] == 9 || npc.ai[3] == 12)
            {
                aiTimerFire = 150;
            }

            if (npc.ai[1] >= 150)
            {
                npc.ai[1] = 0;
            }

            if (npc.ai[1] == aiTimerFire && Main.netMode != 1)
            {
                Vector2 speed = new Vector2(1f, 0f).RotatedBy((float)(Main.rand.NextDouble() * 3.1415f)) * 6f;
                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, speed.X, speed.Y, mod.ProjectileType("NightcrawlerNothing"), npc.damage / 4, 0, Main.myPlayer);
            }

            if (Main.dayTime)
            {
                npc.active = false;
                npc.NPCLoot();
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc, npc.GetAlpha(ColorUtils.COLOR_GLOWPULSE), true);
            return false;
        }

        public override void NPCLoot()
        {
            SpawnDust();
            npc.active = false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(163, 60);
        }

        public void SpawnDust()
        {
            Vector2 position = npc.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("NightcrawlerDust"), 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 7; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("NightcrawlerDust"), 0, 0, 100, new Color(), 2f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("NightcrawlerDust"), 0, 0, 100, new Color(), 2f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Black * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * 8f;
            }
        }
    }
}