using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria.Graphics.Shaders;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    [AutoloadBossHead]
    public class ZeroProtocol : ModNPC
    {
        public int timer;
        public static int type;
        public int damage = 0;
        public bool PlayerDead = false;

        public bool Counterattack = false;
        public int deathTimer = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ZER0 PR0T0C0L");
            Main.npcFrameCount[npc.type] = 7; 
            NPCID.Sets.TrailCacheLength[npc.type] = 20;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }
        public override void SetDefaults()
        {
            npc.lifeMax = 600000;
			npc.damage = 200;
			npc.defense = 70;
            npc.knockBackResist = 0f;
            npc.width = 170;
            npc.height = 170;
            npc.friendly = false;
            npc.aiStyle = 0;
            npc.value = Item.sellPrice(0, 40, 0, 0);
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.NPCHit, "Sounds/Sounds/Zerohit");
            npc.DeathSound = ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/ZeroDeath");
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Zero2");
            musicPriority = MusicPriority.BossHigh;
            npc.netAlways = true;
            bossBag = ModLoader.GetMod("AAMod").ItemType("ZeroBag");
            npc.npcSlots = 200;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * .7f);
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
        }

        public float[] Move = new float[4];
        public float[] start = new float[1];
        public float[] Minion = new float[1];
        public int[] Counter = new int[3];

        Vector2 ShootDir = new Vector2(0,0);

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Move[0]);
                writer.Write(Move[1]);
                writer.Write(Move[2]);
                writer.Write(Move[3]);
                writer.Write(Counter[0]); //the hit TP counter
                writer.Write(Counter[1]); //the charge counter
                writer.Write(Counter[2]); //the pointchange counter
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Move[0] = reader.ReadFloat();
                Move[1] = reader.ReadFloat();
                Move[2] = reader.ReadFloat();
                Move[3] = reader.ReadFloat();
                Counter[0] = reader.ReadInt();
                Counter[1] = reader.ReadInt();
                Counter[2] = reader.ReadInt();
            }
        }

        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                if (!AAWorld.downedZero)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("ZeroAwakened1"), Color.PaleVioletRed);
                    Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("ZeroRune"));
                }

                AAWorld.downedZero = true;

                if (Main.rand.Next(10) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("ZeroTrophy"));
                }
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("ZeroMask"));
                }
                if (Main.rand.Next(50) == 0 && AAWorld.downedShen)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("EXSoul"));
                }
                for(int i = 0; i < 10; i++) npc.DropBossBags();
                return;
            }
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            if (Main.expertMode)
            {
                potionType = ItemID.SuperHealingPotion;
                Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0f, 0f), mod.ProjectileType("ZeroDeath1"), 0, 0);
            }
            else
            {
                potionType = 0;
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Counter[0] > 3000 && npc.ai[0] != 4 && npc.ai[0] != 2 && !isCharging)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Teleport(Main.rand.Next(2) == 0? 1:2);
                    npc.ai[0] = 4;
                    npc.ai[1] = 0;
                    npc.ai[2] = 0;
                    npc.ai[3] = 0;
                    Counter[0] = 0;
                    Counterattack = true;
                    npc.netUpdate = true;
                }
            }
            else if(npc.ai[0] != 4 && npc.ai[0] != 2 && !isCharging)
            {
                int TeleportChance = 1000 * (npc.life / npc.lifeMax);
                if (TeleportChance < 10)
                {
                    TeleportChance = 10;
                }
                if (Main.rand.Next(TeleportChance) == 0)
                {
                    Teleport(0);
                }
            }
            if (npc.life <= 0 && !Main.expertMode)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroAwakened4"), Color.Red.R, Color.Red.G, Color.Red.B);
            }
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            Texture2D tex = Main.npcTexture[npc.type];
            Texture2D afterimage = mod.GetTexture("Bosses/Zero/Protocol/ZeroTrail");
            Texture2D glowTex = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/ZeroProtocol_Glow");
            if (isCharging)
            {
                tex = mod.GetTexture("Bosses/Zero/Protocol/ZeroProtocolCharge");
                afterimage = mod.GetTexture("Bosses/Zero/Protocol/ZeroProtocolChargeTrail");
                glowTex = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/ZeroProtocol_Glow");
            }
            
            if(!(npc.ai[0] == 4 && NPC.CountNPCS(mod.NPCType("ZeroEcho")) > 0 && !Counterattack))
            {
                BaseDrawing.DrawAfterimage(spritebatch, afterimage, 0, npc, 1, 1, 8, true, 0, 0, Color.Black, npc.frame, 7);
                BaseDrawing.DrawTexture(spritebatch, tex, 0, npc, dColor);
                npc.height = 120;
                if(!isCharging)
                {
                    npc.height = 170;
                    BaseDrawing.DrawAura(spritebatch, glowTex, 0, npc.position, npc.width, npc.height, auraPercent, 1f, 1f, npc.rotation, npc.direction, 7, npc.frame, 0f, 0f, AAColor.Oblivion);
                    BaseDrawing.DrawTexture(spritebatch, glowTex, 0, npc, AAColor.Oblivion);
                }
            }
            
            return false;
        }

        bool isCharging = false;

        public override void AI()
        {
            if((NPC.CountNPCS(mod.NPCType("ZeroEcho")) > 0) || isCharging || npc.ai[0] == 2 || npc.ai[0] == 4)
            {
                npc.chaseable = false;
                npc.defense = 9999;
            }
            else
            {
                npc.chaseable = true;
                npc.defense = npc.defDefense;
            }
            
            int Repeats;
            if (npc.life < npc.life * (2 / 3))
            {
                Counter[0] ++;
                Counter[1] ++;
                Repeats = 4;
            }
            else if (npc.life < npc.life / 3)
            {
                Counter[0] ++;
                Counter[1] ++;
                Repeats = 5;
            }
            else
            {
                Counter[0] += 2;
                Counter[1] += 2;
                Repeats = 3;
            }
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];

            if (!AliveCheck(player)) return;

            if(Counter[0] >= 4000)
            {
                Counter[0] = 4000;
            }

            if(Counter[1] >= 6000)
            {
                Counter[1] = 0;
                isCharging = true;
                npc.ai[0] = 5;
                npc.ai[1] = 0;
                npc.ai[2] = 0;
                npc.ai[3] = 0;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                AAWorld.zeroUS = false;
            }

            for (int m = npc.oldPos.Length - 1; m > 0; m--)
            {
                npc.oldPos[m] = npc.oldPos[m - 1];
            }
            npc.oldPos[0] = npc.position;

            int Changerate = npc.life < npc.lifeMax / 2 ? 150 : 120;

            if (npc.ai[2]++ > Changerate && !Counterattack)
            {
                if (npc.ai[0] != 0)
                {
                    npc.velocity *= .0f;
                }
                switch (npc.ai[0])
                {
                    case 0:
                        if (!AliveCheck(player))
                            break;
                        float spread = 45f * 0.0174f;
                        Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                        dir *= 12f;
                        float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                        double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                        double deltaAngle = spread / 6f;
                        if (npc.ai[2] % 30 == 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (npc.ai[2] % Main.rand.Next(10) == 0 && Main.rand.Next(2) == 0)
                                {
                                    double offsetAngle = startAngle + (deltaAngle * i);
                                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), mod.ProjectileType("StaticSphere"), npc.damage / 4, 5, Main.myPlayer);
                                }
                            }
                        }
                        if (npc.ai[2] > 271)
                        {
                            AIChange();
                        }
                        break;
                    case 1:
                        if (!AliveCheck(player))
                            break;
                        if (npc.ai[2] % 30 == 0 && npc.ai[2] < 121)
                        {
                            Teleport(3);
                        }

                        if (npc.ai[2] % 60 == 30)
                        {
                            Attack(Main.rand.Next(4));
                        }

                        if (npc.ai[3] < Repeats && npc.ai[2] > 280)
                        {
                            npc.ai[3]++;
                            npc.ai[2] = Changerate;
                        }
                        else
                        {
                            AIChange();
                        }

                        break;
                    case 2:
                        if (!AliveCheck(player))
                            break;
                        npc.velocity *= 0;
                        if (npc.ai[2] == 280)
                        {
                            Teleport(3);
                            if (npc.life > npc.lifeMax / 2)
                            {
                                if(Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    if (Main.rand.Next(2) == 0)
                                    {
                                        int dirY = player.velocity.Y > 0? 1:-1;

                                        int yPos = Math.Abs(player.velocity.Y) > 4f? -500 * dirY : -750 * dirY;

                                        for (int z = 0; z < 7; z++)
                                        {
                                            int a1 = Projectile.NewProjectile(new Vector2(player.Center.X, player.Center.Y), Vector2.Zero, mod.ProjectileType("Blast"), damage, 3, Main.myPlayer, 0f, 0f);
                                            int a2 = Projectile.NewProjectile(new Vector2(player.Center.X, player.Center.Y), Vector2.Zero, mod.ProjectileType("Blast"), damage, 3, Main.myPlayer, 1f, 0f);
                                            Main.projectile[a1].Center = player.Center + new Vector2(-500, yPos);
                                            Main.projectile[a2].Center = player.Center + new Vector2(500, yPos);
                                            yPos += 250 * dirY;
                                        }
                                    }
                                    else
                                    {
                                        int dirX = player.velocity.X > 0? 1:-1;

                                        int xPos = Math.Abs(player.velocity.X) > 4f? -500 * dirX : -750 * dirX;

                                        for (int z = 0; z < 7; z++)
                                        {
                                            int h1 = Projectile.NewProjectile(new Vector2(player.Center.X, player.Center.Y), Vector2.Zero, mod.ProjectileType("Blast"), damage, 3, Main.myPlayer , 2f, 0f);
                                            int h2 = Projectile.NewProjectile(new Vector2(player.Center.X, player.Center.Y), Vector2.Zero, mod.ProjectileType("Blast"), damage, 3, Main.myPlayer , 3f, 0f);
                                            Main.projectile[h1].Center = player.Center + new Vector2(xPos, -500);
                                            Main.projectile[h2].Center = player.Center + new Vector2(xPos, 500);
                                            xPos += 250 * dirX;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if(Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    int dirX = player.velocity.X > 0? 1:-1;
                                    int dirY = player.velocity.Y > 0? 1:-1;

                                    int xPos = Math.Abs(player.velocity.X) > 4f? -500 * dirX : -750 * dirX;
                                    int yPos = Math.Abs(player.velocity.Y) > 4f? -500 * dirY : -750 * dirY;

                                    for (int z = 0; z < 13; z++)
                                    {
                                        int a1 = Projectile.NewProjectile(new Vector2(player.Center.X, player.Center.Y), Vector2.Zero, mod.ProjectileType("Blast"), damage, 3, Main.myPlayer, 0f, 0f);
                                        int a2 = Projectile.NewProjectile(new Vector2(player.Center.X, player.Center.Y), Vector2.Zero, mod.ProjectileType("Blast"), damage, 3, Main.myPlayer, 1f, 0f);
                                        Main.projectile[a1].Center = player.Center + new Vector2(-500, yPos);
                                        Main.projectile[a2].Center = player.Center + new Vector2(500, yPos);
                                        yPos += 250 * dirY;
                                    }
                                    for (int z = 0; z < 13; z++)
                                    {
                                        int h1 = Projectile.NewProjectile(new Vector2(player.Center.X, player.Center.Y), Vector2.Zero, mod.ProjectileType("Blast"), damage, 3, Main.myPlayer , 2f, 0f);
                                        int h2 = Projectile.NewProjectile(new Vector2(player.Center.X, player.Center.Y), Vector2.Zero, mod.ProjectileType("Blast"), damage, 3, Main.myPlayer , 3f, 0f);
                                        Main.projectile[h1].Center = player.Center + new Vector2(xPos, -500);
                                        Main.projectile[h2].Center = player.Center + new Vector2(xPos, 500);
                                        xPos += 250 * dirX;
                                    }
                                }
                            }
                        }
                        if (npc.ai[2] > 520)
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = 0;
                            npc.netUpdate = true;
                        }
                        break;
                    case 3:
                        if (!AliveCheck(player))
                            break;
                        if (npc.ai[2] == (npc.life < npc.lifeMax / 2 ? 200 : 300))
                        {
                            Teleport(3);
                            if(Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                int a = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0f, -12f), mod.ProjectileType("ProtoStar"), damage, 3);
                                Main.projectile[a].Center = npc.Center + new Vector2(-100, 0);
                                int b = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0f, 12f), mod.ProjectileType("ProtoStar"), damage, 3);
                                Main.projectile[b].Center = npc.Center + new Vector2(100, 0);
                                int c = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(-12f, 0), mod.ProjectileType("ProtoStar"), damage, 3);
                                Main.projectile[c].Center = npc.Center + new Vector2(0, 100);
                                int d = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(12f, 0), mod.ProjectileType("ProtoStar"), damage, 3);
                                Main.projectile[d].Center = npc.Center + new Vector2(0, -100);
                                if (npc.life < npc.lifeMax / 2)
                                {
                                    int a1 = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0f, 12f), mod.ProjectileType("ProtoStar"), damage, 3);
                                    Main.projectile[a1].Center = npc.Center + new Vector2(-100, 0);
                                    int b1 = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0f, -12f), mod.ProjectileType("ProtoStar"), damage, 3);
                                    Main.projectile[b1].Center = npc.Center + new Vector2(100, 0);
                                    int c1 = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(12f, 0), mod.ProjectileType("ProtoStar"), damage, 3);
                                    Main.projectile[c1].Center = npc.Center + new Vector2(0, 100);
                                    int d1 = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(-12f, 0), mod.ProjectileType("ProtoStar"), damage, 3);
                                    Main.projectile[d1].Center = npc.Center + new Vector2(0, -100);
                                    int e = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(8f, -8f), mod.ProjectileType("ProtoStar"), damage, 3);
                                    Main.projectile[e].Center = npc.Center + new Vector2(-80, -80);
                                    int e1 = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(-8f, 8f), mod.ProjectileType("ProtoStar"), damage, 3);
                                    Main.projectile[e1].Center = npc.Center + new Vector2(-80, -80);
                                    int f = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(-8f, 8f), mod.ProjectileType("ProtoStar"), damage, 3);
                                    Main.projectile[f].Center = npc.Center + new Vector2(80, 80);
                                    int f1 = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(8f, -8f), mod.ProjectileType("ProtoStar"), damage, 3);
                                    Main.projectile[f1].Center = npc.Center + new Vector2(80, 80);
                                    int g = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(-8f, -8f), mod.ProjectileType("ProtoStar"), damage, 3);
                                    Main.projectile[g].Center = npc.Center + new Vector2(-80, 80);
                                    int g1 = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(8f, 8f), mod.ProjectileType("ProtoStar"), damage, 3);
                                    Main.projectile[g1].Center = npc.Center + new Vector2(-80, 80);
                                    int h = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(8f, 8f), mod.ProjectileType("ProtoStar"), damage, 3);
                                    Main.projectile[h].Center = npc.Center + new Vector2(80, -80);
                                    int h1 = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), new Vector2(-8f, -8f), mod.ProjectileType("ProtoStar"), damage, 3);
                                    Main.projectile[h1].Center = npc.Center + new Vector2(80, -80);
                                }
                            }
                        }

                        if (npc.ai[2] > (npc.life < npc.lifeMax / 2 ? 260 : 360))
                        {
                            AIChange();
                        }

                        break;
                    case 4:
                        if (!AliveCheck(player))
                            break;
                        if (npc.ai[2] < (npc.life < npc.lifeMax / 2 ? 360 : 270))
                        {
                            if (npc.ai[3] ++  > 60)
                            {
                                npc.ai[3] = 0;
                                Teleport(0);
                                NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ZeroEcho"));
                            }
                        }
                        if(npc.ai[2] == (npc.life < npc.lifeMax / 2 ? 400 : 310))
                        {
                            npc.ai[1] = 1f;
                        }
                        else
                        {
                            npc.ai[1] = 0f;
                        }
                        if(npc.ai[2] >= (npc.life < npc.lifeMax / 2 ? 480 : 390))
                        {
                            npc.ai[0] = 1;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = 0;
                        }
                        break;

                    case 5:
                        if (!AliveCheck(player))
                            break;
                        
                        Counterattack = false;
                        
                        if (npc.ai[1]++ == 100)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroAwakened11"), Color.Red.R, Color.Red.G, Color.Red.B);
                            if (ShootDir == new Vector2(0,0)) ShootDir = npc.DirectionTo(player.Center.RotatedBy(1.57f));
                            Projectile.NewProjectile(npc.Center + 60f * ShootDir, 10f * ShootDir, mod.ProjectileType("EchoRay"), npc.damage, 3f, Main.myPlayer, -6.2831855f / 450f, npc.whoAmI);
                            npc.ai[3] = 1f;
                        }
                        else if(npc.ai[1] > 100 && npc.ai[1] < 190)
                        {
                            npc.ai[3] = 0f;
                            npc.rotation -= 6.2831855f / 450f;
                        }
                        else
                        {
                            npc.ai[3] = 0f;
                        }

                        if(npc.ai[1] < 85)
                        {
                            if(npc.ai[2] % (npc.life < npc.lifeMax / 2? 40:60) == 10)
                            {
                                Teleport(3);
                                NPC.NewNPC((int)player.Center.X + 50 * Main.rand.Next(4, 6) * (Main.rand.Next(2) == 0? -1:1), (int)player.Center.Y + 50 * Main.rand.Next(4, 6) * (Main.rand.Next(2) == 0? -1:1), mod.NPCType("ZeroMini"));
                            }
                            npc.rotation = npc.DirectionTo(player.Center.RotatedBy(1.57f)).ToRotation() + (float)Math.PI/2;
                            if(npc.rotation > 2 * Math.PI) npc.rotation -= 6.2831855f;
                            ShootDir = npc.DirectionTo(player.Center.RotatedBy(1.57f));
                        }

                        if (npc.ai[1] >= 190)
                        {
                            isCharging = false;
                            npc.ai[0] = 1;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = 0;
                        }
                        break;
                    
                    default:
                        if (!AliveCheck(player))
                            break;
                        npc.ai[0] = 0;
                        goto case 0;
                }
            }
            else if(isCharging)
            {
                if(npc.ai[2] == 10)
                {

                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroAwakened8"), Color.Red.R, Color.Red.G, Color.Red.B);
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroAwakened9"), Color.Red.R, Color.Red.G, Color.Red.B);
                }
                if(npc.ai[2] == 40)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroAwakened10"), Color.Red.R, Color.Red.G, Color.Red.B);
                }
                if(npc.ai[2] == 110)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(@"[Y]", Color.Red.R, Color.Red.G, Color.Red.B);
                }
                npc.rotation = npc.DirectionTo(player.Center).ToRotation() + (float)Math.PI/2;
                if(npc.ai[2] % (npc.life < npc.lifeMax / 2? 60:80) == 20)
                {
                    Teleport(3);
                    if(Main.netMode != NetmodeID.MultiplayerClient) NPC.NewNPC((int)player.Center.X + 50 * Main.rand.Next(4, 6) * (Main.rand.Next(2) == 0? -1:1), (int)player.Center.Y + 50 * Main.rand.Next(4, 6) * (Main.rand.Next(2) == 0? -1:1), mod.NPCType("ZeroMini"));
                }
                Counterattack = false;
                npc.ai[1] = 0f;
                npc.ai[3] = 0f;
            }
            else if(Counterattack)
            {
                npc.ai[2] ++;
                npc.position = player.Center - new Vector2(0, 600);
                npc.velocity *= 0;

                if(npc.ai[2] == 240 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPC((int)player.Center.X, (int)player.Center.Y + 500, mod.NPCType("ZeroEcho"));
                    NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 500, mod.NPCType("ZeroEcho"));
                    NPC.NewNPC((int)player.Center.X + 500, (int)player.Center.Y, mod.NPCType("ZeroEcho"));
                    NPC.NewNPC((int)player.Center.X - 500, (int)player.Center.Y, mod.NPCType("ZeroEcho"));
                    NPC.NewNPC((int)player.Center.X + 500, (int)player.Center.Y + 500, mod.NPCType("ZeroEcho"));
                    NPC.NewNPC((int)player.Center.X - 500, (int)player.Center.Y - 500, mod.NPCType("ZeroEcho"));
                    NPC.NewNPC((int)player.Center.X + 500, (int)player.Center.Y - 500 , mod.NPCType("ZeroEcho"));
                    NPC.NewNPC((int)player.Center.X - 500, (int)player.Center.Y + 500, mod.NPCType("ZeroEcho"));
                }
                
                if(npc.ai[2] == 420)
                {
                    npc.ai[1] = 1f;
                }
                else
                {
                    npc.ai[1] = 0f;
                }

                if(npc.ai[2] > 520)
                {
                    Counterattack = false;
                    npc.ai[0] = 2;
                    npc.ai[1] = 0;
                    npc.ai[2] = 0;
                    npc.ai[3] = 0;
                }
                
            }
            else
            {
                BaseAI.AISkull(npc, ref Move, true, 14, 350, .04f, .05f);

                int Frequency = Main.rand.Next(30, 50);
                if (npc.life < npc.lifeMax / 2)
                {
                    Frequency = Main.rand.Next(20, 50);
                }
                if (npc.life < npc.lifeMax / 4)
                {
                    Frequency = Main.rand.Next(10, 40);
                }
                if (Main.rand.Next(2) == 0)
                {
                    BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, mod.ProjectileType("GlitchBomb"), ref npc.ai[3], Frequency, npc.damage / 3, 10, true);
                }
                else
                {
                    BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, mod.ProjectileType("GlitchRocket"), ref npc.ai[3], Frequency, npc.damage / 3, 10, true);
                }
            }

            if((npc.Center - player.Center).Length() > 2000 && !isCharging && !Counterattack)
            {
                Teleport(0);
            }

            if(npc.ai[0] != 5)
            {
                npc.direction = npc.spriteDirection = 1;
                npc.rotation = 0;
            }
        }

        private void AIChange()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.ai[0] = Main.rand.Next(5);
                npc.ai[1] = 0;
                npc.ai[2] = 0;
                if (npc.ai[0] == 2 || npc.ai[0] == 4)
                {
                    Teleport(Main.rand.Next(2) == 0? 1:2);
                }
                else if ((npc.life < npc.lifeMax * (3 / 4)) && Main.rand.Next(3) == 0)
                {
                    Teleport(Main.rand.Next(2) == 0? 1:2);
                }
                else if ((npc.life < npc.lifeMax / 2) && Main.rand.Next(2) == 0)
                {
                    Teleport(Main.rand.Next(2) == 0? 1:2);
                }
                if (npc.life < npc.lifeMax / 4)
                {
                    Teleport(Main.rand.Next(2) == 0? 1:2);
                }
            }
            npc.netUpdate = true;
        }

        public bool AliveCheck(Player player)
        {
            bool tooFar = Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 8000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 8000f;
            if (player.dead || tooFar || !player.active)
            {
                npc.TargetClosest(true);

                if (Main.player[npc.target].dead || !Main.player[npc.target].active || tooFar)
                {
                    if (!PlayerDead)
                    {
                        if (player.dead || !player.active)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroAwakened6"), Color.Red.R, Color.Red.G, Color.Red.B);
                        }
                        else if (tooFar)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ZeroAwakened7"), Color.Red.R, Color.Red.G, Color.Red.B);
                        }
                        PlayerDead = true;
                    }
                    npc.velocity.Y = npc.velocity.Y - 0.04f;
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    npc.ai[1] = 0;
                    npc.ai[2] = 0;
                    npc.ai[3] = 0;
                    if (npc.position.Y + npc.height - npc.velocity.Y <= 0 && Main.netMode != NetmodeID.MultiplayerClient) { BaseAI.KillNPC(npc); npc.netUpdate2 = true; }
                    return false;
                }
            }
            return true;
        }


        public void Teleport(int a = 0)
        {
            bool safe = false;
            Player player = Main.player[npc.target];
            Vector2 targetPos = player.Center; 
            TPDust();

            if (a == 0)
            {
                while (!safe)
                {
                    int posX = Main.rand.Next(-900, 900);
                    int posY = Main.rand.Next(-900, 900);

                    if ((posX < 400 && posX > -400) && (posY < 400 && posY > -400))
                    {
                        return;
                    }
                    npc.position = new Vector2(targetPos.X + posX, targetPos.Y + posY);
                    safe = true;
                }
            }
            else if (a == 1)
            {
                targetPos.X += 430 * (npc.Center.X > targetPos.X ? -1 : 1);
                targetPos.Y -= 430;
                npc.position = new Vector2(targetPos.X, targetPos.Y);
            }
            else if (a == 2)
            {
                targetPos.X += 430 * (npc.Center.X > targetPos.X ? -1 : 1);
                targetPos.Y += 430;
                npc.position = new Vector2(targetPos.X, targetPos.Y);
            }
            else
            {
                npc.Center = player.Center + new Vector2(0, -200);
            }

            npc.velocity *= 0;
            TPDust();
        }

        public void TPDust()
        {
            Vector2 position = npc.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, 226, 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num86].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 7; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num88].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num88].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Black * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * 8f;
            }
            for (int num89 = 0; num89 < 5; num89++)
            {
                int num90 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num90].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num90].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += npc.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 15; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num92].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num92].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += npc.DirectionTo(Main.dust[num92].position) * 3f;
            }
        }

        int Frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 3)
            {
                npc.frameCounter = 0;
                Frame += 1;
            }

            if (Frame > 6)
            {
                Frame = 0;
            }

            npc.frame.Y = frameHeight * Frame;
        }

        public void Attack(int Attack)
        {
            if (Attack == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == 1)
                    {
                        NPC.NewNPC((int)npc.Center.X + 30, (int)npc.Center.Y + 30, mod.NPCType("NullZP"));
                    }
                    else if (i == 2)
                    {
                        NPC.NewNPC((int)npc.Center.X + 30, (int)npc.Center.Y - 30, mod.NPCType("NullZP"));
                    }
                    else if (i == 3)
                    {
                        NPC.NewNPC((int)npc.Center.X - 30, (int)npc.Center.Y + 30, mod.NPCType("NullZP"));
                    }
                    else
                    {
                        NPC.NewNPC((int)npc.Center.X - 30, (int)npc.Center.Y - 30, mod.NPCType("NullZP"));
                    }
                }
            }
            else if (Attack == 1)
            {

                float spread = 12f * 0.0174f;
                double startAngle = Math.Atan2(npc.velocity.X, npc.velocity.Y) - spread / 2;
                double deltaAngle = 6;
                double offsetAngle;
                for (int i = 0; i < 6; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle) * 4f), (float)(Math.Cos(offsetAngle) * 2f), mod.ProjectileType("GlitchRocket"), 67, 0, Main.myPlayer, 0f, 0f);
                }
            }
            else if (Attack == 2)
            {
                float spread = 12f * 0.0174f;
                double startAngle = Math.Atan2(npc.velocity.X, npc.velocity.Y) - spread / 2;
                double deltaAngle = 5;
                double offsetAngle;
                for (int i = 0; i < 5; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle) * 4f), (float)(Math.Cos(offsetAngle) * 2f), mod.ProjectileType("Error"), 67, 0, Main.myPlayer, 0f, 0f);
                }
            }
            else if (Attack == 3)
            {
                float spread = 12f * 0.0174f;
                double startAngle = Math.Atan2(npc.velocity.X, npc.velocity.Y) - spread / 2;
                double deltaAngle = 4;
                double offsetAngle;
                for (int i = 0; i < 4; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle) * 2), (float)Math.Cos(offsetAngle), mod.ProjectileType("StaticSphere"), 67, 0, Main.myPlayer, 0f, 0f);
                }
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (Main.rand.Next(2) == 0)
			{
				target.AddBuff(mod.BuffType("Unstable"), 360, true);
			}
		}
    }
}
