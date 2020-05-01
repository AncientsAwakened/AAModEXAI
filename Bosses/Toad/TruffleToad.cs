using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using AAMod;

namespace AAModEXAI.Bosses.Toad
{
    [AutoloadBossHead]
    public class TruffleToad : ModNPC
    {
        public float bossLife;
        public int damage = 0;

        public int TeleCooldown = 300;
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

                writer.Write(Minion[0]);
                writer.Write(Minion[1]);
                writer.Write(Minion[2]);

                writer.Write(TeleCooldown);
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

                Minion[0] = reader.ReadBool();
                Minion[1] = reader.ReadBool();
                Minion[2] = reader.ReadBool();

                TeleCooldown = reader.ReadInt();
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Truffle Toad");
            Main.npcFrameCount[npc.type] = 12;
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 2000;
            npc.damage = 20;
            npc.defense = 10;
            npc.knockBackResist = 0f;
            npc.value = Item.sellPrice(0, 1, 0, 0);
            npc.aiStyle = -1;
            npc.width = 98;
            npc.height = 72;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = false;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/TODE");
            npc.netAlways = true;
            bossBag = ModLoader.GetMod("AAMod").ItemType("ToadBag");
            npc.alpha = 255;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = new LegacySoundStyle(29, 13, Terraria.Audio.SoundType.Sound);
            if (Main.expertMode)
            {
                npc.defense = 20;
            }
        }

        public static int AISTATE_JUMP = 0, AISTATE_BARF = 1, AISTATE_JUMPALOT = 2, AISTATE_BUBBLES = 3, AISTATE_SEED = 4, AISTATE_STOMP = 5, AISTATE_TOADS = 6, AISTATE_BUBBLES2 = 7;
        public float[] internalAI = new float[5];
        public bool[] Minion = new bool[3];
        public bool tonguespawned = false;
        public bool TongueAttack = false;
        public float AIChangeRate = 180;
        public float JumpX = 6f, JumpY = -8f, JumpX2 = 6f, JumpY2 = -10f;

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
            npc.TargetClosest();
            Player player = Main.player[npc.target]; // makes it so you can reference the player the npc is targetting
            AAModGlobalNPC.Toad = npc.whoAmI;

            Vector2 tile = new Vector2(npc.Center.X,npc.Center.Y + npc.height / 2);
            bool tileCheck = Main.tile[(int)(tile.X / 16), (int)(tile.Y / 16)].active() && (TileID.Sets.Platforms[Main.tile[(int)(tile.X / 16), (int)(tile.Y / 16)].type] || Main.tileSolid[Main.tile[(int)(tile.X / 16), (int)(tile.Y / 16)].type]);
            if (player.Center.Y + player.height / 2 >= npc.Center.Y + npc.height / 2 + 20f && tileCheck) 
            {
                npc.noTileCollide = true;
                internalAI[4] = 1f;
            }
            if (internalAI[4] == 1f)
            {
                npc.noTileCollide = true;
                npc.noGravity = false;
                if (player.Center.Y + player.height / 2 <= npc.Center.Y + npc.height / 2) 
                {
                    npc.noTileCollide = false;
                    if(tileCheck)
                    {
                        npc.velocity.X *= .2f;
                        npc.velocity.Y = 0f;
                    }
                    internalAI[4] = 2f;
                }
            }
            else if (internalAI[4] == 2f)
            {
                npc.noTileCollide = false;
                if(tileCheck)
                {
                    npc.velocity.X *= .2f;
                    npc.velocity.Y = 0f;
                    internalAI[4] = 0;
                }
            }

            if (player.dead || !player.active || !player.ZoneGlowshroom)
            {
                npc.TargetClosest();
                if (player.dead || !player.active || !player.ZoneGlowshroom)
                {
                    npc.alpha += 5;
                    if (npc.alpha >= 255)
                    {
                        npc.active = false;
                        npc.netUpdate = true;
                    }
                }
            }

            if (player != null)
            {
                if(TeleCooldown > 0)
                {
                    TeleCooldown --;
                }
                float dist = npc.Distance(player.Center);
                Vector2 tileabove = new Vector2(npc.Center.X,npc.Center.Y - npc.height / 2);
                Vector2 tileleft = new Vector2(npc.Center.X - npc.width / 2,npc.Center.Y);
                Vector2 tileright = new Vector2(npc.Center.X + npc.width / 2,npc.Center.Y);
                Vector2 tile1 = new Vector2(npc.Center.X - npc.width / 2,npc.Center.Y - npc.height / 2);
                Vector2 tile2 = new Vector2(npc.Center.X + npc.width / 2,npc.Center.Y - npc.height / 2);
                bool tileCheckabove = Main.tile[(int)(tileabove.X / 16), (int)(tileabove.Y / 16)].active() && Main.tileSolid[Main.tile[(int)(tileabove.X / 16), (int)(tileabove.Y / 16)].type];
                bool tileCheckleft = Main.tile[(int)(tileleft.X / 16), (int)(tileleft.Y / 16)].active() && Main.tileSolid[Main.tile[(int)(tileleft.X / 16), (int)(tileleft.Y / 16)].type];
                bool tileCheckright = Main.tile[(int)(tileright.X / 16), (int)(tileright.Y / 16)].active() && Main.tileSolid[Main.tile[(int)(tileright.X / 16), (int)(tileright.Y / 16)].type];
                bool tileCheck1 = Main.tile[(int)(tile1.X / 16), (int)(tile1.Y / 16)].active() && Main.tileSolid[Main.tile[(int)(tile1.X / 16), (int)(tile1.Y / 16)].type];
                bool tileCheck2 = Main.tile[(int)(tile2.X / 16), (int)(tile2.Y / 16)].active() && Main.tileSolid[Main.tile[(int)(tile2.X / 16), (int)(tile2.Y / 16)].type];
                bool tiletele = TeleCooldown == 0 && !npc.noTileCollide && ((tileCheckabove && npc.collideY) || (tileCheckleft && npc.collideX) || (tileCheckright && npc.collideX) || ((tileCheck1 || tileCheck2) && (npc.collideX || npc.collideY)));
                if (dist > 400 || tiletele)
                {
                    npc.alpha += 3;
                    if (npc.alpha >= 255)
                    {
                        Vector2 tele = new Vector2(player.Center.X, player.Center.Y - 350);
                        npc.Center = tele;
                        if(tiletele) TeleCooldown = 300;
                        for (int m = 0; m < 6; m++)
                        {
                            Dust.NewDust(npc.Center, npc.width, npc.height, DustID.Blood, npc.velocity.RotatedBy(Main.rand.NextFloat() * 3.1415926f).X * 0.2f, npc.velocity.RotatedBy(Main.rand.NextFloat() * 3.1415926f).Y * 0.2f, ModLoader.GetMod("AAMod").DustType("ShroomDust"), default, 1.5f);
                        }
                        npc.netUpdate = true;
                    }
                }
                else
                {
                    npc.alpha -= 5;
                    if (npc.alpha <= 0)
                    {
                        npc.alpha = 0;
                    }
                }
            }

            int[] Shrooms = BaseAI.GetNPCs(npc.Center, mod.NPCType("ShroomGlow"), 1000);
            if (Shrooms != null && Shrooms.Length > 0)
            {
                float ShroomCount = 1 + (Shrooms.Length / 10);
                npc.damage = (int)(npc.defDamage * ShroomCount);
                npc.defense = (int)(npc.defDefense * ShroomCount);
                if(internalAI[3] ++ > 20)
                {
                    if(npc.life < npc.lifeMax) npc.life += Shrooms.Length;
                    internalAI[3] = 0;
                }
                AIChangeRate = 120;
                JumpX = 8f; JumpY = -10f; JumpX2 = 10f; JumpY2 = -14f;
                if (Main.netMode != 2 && Main.LocalPlayer.miscCounter % 2 == 0)
                {
                    for (int m = 0; m < Shrooms.Length; m++)
                    {
                        NPC npc2 = Main.npc[Shrooms[m]];
                        if (npc2 != null && npc2.active)
                        {
                            int dustID = Dust.NewDust(npc2.position, npc2.width, npc2.height, ModLoader.GetMod("AAMod").DustType("ShroomDust"));
                            Main.dust[dustID].position += npc.position - npc.oldPosition;
                            Main.dust[dustID].velocity = (npc.Center - npc2.Center) * 0.10f;
                            Main.dust[dustID].noGravity = true;
                        }
                    }
                }
            }
            else
            {
                npc.damage = npc.defDamage;
                npc.defense = npc.defDefense;
                AIChangeRate = 180;
                JumpX = 6f; JumpY = -8f; JumpX2 = 6f; JumpY2 = -10f;
            }

            if (npc.velocity.Y != 0)
            {
                if (npc.velocity.X < 0)
                {
                    npc.spriteDirection = 1;
                }
                else if (npc.velocity.X > 0)
                {
                    npc.spriteDirection = -1;
                }
            }
            else
            {
                if (player.position.X < npc.position.X)
                {
                    npc.spriteDirection = 1;
                }
                else if (player.position.X > npc.position.X)
                {
                    npc.spriteDirection = -1;
                }
            }

            if (internalAI[0] == AISTATE_JUMP)
            {
                npc.wet = false;
                AITortoise();
                //BaseAI.AISlime(npc, ref npc.ai, false, 20, JumpX, JumpY, JumpX2, JumpY2);
                internalAI[1]++;
                if (internalAI[1] == 179)
                {
                    Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 13);
                }
                if (internalAI[1] >= AIChangeRate && Main.netMode != 1)
                {
                    npc.velocity.X = 0f;
                    internalAI[1] = 0;
                    internalAI[0] = Main.rand.Next(Main.expertMode ? 8 : 7);
                    internalAI[2] = 0;
                    npc.ai = new float[4];
                    npc.netUpdate = true;
                }
            }
            else if (internalAI[0] == AISTATE_BARF)
            {
                if (Main.netMode != 1 && npc.velocity.Y == 0)
                {
                    internalAI[1]++;
                }
                npc.velocity.X *= .98f;
                if (internalAI[1] >= 35)
                {
                    if (npc.velocity.Y == 0 && Main.netMode != 1)
                    {
                        internalAI[2]++;
                    }
                    if (internalAI[2] > 5)
                    {
                        internalAI[2] = 0;
                        if (npc.direction == -1)
                        {
                            Projectile.NewProjectile(npc.Center, new Vector2(-6 + Main.rand.Next(0, 6), -4 + Main.rand.Next(-2, 0)), mod.ProjectileType("ToadBomb"), damage, 3);
                        }
                        else
                        {
                            Projectile.NewProjectile(npc.Center, new Vector2(6 + Main.rand.Next(-6, 0), -4 + Main.rand.Next(-2, 0)), mod.ProjectileType("ToadBomb"), damage, 3);
                        }
                        npc.netUpdate = true;
                    }
                }
                if (internalAI[1] >= 100)
                {
                    internalAI[0] = AISTATE_JUMP;
                    internalAI[1] = 0;
                    internalAI[2] = 0;
                    npc.netUpdate = true;
                }
            }
            else if (internalAI[0] == AISTATE_JUMPALOT)
            {
                internalAI[1]++;// if (npc.ai[0] < -10) npc.ai[0] = -10; //force rapid jumping
                AITortoise();
                npc.wet = false;
                //BaseAI.AISlime(npc, ref npc.ai, false, -10, JumpX, JumpY, JumpX2, JumpY2);
                if (Main.netMode != 1)
                {
                    internalAI[1]++;
                }
                if (internalAI[1] >= 300)
                {
                    npc.velocity.X = 0f;
                    internalAI[1] = 0;
                    internalAI[0] = 0;
                    internalAI[2] = 0;
                    npc.ai = new float[4];
                    npc.netUpdate = true;
                }
            }
            else if (internalAI[0] == AISTATE_BUBBLES)
            {
                if (Main.netMode != 1 && npc.velocity.Y == 0)
                {
                    internalAI[1]++;
                }
                npc.velocity.X *= .98f;
                if (internalAI[1] >= 35)
                {
                    if (npc.velocity.Y == 0 && Main.netMode != 1)
                    {
                        internalAI[2]++;
                    }
                    if (internalAI[2] > 8)
                    {
                        internalAI[2] = 0;
                        if (npc.direction == -1)
                        {
                            Projectile.NewProjectile(npc.Center, new Vector2(-6 + Main.rand.Next(0, 6), -4 + Main.rand.Next(-2, 0)), mod.ProjectileType("FungusBubble"), damage, 3);
                        }
                        else
                        {
                            Projectile.NewProjectile(npc.Center, new Vector2(6 + Main.rand.Next(-6, 0), -4 + Main.rand.Next(-2, 0)), mod.ProjectileType("FungusBubble"), damage, 3); //Originally 35 damage
                        }
                        npc.netUpdate = true;
                    }
                }
                if (internalAI[1] >= 100)
                {
                    internalAI[0] = AISTATE_JUMP;
                    internalAI[1] = 0;
                    internalAI[2] = 0;
                    npc.ai = new float[4];
                    npc.netUpdate = true;
                }
            }
            else if (internalAI[0] == AISTATE_SEED)
            {
                if (Main.netMode != 1 && npc.velocity.Y == 0)
                {
                    internalAI[1]++;
                }
                npc.velocity.X *= .98f;
                if (internalAI[1] >= 35)
                {
                    if (npc.velocity.Y == 0 && Main.netMode != 1)
                    {
                        internalAI[2]++;
                    }
                    if (internalAI[2] > 25)
                    {
                        internalAI[2] = 0;
                        if (npc.direction == -1)
                        {
                            Projectile.NewProjectile(npc.Center, new Vector2(-6 + Main.rand.Next(0, 6), -4 + Main.rand.Next(-2, 0)), mod.ProjectileType("Seed"), 0, 0);
                        }
                        else
                        {
                            Projectile.NewProjectile(npc.Center, new Vector2(6 + Main.rand.Next(-6, 0), -4 + Main.rand.Next(-2, 0)), mod.ProjectileType("Seed"), 0, 0);
                        }
                        npc.netUpdate = true;
                    }
                }
                if (internalAI[1] >= 100)
                {
                    internalAI[0] = AISTATE_JUMP;
                    internalAI[1] = 0;
                    internalAI[2] = 0;
                    npc.ai = new float[4];
                    npc.netUpdate = true;
                }
            }
            else if (internalAI[0] == AISTATE_STOMP)
            {
                if (internalAI[2] == 0)
                {
                    if (Main.netMode != 1 && npc.velocity.Y == 0)
                    {
                        npc.TargetClosest(true);
                        npc.velocity.X = 6 * npc.direction;
                        npc.velocity.Y = -10f;
                        internalAI[2] = 1f;
                        npc.netUpdate = true;
                    }
                }
                else
                {
                    if (npc.velocity.Y == 0f)
                    {
                        Main.PlaySound(SoundID.Item14, npc.position);
                        npc.ai[0] = 0f;
                        for (int num622 = (int)npc.position.X - 20; num622 < (int)npc.position.X + npc.width + 40; num622 += 20)
                        {
                            for (int num623 = 0; num623 < 4; num623++)
                            {
                                int num624 = Dust.NewDust(new Vector2(npc.position.X - 20f, npc.position.Y + npc.height), npc.width + 20, 4, 31, 0f, 0f, 100, default, 1.5f);
                                Main.dust[num624].velocity *= 0.2f;
                            }
                            int num625 = Gore.NewGore(new Vector2(num622 - 20, npc.position.Y + npc.height - 8f), default, Main.rand.Next(61, 64), 1f);
                            Main.gore[num625].velocity *= 0.4f;
                        }
                        for (int a = 0; a < 4; a++)
                        {
                            NPC.NewNPC((int)(npc.position.X + Main.rand.Next(40)), (int)(npc.position.Y + npc.height), mod.NPCType("GlowshroomGrow"));
                        }
                        internalAI[0] = AISTATE_JUMP;
                        internalAI[1] = 0;
                        internalAI[2] = 0;
                        npc.ai = new float[4];
                        npc.netUpdate = true;
                    }
                    else
                    {
                        npc.TargetClosest(true);
                        if (npc.position.X < Main.player[npc.target].position.X && npc.position.X + npc.width > Main.player[npc.target].position.X + Main.player[npc.target].width)
                        {
                            npc.velocity.X = npc.velocity.X * 0.9f;
                            npc.velocity.Y = npc.velocity.Y + 0.4f;
                        }
                        else
                        {
                            if (npc.direction < 0)
                            {
                                npc.velocity.X = npc.velocity.X - 0.2f;
                            }
                            else if (npc.direction > 0)
                            {
                                npc.velocity.X = npc.velocity.X + 0.2f;
                            }
                            float num626 = 3f;
                            if (npc.life < npc.lifeMax)
                            {
                                num626 += 1f;
                            }
                            if (npc.life < npc.lifeMax / 2)
                            {
                                num626 += 1f;
                            }
                            if (npc.life < npc.lifeMax / 4)
                            {
                                num626 += 1f;
                            }
                            if (npc.velocity.X < -num626)
                            {
                                npc.velocity.X = -num626;
                            }
                            if (npc.velocity.X > num626)
                            {
                                npc.velocity.X = num626;
                            }
                        }
                    }
                }
            }
            else if (internalAI[0] == AISTATE_BUBBLES2)
            {
                if (Main.netMode != 1 && npc.velocity.Y == 0)
                {
                    internalAI[1]++;
                }
                npc.velocity.X *= .98f;
                if (internalAI[1] >= 35)
                {
                    if (npc.velocity.Y == 0 && Main.netMode != 1)
                    {
                        internalAI[2]++;
                    }
                    if (internalAI[2] > 20)
                    {
                        internalAI[2] = 0;
                        if (npc.direction == -1)
                        {
                            Projectile.NewProjectile(npc.Center, new Vector2(-6 + Main.rand.Next(0, 6), -4 + Main.rand.Next(-2, 0)), mod.ProjectileType("ToadBubble"), damage, 3);
                        }
                        else
                        {
                            Projectile.NewProjectile(npc.Center, new Vector2(6 + Main.rand.Next(-6, 0), -4 + Main.rand.Next(-2, 0)), mod.ProjectileType("ToadBubble"), damage, 3);
                        }
                        npc.netUpdate = true;
                    }
                }
                if (internalAI[1] >= 100)
                {
                    internalAI[0] = AISTATE_JUMP;
                    internalAI[1] = 0;
                    internalAI[2] = 0;
                    npc.netUpdate = true;
                }
            }
            else if (internalAI[0] == AISTATE_TOADS)
            {
                if (Main.netMode != 1 && npc.velocity.Y == 0)
                {
                    internalAI[1]++;
                }
                npc.velocity.X *= .98f;
                if (internalAI[1] == 35)
                {
                    NPC.NewNPC((int)(npc.Center.X - 30f), (int)(npc.Center.Y - 16), mod.NPCType("TinyToad"));
                    NPC.NewNPC((int)npc.Center.X, (int)(npc.Center.Y - 16), mod.NPCType("TinyToad"));
                    NPC.NewNPC((int)(npc.Center.X + 30f), (int)(npc.Center.Y - 16), mod.NPCType("TinyToad"));
                }
                if (internalAI[1] >= 100)
                {
                    internalAI[0] = AISTATE_JUMP;
                    internalAI[1] = 0;
                    internalAI[2] = 0;
                    npc.netUpdate = true;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.velocity.Y == 0 && internalAI[0] != AISTATE_JUMP)
            {
                if (internalAI[0] == AISTATE_BARF || internalAI[0] == AISTATE_BUBBLES || internalAI[0] == AISTATE_BUBBLES2)
                {
                    if (npc.frame.Y < frameHeight * 6)
                    {
                        npc.frame.Y = frameHeight * 6;
                    }
                    if (npc.frameCounter >= 10)
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y += frameHeight;
                        if (npc.frame.Y > frameHeight * 11)
                        {
                            npc.frame.Y = frameHeight * 11;
                        }
                    }
                }
                else
                {
                    npc.frame.Y = 0;
                }
            }
            else
            {
                if (npc.frameCounter >= 10)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y += frameHeight;
                    if (npc.frame.Y > (frameHeight * 4))
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y = frameHeight * 4;
                    }
                }
            }
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("ToadTrophy"));
            }
            AAWorld.downedToad = true;
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("ToadMask"));
                }
                string[] lootTable = { "MushrockStaff", "ToadTongue", "Todegun" };
                int loot = Main.rand.Next(lootTable.Length);
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType(lootTable[loot]));
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * .8f);
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D GlowTex = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/TruffleToad_Glow");

            BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc, dColor, true);
            BaseDrawing.DrawTexture(sb, GlowTex, 0, npc, ColorUtils.COLOR_GLOWPULSE, true);
            return false;
        }

        private void AITortoise()
        {
            npc.TargetClosest(true);
            bool flag31 = true;
            int num513 = 0;
            if (npc.velocity.X < 0f)
            {
                num513 = -1;
            }
            if (npc.velocity.X > 0f)
            {
                num513 = 1;
            }
            Vector2 position = npc.position;
            position.X += npc.velocity.X;
            int num514 = (int)((position.X + npc.width / 2 + (npc.width / 2 + 1) * num513) / 16f);
            int num515 = (int)((position.Y + npc.height - 1f) / 16f);
            if (num514 * 16 < position.X + npc.width && num514 * 16 + 16 > position.X && ((Main.tile[num514, num515].nactive() && !Main.tile[num514, num515].topSlope() && !Main.tile[num514, num515 - 1].topSlope() && ((Main.tileSolid[Main.tile[num514, num515].type] && !Main.tileSolidTop[Main.tile[num514, num515].type]) || (flag31 && Main.tileSolidTop[Main.tile[num514, num515].type] && (!Main.tileSolid[Main.tile[num514, num515 - 1].type] || !Main.tile[num514, num515 - 1].nactive()) && Main.tile[num514, num515].type != 16 && Main.tile[num514, num515].type != 18 && Main.tile[num514, num515].type != 134))) || (Main.tile[num514, num515 - 1].halfBrick() && Main.tile[num514, num515 - 1].nactive())) && (!Main.tile[num514, num515 - 1].nactive() || !Main.tileSolid[Main.tile[num514, num515 - 1].type] || Main.tileSolidTop[Main.tile[num514, num515 - 1].type] || (Main.tile[num514, num515 - 1].halfBrick() && (!Main.tile[num514, num515 - 4].nactive() || !Main.tileSolid[Main.tile[num514, num515 - 4].type] || Main.tileSolidTop[Main.tile[num514, num515 - 4].type]))) && (!Main.tile[num514, num515 - 2].nactive() || !Main.tileSolid[Main.tile[num514, num515 - 2].type] || Main.tileSolidTop[Main.tile[num514, num515 - 2].type]) && (!Main.tile[num514, num515 - 3].nactive() || !Main.tileSolid[Main.tile[num514, num515 - 3].type] || Main.tileSolidTop[Main.tile[num514, num515 - 3].type]) && (!Main.tile[num514 - num513, num515 - 3].nactive() || !Main.tileSolid[Main.tile[num514 - num513, num515 - 3].type] || Main.tileSolidTop[Main.tile[num514 - num513, num515 - 3].type]))
            {
                float num516 = num515 * 16;
                if (Main.tile[num514, num515].halfBrick())
                {
                    num516 += 8f;
                }
                if (Main.tile[num514, num515 - 1].halfBrick())
                {
                    num516 -= 8f;
                }
                if (num516 < position.Y + npc.height)
                {
                    float num517 = position.Y + npc.height - num516;
                    if (num517 <= 16.1)
                    {
                        npc.gfxOffY += npc.position.Y + npc.height - num516;
                        npc.position.Y = num516 - npc.height;
                        if (num517 < 9f)
                        {
                            npc.stepSpeed = 0.75f;
                        }
                        else
                        {
                            npc.stepSpeed = 1.5f;
                        }
                    }
                }
            }
            if (npc.ai[0] == 0f)
            {
                npc.velocity.X = npc.velocity.X * 0.5f;
                npc.ai[1] += 1f;
                if (npc.ai[1] >= 30f)
                {
                    npc.netUpdate = true;
                    npc.TargetClosest(true);
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                    npc.ai[0] = 2f;
                }
            }
            else
            {
                if (npc.ai[0] == 2f)
                {
                    if (Main.expertMode)
                    {
                        npc.damage = (int)(npc.defDamage * 2 * 0.9);
                    }
                    else
                    {
                        npc.damage = npc.defDamage * 2;
                    }
                    npc.defense = npc.defDefense * 2;
                    npc.ai[1] += 1f;
                    if (npc.ai[1] == 1f)
                    {
                        npc.netUpdate = true;
                        npc.TargetClosest(true);
                        npc.ai[2] += 0.3f;
                        npc.ai[1] += 1f;
                        bool flag34 = Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height);
                        float num531 = 10f;
                        if (!flag34)
                        {
                            num531 = 6f;
                        }
                        Vector2 vector67 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                        float num532 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector67.X;
                        float num533 = Math.Abs(num532) * 0.2f;
                        if (npc.directionY > 0)
                        {
                            num533 = 0f;
                        }
                        float num534 = Main.player[npc.target].position.Y - vector67.Y - num533;
                        float num535 = (float)Math.Sqrt(num532 * num532 + num534 * num534);
                        npc.netUpdate = true;
                        num535 = num531 / num535;
                        num532 *= num535;
                        num534 *= num535;
                        if (!flag34)
                        {
                            num534 = -10f;
                        }
                        npc.velocity.X = num532;
                        npc.velocity.Y = num534;
                        npc.ai[3] = npc.velocity.X;
                    }
                    else
                    {
                        if (npc.position.X + npc.width > Main.player[npc.target].position.X && npc.position.X < Main.player[npc.target].position.X + Main.player[npc.target].width && npc.position.Y < Main.player[npc.target].position.Y + Main.player[npc.target].height)
                        {
                            npc.velocity.X = npc.velocity.X * 0.8f;
                            npc.ai[3] = 0f;
                            if (npc.velocity.Y < 0f)
                            {
                                npc.velocity.Y = npc.velocity.Y + 0.2f;
                            }
                        }
                        if (npc.ai[3] != 0f)
                        {
                            npc.velocity.X = npc.ai[3];
                            npc.velocity.Y = npc.velocity.Y - 0.22f;
                        }
                        if (npc.ai[1] >= 90f)
                        {
                            npc.noGravity = false;
                            npc.ai[1] = 0f;
                            npc.ai[0] = 3f;
                        }
                    }
                    if (npc.wet && npc.directionY < 0)
                    {
                        npc.velocity.Y = npc.velocity.Y - 0.3f;
                    }
                    return;
                }
                if (npc.ai[0] == 3f)
                {
                    if (npc.wet && npc.directionY < 0)
                    {
                        npc.velocity.Y = npc.velocity.Y - 0.3f;
                    }
                    npc.velocity.X = npc.velocity.X * 0.96f;
                    if (npc.ai[2] > 0f)
                    {
                        npc.ai[2] -= 0.01f;
                    }
                    if (npc.ai[2] <= 0f && (npc.velocity.Y == 0f || npc.wet))
                    {
                        npc.netUpdate = true;
                        npc.ai[2] = 0f;
                        npc.ai[1] = 0f;
                        npc.ai[0] = 4f;
                        return;
                    }
                }
                else
                {
                    if (npc.ai[0] == 5f)
                    {
                        npc.damage = (int)(npc.defDamage * (Main.expertMode ? 1.4f : 1.8f));
                        npc.defense = npc.defDefense * 2;
                        npc.knockBackResist = 0f;
                        if (Main.rand.Next(3) < 2)
                        {
                            int num536 = Dust.NewDust(npc.Center - new Vector2(30f), 60, 60, 6, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, default(Color), 1.5f);
                            Main.dust[num536].noGravity = true;
                            Dust dust3 = Main.dust[num536];
                            dust3.velocity *= 0.2f;
                            Main.dust[num536].fadeIn = 1f;
                        }
                        npc.ai[1] += 1f;
                        if (npc.ai[3] > 0f)
                        {
                            int num;
                            if (npc.ai[3] == 1f)
                            {
                                Vector2 vector68 = npc.Center - new Vector2(50f);
                                for (int num537 = 0; num537 < 32; num537 = num + 1)
                                {
                                    int num538 = Dust.NewDust(vector68, 100, 100, 6, 0f, 0f, 100, default(Color), 2.5f);
                                    Main.dust[num538].noGravity = true;
                                    Dust dust3 = Main.dust[num538];
                                    dust3.velocity *= 3f;
                                    num538 = Dust.NewDust(vector68, 100, 100, 6, 0f, 0f, 100, default(Color), 1.5f);
                                    dust3 = Main.dust[num538];
                                    dust3.velocity *= 2f;
                                    Main.dust[num538].noGravity = true;
                                    num = num537;
                                }
                                for (int num539 = 0; num539 < 4; num539 = num + 1)
                                {
                                    int num540 = Gore.NewGore(vector68 + new Vector2(50 * Main.rand.Next(100) / 100f, 50 * Main.rand.Next(100) / 100f) - Vector2.One * 10f, default(Vector2), Main.rand.Next(61, 64), 1f);
                                    Gore gore = Main.gore[num540];
                                    gore.velocity *= 0.3f;
                                    Gore gore2 = Main.gore[num540];
                                    gore2.velocity.X = gore2.velocity.X + Main.rand.Next(-10, 11) * 0.05f;
                                    Gore gore3 = Main.gore[num540];
                                    gore3.velocity.Y = gore3.velocity.Y + Main.rand.Next(-10, 11) * 0.05f;
                                    num = num539;
                                }
                            }
                            for (int num541 = 0; num541 < 5; num541 = num + 1)
                            {
                                int num542 = Dust.NewDust(npc.position, npc.width, npc.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                                Main.dust[num542].velocity = Main.dust[num542].velocity * Main.rand.NextFloat();
                                num = num541;
                            }
                            npc.ai[3] += 1f;
                            if (npc.ai[3] >= 10f)
                            {
                                npc.ai[3] = 0f;
                            }
                        }
                        if (npc.ai[1] == 1f)
                        {
                            npc.netUpdate = true;
                            npc.TargetClosest(true);
                            bool flag35 = Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height);
                            float num543 = 16f;
                            if (!flag35)
                            {
                                num543 = 10f;
                            }
                            Vector2 vector69 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                            float num544 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector69.X;
                            float num545 = Math.Abs(num544) * 0.2f;
                            if (npc.directionY > 0)
                            {
                                num545 = 0f;
                            }
                            float num546 = Main.player[npc.target].position.Y - vector69.Y - num545;
                            float num547 = (float)Math.Sqrt(num544 * num544 + num546 * num546);
                            npc.netUpdate = true;
                            num547 = num543 / num547;
                            num544 *= num547;
                            num546 *= num547;
                            if (!flag35)
                            {
                                num546 = -12f;
                            }
                            npc.velocity.X = num544;
                            npc.velocity.Y = num546;
                        }
                        else
                        {
                            npc.velocity.X = npc.velocity.X * 0.9f;
                            if (npc.velocity.Y < 0f)
                            {
                                npc.velocity.Y = npc.velocity.Y + 0.2f;
                            }
                            if (npc.ai[2] == 0f || npc.ai[1] >= 1200f)
                            {
                                npc.ai[1] = 0f;
                                npc.ai[0] = 4f;
                            }
                        }
                        if (npc.wet && npc.directionY < 0)
                        {
                            npc.velocity.Y = npc.velocity.Y - 0.3f;
                        }
                        return;
                    }
                    if (npc.ai[0] == 4f)
                    {
                        npc.velocity.X = 0f;
                        npc.ai[1] += 1f;
                        if (npc.ai[1] >= 30f)
                        {
                            npc.TargetClosest(true);
                            npc.netUpdate = true;
                            npc.ai[1] = 0f;
                            npc.ai[0] = 0f;
                        }
                        if (npc.wet)
                        {
                            npc.ai[0] = 2f;
                            npc.ai[1] = 0f;
                            return;
                        }
                    }
                }
            }
        }
    }
}


