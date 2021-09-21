using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using System.IO;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Athena.Olympian
{
    [AutoloadBossHead]
    public class AthenaA : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Olympian Athena");
            Main.npcFrameCount[npc.type] = 7;
        }

        public int damage = 0;

        public static Point CloudPoint = new Point((int)(Main.maxTilesX * 0.65f), 100);
        public Vector2 Origin = new Vector2((int)(Main.maxTilesX * 0.65f), 100) * 16;

        public override void SetDefaults()
        {
            npc.width = 152;
            npc.height = 114;
            npc.value = BaseUtility.CalcValue(0, 10, 0, 0);
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.lifeMax = 140000;
            npc.defense = 80;
            npc.damage = 110;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.boss = true;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/AthenaA");
            npc.noTileCollide = true;
            bossBag = ModLoader.GetMod("AAMod").ItemType("AthenaABag");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public float[] internalAI = new float[4];

        public float poschange = 1;

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
                writer.Write(poschange);
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
                poschange = reader.ReadFloat();
            }
        }

        public int timer = 0;

        public override void AI()
        {
            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }
            Player player = Main.player[npc.target];

            if (internalAI[2] < 1 && npc.life < npc.lifeMax / 2 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("AthenaDark"));
                NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("AthenaLight"));
                internalAI[2] ++;
                npc.netUpdate = true;
            }
            else if(internalAI[2] < 2 && npc.life < npc.lifeMax / 3 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                timer ++;
                if(timer == 120)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("No!! I don't want to lose.", Color.CornflowerBlue);
                    npc.netUpdate = true;
                }
                if(timer == 240)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("My sister from the Varia World will help me!", Color.CornflowerBlue);
                    npc.netUpdate = true;
                }
                if(timer == 360)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("The Varia Fallen Angle!", Color.CornflowerBlue);
                    npc.netUpdate = true;
                    Vector2 Origin = new Vector2((int)(Main.maxTilesX * 0.65f), 100) * 16;
                    Vector2 Acropolis = new Vector2(Origin.X + (80 * 16), Origin.Y + (79 * 16));
                    Main.PlaySound(SoundID.Item, (int)Acropolis.X, (int)Acropolis.Y, 14);
                    int p = NPC.NewNPC((int)Acropolis.X, (int)Acropolis.Y, mod.NPCType("FallenAngleSummonRune"));
                    Main.npc[p].Center = Acropolis;
                    Main.npc[p].ai[1] = (float)npc.whoAmI;
                    internalAI[2] ++;
                }
            }

            Vector2 targetPos;

            switch ((int)npc.ai[0])
            {
                case 0:
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 500 * poschange;
                    
                    for(int pos = -200; pos < 200; pos += 50)
                    {
                        targetPos.Y = player.Center.Y - pos;
                        if(Collision.CanHit(targetPos, npc.width, npc.height, player.position, player.width, player.height))
                        {
                            break;
                        }
                    }
                    MoveToVector2(targetPos);

                    if(Math.Abs(npc.Center.X - targetPos.X) + Math.Abs(npc.Center.Y - targetPos.Y) < 70f)
                    {
                        int proj = BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, mod.ProjectileType("AthenaMagic"), ref npc.ai[1], 30, npc.damage / 2, 10, false);
                        if(proj > -1)
                        {
                            poschange = -poschange;
                            Teleport(2);
                        }
                    }

                    if (internalAI[3]++ >= 250 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int Choice = Main.rand.Next(2);
                        if (Choice == 0)
                        {
                            NPC.NewNPC((int)npc.Center.X + 100, (int)npc.Center.Y, mod.NPCType("OlympianDragon"));
                            NPC.NewNPC((int)npc.Center.X - 100, (int)npc.Center.Y, mod.NPCType("OlympianDragon"));
                        }
                        else
                        {
                            NPC Seraph1 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + 150, mod.NPCType("SeraphA"))];
                            for (int i = 0; i < 3; i++)
                            {
                                Dust.NewDust(Seraph1.position, Seraph1.height, Seraph1.width, ModContent.DustType<Feather>(), Main.rand.Next(-1, 2), 1, 0);
                            }
                            NPC Seraph2 = Main.npc[NPC.NewNPC((int)npc.Center.X + 150, (int)npc.Center.Y - 75, mod.NPCType("SeraphA"))];
                            for (int i = 0; i < 3; i++)
                            {
                                Dust.NewDust(Seraph2.position, Seraph2.height, Seraph2.width, ModContent.DustType<Feather>(), Main.rand.Next(-1, 2), 1, 0);
                            }
                            NPC Seraph3 = Main.npc[NPC.NewNPC((int)npc.Center.X + 150, (int)npc.Center.Y - 75, mod.NPCType("SeraphA"))];
                            for (int i = 0; i < 3; i++)
                            {
                                Dust.NewDust(Seraph3.position, Seraph3.height, Seraph3.width, ModContent.DustType<Feather>(), Main.rand.Next(-1, 2), 1, 0);
                            }
                        }
                        internalAI[3] = 0;
                        npc.netUpdate = true;
                    }
                    if (npc.ai[2]++ > 560)
                    {
                        Teleport(1);
                        poschange = npc.Center.X - player.Center.X > 0? 1 : -1;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }
                    break;
                case 1:
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 500 * poschange;

                    for(int pos = -200; pos < 200; pos += 50)
                    {
                        targetPos.Y = player.Center.Y - pos;
                        if(Collision.CanHit(targetPos, npc.width, npc.height, player.position, player.width, player.height))
                        {
                            break;
                        }
                    }

                    MoveToVector2(targetPos);

                    if(Math.Abs(npc.Center.X - targetPos.X) + Math.Abs(npc.Center.Y - targetPos.Y) < 70f)
                    {
                        int proj2 = BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, mod.ProjectileType("SwiftwindStrikeSpear"), ref npc.ai[1], 30, npc.damage / 2, 10, false);

                        if(proj2 > -1)
                        {
                            poschange = -poschange;
                            Teleport(2);
                        }
                    }

                    if (npc.ai[2]++ > 400)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }

                    break;
                case 2:
                    if (!AliveCheck(player))
                        break;

                    npc.velocity *= 0;
                    if (npc.ai[2] == 0)
                    {
                        Teleport(0);
                    }

                    npc.ai[2]++;

                    if (npc.ai[2] == 120)
                    {
                        int projType = mod.ProjectileType("RazorGust");
                        float spread = 45f * 0.0174f;
                        Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                        dir *= 14f;
                        float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                        double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                        double deltaAngle = spread / 6f;
                        for (int i = 0; i < 3; i++)
                        {
                            double offsetAngle = startAngle + (deltaAngle * i);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), projType, npc.damage / 2, 5, Main.myPlayer);
                        }
                    }
                    if (npc.ai[2] == 180 && npc.life < npc.lifeMax / 2)
                    {
                        int projType = mod.ProjectileType("RazorGust");
                        float spread = 45f * 0.0174f;
                        Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                        dir *= 14f;
                        float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                        double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                        double deltaAngle = spread / 6f;
                        for (int i = 0; i < 3; i++)
                        {
                            double offsetAngle = startAngle + (deltaAngle * i);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), projType, npc.damage / 2, 5, Main.myPlayer);
                        }
                    }
                    if (npc.ai[2] > 220)
                    {
                        if (npc.ai[3] < Repeats())
                        {
                            npc.ai[2] = 0;
                            npc.ai[3]++;
                        }
                        else
                        {
                            Teleport(3);
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = 0;
                        }
                    }
                    break;
                case 3:

                    npc.ai[1]++;

                    targetPos = player.Center;
                    targetPos.Y -= 500;
                    MoveToVector2(targetPos);

                    if (npc.ai[1] == 120)
                    {
                        int a = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(8f, -8f), mod.ProjectileType("RuneSpawn"), npc.damage / 2, 3);
                        Main.projectile[a].Center = npc.Center;
                        int b = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(8f, 8f), mod.ProjectileType("RuneSpawn"), npc.damage / 2, 3);
                        Main.projectile[b].Center = npc.Center;
                        int c = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(-8f, 8f), mod.ProjectileType("RuneSpawn"), npc.damage / 2, 3);
                        Main.projectile[c].Center = npc.Center;
                        int d = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(-8f, -8f), mod.ProjectileType("RuneSpawn"), npc.damage / 2, 3);
                        Main.projectile[d].Center = npc.Center;
                    }
                    if (npc.ai[1] > 180)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }
                    break;
                case 4: //prepare for queen bee dashes
                    if (!AliveCheck(player))
                        break;
                    if (++npc.ai[1] > 30)
                    {
                        targetPos = player.Center;
                        targetPos.X += 1000 * (npc.Center.X < targetPos.X ? -1 : 1);
                        DashMovement(targetPos, 0.8f);
                        if (npc.ai[1] > 180 || Math.Abs(npc.Center.Y - targetPos.Y) < 50) //initiate dash
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.netUpdate = true;
                            npc.velocity.X = -30 * (npc.Center.X < player.Center.X ? -1 : 1);
                            npc.velocity.Y *= 0.1f;
                        }
                    }
                    else
                    {
                        npc.velocity *= 0.9f; //decelerate briefly
                    }
                    npc.rotation = 0;
                    break;

                case 5: //dashing
                    npc.spriteDirection = npc.velocity.X > 0 ? -1 : 1;
                    if (++npc.ai[1] > 240 || (Math.Sign(npc.velocity.X) > 0 ? npc.Center.X > player.Center.X + 900 : npc.Center.X < player.Center.X - 900))
                    {
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        if (++npc.ai[3] >= Repeats()) //repeat dash three times
                        {
                            npc.ai[0]++;
                            npc.ai[3] = 0;
                        }
                        else
                            npc.ai[0]--;
                        npc.netUpdate = true;
                    }
                    break;
                default:
                    npc.spriteDirection = npc.direction;
                    poschange = npc.Center.X - player.Center.X > 0? 1 : -1;
                    npc.ai[0] = 0;
                    goto case 0;

            }

            if (player.Center.X < npc.Center.X + 200)
            {
                npc.direction = -1;
            }
            else
            {
                npc.direction = 1;
            }

            npc.rotation = 0;
        }

        public int Repeats()
        {
            if (npc.life < npc.lifeMax * (2/3))
            {
                return 5;
            }
            else if (npc.life < npc.lifeMax / 3)
            {
                return 4;
            }
            else
            {
                return 3;
            }
        }

        public void Teleport(int where)
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
                int num88 = Dust.NewDust(position, num84, height3, DustID.Electric, 0f, 0f, 50, default, 3.7f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = false;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, DustID.Electric, 0f, 0f, 25, default, 1.5f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = false;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Black * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * 8f;
            }
            for (int num89 = 0; num89 < 10; num89++)
            {
                int num90 = Dust.NewDust(position, num84, height3, DustID.Electric, 0f, 0f, 0, default, 2.7f);
                Main.dust[num90].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = false;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += npc.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 30; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, DustID.Electric, 0f, 0f, 0, default, 1.5f);
                Main.dust[num92].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = false;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += npc.DirectionTo(Main.dust[num92].position) * 3f;
            }
            if (where == 0)
            {
                npc.Center = CloudPick();
            }
            else if (where == 1)
            {
                Vector2 targetPos = Main.player[npc.target].Center;
                targetPos.X += 500 * (npc.Center.X < targetPos.X ? 1 : -1);
                targetPos.Y -= 200;
                npc.position = targetPos;
            }
            else if (where == 2)
            {
                Vector2 targetPos = Main.player[npc.target].Center;
                targetPos.X += 500 * poschange;
                targetPos.Y -= 200;
                npc.position = targetPos;
            }
            else
            {
                npc.position = new Vector2(Origin.X + (79 * 16), Origin.Y + (79 * 16));
            }

            position = npc.Center + (Vector2.One * -20f);
            num84 = 40;
            height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, 240, 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 15; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, DustID.Electric, 0f, 0f, 50, default, 3.7f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, DustID.Electric, 0f, 0f, 25, default, 1.5f);
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
                int num90 = Dust.NewDust(position, num84, height3, DustID.Electric, 0f, 0f, 0, default, 2.7f);
                Main.dust[num90].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += npc.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 30; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, DustID.Electric, 0f, 0f, 0, default, 1.5f);
                Main.dust[num92].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += npc.DirectionTo(Main.dust[num92].position) * 3f;
            }
        }


        public Vector2 CloudPick()
        {
            int CloudChoice = Main.rand.Next(12);
            Vector2 Cloud1 = new Vector2(Origin.X + (79 * 16), Origin.Y + (10 * 16));
            Vector2 Cloud2 = new Vector2(Origin.X + (112 * 16), Origin.Y + (19 * 16));
            Vector2 Cloud3 = new Vector2(Origin.X + (135 * 16), Origin.Y + (40 * 16));
            Vector2 Cloud4 = new Vector2(Origin.X + (140 * 16), Origin.Y + (69 * 16));
            Vector2 Cloud5 = new Vector2(Origin.X + (135 * 16), Origin.Y + (99 * 16));
            Vector2 Cloud6 = new Vector2(Origin.X + (112 * 16), Origin.Y + (120 * 16));
            Vector2 Cloud7 = new Vector2(Origin.X + (79 * 16), Origin.Y + (129 * 16));
            Vector2 Cloud8 = new Vector2(Origin.X + (46 * 16), Origin.Y + (120 * 16));
            Vector2 Cloud9 = new Vector2(Origin.X + (23 * 16), Origin.Y + (99 * 16));
            Vector2 Cloud10 = new Vector2(Origin.X + (18 * 16), Origin.Y + (69 * 16));
            Vector2 Cloud11 = new Vector2(Origin.X + (23 * 16), Origin.Y + (40 * 16));
            Vector2 Cloud12 = new Vector2(Origin.X + (46 * 16), Origin.Y + (19 * 16));
            if (CloudChoice == 1)
            {
                return Cloud2;
            }
            else if (CloudChoice == 2)
            {
                return Cloud3;
            }
            else if (CloudChoice == 3)
            {
                return Cloud4;
            }
            else if (CloudChoice == 4)
            {
                return Cloud5;
            }
            else if (CloudChoice == 5)
            {
                return Cloud6;
            }
            else if (CloudChoice == 6)
            {
                return Cloud7;
            }
            else if (CloudChoice == 7)
            {
                return Cloud8;
            }
            else if (CloudChoice == 8)
            {
                return Cloud9;
            }
            else if (CloudChoice == 9)
            {
                return Cloud10;
            }
            else if (CloudChoice == 10)
            {
                return Cloud11;
            }
            else if (CloudChoice == 11)
            {
                return Cloud12;
            }
            else
            {
                return Cloud1;
            }

        }

        private bool AliveCheck(Player player)
        {
            AAMod.AAPlayer modPlayer = player.GetModPlayer<AAMod.AAPlayer>();
            Vector2 Acropolis = new Vector2(Origin.X + (79 * 16), Origin.Y + (79 * 16));
            if (player.dead || !player.active || Vector2.Distance(npc.position, player.position) > 6000 || !modPlayer.ZoneAcropolis || Vector2.Distance(Acropolis, player.position) > 1500)
            {
                npc.TargetClosest();
                if (player.dead || !player.active || Math.Abs(Vector2.Distance(npc.position, player.position)) > 6000 || !modPlayer.ZoneAcropolis || Vector2.Distance(Acropolis, player.position) > 1500)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("AthenaA1"), Color.CornflowerBlue);
                    int p = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("AthenaFlee"));
                    Main.npc[p].Center = npc.Center;
                    npc.active = false;
                    npc.netUpdate = true;
                    return false;
                }
            }
            if (npc.timeLeft < 600)
                npc.timeLeft = 600;
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 6)
            {
                npc.frame.Y += frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 7)
            {
                npc.frame.Y = 0;
            }
        }

        public void MoveToVector2(Vector2 p)
        {
            float moveSpeed = 16f;
            float velMultiplier = 1f;
            Vector2 dist = p - npc.Center;
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

        private void DashMovement(Vector2 targetPos, float speedModifier)
        {
            if (npc.Center.X < targetPos.X)
            {
                npc.velocity.X += speedModifier;
                if (npc.velocity.X < 0)
                    npc.velocity.X += speedModifier * 2;
            }
            else
            {
                npc.velocity.X -= speedModifier;
                if (npc.velocity.X > 0)
                    npc.velocity.X -= speedModifier * 2;
            }
            if (npc.Center.Y < targetPos.Y)
            {
                npc.velocity.Y += speedModifier;
                if (npc.velocity.Y < 0)
                    npc.velocity.Y += speedModifier * 2;
            }
            else
            {
                npc.velocity.Y -= speedModifier;
                if (npc.velocity.Y > 0)
                    npc.velocity.Y -= speedModifier * 2;
            }
            if (Math.Abs(npc.velocity.X) > 30)
                npc.velocity.X = 30 * Math.Sign(npc.velocity.X);
            if (Math.Abs(npc.velocity.Y) > 30)
                npc.velocity.Y = 30 * Math.Sign(npc.velocity.Y);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void NPCLoot()
        {
            if (!AAWorld.downedAthenaA)
            {
                int p = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("AthenaDefeat"), 0, 0, 0, 1);
                Main.npc[p].Center = npc.Center;
            }
            else
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("AthenaA2"), Color.CornflowerBlue);
                int p = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("AthenaFlee"));
                Main.npc[p].Center = npc.Center;
            }
            if(Main.expertMode)
            {
                for(int i = 0; i < 10; i++) npc.DropBossBags();
                if (!AAWorld.downedAthenaA) npc.DropItemInstanced(npc.position, new Vector2(npc.width, npc.height), ModContent.ItemType<Items.EXSoul>(), 1, true);
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("AthenaAMask"));
                }
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("GoddessFeather"), Main.rand.Next(20, 25));
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("SkyCrystal"), Main.rand.Next(25, 40));
                string[] lootTable = { "HurricaneStone", "Olympia", "Windfury", "GaleForce" };
                int loot = Main.rand.Next(lootTable.Length);
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType(lootTable[loot]));
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("StarChart"));
            }
            AAWorld.downedAthenaA = true;
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D tex = Main.npcTexture[npc.type];
            Texture2D tex2 = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/AthenaA_Glow");
            Texture2D tex3 = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/AthenaA_Glow1");
            Color lightColor = BaseDrawing.GetLightColor(npc.Center);
            BaseDrawing.DrawAfterimage(sb, tex, 0, npc.position, npc.width, npc.height, npc.oldPos, npc.scale, npc.rotation, npc.direction, 7, npc.frame, 1f, 1f, 5, false, 0f, 0f, Color.CornflowerBlue);
            BaseDrawing.DrawTexture(sb, tex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 7, npc.frame, lightColor);
            BaseDrawing.DrawTexture(sb, tex2, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 7, npc.frame, AAColor.Flash);
            BaseDrawing.DrawTexture(sb, tex3, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 7, npc.frame, Color.White);
            return false;
        }
    }
}