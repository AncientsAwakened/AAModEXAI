
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Shen
{
    [AutoloadBossHead]
    public class ShenA : Shen
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shen Doragon Awakened");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.damage = 300;
			npc.defense = 240;
            npc.lifeMax = 1500000;
            npc.value = Item.sellPrice(1, 0, 0, 0);
            bossBag = ModLoader.GetMod("AAMod").ItemType("ShenCache");
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/ShenA");
            musicPriority = (MusicPriority)11;
            isAwakened = true;
            npc.alpha = 255;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.defense = (int)(npc.defense * 1.2f);
            npc.damage = (int)(npc.damage * .8f);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(FleeTimer[0]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                FleeTimer[0] = reader.ReadFloat();
            }
        }

        public NPC Head1;
        public NPC Head2;
        public NPC Head3;
        public NPC Head4;
        public NPC Head5;
        public NPC Head6;

        public bool halfLifeAIChange = false;
        public bool ShootAkumaProj = false;

        public override void AI()
        {
            halfLifeAIChange = npc.life <= npc.lifeMax * .5f;
            Main.dayTime = false;
            Main.time = 18000;

            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            Vector2 targetPos;

            if (!AliveCheck(player)) return;

            Dashing = false;
            if (Roaring) roarTimer--;

            if (Dashing)
            {
                if (npc.width != chargeWidth)
                {
                    Vector2 center = npc.Center;
                    npc.width = chargeWidth;
                    npc.Center = center;
                    npc.netUpdate = true;
                }
            }
            else
            if (npc.width != normalWidth)
            {
                Vector2 center = npc.Center;
                npc.width = normalWidth;
                npc.Center = center;
                npc.netUpdate = true;
            }

            if (NPC.AnyNPCs(mod.NPCType("FuryAshe")) || NPC.AnyNPCs(mod.NPCType("WrathHaruka")))
            {
                if (npc.alpha > 50)
                {
                    npc.alpha = 50;
                }
                else
                {
                    npc.alpha += 4;
                }
                npc.dontTakeDamage = true;
                halfLifeAIChange = false;
            }
            else
            {
                if (npc.alpha > 0)
                {
                    for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                    {
                        int dust = ModContent.DustType<DiscordLight>();
                        int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, dust, 0f, 0f, 100, default, 2f);
                        Main.dust[num935].noGravity = true;
                        Main.dust[num935].noLight = true;
                    }
                    npc.alpha -= 4;
                }
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
                npc.dontTakeDamage = false;
            }

            if (player.dead || !player.active || Vector2.Distance(npc.Center, player.Center) > 10000)
            {
                npc.TargetClosest();

                if (player.dead || !player.active || Vector2.Distance(npc.Center, player.Center) > 10000)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient && FleeTimer[0]++ >= 120)
                    {
                        if (FleeTimer[0] < 130)
                        {
                            npc.velocity.Y += 1f;
                            npc.netUpdate = true;
                        }
                        else if (FleeTimer[0] == 130)
                        {
                            npc.velocity.Y = -6f;
                            npc.netUpdate = true;
                        }
                        else if (FleeTimer[0] > 130)
                        {
                            npc.velocity.Y = -6f;
                        }
                        if (npc.position.Y + npc.velocity.Y <= 0f && Main.netMode != NetmodeID.MultiplayerClient) { BaseAI.KillNPC(npc); npc.netUpdate = true; }
                    }
                }
                else
                {
                    FleeTimer[0] = 0;
                }
            }

            switch ((int)npc.ai[0])
            {
                case 0: //target for first time, navigate beside player
                    if (!npc.HasPlayerTarget)
                        npc.TargetClosest();
                    if (!AliveCheck(Main.player[npc.target]))
                        break;
                    if(halfLifeAIChange)
                    {
                        
                        targetPos = player.Center;
                        targetPos.X += 600 * (npc.Center.X < targetPos.X ? -1 : 1);

                        Movement(targetPos, 1f);

                        SoulCheck();

                        if (++npc.ai[2] > 480)
                        {
                            Roar(roarTimerMax, false);
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = npc.Center.X < player.Center.X ? 0 : (float)Math.PI;
                            npc.netUpdate = true;
                            npc.velocity.X = 2 * (npc.Center.X < player.Center.X ? -1 : 1);
                            npc.velocity.Y *= 0.2f;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(npc.ai[3]), mod.ProjectileType("ShenDeathray"), npc.damage * 2, 0f, Main.myPlayer, 0, npc.whoAmI);
                        }
                        if (++npc.ai[1] > 60)
                        {
                            npc.ai[1] = 0;
                            Roar(roarTimerMax, false);
                            npc.netUpdate = true;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                for (int i = -2; i <= 2; i++)
                                    Projectile.NewProjectile(npc.Center, 30 * Vector2.UnitX.RotatedBy(Math.PI / 4 * i) * (npc.Center.X < player.Center.X ? -1 : 1), mod.ProjectileType("ShenFireballSpread"), npc.damage / 4, 0f, Main.myPlayer, 20, 20 + 60);
                        }
                    }
                    else
                    {
                        targetPos = player.Center;
                        targetPos.X += 600 * (npc.Center.X < targetPos.X ? -1 : 1);
                        Movement(targetPos, 1f);
                        if (++npc.ai[2] > 480)
                        {
                            Roar(roarTimerMax, false);
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = npc.Center.X < player.Center.X ? 0 : (float)Math.PI;
                            npc.netUpdate = true;
                            npc.velocity.X = 2 * (npc.Center.X < player.Center.X ? -1 : 1);
                            npc.velocity.Y *= 0.2f;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(npc.ai[3]), mod.ProjectileType("ShenDeathray"), npc.damage * 2, 0f, Main.myPlayer, 0, npc.whoAmI);
                        }
                        if (++npc.ai[1] > 60 && npc.ai[2] < 240)
                        {
                            npc.ai[1] = 0;
                            Roar(roarTimerMax, false);
                            npc.netUpdate = true;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                for (int i = -2; i <= 2; i++)
                                    Projectile.NewProjectile(npc.Center, 30 * Vector2.UnitX.RotatedBy(Math.PI / 4 * i) * (npc.Center.X < player.Center.X ? -1 : 1), mod.ProjectileType("ShenFireballSpread"), npc.damage / 4, 0f, Main.myPlayer, 20, 20 + 60);
                        }
                    }
                    break;

                case 1: //firing mega ray
                    if (++npc.ai[1] > 120)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 2: //fly to corner for dash
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 400 * (npc.Center.X < targetPos.X ? -1 : 1);
                    targetPos.Y -= 400;
                    Movement(targetPos, 1.3f);
                    if (halfLifeAIChange)
                    {
                        targetPos.X += 800 * (npc.Center.X < targetPos.X ? -1 : 1);
                        targetPos.Y -= 800;
                        Tele(targetPos);
                        npc.ai[1] = 70;
                        if (++npc.ai[1] > 70) //initiate dash
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.velocity = PredictPlayerMovement(45f, player);
                            npc.velocity.Normalize();
                            npc.velocity *= 45f;
                            npc.netUpdate = true;
                        }
                        npc.rotation = 0;
                        break;
                    }
                    if (++npc.ai[1] > 70 && Math.Abs(npc.Center.Y - targetPos.Y) < 100) //initiate dash
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.velocity = PredictPlayerMovement(45f, player);
                        npc.velocity.Normalize();
                        npc.velocity *= 45f;
                        npc.netUpdate = true;
                    }
                    npc.rotation = 0;
                    break;

                case 3: //dashing
                    if (npc.Center.Y > player.Center.Y + 800 || Math.Abs(npc.Center.X - player.Center.X) > 1500)
                    {
                        npc.velocity.Y *= 0.5f;
                        npc.ai[1] = 0;
                        if (++npc.ai[2] >= 3) //repeat three times
                        {
                            npc.ai[0]++;
                            npc.ai[2] = 0;
                        }
                        else
                            npc.ai[0]--;
                        npc.netUpdate = true;
                    }
                    Dashing = true;
                    npc.rotation = npc.velocity.ToRotation();
                    if (npc.velocity.X < 0)
                        npc.rotation += (float)Math.PI;
                    break;

                case 4: //prepare for queen bee dashes
                    if (!AliveCheck(player))
                        break;
                    if (++npc.ai[1] > 30)
                    {
                        targetPos = player.Center;
                        targetPos.X += 1000 * (npc.Center.X < targetPos.X ? -1 : 1);
                        Movement(targetPos, 0.8f);
                        if (npc.ai[1] > 180 || Math.Abs(npc.Center.Y - targetPos.Y) < 50) //initiate dash
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.netUpdate = true;
                            npc.velocity.X = -40 * (npc.Center.X < player.Center.X ? -1 : 1);
                            npc.velocity.Y *= 0.1f;
                        }
                    }
                    else
                    {
                        npc.velocity *= 0.9f; //decelerate briefly
                    }
                    npc.rotation = 0;
                    break;

                case 5: //dashing, leave trail of vertical deathrays
                    if (npc.ai[3] == 0 && --npc.ai[2] < 0) //spawn rays on first dash only
                    {
                        npc.ai[2] = 4;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.UnitY, mod.ProjectileType("ShenDeathrayVertical"), npc.damage / 4, 0f, Main.myPlayer, 0f, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, -Vector2.UnitY, mod.ProjectileType("ShenDeathrayVertical"), npc.damage / 4, 0f, Main.myPlayer, 0f, npc.whoAmI);
                        }
                    }
                    if (++npc.ai[1] > 240 || (Math.Sign(npc.velocity.X) > 0 ? npc.Center.X > player.Center.X + 900 : npc.Center.X < player.Center.X - 900))
                    {
                        Roar(roarTimerMax, false);
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        if (++npc.ai[3] >= 3) //repeat dash three times
                        {
                            npc.ai[0]++;
                            npc.ai[3] = 0;
                        }
                        else
                            npc.ai[0]--;
                        npc.netUpdate = true;
                    }
                    Dashing = true;
                    break;

                case 6: //fly at player, spit mega balls
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 700 * (npc.Center.X < targetPos.X ? -1 : 1);
                    Movement(targetPos, 0.5f);
                    if (++npc.ai[2] > 60)
                    {
                        npc.ai[2] = 0;
                        Roar(roarTimerMax, false);
                        npc.netUpdate = true;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 spawnPos = npc.Center;
                            spawnPos.X += 250 * (npc.Center.X < player.Center.X ? 1 : -1);
                            Vector2 vel = (player.Center - spawnPos) / 30;
                            if (vel.Length() < 25)
                                vel = Vector2.Normalize(vel) * 25;
                            Projectile.NewProjectile(spawnPos, vel, mod.ProjectileType("ShenFireballFrag"), npc.damage / 4, 0f, Main.myPlayer);
                        }
                    }
                    if (++npc.ai[1] > 210)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 7:
                    if(halfLifeAIChange)
                    {
                        if (!AliveCheck(player))
                        break;
                        if (++npc.ai[1] > 30)
                        {
                            targetPos = player.Center;
                            targetPos.X += 1000 * (npc.Center.X < targetPos.X ? -1 : 1);
                            Movement(targetPos, 0.8f);
                            if (npc.ai[1] > 180 || Math.Abs(npc.Center.Y - targetPos.Y) < 50) //initiate dash
                            {
                                npc.ai[0]++;
                                npc.ai[1] = 0;
                                npc.netUpdate = true;
                                npc.velocity.X = -40 * (npc.Center.X < player.Center.X ? -1 : 1);
                                npc.velocity.Y *= 0.1f;
                            }
                        }
                        else
                        {
                            npc.velocity *= 0.9f; //decelerate briefly
                        }
                        npc.rotation = 0;
                        break;
                    }
                    else goto case 2;
                case 8: 
                    if(halfLifeAIChange)
                    {
                        if (npc.ai[1] == 0)
                        {
                            ShootAkumaProj = false;
                        }
                        if (!ShootAkumaProj && (player.position.X > npc.position.X + 30f && npc.position.X + npc.width > player.position.X + player.width + 30f))
                        {
                            if(player.Center.Y < npc.Center.Y)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    for (int i = 0; i * 20 < npc.width; i++)
                                    {
                                        Vector2 vel = -5f * Vector2.UnitY;
                                        vel.X += Main.rand.NextFloat(-1f, 1f);
                                        vel.Y += Main.rand.NextFloat(-.5f, .5f);
                                        vel *= 8f;
                                        Vector2 ShootPos = new Vector2(npc.position.X + i * 20, npc.Center.Y);
                                        Projectile.NewProjectile(ShootPos, vel, mod.ProjectileType("DiscordianInferno"), npc.damage / 4, 0f, Main.myPlayer);
                                    }
                                }
                            }
                            else
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    for (int i = 0; i * 10 < npc.width; i++)
                                    {
                                        Vector2 vel = 4f * Vector2.UnitY;
                                        vel.X += Main.rand.NextFloat(-1f, 1f);
                                        vel.Y += Main.rand.NextFloat(-1f, 1f);
                                        Vector2 ShootPos = new Vector2(npc.position.X + i * 20, npc.Center.Y);
                                        Projectile.NewProjectile(ShootPos, vel, mod.ProjectileType("AkumaRock"), npc.damage / 4, 0f, Main.myPlayer);
                                    }
                                }
                            }
                            ShootAkumaProj = true;
                        }
                        if (++npc.ai[1] > 240 || (Math.Sign(npc.velocity.X) > 0 ? npc.Center.X > player.Center.X + 900 : npc.Center.X < player.Center.X - 900))
                        {
                            Roar(roarTimerMax, false);
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            if (++npc.ai[3] >= 4)
                            {
                                npc.ai[0]++;
                                npc.ai[3] = 0;
                            }
                            else
                                npc.ai[0]--;
                            npc.netUpdate = true;
                        }
                        Dashing = true;
                    }
                    else goto case 3;
                    break;
                case 9: //prepare for fishron dash
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center + player.DirectionTo(npc.Center) * 600;
                    Movement(targetPos, 0.8f);
                    if (++npc.ai[1] > 20)
                    {
                        Roar(roarTimerMax, false);
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                        npc.velocity = npc.DirectionTo(player.Center) * 40;
                    }
                    npc.rotation = 0;
                    break;

                case 10: //dashing
                    if (++npc.ai[2] > 3)
                    {
                        npc.ai[2] = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(Math.PI / 2), mod.ProjectileType("ShenFireballAccel"), npc.damage / 4, 0f, Main.myPlayer, 0.01f);
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(-Math.PI / 2), mod.ProjectileType("ShenFireballAccel"), npc.damage / 4, 0f, Main.myPlayer, 0.01f);
                        }
                    }
                    if (++npc.ai[1] > 40)
                    {
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        if (++npc.ai[3] >= 5) //dash five times
                        {
                            npc.ai[0]++;
                            npc.ai[3] = 0;
                        }
                        else
                            npc.ai[0]--;
                        npc.netUpdate = true;
                    }
                    Dashing = true;
                    npc.rotation = npc.velocity.ToRotation();
                    if (npc.velocity.X < 0)
                        npc.rotation += (float)Math.PI;
                    break;

                case 11: //fly up, prepare to spit mega homing and dash
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 600 * (npc.Center.X < targetPos.X ? -1 : 1);
                    targetPos.Y -= 600;
                    Movement(targetPos, 0.8f);
                    if (++npc.ai[1] > 180 || npc.Distance(targetPos) < 50)
                    {
                        Roar(roarTimerMax, false);
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                        npc.velocity.X = -40 * (npc.Center.X < player.Center.X ? -1 : 1);
                        npc.velocity.Y = 5f;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("ShenFireballHoming"), npc.damage / 3, 0f, Main.myPlayer, npc.target, 8f);
                    }
                    npc.rotation = 0;
                    break;

                case 12: //dashing
                    Dashing = true;
                    npc.velocity *= 0.99f;
                    if (++npc.ai[1] > 30)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 13: //hover nearby, shoot lightning
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 700 * (npc.Center.X < targetPos.X ? -1 : 1);
                    Movement(targetPos, 0.7f);
                    if(halfLifeAIChange)
                    {
                        SoulCheck();
                    }
                    if (++npc.ai[2] > 40)
                    {
                        Roar(roarTimerMax, false);
                        npc.ai[2] = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient) //spawn lightning
                        {
                            Vector2 infernoPos = new Vector2(200f, npc.direction == -1 ? 65f : -45f);
                            Vector2 vel = new Vector2(MathHelper.Lerp(6f, 8f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-4f, 4f, (float)Main.rand.NextDouble()));
                            if (player.active && !player.dead)
                            {
                                float rot = BaseUtility.RotationTo(npc.Center, player.Center);
                                infernoPos = BaseUtility.RotateVector(Vector2.Zero, infernoPos, rot);
                                vel = BaseUtility.RotateVector(Vector2.Zero, vel, rot);
                                vel *= MoveSpeed / _normalSpeed; //to compensate for players running away
                                int dir = npc.Center.X < player.Center.X ? 1 : -1;
                                if ((dir == -1 && npc.velocity.X < 0) || (dir == 1 && npc.velocity.X > 0)) vel.X += npc.velocity.X;
                                vel.Y += npc.velocity.Y;
                                infernoPos += npc.Center;
                            }
                            Projectile.NewProjectile((int)infernoPos.X, (int)infernoPos.Y + 16, vel.X * 2, vel.Y * 2, mod.ProjectileType("ChaosLightning"), npc.damage / 4, 0f, Main.myPlayer, vel.ToRotation(), 0f);
                        }
                    }
                    if (++npc.ai[1] > 360)
                    {
                        if(halfLifeAIChange)
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = npc.Distance(player.Center);
                            npc.netUpdate = true;
                            npc.velocity.X = 2 * (npc.Center.X < player.Center.X ? -1 : 1);
                            npc.velocity.Y *= 0.2f;
                        }
                        else
                        {
                            npc.ai[0] += 2;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = npc.Distance(player.Center);
                            npc.netUpdate = true;
                            npc.velocity = npc.DirectionTo(player.Center).RotatedBy(Math.PI / 2) * 40;
                        }
                    }
                    break;

                case 14: 
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 700 * (npc.Center.X < targetPos.X ? -1 : 1);
                    Movement(targetPos, 0.7f);
                    if (++npc.ai[1] > 540)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = npc.Distance(player.Center);
                        npc.netUpdate = true;
                        npc.velocity = npc.DirectionTo(player.Center).RotatedBy(Math.PI / 2) * 40;
                    }
                    break;

                case 15: //fly in jumbo circle
                    npc.velocity -= npc.velocity.RotatedBy(Math.PI / 2) * npc.velocity.Length() / npc.ai[3];
                    if (++npc.ai[2] > 1)
                    {
                        npc.ai[2] = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            const float ai0 = 0.004f;
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(Math.PI / 2), mod.ProjectileType("ShenFireballAccel"), npc.damage / 4, 0f, Main.myPlayer, ai0);
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(-Math.PI / 2), mod.ProjectileType("ShenFireballAccel"), npc.damage / 4, 0f, Main.myPlayer, ai0);
                        }
                    }
                    if (npc.ai[1] <= 1)
                    {
                        Roar(roarTimerMax, false);
                    }
                    if (++npc.ai[1] > 150)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                    }
                    npc.rotation = (float)Math.Atan2((double)(npc.velocity.Y * (float)npc.direction), (double)(npc.velocity.X * (float)npc.direction));
                    Dashing = true;
                    break;

                case 16: //wait for old attack to go away
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 600 * (npc.Center.X < targetPos.X ? -1 : 1);
                    Movement(targetPos, 1f);
                    if (++npc.ai[2] > 120)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                    npc.rotation = 0;
                    break;
                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }
        }

        private bool AliveCheck(Player player)
        {
            if ((!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 5000f))
            {
                npc.TargetClosest();
                if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 5000f)
                {
                    if (npc.timeLeft > 60)
                        npc.timeLeft = 60;
                    BaseAI.KillNPC(npc);
                    npc.netUpdate2 = true;
                    return false;
                }
            }
            if (npc.timeLeft < 600)
                npc.timeLeft = 600;
            return true;
        }

        private void Movement(Vector2 targetPos, float speedModifier)
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

        public override void NPCLoot()
        {
            if (Head1 != null || Head1.active || Head1.life > 0)
            {
                Head1.active = false;
                Head1.life = 0;
            }
            if (Head2 != null || Head2.active || Head2.life > 0)
            {
                Head2.active = false;
                Head2.life = 0;
            }
            if (Head3 != null || Head3.active || Head3.life > 0)
            {
                Head3.active = false;
                Head3.life = 0;
            }
            if (Head4 != null || Head4.active || Head4.life > 0)
            {
                Head4.active = false;
                Head4.life = 0;
            }
            if (Head5 != null || Head5.active || Head5.life > 0)
            {
                Head5.active = false;
                Head5.life = 0;
            }
            if (Head6 != null || Head6.active || Head6.life > 0)
            {
                Head6.active = false;
                Head6.life = 0;
            }
            if (npc.type == mod.NPCType("ShenA"))
            {
                if (Main.expertMode)
                {
                    npc.DropLoot(AAMod.Items.Vanity.Mask.ShenAMask.type, 1f / 7);
                    if (!AAWorld.downedShen)
                    {
                        npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("ChaosRune"));
                    }

                    BaseAI.DropItem(npc, ModLoader.GetMod("AAMod").ItemType("ShenATrophy"), 1, 1, 15, true);

                    if (!NPC.AnyNPCs(mod.NPCType("ShenDefeat")))
                    {
                        NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ShenDefeat"));
                    }

                    for(int i = 0; i < 10; i++) npc.DropBossBags();
                }
            }
                
        }

            bool Dashing = false;

        public override void FindFrame(int frameHeight)
        {
            Player player = Main.player[npc.target];
            npc.frame = new Rectangle(0, Roaring ? frameY : 0, 444, frameY);
            if (Dashing)
            {
                npc.frameCounter = 0;
                wingFrame.Y = wingFrameY;
            }
            else
            {
                npc.frameCounter++;
                if (npc.frameCounter >= 5)
                {
                    npc.frameCounter = 0;
                    wingFrame.Y += wingFrameY;
                    if (wingFrame.Y > (wingFrameY * 4))
                    {
                        npc.frameCounter = 0;
                        wingFrame.Y = 0;
                    }
                }
                if (npc.ai[0] != 1 && npc.ai[0] != 15)
                {
                    npc.spriteDirection = npc.Center.X < player.Center.X ? 1 : -1;
                }
            }
        }

        public bool Health9 = false;
        public bool Health8 = false;
        public bool Health7 = false;
        public bool Health6 = false;
        public bool Health5 = false;
        public bool HealthOneHalf = false;

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

        public override void HitEffect(int hitDirection, double damage)
        {
            base.HitEffect(hitDirection, damage);
            if (npc.life <= npc.lifeMax * 0.9f && !Health9)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA1"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA2"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health9 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.8f && !Health8)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA3"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA4"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health8 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.7f && !Health7)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA5"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA6"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health7 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.6f && !Health6)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA7"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA8"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health6 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.5f && !Health5)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(BossDialogue(), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                Health5 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.3f && !Health3)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA11"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA12"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health3 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.2f && !Health2)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA13"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA14"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health2 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.1f && !Health1)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA15"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA16"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health1 = true;
                npc.netUpdate = true;
            }
            if (Health2)
            {
               // music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/LastStand");
            }
        }

        public bool DownedRag => (bool)ModSupport.GetModWorldConditions("ThoriumMod", "ThoriumWorld", "downedRealityBreaker", false, true);
        public bool DownedScal => (bool)ModSupport.GetModWorldConditions("CalamityMod", "CalamityWorld", "downedSCal", false, true);
        public bool DownedMantid => (bool)ModSupport.GetModWorldConditions("GRealm", "MWorld", "downedMatriarch", false, true);
        public bool DownedNeb => (bool)ModSupport.GetModWorldConditions("Redemption", "RedeWorld", "downedNebuleus", false, true);
        public bool DownedOverseer => (bool)ModSupport.GetModWorldConditions("SpiritMod", "MyWorld", "downedOverseer", false, true);
        //public bool DownedDuo => JetshiftModLoader.GetMod("AAMod").JetshiftWorld.downedCosmicMystery;

        public string BossDialogue()
        {
            WeightedRandom<string> Text = new WeightedRandom<string>();

            bool a = false;

            if (ModSupport.GetMod("ThoriumMod") != null && DownedRag)
            {
                a = true;
                Text.Add(AAMod.Lang.BossChat("ShenAThorium"));
            }

            if (ModSupport.GetMod("CalamityMod") != null && DownedScal)
            {
                a = true;
                Text.Add(AAMod.Lang.BossChat("ShenACalamity"));
            }

            if (ModSupport.GetMod("GRealm") != null && DownedMantid)
            {
                a = true;
                Text.Add(AAMod.Lang.BossChat("ShenAGRealm"));
            }

            if (ModSupport.GetMod("Redemption") != null && DownedNeb)
            {
                a = true;
                Text.Add(AAMod.Lang.BossChat("ShenARedemption"));
            }

            if (ModSupport.GetMod("SpiritMod") != null && DownedOverseer)
            {
                a = true;
                Text.Add(AAMod.Lang.BossChat("ShenASpirit"));
            }

            /*if (AAMod.jsLoaded && DownedDuo)
            {
                a = true;
                Text.Add("But slaying those two meteor-squatting crystal things? That's quite an eye-catcher.");
            }*/

            if (!a)
            {
                Text.Add(AAMod.Lang.BossChat("ShenANoMod"));
            }
            return Text;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if(npc.life <= npc.lifeMax * .5f)
            {
                damage *= .8f;
                if(npc.ai[0] == 0 || npc.ai[0] == 15 || npc.ai[0] == 16 || npc.ai[0] == 17)
                {
                    damage = 1.0;
                }
            }
            return false;
        }

        public void Tele(Vector2 tele)
        {
            npc.Center = tele;

            int pieCut = 20;
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(npc.Center.X - 1, npc.Center.Y - 1), 2, 2, ModContent.DustType<DiscordLight>(), 0f, 0f, 100, Color.White, 1f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(6f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(npc.Center.X - 1, npc.Center.Y - 1), 2, 2, ModContent.DustType<DiscordLight>(), 0f, 0f, 100, Color.White, 1.5f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(9f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
        }

        public void SoulCheck()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Head1 == null || !Head1.active || Head1.life == 0)
                {
                    Head1 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ShenSoul"), 0)];
                    ((ShenSoul)Head1.modNPC).internalAI[0] = npc.whoAmI;
                    ((ShenSoul)Head1.modNPC).internalAI[1] = 100f;
                    ((ShenSoul)Head1.modNPC).internalAI[2] = - 600f;
                    ((ShenSoul)Head1.modNPC).internalAI[3] = 3;
                    Head1.netUpdate = true;
                }
                if (Head2 == null || !Head2.active || Head2.life == 0)
                {
                    Head2 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ShenSoul"), 0)];
                    ((ShenSoul)Head2.modNPC).internalAI[0] = npc.whoAmI;
                    ((ShenSoul)Head2.modNPC).internalAI[1] = 50f;
                    ((ShenSoul)Head2.modNPC).internalAI[2] = - 400f;
                    ((ShenSoul)Head2.modNPC).internalAI[3] = 2;
                    Head2.netUpdate = true;
                }
                if (Head3 == null || !Head3.active || Head3.life == 0)
                {
                    Head3 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ShenSoul"), 0)];
                    ((ShenSoul)Head3.modNPC).internalAI[0] = npc.whoAmI;
                    ((ShenSoul)Head3.modNPC).internalAI[1] = 0;
                    ((ShenSoul)Head3.modNPC).internalAI[2] = - 200f;
                    ((ShenSoul)Head3.modNPC).internalAI[3] = 1;
                    Head3.netUpdate = true;
                }
                if (Head4 == null || !Head4.active || Head4.life == 0)
                {
                    Head4 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ShenSoul"), 0)];
                    ((ShenSoul)Head4.modNPC).internalAI[0] = npc.whoAmI;
                    ((ShenSoul)Head4.modNPC).internalAI[1] = 0;
                    ((ShenSoul)Head4.modNPC).internalAI[2] = 200f;
                    ((ShenSoul)Head4.modNPC).internalAI[3] = 1;
                    Head4.netUpdate = true;
                }
                if (Head5 == null || !Head5.active || Head5.life == 0)
                {
                    Head5 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ShenSoul"), 0)];
                    ((ShenSoul)Head5.modNPC).internalAI[0] = npc.whoAmI;
                    ((ShenSoul)Head5.modNPC).internalAI[1] = 50f;
                    ((ShenSoul)Head5.modNPC).internalAI[2] = 400f;
                    ((ShenSoul)Head5.modNPC).internalAI[3] = 2;
                    Head5.netUpdate = true;
                }
                if (Head6 == null || !Head6.active || Head6.life == 0)
                {
                    Head6 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ShenSoul"), 0)];
                    ((ShenSoul)Head6.modNPC).internalAI[0] = npc.whoAmI;
                    ((ShenSoul)Head6.modNPC).internalAI[1] = 100f;
                    ((ShenSoul)Head6.modNPC).internalAI[2] = 600f;
                    ((ShenSoul)Head6.modNPC).internalAI[3] = 3;
                    Head6.netUpdate = true;
                }
                if (Head1 != null && Head2 != null && Head3 != null && Head4 != null && Head5 != null && Head6 != null)
                {
                    ((ShenSoul)Head1.modNPC).internalAI[3] = 3;
                    ((ShenSoul)Head2.modNPC).internalAI[3] = 2;
                    ((ShenSoul)Head3.modNPC).internalAI[3] = 1;
                    ((ShenSoul)Head4.modNPC).internalAI[3] = 1;
                    ((ShenSoul)Head5.modNPC).internalAI[3] = 2;
                    ((ShenSoul)Head6.modNPC).internalAI[3] = 3;
                }
            }
            else
            {
                int[] npcs = BaseAI.GetNPCs(npc.Center, -1, default, 200f, null);
                if (npcs != null && npcs.Length > 0)
                {
                    foreach (int npcID in npcs)
                    {
                        NPC npc2 = Main.npc[npcID];
                        if (npc2 != null)
                        {
                            if (Head1 == null && npc2.type == mod.NPCType("ShenSoul") && ((ShenSoul)npc2.modNPC).internalAI[0] == npc.whoAmI)
                            {
                                Head1 = npc2;
                            }
                            else
                            if (Head2 == null && npc2.type == mod.NPCType("ShenSoul") && ((ShenSoul)npc2.modNPC).internalAI[0] == npc.whoAmI)
                            {
                                Head2 = npc2;
                            }
                            else
                            if (Head3 == null && npc2.type == mod.NPCType("ShenSoul") && ((ShenSoul)npc2.modNPC).internalAI[0] == npc.whoAmI)
                            {
                                Head3 = npc2;
                            }
                            else
                            if (Head4 == null && npc2.type == mod.NPCType("ShenSoul") && ((ShenSoul)npc2.modNPC).internalAI[0] == npc.whoAmI)
                            {
                                Head4 = npc2;
                            }
                            else
                            if (Head5 == null && npc2.type == mod.NPCType("ShenSoul") && ((ShenSoul)npc2.modNPC).internalAI[0] == npc.whoAmI)
                            {
                                Head5 = npc2;
                            }
                            else
                            if (Head6 == null && npc2.type == mod.NPCType("ShenSoul") && ((ShenSoul)npc2.modNPC).internalAI[0] == npc.whoAmI)
                            {
                                Head6 = npc2;
                            }
                        }
                    }
                    if (Head1 != null && Head2 != null && Head3 != null && Head4 != null && Head5 != null && Head6 != null)
                    {
                        ((ShenSoul)Head1.modNPC).internalAI[3] = 3;
                        ((ShenSoul)Head2.modNPC).internalAI[3] = 2;
                        ((ShenSoul)Head3.modNPC).internalAI[3] = 1;
                        ((ShenSoul)Head4.modNPC).internalAI[3] = 1;
                        ((ShenSoul)Head5.modNPC).internalAI[3] = 2;
                        ((ShenSoul)Head6.modNPC).internalAI[3] = 3;
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color drawColor)
        {
            Texture2D currentTex = Main.npcTexture[npc.type];
            Texture2D currentWingTex1 = mod.GetTexture("Bosses/Shen/ShenWingBack");
            Texture2D currentWingTex2 = mod.GetTexture("Bosses/Shen/ShenWingFront");
            Texture2D glowTex = mod.GetTexture("Bosses/Shen/ShenA_Glow");

            //offset
            npc.position.Y += 130f;

            //draw body/charge afterimage
            BaseDrawing.DrawTexture(sb, currentWingTex1, 0, npc.position + new Vector2(0, npc.gfxOffY), npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 5, wingFrame, drawColor);
            if (Dashing)
            {
                BaseDrawing.DrawAfterimage(sb, currentTex, 0, npc, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            }
            BaseDrawing.DrawTexture(sb, currentTex, 0, npc, drawColor);

            //draw glow/glow afterimage
            BaseDrawing.DrawTexture(sb, glowTex, 0, npc, AAColor.Shen3);
            BaseDrawing.DrawAfterimage(sb, glowTex, 0, npc, 0.3f, 1f, 8, false, 0f, 0f, AAColor.Shen3);

            //draw wings
            BaseDrawing.DrawTexture(sb, currentWingTex2, 0, npc.position + new Vector2(0, npc.gfxOffY), npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 5, wingFrame, drawColor);

            //deoffset
            npc.position.Y -= 130f; // offsetVec;			

            return false;
        }
    }

}
