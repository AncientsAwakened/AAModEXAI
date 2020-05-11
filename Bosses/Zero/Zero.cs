using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod;

namespace AAModEXAI.Bosses.Zero
{
    [AutoloadBossHead]
    public class Zero : ModNPC
    {
        public int damage = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zero");
            Main.npcFrameCount[npc.type] = 7;
        }

        public override void SetDefaults()
        {
            npc.damage = 59;
			npc.defense = 200;
			npc.lifeMax = 600000;
            if (Main.expertMode)
            {
                npc.value = 0;
            }
            else
            {
                npc.value = Item.sellPrice(0, 30, 0, 0);
            }
            npc.width = 206;
            npc.height = 208;
            npc.aiStyle = -1;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCHit4;
            npc.noGravity = true;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Zero");
            npc.noTileCollide = true;
            npc.knockBackResist = -1f;
            npc.boss = true;
            npc.friendly = false;
            npc.npcSlots = 100;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            npc.lavaImmune = true;
            npc.netAlways = true;
            musicPriority = MusicPriority.BossHigh;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * .7f);
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= (int)(npc.lifeMax * .66f) && !RespawnArms1 && Main.netMode != 1)
            {
                WeaponCount += 1;
                npc.ai[1] = 0;
                RespawnArms1 = true;

                RespawnArms();
                if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroBoss10"), Color.Red, false);
                npc.netUpdate = true;
            }
            if (npc.life <= (int)(npc.lifeMax * .33f) && !RespawnArms2 && Main.netMode != 1)
            {
                WeaponCount += 1;
                npc.ai[1] = 0;
                RespawnArms2 = true;
                RespawnArms();
                if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroBoss10"), Color.Red, false);
                npc.netUpdate = true;
            }

            if (npc.life <= 0 && npc.type == mod.NPCType("Zero"))
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZeroGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZeroGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZeroGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZeroGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZeroGore"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZeroGore3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZeroGore3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZeroGore3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ZeroGore3"), 1f);
                if (!Main.expertMode)
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroBoss2"), Color.Red.R, Color.Red.G, Color.Red.B);
                }
            }
        }

        bool hasArms = false;
        public void RespawnArms()
        {
            hasArms = NPC.AnyNPCs(mod.NPCType("VoidStar")) ||
                   NPC.AnyNPCs(mod.NPCType("Taser")) ||
                   NPC.AnyNPCs(mod.NPCType("RealityCannon")) ||
                   NPC.AnyNPCs(mod.NPCType("RiftShredder")) ||
                   NPC.AnyNPCs(mod.NPCType("Neutralizer")) ||
                   NPC.AnyNPCs(mod.NPCType("OmegaVolley")) ||
                   NPC.AnyNPCs(mod.NPCType("NovaFocus")) ||
                   NPC.AnyNPCs(mod.NPCType("GenocideCannon"));

            if (Main.netMode != 1 && !hasArms)
            {
                npc.ai[0] = 10f;

                for (int m = 0; m < WeaponCount; m++)
                {
                    int npcID = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType(ArmChoice()), 0, m);
                    Main.npc[npcID].Center = npc.Center;
                    Main.npc[npcID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
                    Main.npc[npcID].velocity *= 8f;
                    Main.npc[npcID].netUpdate2 = true; Main.npc[npcID].netUpdate = true;
                }

                internalAI[3] = 1;
                Distance = 0;
                npc.netUpdate = true;
            }
        }

        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("ApocalyptitePlate"), 2, 4);

                if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroBoss1"), Color.Red.R, Color.Red.G, Color.Red.B);
                if (AAWorld.downedZero)
                {
                    int z = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("ZeroProtocol"), 0, 0, 0, 0, 0, npc.target);
                    Main.npc[z].Center = npc.Center;

                    int b = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModLoader.GetMod("AAMod").ProjectileType("ShockwaveBoom"), 0, 1, Main.myPlayer, 0, 0);
                    Main.projectile[b].Center = npc.Center;
                }
                else
                {
                    int z = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModLoader.GetMod("AAMod").NPCType("ZeroTransition"), 0, 0, 0, 0, 0, npc.target);
                    Main.npc[z].Center = npc.Center;
                }

                npc.netUpdate = true;
            }
            else
            {
                if (!AAWorld.downedZero)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("ZeroBoss3"), Color.PaleVioletRed);
                }
                AAWorld.downedZero = true;
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("ApocalyptitePlate"), 2, 4);
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("UnstableSingularity"), 25, 35);
                string[] lootTable =
                {
                    "Battery",
                    "ZeroArrow",
                    "Vortex",
                    "EventHorizon",
                    "RealityCannon",
                    "RiftShredder",
                    "VoidStar",
                    "TeslaHand",
                    "ZeroStar",
                    "Neutralizer",
                    "ZeroTerratool",
                    "DoomPortal",
                    "Gigataser",
                    "OmegaVolley",
                    "GenocideCannon"
                };
                int loot = Main.rand.Next(lootTable.Length);
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType(lootTable[loot]));
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("ZeroCore"), 1f / 10f);
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("ZeroMask"), 1f / 7f);
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("ZeroTrophy"), 1f / 10f);
                if (Main.rand.Next(50) == 0 && AAWorld.downedAllAncients)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("RealityStone"));
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            if (!Main.expertMode)
            {
                potionType = ItemID.SuperHealingPotion;   //boss drops
            }
            else
            {
                potionType = 0;
            }
        }

        public Color GetGlowAlpha()
        {
            return AAColor.ZeroShield * (Main.mouseTextColor / 255f);
        }

        public int frameCounters;
        public int normalFrame;
        public int switchOneFrame;
        public int openFrame;
        public int switchTwoFrame;

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D glowTex = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/Zero_Glow");
            Texture2D Shield = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Zero/ZeroShield");
            Texture2D Ring = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Zero/ZeroShieldRing");
            Texture2D RingGlow = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/ZeroShieldRing_Glow");
            Texture2D normalAni = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Zero/Zer01");
            Texture2D normalGlow = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Zero/Zer01_Glow");
            Texture2D switchOneAni = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Zero/Zer01to2");
            Texture2D switchOneGlow = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Zero/Zer01to2_Glow");
            Texture2D openAni = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Zero/Zer02");
            Texture2D openGlow = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Zero/Zer02_Glow");
            Texture2D switchTwoAni = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Zero/Zer02to1");
            Texture2D switchTwoGlow = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Zero/Zer02to1_Glow");
            Vector2 drawCenter = new Vector2(npc.Center.X, npc.Center.Y);
            if (npc.ai[1] == 0)
            {
                BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc, dColor);
                BaseDrawing.DrawTexture(spritebatch, glowTex, 0, npc, AAColor.COLOR_WHITEFADE1);
            }
            else if (npc.ai[1] == 1)
            {
                int num214 = normalAni.Height / 5;
                int y6 = num214 * normalFrame;
                Main.spriteBatch.Draw(normalAni, drawCenter - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, normalAni.Width, num214)), dColor * ((255 - npc.alpha) / 255f), npc.rotation, new Vector2(normalAni.Width / 2f, num214 / 2f), npc.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(normalGlow, drawCenter - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, normalAni.Width, num214)), AAColor.COLOR_WHITEFADE1, npc.rotation, new Vector2(normalAni.Width / 2f, num214 / 2f), npc.scale, npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            else if (npc.ai[1] == 2)
            {
                int num214 = switchOneAni.Height / 5;
                int y6 = num214 * switchOneFrame;
                Main.spriteBatch.Draw(switchOneAni, drawCenter - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, switchOneAni.Width, num214)), dColor * ((255 - npc.alpha) / 255f), npc.rotation, new Vector2(switchOneAni.Width / 2f, num214 / 2f), npc.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(switchOneGlow, drawCenter - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, switchOneAni.Width, num214)), AAColor.COLOR_WHITEFADE1, npc.rotation, new Vector2(switchOneAni.Width / 2f, num214 / 2f), npc.scale, npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            else if (npc.ai[1] == 3)
            {
                int num214 = openAni.Height / 5;
                int y6 = num214 * openFrame;
                Main.spriteBatch.Draw(openAni, drawCenter - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, openAni.Width, num214)), dColor * ((255 - npc.alpha) / 255f), npc.rotation, new Vector2(openAni.Width / 2f, num214 / 2f), npc.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(openGlow, drawCenter - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, openAni.Width, num214)), AAColor.COLOR_WHITEFADE1, npc.rotation, new Vector2(openAni.Width / 2f, num214 / 2f), npc.scale, npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            else if (npc.ai[1] == 4)
            {
                int num214 = switchTwoAni.Height / 5;
                int y6 = num214 * switchTwoFrame;
                Main.spriteBatch.Draw(switchTwoAni, drawCenter - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, switchTwoAni.Width, num214)), dColor * ((255 - npc.alpha) / 255f), npc.rotation, new Vector2(switchTwoAni.Width / 2f, num214 / 2f), npc.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(switchTwoGlow, drawCenter - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, switchTwoAni.Width, num214)), AAColor.COLOR_WHITEFADE1, npc.rotation, new Vector2(switchTwoAni.Width / 2f, num214 / 2f), npc.scale, npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }


            if (ShieldScale > 0)
            {
                BaseDrawing.DrawTexture(spritebatch, Shield, 0, npc.position, npc.width, npc.height, ShieldScale, 0, 0, 1, new Rectangle(0, 0, Shield.Width, Shield.Height), GetGlowAlpha(), true);
                BaseDrawing.DrawTexture(spritebatch, Ring, 0, npc.position, npc.width, npc.height, ShieldScale * 2, RingRoatation, 0, 1, new Rectangle(0, 0, Ring.Width, Ring.Height), dColor, true);
                BaseDrawing.DrawTexture(spritebatch, RingGlow, 0, npc.position, npc.width, npc.height, ShieldScale * 2, RingRoatation, 0, 1, new Rectangle(0, 0, Ring.Width, Ring.Height), AAColor.COLOR_WHITEFADE1, true);
            }
            return false;
        }

        public int MinionTimer = 0;
        public float Distance = 0;
        public float[] internalAI = new float[5];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == 2 || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
                writer.Write(internalAI[4]);
                writer.Write(Distance);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == 1)
            {
                internalAI[0] = reader.ReadFloat();
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
                internalAI[4] = reader.ReadFloat();
                Distance = reader.ReadFloat();
            }
        }


        public bool saythelinezero = false;
        public float ShieldScale = 0.5f;
        public float RingRoatation = 0;
        public int WeaponCount = Main.expertMode ? 6 : 4;
        bool RespawnArms1;
        bool RespawnArms2;

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

            if (npc.ai[0] > 0)
            {
                npc.ai[0]--;
            }

            npc.TargetClosest();

            if (Main.netMode != 1 && internalAI[3] == 0 && npc.ai[1] == 0)
            {
                RespawnArms();
                npc.netUpdate = true;
            }

            if (NPC.AnyNPCs(mod.NPCType("VoidStar")) ||
                NPC.AnyNPCs(mod.NPCType("Taser")) ||
                NPC.AnyNPCs(mod.NPCType("RealityCannon")) ||
                NPC.AnyNPCs(mod.NPCType("RiftShredder")) ||
                NPC.AnyNPCs(mod.NPCType("Neutralizer")) ||
                NPC.AnyNPCs(mod.NPCType("OmegaVolley")) ||
                NPC.AnyNPCs(mod.NPCType("NovaFocus")) ||
                NPC.AnyNPCs(mod.NPCType("GenocideCannon")))
            {
                npc.ai[1] = 0;
            }
            else
            {
                if (npc.ai[1] == 0)
                {
                    npc.ai[1] = 1;
                }
            }

            if (Distance < 160f)
            {
                Distance += 5f;
            }
            else
            {
                if (Main.netMode != 1)
                {
                    Distance = 160f;
                    npc.netUpdate = true;
                }
            }

            if (Main.netMode != 1)
            {
                AAWorld.zeroUS = false;
            }

            Player player = Main.player[npc.target];

            RingRoatation += 0.03f;

            if (player.dead || !player.active || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 6000f)
            {
                npc.TargetClosest();
                if (player.dead || !player.active || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 6000f)
                {
                    npc.Transform(ModLoader.GetMod("AAMod").NPCType("ZeroDeactivated"));
                }
                return;
            }
            if (ShieldScale < .5f)
            {
                ShieldScale += .05f;
            }

            if (ShieldScale > .5f)
            {
                ShieldScale = .5f;
            }

            if (internalAI[1] == 0)
            {
                npc.velocity.Y += 0.003f;
                if (npc.velocity.Y > .3f)
                {
                    internalAI[1] = 1f;
                    npc.netUpdate = true;
                }
            }
            else if (internalAI[1] == 1)
            {
                npc.velocity.Y -= 0.003f;
                if (npc.velocity.Y < -.3f)
                {
                    internalAI[1] = 0f;
                    npc.netUpdate = true;
                }
            }

            if (npc.ai[1] == 0)
            {
                npc.dontTakeDamage = true;
                npc.chaseable = false;
                npc.damage = 0;
                saythelinezero = false;
            }
            else
            {
                npc.dontTakeDamage = false;
                npc.chaseable = true;
                npc.damage = 160;
                if (Main.netMode != 1)
                {
                    npc.ai[2]++;
                }

                if (npc.ai[3] == 3)
                {
                    npc.defense = 75;
                }
                else
                {
                    npc.defense = 150;
                }

                if (npc.ai[3] == 0)
                {
                    if (npc.ai[2] % 20 == 0)
                    {
                        float Speed = 16f;
                        Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
                        int type = mod.ProjectileType("ZeroBeam1");
                        Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 33);
                        float rotation = (float)Math.Atan2(vector8.Y - (player.position.Y + (player.height * 0.5f)), vector8.X - (player.position.X + (player.width * 0.5f)));
                        Projectile.NewProjectile(vector8.X, vector8.Y, (float)(Math.Cos(rotation) * Speed * -1), (float)(Math.Sin(rotation) * Speed * -1), type, damage, 0f, 0);
                    }
                    if (npc.ai[2] >= 141 && Main.netMode != 1)
                    {
                        npc.ai[2] = 0;
                        npc.ai[3] = 3;
                    }
                }
                else if (npc.ai[3] == 1)
                {
                    if (npc.ai[2] % 30 == 0)
                    {
                        Main.PlaySound(SoundID.Item74, (int)npc.position.X, (int)npc.position.Y);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0 + Main.rand.Next(-14, 14), 0 + Main.rand.Next(-14, 14), mod.ProjectileType("ZeroRocket"), damage, 3); //Originally 85 damage
                    }
                    if (npc.ai[2] >= 151 && Main.netMode != 1)
                    {
                        npc.ai[2] = 0;
                        npc.ai[3] = 3;
                    }
                }
                else if (npc.ai[3] == 2)
                {
                    if (Collision.CanHit(npc.position, npc.width, npc.height, player.Center, player.width, player.height))
                    {
                        int[] array4 = new int[5];
                        Vector2[] array5 = new Vector2[5];
                        int num838 = 0;
                        float num839 = 2000f;
                        for (int num840 = 0; num840 < 255; num840++)
                        {
                            if (Main.player[num840].active && !Main.player[num840].dead)
                            {
                                Vector2 center9 = Main.player[num840].Center;
                                float num841 = Vector2.Distance(center9, npc.Center);
                                if (num841 < num839 && Collision.CanHit(npc.Center, 1, 1, center9, 1, 1))
                                {
                                    array4[num838] = num840;
                                    array5[num838] = center9;
                                    if (++num838 >= array5.Length)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        if (Main.rand.Next(10) == 10)
                        {
                            for (int num842 = 0; num842 < num838; num842++)
                            {
                                Vector2 vector82 = array5[num842] - npc.Center;
                                float ai = Main.rand.Next(100);
                                Vector2 vector83 = Vector2.Normalize(vector82.RotatedByRandom(0.78539818525314331)) * 14f;
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vector83.X, vector83.Y, mod.ProjectileType("ZeroShock"), damage, 0f, Main.myPlayer, vector82.ToRotation(), ai);
                            }
                        }
                    }
                    if (npc.ai[2] >= 180 && Main.netMode != 1)
                    {
                        npc.ai[2] = 0;
                        npc.ai[3] = 3;
                    }
                }
                else
                {
                    if (npc.ai[2] == 5)
                    {
                        int TeleportPos = Main.rand.Next(5);
                        int VoidHeight = 140;
                        Point spawnTilePos = new Point((Main.maxTilesX / 15 * 14) + (Main.maxTilesX / 15 / 2) - 100, VoidHeight);
                        Vector2 Origin = new Vector2(spawnTilePos.X * 16, spawnTilePos.Y * 16);

                        switch (TeleportPos)
                        {
                            case 0:
                                npc.position = Origin;
                                break;
                            case 1:
                                npc.position = Origin + new Vector2(0, 640);
                                break;
                            case 2:
                                npc.position = Origin + new Vector2(0, -640);
                                break;
                            case 3:
                                npc.position = Origin + new Vector2(640, 0);
                                break;
                            case 4:
                                npc.position = Origin + new Vector2(-640, 0);
                                break;
                        }
                    }
                    if (npc.life > npc.lifeMax * (2 / 3))
                    {
                        if (npc.ai[2] == 80 || npc.ai[2] == 240) // + lasers
                        {
                            Main.PlaySound(SoundID.Item73, (int)npc.position.X, (int)npc.position.Y);
                            Projectile.NewProjectile(npc.Center, new Vector2(0f, -12f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(0f, 12f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12f, 0f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(12f, 0f), mod.ProjectileType("ZeroBlast"), damage, 3);
                        }
                        if (npc.ai[2] == 160 || npc.ai[2] == 320) // x lasers
                        {
                            Main.PlaySound(SoundID.Item73, (int)npc.position.X, (int)npc.position.Y);
                            Projectile.NewProjectile(npc.Center, new Vector2(8f, 8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(8f, -8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-8f, 8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-8f, -8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                        }
                    }
                    else if (npc.life > npc.lifeMax / 3)
                    {
                        if (npc.ai[2] == 80) // + lasers
                        {
                            Main.PlaySound(SoundID.Item73, (int)npc.position.X, (int)npc.position.Y);
                            Projectile.NewProjectile(npc.Center, new Vector2(0f, -12f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(0f, 12f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12f, 0f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(12f, 0f), mod.ProjectileType("ZeroBlast"), damage, 3);
                        }
                        else if (npc.ai[2] == 160)
                        {
                            Main.PlaySound(SoundID.Item73, (int)npc.position.X, (int)npc.position.Y);
                            Projectile.NewProjectile(npc.Center, new Vector2(0f, -12f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(0f, 12f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12f, 0f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(12f, 0f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(8f, 8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(8f, -8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-8f, 8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-8f, -8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                        }
                        else if (npc.ai[2] == 240)
                        {
                            Main.PlaySound(SoundID.Item73, (int)npc.position.X, (int)npc.position.Y);
                            Projectile.NewProjectile(npc.Center, new Vector2(8f, 8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(8f, -8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-8f, 8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-8f, -8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                        }
                        else if (npc.ai[2] == 320)
                        {
                            Projectile.NewProjectile(npc.Center, new Vector2(0f, -12f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(0f, 12f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12f, 0f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(12f, 0f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(8f, 8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(8f, -8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-8f, 8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-8f, -8f), mod.ProjectileType("ZeroBlast"), damage, 3);
                        }
                    }
                    else
                    {
                        if (npc.ai[2] == 80) // + lasers
                        {
                            Main.PlaySound(SoundID.Item73, (int)npc.position.X, (int)npc.position.Y);
                            Projectile.NewProjectile(npc.Center, new Vector2(0f, -12f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(0f, 12f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12f, 0f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(12f, 0f), mod.ProjectileType("ZeroLaser"), damage, 3);
                        }
                        else if (npc.ai[2] == 160)
                        {
                            Main.PlaySound(SoundID.Item73, (int)npc.position.X, (int)npc.position.Y);
                            Projectile.NewProjectile(npc.Center, new Vector2(8f, 8f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(8f, -8f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-8f, 8f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-8f, -8f), mod.ProjectileType("ZeroLaser"), damage, 3);
                        }
                        else if (npc.ai[2] == 240)
                        {
                            Main.PlaySound(SoundID.Item73, (int)npc.position.X, (int)npc.position.Y);
                            Projectile.NewProjectile(npc.Center, new Vector2(0f, -12f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(0f, 12f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12f, 0f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(12f, 0f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(8f, 8f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(8f, -8f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-8f, 8f), mod.ProjectileType("ZeroLaser"), damage, 3);
                            Projectile.NewProjectile(npc.Center, new Vector2(-8f, -8f), mod.ProjectileType("ZeroLaser"), damage, 3);
                        }
                        if (Main.netMode != 1)
                        {
                            if (npc.ai[2] >= 320)
                            {
                                npc.ai[3] = Main.rand.Next(3);
                                npc.ai[2] = 0;
                                npc.netUpdate = true;
                            }
                        }
                    }

                    if (Main.netMode != 1)
                    {
                        if (npc.ai[2] >= 400)
                        {
                            npc.ai[3] = Main.rand.Next(3);
                            npc.ai[2] = 0;
                            npc.netUpdate = true;
                        }
                    }
                }

                if (ShieldScale > 0)
                {
                    ShieldScale -= .07f;
                }
                if (ShieldScale < 0)
                {
                    ShieldScale = 0;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[1] == 0)
            {
                npc.frame.Y = 0;
            }
            else if (npc.ai[1] == 1)
            {
                if (npc.ai[3] == 3)
                {
                    frameCounters = 0;
                    npc.ai[1]++;
                }
                else
                {
                    frameCounters++;
                    if (frameCounters > 4)
                    {
                        normalFrame++;
                        frameCounters = 0;
                    }
                    if (normalFrame >= 5)
                    {
                        normalFrame = 0;
                    }
                }
            }
            else if (npc.ai[1] == 2)
            {
                frameCounters++;
                if (frameCounters > 4)
                {
                    switchOneFrame++;
                    frameCounters = 0;
                }
                if (switchOneFrame >= 5)
                {
                    switchOneFrame = 0;
                    npc.ai[1]++;
                }
            }
            else if (npc.ai[1] == 3)
            {
                if (npc.ai[3] == 3)
                {
                    frameCounters++;
                    if (frameCounters > 4)
                    {
                        openFrame++;
                        frameCounters = 0;
                    }
                    if (openFrame >= 5)
                    {
                        openFrame = 0;
                    }
                }
                else
                {
                    frameCounters++;
                    if (frameCounters > 4)
                    {
                        switchTwoFrame++;
                        frameCounters = 0;
                    }
                    if (switchTwoFrame >= 5)
                    {
                        switchTwoFrame = 0;
                        npc.ai[1] = 1;
                    }
                }
            }
        }

        public string ArmChoice()
        {
            string Choice = null;
            while (Choice == null)
            {
                int Arms = Main.rand.Next(8);
                switch (Arms)
                {
                    case 0:
                        Choice = "GenocideCannon";
                        break;
                    case 1:
                        Choice = "Neutralizer";
                        break;
                    case 2:
                        Choice = "NovaFocus";
                        break;
                    case 3:
                        Choice = "OmegaVolley";
                        break;
                    case 4:
                        Choice = "RealityCannon";
                        break;
                    case 5:
                        Choice = "RiftShredder";
                        break;
                    case 6:
                        Choice = "Taser";
                        break;
                    case 7:
                        Choice = "VoidStar";
                        break;
                }

                if (NPC.AnyNPCs(mod.NPCType(Choice)))
                {
                    Choice = null;
                }
            }
            return Choice;
        }

        public void MoveToPoint(Vector2 point)
        {
            float moveSpeed = 16f;
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
    }
}
