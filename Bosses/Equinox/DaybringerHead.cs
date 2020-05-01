using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod.Dusts;
using System.IO;
using AAMod;

namespace AAModEXAI.Bosses.Equinox
{
    [AutoloadBossHead]	
	public class DaybringerHead : ModNPC
	{	
		public bool nightcrawler = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Daybringer");
            Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
            npc.lifeMax = 125000;
			npc.damage = 200;
			npc.defense = 100;
            npc.value = Item.sellPrice(0, 10, 0, 0);
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            npc.knockBackResist = 0f;
            npc.width = 68;
            npc.height = 68;
            npc.boss = true;
            npc.aiStyle = -1;
			npc.timeLeft = 500;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.DeathSound = null;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Equinox");
            musicPriority = MusicPriority.BossHigh;
            bossBag = ModLoader.GetMod("AAMod").ItemType("EquinoxBag");
		}

        public float[] internalAI = new float[8];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                if(npc.type == mod.NPCType("DaybringerHead") || npc.type == mod.NPCType("NightcrawlerHead"))
                {
                    writer.Write(internalAI[0]);
                    writer.Write(internalAI[1]);
                    writer.Write(internalAI[2]);
                    writer.Write(internalAI[3]);
                    writer.Write(internalAI[4]);
                    writer.Write(internalAI[5]);
                    writer.Write(internalAI[6]);
                    writer.Write(internalAI[7]);
                    
                    writer.Write(preShootingSun);
                    writer.Write(preDeathRay);
                    writer.Write(isDeathRay);
                    writer.Write(CloudCooldown);
                }

                writer.Write(initCustom);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if(npc.type == mod.NPCType("DaybringerHead") || npc.type == mod.NPCType("NightcrawlerHead"))
                {
                    internalAI[0] = reader.ReadFloat(); //DaybringerCounter
                    internalAI[1] = reader.ReadFloat(); //NightclawerCounter
                    internalAI[2] = reader.ReadFloat();
                    internalAI[3] = reader.ReadFloat();
                    internalAI[4] = reader.ReadFloat(); //DaybringerPosCheck
                    internalAI[5] = reader.ReadFloat(); //VelocitySave
                    internalAI[6] = reader.ReadFloat(); //VelocitySave
                    internalAI[7] = reader.ReadFloat();
                    
                    preShootingSun = reader.ReadBoolean();
                    preDeathRay = reader.ReadBoolean();
                    isDeathRay = reader.ReadBoolean();
                    CloudCooldown = reader.ReadInt();
                }

                initCustom = reader.ReadBoolean();
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override void BossHeadRotation(ref float rotation)
		{
			rotation = npc.rotation;
		}

		public override bool CheckActive()
		{
			npc.timeLeft--;
			return npc.timeLeft < 50;
		}

		public void HandleDayNightCycle()
		{
			bool daybringerExists = NPC.AnyNPCs(mod.NPCType("DaybringerHead"));
			bool nightcrawlerExists = NPC.AnyNPCs(mod.NPCType("NightcrawlerHead"));
			if (daybringerExists && nightcrawlerExists)
            {
                if((npc.type == mod.NPCType("DaybringerHead") && Main.dayTime && !preShootingSun) || (npc.type == mod.NPCType("NightcrawlerHead") && !Main.dayTime && !preDeathRay))
                {
                    if (Main.expertMode)
                    {
                        Main.fastForwardTime = true;
                        Main.dayRate = 20;
                    }else
                    {
                        Main.fastForwardTime = true;
                        Main.dayRate = 15;
                    }
                }
                else if((npc.type == mod.NPCType("DaybringerHead") && preShootingSun) || (npc.type == mod.NPCType("NightcrawlerHead") && (preDeathRay || isDeathRay)))
                {
                    Main.dayRate = 0;
                    Main.fastForwardTime = false;
                    Main.time --;
                }
            }else
            if ((daybringerExists && !nightcrawlerExists))
            {
                Main.fastForwardTime = true;
                Main.dayTime = true;
                Main.dayRate = 0;
                if(preShootingSun)
                {
                    Main.fastForwardTime = false;
                    Main.time --;
                }
            }else
            if ((!daybringerExists && nightcrawlerExists))
            {
                Main.fastForwardTime = true;
                Main.dayTime = false;
                Main.dayRate = 0;
                if(preDeathRay || isDeathRay)
                {
                    Main.fastForwardTime = false;
                    Main.time --;
                }
            }else
            {
                Main.dayRate = 1;
                Main.fastForwardTime = false;
            }
		}
        bool preDeathRay = false;
        bool isDeathRay = false;
        bool preShootingSun = false;
		bool prevWormStronger = false;
		bool initCustom = false;
        public int CloudCount = Main.expertMode ? 8 : 6;
        public int CloudCooldown = 400;

        public override bool PreAI()
        {
            bool isHead = npc.type == mod.NPCType("DaybringerHead") || npc.type == mod.NPCType("NightcrawlerHead");
            if (Main.netMode != 1 && !initCustom)
            {
                initCustom = true;
                internalAI[7] += npc.whoAmI % 7 * 12; //so it doesn't pew all at once
                npc.velocity.X += 0.1f;
                npc.velocity.Y -= 4f;
            }
            if (isHead)
            {
                HandleDayNightCycle();
            }

            bool isDay = Main.dayTime;
            bool wormStronger = (nightcrawler && !isDay) || (!nightcrawler && isDay);

            float wormDistance = -26f;
            int aiCount = 2;
            float moveSpeedMax = 16f;
            npc.damage = 200;
			npc.defense = 100;

            if (wormStronger != prevWormStronger)
            {
                int dustType = nightcrawler ? ModLoader.GetMod("AAMod").DustType("NightcrawlerDust") : ModLoader.GetMod("AAMod").DustType("DaybringerDust");
                for (int k = 0; k < 10; k++)
                {
                    int dustID = Dust.NewDust(npc.position, npc.width, npc.height, dustType, (int)(npc.velocity.X * 0.2f), (int)(npc.velocity.Y * 0.2f), 0, default, 1.5f);
                    Main.dust[dustID].noGravity = true;
                }
            }

            if (wormStronger)
            {
                if(Main.netMode == 0) 
                {
                    npc.width = 136;
                    npc.height = 136;
                    wormDistance = -52f;
                }
                aiCount = !nightcrawler ? 6 : 4;
                moveSpeedMax = !nightcrawler ? 15f : 12f;
                npc.damage = 300;
				npc.defense = ((!this.nightcrawler) ? 120 : 150);
            }

            for (int m = 0; m < aiCount; m++)
            {
                int Length = nightcrawler ? 24 : 30;
                int[] wormTypes = nightcrawler ? new int[] { mod.NPCType("NightcrawlerHead"), mod.NPCType("NightcrawlerBody"), mod.NPCType("NightcrawlerTail") } : new int[] { mod.NPCType("DaybringerHead"), mod.NPCType("DaybringerBody"), mod.NPCType("DaybringerTail") };
                AAAI.AIWorm(npc, wormTypes, Length, wormDistance, moveSpeedMax, 0.07f, true, false, false, false, false, false);
            }

            if (isHead) //prevents despawn and allows them to run away
            {
                bool foundTarget = TargetClosest();
                if (foundTarget)
                {
                    npc.timeLeft = 300;
                }
                else
                {
                    if (npc.timeLeft > 50) npc.timeLeft = 50;
                    npc.velocity.Y -= 0.2f;
                    if (npc.velocity.Y < -20f) npc.velocity.Y = -20f;
                    return false;
                }
            }
            else
            {
                npc.timeLeft = 300; //pieces should not despawn naturally, only despawn when the head does
            }
            
            Player target = Main.player[npc.target];
            
            if (npc.type == mod.NPCType("NightcrawlerHead"))
            {
                if(isDeathRay)
                {
                    goto ExtraAI;
                }
                if(preDeathRay)
                {
                    npc.defense = 9999;
                    if((npc.Center - target.Center).Length() < 300f)
                    {
                        isDeathRay = true;
                        npc.netUpdate = true;
                    }

                    if (npc.Center.X < target.Center.X)
                    {
                        npc.velocity.X += 0.5f;
                        if (npc.velocity.X < 0)
                            npc.velocity.X += 0.5f * 2;
                    }
                    else
                    {
                        npc.velocity.X -= 0.5f;
                        if (npc.velocity.X > 0)
                            npc.velocity.X -= 0.5f * 2;
                    }
                    if (npc.Center.Y < target.Center.Y)
                    {
                        npc.velocity.Y += 0.5f;
                        if (npc.velocity.Y < 0)
                            npc.velocity.Y += 0.5f * 2;
                    }
                    else
                    {
                        npc.velocity.Y -= 0.5f;
                        if (npc.velocity.Y > 0)
                            npc.velocity.Y -= 0.5f * 2;
                    }

                    if(npc.velocity.X > 30f) npc.velocity.X = 30f;
                    if(npc.velocity.Y > 30f) npc.velocity.Y = 30f;

                    internalAI[5] = npc.velocity.X;
                    internalAI[6] = npc.velocity.Y;
                }
            }
            if (npc.type == mod.NPCType("DaybringerHead"))
            {
                if(preShootingSun)
                {
                    npc.defense = 9999;
                    npc.TargetClosest(false);
                    goto ExtraAI;
                }
            }

            if(!isHead)
            {
                npc.defense = Main.npc[npc.realLife].defense;
            }
            goto Normal;

            ExtraAI:
            if(npc.type == mod.NPCType("NightcrawlerHead"))
            {
                npc.defense = 9999;
                npc.TargetClosest(false);
                npc.velocity = new Vector2(internalAI[5], internalAI[6]);
                
                if(internalAI[2] < 120)
                {
                    Vector2 newvelocity = npc.velocity + Vector2.Normalize(npc.velocity.RotatedBy((float)Math.PI/2)) * 0.58f;
                    npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
                    npc.velocity = Vector2.Normalize(newvelocity) * 16f;
                }
                else
                {
                    Vector2 newvelocity = npc.velocity + Vector2.Normalize(npc.velocity.RotatedBy((float)Math.PI/2)) * 0.03625f;
                    npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
                    npc.velocity = Vector2.Normalize(newvelocity) * 4f;
                }

                if (internalAI[2]++ == 90)
                {
                    for (int i = 0; i < Main.maxNPCs; i+=2)
                    {
                        if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("NightcrawlerBody") && Main.npc[i].realLife == npc.whoAmI)
                        {
                            if (Main.netMode != 1)
                            {
                                Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f)) * 8f;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("NightclawerDeathraySmall"), npc.damage / 2, 0, Main.myPlayer, 0, i);
                            }
                        }
                    }
                }
                if (internalAI[2] >= 90)
                {
                    for(int deathRay = 0; deathRay < Main.maxProjectiles; deathRay++)
                    {
                        if(Main.projectile[deathRay].active && Main.projectile[deathRay].type == mod.ProjectileType("NightclawerDeathraySmall") || Main.projectile[deathRay].type == mod.ProjectileType("NightclawerDeathray") && Main.projectile[deathRay].ai[1] == npc.whoAmI)
                        {
                            return false;
                        }
                    }
                }

                internalAI[5] = npc.velocity.X;
                internalAI[6] = npc.velocity.Y;

                if(internalAI[2] > 400)
                {
                    internalAI[2] = 0;
                    isDeathRay = false;
                    preDeathRay = false;
                    npc.netUpdate = true;
                }
            }
            if(npc.type == mod.NPCType("DaybringerHead"))
            {
                npc.defense = 9999;
                npc.TargetClosest(false);
                npc.velocity = new Vector2(internalAI[5], internalAI[6]);
                Vector2 targetpos = target.Center - new Vector2(0, 2000f);
                Vector2 targetpos2 = target.Center - new Vector2(1000f, 1000f);
                Vector2 targetpos3 = target.Center - new Vector2(-1000f, 1000f);

                if(internalAI[4] == 0)
                {
                    if(Math.Abs(npc.Center.X - targetpos.X) + Math.Abs(npc.Center.Y - targetpos.Y) < 100f)
                    {
                        internalAI[4] = 1f;
                    }
                }
                else if(internalAI[4] == 1)
                {
                    targetpos = targetpos2;
                    if(Math.Abs(npc.Center.X - targetpos.X) + Math.Abs(npc.Center.Y - targetpos.Y) < 100f)
                    {
                        internalAI[4] = 2f;
                        if(Main.netMode != 1)
                        {
                            for (int i = 0; i < Main.maxNPCs; i+= 3)
                            {
                                if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("DaybringerBody") && Main.npc[i].realLife == npc.whoAmI && AAGlobalProjectile.CountProjectiles(mod.ProjectileType("DaybringerSun")) < 3)
                                {
                                    Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f)) * 8f;
                                    Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, -speed.X, -speed.Y, mod.ProjectileType("DaybringerSun"), npc.damage / 3, 1, 255);
                                }
                            }
                        }
                    }
                }
                else if(internalAI[4] == 2)
                {
                    targetpos = targetpos3;
                    if(Math.Abs(npc.Center.X - targetpos.X) + Math.Abs(npc.Center.Y - targetpos.Y) < 100f)
                    {
                        internalAI[4] = 1f;
                        if(Main.netMode != 1)
                        {
                            for (int i = 0; i < Main.maxNPCs; i+= 3)
                            {
                                if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("DaybringerBody") && Main.npc[i].realLife == npc.whoAmI && AAGlobalProjectile.CountProjectiles(mod.ProjectileType("DaybringerSun")) < 3)
                                {
                                    Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f)) * 8f;
                                    Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, -speed.X, -speed.Y, mod.ProjectileType("DaybringerSun"), npc.damage / 3, 1, 255);
                                }
                            }
                        }
                    }
                }
                if (internalAI[3] % 200 == 60 && Main.netMode != 1)
                {
                    Vector2 speed = Vector2.Normalize(npc.velocity) * 8f;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, speed.X, speed.Y, mod.ProjectileType("DaybringerSun"), npc.damage / 2, 1, 255);
                }
                if (npc.Center.X < targetpos.X)
                {
                    npc.velocity.X += 0.5f;
                    if (npc.velocity.X < 0)
                        npc.velocity.X += 0.5f * 2;
                }
                else
                {
                    npc.velocity.X -= 0.5f;
                    if (npc.velocity.X > 0)
                        npc.velocity.X -= 0.5f * 2;
                }
                if (npc.Center.Y < targetpos.Y)
                {
                    npc.velocity.Y += 0.5f;
                    if (npc.velocity.Y < 0)
                        npc.velocity.Y += 0.5f * 2;
                }
                else
                {
                    npc.velocity.Y -= 0.5f;
                    if (npc.velocity.Y > 0)
                        npc.velocity.Y -= 0.5f * 2;
                }

                if(npc.velocity.X > 30f) npc.velocity.X = 30f;
                if(npc.velocity.Y > 30f) npc.velocity.Y = 30f;

                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

                internalAI[5] = npc.velocity.X;
                internalAI[6] = npc.velocity.Y;

                if(internalAI[3]++ > 700)
                {
                    internalAI[3] = 0;
                    internalAI[5] = 0;
                    internalAI[6] = 0;
                    preShootingSun = false;
                    npc.netUpdate = true;
                }
            }
            return false;

            Normal:

            npc.spriteDirection = 1;
            prevWormStronger = wormStronger;

            if (npc.type == mod.NPCType("NightcrawlerHead") && NPC.CountNPCS(mod.NPCType("NCCloud")) < CloudCount && CloudCooldown > 0 && Main.netMode != 1)
            {
                CloudCooldown--;

                if (CloudCooldown <= 0)
                {
                    CloudCooldown = 0;
                }
            }

            if(isDay && !preShootingSun)
            {
                if(isHead && npc.type == mod.NPCType("DaybringerHead"))
                {
                    internalAI[0] += 1f;
                    if(internalAI[0] % 360 == 0)
                    {
                        for(int playerid = 0; playerid < 255; playerid++)
                        {
                            if(Main.player[playerid].active && !Main.player[playerid].dead && Main.player[playerid] != null && Main.player[playerid].ownedProjectileCounts[mod.ProjectileType("DaybringerStars")] <= 0)
                            {
                                if (npc.life > npc.lifeMax / 2)
                                {
                                    if (Main.rand.Next(2) == 0)
                                    {
                                        Projectile.NewProjectile(Main.player[playerid].Center.X - 200f, Main.player[playerid].Center.Y + 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, -200f, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X, Main.player[playerid].Center.Y - 300f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 0, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X + 200f, Main.player[playerid].Center.Y + 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 200f, playerid);
                                    }
                                    else
                                    {
                                        Projectile.NewProjectile(Main.player[playerid].Center.X + 200f, Main.player[playerid].Center.Y - 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, -200f, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X, Main.player[playerid].Center.Y + 300f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 0, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X - 200f, Main.player[playerid].Center.Y - 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 200f, playerid);
                                    }
                                }
                                else
                                {
                                    if (Main.rand.Next(2) == 0)
                                    {
                                        Projectile.NewProjectile(Main.player[playerid].Center.X - 200f, Main.player[playerid].Center.Y + 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, -200f, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X + 200f, Main.player[playerid].Center.Y + 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 200f, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X - 200f, Main.player[playerid].Center.Y - 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, -200f, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X + 200f, Main.player[playerid].Center.Y - 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 200f, playerid);
                                    }
                                    else
                                    {
                                        Projectile.NewProjectile(Main.player[playerid].Center.X, Main.player[playerid].Center.Y + 300f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 0, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X, Main.player[playerid].Center.Y - 300f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 0, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X + 300f, Main.player[playerid].Center.Y, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 0, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X - 300f, Main.player[playerid].Center.Y, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 0, playerid);
                                    }
                                }
                            }
                        }
                    }
                    if(internalAI[0] % 120 == 30 && Main.netMode != 1)
                    {
                        for (int i = 0; i < Main.maxNPCs; i += 1)
                        {
                            if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("DaybringerBody") && Main.npc[i].realLife == npc.whoAmI)
                            {
                                Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f)) * 12f;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("DayBringerDarts"), npc.damage / 3, 0, Main.myPlayer);
                                speed = -speed;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("DayBringerDarts"), npc.damage / 3, 0, Main.myPlayer);
                            }
                        }
                    }
                    if(internalAI[0] % 120 == 60 && Main.netMode != 1)
                    {
                        for (int i = 0; i < Main.maxNPCs; i+=2)
                        {
                            if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("DaybringerBody") && Main.npc[i].realLife == npc.whoAmI && Main.rand.Next(15) == 0)
                            {
                                Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f)) * 8f;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("DaybringerOrb"), npc.damage / 3, 0, Main.myPlayer, 0, npc.whoAmI);
                                speed = -speed;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("DaybringerOrb"), npc.damage / 3, 0, Main.myPlayer, 0, npc.whoAmI);
                            }
                        }
                    }
                    

                    if(internalAI[0] > 1200)
                    {
                        if(Main.expertMode) preShootingSun = true;
                        internalAI[0] = 0f;
                        npc.netUpdate = true;
                    }
                }
            }
            if(!isDay && !preDeathRay)
            {
                if(isHead && npc.type == mod.NPCType("NightcrawlerHead"))
                {
                    internalAI[1] += 1f;
                    if (Main.netMode != 1 && CloudCooldown <= 0)
                    {
                        for(int i = 0; i < 200; i++)
                        {
                            if(Main.npc[i].type == mod.NPCType("NCCloud"))
                            {
                                Main.npc[i].life = 0;
                                Main.npc[i].NPCLoot();
                                Main.npc[i].active = false;
                            } 
                        }
                        CloudCooldown = 400;
                        float rotation = 2f * (float)Math.PI / CloudCount;
                        for (int m = 0; m < CloudCount; m++)
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("NCCloud"), 0, 0, 0, 0, rotation * m);
                            if (Main.netMode == 2 && n < 200)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }

                    if(internalAI[1] % 150 == 0 && Main.netMode != 1)
                    {
                        for(int playerid = 0; playerid < 255; playerid++)
                        {
                            if(Main.player[playerid].active && !Main.player[playerid].dead && Main.player[playerid] != null && Main.player[playerid].ownedProjectileCounts[mod.ProjectileType("DaybringerStars")] <= 0)
                            {
                                Projectile.NewProjectile(Main.player[playerid].Center.X + Main.rand.Next(-5, 5) * 40f, Main.player[playerid].Center.Y + Main.rand.Next(-5, 5) * 40f, 0, 0, mod.ProjectileType("NightclawerCloud"), npc.damage / 6, 0, 255);
                            }
                        }
                    }
                    
                    if(internalAI[1] % 380 == 90 && Main.netMode != 1)
                    {
                        for (int i = 0; i < Main.maxNPCs; i+= 1)
                        {
                            if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("NightcrawlerBody") && Main.npc[i].realLife == npc.whoAmI)
                            {
                                Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f)) * .5f;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("NightclawerScythe"), npc.damage / 3, 0, Main.myPlayer, npc.rotation, npc.spriteDirection);
                                speed = -speed;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("NightclawerScythe"), npc.damage / 3, 0, Main.myPlayer, npc.rotation, npc.spriteDirection);
                            }
                        }
                    }

                    
                    if(internalAI[1] % 120 == 90 && Main.netMode != 1)
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("NightcrawlerBody") && Main.npc[i].realLife == npc.whoAmI && Main.rand.Next(4) == 0)
                            {
                                Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f));
                                speed = (Main.rand.Next(2) == 0 ? 1: -1) * speed;
                                float ai = Main.rand.Next(120);
                                Vector2 speedR = Vector2.Normalize(speed.RotatedByRandom(0.6)) * 20f;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speedR.X, speedR.Y, mod.ProjectileType("NightclawerLaser"), npc.damage / 3, 0, Main.myPlayer, speed.ToRotation() + 1000f, ai);
                            }
                        }
                    }

                    if(internalAI[1] > 1200)
                    {
                        internalAI[1] = 0f;
                        if(Main.expertMode) preDeathRay = true;
                        npc.netUpdate = true;
                    }
                }
            }
            return false;
        }

		public int playerTooFarDist = 16000; //1000 tile radius, these worms move fast!		
		public bool TargetClosest()
		{
			int[] players = BaseAI.GetPlayers(npc.Center, Math.Min(20000f, playerTooFarDist * 3));
			float dist = 999999999f;
			int foundPlayer = -1;
			for (int m = 0; m < players.Length; m++)
			{
				Player p = Main.player[players[m]];
				if (Vector2.Distance(p.Center, npc.Center) < dist)
				{
					dist = Vector2.Distance(p.Center, npc.Center);
					foundPlayer = p.whoAmI;
				}
			}
			if (foundPlayer != -1)
			{
				BaseAI.SetTarget(npc, foundPlayer);
				return true;
			}
			return false;
		}

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.65f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.85f);
		}

		bool spawnedGore = false;
        public override void HitEffect(int hitDirection, double damage)
        {
			int dustType = nightcrawler ? ModLoader.GetMod("AAMod").DustType("NightcrawlerDust") : ModLoader.GetMod("AAMod").DustType("DaybringerDust");
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, dustType, hitDirection, -1f, 0, default, 1.2f);
            }
            if (npc.life <= 0 || (npc.life - damage <= 0))
            {			
				Main.dayRate = 1;
                Main.fastForwardTime = false;	
				if(!spawnedGore)
				{
					spawnedGore = true;
					bool isHead = npc.type == mod.NPCType("DaybringerHead") || npc.type == mod.NPCType("NightcrawlerHead");
					bool isBody = npc.type == mod.NPCType("DaybringerBody") || npc.type == mod.NPCType("NightcrawlerBody");						
					if(nightcrawler)
					{
						if(isHead)
						{
							Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/NCGore1"), 1f);	
							Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/NCGore2"), 1f);						
						}else
						if(isBody)
						{
							Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/NCGore3"), 1f);							
						}else
						{
							Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/NCGore4"), 1f);						
						}
					}else
					{
						if(isHead)
						{
							Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DBGore1"), 1f);	
							Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DBGore2"), 1f);						
						}else
						if(isBody)
						{
							Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DBGore3"), 1f);							
						}else
						{
							Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DBGore4"), 1f);						
						}					
					}
					for (int k = 0; k < 15; k++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, dustType, hitDirection, -1f, 0, default, 1.5f);
					}
				}
            }
        }

        public override void NPCLoot()
        {
            int otherWormAlive = nightcrawler ? mod.NPCType("DaybringerHead") : mod.NPCType("NightcrawlerHead");
            if (!nightcrawler)
            {
                AAWorld.downedDB = true;
                BaseAI.DropItem(npc, ModLoader.GetMod("AAMod").ItemType("DBTrophy"), 1, 1, 15, true);
            }
            else
            {
                AAWorld.downedNC = true;
                BaseAI.DropItem(npc, ModLoader.GetMod("AAMod").ItemType("NCTrophy"), 1, 1, 15, true);
            }
            if (NPC.CountNPCS(otherWormAlive) == 0)
            {
                AAWorld.downedEquinox = true;
            }
			string wormType = nightcrawler ? "Nightcrawler" : "Daybringer";
			if (Main.rand.Next(10) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType(wormType + "Trophy"));
			}
			if (Main.expertMode)
			{
                npc.DropBossBags();
			}
			else
			{
				if (Main.rand.Next(7) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType(wormType + "Mask"));
				}
                if (!nightcrawler)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("Stardust"), Main.rand.Next(30, 75));
                }
                else
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("DarkEnergy"), Main.rand.Next(30, 75));
                }
                if (AAWorld.RadiumOre)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("StarIdol"));
                }
			}
        }

		public Color GetAuraAlpha()
		{
			Color c = Color.White * (Main.mouseTextColor / 255f);
			//c.A = 255;
			return c;
		}

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            MakeSegmentsImmune(npc, player.whoAmI);
            ModifyCritArea(npc, ref crit);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MakeSegmentsImmune(npc, projectile.owner);
            ModifyCritArea(npc, ref crit);
            if (projectile.penetrate != 1)
            {
                for(int i = 0; i < Main.maxNPCs; i ++)
                {
                    if(Main.npc[i].active && (Main.npc[i].whoAmI == npc.realLife || (Main.npc[i].realLife >= 0 && Main.npc[i].realLife == npc.realLife)))
                    {
                        Main.npc[i].immune[projectile.owner] = 10;
                    }
                }
                damage = (int)(damage * .44f);
            }
            if (npc.type != mod.NPCType("DaybringerHead") && npc.type != mod.NPCType("NightcrawlerHead"))
            {
                damage = (int)(damage * .76f);
            }
        }

        private void ModifyCritArea(NPC npc, ref bool crit)
        {
            if (npc.realLife >= 0)
            {
                if (npc.whoAmI == npc.realLife)
                {
                    crit = true;
                }
                if (npc.ai[0] == 0)
                {
                    crit = false;
                }
            }
        }

        public override void UpdateLifeRegen(ref int damage)
        {
            if (npc.realLife >= 0 && npc.whoAmI != npc.realLife)
            {
                damage = 0;
                npc.lifeRegen = 0;
            }
        }

        public void MakeSegmentsImmune(NPC npc, int id)
        {
            if (npc.realLife >= 0)
            {
                bool last = false;
                NPC parent = Main.npc[npc.realLife];
                parent.lifeRegen = npc.lifeRegen;
                int i = 0;
                while (parent.ai[0] > 0 || last)
                {
                    if (i++ > 200) { return; }
                    parent.immune[id] = npc.immune[id];
                    for (int j = 0; j < npc.buffType.Length; j++)
                    {
                        if (npc.buffType[j] > 0 && npc.buffTime[j] > 0)
                        {
                            parent.buffType[j] = npc.buffType[j];
                            parent.buffTime[j] = npc.buffTime[j];
                        }
                    }
                    if (last) { break; }
                    parent = Main.npc[(int)parent.ai[0]];
                    if (parent.ai[0] == 0) { last = true; }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            bool wormStronger = (nightcrawler && !Main.dayTime) || (!nightcrawler && Main.dayTime);
            Texture2D tex = Main.npcTexture[npc.type];
            npc.width = 68;
            npc.height = 68;
            if (wormStronger)
            {
                string texName = "NPCs/Bosses/Equinox/";
                if (npc.type == mod.NPCType("DaybringerHead")) { texName += "DaybringerHeadBig"; }
                else
                if (npc.type == mod.NPCType("DaybringerBody")) { texName += "DaybringerBodyBig"; }
                else
                if (npc.type == mod.NPCType("DaybringerTail")) { texName += "DaybringerTailBig"; }
                else
                if (npc.type == mod.NPCType("NightcrawlerHead")) { texName += "NightcrawlerHeadBig"; }
                else
                if (npc.type == mod.NPCType("NightcrawlerBody")) { texName += "NightcrawlerBodyBig"; }
                else
                if (npc.type == mod.NPCType("NightcrawlerTail")) { texName += "NightcrawlerTailBig"; }
                tex = ModLoader.GetMod("AAMod").GetTexture(texName);

                int diff = Main.LocalPlayer.miscCounter % 50;
                float diffFloat = diff / 50f;
                float auraPercent = BaseUtility.MultiLerp(diffFloat, 0f, 1f, 0f); //did it this way so it's syncronized between all the segments
                BaseDrawing.DrawAura(spritebatch, tex, 0, npc, auraPercent, 2f, 0f, 0f, GetAuraAlpha());
            }
            BaseDrawing.DrawTexture(spritebatch, tex, 0, npc, Color.White); //GetAuraAlpha());				
            return false;
        }
    }
}