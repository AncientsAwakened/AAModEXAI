using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

using AAModEXAI.Bosses.Rajah.Supreme;
using AAModEXAI.Bosses.Rajah.Supreme.RoyalRabbit;
using AAModEXAI.Dusts;
using AAModEXAI.Localization;

namespace AAModEXAI.Bosses.Rajah
{
    [AutoloadBossHead]
    public class SupremeRajah : Rajah
    {
        public override string Texture => "AAModEXAI/Bosses/Rajah/Supreme/SupremeRajah";
        public int damage = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Supreme Emperor Rabbit; Champion of the Innocent");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.damage = 310;
            npc.defense = 0;
            npc.lifeMax = 1800000;
            npc.life = 1800000;
            isSupreme = true;
            npc.value = Item.sellPrice(3, 0, 0, 0);
        }
        public override string BossHeadTexture => "AAModEXAI/Bosses/Rajah/SupremeRajah_Head_Boss";

        

        public bool isSupreme = false;
        public bool halflifeChange = false;
        public float RabbitWave = 0f;

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
                writer.Write(internalAI[5]);
                writer.Write(RabbitWave);
                writer.Write(halflifeChange);
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
                internalAI[5] = reader.ReadFloat(); //ShootFromSkyPos
                RabbitWave = reader.ReadFloat();
                halflifeChange = reader.ReadBool();
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
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Rajah"), npc.Center);
        }

        public Vector2 WeaponPos;
        public Vector2 StaffPos;

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (isSupreme)
            {
                damage *= .5f;
            }
            return true;
        }

        public float ProjSpeed()
        {
            if (npc.life < (npc.lifeMax * .85f)) //The lower the health, the more damage is done
            {
                return 15f;
            }
            if (npc.life < (npc.lifeMax * .7f))
            {
                return 16f;
            }
            if (npc.life < (npc.lifeMax * .65f))
            {
                return 17f;
            }
            if (npc.life < (npc.lifeMax * .4f))
            {
                return 18f;
            }
            if (npc.life < (npc.lifeMax * .25f))
            {
                return 19f;
            }
            if (npc.life < (npc.lifeMax * .1f))
            {
                return 20f;
            }
            return 14f;
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
            WeaponPos = new Vector2(npc.Center.X + (npc.direction == 1 ? -78 : 78), npc.Center.Y - 9);
            StaffPos = new Vector2(npc.Center.X + (npc.direction == 1 ? 78 : -78), npc.Center.Y - 9);
            if (Roaring) roarTimer--;

            if (npc.ai[3] != 0 && !DefenseLine && !AAModEXAIWorld.downedRajahsRevenge && Main.netMode != NetmodeID.MultiplayerClient)
            {
                DefenseLine = true;
                BaseUtility.Chat(Trans.text("Rajah", "SupremeRajahChat"), Color.MediumPurple);

            }
            if (npc.life <= npc.lifeMax / 7 && !SayLine && Main.netMode != NetmodeID.MultiplayerClient)
            {
                SayLine = true;
                string Name;

                int bunnyKills = NPC.killCount[Item.NPCtoBanner(NPCID.Bunny)];
                if (bunnyKills >= 100 && !AAModEXAIWorld.downedRajahsRevenge)
                {
                    Name = Trans.text("Rajah", "SupremeRajahChat2");
                }
                else
                {
                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        Name = Trans.text("Rajah", "SupremeRajahChat3");
                    }
                    else
                    {
                        Name = Main.LocalPlayer.name;
                    }
                }
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("Rajah", "SupremeRajahChat4") + Name.ToUpper() + Trans.text("Rajah", "SupremeRajahChat5"), 107, 137, 179);
            }

            Player player = Main.player[npc.target];
            if (npc.target >= 0 && Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
                if (Main.player[npc.target].dead)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("Rajah", "SupremeRajahChat6"), 107, 137, 179);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.position, npc.velocity, ModContent.ProjectileType<SupremeRajahBookIt>(), damage, 0, Main.myPlayer);
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
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("Rajah", "SupremeRajahChat7"), 107, 137, 179);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.position, npc.velocity, ModContent.ProjectileType<SupremeRajahBookIt>(), damage, 0, Main.myPlayer); //Originally 100 damage
                    }
                    npc.active = false;
                    npc.noTileCollide = true;
                    npc.netUpdate = true;
                    return;
                }
            }

            if (npc.target <= 0 || npc.target == 255 || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }


            if (player.Center.X < npc.Center.X)
            {
                npc.direction = 1;
            }
            else
            {
                npc.direction = -1;
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

            if(npc.life < (npc.lifeMax * .7f))
            {
                if(!halflifeChange)
                {
                    internalAI[3] = 0;
                    npc.ai[2] = 0;
                    npc.ai[3] = 0;
                    halflifeChange = true;
                    npc.netUpdate = true;
                }
                else
                {
                    SRajahChanges();
                }
                return;
            }

            StompAI();

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
                        npc.ai[3] = Main.rand.Next(7);
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
                            if (NPC.CountNPCS(ModContent.NPCType<RabbitcopterSoldier>()) + AAModEXAIGlobalProjectile.CountProjectiles(ModContent.ProjectileType<BunnySummon1>()) < 5)
                            {
                                Projectile.NewProjectile(StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon1>(), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 200, (int)npc.Center.X + 200), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                Projectile.NewProjectile(StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon1>(), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 200, (int)npc.Center.X + 200), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                Projectile.NewProjectile(StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon1>(), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 200, (int)npc.Center.X + 200), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
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
                                if (NPC.CountNPCS(ModContent.NPCType<RabbitcopterSoldier>()) + AAModEXAIGlobalProjectile.CountProjectiles(ModContent.ProjectileType<BunnySummon1>()) < 5)
                                {
                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon1>(), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon1>(), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon1>(), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                }
                            }
                            else if (npc.ai[1] == 1)
                            {
                                if (NPC.CountNPCS(ModContent.NPCType<BunnyBrawler>()) + AAModEXAIGlobalProjectile.CountProjectiles(ModContent.ProjectileType<BunnySummon2>()) < 5)
                                {
                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon2>(), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon2>(), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
                                }
                            }
                            else if (npc.ai[1] == 2)
                            {
                                if (NPC.CountNPCS(ModContent.NPCType<BunnyBattler>()) + AAModEXAIGlobalProjectile.CountProjectiles(ModContent.ProjectileType<BunnySummon3>()) < 8)
                                {
                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon3>(), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));

                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon3>(), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));

                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon3>(), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));

                                    Projectile.NewProjectile(StaffPos, Vector2.Zero, ModContent.ProjectileType<BunnySummon3>(), 0, 0, Main.myPlayer, Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X + 500), Main.rand.Next((int)npc.Center.Y - 200, (int)npc.Center.Y - 50));
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
                        int Rocket = ModContent.ProjectileType<RajahRocketEXR>();
                        Vector2 shoot = PredictPlayerMovement(ProjSpeed(), player);
                        shoot.Normalize();
                        shoot *= ProjSpeed();
                        Projectile.NewProjectile(WeaponPos.X, WeaponPos.Y, shoot.X, shoot.Y, Rocket, damage, 5, Main.myPlayer);
                        npc.netUpdate = true;
                    }
                }
                else if (npc.ai[3] == 2) //Royal Scepter
                {
                    int carrots = 5;
                    int carrotType = ModContent.ProjectileType<CarrotEXR>();
                    float spread = 45f * 0.0174f * .5f;
                    Vector2 dir = PredictPlayerMovement(ProjSpeed() + 3f, player);
                    dir.Normalize();
                    dir *= ProjSpeed() + 3f;
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
                    int Javelin = ModContent.ProjectileType<BaneTEXR>();
                    if (internalAI[3] == 40)
                    {
                        Vector2 dir = PredictPlayerMovement(ProjSpeed(), player);
                        dir.Normalize();
                        dir *= ProjSpeed();
                        Projectile.NewProjectile(WeaponPos.X, WeaponPos.Y, dir.X, dir.Y, Javelin, damage, 5, Main.myPlayer);
                    }
                    if (internalAI[3] > 60)
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
                        Projectile.NewProjectile(WeaponPos.X, WeaponPos.Y, dir.X, dir.Y, ModContent.ProjectileType<ExcalihareR>(), damage, 5, Main.myPlayer);
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
                    float delay = 15;
                    if (internalAI[3] > delay)
                    {
                        internalAI[3] = 0;
                        for (int i = 0; i < Arrows; i++)
                        {
                            double offsetAngle = startAngle + (deltaAngle * i);
                            Projectile.NewProjectile(WeaponPos.X, WeaponPos.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<CarrowR>(), damage, 5, Main.myPlayer);
                        }
                        npc.netUpdate = true;
                    }
                }
                else if (npc.ai[3] == 6) //Rabbits Wrath
                {
                    if (internalAI[3] > 5)
                    {
                        internalAI[3] = 0;
                        if(npc.ai[2] < 20) internalAI[5] = player.Center.X;
                        if(internalAI[5] - player.Center.X > 200f) internalAI[5] = player.Center.X + 100f;
                        else if(internalAI[5] - player.Center.X < -200f) internalAI[5] = player.Center.X - 100f;
                        Vector2 ShootPos = new Vector2(internalAI[5], player.Center.Y) + new Vector2(player.velocity.X / 10f * ((npc.ai[2] / 60) % 20) * 600f / 14f, 0);
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
                            int p = Projectile.NewProjectile(vector2.X, vector2.Y, speedX5, speedY6, ModContent.ProjectileType<CarrotEXR>(), damage, 6, Main.myPlayer, 0, 0);
                            Main.projectile[p].tileCollide = false;
                        }
                        npc.netUpdate = true;
                    }
                }
                else if (npc.ai[3] == 7) //Carrot Farmer
                {
                    if (!AAModEXAIGlobalProjectile.AnyProjectiles(ModContent.ProjectileType<CarrotFarmerR>()))
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<CarrotFarmerR>(), damage, 3f, Main.myPlayer, npc.whoAmI);
                        npc.netUpdate = true;
                    }
                }
            }
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
                return "Bosses/Rajah/RajahArmsB";
            }
            else if (npc.ai[3] == 2) //Scepter
            {
                return "Bosses/Rajah/RajahArmsR";
            }
            else if (npc.ai[3] == 3 && internalAI[3] <= 40) //Javelin
            {
                return "Bosses/Rajah/RajahArmsS";
            }
            else if (npc.ai[3] == 4) //Excalihare
            {
                return "Bosses/Rajah/Supreme/Excalihare";
            }
            else if (npc.ai[3] == 5) //Fluffy Fury
            {
                return "Bosses/Rajah/Supreme/FluffyFury";
            }
            else if (npc.ai[3] == 6) //Rabbits Wrath
            {
                return "Bosses/Rajah/Supreme/RabbitsWrath";
            }
            else
            {
                return "BlankTex";
            }
        }

        public void StompAI()
        {
            Player player = Main.player[npc.target];
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
                            Projectile.NewProjectile(num622 - 20, npc.position.Y + npc.height - 8f, 0, 0, ModContent.ProjectileType<RajahStomp>(), damage, 6, Main.myPlayer, 0, 0);
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
            if (npc.type == ModContent.NPCType<SupremeRajah>())
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
            Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/SupremeRajahHelmet1"), 1f);
            Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/SupremeRajahHelmet2"), 1f);
            Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/SupremeRajahHelmet3"), 1f);
            if (!AAModEXAIWorld.downedRajahsRevenge)
            {
                int n = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<SupremeRajahDefeat>());
                Main.npc[n].Center = npc.Center;
            }
            else
            {
                string Name;
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    Name = Trans.text("Rajah", "SupremeRajahChat3");
                }
                else
                {
                    Name = Main.LocalPlayer.name;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("Rajah", "SupremeRajahChat8") + Name + Trans.text("Rajah", "SupremeRajahChat9"), 107, 137, 179, true);
                int p = Projectile.NewProjectile(npc.position, npc.velocity, ModContent.ProjectileType<SupremeRajahLeave>(), 100, 0, Main.myPlayer);
                Main.projectile[p].Center = npc.Center;
            }
            if (Main.expertMode)
            {
                for(int i = 0; i < 10; i++) npc.DropBossBags();
            }
            npc.value = 0f;
            npc.boss = false;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
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
                Glow = mod.GetTexture("Bosses/Rajah/Glowmasks/Rajah" + IsRoaring + "_Fly_Glow");
                SupremeGlow = mod.GetTexture("Bosses/Rajah/Glowmasks/SupremeRajah" + IsRoaring + "_Fly_Glow");
                SupremeEyes = mod.GetTexture("Bosses/Rajah/Glowmasks/SupremeRajah" + IsRoaring + "_Fly_Eyes");
            }
            else
            {
                RajahTex = mod.GetTexture("Bosses/Rajah/" + Supreme + "Rajah" + IsRoaring);
                Glow = mod.GetTexture("Bosses/Rajah/Glowmasks/Rajah" + IsRoaring + "_Glow");
                SupremeGlow = mod.GetTexture("Bosses/Rajah/Glowmasks/SupremeRajah" + IsRoaring + "_Glow");
                SupremeEyes = mod.GetTexture("Bosses/Rajah/Glowmasks/SupremeRajah" + IsRoaring + "_Eyes");
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
                ArmTex = mod.GetTexture(WeaponTexture());
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

        private int[] bunny = new int[2];

        private Rectangle AreaBox;


        public void SRajahChanges()
        {
            Player player = Main.player[npc.target];
            if(!AAModEXAIWorld.CRajahFirst)
            {
                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
                bool flag = true;
                internalAI[1] = 1f;
                npc.velocity.X = 0f;
                npc.dontTakeDamage = true;
                npc.noGravity = false;
                npc.noTileCollide = false;
                for (int i = 0; i < 200; i ++)
                {
                    if (Main.player[i].active && Main.player[i].statLife > 0)
                    {
                        if((Main.player[i].Center - npc.Center).Length() > 500)
                        {
                            flag = false;
                            break;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                if(RabbitWave == 0f)
                {
                    RabbitWave += 0.5f;
                }
                if (npc.velocity.Y == 0 && Main.netMode != NetmodeID.MultiplayerClient && flag)
                {
                    npc.ai[2]++;
                }
                if(!flag)
                {
                    if(internalAI[3] ++ > 600)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("Rajah", "SupremeRajahChat7"), 107, 137, 179);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.position, npc.velocity, ModContent.ProjectileType<SupremeRajahBookIt>(), damage, 0, Main.myPlayer); //Originally 100 damage
                        }
                        npc.active = false;
                        npc.noTileCollide = true;
                        npc.netUpdate = true;
                        return;
                    }
                }

                if (npc.ai[2] == 0)
                {
                    string playername;
                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        playername = Trans.text("Rajah", "SupremeRajahChat3");
                    }
                    else
                    {
                        playername = Main.LocalPlayer.name;
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("Hey, " + playername + ", come to me.", 107, 137, 179, true);
                    npc.ai[2] += 1;
                }
                if (npc.ai[2] == 240)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("I think you may have some misunderstandings about me, the SuperAncents", 107, 137, 179, true);
                }
                if (npc.ai[2] == 480)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("Do you think I'm just faster, have more life health or just can do more damage to you?", 107, 137, 179, true);
                }
                if (npc.ai[2] == 720)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("Look at these two cute bunnies.", 107, 137, 179, true);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        bunny[0] = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.Bunny);
                        bunny[1] = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.Bunny);
						Main.npc[bunny[0]].dontTakeDamage = true;
                        Main.npc[bunny[1]].dontTakeDamage = true;
                        Main.npc[bunny[0]].velocity.X = 3f;
                        Main.npc[bunny[1]].velocity.X = -3f;
                    }
                }
                if (npc.ai[2] == 960)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("Then the first lesson I'm going to teach you is", 107, 137, 179, true);
                }
                if (npc.ai[2] == 1260)
                {
                    music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/SuperAncients");
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("'AWAKENED', this kind of thing is as easy as winking for me!", 107, 137, 179, true);
                    for(int proj = 0; proj < 1000; proj ++)
                    {
                        if (Main.projectile[proj].active && Main.projectile[proj].friendly && !Main.projectile[proj].hostile)
                        {
                            Main.projectile[proj].hostile = true;
                            Main.projectile[proj].friendly = false;
                            Vector2 vector = Main.projectile[proj].Center - npc.Center;
                            vector.Normalize();
                            Vector2 reflectvelocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                            reflectvelocity.Normalize();
                            reflectvelocity *= vector.Length();
                            reflectvelocity += vector * 20f;
                            reflectvelocity.Normalize();
                            reflectvelocity *= vector.Length();
                            if(reflectvelocity.Length() < 20f)
                            {
                                reflectvelocity.Normalize();
                                reflectvelocity *= 20f;
                            }

                            Main.projectile[proj].penetrate = 1;

                            Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().reflectvelocity = reflectvelocity;
                            Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().isReflecting = true;
                            Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().ReflectConter = 180;
                        }
                    }
                    if(bunny[0] < 1000 && bunny[0] >= 0 && Main.npc[bunny[0]].type == NPCID.Bunny)
                    {
                        Main.npc[bunny[0]].dontTakeDamage = true;
                        Main.npc[bunny[0]].life = 0;
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int id = AAGlobalNPC.ChangeToSAABoss(bunny[0], ModContent.NPCType<RoyalRabbitArcher>());
                            if (Main.netMode == NetmodeID.Server && id < 200) NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);
                            Main.npc[id].netUpdate = true;
                        }
                        Main.npc[bunny[0]].netUpdate = true;
                        int b = Projectile.NewProjectile(Main.npc[bunny[0]].Center.X, Main.npc[bunny[0]].Center.Y, 0f, 0f, ModContent.ProjectileType<Effects.ShockwaveBoom>(), 0, 1, Main.myPlayer, 0, 0);
                        Main.projectile[b].Center = Main.npc[bunny[0]].Center;
                    }
                    if(bunny[1] < 1000 && bunny[1] >= 0 && Main.npc[bunny[1]].type == NPCID.Bunny)
                    {
                        Main.npc[bunny[1]].dontTakeDamage = true;
                        Main.npc[bunny[1]].life = 0;
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int id = AAGlobalNPC.ChangeToSAABoss(bunny[1], ModContent.NPCType<RoyalRabbitMagic>());
                            if (Main.netMode == NetmodeID.Server && id < 200) NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);
                            Main.npc[id].netUpdate = true;
                        }
                        Main.npc[bunny[1]].netUpdate = true;
                        int b = Projectile.NewProjectile(Main.npc[bunny[1]].Center.X, Main.npc[bunny[1]].Center.Y, 0f, 0f, ModContent.ProjectileType<Effects.ShockwaveBoom>(), 0, 1, Main.myPlayer, 0, 0);
                        Main.projectile[b].Center = Main.npc[bunny[1]].Center;
                    }
                    AAModEXAIWorld.CRajahFirst = true;
                    npc.ai[2] = 0;
                    internalAI[3] = 0;
                }
                return;
            }

            if (npc.life < (npc.lifeMax * .7f))
            {
                if(RabbitWave == 0f)
                {
                    music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/SuperAncients");
                    RabbitWave = 0.5f;
                    Projectile.NewProjectile(npc.Center, new Vector2(4f, 0f), ModContent.ProjectileType<RoyalBunnySummon>(), 0, 0, Main.myPlayer, ModContent.NPCType<RoyalRabbitMagic>(), 0f);
                    Projectile.NewProjectile(npc.Center, new Vector2(-4f, 0f), ModContent.ProjectileType<RoyalBunnySummon>(), 0, 0, Main.myPlayer, ModContent.NPCType<RoyalRabbitArcher>(), 0f);
                }
                if(RabbitWave == 0.5f && !NPC.AnyNPCs(ModContent.NPCType<RoyalRabbitArcher>()) && !NPC.AnyNPCs(ModContent.NPCType<RoyalRabbitMagic>()) && !AAModEXAIGlobalProjectile.AnyProjectiles(ModContent.ProjectileType<RoyalBunnySummon>()))
                {
                    RabbitWave = 1f;
                }
            }
            if (npc.life < (npc.lifeMax * .5f))
            {
                if(RabbitWave == 1f)
                {
                    Vector2 vector = new Vector2(player.position.X, player.position.Y);
                    AreaBox.X = (int)(vector.X - 1000f);
                    AreaBox.Y = (int)(vector.Y - 1000f);
                    AreaBox.Width = 2000;
                    AreaBox.Height = 2000;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int RecX = (int)(AreaBox.X + AreaBox.Width / 2) / 16;
                        int RecY = (int)(AreaBox.Y + AreaBox.Height / 2) / 16;
                        int HalfLongth = AreaBox.Width / 2 / 16 + 1;
                        for (int i = RecX - HalfLongth; i <= RecX + HalfLongth; i++)
                        {
                            for (int j = RecY - HalfLongth; j <= RecY + HalfLongth; j++)
                            {
                                if ((i == RecX - HalfLongth || i == RecX + HalfLongth || j == RecY - HalfLongth || j == RecY + HalfLongth) && !Main.tile[i, j].active())
                                {
                                    Main.tile[i, j].type = (ushort)ModContent.TileType<RajahTerraCrystal>();
                                    Main.tile[i, j].active(true);
                                }
                                else
                                {
                                    continue;
                                }
                                if (Main.netMode == NetmodeID.Server)
                                {
                                    NetMessage.SendTileSquare(-1, i, j, 1, 0);
                                }
                                else
                                {
                                    WorldGen.SquareTileFrame(i, j, true);
                                }
                            }
                        }
                    }
                    Projectile.NewProjectile(npc.Center, new Vector2(4f, 0f), ModContent.ProjectileType<RoyalBunnySummon>(), 0, 0, Main.myPlayer, ModContent.NPCType<RoyalRabbitLancer>(), 0f);
                    Projectile.NewProjectile(npc.Center, new Vector2(-4f, 0f), ModContent.ProjectileType<RoyalBunnySummon>(), 0, 0, Main.myPlayer, ModContent.NPCType<RoyalRabbitSummoner>(), 0f);
                    RabbitWave = 1.5f;
                }
                if(RabbitWave == 1.5f && !NPC.AnyNPCs(ModContent.NPCType<RoyalRabbitLancer>()) && !NPC.AnyNPCs(ModContent.NPCType<RoyalRabbitSummoner>()) && !AAModEXAIGlobalProjectile.AnyProjectiles(ModContent.ProjectileType<RoyalBunnySummon>()))
                {
                    RabbitWave = 2f;
                }
            }
            if (npc.life < (npc.lifeMax * .3f))
            {
                if(RabbitWave == 2f)
                {
                    RabbitWave = 2.5f;
                    Projectile.NewProjectile(npc.Center, new Vector2(4f, 0f), ModContent.ProjectileType<RoyalBunnySummon>(), 0, 0, Main.myPlayer, ModContent.NPCType<RoyalRabbitScourger>(), 0f);
                    Projectile.NewProjectile(npc.Center, new Vector2(-4f, 0f), ModContent.ProjectileType<RoyalBunnySummon>(), 0, 0, Main.myPlayer, ModContent.NPCType<RoyalRabbitShooter1>(), 0f);
                }
                if(RabbitWave == 2.5f && !NPC.AnyNPCs(ModContent.NPCType<RoyalRabbitScourger>()) && !NPC.AnyNPCs(ModContent.NPCType<RoyalRabbitShooter1>()) && !AAModEXAIGlobalProjectile.AnyProjectiles(ModContent.ProjectileType<RoyalBunnySummon>()))
                {
                    RabbitWave = 3f;
                }
            }
            if (npc.life < (npc.lifeMax * .1f))
            {
                if(RabbitWave == 3f)
                {
                    RabbitWave = 3.5f;
                    Projectile.NewProjectile(npc.Center, new Vector2(4f, 0f), ModContent.ProjectileType<RoyalBunnySummon>(), 0, 0, Main.myPlayer, ModContent.NPCType<RoyalRabbitShooter2>(), 0f);
                    Projectile.NewProjectile(npc.Center, new Vector2(-4f, 0f), ModContent.ProjectileType<RoyalBunnySummon>(), 0, 0, Main.myPlayer, ModContent.NPCType<RoyalRabbitShooter3>(), 0f);
                }
                if(RabbitWave == 3.5f && !NPC.AnyNPCs(ModContent.NPCType<RoyalRabbitShooter2>()) && !NPC.AnyNPCs(ModContent.NPCType<RoyalRabbitShooter3>()) && !AAModEXAIGlobalProjectile.AnyProjectiles(ModContent.ProjectileType<RoyalBunnySummon>()))
                {
                    RabbitWave = 4f;
                }
            }

            if(RabbitWave != 0f && RabbitWave != 1f && RabbitWave != 2f && RabbitWave != 3f && RabbitWave != 4f)
            {
                npc.damage = 0;
                internalAI[1] = 0f;
                npc.dontTakeDamage = true;
                npc.noGravity = true;
                npc.noTileCollide = true;

                Vector2 targetPos = player.Center;
                targetPos.Y -= 500f;

                if (npc.Distance(targetPos) > 50)
                {
                    float speedModifier = .5f;
                    if (npc.Center.X < targetPos.X)
                    {
                        npc.velocity.X += speedModifier;
                        if (npc.velocity.X < 0)
                            npc.velocity.X += speedModifier * 1.3f;
                    }
                    else
                    {
                        npc.velocity.X -= speedModifier;
                        if (npc.velocity.X > 0)
                            npc.velocity.X -= speedModifier * 1.3f;
                    }
                    if (npc.Center.Y < targetPos.Y)
                    {
                        npc.velocity.Y += speedModifier;
                        if (npc.velocity.Y < 0)
                            npc.velocity.Y += speedModifier * 1.3f;
                    }
                    else
                    {
                        npc.velocity.Y -= speedModifier;
                        if (npc.velocity.Y > 0)
                            npc.velocity.Y -= speedModifier * 1.3f;
                    }
                }
                return;
            }
            else
            {
                npc.dontTakeDamage = false;
            }
            
            StompAI();
        }
    }
}
