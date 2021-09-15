using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

using AAModEXAI.Misc;
using AAModEXAI.Bosses.Sag;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Zero.ZeroSag
{
    [AutoloadBossHead]
    public class ZeroSag : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zero-Sagittarius");
            Main.npcFrameCount[npc.type] = 9;
		}

		public override void SetDefaults()
        {
            npc.lifeMax = 30000;
            npc.boss = true;
            npc.defense = 90;
            npc.damage = 70;
            npc.width = 124;
            npc.height = 206;
            npc.aiStyle = -1;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 80000f;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.alpha = 255;
        }

        public float[] internalAI = new float[3];
        Vector2 targetPos;
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(targetPos.X);
                writer.Write(targetPos.Y);
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
                targetPos.X = reader.ReadFloat();
                targetPos.Y = reader.ReadFloat();
            }
        }

        bool lowHealth = false;

        public override void AI()
        {
            npc.TargetClosest(true);

            Player player = Main.player[npc.target];

            #region Direction & Alpha

            if (npc.ai[0] != 2)
            {
                if (player.Center.X > npc.Center.X)
                {
                    npc.direction = 1;
                }
                else
                {
                    npc.direction = -1;
                }
            }
            else
            {
                if (npc.velocity.X > 0)
                {
                    npc.direction = 1;
                }
                else
                {
                    npc.direction = -1;
                }
            }

            for (int m = npc.oldPos.Length - 1; m > 0; m--)
            {
                npc.oldPos[m] = npc.oldPos[m - 1];
            }
            npc.oldPos[0] = npc.position;

            #endregion

            switch ((int)npc.ai[0])
            {
                case 0:
                    if (!DeathCheck())
                        return;

                    int pos;
                    if (player.Center.X > npc.Center.X) //If NPC's X position is less than the player's
                    {
                        pos = 300;
                    }
                    else //If NPC's X position is higher than the player's
                    {
                        pos = -300;
                    }

                    Vector2 wantedVelocity = player.Center - new Vector2(pos, 250);

                    MoveToPoint(wantedVelocity, 9f);

                    Shooting();

                    if (npc.ai[1]++ > (Main.expertMode ? 480 : 600))
                    {
                        npc.ai[0] ++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        internalAI[2] = 0;
                    }
                    break;


                case 1:
                    if (!DeathCheck())
                        return;
                    if (++npc.ai[1] > 10)
                    {
                        targetPos = player.Center;
                        targetPos.X += 1000 * (npc.Center.X < targetPos.X ? -1 : 1);
                        Movement(targetPos, 1f);
                        if (npc.ai[1] > 120 || Math.Abs(npc.Center.Y - targetPos.Y) < 16) //initiate dash
                        {
                            npc.ai[0] ++;
                            npc.rotation += npc.velocity.X * 0.05f;
                            npc.ai[1] = 0;
                            npc.netUpdate = true;
                            int speed = npc.life < npc.lifeMax / 3 ? 18 : 20;
                            npc.velocity.X = -speed * (npc.Center.X < player.Center.X ? -1 : 1);
                            npc.velocity.Y *= 0f;
                        }
                    }
                    else
                    {
                        npc.velocity *= 0.9f; //decelerate briefly
                    }
                    npc.rotation = 0;
                    break;

                case 2:
                    if (++npc.ai[1] > 240 || (Math.Sign(npc.velocity.X) > 0 ? npc.Center.X > player.Center.X + 900 : npc.Center.X < player.Center.X - 900))
                    {
                        if(internalAI[2] < 4)
                        {
                            npc.ai[0] --;
                            internalAI[2] ++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            internalAI[0] = 0;
                        }
                        else
                        {
                            npc.ai[0] = 0;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = Main.rand.Next(5);
                            internalAI[0] = 0;
                            internalAI[2] = 0;
                        }
                        npc.netUpdate = true;
                    }
                    break;
                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }

            if (npc.alpha > 0)
            {
                npc.alpha -= 10;
            }
            if (npc.alpha <= 0)
            {
                npc.alpha = 0;
            }

            if (npc.life < npc.lifeMax / 3 && internalAI[1]++ % 90 == 0)
            {
                Vector2 SparkPos = npc.Center + new Vector2(Main.rand.Next(-48, 48), 0);
                Vector2 SparkSpeed = new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(0, 4));
                Projectile.NewProjectile(SparkPos, SparkSpeed, ModContent.ProjectileType<SagSpark>(), npc.damage / 4, 1);

                for (int num242 = 0; num242 < 5; num242++)
                {
                    int num243 = Dust.NewDust(SparkPos, 0, 0, 226, SparkSpeed.X, SparkSpeed.Y, 0, default, 1f);
                    Main.dust[num243].scale = 0.5f;
                    Main.dust[num243].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                }
            }
            if (npc.ai[0] != 2)
            {
                npc.rotation = 0;
            }
        }

        public void Shooting()
        {
            Player player = Main.player[npc.target];

            switch ((int)npc.ai[3])
            {
                case 0:
                    BaseAI.ShootPeriodic(npc, player.Center, player.width, player.height, ModContent.ProjectileType<SagShot>(), ref npc.ai[2], 30, npc.damage / 4, 9, false, new Vector2(-36 * npc.direction, -51));
                    break;
                case 1:
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        internalAI[0]++;
                        if (internalAI[0] > 180)
                        {
                            internalAI[0] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    if (internalAI[0] > 40)
                    {
                        BaseAI.ShootPeriodic(npc, player.Center + new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)), player.width, player.height, ModContent.ProjectileType<SagiStar>(), ref npc.ai[2], 30, npc.damage / 4, 9, false, new Vector2(36 * npc.direction, -51));
                    }
                    break;
                case 2:
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        internalAI[0]++;
                        if (internalAI[0] > 210)
                        {
                            internalAI[0] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    if (internalAI[0] > 40 && internalAI[0] % 30 == 0)
                    {
                        Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.Next(3, 7) * npc.direction, -6f), ModContent.ProjectileType<SagBomb>(), npc.damage / 4, 3);
                    }
                    break;
                case 3:
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        internalAI[0]++;
                        if (internalAI[0] > 240)
                        {
                            internalAI[0] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    if (internalAI[0] > 40)
                    {
                        BaseAI.ShootPeriodic(npc, player.Center + new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)), player.width, player.height, ModContent.ProjectileType<ZeroBeam1>(), ref npc.ai[2], 40, npc.damage / 4, 9, false);
                    }
                    break;
                case 4:
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        internalAI[0]++;
                    }
                    if (internalAI[0] > 80)
                    {
                        BaseAI.ShootPeriodic(npc, player.Center + new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)), player.width, player.height, ModContent.ProjectileType<SagBlast>(), ref npc.ai[2], 50, npc.damage / 4, 9, false, new Vector2(36 * npc.direction, -51));
                    }
                    break;
                default:
                    npc.ai[3] = 0;
                    goto case 0;
            }

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

        public override void FindFrame(int frameHeight)
        {
            int frameSpeed;
            if (npc.ai[0] != 1)
            {
                frameSpeed = npc.ai[0] == 2 ? 3 : 12 - (int)npc.velocity.X;
                if (npc.velocity.X != 0)
                {
                    if (npc.frameCounter++ > frameSpeed)
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y += frameHeight;
                        if (npc.frame.Y > frameHeight * 3)
                        {
                            npc.frame.Y = 0;
                        }
                    }
                }
            }
            else
            {
                frameSpeed = 7;
                if (npc.frame.Y < frameHeight * 4)
                {
                    npc.frame.Y = frameHeight * 4;
                }
                if (npc.frameCounter++ > frameSpeed)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y += frameHeight;
                    if (npc.frame.Y > frameHeight * 8)
                    {
                        npc.frame.Y = frameHeight * 6;
                    }
                }
            }
        }

        public bool DeathCheck()
        {
            bool ZoneVoid = (bool) ModSupport.GetModPlayerConditions("AAMod", Main.player[npc.target], "AAPlayer", "ZoneVoid", false, false);
            if (Main.player[npc.target].dead || Vector2.Distance(Main.player[npc.target].Center, npc.Center) > 5000 || !ZoneVoid)
            {
                npc.TargetClosest(true);
                if (Main.player[npc.target].dead || Vector2.Distance(Main.player[npc.target].Center, npc.Center) > 5000 || !ZoneVoid)
                {
                    npc.velocity *= .7f;
                    npc.alpha += 5;
                    if (npc.alpha >= 255)
                    {
                        npc.active = false;
                    }
                    if (!Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) <= 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) >= 6000f)
                    {
                        npc.TargetClosest(true);
                    }
                    return false;
                }
            }
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SagBodyGore"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SagHeadGore"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SagLegGore"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SagLegGore"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SagNeckGore"), 1f); 
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
            else
            {
                for (int num242 = 0; num242 < 3; num242++)
                {
                    int num243 = Dust.NewDust(npc.position, npc.width, npc.height, 226, -2.5f * hitDirection, -2.5f, 0, default, 1f);
                    Main.dust[num243].scale = 0.5f;
                    Main.dust[num243].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                }
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            if (npc.ai[0] == 2)
            {
                BaseDrawing.DrawAfterimage(sb, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.oldPos, npc.scale, npc.rotation, npc.direction, 9, npc.frame, 1f, 1f, 7, true, 0, 0, Color.White);
            }
            BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 9, npc.frame, npc.GetAlpha(dColor), false); ;
            BaseDrawing.DrawTexture(sb, mod.GetTexture("Bosses/Sag/SagGlow/Sag_Glow"), 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 9, npc.frame, npc.GetAlpha(ColorUtils.COLOR_GLOWPULSE), false);
            return false;
        }

        public void MoveToPoint(Vector2 point, float moveSpeed)
        {
            float velMultiplier = 1f;
            Vector2 dist = point - npc.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < moveSpeed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
            }
            if (length < 200f)
            {
                moveSpeed *= (length / 200f);
            }
            npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            npc.velocity *= moveSpeed;
            npc.velocity *= velMultiplier;
        }

    }
}
