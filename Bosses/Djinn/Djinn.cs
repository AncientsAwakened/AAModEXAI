
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Djinn
{
    [AutoloadBossHead]
    public class Djinn : ModNPC
    {
        public int damage = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desert Djinn");
            Main.npcFrameCount[npc.type] = 15;
        }

        public override void SetDefaults()
        {
            npc.width = 70;
            npc.height = 80;
            npc.aiStyle = -1;
            npc.damage = 40;
            npc.defense = 15;
            npc.lifeMax = 6000;
            npc.buffImmune[20] = true;
            npc.buffImmune[44] = true;
            npc.value = 50000f;
            npc.HitSound = SoundID.NPCHit23;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.knockBackResist = 0f;
            npc.boss = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Djinn");
            bossBag = ModLoader.GetMod("AAMod").ItemType("DjinnBag");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
            npc.damage = (int)(npc.damage * 1.6f);
            npc.defense = (int)(npc.defense * 1.2f);
        }

        public int runonce = 0;
        public int FrameHeight = 130;

        public float[] internalAI = new float[4];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
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
            }
        }

        bool selectPoint = false;
        Vector2 MovePoint;
        bool soundPlayed = false;

        public override void AI()
        {
            if (Main.expertMode)
            {
                damage = npc.damage / 4;
            }
            else
            {
                damage = npc.damage / 2;
            }
            Player player = Main.player[npc.target];
            if (runonce == 0)
            {
                StartSandstorm();
                runonce += 1;
            }
	        if (internalAI[0] == 2 && npc.ai[3] < 60)
            {
					npc.velocity.X *= 0.97f;
			}
		
            if (internalAI[0] == 2 && npc.ai[3] > 120)
            {
                if (npc.velocity.X > 0)
                {
                    npc.direction = 1;
                }
                else
                {
                    npc.direction = -1;
                }
            }
            else
            {
                if (player.Center.X > npc.Center.X)
                {
                    npc.direction = 1;
                }
                else
                {
                    npc.direction = -1;
                }
            }

            if (player.dead || !player.active)
            {
                npc.TargetClosest(true);
                if (player.dead || !player.active)
                {
                    FuriousFlexing();
                    return;
                }
            }
            else if (!player.ZoneDesert)
            {
                if (!soundPlayed)
                {
                    soundPlayed = true;
                    Main.PlaySound(SoundID.Roar, (int)npc.position.X, (int)npc.position.Y, 0);
                }
                npc.damage = 200 * (Main.expertMode ? (int)(npc.damage * 1.6f) : 1);
                npc.defense = 1000;
                npc.ai[3]++;
                BaseAI.AIFlier(npc, ref npc.ai, true, 0.3f, 0.3f, 16f, 16f, false, 300);

                if (npc.localAI[0]++ > 50)
                {
                    npc.localAI[0] = 0;
                    Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-200, 200), npc.Center.Y + Main.rand.Next(-100, 100), 0, 0, mod.ProjectileType("Menacing"), 0, 0, Main.myPlayer);
                }
                return;
            }
            else
            {
                soundPlayed = false;
                npc.defense = 15;
                Sandstorm.TimeLeft = 10;
                if (npc.alpha <= 0)
                {
                    npc.alpha = 0;
                }
                else
                {
                    npc.alpha -= 2;
                }
            }


            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.damage = 30 * (Main.expertMode ? (int)(npc.damage * 1.6f) : 1); ;
                internalAI[1]++;
                if (internalAI[1] >= 300)
                {
					
                    selectPoint = true; ;
                    internalAI[0] = Main.rand.Next(3);
                    internalAI[1] = 0;
                    npc.ai[3] = 0;
                    npc.ai = new float[4];
                    npc.netUpdate = true;
                }
            }

            if (internalAI[0] == 0)
            {
                npc.damage = 30 * (Main.expertMode ? (int)(npc.damage * 1.6f) : 1); ;
                npc.ai[3]++;
                npc.velocity.X = 0;
                npc.velocity.Y = 0;
                if (npc.ai[3] == 9 || npc.ai[3] == 36 || npc.ai[3] == 72)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient && AAGlobalProjectile.CountProjectiles(658) < 5)
                    {
                        FireProjectile();
                        npc.netUpdate = true;
                    }
                }
                if (npc.ai[3] > 90 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    internalAI[0] = 10;
                    internalAI[1] = 0;
                    npc.ai = new float[4];
                    npc.ai[3] = 0;
                    npc.netUpdate = true;
                }
            }
            else if (internalAI[0] == 1)
            {
                npc.damage = 60 * (Main.expertMode ? (int)(npc.damage * 1.6f) : 1); ;
                npc.ai[3]++;
                BaseAI.AIFlier(npc, ref npc.ai, true, 0.1f, 0.1f, 6f, 6f, false, 300);
                npc.damage = 40;

                if (npc.ai[3] % 30 == 0)
                {
                    Projectile.NewProjectile(npc.position + new Vector2(Main.rand.Next(70), Main.rand.Next(80)), Vector2.Zero, mod.ProjectileType("Menacing"), 0, 0, Main.myPlayer);
                }
                if (npc.ai[3] > 200 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    internalAI[0] = 10;
                    internalAI[1] = 0;
                    npc.ai = new float[4];
                    npc.ai[3] = 0;
                    npc.netUpdate = true;
                }
            }
            else if (internalAI[0] == 2)
            {
                npc.ai[3]++;
				
                npc.damage = 50 * (Main.expertMode ? (int)(npc.damage * 1.6f) : 1); ;
                if (npc.ai[3] < 120 && npc.ai[3] > 60)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (selectPoint)
                        {
							
							 
                            float point = 700 * npc.direction;
                            MovePoint = player.Center + new Vector2(point, 0);
							MoveToPoint(MovePoint, 10f);
                            selectPoint = false;
                            npc.netUpdate = true;
                        }
                        npc.damage = 20 * (Main.expertMode ? (int)(npc.damage * 1.6f) : 1); ;
                        npc.netUpdate = true;
                    }
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (npc.ai[3] == 120)
                        {
							 
                            //float point = 500 * npc.direction;
                            MovePoint = new Vector2(player.Center.X, npc.position.Y);
							MoveToPoint(MovePoint, 10f);
                            npc.netUpdate = true;
                        }
                        npc.damage = 40 * (Main.expertMode ? (int)(npc.damage * 1.6f) : 1); ;
                        npc.netUpdate = true;
                    }
                }

               

                if (npc.ai[3] > 160 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.damage = 30;
                    internalAI[0] = 10;
                    internalAI[1] = 0;
                    npc.ai = new float[4];
                    npc.ai[3] = 0;
                    npc.netUpdate = true;
                }
            }
            else
            {
                npc.damage = 30;
                BaseAI.AIFlier(npc, ref npc.ai, true, 0.1f, 0.04f, 4f, 2f, false, 300);
            }
        }

        int Frame = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (internalAI[0] == 0)
            {
                if (Frame < 6 || Frame > 14)
                {
                    Frame = 6;
                }
                if (npc.ai[3] > 0)
                {
                    Frame = 6;
                }
                if (npc.ai[3] > 9)
                {
                    Frame = 7;
                }
                if (npc.ai[3] > 18)
                {
                    Frame = 8;
                }
                if (npc.ai[3] > 27)
                {
                    Frame = 9;
                }
                if (npc.ai[3] > 36)
                {
                    Frame = 10;
                }
                if (npc.ai[3] > 45)
                {
                    Frame = 11;
                }
                if (npc.ai[3] > 54)
                {
                    Frame = 12;
                }
                if (npc.ai[3] > 63)
                {
                    Frame = 13;
                }
                if (npc.ai[3] > 72)
                {
                    Frame = 14;
                }
                npc.frame.Y = Frame * frameHeight;
                return;
            }
            else if (internalAI[0] == 1 || !Main.player[npc.target].ZoneDesert)
            {
                if (npc.frameCounter > 5)
                {
                    npc.frame.Y += frameHeight;
                    npc.frameCounter = 0;
                }
                if (npc.frame.Y > FrameHeight * 5)
                {
                    npc.frame.Y = 0;
                }
                return;
            }
            else if (internalAI[0] == 2)
            {
                if (npc.ai[3] < 60)
                {
					
                    if (npc.frameCounter > 9)
                    {
                        npc.frame.Y += frameHeight;
                        npc.frameCounter = 0;
                    }
                    if (npc.frame.Y > FrameHeight * 3)
                    {
                        npc.frame.Y = 0;
                    }
                }
                else
                {
					
                    if (npc.frame.Y < FrameHeight * 4)
                    {
                        npc.frame.Y = FrameHeight * 4;
                    }
                    if (npc.frameCounter > 9)
                    {
                        npc.frame.Y += frameHeight;
                        npc.frameCounter = 0;
                    }
			    if (npc.frame.Y > FrameHeight * 7)
                {
					
                     npc.frame.Y = FrameHeight * 5;
                }
                }
               
                return;
            }
            else
            {
                if (npc.frameCounter > 9)
                {
                    npc.frame.Y += 130;
                    npc.frameCounter = 0;
                }
                if (npc.frame.Y > FrameHeight * 5)
                {
                    npc.frame.Y = 0;
                }
                return;
            }
        }

        public void FuriousFlexing()
        {
            npc.velocity.X *= .85f;
            npc.velocity.Y *= .85f;
            npc.alpha += 2;
            if (npc.alpha >= 255)
            {
                npc.active = false;
            }
            if (npc.ai[3] < 300)
            {
                npc.ai[3] = 300;
            }
            if (npc.frameCounter > 5)
            {
                npc.frame.Y += 130;
                npc.frameCounter = 0;
                if (npc.ai[3] > 381)
                {
                    npc.ai[3] = 300;
                }
            }
        }

        public void FireProjectile()
        {
            List<Point> list4 = new List<Point>();
            Vector2 vec5 = Main.player[npc.target].Center + new Vector2(Main.player[npc.target].velocity.X * 30f, 0f);
            Point point14 = vec5.ToTileCoordinates();
            int num1468 = 0;
            while (num1468 < 1000 && list4.Count < 1)
            {
                bool flag118 = false;
                int num1469 = Main.rand.Next(point14.X - 30, point14.X + 30 + 1);
                foreach (Point current in list4)
                {
                    if (Math.Abs(current.X - num1469) < 10)
                    {
                        flag118 = true;
                        break;
                    }
                }
                if (!flag118)
                {
                    int startY = point14.Y - 20;
                    Collision.ExpandVertically(num1469, startY, out int num1470, out int num1471, 1, 51);
                    if (StrayMethods.CanSpawnSandstormHostile(new Vector2(num1469, num1471 - 15) * 16f, 15, 15))
                    {
                        list4.Add(new Point(num1469, num1471 - 15));
                    }
                }
                num1468++;
            }
            foreach (Point current2 in list4)
            {
                Projectile.NewProjectile(current2.X * 16, current2.Y * 16, 0f, 0f, 658, damage, 0f, Main.myPlayer, 0f, 0f);
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            npc.position.X = npc.position.X + npc.width / 2;
            npc.position.Y = npc.position.Y + npc.height / 2;
            npc.position.X = npc.position.X - npc.width / 2;
            npc.position.Y = npc.position.Y - npc.height / 2;
            int dust = ModLoader.GetMod("AAMod").DustType("SandDust");
            for (int Loop = 0; Loop < 5; Loop++)
            {
                int d = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, dust, 0f, 0f, 0);
                Main.dust[d].velocity.Y = hitDirection * 0.1F;
                Main.dust[d].noGravity = false;
            }
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DjinnGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DjinnGore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DjinnGore3"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DjinnGore4"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DjinnGore5"), 1f);
                for (int Loop = 0; Loop < 60; Loop++)
                {
                    int d = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, dust, 0f, 0f, 0);
                    Main.dust[d].velocity.X *= 0f;
                    Main.dust[d].noGravity = false;
                }
            }
        }

        public void MoveToPoint(Vector2 point, float moveSpeed)
        {
            if (moveSpeed == 0f || npc.Center == point) return; //don't move if you have no move speed
            float velMultiplier = 1f;
            Vector2 dist = point - npc.Center;
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
            npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            npc.velocity *= moveSpeed;
            npc.velocity *= velMultiplier;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }


        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
            AAWorld.downedDjinn = true;
        }

        public override void NPCLoot()
        {
            Sandstorm.TimeLeft = 0;
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("DjinnTrophy"));
            }
            if (!Main.expertMode)
            {
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("DjinnMask"));
                }
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("DesertMana"), 10, 15);
                string[] lootTable = { "Djinnerang", "SandLamp", "SandScepter", "SandstormCrossbow", "SultanScimitar" };
                int loot = Main.rand.Next(lootTable.Length);
                npc.DropLoot(AAMod.Items.Vanity.Mask.DjinnMask.type, 1f / 7);
                if (Main.rand.Next(6) == 0)
                {
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("Sandagger"), 90, 120);
                }
                else
                {
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType(lootTable[loot]));
                }
            }
            else
            {
                for(int i = 0; i < 10; i++) npc.DropBossBags();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D CurrentTex;
            Texture2D texture = Main.npcTexture[npc.type];
            Texture2D MudaMuda = mod.GetTexture("Bosses/Djinn/DesertDjinnMudaMuda");
            Texture2D Punch = mod.GetTexture("Bosses/Djinn/DesertDjinnPunch");

            if (internalAI[0] == 1 || !Main.player[npc.target].ZoneDesert)
            {
                CurrentTex = MudaMuda;
            }
            else if (internalAI[0] == 2)
            {
                CurrentTex = Punch;
            }
            else
            {
                CurrentTex = texture;
            }

            var effects = npc.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            if (!Main.player[npc.target].ZoneDesert)
            {
                drawColor = Color.Goldenrod;
                BaseDrawing.DrawAfterimage(spriteBatch, CurrentTex, 0, npc, 1, 1, 7, false, 0, 0, drawColor, npc.frame, 15);
            }

            spriteBatch.Draw(CurrentTex, npc.Center - Main.screenPosition, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);

            return false;
        }

        private static void StartSandstorm()
        {
            Sandstorm.Happening = true;
            Sandstorm.TimeLeft = (int)(3600f * (8f + Main.rand.NextFloat() * 16f));
            ChangeSeverityIntentions();
        }

        private static void ChangeSeverityIntentions()
        {
            if (Sandstorm.Happening)
            {
                Sandstorm.IntendedSeverity = 0.4f + Main.rand.NextFloat();
            }
            else if (Main.rand.Next(3) == 0)
            {
                Sandstorm.IntendedSeverity = 0f;
            }
            else
            {
                Sandstorm.IntendedSeverity = Main.rand.NextFloat() * 0.3f;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
            }
        }
    }
}
