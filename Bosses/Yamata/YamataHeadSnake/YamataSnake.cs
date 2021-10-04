
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Yamata.YamataHeadSnake
{
    public class YamataSnake : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yamata's Head");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.noTileCollide = true;
            npc.width = 46;
            npc.height = 62;
            npc.aiStyle = -1;
            npc.netAlways = true;
            npc.damage = 140;
            npc.defense = 100;
            npc.lifeMax = 25000;
            npc.DeathSound = mod.GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/YamataRoar");
            npc.noGravity = true;
            npc.knockBackResist = 0f;
            npc.value = 0f;
            npc.scale = 1f;
            npc.alpha = 255;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public int timer = 0;

        public override void AI()
        {
            if (npc.localAI[3] == 0f)
            {
                Main.PlaySound(SoundID.Item119, npc.position);
                npc.localAI[3] = 1f;
            }

            npc.dontTakeDamage = npc.alpha > 0;
            if (npc.dontTakeDamage)
            {
                for (int j = 0; j < 2; j++)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 228, 0f, 0f, 100, default, 2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].noLight = true;
                }
            }

            npc.alpha -= 42;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }

            bool flag = true;
            float speedY = 0.2f;

            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }

            if (!Main.player[npc.target].active || Main.player[npc.target].dead)
            {
                npc.TargetClosest(false);
                if (!Main.player[npc.target].active || Main.player[npc.target].dead)
                {
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.ai[0] == 0f)
                {
                    npc.ai[3] = npc.whoAmI;
                    npc.realLife = npc.whoAmI;
                    int npcWhoAmI = npc.whoAmI;

                    for (int l = 0; l < 5; l++)
                    {
                        int type = mod.NPCType("YamataSnakeBody");
                        if (l == 4)
                        {
                            type = mod.NPCType("YamataSnakeTail");
                        }

                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int newNPC = NPC.NewNPC((int)npc.Center.X, (int)(npc.position.Y + npc.height), type, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                            if (Main.netMode == NetmodeID.Server && newNPC < 200) NetMessage.SendData(23, -1, -1, null, newNPC);
                            Main.npc[newNPC].ai[3] = npc.whoAmI;
                            Main.npc[newNPC].realLife = npc.whoAmI;
                            Main.npc[newNPC].ai[1] = npcWhoAmI;

                            Main.npc[npcWhoAmI].ai[0] = newNPC;
                            npcWhoAmI = newNPC;
                        }
                        npc.netUpdate = true;
                    }
                }
            }

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

            float num37 = 18f;
            float num38 = 0.28f;

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

                if (num63 > npc.frameCounter)
                {
                    npc.frameCounter += 1.0;
                }

                if (num63 < npc.frameCounter)
                {
                    npc.frameCounter -= 1.0;
                }

                if (npc.frameCounter < 0.0)
                {
                    npc.frameCounter = 0.0;
                }

                if (npc.frameCounter > 15.0)
                {
                    npc.frameCounter = 15.0;
                }

                

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.alpha <= 0)
                {
                    timer++;
                }
                int aiTimerFire = 300;
                Player targetPlayer = Main.player[npc.target];
                if (targetPlayer != null && timer >= 300 && num62 < 350f)
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 20);
                        Vector2 dir = Vector2.Normalize(targetPlayer.Center - npc.Center);
                        dir *= 5f;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, dir.X, dir.Y, mod.ProjectileType("YamataABreath"), npc.damage / 4, 0f, Main.myPlayer);
                    }
                    timer = 0;
                }
            }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            int Frame = 0;
            if(npc.frameCounter < 5.0)
            {
                Frame = 0;
            }
            else if(npc.frameCounter < 10.0)
            {
                Frame = 1;
            }
            else
            {
                Frame = 2;
            }
            npc.frame.Y = Frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
        //    int frameCount = /*npc.type == Terraria.ModLoader.mod.NPCType("AsheDragon") ? 3 :*/ 1;
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation + 3.1415f, npc.direction, Main.npcFrameCount[npc.type], npc.frame, dColor, true);

            return false;
        }

        public override void NPCLoot()
        {
            if (npc.life <= 0)
            {
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity), mod.ProjectileType("Yanker"), npc.damage / 2, 0f, Main.myPlayer);
                }
            }
        }
    }

    public class YamataSnakeBody : YamataSnake
    {
        public override string Texture => "AAModEXAI/Bosses/Yamata/YamataHeadSnake/YamataSnakeBody";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yamata's Head");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.width = 34;
            npc.height = 50;
            npc.dontCountMe = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override bool PreAI()
        {
            if (npc.ai[3] > 0)
            {
                npc.realLife = (int)npc.ai[3];
            }

            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }

            if (Main.player[npc.target].dead && npc.timeLeft > 300)
            {
                npc.timeLeft = 300;
            }

            npc.direction = npc.velocity.X < 0f ? -1 : 1;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!Main.npc[(int)npc.ai[1]].active)
                {
                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.active = false;
                    npc.netUpdate = true;
                }
            }

            if (Main.npc[(int)npc.ai[1]].alpha < 128)
            {
                if (npc.alpha != 0)
                {
                    for (int num934 = 0; num934 < 2; num934++)
                    {
                        int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModContent.DustType<YamataDust>(), 0f, 0f, 100, default, 2f);
                        Main.dust[num935].noGravity = false;
                        Main.dust[num935].noLight = false;
                    }
                }

                npc.alpha -= 42;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
            }


            if (npc.ai[1] < (double)Main.npc.Length)
            {
                float dirX = Main.npc[(int)npc.ai[1]].Center.X - npc.Center.X;
                float dirY = Main.npc[(int)npc.ai[1]].Center.Y - npc.Center.Y;
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;

                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - npc.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                npc.direction = dirX < 0f ? 1 : -1;
                npc.position.X += posX;
                npc.position.Y += posY;
            }

            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 1, npc.frame, drawColor, true);
            return false;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[npc.target];
            if (player.vortexStealthActive && projectile.ranged)
            {
                damage /= 2;
                crit = false;
            }

            if (projectile.penetrate == -1 && !projectile.minion)
            {
                damage = (int)(damage * .2f);
            }
            else if(projectile.penetrate == -1 && projectile.minion)
            {
                damage = (int)(damage * .7f);
            }
            else if (projectile.penetrate >= 1)
            {
                projectile.damage *= (int).2;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.8f);
        }
    }

    public class YamataSnakeTail : YamataSnake
    {
        public override string Texture => "AAModEXAI/Bosses/Yamata/YamataHeadSnake/YamataSnakeTail";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yamata's Head");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.width = 26;
            npc.height = 60;
            npc.dontCountMe = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override bool PreAI()
        {
            if (npc.ai[3] > 0)
            {
                npc.realLife = (int)npc.ai[3];
            }

            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }

            if (Main.player[npc.target].dead && npc.timeLeft > 300)
            {
                npc.timeLeft = 300;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!Main.npc[(int)npc.ai[1]].active)
                {
                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.active = false;
                    npc.netUpdate = true;
                }
            }

            if (Main.npc[(int)npc.ai[1]].alpha < 128)
            {
                if (npc.alpha != 0)
                {
                    for (int num934 = 0; num934 < 2; num934++)
                    {
                        int num935 = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<YamataDust>(), 0f, 0f, 100, default, 2f);
                        Main.dust[num935].noGravity = false;
                        Main.dust[num935].noLight = false;
                    }
                }

                npc.alpha -= 42;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
            }

            npc.direction = npc.velocity.X < 0f ? -1 : 1;

            if (npc.ai[1] < (double)Main.npc.Length)
            {
                float dirX = Main.npc[(int)npc.ai[1]].Center.X - npc.Center.X;
                float dirY = Main.npc[(int)npc.ai[1]].Center.Y - npc.Center.Y;
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;

                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - npc.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;

                npc.direction = dirX < 0f ? 1 : -1;
                npc.position.X += posX;
                npc.position.Y += posY;
            }

            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 1, npc.frame, drawColor, true);
            return false;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[npc.target];
            if (player.vortexStealthActive && projectile.ranged)
            {
                damage /= 2;
                crit = false;
            }

            if (projectile.penetrate == -1 && !projectile.minion)
            {
                damage = (int)(damage * .2f);
            }
            else if(projectile.penetrate == -1 && projectile.minion)
            {
                damage = (int)(damage * .7f);
            }
            else if (projectile.penetrate >= 1)
            {
                projectile.damage *= (int).2;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.8f);
        }
    }
}