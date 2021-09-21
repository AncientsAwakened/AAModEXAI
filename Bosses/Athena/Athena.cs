using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using System.IO;
using AAMod;
using AAMod.NPCs.Enemies.Sky;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Athena
{
    [AutoloadBossHead]
    public class Athena : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 7;
        }

        public static Point CloudPoint = new Point((int)(Main.maxTilesX * 0.65f), 100);
        public Vector2 Origin = new Vector2((int)(Main.maxTilesX * 0.65f), 100) * 16;
        public int damage = 0;

        public override void SetDefaults()
        {
            npc.width = 152;
            npc.height = 114;
            npc.value = BaseUtility.CalcValue(0, 10, 0, 0);
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.lifeMax = 40000;
            npc.defense = 20;
            npc.damage = 90;
            if(NPC.downedMoonlord)
            {
                npc.damage = 100;
                npc.defense = 45;
                npc.lifeMax = 80000;
            }
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.boss = true;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Athena");
            npc.alpha = 255;
            npc.noTileCollide = true;
            bossBag = ModLoader.GetMod("AAMod").ItemType("AthenaBag");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public float[] internalAI = new float[5];
        public float[] FlyAI = new float[2];
        public Vector2 MoveVector2;
        public bool Seen = false;

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
                writer.Write(FlyAI[0]);
                writer.Write(FlyAI[1]);
                writer.Write(MoveVector2.X);
                writer.Write(MoveVector2.Y);
                writer.Write(Seen);
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
                FlyAI[0] = reader.ReadFloat();
                FlyAI[1] = reader.ReadFloat();
                MoveVector2.X = reader.ReadFloat();
                MoveVector2.Y = reader.ReadFloat();
                Seen = reader.ReadBool();
            }
        }
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
            Player player = Main.player[npc.target];
            AAMod.AAPlayer modPlayer = player.GetModPlayer<AAMod.AAPlayer>();

            Vector2 Acropolis = new Vector2(Origin.X + (80 * 16), Origin.Y + (79 * 16));

            //Preamble Shite 

            if (internalAI[2] != 1)
            {
                npc.dontTakeDamage = true;
                music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
                if (Vector2.Distance(npc.Center, Acropolis) < 10)
                {
                    npc.velocity *= 0;

                    if (Seen)
                    {
                        if (player.Center.X < npc.Center.X + 32)
                        {
                            npc.direction = -1;
                        }
                        else
                        {
                            npc.direction = 1;
                        }
                    }

                    if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && internalAI[3] < 180)
                    {
                        Seen = true;
                        npc.netUpdate = true;
                    }

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (!Seen)
                        {
                            internalAI[4]++; 
                            if (internalAI[4] == 60)
                            {
                                CombatText.NewText(npc.Hitbox, Color.CadetBlue, "..."); 
                            }

                            if (internalAI[4] == 180)
                            {
                                CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.EnemyChat("AthenaChat1"));
                            }

                            if (internalAI[4] >= 300)
                            {
                                CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.EnemyChat("AthenaChat2"));
                                npc.active = false;
                                int p = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("AthenaFlee"));
                                Main.npc[p].Center = npc.Center;
                            }
                            return;
                        }

                        if (internalAI[3]++ < 420)
                        {
                            if (!AAWorld.downedAthena)
                            {

                                if (internalAI[3] == 60)
                                {
                                    CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena1"));
                                }

                                if (internalAI[3] == 180)
                                {
                                    string s = "";
                                    if (Main.ActivePlayersCount > 1)
                                    {
                                        s = AAMod.Lang.BossChat("Athena2");
                                    }
                                    CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena3") + s + "!");
                                }

                                if (internalAI[3] == 300)
                                {
                                    CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena4"));
                                }

                                if (internalAI[3] == 420)
                                {
                                    CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena5"));
                                }

                                if (internalAI[3] >= 420)
                                {
                                    CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena6"));
                                    internalAI[2] = 1;

                                    npc.netUpdate = true;
                                }
                            }
                            else if (AAWorld.AthenaHerald && !AAWorld.downedAthenaA)
                            {
                                if (internalAI[3] == 60)
                                {
                                    CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena12"));
                                }

                                if (internalAI[3] == 180)
                                {
                                    CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena13"));
                                }

                                if (internalAI[3] == 300)
                                {
                                    CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena14"));
                                    internalAI[2] = 1;
                                    npc.netUpdate = true;
                                }
                            }
                            else
                            {
                                if (internalAI[3] == 60)
                                {
                                    CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena7"));
                                }

                                if (internalAI[3] >= 180)
                                {
                                    CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena8"));
                                    internalAI[2] = 1;
                                    npc.netUpdate = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    npc.spriteDirection = npc.direction = npc.velocity.X > 0 ? 1 : -1;
                    MoveToVector2(Acropolis);
                }
            }
            else
            {
                if (player.Center.X < npc.Center.X + 32)
                {
                    npc.direction = -1;
                }
                else
                {
                    npc.direction = 1;
                }

                npc.dontTakeDamage = false;
                if (player.dead || !player.active || Vector2.Distance(npc.position, player.position) > 5000 || !modPlayer.ZoneAcropolis)
                {
                    npc.TargetClosest();
                    if (player.dead || !player.active || Math.Abs(Vector2.Distance(npc.position, player.position)) > 5000 || !modPlayer.ZoneAcropolis)
                    {
                        CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena9"));
                        int p = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("AthenaFlee"));
                        Main.npc[p].Center = npc.Center;
                        npc.active = false;
                        npc.netUpdate = true;
                    }
                }

                music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Athena");

                if (internalAI[0]++ > 300 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int pChoice = Main.rand.Next(2);
                    if (pChoice == 0)
                    {
                        NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("OwlRune"));
                    }
                    internalAI[0] = 0;
                }

                if (internalAI[1] == 0) //Acropolis Phase
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        npc.ai[3]++;
                    }

                    if (Vector2.Distance(player.Center, Acropolis) > 1280)
                    {
                        if (npc.ai[2] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            npc.ai[2] = 1;
                            npc.netUpdate = true;
                        }
                        MoveToVector2(Acropolis);
                    }
                    else
                    {
                        if (npc.ai[2] == 1 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            npc.ai[2] = 0;
                            npc.netUpdate = true;
                        }
                        BaseAI.AISpaceOctopus(npc, ref FlyAI, Main.player[npc.target].Center, 0.1f, 8f, 220f, 70f, ShootFeather);
                    }

                    if (npc.ai[3] > 600)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            internalAI[1] = 1;
                            npc.ai[0] = 0;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = 0;
                            MoveVector2 = CloudPick();
                            npc.netUpdate = true;
                        }
                    }
                }
                else //Cloud Phase
                {
                    if (MoveVector2 == new Vector2(0, 0) && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        MoveVector2 = CloudPick();
                        npc.netUpdate = true;
                    }
                    npc.ai[1]++;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (npc.ai[1] == 300)
                        {
                            if (Main.rand.Next(5) == 0)
                            {
                                internalAI[1] = 0;
                                npc.ai[0] = 0;
                                npc.ai[1] = 0;
                                npc.ai[2] = 0;
                                npc.ai[3] = 0;
                                npc.netUpdate = true;
                                return;
                            }
                            npc.ai[0] = 0;
                            MoveVector2 = CloudPick();
                            npc.netUpdate = true;
                        }
                    }
                    if (Vector2.Distance(npc.Center, MoveVector2) < 10)
                    {
                        if (npc.ai[2] == 1 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.netUpdate = true;
                        }
                        npc.velocity *= 0;

                        if (npc.ai[1] % 200 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int Choice = Main.rand.Next(2);
                            if (Choice == 0)
                            {
                                NPC.NewNPC((int)npc.Center.X + 100, (int)npc.Center.Y, mod.NPCType("OlympianDragon"));
                                NPC.NewNPC((int)npc.Center.X - 100, (int)npc.Center.Y, mod.NPCType("OlympianDragon"));
                            }
                            else
                            {
                                NPC Seraph1 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + 100, mod.NPCType("SeraphA"))];
                                for (int i = 0; i < 3; i++)
                                {
                                   Dust d = Main.dust[Dust.NewDust(Seraph1.position, Seraph1.height, Seraph1.width, ModContent.DustType<Feather>(), Main.rand.Next(-1, 2), 1, 0)];
                                }
                                NPC Seraph2 = Main.npc[NPC.NewNPC((int)npc.Center.X + 100, (int)npc.Center.Y - 50, mod.NPCType("SeraphA"))];
                                for (int i = 0; i < 3; i++)
                                {
                                    Dust d = Main.dust[Dust.NewDust(Seraph2.position, Seraph2.height, Seraph2.width, ModContent.DustType<Feather>(), Main.rand.Next(-1, 2), 1, 0)];
                                }
                                NPC Seraph3 = Main.npc[NPC.NewNPC((int)npc.Center.X + 100, (int)npc.Center.Y - 50, mod.NPCType("SeraphA"))];
                                for (int i = 0; i < 3; i++)
                                {
                                    Dust d = Main.dust[Dust.NewDust(Seraph3.position, Seraph3.height, Seraph3.width, ModContent.DustType<Feather>(), Main.rand.Next(-1, 2), 1, 0)];
                                }
                            }
                            npc.netUpdate = true;
                        }

                        if (npc.ai[1] % 60 == 0)
                        {
                            if (Vector2.Distance(player.Center, npc.Center) < 900)
                            {
                                ShootFeather(npc, npc.velocity);
                            }
                        }
                    }
                    else
                    {
                        if (npc.ai[2] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            npc.ai[2] = 1;
                            npc.netUpdate = true;
                        }
                        MoveToVector2(MoveVector2);
                    }
                }
            }

            npc.rotation = 0;
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

        public void ShootFeather(NPC npc, Vector2 velocity)
        {
            Player player = Main.player[npc.target];
            int projType = mod.ProjectileType("SeraphFeather");
            float spread = 30f * 0.0174f;
            Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
            dir *= 14f;
            float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
            double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
            double deltaAngle = spread / 6f;
            for (int i = 0; i < 3; i++)
            {
                double offsetAngle = startAngle + (deltaAngle * i);
                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), projType, damage, 2, Main.myPlayer);
                Main.projectile[p].tileCollide = false;
            }
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
            float moveSpeed = 25f;
            if (internalAI[2] != 1)
            {
                moveSpeed = 14f;
            }
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

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void NPCLoot()
        {
            AAWorld.downedAthena = true;

            if (NPC.downedMoonlord)
            {
                if (!AAWorld.downedAthenaA)
                {
                    int a = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("AthenaDefeat"));
                    Main.npc[a].Center = npc.Center;
                }
                else
                {
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
                    int a = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("AthenaA"));
                    Main.npc[a].Center = npc.Center;
                    int b = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<Effects.ShockwaveBoom>(), 0, 1, Main.myPlayer, 0, 0);
                    Main.projectile[b].Center = npc.Center;
                    CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena10"));

                    Main.projectile[b].netUpdate = true;
                }
                return;
            }

            if (Main.expertMode)
            {
                for(int i = 0; i < 10; i++) npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("AthenaMask"));
                }
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("GoddessFeather"), Main.rand.Next(20, 25));
                string[] lootTable = { "DivineWindCharm", "GaleOfWings", "RazorwindLongbow", "SkycutterKopis", "OlympianWings"};
                int loot = Main.rand.Next(lootTable.Length);
                npc.DropLoot(ModLoader.GetMod("AAMod").ItemType(lootTable[loot]));
            }


            CombatText.NewText(npc.Hitbox, Color.CadetBlue, AAMod.Lang.BossChat("Athena11"));
            int p = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("AthenaFlee"));
            Main.npc[p].Center = npc.Center;
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D tex = internalAI[2] != 1 ? mod.GetTexture("Bosses/Athena/SassyBitch") : Main.npcTexture[npc.type];
            Color lightColor = BaseDrawing.GetLightColor(npc.Center);

            if (npc.ai[2] == 1)
            {
                BaseDrawing.DrawAfterimage(sb, tex, 0, npc.position, npc.width, npc.height, npc.oldPos, npc.scale, npc.rotation, npc.direction, 7, npc.frame, 1f, 1f, 5, false, 0f, 0f);
            }
            BaseDrawing.DrawTexture(sb, tex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 7, npc.frame, lightColor);
            return false;
        }
    }

    public class AthenaFlee : ModNPC
    {
        public override string Texture => "AAModEXAI/Bosses/Athena/Athena";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Athena");
            Main.npcFrameCount[npc.type] = 7;
        }
        public override void SetDefaults()
        {
            npc.width = 152;
            npc.height = 114;
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.defense = 1;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            npc.damage = 0;
            npc.value = 0;
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[0]++ >= 120)
            {
                if (npc.ai[0] >= 120 && npc.ai[0] < 130)
                {
                    npc.velocity.Y += 1f;
                    npc.netUpdate = true;
                }
                else if (npc.ai[0] == 130)
                {
                    npc.netUpdate = true;
                }
                else if (npc.ai[0] >= 130)
                {
                    npc.velocity.Y -= 0.5f;
                    if (npc.velocity.Y < -8f) npc.velocity.Y = -8f;
                }
                if (npc.position.Y + npc.velocity.Y <= 0f && Main.netMode != NetmodeID.MultiplayerClient) { BaseAI.KillNPC(npc); npc.netUpdate = true; }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 6)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * Main.npcFrameCount[npc.type])
            {
                npc.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            BaseDrawing.DrawAfterimage(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.oldPos, npc.scale, npc.rotation, npc.direction, 7, npc.frame, 1f, 1f, 5, false, 0f, 0f);
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 7, npc.frame, npc.GetAlpha(lightColor), false);
            return false;
        }
    }
}