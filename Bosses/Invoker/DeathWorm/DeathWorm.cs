using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
using AAModEXAI.Dusts;
using AAModEXAI.Misc;
using AAModEXAI.Base;
using AAModEXAI.Localization;

namespace AAModEXAI.Bosses.Invoker.DeathWorm
{
    [AutoloadBossHead]
    public class DeathWorm : ModNPC
    {
        public bool loludided;
        private bool weakness;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invoked Tatzel");
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override void SetDefaults()
        {
            npc.noTileCollide = true;
            npc.height = 80;
            npc.width = 80;
            npc.aiStyle = -1;
            npc.netAlways = true;
            npc.knockBackResist = 0f;
            npc.damage = 140;
			npc.defense = 250;
			npc.lifeMax = 570000;
            if (Main.expertMode)
            {
                npc.value = Item.sellPrice(0, 0, 0, 0);
            }
            else
            {
                npc.value = Item.sellPrice(0, 30, 0, 0);
            }
            npc.knockBackResist = 0f;
            npc.boss = true;
            npc.aiStyle = -1;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = mod.GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/AkumaRoar");
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            npc.buffImmune[BuffID.Wet] = false;
            npc.alpha = 255;
            musicPriority = MusicPriority.BossHigh;
        }

        private bool fireAttack;
        private int attackFrame;
        private int attackCounter;
        private int attackTimer;
        public static int MinionCount = 0;
        public int MaxMinons = Main.expertMode ? 3 : 4;
        public int damage = 0;

        public float[] internalAI = new float[4];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
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
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override bool PreAI()
        {
            WormMovementMethod();
            return false;
        }

        public void WormMovementMethod()
        {
            int npcLeftPos = (int)(npc.position.X / 16f) - 1;
            int npcRightPos = (int)((npc.position.X + npc.width) / 16f) + 2;
            int npcBottomPos = (int)(npc.position.Y / 16f) - 1;
            int npcTopPos = (int)((npc.position.Y + npc.height) / 16f) + 2;

            if (npcLeftPos < 0)
            {
                npcLeftPos = 0;
            }

            if (npcRightPos > Main.maxTilesX)
            {
                npcRightPos = Main.maxTilesX;
            }

            if (npcBottomPos < 0)
            {
                npcBottomPos = 0;
            }

            if (npcTopPos > Main.maxTilesY)
            {
                npcTopPos = Main.maxTilesY;
            }

            npc.direction = npc.velocity.X < 0f ? 1 : -1;

            float num37 = 20f;
            float num38 = 0.55f;

            Vector2 NPCCenter = npc.Center;
            float playerCenterX = Main.player[npc.target].Center.X;
            float playerCenterY = Main.player[npc.target].Center.Y;

            playerCenterX = (int)(playerCenterX / 16f) * 16;
            playerCenterY = (int)(playerCenterY / 16f) * 16;
            NPCCenter.X = (int)(NPCCenter.X / 16f) * 16;
            NPCCenter.Y = (int)(NPCCenter.Y / 16f) * 16;
            playerCenterX -= NPCCenter.X;
            playerCenterY -= NPCCenter.Y;

            float num53 = (float)Math.Sqrt(playerCenterX * playerCenterX + playerCenterY * playerCenterY);
            if (npc.ai[1] > 0f && npc.ai[1] < Main.npc.Length)
            {
                try
                {
                    NPCCenter = npc.Center;
                    playerCenterX = Main.npc[(int)npc.ai[1]].Center.X - NPCCenter.X;
                    playerCenterY = Main.npc[(int)npc.ai[1]].Center.Y - NPCCenter.Y;
                }
                catch
                {
                }

                npc.rotation = (float)Math.Atan2(playerCenterY, playerCenterX) + 1.57f;
                int num54 = 42;
                num53 = (num53 - num54) / num53;
                playerCenterX *= num53;
                playerCenterY *= num53;
                npc.velocity = Vector2.Zero;
                npc.position.X += playerCenterX;
                npc.position.Y += playerCenterY;
            }
            else
            {
                float num56 = Math.Abs(playerCenterX);
                float num57 = Math.Abs(playerCenterY);
                float num58 = num37 / num53;
                playerCenterX *= num58;
                playerCenterY *= num58;
                bool flag6 = false;

                if (((npc.velocity.X > 0f && playerCenterX < 0f) || (npc.velocity.X < 0f && playerCenterX > 0f) || (npc.velocity.Y > 0f && playerCenterY < 0f) || (npc.velocity.Y < 0f && playerCenterY > 0f)) && Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) > num38 / 2f && num53 < 300f)
                {
                    flag6 = true;

                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num37)
                    {
                        npc.velocity *= 1.1f;
                    }
                }

                if (npc.position.Y > Main.player[npc.target].position.Y || Main.player[npc.target].dead)
                {
                    flag6 = true;

                    if (Math.Abs(npc.velocity.X) < num37 / 2f)
                    {
                        if (npc.velocity.X == 0f)
                        {
                            npc.velocity.X -= npc.direction;
                        }

                        npc.velocity.X *= 1.1f;
                    }
                    else if (npc.velocity.Y > -num37)
                    {
                        npc.velocity.Y -= num38;
                    }
                }

                if (!flag6)
                {
                    if ((npc.velocity.X > 0f && playerCenterX > 0f) || (npc.velocity.X < 0f && playerCenterX < 0f) || (npc.velocity.Y > 0f && playerCenterY > 0f) || (npc.velocity.Y < 0f && playerCenterY < 0f))
                    {
                        if (npc.velocity.X < playerCenterX)
                        {
                            npc.velocity.X += num38;
                        }
                        else if (npc.velocity.X > playerCenterX)
                        {
                            npc.velocity.X -= num38;
                        }

                        if (npc.velocity.Y < playerCenterY)
                        {
                            npc.velocity.Y += num38;
                        }
                        else if (npc.velocity.Y > playerCenterY)
                        {
                            npc.velocity.Y -= num38;
                        }

                        if (Math.Abs(playerCenterY) < num37 * 0.2 && ((npc.velocity.X > 0f && playerCenterX < 0f) || (npc.velocity.X < 0f && playerCenterX > 0f)))
                        {
                            npc.velocity.Y += npc.velocity.Y > 0f ? num38 * 2f : -num38 * 2f;
                        }

                        if (Math.Abs(playerCenterX) < num37 * 0.2 && ((npc.velocity.Y > 0f && playerCenterY < 0f) || (npc.velocity.Y < 0f && playerCenterY > 0f)))
                        {
                            npc.velocity.X += npc.velocity.X > 0f ? num38 * 2f : -num38 * 2f;
                        }
                    }
                    else if (num56 > num57)
                    {
                        if (npc.velocity.X < playerCenterX)
                        {
                            npc.velocity.X += num38 * 1.1f;
                        }
                        else if (npc.velocity.X > playerCenterX)
                        {
                            npc.velocity.X -= num38 * 1.1f;
                        }

                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num37 * 0.5)
                        {
                            npc.velocity.Y += npc.velocity.Y > 0f ? num38 : -num38;
                        }
                    }
                    else
                    {
                        if (npc.velocity.Y < playerCenterY)
                        {
                            npc.velocity.Y += num38 * 1.1f;
                        }
                        else if (npc.velocity.Y > playerCenterY)
                        {
                            npc.velocity.Y -= num38 * 1.1f;
                        }

                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num37 * 0.5)
                        {
                            npc.velocity.X += npc.velocity.X > 0f ? num38 : -num38;
                        }
                    }
                }

                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

                float num62 = Vector2.Distance(Main.player[npc.target].Center, npc.Center);
                int num63 = 0;
                if (Vector2.Normalize(Main.player[npc.target].Center - npc.Center).ToRotation().AngleTowards(npc.velocity.ToRotation(), (float)Math.PI / 2) == npc.velocity.ToRotation() && num62 < 350f)
                {
                    num63 = 15;
                }
            }
            return;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            Texture2D attackAni = mod.GetTexture("Bosses/Akuma/Akuma");
            if (fireAttack == false)
            {
                spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            if (fireAttack == true)
            {
                Vector2 drawCenter = new Vector2(npc.Center.X, npc.Center.Y);
                int num214 = attackAni.Height / 3;
                int y6 = num214 * attackFrame;
                Main.spriteBatch.Draw(attackAni, drawCenter - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, attackAni.Width, num214)), drawColor, npc.rotation, new Vector2(attackAni.Width / 2f, num214 / 2f), npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            return false;
        }

        public override void NPCLoot()
        {
            npc.value = 0f;
            npc.boss = false;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                npc.position.X = npc.position.X + npc.width / 2;
                npc.position.Y = npc.position.Y + npc.height / 2;
                npc.position.X = npc.position.X - npc.width / 2;
                npc.position.Y = npc.position.Y - npc.height / 2;
                int dust1 = ModContent.DustType<AkumaDust>();
                int dust2 = ModContent.DustType<AkumaDust>();
                Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, dust1, 0f, 0f, 0);
                Main.dust[dust1].velocity *= 0.5f;
                Main.dust[dust1].scale *= 1.3f;
                Main.dust[dust1].fadeIn = 1f;
                Main.dust[dust1].noGravity = false;
                Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, dust2, 0f, 0f, 0);
                Main.dust[dust2].velocity *= 0.5f;
                Main.dust[dust2].scale *= 1.3f;
                Main.dust[dust2].fadeIn = 1f;
                Main.dust[dust2].noGravity = true;
            }
        }


        public int roarTimer = 0; //if this is > 0, then use the roaring frame.
        public int roarTimerMax = 120; //default roar timer. only changed for fire breath as it's longer.
        public bool Roaring //wether or not he is roaring. only used clientside for frame visuals.
        {
            get
            {
                return roarTimer > 0;
            }
        }

        public void Roar(int timer, bool fireSound)
        {
            roarTimer = timer;
            if (fireSound)
            {
                Main.PlaySound(SoundID.NPCKilled, (int)npc.Center.X, (int)npc.Center.Y, 60);
            }
            else
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/AkumaRoar"), npc.Center);
            }
        }


        public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
        {
            spriteEffects = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = npc.rotation;
        }
    }
/*
    public class DeathWormBody : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invoked Tatzel");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 600;
            cooldownSlot = 1;
        }

        public override bool AI()
        {
            NPC npc = Main.npc[(int)projectile.ai[1]];
            Vector2 chasePosition = Main.npc[(int)npc.ai[1]].Center;
            Vector2 directionVector = chasePosition - npc.Center;
            npc.spriteDirection = (directionVector.X > 0f) ? 1 : -1;
            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
                npc.TargetClosest(true);
            if (Main.player[npc.target].dead && npc.timeLeft > 300)
                npc.timeLeft = 300;
            if (projectile.alpha != 0)
            {
                for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                {
                    int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModContent.DustType<AkumaDust>(), 0f, 0f, 100, default, 2f);
                    Main.dust[num935].noGravity = true;
                    Main.dust[num935].noLight = true;
                }
            }
            npc.alpha -= 12;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }


            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[3]].type != ModContent.NPCType<DeathWorm>())
                {
                    projectile.Kill();
                }
            }

            if (npc.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float dirX = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - npcCenter.X;
                float dirY = Main.npc[(int)npc.ai[1]].position.Y + Main.npc[(int)npc.ai[1]].height / 2 - npcCenter.Y;
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - npc.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;

                if (dirX < 0f)
                {
                    npc.spriteDirection = 1;

                }
                else
                {
                    npc.spriteDirection = -1;
                }

                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;
            }

            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(true);
            }
            npc.netUpdate = true;
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            float k = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], 22f * projectile.scale, ref k))
            {
                return true;
            }
            return false;
        }
    }
*/
}

