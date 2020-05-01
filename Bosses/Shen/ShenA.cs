
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using AAMod;

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
            npc.damage = 260;
			npc.defense = 240;
            npc.lifeMax = 1000000;
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

        public override void AI()
        {
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
            }
            else
            {
                if (npc.alpha > 0)
                {
                    for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                    {
                        int dust = ModLoader.GetMod("AAMod").DustType("DiscordLight");
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
                    if (Main.netMode != 1 && FleeTimer[0]++ >= 120)
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
                        if (npc.position.Y + npc.velocity.Y <= 0f && Main.netMode != 1) { BaseAI.KillNPC(npc); npc.netUpdate = true; }
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
                    targetPos = player.Center;
                    targetPos.X += 600 * (npc.Center.X < targetPos.X ? -1 : 1);
                    Movement(targetPos, 1f);
                    if (++npc.ai[2] > 240)
                    {
                        Roar(roarTimerMax, false);
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = npc.Center.X < player.Center.X ? 0 : (float)Math.PI;
                        npc.netUpdate = true;
                        npc.velocity.X = 2 * (npc.Center.X < player.Center.X ? -1 : 1);
                        npc.velocity.Y *= 0.2f;
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(npc.ai[3]), mod.ProjectileType("ShenDeathray"), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                    }
                    if (++npc.ai[1] > 60)
                    {
                        npc.ai[1] = 0;
                        Roar(roarTimerMax, false);
                        npc.netUpdate = true;
                        if (Main.netMode != 1)
                            for (int i = -2; i <= 2; i++)
                                Projectile.NewProjectile(npc.Center, 30 * Vector2.UnitX.RotatedBy(Math.PI / 4 * i) * (npc.Center.X < player.Center.X ? -1 : 1), mod.ProjectileType("ShenFireballSpread"), npc.damage / 4, 0f, Main.myPlayer, 20, 20 + 60);
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
                    targetPos.X += 800 * (npc.Center.X < targetPos.X ? -1 : 1);
                    targetPos.Y -= 800;
                    Movement(targetPos, 1.2f);
                    if (++npc.ai[1] > 180 || Math.Abs(npc.Center.Y - targetPos.Y) < 100) //initiate dash
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                        npc.velocity = npc.DirectionTo(player.Center) * 45;
                    }
                    npc.rotation = 0;
                    break;

                case 3: //dashing
                    if (npc.Center.Y > player.Center.Y + 700 || Math.Abs(npc.Center.X - player.Center.X) > 1500)
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
                        if (Main.netMode != 1)
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
                        if (Main.netMode != 1)
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

                case 7: goto case 2;
                case 8: goto case 3;

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
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(Math.PI / 2), mod.ProjectileType("ShenFireballAccel"), npc.damage / 4, 0f, Main.myPlayer, 0.01f, 0.01f);
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(-Math.PI / 2), mod.ProjectileType("ShenFireballAccel"), npc.damage / 4, 0f, Main.myPlayer, 0.01f, -0.01f);
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
                        if (Main.netMode != 1)
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
                    if (++npc.ai[2] > 40)
                    {
                        Roar(roarTimerMax, false);
                        npc.ai[2] = 0;
                        if (Main.netMode != 1) //spawn lightning
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
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = npc.Distance(player.Center);
                        npc.netUpdate = true;
                        npc.velocity = npc.DirectionTo(player.Center).RotatedBy(Math.PI / 2) * 40;
                    }
                    break;

                case 14: //fly in jumbo circle
                    npc.velocity -= npc.velocity.RotatedBy(Math.PI / 2) * npc.velocity.Length() / npc.ai[3];
                    if (++npc.ai[2] > 1)
                    {
                        npc.ai[2] = 0;
                        if (Main.netMode != 1)
                        {
                            const float ai0 = 0.004f;
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(Math.PI / 2), mod.ProjectileType("ShenFireballAccel"), npc.damage / 4, 0f, Main.myPlayer, ai0, -ai0);
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(-Math.PI / 2), mod.ProjectileType("ShenFireballAccel"), npc.damage / 4, 0f, Main.myPlayer, ai0, ai0);
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
                    npc.rotation = npc.velocity.ToRotation();
                    Dashing = true;
                    break;

                case 15: //wait for old attack to go away
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

                    npc.DropBossBags();
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
                if (npc.ai[0] != 1)
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

        public override void HitEffect(int hitDirection, double damage)
        {
            base.HitEffect(hitDirection, damage);
            if (npc.life <= npc.lifeMax * 0.9f && !Health9)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA1"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA2"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health9 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.8f && !Health8)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA3"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA4"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health8 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.7f && !Health7)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA5"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA6"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health7 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.6f && !Health6)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA7"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA8"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health6 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.5f && !Health5)
            {
                if (Main.netMode != 1) AAModEXAI.Chat(BossDialogue(), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                Health5 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.3f && !Health3)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA11"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA12"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health3 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.2f && !Health2)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA13"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA14"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health2 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.1f && !Health1)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA15"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenA16"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
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

        public override bool PreDraw(SpriteBatch sb, Color drawColor)
        {
            Texture2D currentTex = Main.npcTexture[npc.type];
            Texture2D currentWingTex1 = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Shen/ShenWingBack");
            Texture2D currentWingTex2 = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Shen/ShenWingFront");
            Texture2D glowTex = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Shen/ShenA_Glow");

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
