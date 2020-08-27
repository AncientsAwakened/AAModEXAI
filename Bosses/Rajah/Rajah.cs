using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using System.IO;
using AAMod.Items.Boss.Rajah;
using Terraria.Graphics.Shaders;
using AAModEXAI.Bosses.Rajah.Supreme;
using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Rajah
{
    [AutoloadBossHead]
    public class Rajah : ModNPC
    {
        public override string Texture => "AAModEXAI/Bosses/Rajah/Rajah";
        public int damage = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rajah Rabbit");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.width = 130;
            npc.height = 220;
            npc.aiStyle = -1;
            npc.damage = 130;
            npc.defense = 90;
            npc.lifeMax = 65000;
            npc.knockBackResist = 0f;
            npc.npcSlots = 1000f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/Rajah");
            npc.value = Item.sellPrice(0, 1, 10, 0);
            npc.boss = true;
            npc.netAlways = true;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/RajahTheme");
            bossBag = ModLoader.GetMod("AAMod").ItemType("RajahBag");
        }

        public bool isSupreme = false;
        public float[] internalAI = new float[6];
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
                writer.Write(isSupreme);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat(); //SpaceOctopus AI stuff
                internalAI[1] = reader.ReadFloat(); //Is Flying
                internalAI[2] = reader.ReadFloat(); //Is Jumping
                internalAI[3] = reader.ReadFloat(); //Minion/Rocket Timer
                internalAI[4] = reader.ReadFloat(); //JumpFlyControl and Vertical dash
                isSupreme = reader.ReadBool();
            }
        }

        private Texture2D RajahTex;
        private Texture2D Glow;
        private Texture2D SupremeGlow;
        private Texture2D SupremeEyes;
        private Texture2D ArmTex;
        public int WeaponFrame = 0;
        public Vector2 MovePoint;
        public bool SelectPoint = false;

        /*
         * npc.ai[0] = Jump Timer
         * npc.ai[1] = Ground Minion Alternation
         * npc.ai[2] = Weapon Change timer
         * npc.ai[3] = Weapon type
         */

        public int roarTimer = 0;
        public int roarTimerMax = 240;
        public bool Roaring => roarTimer > 0;

        public void Roar(int timer)
        {
            roarTimer = timer;
            Main.PlaySound(ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Rajah"), npc.Center);
        }

        public Vector2 WeaponPos;
        public Vector2 StaffPos;

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (isSupreme)
            {
                damage *= .7f;
            }
            return true;
        }

        public float ProjSpeed()
        {
            if (npc.life < (npc.lifeMax * .85f)) //The lower the health, the more damage is done
            {
                return isSupreme ? 15f : 11f;
            }
            if (npc.life < (npc.lifeMax * .7f))
            {
                return isSupreme ? 16f : 12f;
            }
            if (npc.life < (npc.lifeMax * .65f))
            {
                return isSupreme ? 17f : 13f;
            }
            if (npc.life < (npc.lifeMax * .4f))
            {
                return isSupreme ? 18f : 14f;
            }
            if (npc.life < (npc.lifeMax * .25f))
            {
                return isSupreme ? 19f : 15f;
            }
            if (npc.life < (npc.lifeMax * .1f))
            {
                return isSupreme ? 20f : 16f;
            }
            return isSupreme ? 14f : 10f;
        }

        private bool SayLine = false;
        private bool DefenseLine = false;

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
            AAModGlobalNPC.Rajah = npc.whoAmI;
            WeaponPos = new Vector2(npc.Center.X + (npc.direction == 1 ? -78 : 78), npc.Center.Y - 9);
            StaffPos = new Vector2(npc.Center.X + (npc.direction == 1 ? 78 : -78), npc.Center.Y - 9);
            if (Roaring) roarTimer--;

            if (Main.netMode != NetmodeID.MultiplayerClient && npc.type == mod.NPCType("SupremeRajah") && isSupreme == false)
            {
                isSupreme = true;
                npc.netUpdate = true;
            }

            if (isSupreme)
            {
                if (npc.ai[3] != 0 && !DefenseLine && !AAModEXAIWorld.downedRajahsRevenge && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    DefenseLine = true;
                    BaseUtility.Chat(AAMod.Lang.BossChat("SupremeRajahChat"), Color.MediumPurple);

                }
                if (npc.life <= npc.lifeMax / 7 && !SayLine && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    SayLine = true;
                    string Name;

                    int bunnyKills = NPC.killCount[Item.NPCtoBanner(NPCID.Bunny)];
                    if (bunnyKills >= 100 && !AAModEXAIWorld.downedRajahsRevenge)
                    {
                        Name = "MUDERER";
                    }
                    else
                    {
                        if (Main.netMode != NetmodeID.SinglePlayer)
                        {
                            Name = "Terrarians";
                        }
                        else
                        {
                            Name = Main.LocalPlayer.name;
                        }
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("Rajah5") + Name.ToUpper() + AAMod.Lang.BossChat("Rajah6"), 107, 137, 179);
                    music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/LastStand");
                }
            }

            Player player = Main.player[npc.target];
            if (npc.target >= 0 && Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
                if (Main.player[npc.target].dead)
                {
                    if (isSupreme)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("Rajah7"), 107, 137, 179);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.position, npc.velocity, mod.ProjectileType("SupremeRajahBookIt"), damage, 0, Main.myPlayer);
                        }
                    }
                    else
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("Rajah2"), 107, 137, 179);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.position, npc.velocity, mod.ProjectileType("RajahBookIt"), damage, 0, Main.myPlayer);
                        }
                    }
                    npc.active = false;
                    npc.noTileCollide = true;
                    npc.netUpdate = true;
                    return;
                }
            }

            if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) + Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) > 10000)
            {
                npc.TargetClosest(true);
                if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) + Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) > 10000)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("Rajah3"), 107, 137, 179);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (isSupreme)
                        {
                            Projectile.NewProjectile(npc.position, npc.velocity, mod.ProjectileType("SupremeRajahBookIt"), damage, 0, Main.myPlayer); //Originally 100 damage
                        }
                        else
                        {
                            Projectile.NewProjectile(npc.position, npc.velocity, mod.ProjectileType("RajahBookIt"), damage, 0, Main.myPlayer);
                        }
                    }
                    npc.active = false;
                    npc.noTileCollide = true;
                    npc.netUpdate = true;
                    return;
                }
            }


            if (player.Center.X < npc.Center.X)
            {
                npc.direction = 1;
            }
            else
            {
                npc.direction = -1;
            }

            if (internalAI[4] == 0)
            {
                if(player.Center.Y + player.height / 2 < npc.Center.Y + npc.height / 2 - 30f || Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) + Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) > 2000 || isDashing)
                {
                    npc.noTileCollide = true;
                    npc.noGravity = true;
                    internalAI[4] = 2f;
                    npc.ai[0] = 0;
                    return;
                }
                else
                {
                    npc.noTileCollide = true;
                    npc.noGravity = false;
                    isDashing = false;
                    JumpAI();
                }
            }
            else if(internalAI[4] == 1f)
            {
                npc.noTileCollide = true;
                npc.noGravity = true;
                isDashing = false;
                if (player.Center.Y + player.height / 2 <= npc.Center.Y + npc.height / 2 + 20f) 
                {
                    if(npc.collideY && npc.velocity.Y > 0)
                    {
                        Main.PlaySound(SoundID.Item14, npc.position);
                        for (int num622 = (int)npc.position.X - 20; num622 < (int)npc.position.X + npc.width + 40; num622 += 20)
                        {
                            for (int num623 = 0; num623 < 4; num623++)
                            {
                                int num624 = Dust.NewDust(new Vector2(npc.position.X - 20f, npc.position.Y + npc.height), npc.width + 20, 4, 31, 0f, 0f, 100);
                                Main.dust[num624].velocity *= 0.2f;
                            }
                            Projectile.NewProjectile(num622 - 20, npc.position.Y + npc.height - 8f, 0, 0, mod.ProjectileType("RajahStomp"), damage, 6, Main.myPlayer, 0, 0);
                            int num625 = Gore.NewGore(new Vector2(num622 - 20, npc.position.Y + npc.height - 8f), default, Main.rand.Next(61, 64), 1f);
                            Main.gore[num625].velocity *= 0.4f;
                        }
                    }
                    npc.noTileCollide = false;
                    npc.velocity.X *= .2f;
                    npc.velocity.Y = -2f;
                    internalAI[4] = 0f;
                    npc.ai[0] = 0;
                    npc.netUpdate = true;
                    return;
                }
                if(Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) > 1000)
                {
                    npc.noTileCollide = true;
                    npc.noGravity = true;
                    internalAI[4] = 2f;
                    npc.ai[0] = 0;
                }
            }
            else if(internalAI[4] == 2f)
            {
                npc.noTileCollide = true;
                npc.noGravity = true;
                FlyAI();
                if(Math.Abs(npc.Center.X - player.Center.X) < 50f && player.position.Y > npc.Center.Y + npc.height / 2)
                {
                    internalAI[4] = 3f;
                    npc.netUpdate = true;
                }
            }
            else if(internalAI[4] == 3f)
            {
                npc.noTileCollide = true;
                npc.noGravity = true;
                isDashing = true;
                if(player.velocity.X == 0)
                {
                    npc.velocity = (player.Center - npc.Center) * .06f;
                }
                else
                {
                    npc.velocity = (player.Center + new Vector2(100f * (player.velocity.X > 0? 1 : -1), 0) - npc.Center) * .06f;
                }
                npc.velocity = Vector2.Normalize(npc.velocity) * 26f;
                if(npc.velocity.X > 10f) npc.velocity.X = 10f;
                internalAI[0] = 0f;
                internalAI[4] = 1f;
            }
            else if(internalAI[4] == 4f)
            {
                npc.noTileCollide = true;
                npc.noGravity = false;
                isDashing = false;
                if (player.Center.Y + player.height / 2 <= npc.Center.Y + npc.height / 2 + 20f) 
                {
                    internalAI[0] = 0f;
                    internalAI[4] = 1f;
                }
            }

            if (npc.target <= 0 || npc.target == 255 || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.ai[2]++;
                internalAI[3]++;
            }
            if (npc.ai[2] >= 500)
            {
                internalAI[3] = 0;
                npc.ai[2] = 0;
                npc.ai[3] = 0;
                npc.netUpdate = true;
            }
            else if (npc.ai[3] == 0 && npc.ai[2] >= ChangeRate())
            {
                if (Main.rand.Next(5) == 0)
                {
                    Roar(roarTimerMax);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    internalAI[3] = 0;
                    npc.ai[2] = 0;
                    if (ModSupport.GetMod("ThoriumMod") != null && Main.rand.Next(7) == 0)
                    {
                        npc.ai[3] = 7;
                    }
                    else
                    {
                        if (isSupreme)
                        {
                            npc.ai[3] = Main.rand.Next(7);
                        }
                        else
                        {
                            npc.ai[3] = Main.rand.Next(4);
                        }
                    }
                }
                npc.netUpdate = true;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.ai[3] == 0) //Minion Phase
                {
                    if (internalAI[3] >= 80)
                    {
                        internalAI[3] = 0;
                        if (internalAI[1] == 0)
                        {
                            if (NPC.CountNPCS(mod.NPCType("RabbitcopterSoldier")) + AAGlobalProjectile.CountProjectiles(mod.ProjectileType("BunnySummon1")) < 5)
                            {
                                Projectile.NewProjectile(StaffPos, Vector2.Zero, mod.ProjectileType("BunnySummon1"), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 200, (int)npc.Center.X + 200), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                Projectile.NewProjectile(StaffPos, Vector2.Zero, mod.ProjectileType("BunnySummon1"), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 200, (int)npc.Center.X + 200), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                Projectile.NewProjectile(StaffPos, Vector2.Zero, mod.ProjectileType("BunnySummon1"), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 200, (int)npc.Center.X + 200), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                            }
                            npc.netUpdate = true;
                        }
                        else
                        {
                            if (npc.ai[1] > 2)
                            {
                                npc.ai[1] = 0;
                            }
                            if (npc.ai[1] == 0)
                            {
                                if (NPC.CountNPCS(mod.NPCType("RabbitcopterSoldier")) + AAGlobalProjectile.CountProjectiles(mod.ProjectileType("BunnySummon1")) < 5)
                                {
                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, mod.ProjectileType("BunnySummon1"), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, mod.ProjectileType("BunnySummon1"), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, mod.ProjectileType("BunnySummon1"), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                }
                            }
                            else if (npc.ai[1] == 1)
                            {
                                if (NPC.CountNPCS(mod.NPCType("BunnyBrawler")) + AAGlobalProjectile.CountProjectiles(mod.ProjectileType("BunnySummon2")) < 5)
                                {
                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, mod.ProjectileType("BunnySummon2"), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, mod.ProjectileType("BunnySummon2"), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                }
                            }
                            else if (npc.ai[1] == 2)
                            {
                                if (NPC.CountNPCS(mod.NPCType("BunnyBattler")) + AAGlobalProjectile.CountProjectiles(mod.ProjectileType("BunnySummon3")) < 8)
                                {
                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, mod.ProjectileType("BunnySummon3"), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));

                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, mod.ProjectileType("BunnySummon3"), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));

                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, mod.ProjectileType("BunnySummon3"), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));

                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, mod.ProjectileType("BunnySummon3"), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                }
                            }
                            npc.ai[1] += 1;
                            npc.netUpdate = true;
                        }
                    }
                }
                else if (npc.ai[3] == 1) //Bunzooka
                {
                    if (internalAI[3] > 40)
                    {
                        internalAI[3] = 0;
                        int Rocket = isSupreme ? mod.ProjectileType("RajahRocketEXR") : mod.ProjectileType("RajahRocket");
                        Vector2 shoot = PredictPlayerMovement(ProjSpeed(), player);
                        shoot.Normalize();
                        shoot *= ProjSpeed();
                        Projectile.NewProjectile(WeaponPos.X, WeaponPos.Y, shoot.X, shoot.Y, Rocket, damage, 5, Main.myPlayer);
                        npc.netUpdate = true;
                    }
                }
                else if (npc.ai[3] == 2) //Royal Scepter
                {
                    int carrots = isSupreme ? 5 : 3;
                    int carrotType = isSupreme ? mod.ProjectileType("CarrotEXR") : mod.ProjectileType("CarrotHostile");
                    float spread = 45f * 0.0174f * .5f;
                    Vector2 dir = PredictPlayerMovement(ProjSpeed() + (isSupreme? 3 : 1), player);
                    dir.Normalize();
                    dir *= ProjSpeed() + (isSupreme? 3 : 1);
                    float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                    double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                    double deltaAngle = spread / carrots * 2;
                    if (internalAI[3] > 40)
                    {
                        internalAI[3] = 0;
                        for (int i = 0; i < carrots; i++)
                        {
                            double offsetAngle = startAngle + deltaAngle * (i - (int)(carrots * .5f));
                            Projectile.NewProjectile(WeaponPos.X, WeaponPos.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), carrotType, damage, 5, Main.myPlayer, 0);
                        }
                        npc.netUpdate = true;
                    }
                }
                else if (npc.ai[3] == 3) //Javelin
                {
                    int Javelin = isSupreme ? mod.ProjectileType("BaneTEXR") : mod.ProjectileType("BaneR");
                    if (internalAI[3] == (isSupreme ? 40 : 60))
                    {
                        Vector2 dir = PredictPlayerMovement(ProjSpeed(), player);
                        dir.Normalize();
                        dir *= ProjSpeed();
                        Projectile.NewProjectile(WeaponPos.X, WeaponPos.Y, dir.X, dir.Y, Javelin, damage, 5, Main.myPlayer);
                    }
                    if (internalAI[3] > (isSupreme ? 60 : 90))
                    {
                        internalAI[3] = 0;
                    }
                    npc.netUpdate = true;
                }
                else if (npc.ai[3] == 4) //Excalihare
                {
                    if (internalAI[3] > 20)
                    {
                        internalAI[3] = 0;
                        Vector2 dir = PredictPlayerMovement(ProjSpeed() + 3f, player);
                        dir.Normalize();
                        dir *= ProjSpeed() + 3f;
                        Projectile.NewProjectile(WeaponPos.X, WeaponPos.Y, dir.X, dir.Y, mod.ProjectileType("ExcalihareR"), damage, 5, Main.myPlayer);
                        npc.netUpdate = true;
                    }
                }
                else if (npc.ai[3] == 5) //Fluffy Fury
                {
                    int Arrows = Main.rand.Next(2, 4);
                    float spread = 45f * 0.0174f * .3f;
                    Vector2 dir = PredictPlayerMovement(ProjSpeed(), player);
                    dir.Normalize();
                    dir *= ProjSpeed();
                    float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                    double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                    double deltaAngle = spread / (Arrows * 2);
                    float delay = isSupreme? 15 : 50;
                    if (internalAI[3] > delay)
                    {
                        internalAI[3] = 0;
                        for (int i = 0; i < Arrows; i++)
                        {
                            double offsetAngle = startAngle + (deltaAngle * i);
                            Projectile.NewProjectile(WeaponPos.X, WeaponPos.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), mod.ProjectileType("CarrowR"), damage, 5, Main.myPlayer);
                        }
                        npc.netUpdate = true;
                    }
                }
                else if (npc.ai[3] == 6) //Rabbits Wrath
                {
                    if (internalAI[3] > 5)
                    {
                        internalAI[3] = 0;
                        Vector2 ShootPos = player.Center + new Vector2(player.velocity.X / 10f * ((npc.ai[2] / 60) % 20) * 600f / 14f, 0);
                        Vector2 vector12 = ShootPos;
                        float num75 = 14f;
                        for (int num120 = 0; num120 < 3; num120++)
                        {
                            Vector2 vector2 = ShootPos + new Vector2(-(float)Main.rand.Next(0, 401) * player.direction, -600f);
                            vector2.Y -= 120 * num120;
                            Vector2 vector13 = vector12 - vector2;
                            if (vector13.Y < 0f)
                            {
                                vector13.Y *= -1f;
                            }
                            if (vector13.Y < 20f)
                            {
                                vector13.Y = 20f;
                            }
                            vector13.Normalize();
                            vector13 *= num75;
                            float num82 = vector13.X;
                            float num83 = vector13.Y;
                            float speedX5 = num82;
                            float speedY6 = num83 + Main.rand.Next(-40, 41) * 0.02f;
                            int p = Projectile.NewProjectile(vector2.X, vector2.Y, speedX5, speedY6, mod.ProjectileType("CarrotEXR"), damage, 6, Main.myPlayer, 0, 0);
                            Main.projectile[p].tileCollide = false;
                        }
                        npc.netUpdate = true;
                    }
                }
                else if (npc.ai[3] == 7) //Carrot Farmer
                {
                    if (!AAGlobalProjectile.AnyProjectiles(mod.ProjectileType("CarrotFarmerR")))
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("CarrotFarmerR"), damage, 3f, Main.myPlayer, npc.whoAmI);
                        npc.netUpdate = true;
                    }
                }
            }

            if (Main.expertMode)
            {
                if (npc.life < (npc.lifeMax * .85f)) //The lower the health, the more damage is done
                {
                    npc.damage = (int)(npc.defDamage * 1.1f);
                }
                if (npc.life < (npc.lifeMax * .7f))
                {
                    npc.damage = (int)(npc.defDamage * 1.3f);
                }
                if (npc.life < (npc.lifeMax * .65f))
                {
                    npc.damage = (int)(npc.defDamage * 1.5f);
                }
                if (npc.life < (npc.lifeMax * .4f))
                {
                    npc.damage = (int)(npc.defDamage * 1.7f);
                }
                if (npc.life < (npc.lifeMax * .25f))
                {
                    npc.damage = (int)(npc.defDamage * 1.9f);
                }
                if (npc.life < (npc.lifeMax / 7))
                {
                    npc.damage = (int)(npc.defDamage * 2.2f);
                }
            }
            else
            {
                if (npc.life == npc.lifeMax / 7)
                {
                    npc.damage = (int)(npc.defDamage * 1.5f);
                }
            }

            npc.rotation = 0;
        }

        public bool TileBelowEmpty()
        {
            int tileX = (int)(npc.Center.X / 16f) + npc.direction * 2;
            int tileY = (int)((npc.position.Y + npc.height) / 16f);

            for (int tY = tileY; tY < tileY + 17; tY++)
            {
                if (Main.tile[tileX, tY] == null)
                {
                    Main.tile[tileX, tY] = new Tile();
                }
                if ((Main.tile[tileX, tY].nactive() && Main.tileSolid[Main.tile[tileX, tY].type] && !TileID.Sets.Platforms[Main.tile[tileX, tY].type]) || Main.tile[tileX, tY].liquid > 0)
                {
                    return false;
                }
            }
            return true;
        }

        public string WeaponTexture()
        {
            if (npc.ai[3] == 1) //Bunzooka
            {
                return "NPCs/Bosses/Rajah/RajahArmsB";
            }
            else if (npc.ai[3] == 2) //Scepter
            {
                return "NPCs/Bosses/Rajah/RajahArmsR";
            }
            else if (npc.ai[3] == 3 && internalAI[3] <= (isSupreme ? 40 : 60)) //Javelin
            {
                return "NPCs/Bosses/Rajah/RajahArmsS";
            }
            else if (npc.ai[3] == 4) //Excalihare
            {
                return "NPCs/Bosses/Rajah/Supreme/Excalihare";
            }
            else if (npc.ai[3] == 5) //Fluffy Fury
            {
                return "NPCs/Bosses/Rajah/Supreme/FluffyFury";
            }
            else if (npc.ai[3] == 6) //Rabbits Wrath
            {
                return "NPCs/Bosses/Rajah/Supreme/RabbitsWrath";
            }
            else
            {
                return "BlankTex";
            }
        }

        public void JumpAI()
        {
            internalAI[1] = 1;
            if (npc.ai[0] == 0f)
            {
                npc.noTileCollide = false;
                if (npc.velocity.Y == 0f)
                {
                    npc.velocity.X = npc.velocity.X * 0.8f;
                    internalAI[2] += 1f;
                    if (internalAI[2] > 0f)
                    {
                        if (npc.life < (npc.lifeMax * .85f)) //The lower the health, the more frequent the jumps
                        {
                            internalAI[2] += 2;
                        }
                        if (npc.life < (npc.lifeMax * .7f))
                        {
                            internalAI[2] += 2;
                        }
                        if (npc.life < (npc.lifeMax * .65f))
                        {
                            internalAI[2] += 2;
                        }
                        if (npc.life < (npc.lifeMax * .4f))
                        {
                            internalAI[2] += 2;
                        }
                        if (npc.life < (npc.lifeMax * .25f))
                        {
                            internalAI[2] += 2;
                        }
                        if (npc.life < (npc.lifeMax * .1f))
                        {
                            internalAI[2] += 2;
                        }
                    }
                    if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) > 800f)
                    {
                        internalAI[2] = -1f;
                    }
                    if (internalAI[2] >= 250f)
                    {
                        internalAI[2] = -20f;
                    }
                    else if (internalAI[2] == -1f)
                    {
                        npc.TargetClosest(true);
                        float longth = Math.Abs(npc.Center.X - Main.player[npc.target].Center.X);
                        npc.velocity.X = (6 + longth * .01f) * npc.direction;
                        npc.velocity.Y = -12.1f;
                        npc.ai[0] = 1f;
                        internalAI[2] = 0f;
                        npc.netUpdate = true;
                    }
                }
            }
            else if (npc.ai[0] == 1f)
            {
                if (npc.velocity.Y == 0f)
                {
                    Main.PlaySound(SoundID.Item14, npc.position);
                    npc.ai[0] = 0f;
                    for (int num622 = (int)npc.position.X - 20; num622 < (int)npc.position.X + npc.width + 40; num622 += 20)
                    {
                        for (int num623 = 0; num623 < 4; num623++)
                        {
                            int num624 = Dust.NewDust(new Vector2(npc.position.X - 20f, npc.position.Y + npc.height), npc.width + 20, 4, 31, 0f, 0f, 100);
                            Main.dust[num624].velocity *= 0.2f;
                        }
                        int num625 = Gore.NewGore(new Vector2(num622 - 20, npc.position.Y + npc.height - 8f), default, Main.rand.Next(61, 64), 1f);
                        Main.gore[num625].velocity *= 0.4f;
                    }
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
                        
                        float num626 = 3f;
                        float longth = Math.Abs(npc.Center.X - Main.player[npc.target].Center.X);
                        num626 = 3f + longth * .056f;
                        
                        if (Main.player[npc.target].velocity.X != 0)
                        {
                            num626 += Math.Abs(Main.player[npc.target].velocity.X);
                        }

                        if (npc.direction < 0)
                        {
                            npc.velocity.X = npc.velocity.X - 0.2f;
                        }
                        else if (npc.direction > 0)
                        {
                            npc.velocity.X = npc.velocity.X + 0.2f;
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

                Player player = Main.player[npc.target];
                if(player.Center.Y + player.height / 2 <= npc.Center.Y + npc.height / 2 + 20f && npc.velocity.Y > 0)
                {
                    internalAI[4] = 4f;
                    npc.ai[0] = 0;
                    npc.netUpdate = true;
                    return;
                }
                else if(Math.Abs(npc.Center.X - player.Center.X) < 50f && player.position.Y > npc.Center.Y + npc.height / 2)
                {
                    internalAI[4] = 3f;
                    npc.ai[0] = 0;
                    npc.netUpdate = true;
                    return;
                }
            }
        }

        bool isDashing = false;
        public void FlyAI()
        {
            float speed = 14f;
            if (isSupreme)
            {
                if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) + Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) > 1000)
                {
                    speed = 50f;
                    isDashing = true;
                }
                else
                {
                    speed = 20f;
                    isDashing = false;
                }
            }
            else if (npc.life < (npc.lifeMax * .85f)) //The lower the health, the more damage is done
            {
                speed = 15f;
            }
            else if (npc.life < (npc.lifeMax * .7f))
            {
                speed = 16f;
            }
            else if (npc.life < (npc.lifeMax * .65f))
            {
                speed = 17f;
            }
            else if (npc.life < (npc.lifeMax * .4f))
            {
                speed = 18f;
            }
            else if (npc.life < (npc.lifeMax * .25f))
            {
                speed = 19f;
            }
            else if (npc.life < (npc.lifeMax * .1f))
            {
                speed = 20f;
            }
            AISpaceOctopus(npc, Main.player[npc.target].Center, .35f, speed, 300);
            internalAI[1] = 0;
        }

        public static void AISpaceOctopus(NPC npc, Vector2 targetCenter = default(Vector2), float moveSpeed = 0.15f, float velMax = 5f, float hoverDistance = 250f)
		{
            float pos = 200f;
            if(Main.player[npc.target].velocity.X == 0)
            {
                pos = 0;
            }
            else
            {
                pos = (Main.player[npc.target].velocity.X > 0? 1f: -1f) * 200f;
            }
			Vector2 wantedVelocity = targetCenter - npc.Center + new Vector2(pos, -hoverDistance);
			float dist = (float)Math.Sqrt(wantedVelocity.X * wantedVelocity.X + wantedVelocity.Y * wantedVelocity.Y);
			if (dist < 20f)
			{
				wantedVelocity = npc.velocity;
			}
			else if (dist < 40f)
			{
				wantedVelocity.Normalize();
				wantedVelocity *= velMax * 0.35f;
			}
			else if (dist < 80f)
			{
				wantedVelocity.Normalize();
				wantedVelocity *= velMax * 0.65f;
			}
			else
			{
				wantedVelocity.Normalize();
				wantedVelocity *= velMax;
			}
			if (npc.velocity.X < wantedVelocity.X)
			{
				npc.velocity.X = npc.velocity.X + moveSpeed;
				if (npc.velocity.X < 0f && wantedVelocity.X > 0f)
				{
					npc.velocity.X = npc.velocity.X + moveSpeed;
				}
			}
			else if (npc.velocity.X > wantedVelocity.X)
			{
				npc.velocity.X = npc.velocity.X - moveSpeed;
				if (npc.velocity.X > 0f && wantedVelocity.X < 0f)
				{
					npc.velocity.X = npc.velocity.X - moveSpeed;
				}
			}
			if (npc.velocity.Y < wantedVelocity.Y)
			{
				npc.velocity.Y = npc.velocity.Y + moveSpeed;
				if (npc.velocity.Y < 0f && wantedVelocity.Y > 0f)
				{
					npc.velocity.Y = npc.velocity.Y + moveSpeed;
				}
			}
			else if (npc.velocity.Y > wantedVelocity.Y)
			{
				npc.velocity.Y = npc.velocity.Y - moveSpeed;
				if (npc.velocity.Y > 0f && wantedVelocity.Y < 0f)
				{
					npc.velocity.Y = npc.velocity.Y - moveSpeed;
				}
			}
        }

        public int ChangeRate()
        {
            if (npc.type == mod.NPCType("SupremeRajah"))
            {
                return 120;
            }
            return 240;
        }

        public override void FindFrame(int frameHeight)
        {
            if (internalAI[1] == 0)
            {
                WeaponFrame = frameHeight * 5;
                if (npc.frameCounter++ > 3)
                {
                    npc.frame.Y += frameHeight;
                    npc.frameCounter = 0;
                    if (npc.frame.Y > frameHeight * 7)
                    {
                        npc.frame.Y = 0;
                    }
                }
            }
            else
            {
                WeaponFrame = npc.frame.Y;
                if (npc.ai[0] == 0f)
                {
                    if (internalAI[2] < -17f)
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y = 0;
                    }
                    else if (internalAI[2] < -14f)
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y = frameHeight;
                    }
                    else if (internalAI[2] < -11f)
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y = frameHeight * 2;
                    }
                    else if (internalAI[2] < -8f)
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y = frameHeight * 3;
                    }
                    else if (internalAI[2] < -5f)
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y = frameHeight * 4;
                    }
                    else if (internalAI[2] < -2f)
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y = frameHeight * 5;
                    }
                    else
                    {
                        if (npc.frameCounter++ > 7.5f)
                        {
                            npc.frameCounter = 0;
                            npc.frame.Y += frameHeight;
                            if (npc.frame.Y > frameHeight * 2)
                            {
                                npc.frame.Y = 0;
                            }
                        }
                    }
                }
                else if (npc.ai[0] == 1f)
                {
                    if (npc.velocity.Y != 0f)
                    {
                        npc.frame.Y = frameHeight * 5;
                    }
                    else
                    {
                        npc.frameCounter++;
                        if  (npc.frame.Y > 3)
                        {
                            if (npc.frameCounter > 0)
                            {
                                npc.frameCounter = 0;
                                npc.frame.Y = frameHeight * 6;
                            }
                            else if (npc.frameCounter > 4)
                            {
                                npc.frameCounter = 0;
                                npc.frame.Y = frameHeight * 7;
                            }
                            else if (npc.frameCounter > 8)
                            {
                                npc.frameCounter = 0;
                                npc.frame.Y = 0;
                            }
                        }
                        else
                        {
                            if (npc.frameCounter > 7.5f)
                            {
                                npc.frameCounter = 0;
                                npc.frame.Y += frameHeight;
                                if (npc.frame.Y > frameHeight * 2)
                                {
                                    npc.frame.Y = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("RajahTrophy"));
            }
            if (isSupreme)
            {
                Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/SupremeRajahHelmet1"), 1f);
                Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/SupremeRajahHelmet2"), 1f);
                Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/SupremeRajahHelmet3"), 1f);
                if (!AAModEXAIWorld.downedRajahsRevenge)
                {
                    int n = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("SupremeRajahDefeat"));
                    Main.npc[n].Center = npc.Center;
                }
                else
                {
                    string Name;
                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        Name = "Terrarians";
                    }
                    else
                    {
                        Name = Main.LocalPlayer.name;
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("Rajah8") + Name + AAMod.Lang.BossChat("Rajah9"), 107, 137, 179, true);
                    int p = Projectile.NewProjectile(npc.position, npc.velocity, mod.ProjectileType("SupremeRajahLeave"), 100, 0, Main.myPlayer);
                    Main.projectile[p].Center = npc.Center;
                }
                if (Main.expertMode)
                {
                    for(int i = 0; i < 10; i++) npc.DropBossBags();
                }
                else
                {
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("ChampionPlate"), Main.rand.Next(15, 31));
                    string[] lootTable = { "Excalihare", "FluffyFury", "RabbitsWrath" };
                    int loot = Main.rand.Next(lootTable.Length);
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType(lootTable[loot]));
                }
            }
            else
            {
                int bunnyKills = NPC.killCount[Item.NPCtoBanner(NPCID.Bunny)];
                if (bunnyKills >= 100)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("Rajah4"), 107, 137, 179, true);
                }
                Projectile.NewProjectile(npc.position, npc.velocity, mod.ProjectileType("RajahBookIt"), 100, 0, Main.myPlayer);
                if (!Main.expertMode)
                {
                    if (Main.rand.Next(7) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("RajahMask"));
                    }
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("RajahPelt"), Main.rand.Next(10, 26));
                    string[] lootTableA = { "BaneOfTheBunny", "Bunnyzooka", "RoyalScepter", "Punisher", "RabbitcopterEars" };
                    int lootA = Main.rand.Next(lootTableA.Length);
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType(lootTableA[lootA]));
                }
                else
                {
                    for(int i = 0; i < 10; i++) npc.DropBossBags();
                }
            }
            AAWorld.downedRajah = true;
            npc.value = 0f;
            npc.boss = false;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            if (isSupreme)
            {
                potionType = ModLoader.GetMod("AAMod").ItemType("TheBigOne");
                return;
            }
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * .6f);
        }

        public void RajahTexture()
        {
            string IsRoaring = Roaring ? "Roar" : "";
            string Supreme = isSupreme ? "Supreme/Supreme" : "";
            if (internalAI[1] == 0)
            {
                RajahTex = mod.GetTexture("Bosses/Rajah/" + Supreme + "Rajah" + IsRoaring + "_Fly");
                Glow = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/Rajah" + IsRoaring + "_Fly_Glow");
                SupremeGlow = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/SupremeRajah" + IsRoaring + "_Fly_Glow");
                SupremeEyes = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/SupremeRajah" + IsRoaring + "_Fly_Eyes");
            }
            else
            {
                RajahTex = mod.GetTexture("Bosses/Rajah/" + Supreme + "Rajah" + IsRoaring);
                Glow = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/Rajah" + IsRoaring + "_Glow");
                SupremeGlow = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/SupremeRajah" + IsRoaring + "_Glow");
                SupremeEyes = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/SupremeRajah" + IsRoaring + "_Eyes");
            }
        }
        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            bool RageMode = !isSupreme && npc.life < npc.lifeMax / 7;
            bool SupremeRageMode = isSupreme && npc.life < npc.lifeMax / 7;
            RajahTexture();
            if (isSupreme && isDashing)
            {
                BaseDrawing.DrawAfterimage(spriteBatch, RajahTex, 0, npc, 1f, 1f, 10, false, 0f, 0f, Main.DiscoColor);
            }
            if (RageMode)
            {
                Color RageColor = BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Firebrick, drawColor, Color.Firebrick);
                BaseDrawing.DrawAura(spriteBatch, RajahTex, 0, npc.position, npc.width, npc.height, auraPercent, 1f, 1f, 0f, npc.direction, 8, npc.frame, 0f, -5f, RageColor);
            }
            else if (SupremeRageMode)
            {
                BaseDrawing.DrawAura(spriteBatch, RajahTex, 0, npc.position, npc.width, npc.height, auraPercent, 1f, 1f, 0f, npc.direction, 8, npc.frame, 0f, -5f, Main.DiscoColor);
            }
            if (npc.ai[3] != 0 && npc.ai[3] < 6) //If holding a weapon
            {
                ArmTex = ModLoader.GetMod("AAMod").GetTexture(WeaponTexture());
                Rectangle WeaponRectangle = new Rectangle(0, WeaponFrame, 300, 220);
                BaseDrawing.DrawTexture(spriteBatch, ArmTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 8, WeaponRectangle, drawColor, true);
            }
            BaseDrawing.DrawTexture(spriteBatch, RajahTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 8, npc.frame, drawColor, true);
            if (npc.ai[3] == 6) //If Rabbits Wrath
            {
                ArmTex = mod.GetTexture("Bosses/Rajah/Supreme/RabbitsWrath");
                Rectangle WeaponRectangle = new Rectangle(0, WeaponFrame, 300, 220);
                BaseDrawing.DrawTexture(spriteBatch, ArmTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 8, WeaponRectangle, drawColor, true);
            }
            if (RageMode)
            {
                int shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
                BaseDrawing.DrawTexture(spriteBatch, Glow, shader, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 8, npc.frame, Color.White, true);
            }
            if (SupremeRageMode)
            {
                BaseDrawing.DrawTexture(spriteBatch, Glow, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 8, npc.frame, Main.DiscoColor, true);
                BaseDrawing.DrawAura(spriteBatch, Glow, 0, npc.position, npc.width, npc.height, auraPercent, 1f, 1f, 0f, npc.direction, 8, npc.frame, 0f, -5f, Main.DiscoColor);
                BaseDrawing.DrawTexture(spriteBatch, SupremeGlow, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 8, npc.frame, Main.DiscoColor, true);
                BaseDrawing.DrawAura(spriteBatch, SupremeGlow, 0, npc.position, npc.width, npc.height, auraPercent, 1f, 1f, 0f, npc.direction, 8, npc.frame, 0f, -5f, Main.DiscoColor);
                return false;
            }
            else if (isSupreme)
            {
                BaseDrawing.DrawTexture(spriteBatch, SupremeEyes, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 8, npc.frame, Main.DiscoColor, true);
            }
            return false;
        }

        public override string BossHeadTexture => "AAModEXAI/Bosses/Rajah/Rajah_Head_Boss";

        public void MoveToPoint(Vector2 point)
        {
            float moveSpeed = 30f;
            if (moveSpeed == 0f || npc.Center == point) return;
            float velMultiplier = 1f;
            Vector2 dist = point - npc.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            npc.velocity *= moveSpeed;
            npc.velocity *= velMultiplier;
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if(internalAI[4] == 4f || internalAI[4] == 2f || internalAI[4] == 1f)
            {
                target.wingTime = 0;
                target.velocity.Y = 1f;
            }
        }

        public Vector2 PredictPlayerMovement(float speed, Player player)
        {
            Vector2 npctoplayer = player.Center - npc.Center;
            float playerspeed = (float)Math.Sqrt(player.velocity.X * player.velocity.X + player.velocity.Y * player.velocity.Y);
            float distance = (float)Math.Sqrt(npctoplayer.X * npctoplayer.X + npctoplayer.Y * npctoplayer.Y);
            float deg = player.velocity.ToRotation() - npctoplayer.ToRotation();
            float speedtoplayer = 2f * speed * (float)Math.Cos(deg) + 2f * (float)Math.Sqrt(speed * speed * Math.Cos(deg) * Math.Cos(deg) + speed * speed - playerspeed * playerspeed);
            float movetime = distance / speedtoplayer;
            
            
            if(speed <= playerspeed)
            {
                float newspeed = playerspeed + 10f;
                speedtoplayer = 2f * newspeed * (float)Math.Cos(deg) + 2f * (float)Math.Sqrt(newspeed * newspeed * Math.Cos(deg) * Math.Cos(deg) + newspeed * newspeed - playerspeed * playerspeed);
                movetime = distance / speedtoplayer;
                Vector2 velocity = player.velocity + npctoplayer / movetime;
                velocity.Normalize();
                velocity *= speed;
                return velocity;
            }

            return player.velocity + npctoplayer / movetime;
        }
    }

    [AutoloadBossHead]
    public class SupremeRajah : Rajah
    {
        public override string Texture => "AAModEXAI/Bosses/Rajah/Supreme/SupremeRajah";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rajah Rabbit; Champion of the Innocent");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.damage = 310;
            npc.defense = 0;
            npc.lifeMax = 1200000;
            npc.life = 1200000;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Rajah");
            bossBag = ModLoader.GetMod("AAMod").ItemType("RajahCache");
            isSupreme = true;
            npc.value = Item.sellPrice(3, 0, 0, 0);
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/RajahTheme");
        }
        public override string BossHeadTexture => "AAModEXAI/Bosses/Rajah/SupremeRajah_Head_Boss";
    }
}
