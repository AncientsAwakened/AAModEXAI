
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAModEXAI.Bosses.Shen.Projectiles;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;
using AAModEXAI.Bosses;

namespace AAModEXAI.Bosses.Shen
{
    [AutoloadBossHead]
    public class Shen : ModNPC
    {
        public int damage = 0;

        public bool SpawnGrips = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shen Doragon; Discordian Doomsayer");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.noTileCollide = true;
            npc.height = 100;
            npc.width = 444;
            npc.aiStyle = -1;
            npc.netAlways = true;
            npc.knockBackResist = 0f;
            npc.damage = 250;
			npc.defense = 200;
            npc.lifeMax = 800000;
            npc.value = Item.sellPrice(20, 0, 0, 0);
            npc.knockBackResist = 0f;
            npc.boss = true;
            npc.aiStyle = -1;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/ShenRoar");
            musicPriority = (MusicPriority)11;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            npc.buffImmune[ModLoader.GetMod("AAMod").BuffType("Terrablaze")] = false;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * .8f);
        }

        public bool Weakness = false;
        public bool isAwakened = false;
        public float _normalSpeed = 15f;
        public float _chargeSpeed = 40f;
        public float MoveSpeed
        {
            get
            {
                float playerRunAcceleration = 1f;
                if (Main.player[npc.target].active && !Main.player[npc.target].dead) //if you have a target, speed up to keep up
                {
                    playerRunAcceleration = Math.Max(Math.Abs(Main.player[npc.target].moveSpeed), Main.player[npc.target].runAcceleration);
                    if (playerRunAcceleration <= 1f) playerRunAcceleration = 1f;
                }
                if (Dashing)
                {
                    return _chargeSpeed * playerRunAcceleration;
                }
                else
                {
                    return _normalSpeed * playerRunAcceleration;
                }
            }
        }

        //clientside stuff
        public Rectangle wingFrame = new Rectangle(0, 0, 444, 400); //the wing frame.
        public int wingFrameY = 400; //the frame height for the wings.
        public int frameY = 400; //the frame height for the body.
        public int roarTimer = 0; //if this is > 0, then use the roaring frame.
        public int roarTimerMax = 120; //default roar timer. only changed for fire breath as it's longer.
        public bool Roaring => roarTimer > 0; //wether or not he is roaring. only used clientside for frame visuals.

        public int chargeWidth = 50;
        public int normalWidth = 444;

        public override void BossLoot(ref string name, ref int potionType)
        {
            if (Main.expertMode && !isAwakened)
            {
                potionType = 0;
                return;
            }
            potionType = ModLoader.GetMod("AAMod").ItemType("GrandHealingPotion");
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public void Roar(int timer, bool fireSound)
        {
            roarTimer = timer;
            if (fireSound)
            {
                Main.PlaySound(4, (int)npc.Center.X, (int)npc.Center.Y, 60);
            }
            else
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/ShenRoar"), npc.Center);
            }
        }

        public int Side;
        public bool Health4 = false;
        public bool Health3 = false;
        public bool Health2 = false;
        public bool Health1 = false;

        public float[] FleeTimer = new float[1];

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
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            Vector2 targetPos;

            Main.dayTime = false;
            Main.time = 18000;

            if (!AliveCheck(player)) return;

            #region ProjIDs

            int AccelR = mod.ProjectileType("FireballAccelR");
            int AccelB = mod.ProjectileType("FireballAccelB");

            int FragR = mod.ProjectileType("FireballFragR");
            int FragB = mod.ProjectileType("FireballFragB");

            int HomingR = mod.ProjectileType("FireballHomingR");
            int HomingB = mod.ProjectileType("FireballHomingB");

            int SpreadR = mod.ProjectileType("FireballSpreadR");
            int SpreadB = mod.ProjectileType("FireballSpreadB");

            int Accel = npc.spriteDirection == 1 ? AccelR : AccelB;
            int Homing = npc.spriteDirection == 1 ? HomingR : HomingB;
            int Spread = npc.spriteDirection == 1 ? SpreadR : SpreadB;
            int Frag = npc.spriteDirection == 1 ? FragR : FragB;
            int Inferno = mod.ProjectileType("DiscordianInferno");

            #endregion

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

            if (NPC.AnyNPCs(mod.NPCType("BlazeGrip")) || NPC.AnyNPCs(mod.NPCType("AbyssGrip")))
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
                        int dust = spawnDust == 1 ? ModContent.DustType<AkumaADust>() : ModContent.DustType<YamataADust>();
                        if (Main.rand.Next(4) == 0) dust = ModContent.DustType<DiscordDust>();
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
                    }
                    if (++npc.ai[1] > 60)
                    {
                        npc.ai[1] = 0;
                        Roar(roarTimerMax, false);
                        npc.netUpdate = true;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            for (int i = -2; i <= 2; i++)
                                Projectile.NewProjectile(npc.Center, 30 * Vector2.UnitX.RotatedBy(Math.PI / 4 * i) * (npc.Center.X < player.Center.X ? -1 : 1), Spread, npc.damage / 4, 0f, Main.myPlayer, 20, 20 + 60);
                    }
                    break;

                case 1: //Fire Breath
                    BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, mod.ProjectileType("ShenABreath"), ref npc.ai[2], 5, npc.damage / 2, 18, false, new Vector2(167 * npc.direction, 0));
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

                case 5: //dashing
                    if (npc.ai[3] == 0 && --npc.ai[2] < 0)
                    {
                        npc.ai[2] = 4;
                        Roar(roarTimerMax, false);
                    }
                    if (++npc.ai[1] > 240 || (Math.Sign(npc.velocity.X) > 0 ? npc.Center.X > player.Center.X + 900 : npc.Center.X < player.Center.X - 900))
                    {
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
                    Movement(targetPos, .8f);
                    if (++npc.ai[2] > 80)
                    {
                        npc.ai[2] = 0;
                        Roar(roarTimerMax, false);
                        npc.netUpdate = true;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 spawnPos = npc.Center;
                            spawnPos.X += 250 * (npc.Center.X < player.Center.X ? 1 : -1);
                            spawnPos.Y -= 25;
                            Vector2 vel = (player.Center - spawnPos) / 30;
                            if (vel.Length() < 25)
                                vel = Vector2.Normalize(vel) * 25;
                            Projectile.NewProjectile(spawnPos, vel, Frag, npc.damage / 4, 0f, Main.myPlayer);
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
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            const float ai0 = 0.01f;
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(Math.PI / 2), Accel, npc.damage / 2, 0f, Main.myPlayer, ai0);
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(-Math.PI / 2), Accel, npc.damage / 2, 0f, Main.myPlayer, ai0);
                        }
                    }
                    
                    if (++npc.ai[1] > 40)
                    {
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        if (++npc.ai[3] >= 5) //dash three times
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
                        npc.velocity.X = -30 * (npc.Center.X < player.Center.X ? -1 : 1);
                        npc.velocity.Y = 5f;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, Homing, npc.damage / 2, 0f, Main.myPlayer, npc.target, 8f);
                    }
                    npc.rotation = 0;
                    break;

                case 12: //dashing
                    Dashing = true;
                    npc.velocity *= 0.98f;
                    if (++npc.ai[1] > 30)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 13: //hover nearby, shoot fireballs
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 700 * (npc.Center.X < targetPos.X ? -1 : 1);
                    Movement(targetPos, 0.7f);
                    if (++npc.ai[2] > 60)
                    {
                        Roar(roarTimerMax, false);
                        npc.ai[2] = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient) //spawn lightning
                        {
                            Vector2 infernoPos = new Vector2(200f, npc.direction == 1 ? 65f : -45f);
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
                                infernoPos.Y -= 70;
                            }
                            //REMEMBER: PROJECTILES DOUBLE DAMAGE so to get an accurate damage count you divide it by 2!
                            float InfernoType;
                            if (npc.spriteDirection == -1)
                            {
                                InfernoType = 1;
                            }
                            else
                            {
                                InfernoType = 2;
                            }

                            int projectile = Projectile.NewProjectile((int)infernoPos.X, (int)infernoPos.Y, vel.X, vel.Y, Inferno, damage, 0f, Main.myPlayer, InfernoType, 0f);
                            Main.projectile[projectile].velocity = vel;
                            Main.projectile[projectile].netUpdate = true;
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
                    if (++npc.ai[2] > 2)
                    {
                        npc.ai[2] = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            const float ai0 = 0.004f;
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(Math.PI / 2), Accel, npc.damage / 4, 0f, Main.myPlayer, ai0);
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(-Math.PI / 2), Accel, npc.damage / 4, 0f, Main.myPlayer, ai0);
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
                npc.spriteDirection = npc.Center.X < player.Center.X ? 1 : -1;
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

        bool Dashing = false;

        public void HandleFrames(Player player)
        {
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
            }
            npc.direction = npc.Center.X < player.Center.X ? 1 : -1;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage *= .8f;
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            Player player = Main.player[npc.target];
            if (npc.life <= npc.lifeMax / 2 && !SpawnGrips && !isAwakened)
            {
                SpawnGrips = true;
                if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon2"), Color.DarkMagenta);
                SpawnBossMethod.SpawnBoss(player, mod.NPCType("AbyssGrip"), false, 0, 0);
                SpawnBossMethod.SpawnBoss(player, mod.NPCType("BlazeGrip"), false, 0, 0);
                Main.PlaySound(SoundID.Roar, player.position, 0);
            }
            if (npc.life <= npc.lifeMax * .5f && !SpawnGrips && isAwakened)
            {
                SpawnGrips = true;

                if (AAWorld.downedShen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon3"), Color.DarkMagenta);
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon4"), new Color(102, 20, 48));
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon5"), new Color(72, 78, 117));
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon6"), Color.DarkMagenta);
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon7"), new Color(102, 20, 48));
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon8"), new Color(72, 78, 117));
                }

                SpawnBossMethod.SpawnBoss(player, mod.NPCType("FuryAshe"), false, 0, 0);
                SpawnBossMethod.SpawnBoss(player, mod.NPCType("WrathHaruka"), false, 0, 0);
            }

            if (npc.life <= npc.lifeMax * 0.80f && !Health4 && !isAwakened)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon19"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon20"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health4 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.66f && !Health3 && !isAwakened)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon21"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon22"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health3 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.30f && !Health1 && !isAwakened)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon23"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon24"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health1 = true;
                npc.netUpdate = true;
            }
        }

        public override void NPCLoot()
        {
            if (npc.type != mod.NPCType("ShenA"))
            {
                npc.DropLoot(AAMod.Items.Vanity.Mask.ShenMask.type, 1f / 7);
                if (!Main.expertMode)
                {
                    if (!AAWorld.downedShen)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon16"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                    }
                    else
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient) AAModEXAI.Chat(AAMod.Lang.BossChat("ShenDoragon18"), Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                    }
                    AAWorld.downedShen = true;
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType("ChaosScale"), 20, 30);
                    string[] lootTable = { "ChaosSlayer", "MeteorStrike", "Skyfall", "Astroid", "DraconicRipper", "FlamingTwilight", "ShenTerratool", "Timesplitter" };
                    int loot = Main.rand.Next(lootTable.Length);
                    npc.DropLoot(ModLoader.GetMod("AAMod").ItemType(lootTable[loot]));
                    BaseAI.DropItem(npc, ModLoader.GetMod("AAMod").ItemType("ShenTrophy"), 1, 1, 15, true);

                }
                else
                {
                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ShenTransition"));
                }
                BaseAI.DropItem(npc, ModLoader.GetMod("AAMod").ItemType("ShenTrophy"), 1, 1, 15, true);
                npc.value = 0f;
                npc.boss = false;
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color drawColor)
        {
            Texture2D currentTex = npc.spriteDirection == 1 ? mod.GetTexture("Bosses/Shen/ShenDoragonBlue") : Main.npcTexture[npc.type];
            Texture2D currentWingTex = npc.spriteDirection == 1 ? mod.GetTexture("Bosses/Shen/ShenDoragonBlueWings") : mod.GetTexture("Bosses/Shen/ShenDoragonWings");

            //offset
            npc.position.Y += 130f;

            //draw body/charge afterimage
            if (Dashing)
            {
                BaseDrawing.DrawAfterimage(sb, currentTex, 0, npc, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            }
            BaseDrawing.DrawTexture(sb, currentTex, 0, npc, npc.GetAlpha(drawColor), false);
            //draw wings
            BaseDrawing.DrawTexture(sb, currentWingTex, 0, npc.position + new Vector2(0, npc.gfxOffY), npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 5, wingFrame, npc.GetAlpha(drawColor), false);

            //deoffset
            npc.position.Y -= 130f;
            return false;
        }
        public static int ShootPeriodic(Entity codable, Vector2 position, int width, int height, int projType, ref float delayTimer, float delayTimerMax = 100f, int damage = -1, float speed = 10f, bool checkCanHit = true)
        {
            int pID = -1;
            if (damage == -1) { Projectile proj = new Projectile(); proj.SetDefaults(projType); damage = proj.damage; }
            bool properSide = codable is NPC ? Main.netMode != NetmodeID.MultiplayerClient : codable is Projectile ? ((Projectile)codable).owner == Main.myPlayer : true;
            if (properSide)
            {
                Vector2 targetCenter = position + new Vector2(width * 0.5f, height * 0.5f);
                delayTimer--;
                if (delayTimer <= 0)
                {
                    if (!checkCanHit || Collision.CanHit(codable.position, codable.width, codable.height, position, width, height))
                    {
                        Vector2 fireTarget = codable.Center + new Vector2(167 * codable.direction, 0);
                        float rot = BaseUtility.RotationTo(codable.Center, targetCenter);
                        fireTarget = BaseUtility.RotateVector(codable.Center, fireTarget, rot);
                        pID = BaseAI.FireProjectile(targetCenter, fireTarget, projType, damage, 0f, speed);
                    }
                    delayTimer = delayTimerMax;
                    if (codable is NPC) { ((NPC)codable).netUpdate = true; }
                }
            }
            return pID;
        }
    }
}
