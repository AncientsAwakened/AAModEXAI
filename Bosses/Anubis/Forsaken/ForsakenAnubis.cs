using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using System;
using System.IO;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    public class ForsakenAnubis : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anubis; Forsaken Judge");
            Main.npcFrameCount[npc.type] = 12;
        }

        public override void SetDefaults()
        {
            npc.width = 88;
            npc.height = 180;
            npc.aiStyle = -1;
            npc.damage = 85;
            npc.defense = 80;
            npc.lifeMax = 150000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.knockBackResist = 0f;
            npc.boss = true;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/AnubisA");
            bossBag = ModLoader.GetMod("AAMod").ItemType("FAnubisBag");
            npc.value = Item.sellPrice(0, 10, 0, 0);
        }

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

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.85f);
        }

        public int RuneCount = 9;

        bool text = false;

        public override void AI()
        {

            int TeleportCount = 0;

            if (npc.life < (int)(npc.lifeMax * .75f))
            {
                TeleportCount = 1;
            }

            if (npc.life < (int)(npc.lifeMax * .5f))
            {
                TeleportCount = 2;
            }

            if (npc.life < (int)(npc.lifeMax * .25f))
            {
                TeleportCount = 3;
            }

            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }

            Player player = Main.player[npc.target];
            
            int anubis = BaseAI.GetNPC(player.Center, ModContent.NPCType<AAMod.NPCs.TownNPCs.Anubis>(), -1);
            if(anubis > -1)
            {
                Main.npc[anubis].life = 0;
                Main.npc[anubis].active = false;
            }

            if (player.Center.X < npc.Center.X)
            {
                npc.direction = npc.spriteDirection = 1;
            }
            else
            {
                npc.direction = npc.spriteDirection = -1;
            }

            npc.dontTakeDamage = false;
            npc.noGravity = true;

            if (internalAI[0] == 0)
            {
                npc.velocity.Y += 0.002f;
                if (npc.velocity.Y > .1f)
                {
                    internalAI[0] = 1f;
                    npc.netUpdate = true;
                }
            }
            else
            if (internalAI[0] == 1)
            {
                npc.velocity.Y -= 0.002f;
                if (npc.velocity.Y < -.1f)
                {
                    internalAI[0] = 0f;
                    npc.netUpdate = true;
                }
            }

            if (npc.life < npc.lifeMax / 3)
            {
                if (internalAI[2] == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int m = 0; m < RuneCount; m++)
                        {
                            int p = Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("CurseGlyphs"), npc.damage / 2, 0, Main.myPlayer);
                            Main.projectile[p].Center = npc.Center;
                            Main.projectile[p].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
                            Main.projectile[p].velocity *= 8f;
                            Main.projectile[p].ai[0] = m;
                            Main.projectile[p].netUpdate2 = true;
                            int dustType = ModContent.DustType<JudgementDust>();
                            int pieCut = 20;
                            for (int i = 0; i < pieCut; i++)
                            {
                                int dustID = Dust.NewDust(Main.projectile[p].position, Main.projectile[p].width, Main.projectile[p].height, dustType, 0f, 0f, 100, Color.White, 1.6f);
                                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(6f, 0f), i / (float)pieCut * 6.28f);
                                Main.dust[dustID].noLight = false;
                                Main.dust[dustID].noGravity = true;
                            }
                            for (int i = 0; i < pieCut; i++)
                            {
                                int dustID = Dust.NewDust(Main.projectile[p].position, Main.projectile[p].width, Main.projectile[p].height, dustType, 0f, 0f, 100, Color.White, 2f);
                                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(9f, 0f), i / (float)pieCut * 6.28f);
                                Main.dust[dustID].noLight = false;
                                Main.dust[dustID].noGravity = true;
                            }
                        }
                    }
                    internalAI[2] = 1;
                }
                npc.damage = 70;
            }

            npc.ai[1]++;

            switch (npc.ai[0])
            {
                case 0:
                    if (!AliveCheck(player))
                        break;

                    if (npc.ai[1] >= 200)
                    {
                        internalAI[1]++;
                        if (internalAI[3] < TeleportCount && internalAI[1] >= 50)
                        {
                            internalAI[3]++;
                            internalAI[1] = 0;
                            Teleport();
                        }
                        else
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = 0;
                            internalAI[3] = 0;
                            internalAI[1] = 0;
                            Teleport();
                        }
                        return;
                    }

                    int proj = Main.rand.Next(2) == 0 ? mod.ProjectileType("ForsakenBlast") : mod.ProjectileType("ForsakenSkull");

                    int damage = npc.damage / 2;

                    BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, proj, ref npc.ai[3], 60, damage, 10, false);

                    if (npc.ai[3] == 30)
                    {
                        Teleport();
                    }
                    break;
                case 1:
                    if (!AliveCheck(player))
                        break;

                    if (npc.ai[1] >= 130)
                    {
                        internalAI[1]++;
                        if (internalAI[3] < TeleportCount && internalAI[1] >= 50)
                        {
                            internalAI[3]++;
                            internalAI[1] = 0;
                            Teleport();
                        }
                        else
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = 0;
                            internalAI[3] = 0;
                            internalAI[1] = 0;
                            Teleport();
                        }
                        return;
                    }

                    if (!text)
                    {
                        text = true;
                        CombatText.NewText(npc.Hitbox, Color.ForestGreen, AAMod.Lang.BossChat("FAnubisCombat"), true);
                    }

                    if (npc.ai[1] == 10 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            if (npc.life < npc.lifeMax / 3)
                            {
                                int a = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, npc.Center.X - 150, npc.Center.Y);
                                int b = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, npc.Center.X + 150, npc.Center.Y);
                                int c = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, npc.Center.X, npc.Center.Y - 150);
                                int d = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, npc.Center.X, npc.Center.Y + 150);
                                int e = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, player.Center.X - 150, player.Center.Y);
                                int f = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, player.Center.X + 150, player.Center.Y);
                                int g = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, player.Center.X, player.Center.Y - 150);
                                int h = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, player.Center.X, player.Center.Y + 150);
                            }
                            else if (npc.life < (2 * npc.lifeMax) / 3)
                            {
                                int a = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, npc.Center.X - 150, npc.Center.Y);
                                int b = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, npc.Center.X + 150, npc.Center.Y);
                                int c = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, npc.Center.X, npc.Center.Y - 150);
                                int d = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, npc.Center.X, npc.Center.Y + 150);
                            }
                            else
                            {
                                int a = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, npc.Center.X - 180, npc.Center.Y - 60);
                                int b = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, npc.Center.X + 180, npc.Center.Y - 60);
                                int c = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("HorusSummon"), 0, 0, Main.myPlayer, npc.Center.X, npc.Center.Y - 200);
                            }
                        }
                        else
                        {
                            if (npc.life < npc.lifeMax / 2)
                            {
                                int m = NPC.NewNPC((int)npc.position.X + 130, (int)npc.position.Y, mod.NPCType("CurseCircle"));
                                Main.npc[m].Center = new Vector2(npc.Center.X + 130, npc.Center.Y);

                                int n = NPC.NewNPC((int)npc.position.X - 130, (int)npc.position.Y, mod.NPCType("CurseCircle"));
                                Main.npc[n].Center = new Vector2(npc.Center.X - 130, npc.Center.Y);

                                int o = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y + 130, mod.NPCType("CurseCircle"));
                                Main.npc[o].Center = new Vector2(npc.Center.X, npc.Center.Y + 130);

                                int p = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y - 130, mod.NPCType("CurseCircle"));
                                Main.npc[p].Center = new Vector2(npc.Center.X, npc.Center.Y - 130);

                                int r = NPC.NewNPC((int)player.position.X, (int)player.position.Y + 130, mod.NPCType("CurseCircle"));
                                Main.npc[o].Center = new Vector2(player.Center.X - 130, player.Center.Y + 130);

                                int s = NPC.NewNPC((int)player.position.X, (int)player.position.Y - 130, mod.NPCType("CurseCircle"));
                                Main.npc[p].Center = new Vector2(player.Center.X - 130, player.Center.Y - 130);
                            }
                            else
                            {
                                int m = NPC.NewNPC((int)npc.position.X + 130, (int)npc.position.Y, mod.NPCType("CurseCircle"));
                                Main.npc[m].Center = new Vector2(npc.Center.X + 130, npc.Center.Y - 60);

                                int n = NPC.NewNPC((int)npc.position.X - 130, (int)npc.position.Y, mod.NPCType("CurseCircle"));
                                Main.npc[n].Center = new Vector2(npc.Center.X - 130, npc.Center.Y - 60);

                                int o = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y + 130, mod.NPCType("CurseCircle"));
                                Main.npc[o].Center = new Vector2(npc.Center.X, npc.Center.Y + 130);
                            }
                        }
                    }
                    break;
                case 2:
                    if (!AliveCheck(player))
                        break;

                    if (npc.ai[1] == 120)
                    {
                        BaseAI.FireProjectile(player.position, npc.position, mod.ProjectileType("ForsakenStaff"), npc.damage / 2, 14, 10, -1);
                    }
                    if (npc.ai[1] == 140)
                    {
                        ScepterTeleport();
                    }

                    if (npc.ai[1] > 160 && !AAGlobalProjectile.AnyProjectiles(mod.ProjectileType("ForsakenStaff")))
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        Teleport();
                    }

                    break;

                case 3:
                    if (!AliveCheck(player))
                        break;

                    int Max = 3;

                    if (npc.life < npc.lifeMax / 2)
                    {
                        Max = 4;
                    }

                    if (npc.ai[1] > 120 &&  Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        float rotation = 2f * (float)Math.PI / Max;
                        Vector2 vel = npc.velocity;
                        vel.Normalize();
                        vel *= 10f;
                        int type = mod.ProjectileType("SunSummon");
                        for (int i = 0; i < Max; i++)
                        {
                            vel = vel.RotatedBy(rotation);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vel.X, vel.Y, type, 0, 4, Main.myPlayer, npc.direction > 0 ? -1f : 1f, 6f);
                        }
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        Teleport();
                        npc.netUpdate = true;
                    }
                    break;

                case 4:
                    if (!AliveCheck(player))
                        break;

                    if (npc.ai[1] >= 320)
                    {
                        internalAI[1]++;
                        if (internalAI[3] < TeleportCount && internalAI[1] >= 50)
                        {
                            internalAI[3]++;
                            internalAI[1] = 0;
                            Teleport();
                        }
                        else
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = 0;
                            internalAI[3] = 0;
                            internalAI[1] = 0;
                            Teleport();
                        }
                        return;
                    }

                    int proj1 = mod.ProjectileType("AnubisSoul");

                    BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, proj1, ref npc.ai[3], 100, npc.damage / 2, 10, false);

                    if (npc.ai[3] == 50)
                    {
                        Teleport();
                    }

                    break;

                case 5:
                    if (!AliveCheck(player))
                        break;
                    if (npc.life > npc.lifeMax / 2)
                    {
                        if (npc.ai[1] > 160 && !AAGlobalProjectile.AnyProjectiles(mod.ProjectileType("Block")))
                        {
                            internalAI[1]++;
                            if (internalAI[3] < TeleportCount && internalAI[1] >= 50)
                            {
                                internalAI[3]++;
                                internalAI[1] = 0;
                                Teleport();
                            }
                            else
                            {
                                npc.ai[0]++;
                                npc.ai[1] = 0;
                                npc.ai[2] = 0;
                                npc.ai[3] = 0;
                                internalAI[3] = 0;
                                internalAI[1] = 0;
                                Teleport();
                            }
                            return;
                        }
                        if (npc.ai[1] == 40)
                        {
                            int l = Projectile.NewProjectile(player.position + new Vector2(-800, 0), Vector2.Zero, mod.ProjectileType("BlockF"), npc.damage / 2, 7, Main.myPlayer, 0, 0);
                            int r = Projectile.NewProjectile(player.position + new Vector2(800, 0), Vector2.Zero, mod.ProjectileType("BlockF"), npc.damage / 2, 7, Main.myPlayer, 1, 0);
                            Main.projectile[l].ai[1] = r;
                            Main.projectile[l].Center = player.Center + new Vector2(-800, 0);
                            Main.projectile[r].ai[1] = l;
                            Main.projectile[r].Center = player.Center + new Vector2(800, 0);
                        }
                        if (npc.ai[1] == 80)
                        {
                            int u = Projectile.NewProjectile(player.position + new Vector2(0, -800), Vector2.Zero, mod.ProjectileType("BlockF1"), npc.damage / 2, 7, Main.myPlayer, 0, 0);
                            int d = Projectile.NewProjectile(player.position + new Vector2(0, 800), Vector2.Zero, mod.ProjectileType("BlockF1"), npc.damage / 2, 7, Main.myPlayer, 1, 0);
                            Main.projectile[u].ai[1] = d;
                            Main.projectile[u].Center = player.Center + new Vector2(0, -800);
                            Main.projectile[d].ai[1] = u;
                            Main.projectile[d].Center = player.Center + new Vector2(0, 800);
                        }
                    }
                    else
                    {
                        if (npc.ai[1] > 240 && !AAGlobalProjectile.AnyProjectiles(mod.ProjectileType("BlockF")))
                        {
                            internalAI[1]++;
                            if (internalAI[3] < TeleportCount && internalAI[1] >= 50)
                            {
                                internalAI[3]++;
                                internalAI[1] = 0;
                                Teleport();
                            }
                            else
                            {
                                npc.ai[0]++;
                                npc.ai[1] = 0;
                                npc.ai[2] = 0;
                                npc.ai[3] = 0;
                                internalAI[3] = 0;
                                internalAI[1] = 0;
                                Teleport();
                            }
                            return;
                        }
                        if (npc.ai[1] % 30 == 0 && npc.ai[1] <= 240)
                        {
                            if (Main.rand.Next(4) == 0)
                            {
                                int l = Projectile.NewProjectile(player.position + new Vector2(-800, 0), Vector2.Zero, mod.ProjectileType("BlockF"), npc.damage / 2, 7, Main.myPlayer, 0, 0);
                                int r = Projectile.NewProjectile(player.position + new Vector2(800, 0), Vector2.Zero, mod.ProjectileType("BlockF"), npc.damage / 2, 7, Main.myPlayer, 1, 0);
                                Main.projectile[l].ai[1] = r;
                                Main.projectile[l].Center = player.Center + new Vector2(-800, 0);
                                Main.projectile[r].ai[1] = l;
                                Main.projectile[r].Center = player.Center + new Vector2(800, 0);
                                int u = Projectile.NewProjectile(player.position + new Vector2(0, -800), Vector2.Zero, mod.ProjectileType("BlockF1"), npc.damage / 2, 7, Main.myPlayer, 0, 0);
                                int d = Projectile.NewProjectile(player.position + new Vector2(0, 800), Vector2.Zero, mod.ProjectileType("BlockF1"), npc.damage / 2, 7, Main.myPlayer, 1, 0);
                                Main.projectile[u].ai[1] = d;
                                Main.projectile[u].Center = player.Center + new Vector2(0, -800);
                                Main.projectile[d].ai[1] = u;
                                Main.projectile[d].Center = player.Center + new Vector2(0, 800);
                            }
                            else if (Main.rand.Next(2) == 0)
                            {
                                int u = Projectile.NewProjectile(player.position + new Vector2(0, -800), Vector2.Zero, mod.ProjectileType("BlockF1"), npc.damage / 2, 7, Main.myPlayer, 0, 0);
                                int d = Projectile.NewProjectile(player.position + new Vector2(0, 800), Vector2.Zero, mod.ProjectileType("BlockF1"), npc.damage / 2, 7, Main.myPlayer, 1, 0);
                                Main.projectile[u].ai[1] = d;
                                Main.projectile[u].Center = player.Center + new Vector2(0, -800);
                                Main.projectile[d].ai[1] = u;
                                Main.projectile[d].Center = player.Center + new Vector2(0, 800);
                            }
                            else
                            {
                                int l = Projectile.NewProjectile(player.position + new Vector2(-800, 0), Vector2.Zero, mod.ProjectileType("BlockF"), npc.damage / 2, 7, Main.myPlayer, 0, 0);
                                int r = Projectile.NewProjectile(player.position + new Vector2(800, 0), Vector2.Zero, mod.ProjectileType("BlockF"), npc.damage / 2, 7, Main.myPlayer, 1, 0);
                                Main.projectile[l].ai[1] = r;
                                Main.projectile[l].Center = player.Center + new Vector2(-800, 0);
                                Main.projectile[r].ai[1] = l;
                                Main.projectile[r].Center = player.Center + new Vector2(800, 0);
                            }
                        }

                    }
                    break;

                case 6:
                    if (!AliveCheck(player))
                        break;
                    if (npc.ai[1] > 180)
                    {
                        internalAI[1]++;
                        if (internalAI[3] < TeleportCount && internalAI[1] >= 50)
                        {
                            internalAI[3]++;
                            internalAI[1] = 0;
                            Teleport();
                        }
                        else
                        {
                            npc.ai[0] = Main.rand.Next(3) == 0? 5:0;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = 0;
                            internalAI[3] = 0;
                            internalAI[1] = 0;
                            Teleport();
                        }
                        return;
                    }
                    if (npc.ai[1] == 120)
                    {
                        if (npc.life > npc.lifeMax / 2)
                        {
                            int a = Projectile.NewProjectile(player.position + new Vector2(-250, 0), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[a].Center = player.Center + new Vector2(-250, 0);
                            Kaboom(Main.projectile[a]);
                            int b = Projectile.NewProjectile(player.position + new Vector2(250, 0), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[b].Center = player.Center + new Vector2(250, 0);
                            Kaboom(Main.projectile[b]);
                            int c = Projectile.NewProjectile(player.position + new Vector2(0, -250), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[c].Center = player.Center + new Vector2(0, -250);
                            Kaboom(Main.projectile[c]);
                            int d = Projectile.NewProjectile(player.position + new Vector2(0, 250), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[d].Center = player.Center + new Vector2(0, 250);
                            Kaboom(Main.projectile[d]);
                            int e = Projectile.NewProjectile(player.position + new Vector2(-200, 200), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[e].Center = player.Center + new Vector2(-200, 200);
                            Kaboom(Main.projectile[e]);
                            int f = Projectile.NewProjectile(player.position + new Vector2(200, 200), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[f].Center = player.Center + new Vector2(200, 200);
                            Kaboom(Main.projectile[f]);
                            int g = Projectile.NewProjectile(player.position + new Vector2(200, -200), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[g].Center = player.Center + new Vector2(200, -200);
                            Kaboom(Main.projectile[g]);
                            int h = Projectile.NewProjectile(player.position + new Vector2(-200, -200), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[h].Center = player.Center + new Vector2(-200, -200);
                            Kaboom(Main.projectile[h]);
                        }
                        else
                        {
                            int a = Projectile.NewProjectile(player.position + new Vector2(-250, 0), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[a].Center = player.Center + new Vector2(-250, 0);
                            Kaboom(Main.projectile[a]);
                            int b = Projectile.NewProjectile(player.position + new Vector2(250, 0), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[b].Center = player.Center + new Vector2(250, 0);
                            Kaboom(Main.projectile[b]);
                            int c = Projectile.NewProjectile(player.position + new Vector2(0, -250), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[c].Center = player.Center + new Vector2(0, -250);
                            Kaboom(Main.projectile[c]);
                            int d = Projectile.NewProjectile(player.position + new Vector2(0, 250), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[d].Center = player.Center + new Vector2(0, 250);
                            Kaboom(Main.projectile[d]);
                            int e = Projectile.NewProjectile(player.position + new Vector2(-200, 200), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[e].Center = player.Center + new Vector2(-200, 200);
                            Kaboom(Main.projectile[e]);
                            int f = Projectile.NewProjectile(player.position + new Vector2(200, 200), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[f].Center = player.Center + new Vector2(200, 200);
                            Kaboom(Main.projectile[f]);
                            int g = Projectile.NewProjectile(player.position + new Vector2(200, -200), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[g].Center = player.Center + new Vector2(200, -200);
                            Kaboom(Main.projectile[g]);
                            int h = Projectile.NewProjectile(player.position + new Vector2(-200, -200), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[h].Center = player.Center + new Vector2(-200, -200);
                            Kaboom(Main.projectile[h]);
                            int a1 = Projectile.NewProjectile(player.position + new Vector2(-500, 0), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[a1].Center = player.Center + new Vector2(-500, 0);
                            Kaboom(Main.projectile[a1]);
                            int b1 = Projectile.NewProjectile(player.position + new Vector2(500, 0), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[b1].Center = player.Center + new Vector2(500, 0);
                            Kaboom(Main.projectile[b1]);
                            int c1 = Projectile.NewProjectile(player.position + new Vector2(0, -500), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[c1].Center = player.Center + new Vector2(0, -500);
                            Kaboom(Main.projectile[c1]);
                            int d1 = Projectile.NewProjectile(player.position + new Vector2(0, 500), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[d1].Center = player.Center + new Vector2(0, 500);
                            Kaboom(Main.projectile[d1]);
                            int e1 = Projectile.NewProjectile(player.position + new Vector2(-400, 400), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[e1].Center = player.Center + new Vector2(-400, 400);
                            Kaboom(Main.projectile[e1]);
                            int f1 = Projectile.NewProjectile(player.position + new Vector2(400, 400), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[f1].Center = player.Center + new Vector2(400, 400);
                            Kaboom(Main.projectile[f1]);
                            int g1 = Projectile.NewProjectile(player.position + new Vector2(400, -400), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[g1].Center = player.Center + new Vector2(400, -400);
                            Kaboom(Main.projectile[g1]);
                            int h1 = Projectile.NewProjectile(player.position + new Vector2(-400, -400), Vector2.Zero, mod.ProjectileType("AnubisFireball"), npc.damage / 2, 7, Main.myPlayer);
                            Main.projectile[h1].Center = player.Center + new Vector2(-400, -400);
                            Kaboom(Main.projectile[h1]);
                        }
                    }
                    break;

                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
            NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<AAMod.NPCs.TownNPCs.Anubis>());

            if (!AAWorld.downedAnubisA)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("FAnubisWin"), Color.ForestGreen);
            }

            AAWorld.downedAnubisA = true;

            if (Main.rand.Next(10) == 0)
            {
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("FAnubisTrophy"));
            }

            if (Main.expertMode)
            {
                for(int i = 0; i < 10; i++) npc.DropBossBags();
                if (!AAWorld.downedAnubisA) npc.DropItemInstanced(npc.position, new Vector2(npc.width, npc.height), ModContent.ItemType<Items.EXSoul>(), 1, true);
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("FAnubisMask"));
                }
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("SoulFragment"), Main.rand.Next(8, 16));
                string[] lootTable = { "Verdict", "Lifeline", "ForsakenStaff", "Soulsplitter", "CursedFury", "HorusCane" };
                int loot = Main.rand.Next(lootTable.Length);
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType(lootTable[loot]));
            }
        }

        int deathtimer = 0;

        public bool AliveCheck(Player player)
        {
            if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 5000f || !player.ZoneDesert)
            {
                npc.TargetClosest();
                if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 5000f || !player.ZoneDesert)
                {
                    deathtimer++;
                    if (Main.netMode != NetmodeID.MultiplayerClient && deathtimer > 240)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("FAnubis"), Color.ForestGreen);
                        int a = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<AAMod.NPCs.TownNPCs.Anubis>());
                        Main.npc[a].Center = npc.Center;
                        npc.active = false;
                    }
                    return false;
                }
                else
                {
                    deathtimer = 0;
                }
            }
            deathtimer = 0;
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter > 7)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.ai[0] == 2 && npc.ai[1] >= 120)
                {
                    if (npc.frame.Y > frameHeight * 11 || npc.frame.Y < frameHeight * 6 )
                    {
                        npc.frame.Y = frameHeight * 6;
                    }
                }
                else
                {
                    if (npc.frame.Y > frameHeight * 5)
                    {
                        npc.frame.Y = 0;
                    }
                }
            }
        }

        public void Teleport()
        {
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
                int num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<ForsakenDust>(), 0f, 0f, 50, default, 3.7f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<ForsakenDust>(), 0f, 0f, 25, default, 1.5f);
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

            Player player = Main.player[npc.target];
            Vector2 targetPos = player.Center;
            int posX = Main.rand.Next(-800, 800);

            int posY = Main.rand.Next(0, 800);
            if (posX > -400 && posX < 400)
            {
                 posY = Main.rand.Next(400, 400);
            }

            npc.position = new Vector2(targetPos.X + posX, targetPos.Y - posY);
            int pieCut = 20;
            Main.PlaySound(SoundID.Item14, npc.position);
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(npc.Center.X - 1, npc.Center.Y - 1), 2, 2, ModContent.DustType<JudgementDust>(), 0f, 0f, 100, Color.White, 1.6f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(6f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(npc.Center.X - 1, npc.Center.Y - 1), 2, 2, ModContent.DustType<JudgementDust>(), 0f, 0f, 100, Color.White, 2f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(9f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
        }

        public void Kaboom(Projectile p)
        {
            Vector2 position = p.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, 240, 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 15; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<ForsakenDust>(), 0f, 0f, 50, default, 3.7f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<ForsakenDust>(), 0f, 0f, 25, default, 1.5f);
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
        }

        public void ScepterTeleport()
        {
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
                int num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<ForsakenDust>(), 0f, 0f, 50, default, 3.7f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<ForsakenDust>(), 0f, 0f, 25, default, 1.5f);
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

            Vector2 targetPos = Main.player[npc.target].Center;
            targetPos.X += 300 * (npc.Center.X < targetPos.X ? 1 : -1);
            targetPos.Y -= 300;
            npc.position = targetPos;

            int pieCut = 20;
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(npc.Center.X - 1, npc.Center.Y - 1), 2, 2, ModContent.DustType<JudgementDust>(), 0f, 0f, 100, Color.White, 1f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(6f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(npc.Center.X - 1, npc.Center.Y - 1), 2, 2, ModContent.DustType<JudgementDust>(), 0f, 0f, 100, Color.White, 1.5f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(9f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
        }
    }
}