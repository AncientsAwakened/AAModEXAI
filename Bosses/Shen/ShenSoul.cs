using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria.ID;

using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Shen
{
    public class ShenSoul : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discordian Soul");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.width = 82;
            npc.height = 48;
            npc.HitSound = SoundID.NPCHit52;
			npc.DeathSound = SoundID.NPCDeath55;
            npc.npcSlots = 0;
            npc.value = BaseUtility.CalcValue(0, 0, 0, 0);
            npc.aiStyle = -1;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noGravity = true;
            npc.damage = 250;
            npc.alpha = 255;

        }

        public float[] internalAI = new float[5];

        public float count = 0f;

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
                writer.Write(internalAI[4]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
                internalAI[4] = reader.ReadFloat();
            }
        }
        public override void AI()
        {
            if (internalAI[3] > 0 || (AAGlobalProjectile.AnyProjectiles(mod.ProjectileType("ShenWaveDeathray")) || AAGlobalProjectile.AnyProjectiles(mod.ProjectileType("ShenWaveDeathraySmall"))))
            {
                npc.TargetClosest(false);
                npc.rotation = 0;
                npc.velocity *= .2f;
                NPC shen = Main.npc[(int)internalAI[0]];
                int shotdirection = npc.spriteDirection = npc.direction;
                if(internalAI[4] == 0)
                {
                    npc.spriteDirection = npc.direction = shen.spriteDirection;
                    shotdirection = npc.direction;
                    npc.Center = new Vector2(shen.Center.X + internalAI[1] * shotdirection, shen.Center.Y + internalAI[2]);
                }
                if(shen.ai[0] == 0 && shen.ai[2] == 240 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float ai0 = (float)Math.PI * 2 / 300 * (internalAI[3] == 2 ? 1 : -1) * Math.Sign(internalAI[2]);
                            Projectile.NewProjectile(npc.Center + npc.direction * new Vector2(npc.width / 2, 0), Vector2.UnitX * shotdirection, mod.ProjectileType("ShenWaveDeathraySmall"), npc.damage, 0f, Main.myPlayer, ai0, npc.whoAmI);
                    internalAI[4] = 1;
                    npc.netUpdate = true;
                }
                if(shen.ai[0] == 14 && shen.ai[1] == internalAI[3] * 120 - 30)
                {
                    internalAI[4] = 1;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(npc.Center + npc.direction * new Vector2(npc.width / 2, 0), npc.DirectionTo(Main.player[shen.target].Center), mod.ProjectileType("ShenWaveDeathraySmall"), npc.damage / 4, 0f, Main.myPlayer, 0f, npc.whoAmI);
                    npc.netUpdate = true;
                }
                if(shen.ai[0] != 0 && shen.ai[0] != 1 && shen.ai[0] != 13 && shen.ai[0] != 14)
                {
                    internalAI[3] = 0;
                }
                return;
            }
            internalAI[4] = 0;
            AI86();
            Lighting.AddLight(npc.Center, AAColor.Shen3.R / 255, AAColor.Shen3.G / 255, AAColor.Shen3.B / 255);
            AAAI.AIShadowflameGhost(npc, ref npc.ai, false, 660f, 0.3f, 15f, 0.2f, 8f, 5f, 10f, 0.4f, 0.4f, 0.95f, 5f);
            if (!NPC.AnyNPCs(mod.NPCType("ShenA")))
            {
                npc.life = 0;
                npc.active = false;
            }
            if (npc.alpha != 0)
            {
                for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                {
                    int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("Discord"), 0f, 0f, 100, default, 2f);
                    Main.dust[num935].noGravity = true;
                    Main.dust[num935].noLight = true;
                }
            }
            npc.alpha -= 12;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
        }

        public Color GetGlowAlpha()
        {
            return new Color(200, 0, 50) * (Main.mouseTextColor / 255f);
        }
        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 1.0;
            if (npc.frameCounter > 4.0)
            {
                npc.frame.Y += frameHeight;
                npc.frameCounter = 0.0;
            }
            if (npc.frame.Y / frameHeight >= 6)
            {
                npc.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            if(internalAI[3] == 0 && internalAI[4] == 0)
            {
                if (npc.velocity.X < 0f)
                {
                    npc.direction = -1;
                }
                else
                {
                    npc.direction = 1;
                }
                if (npc.direction == 1)
                {
                    npc.spriteDirection = 1;
                }
                if (npc.direction == -1)
                {
                    npc.spriteDirection = -1;
                }
                npc.rotation = (float)Math.Atan2((double)(npc.velocity.Y * (float)npc.direction), (double)(npc.velocity.X * (float)npc.direction));
            }
            Texture2D glowTex = mod.GetTexture("Bosses/Shen/ShenSoul");
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc, dColor);
            BaseDrawing.DrawTexture(spritebatch, glowTex, 0, npc, GetGlowAlpha());
            BaseDrawing.DrawAfterimage(spritebatch, glowTex, 0, npc, 0.8f, 1f, 4, false, 0f, 0f, Color.White);
            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("DiscordInferno"), 600);
            target.AddBuff(mod.BuffType("Yanked"), 120);
        }

        public void AI86()
        {
            if (npc.alpha > 0)
            {
                npc.alpha -= 30;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
            }
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            int num;
            for (int num1277 = 0; num1277 < 200; num1277 = num + 1)
            {
                if (num1277 != npc.whoAmI && Main.npc[num1277].active && Main.npc[num1277].type == npc.type)
                {
                    Vector2 vector227 = Main.npc[num1277].Center - npc.Center;
                    if (vector227.Length() < 50f)
                    {
                        vector227.Normalize();
                        if (vector227.X == 0f && vector227.Y == 0f)
                        {
                            if (num1277 > npc.whoAmI)
                            {
                                vector227.X = 1f;
                            }
                            else
                            {
                                vector227.X = -1f;
                            }
                        }
                        vector227 *= 0.4f;
                        npc.velocity -= vector227;
                        NPC npc6 = Main.npc[num1277];
                        npc6.velocity += vector227;
                    }
                }
                num = num1277;
            }
            float num1278 = 120f;
            if (npc.localAI[0] < num1278)
            {
                if (npc.localAI[0] == 0f)
                {
                    Main.PlaySound(SoundID.Item8, npc.Center);
                    npc.TargetClosest(true);
                    if (npc.direction > 0)
                    {
                        npc.velocity.X = npc.velocity.X + 2f;
                    }
                    else
                    {
                        npc.velocity.X = npc.velocity.X - 2f;
                    }
                    for (int num1279 = 0; num1279 < 20; num1279 = num + 1)
                    {
                        Vector2 vector228 = npc.Center;
                        vector228.Y -= 18f;
                        Vector2 value35 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                        value35.Normalize();
                        value35 *= (float)Main.rand.Next(0, 100) * 0.1f;
                        vector228 += value35;
                        value35.Normalize();
                        value35 *= (float)Main.rand.Next(50, 90) * 0.2f;
                        int num1280 = Dust.NewDust(vector228, 1, 1, 27, 0f, 0f, 0, default(Color), 1f);
                        Main.dust[num1280].velocity = -value35 * 0.3f;
                        Main.dust[num1280].alpha = 100;
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dust[num1280].noGravity = true;
                            Dust dust3 = Main.dust[num1280];
                            dust3.scale += 0.3f;
                        }
                        num = num1279;
                    }
                }
                npc.localAI[0] += 1f;
                float num1281 = 1f - npc.localAI[0] / num1278;
                float num1282 = num1281 * 20f;
                int num1283 = 0;
                while ((float)num1283 < num1282)
                {
                    if (Main.rand.Next(5) == 0)
                    {
                        int num1284 = Dust.NewDust(npc.position, npc.width, npc.height, 27, 0f, 0f, 0, default(Color), 1f);
                        Main.dust[num1284].alpha = 100;
                        Dust dust3 = Main.dust[num1284];
                        dust3.velocity *= 0.3f;
                        dust3 = Main.dust[num1284];
                        dust3.velocity += npc.velocity * 0.75f;
                        Main.dust[num1284].noGravity = true;
                    }
                    num = num1283;
                    num1283 = num + 1;
                }
            }
            if (npc.ai[0] == 0f)
            {
                npc.TargetClosest(true);
                npc.ai[0] = 1f;
                npc.ai[1] = (float)npc.direction;
            }
            else if (npc.ai[0] == 1f)
            {
                npc.TargetClosest(true);
                float num1289 = 0.3f;
                float num1290 = 7f;
                float num1291 = 4f;
                float num1292 = 660f;
                float num1293 = 4f;
                if (npc.type == 521)
                {
                    num1289 = 0.7f;
                    num1290 = 14f;
                    num1292 = 500f;
                    num1291 = 6f;
                    num1293 = 3f;
                }
                npc.velocity.X = npc.velocity.X + npc.ai[1] * num1289;
                if (npc.velocity.X > num1290)
                {
                    npc.velocity.X = num1290;
                }
                if (npc.velocity.X < -num1290)
                {
                    npc.velocity.X = -num1290;
                }
                float num1294 = Main.player[npc.target].Center.Y - npc.Center.Y;
                if (Math.Abs(num1294) > num1291)
                {
                    num1293 = 15f;
                }
                if (num1294 > num1291)
                {
                    num1294 = num1291;
                }
                else if (num1294 < -num1291)
                {
                    num1294 = -num1291;
                }
                npc.velocity.Y = (npc.velocity.Y * (num1293 - 1f) + num1294) / num1293;
                if ((npc.ai[1] > 0f && Main.player[npc.target].Center.X - npc.Center.X < -num1292) || (npc.ai[1] < 0f && Main.player[npc.target].Center.X - npc.Center.X > num1292))
                {
                    npc.ai[0] = 2f;
                    npc.ai[1] = 0f;
                    if (npc.Center.Y + 20f > Main.player[npc.target].Center.Y)
                    {
                        npc.ai[1] = -1f;
                    }
                    else
                    {
                        npc.ai[1] = 1f;
                    }
                }
            }
            else if (npc.ai[0] == 2f)
            {
                float num1295 = 0.4f;
                float scaleFactor13 = 0.95f;
                float num1296 = 5f;
                if (npc.type == 521)
                {
                    num1295 = 0.3f;
                    num1296 = 7f;
                    scaleFactor13 = 0.9f;
                }
                npc.velocity.Y = npc.velocity.Y + npc.ai[1] * num1295;
                if (npc.velocity.Length() > num1296)
                {
                    npc.velocity *= scaleFactor13;
                }
                if (npc.velocity.X > -1f && npc.velocity.X < 1f)
                {
                    npc.TargetClosest(true);
                    npc.ai[0] = 3f;
                    npc.ai[1] = (float)npc.direction;
                }
            }
            else if (npc.ai[0] == 3f)
            {
                float num1297 = 0.4f;
                float num1298 = 0.2f;
                float num1299 = 5f;
                float scaleFactor14 = 0.95f;
                npc.velocity.X = npc.velocity.X + npc.ai[1] * num1297;
                if (npc.Center.Y > Main.player[npc.target].Center.Y)
                {
                    npc.velocity.Y = npc.velocity.Y - num1298;
                }
                else
                {
                    npc.velocity.Y = npc.velocity.Y + num1298;
                }
                if (npc.velocity.Length() > num1299)
                {
                    npc.velocity *= scaleFactor14;
                }
                if (npc.velocity.Y > -1f && npc.velocity.Y < 1f)
                {
                    npc.TargetClosest(true);
                    npc.ai[0] = 0f;
                    npc.ai[1] = (float)npc.direction;
                }
            }
        }
    }
}
