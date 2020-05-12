using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using AAMod;

namespace AAModEXAI.Bosses.AH.Haruka
{
    [AutoloadBossHead]
    public class Haruka : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Haruka Yamata");
            Main.npcFrameCount[npc.type] = 28;
        }

        public override void SetDefaults()
        {
            internalAI[0] = 3;
            npc.width = 50;
            npc.height = 60;
            npc.friendly = false;
            npc.damage = 180;
			npc.defense = 120;
            npc.lifeMax = 180000;
            npc.HitSound = SoundID.NPCHit1;
            npc.value = Item.sellPrice(0, 12, 0, 0);
            npc.knockBackResist = 0f;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.lavaImmune = true;
            npc.boss = true;
            npc.netAlways = true;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/AH");
            npc.noGravity = true;
            npc.noTileCollide = true;
            bossBag = ModLoader.GetMod("AAMod").ItemType("AHBag");
        }


        public int ProjectileShoot = -1;
        public int repeat = 10;
        public bool isSlashing = false;

        public Vector2 MovePoint;
        public bool SelectPoint = false;

        public bool Invisible = false;
        public int Frame = 0;

        public int damage = 0;

        public int[] internalAI = new int[10];

        public int[] ShadowNPC = new int[3];
        
        public Vector2 ShadowkingPosition = Vector2.Zero;

        public bool SpawnClone = false;

        public bool Shadowkill = false;

        public bool SHADOWDOG = false;

        public int SHADOWCONTER = 0;

        public int strikebackproj = 0;

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]); //Used as the AI selector
                writer.Write(internalAI[1]); //Used as the Frame Counter
                writer.Write(internalAI[2]); //Used for current frame
                writer.Write(internalAI[3]); //Used to count down to AI change
                writer.Write(internalAI[4]); //Used as an AI Timer
                writer.Write(internalAI[5]);
                writer.Write(internalAI[6]); //Haruka skill counter
                writer.Write(internalAI[7]); //Haruka Shadow Dash Counter
                writer.Write(internalAI[8]); //Shadowkilling colddown
                writer.Write(internalAI[9]); //Noticing Ashe is dead
                writer.Write(ShadowNPC[0]);
                writer.Write(ShadowNPC[1]);
                writer.Write(ShadowNPC[2]);
                writer.Write(SHADOWCONTER);
                writer.Write(strikebackproj);
                writer.Write(ProjectileShoot);
                writer.Write(repeat);
                writer.Write(SelectPoint);
                writer.Write(isSlashing);
                writer.Write(Invisible);
                writer.Write(SpawnClone);
                writer.Write(SHADOWDOG);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadInt();
                internalAI[1] = reader.ReadInt();
                internalAI[2] = reader.ReadInt();
                internalAI[3] = reader.ReadInt();
                internalAI[4] = reader.ReadInt();
                internalAI[5] = reader.ReadInt();
                internalAI[6] = reader.ReadInt();
                internalAI[7] = reader.ReadInt();
                internalAI[8] = reader.ReadInt();
                internalAI[9] = reader.ReadInt();
                ShadowNPC[0] = reader.ReadInt();
                ShadowNPC[1] = reader.ReadInt();
                ShadowNPC[2] = reader.ReadInt();
                SHADOWCONTER = reader.ReadInt();
                strikebackproj = reader.ReadInt();
                ProjectileShoot = reader.ReadInt();
                repeat = reader.ReadInt();
                isSlashing = reader.ReadBool();
                SelectPoint = reader.ReadBool();
                Invisible = reader.ReadBool();
                SpawnClone = reader.ReadBool();
                SHADOWDOG = reader.ReadBool();
            }
        }

        public float XPos = 20f;

        public float pos = 250f;


        public override void HitEffect(int hitDirection, double damage)
        {
            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("AcidDust"), npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            if(Invisible || internalAI[0] == AISTATE_Shadowkilling) return false;
            return null;
        }

        public override void NPCLoot()
        {
            int Ashe = NPC.CountNPCS(mod.NPCType("Ashe"));
            if (Ashe == 0)
            {
                NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("AHDeath"));
                if (Main.expertMode)
                {
                    npc.DropBossBags();
                }
            }
            if (!Main.expertMode)
            {
                string[] lootTableH = { "HarukaKunai", "Masamune", "MizuArashi", "HarukaBox" };
                int lootH = Main.rand.Next(lootTableH.Length);
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType(lootTableH[lootH]));
            }
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("HarukaTrophy"));
            }
            if (Main.netMode != 1) BaseUtility.Chat(AAMod.Lang.BossChat("HarukaDowned"), new Color(72, 78, 117));
            NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("HarukaVanish"));

        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.7f);
        }

        public bool SetMovePos = false;

        public int Shadowdashcounter = 0;

        public static int AISTATE_PROJ = 0, AISTATE_SLASH = 1, AISTATE_SPIN = 2, AISTATE_IDLE = 3, AISTATE_Shadowkilling = 4, AISTATE_ASHEGAG = 5;

        public override void AI()
        {
            Player player = Main.player[npc.target];
            if (Main.expertMode)
            {
                damage = npc.damage / 4;
            }
            else
            {
                damage = npc.damage / 2;
            }

            int Ashe = NPC.CountNPCS(mod.NPCType("Ashe"));
            bool flag = player.dead || !player.active || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 6000f;
            if (Ashe == 0)
            {
                if (flag)
                {
                    npc.TargetClosest(false);
                    if (Main.netMode != 1)
                    {
                        int DeathAnim = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("HarukaVanish"), 0);
                        Main.npc[DeathAnim].velocity = npc.velocity;
                        Main.npc[DeathAnim].netUpdate = true;
                        npc.active = false;
                        npc.netUpdate = true;
                    }
                    return;
                }
            }

            npc.direction = npc.spriteDirection = npc.position.X < player.position.X ? 1 : -1;

            if (SHADOWCONTER <= 0)
            {
                npc.dontTakeDamage = false;
                SHADOWCONTER = 0;
                Shadowdashcounter = 0;
            }

            if (Invisible)
            {
                if (npc.alpha < 255)
                {
                    npc.alpha += 5;
                }
                else
                {
                    npc.chaseable = false;
                    npc.alpha = 255;
                }
            }
            else
            {
                if (npc.alpha > 0)
                {
                    npc.alpha -= 8;
                }
                else
                {
                    npc.chaseable = true;
                    npc.alpha = 0;
                }
            }

            if (Main.netMode != 1)
            {
                internalAI[1]++;
                internalAI[5]++;
                internalAI[6]++;
                internalAI[7]++;
                internalAI[8]++;
            }

            int InvisTimer1 = 1000;

            int InvisTimer2 = 1300;

            if (npc.life < npc.lifeMax * .66f)
            {
                InvisTimer1 = 800;

                InvisTimer2 = 1100;

                if(Main.expertMode) internalAI[6]++;
            }
            if (npc.life < npc.lifeMax * .33f)
            {
                InvisTimer1 = 600;

                InvisTimer2 = 900;

                if(Main.expertMode) internalAI[6] += 2;
            }
            if (internalAI[5] > InvisTimer1)
            {
                if (!Invisible)
                {
                    Invisible = true;
                    npc.netUpdate = true;
                }
            }
            if (internalAI[5] > InvisTimer2 && Main.netMode != 1 && internalAI[0] != AISTATE_Shadowkilling)
            {
                Invisible = false;
                Backstab();
                Vector2 targetCenter = player.position + new Vector2(player.width * 0.5f, player.height * 0.5f);
                Vector2 fireTarget = npc.Center;
                int projType = mod.ProjectileType("HarukaProj");
                BaseAI.FireProjectile(targetCenter, fireTarget, projType, damage*1, 0f, 12f);
                internalAI[0] = Main.rand.Next(2);
                internalAI[5] = 0;
                npc.netUpdate = true;
            }

            if (internalAI[6] >= 6000)
            {
                internalAI[6] = 6000;
            }

            if (internalAI[7] >= 3000)
            {
                internalAI[7] = 3000;
            }

            if (internalAI[8] >= 2000)
            {
                internalAI[8] = 2000;
            }
            
            if (Invisible)
            {
                npc.dontTakeDamage = true;
                internalAI[0] = 3;
            }
            else
            {
                npc.dontTakeDamage = false;
            }

            if (npc.alpha >= 200)
            {
                npc.dontTakeDamage = true;
            }
            else
            {
                npc.dontTakeDamage = false;
            }

            if(Shadowkill && npc.alpha > 250)
            {
                internalAI[0] = AISTATE_Shadowkilling;
                internalAI[1] = 0;
                internalAI[2] = 0;
                internalAI[3] = 0;
                internalAI[5] = 0;
                Invisible = false;
            }
            
            if (ProjectileShoot == 0 || internalAI[0] == AISTATE_SLASH)
            {
                if (Main.netMode != 1)
                {
                    if (internalAI[1] > 4)
                    {
                        internalAI[1] = 0;
                        internalAI[2]++;
                    }
                }
            }
            else
            {
                if (Main.netMode != 1)
                {
                    if (internalAI[1] > 8)
                    {
                        internalAI[1] = 0;
                        internalAI[2]++;
                    }
                }
            }

            if(Main.expertMode && internalAI[0] != AISTATE_Shadowkilling && internalAI[0] != AISTATE_SPIN && SHADOWCONTER <= 0 && !Invisible)
            {
                for(int i=0 ; i < 1000; i++)
                {
                    if(npc.Hitbox.Intersects(Main.projectile[i].Hitbox) && Main.projectile[i].friendly && Main.projectile[i].damage > 0)
                    {
                        if(internalAI[6] >= 3500)
                        {
                            Main.projectile[i].Kill();
                            internalAI[6] -= 3500;
                            strikebackproj ++;
                            internalAI[0] = AISTATE_SPIN;
                        }
                        else if(internalAI[7] >= 3000)
                        {
                            internalAI[7] = 0;
                            SHADOWDOG = true;
                        }
                        break;
                    }
                }
            }

            if(SHADOWDOG)
            {
                SHADOWCONTER ++;
                npc.dontTakeDamage = true;
                internalAI[0] = AISTATE_IDLE;
                if(InvisTimer1 - internalAI[5] <= 360)
                {
                    internalAI[5] -= 360;
                }
                if(internalAI[5] < 0)
                {
                    internalAI[5] = 0;
                }
                if(SHADOWCONTER >= 180)
                {
                    SHADOWDOG = false;
                    internalAI[0] = AISTATE_SLASH;
                    Shadowdashcounter = 0;
                }
            }


            if (internalAI[0] == AISTATE_IDLE)
            {
                if (Main.netMode != 1) 
                {
                    internalAI[3]++;

                    if (internalAI[3] >= 90 && !Invisible && !SHADOWDOG)
                    {
                        internalAI[3] = 0;
                        internalAI[0] = Main.rand.Next(2);
                        if(internalAI[6] >= 1500 && internalAI[8] >= 2000 && Main.expertMode)
                        {
                            internalAI[8] = 0;
                            internalAI[6] -= 1500;
                            Shadowkill = true;
                            Invisible = true;
                        }
                        else if(Main.rand.Next(4) == 0 && internalAI[6] >= 500)
                        {
                            internalAI[0] = AISTATE_SPIN;
                            internalAI[6] -= 500;
                        }
                        npc.ai = new float[4];
                    }
                    else if(internalAI[3] >= 95)
                    {
                        internalAI[3] = 0;
                    }
                }

                if (internalAI[2] > 3 && Main.netMode != 1)
                {
                    internalAI[1] = 0;
                    internalAI[2] = 0;
                }
            }
            else if (internalAI[0] == AISTATE_PROJ)
            {
                if (ProjectileShoot == -1 && Main.netMode != 1)
                {
                    ProjectileShoot = Main.rand.Next(2);
                }
                if (ProjectileShoot == 0)
                {
                    if (internalAI[2] == 5 && internalAI[1] == 3 && Main.netMode != 1)
                    {
                        repeat -= 1;
                        int projType = mod.ProjectileType("HarukaKunai");
                        float spread = 45f * 0.0174f;
                        Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                        dir *= 14f;
                        float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                        double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                        double deltaAngle = spread / 6f;
                        for (int i = 0; i < 3; i++)
                        {
                            double offsetAngle = startAngle + (deltaAngle * i);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), projType, damage*1, 5, Main.myPlayer);
                        }
                    }
                    if ((internalAI[2] < 4 || internalAI[2] > 6) && Main.netMode != 1) 
                    {
                        internalAI[1] = 0;
                        internalAI[2] = 4;
                    }
                    if (repeat <= 0)
                    {
                        npc.frameCounter = 0;
                        Frame = 0;
                        if (Main.netMode != 1)
                        {
                            internalAI[0] = 3;
                            internalAI[1] = 0;
                            internalAI[2] = 0;
                            internalAI[3] = 0;
                            internalAI[4] = 0;
                            ProjectileShoot -= 1;
                            repeat = 12;
                            npc.ai = new float[4];
                        }
                    }
                }
                else if (ProjectileShoot == 1)
                {
                    internalAI[3]++;
                    if (Main.netMode != 1)
                    {
                        if (internalAI[3] == 100 || internalAI[3] == 200 || internalAI[3] == 299)
                        {
                            isSlashing = true;
                        }
                        if (isSlashing)
                        {
                            if (internalAI[2] < 7 || internalAI[2] > 9)
                            {
                                internalAI[1] = 0;
                                internalAI[2] = 7;
                            }
                        }
                        else
                        {
                            if (internalAI[2] > 3)
                            {
                                internalAI[1] = 0;
                                internalAI[2] = 0;
                            }
                        }
                    }

                    if (internalAI[2] == 8 && internalAI[1] == 4 && Main.netMode != 1)
                    {
                        Vector2 targetCenter = player.position + new Vector2(player.width * 0.5f, player.height * 0.5f);
                        Vector2 fireTarget = npc.Center;
                        int projType = mod.ProjectileType("HarukaProj");
                        BaseAI.FireProjectile(targetCenter, fireTarget, projType, damage*1, 0f, 14f);
                        npc.netUpdate = true;
                    }
                    if (isSlashing && internalAI[2] > 9 && Main.netMode != 1)
                    {
                        isSlashing = false;
                        npc.netUpdate = true;
                    }
                    if (internalAI[3] > 300)
                    {
                        npc.frameCounter = 0;
                        Frame = 0;
                        if (Main.netMode != 1)
                        {
                            internalAI[0] = 3;
                            internalAI[1] = 0;
                            internalAI[2] = 0;
                            internalAI[3] = 0;
                            internalAI[4] = 0;
                            ProjectileShoot -= 1;
                            npc.ai = new float[4];
                        }
                    }
                }
            }
            else if (internalAI[0] == AISTATE_SLASH)
            {
                internalAI[3]++;

                if(SHADOWCONTER > 0)
                {
                    SHADOWCONTER--;
                }
                else
                {
                    if (internalAI[2] < 17)
                    {
                        internalAI[1] = 0;
                        internalAI[2] = 17;
                    }
                    if (internalAI[2] > 26)
                    {
                        internalAI[1] = 0;
                        internalAI[2] = 17;
                        internalAI[4] += 1;
                    }
                    if (internalAI[4] > 5)
                    {
                        npc.frameCounter = 0;
                        Frame = 0;
                        if (Main.netMode != 1)
                        {
                            internalAI[0] = 3;
                            internalAI[1] = 0;
                            internalAI[2] = 0;
                            internalAI[3] = 0;
                            internalAI[4] = 0;
                            if(internalAI[6] >= 500 && Main.rand.Next(2) == 0)
                            {
                                internalAI[0] = AISTATE_SPIN;
                                internalAI[6] -= 500;
                            }
                            else
                            {
                                internalAI[0] = 3;
                            }
                            npc.ai = new float[4];
                        }
                    }
                }
            }
            else if (internalAI[0] == AISTATE_SPIN)
            {
                if (internalAI[2] < 10)
                {
                    internalAI[1] = 0;
                    internalAI[2] = 10;
                }
                if (internalAI[2] > 16)
                {
                    internalAI[1] = 0;
                    internalAI[2] = 13;
                }

                if(internalAI[4] < 200)
                {
                    SelectPoint = true;
                }

                if(InvisTimer1 - internalAI[5] <= 120)
                {
                    internalAI[5] -= 120;
                }

                internalAI[4]++;

                if (SelectPoint)
                {
                    MovePoint = player.Center;
                    SelectPoint = false;
                }

                if (strikebackproj != 0)
                {
                    for(int i=0 ; i < 1000; i++)
                    {
                        if(npc.Hitbox.Intersects(Main.projectile[i].Hitbox) && Main.projectile[i].friendly && Main.projectile[i].damage > 0)
                        {
                            strikebackproj ++;
                            break;
                        }
                    }
                }
                

                if (internalAI[4] >= 200)
                {
                    int projType = mod.ProjectileType("HarukaProj");
                    float spread = 45f * 0.0174f;
                    Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                    dir *= 14f;
                    float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                    double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                    double deltaAngle = spread / 6f;

                    if(Main.netMode != 1)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            double offsetAngle = startAngle + (deltaAngle * i);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), projType, damage, 0, Main.myPlayer);
                        }
                        if(strikebackproj != 0)
                        {
                            startAngle -= .1d * (strikebackproj / 2);
                            deltaAngle = spread / 3f;
                            for (int i = 0; i < strikebackproj; i++)
                            {
                                double offsetAngle = startAngle + (deltaAngle * i);
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), mod.ProjectileType("HarukaArrow"), damage, 0, Main.myPlayer);
                            }
                        }
                    }
                    
                    strikebackproj = 0;

                    npc.frameCounter = 0;
                    Frame = 0;

                    if(internalAI[6] >= 1500 && internalAI[8] >= 1200 && Main.expertMode)
                    {
                        internalAI[8] = 0;
                        internalAI[6] -= 1500;
                        Shadowkill = true;
                        Invisible = true;
                    }
                    else if(Main.rand.Next(2) == 0)
                    {
                        internalAI[0] = AISTATE_PROJ;
                    }
                    else
                    {
                        internalAI[0] = 3;
                    }

                    internalAI[1] = 0;
                    internalAI[2] = 0;
                    internalAI[3] = 0;
                    internalAI[4] = 0;
                    pos *= -1f;
                    npc.ai = new float[4];
                }
            }
            else if (internalAI[0] == AISTATE_Shadowkilling)
            {
                ShadowNPC[0] = npc.whoAmI;
                internalAI[4]++;
                if(npc.alpha >= 255)
                {
                    SpawnClone = true;
                }
                Shadowkilling();
            }
            else
            {
                if (Main.netMode != 1)
                {
                    internalAI[0] = 3;
                    internalAI[1] = 0;
                    internalAI[2] = 0;
                    internalAI[3] = 0;
                    npc.ai = new float[4];
                }
            }

            if (internalAI[0] == AISTATE_SLASH || internalAI[0] == AISTATE_SPIN) //Melee Damage/Speed boost
            {
                npc.damage = 190;
                npc.defense = 300;
            }
            else if (internalAI[0] == AISTATE_Shadowkilling) //Melee Damage/Speed boost
            {
                npc.damage = 220;
                npc.defense = 9999;
            }
            else //Reset Stats
            {
                npc.defense = npc.defDefense;
                npc.damage = npc.defDamage;
            }


            if (internalAI[0] == AISTATE_IDLE || Invisible) //When charging the player
            {
                MoveToPoint(player.Center - new Vector2(pos, 0));
            }
            else if (internalAI[0] == AISTATE_PROJ)
            {
                MoveToPoint(player.Center - new Vector2(pos * 2f, 0));
            }
            else if (internalAI[0] == AISTATE_SPIN)
            {
                MoveToPoint(MovePoint);
            }
            else if (internalAI[0] == AISTATE_SLASH) //When charging the player
            {
                if(SHADOWCONTER > 0)
                {
                    LOOPPOINT(player.Center + new Vector2(0, 700f), player.Center - new Vector2(0, 700f));
                }
                else
                {
                    MoveToPoint(player.Center);
                }
            }
            npc.rotation = 0;

            npc.noTileCollide = true;
        }

        public override void PostAI()
        {
            Player player = Main.player[npc.target];
            if (internalAI[0] == AISTATE_ASHEGAG || internalAI[9] >= 480)
            {
                return;
            }
            if (internalAI[0] != AISTATE_SPIN && internalAI[0] != AISTATE_Shadowkilling)
            {
                if (player.Center.X > npc.Center.X) //If NPC's X position is higher than the player's
                {
                    if (pos == -250)
                    {
                        pos = 250;
                    }
                    npc.direction = 1;
                }
                else //If NPC's X position is lower than the player's
                {
                    if (pos == 250)
                    {
                        pos = -250;
                    }
                    npc.direction = -1;
                }
            }
            else
            {
                npc.direction = npc.velocity.X > 0 ? 1 : -1;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (internalAI[0] == AISTATE_ASHEGAG)
            {
                if (npc.frameCounter > 12)
                {
                    npc.frameCounter = 0;
                    Frame++;
                }
            }
            else if (ProjectileShoot == 0 || internalAI[0] == AISTATE_SLASH || internalAI[0] == AISTATE_SPIN)
            {
                if (npc.frameCounter > 4)
                {
                    npc.frameCounter = 0;
                    Frame++;
                }
            }
            else
            {
                if (npc.frameCounter > 8)
                {
                    npc.frameCounter = 0;
                    Frame++;
                }
            }
            if (internalAI[0] == AISTATE_IDLE || internalAI[0] == AISTATE_ASHEGAG)
            {
                if (Frame > 3)
                {
                    npc.frameCounter = 0;
                    Frame = 0;
                }
            }
            else if (internalAI[0] == AISTATE_PROJ)
            {
                if (ProjectileShoot == 0)
                {
                    if (Frame < 4 || Frame > 6)
                    {
                        npc.frameCounter = 0;
                        Frame = 4;
                    }
                }
                else if (ProjectileShoot == 1)
                {
                    internalAI[3]++;
                    if (isSlashing)
                    {
                        if (Frame < 7 || Frame > 9) //Sets to frame 16
                        {
                            npc.frameCounter = 0;
                            Frame = 7;
                        }
                    }
                    else
                    {
                        if (Frame > 3)
                        {
                            npc.frameCounter = 0;
                            Frame = 0;
                        }
                    }
                }
            }
            else if (internalAI[0] == AISTATE_SLASH)
            {
                if (Frame < 17)
                {
                    npc.frameCounter = 0;
                    Frame = 17;
                }
                if (Frame > 26)
                {
                    npc.frameCounter = 0;
                    Frame = 17;
                }
            }
            else if (internalAI[0] == AISTATE_SPIN)
            {
                if (Frame < 10)
                {
                    npc.frameCounter = 0;
                    Frame = 10;
                }
                if (Frame > 16)
                {
                    npc.frameCounter = 0;
                    Frame = 13;
                }
            }
            else if (internalAI[0] == AISTATE_Shadowkilling)
            {
                if (Frame != 27)
                {
                    npc.frameCounter = 0;
                    Frame = 27;
                }
            }
            npc.frame.Y = Frame * frameHeight;
        }

        public void Shadowkilling()
        {
            double Pi = Math.PI;
            Vector2 playerLocation = Main.player[npc.target].position + new Vector2(Main.player[npc.target].width * 0.5f, Main.player[npc.target].height * 0.5f);
            
            Vector2[] spawnpoint = new Vector2[3];
            spawnpoint[0] = playerLocation + 275f * new Vector2((float)Math.Sin(0.5f * Pi), (float)Math.Cos(0.5f * Pi));
            spawnpoint[1] = playerLocation + 275f * new Vector2((float)Math.Sin(1.16f * Pi), (float)Math.Cos(1.13f * Pi));
            spawnpoint[2] = playerLocation + 275f * new Vector2((float)Math.Sin(1.83f * Pi), (float)Math.Cos(1.83f * Pi));

            if(!SpawnClone && (ShadowNPC[0] == -1 || ShadowNPC[1] == -1 || ShadowNPC[2] == -1))
            {
                return;
            }

            if(npc.alpha >= 255 && SpawnClone)
            {
                int k = Main.rand.Next(3);
                npc.position = spawnpoint[k];
                int k1 = k + 1;
                if (k1 >= 3)
                {
                    k1 = 0;
                }
                int k2 = k1 + 1;
                if (k2 >= 3)
                {
                    k2 = 0;
                }
                if(Main.netMode != 1)
                {
                    ShadowNPC[1] = NPC.NewNPC((int)spawnpoint[k1].X, (int)spawnpoint[k1].Y, mod.NPCType("HarukaClone"), 0, npc.whoAmI);
                    NetMessage.SendData(23, -1, -1, null, ShadowNPC[1], 0f, 0f, 0f, 0, 0, 0);
                    ShadowNPC[2] = NPC.NewNPC((int)spawnpoint[k2].X, (int)spawnpoint[k2].Y, mod.NPCType("HarukaClone"), 0, npc.whoAmI);
                    NetMessage.SendData(23, -1, -1, null, ShadowNPC[2], 0f, 0f, 0f, 0, 0, 0);
                }
                npc.netUpdate = true;
                npc.alpha = 250;
                if(ShadowNPC[1] != -1 && ShadowNPC[2] != -1)
                {
                    Main.npc[ShadowNPC[1]].alpha = npc.alpha;
                    Main.npc[ShadowNPC[2]].alpha = npc.alpha;
                    Main.npc[ShadowNPC[1]].boss = true;
                    Main.npc[ShadowNPC[2]].boss = true;
                    Main.npc[ShadowNPC[1]].life = Main.npc[ShadowNPC[0]].life;
                    Main.npc[ShadowNPC[2]].life = Main.npc[ShadowNPC[0]].life;
                }
                SpawnClone = false;
            }
            else if (npc.alpha > 0)
            {
                npc.velocity = Main.player[npc.target].velocity;
                Main.npc[ShadowNPC[1]].velocity = Main.player[npc.target].velocity;
                Main.npc[ShadowNPC[2]].velocity = Main.player[npc.target].velocity;

                

                npc.alpha -= 8;
                Main.npc[ShadowNPC[1]].alpha = npc.alpha;
                Main.npc[ShadowNPC[2]].alpha = npc.alpha;
                ShadowkingPosition = playerLocation;
            }
            else if(!npc.active || npc.life <= 0)
            {
                npc.velocity = Main.player[npc.target].velocity;
                Main.npc[ShadowNPC[1]].velocity = Main.player[npc.target].velocity;
                Main.npc[ShadowNPC[2]].velocity = Main.player[npc.target].velocity;

                Main.npc[ShadowNPC[1]].active = false;
                Main.npc[ShadowNPC[2]].active = false;
            }
            else
            {
                npc.alpha = 0;
                Main.npc[ShadowNPC[1]].alpha = npc.alpha;
                Main.npc[ShadowNPC[2]].alpha = npc.alpha;
                Vector2[] dist = new Vector2[3];
                int k = 0;
                while(k < 3)
                {
                    dist[k] = ShadowkingPosition - Main.npc[ShadowNPC[k]].Center;
                    if(k == 0) npc.velocity = Vector2.Normalize(dist[k]) * 15f;
                    else Main.npc[ShadowNPC[k]].velocity = Vector2.Normalize(dist[k]) * 15f;
                    if (ShadowkingPosition.X > Main.npc[ShadowNPC[k]].Center.X)
                    {
                        Main.npc[ShadowNPC[k]].direction = 1;
                    }
                    else
                    {
                        Main.npc[ShadowNPC[k]].direction = -1;
                    }
                    k++;
                }
            }

            if(internalAI[4] >= 90 || Main.npc[ShadowNPC[1]].Hitbox.Intersects(Main.npc[ShadowNPC[0]].Hitbox) || Main.npc[ShadowNPC[1]].Hitbox.Intersects(Main.npc[ShadowNPC[2]].Hitbox) || Main.npc[ShadowNPC[2]].Hitbox.Intersects(Main.npc[ShadowNPC[0]].Hitbox))
            {
                Vector2 shoot;
                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(ShadowkingPosition.X, ShadowkingPosition.Y, 0, 0, mod.ProjectileType("HarukaStrike"), damage*1, 5, Main.myPlayer);
                    int projType = mod.ProjectileType("HarukaProj");
                    for (int i = 0; i < 12; i++)
                    {
                        shoot = new Vector2((float)Math.Sin(i * (Pi / 6)), (float)Math.Cos(i * (Pi / 6)));
                        shoot *= 13f;
                        Projectile.NewProjectile(ShadowkingPosition.X, ShadowkingPosition.Y, shoot.X, shoot.Y, projType, damage * 1, 5, Main.myPlayer);
                    }
                }

                Main.npc[ShadowNPC[1]].boss = false;
                Main.npc[ShadowNPC[2]].boss = false;
                Main.npc[ShadowNPC[1]].active = false;
                Main.npc[ShadowNPC[2]].active = false;
                
                ShadowNPC[0] = -1;
                ShadowNPC[1] = -1;
                ShadowNPC[2] = -1;
                internalAI[4] = 0;
                ShadowkingPosition = Vector2.Zero;
                internalAI[0] = 3;
                npc.netUpdate = true;
            }
        }

        public void Backstab()
        {
            Vector2 playerLocation = Main.player[npc.target].Center;
            Vector2 playerVelocity = Main.player[npc.target].velocity;
            if(playerVelocity.X < 0)
            {
                npc.position.X = playerLocation.X - 250f;
                npc.position.Y = playerLocation.Y;
            }
            else if(playerVelocity.X > 0)
            {
                npc.position.X = playerLocation.X + 250f;
                npc.position.Y = playerLocation.Y;
            }
            else
            {
                npc.position.X = playerLocation.X - Main.player[npc.target].direction * 250f;
                npc.position.Y = playerLocation.Y;
            }
        }

        public void LOOPPOINT(Vector2 point1, Vector2 point2)
        {
            Shadowdashcounter ++;

            if(Shadowdashcounter < 40)
            {
                MoveToPoint(point2);
            }
            else if(Shadowdashcounter < 80)
            {
                MoveToPoint(point1);
            }
            else
            {
                Shadowdashcounter = 0;
            }

            if(Math.Abs(npc.Center.Y - point1.Y) < 150f)
            {
                MoveToPoint(point2);
                Shadowdashcounter = 0;
            }
            else if(Math.Abs(npc.Center.Y - point2.Y) < 150f)
            {
                MoveToPoint(point1);
                Shadowdashcounter = 41;
            }
            
        }

        public void MoveToPoint(Vector2 point)
        {
            float moveSpeed = 16f;
            if (internalAI[0] == AISTATE_SPIN)
            {
                moveSpeed = 20f;
            }
            if (Vector2.Distance(npc.Center, point) > 500)
            {
                moveSpeed = 25f;
            }
            if (internalAI[0] == AISTATE_SLASH)
            {
                moveSpeed = 25f;
                if(SHADOWCONTER > 0)
                {
                    moveSpeed = 70f;
                }
            }
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
            if (length < 10f)
            {
                moveSpeed *= 0.01f;
            }
            npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            npc.velocity *= moveSpeed;
            npc.velocity *= velMultiplier;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = 0;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if(strikebackproj > 0 && internalAI[0] == AISTATE_SPIN)
            {
                damage = 0.0;
            }
            return false;
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D glowTex = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/Haruka_Glow");
            Texture2D Slash = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/AH/Haruka/HarukaSlash");
            if (internalAI[0] == AISTATE_SPIN)
            {
                if(strikebackproj > 0)
                BaseDrawing.DrawAfterimage(spritebatch, Main.npcTexture[npc.type], 0, npc, 1.5f, 1f, 3, false, 0f, 0f, Color.Red);
                else
                BaseDrawing.DrawAfterimage(spritebatch, Main.npcTexture[npc.type], 0, npc, 1.5f, 1f, 3, false, 0f, 0f, Color.Navy);
            }
            else if(internalAI[0] == AISTATE_Shadowkilling)
            {
                BaseDrawing.DrawAfterimage(spritebatch, Main.npcTexture[npc.type], 0, npc, 1.5f, 1f, 3, false, 0f, 0f, Color.Navy);
            }
            else if(SHADOWDOG && !Invisible)
            {
                Vector2 Position = npc.position;
                Position.X = npc.position.X + (SHADOWCONTER > 60? 60:SHADOWCONTER) * 1f;
                Color Alpha = dColor;
                Alpha.R = (byte)((float)(255 - (SHADOWCONTER > 60? 60:SHADOWCONTER) * 3));
                Alpha.G = (byte)((float)(255 - (SHADOWCONTER > 60? 60:SHADOWCONTER) * 3));
                Alpha.B = (byte)((float)(255 - (SHADOWCONTER > 60? 60:SHADOWCONTER) * 3));
                Alpha.A = (byte)((float)(255 - (SHADOWCONTER > 60? 60:SHADOWCONTER) * 3));
                BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, Position, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 28, npc.frame, npc.GetAlpha(Alpha), false);
                Position.X = npc.position.X - (SHADOWCONTER > 60? 60:SHADOWCONTER) * 1f;
                BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, Position, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 28, npc.frame, npc.GetAlpha(Alpha), false);
            }
            if(internalAI[0] == AISTATE_SLASH && SHADOWCONTER > 0)
            {
                BaseDrawing.DrawAfterimage(spritebatch, Main.npcTexture[npc.type], 0, npc, 1.5f, 1f, 3, false, 0f, 0f, Color.Navy);
            }
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 28, npc.frame, npc.GetAlpha(dColor), false);
            if (Invisible) return false;
            
            BaseDrawing.DrawTexture(spritebatch, Slash, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 28, npc.frame, dColor, false);
            BaseDrawing.DrawTexture(spritebatch, glowTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 28, npc.frame, Color.White, false);
            return false;
        }
    }
}