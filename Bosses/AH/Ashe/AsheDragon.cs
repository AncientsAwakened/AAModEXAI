
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.AH.Ashe
{
    public class AsheDragon : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ashen Dragon");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.noTileCollide = true;
            npc.npcSlots = 5f;
            npc.width = 32;
            npc.height = 32;
            npc.aiStyle = 6;
            npc.netAlways = true;
            npc.damage = 100;
            npc.defense = 40;
            npc.lifeMax = 10000;
            npc.HitSound = SoundID.NPCHit56;
            npc.DeathSound = SoundID.NPCDeath60;
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

            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || (flag && Main.player[npc.target].position.Y < Main.worldSurface * 16.0))
            {
                npc.TargetClosest(true);
            }

            if (Main.player[npc.target].dead || (flag && Main.player[npc.target].position.Y < Main.worldSurface * 16.0))
            {
                if (npc.timeLeft > 300)
                {
                    npc.timeLeft = 300;
                }

                if (flag)
                {
                    npc.velocity.Y += speedY;
                }
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.ai[0] == 0f)
                {
                    npc.ai[3] = npc.whoAmI;
                    npc.realLife = npc.whoAmI;
                    int npcWhoAmI = npc.whoAmI;

                    for (int l = 0; l < 30; l++)
                    {
                        int type = mod.NPCType("AsheDragonBody");
                        if ((l - 2) % 4 == 0 && l < 26)
                        {
                            type = mod.NPCType("AsheDragonArms");
                        }
                        else if (l == 27)
                        {
                            type = mod.NPCType("AsheDragonBody1");
                        }
                        else if (l == 28)
                        {
                            type = mod.NPCType("AsheDragonBody2");
                        }
                        else if (l == 29)
                        {
                            type = mod.NPCType("AsheDragonTail");
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

        public override void NPCLoot()
        {
            for (int num468 = 0; num468 < 3; num468++)
            {
                int num469 = Dust.NewDust(npc.Center, npc.width, 1, ModLoader.GetMod("AAMod").DustType("AkumaDust"), -npc.velocity.X * 0.2f, -npc.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;

                num469 = Dust.NewDust(npc.Center, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("AkumaDust"), -npc.velocity.X * 0.2f, -npc.velocity.Y * 0.2f, 100, default);
                Main.dust[num469].velocity *= 2f;
            }
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
        //    int frameCount = /*npc.type == Terraria.ModLoader.mod.NPCType("AsheDragon") ? 3 :*/ 1;
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, Main.npcFrameCount[npc.type], npc.frame, new Color(Color.White.R, Color.White.G, Color.White.B, 100), true);

            return false;
        }
    }

    public class AsheDragonArms : AsheDragon
    {
        public override string Texture => "AAModEXAI/Bosses/AH/Ashe/AsheDragonArms";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ashen Dragon");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.dontCountMe = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                npc.position.X = npc.Center.X;
                npc.position.Y = npc.Center.Y;
                npc.width = 44;
                npc.height = 78;
                npc.position.X = npc.Center.X;
                npc.position.Y = npc.Center.Y;

                int dust1 = ModLoader.GetMod("AAMod").DustType("AkumaDust");
                Dust.NewDust(npc.position, npc.width, npc.height, dust1, 0f, 0f, 0);
                Main.dust[dust1].velocity *= 0.5f;
                Main.dust[dust1].scale *= 1.3f;
                Main.dust[dust1].fadeIn = 1f;
                Main.dust[dust1].noGravity = false;

                int dust2 = ModLoader.GetMod("AAMod").DustType("AkumaDust");
                Dust.NewDust(npc.position, npc.width, npc.height, dust2, 0f, 0f, 0);
                Main.dust[dust2].velocity *= 0.5f;
                Main.dust[dust2].scale *= 1.3f;
                Main.dust[dust2].fadeIn = 1f;
                Main.dust[dust2].noGravity = true;
            }
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
                        int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 100, default, 2f);
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
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 1, npc.frame, new Color(Color.White.R, Color.White.G, Color.White.B, 100), true);
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

    public class AsheDragonBody : AsheDragon
    {
        public override string Texture => "AAModEXAI/Bosses/AH/Ashe/AsheDragonBody";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ashen Dragon");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.dontCountMe = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                npc.position.X = npc.Center.X;
                npc.position.Y = npc.Center.Y;
                npc.width = 44;
                npc.height = 78;
                npc.position.X = npc.Center.X;
                npc.position.Y = npc.Center.Y;

                int dust1 = ModLoader.GetMod("AAMod").DustType("AkumaDust");
                Dust.NewDust(npc.position, npc.width, npc.height, dust1, 0f, 0f, 0);
                Main.dust[dust1].velocity *= 0.5f;
                Main.dust[dust1].scale *= 1.3f;
                Main.dust[dust1].fadeIn = 1f;
                Main.dust[dust1].noGravity = false;

                int dust2 = ModLoader.GetMod("AAMod").DustType("AkumaDust");
                Dust.NewDust(npc.position, npc.width, npc.height, dust2, 0f, 0f, 0);
                Main.dust[dust2].velocity *= 0.5f;
                Main.dust[dust2].scale *= 1.3f;
                Main.dust[dust2].fadeIn = 1f;
                Main.dust[dust2].noGravity = true;
            }
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
                        int num935 = Dust.NewDust(npc.position, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 100, default, 2f);
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

    public class AsheDragonBody1 : AsheDragon
    {
        public override string Texture => "AAModEXAI/Bosses/AH/Ashe/AsheDragonBody1";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ashen Dragon");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.dontCountMe = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                npc.position.X = npc.Center.X;
                npc.position.Y = npc.Center.Y;
                npc.width = 44;
                npc.height = 78;
                npc.position.X = npc.Center.X;
                npc.position.Y = npc.Center.Y;

                int dust1 = ModLoader.GetMod("AAMod").DustType("AkumaDust");
                Dust.NewDust(npc.position, npc.width, npc.height, dust1, 0f, 0f, 0);
                Main.dust[dust1].velocity *= 0.5f;
                Main.dust[dust1].scale *= 1.3f;
                Main.dust[dust1].fadeIn = 1f;
                Main.dust[dust1].noGravity = false;

                int dust2 = ModLoader.GetMod("AAMod").DustType("AkumaDust");
                Dust.NewDust(npc.position, npc.width, npc.height, dust2, 0f, 0f, 0);
                Main.dust[dust2].velocity *= 0.5f;
                Main.dust[dust2].scale *= 1.3f;
                Main.dust[dust2].fadeIn = 1f;
                Main.dust[dust2].noGravity = true;
            }
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
                        int num935 = Dust.NewDust(npc.position, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 100, default, 2f);
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
                float dirX = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - npc.Center.X;
                float dirY = Main.npc[(int)npc.ai[1]].position.Y + Main.npc[(int)npc.ai[1]].height / 2 - npc.Center.Y;
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

    public class AsheDragonBody2 : AsheDragon
    {
        public override string Texture => "AAModEXAI/Bosses/AH/Ashe/AsheDragonBody2";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ashen Dragon");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.dontCountMe = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                npc.position.X = npc.Center.X;
                npc.position.Y = npc.Center.Y;
                npc.width = 44;
                npc.height = 78;
                npc.position.X = npc.Center.X;
                npc.position.Y = npc.Center.Y;

                int dust1 = ModLoader.GetMod("AAMod").DustType("AkumaDust");
                Dust.NewDust(npc.position, npc.width, npc.height, dust1, 0f, 0f, 0);
                Main.dust[dust1].velocity *= 0.5f;
                Main.dust[dust1].scale *= 1.3f;
                Main.dust[dust1].fadeIn = 1f;
                Main.dust[dust1].noGravity = false;

                int dust2 = ModLoader.GetMod("AAMod").DustType("AkumaDust");
                Dust.NewDust(npc.position, npc.width, npc.height, dust2, 0f, 0f, 0);
                Main.dust[dust2].velocity *= 0.5f;
                Main.dust[dust2].scale *= 1.3f;
                Main.dust[dust2].fadeIn = 1f;
                Main.dust[dust2].noGravity = true;
            }
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
                        int num935 = Dust.NewDust(npc.position, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 100, default, 2f);
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

    public class AsheDragonTail : AsheDragon
    {
        public override string Texture => "AAModEXAI/Bosses/AH/Ashe/AsheDragonTail";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ashen Dragon");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.dontCountMe = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                npc.position.X = npc.Center.X;
                npc.position.Y = npc.Center.Y;
                npc.width = 44;
                npc.height = 78;
                npc.position.X = npc.Center.X;
                npc.position.Y = npc.Center.Y;

                int dust1 = ModLoader.GetMod("AAMod").DustType("AkumaDust");
                Dust.NewDust(npc.position, npc.width, npc.height, dust1, 0f, 0f, 0);
                Main.dust[dust1].velocity *= 0.5f;
                Main.dust[dust1].scale *= 1.3f;
                Main.dust[dust1].fadeIn = 1f;
                Main.dust[dust1].noGravity = false;

                int dust2 = ModLoader.GetMod("AAMod").DustType("AkumaDust");
                Dust.NewDust(npc.position, npc.width, npc.height, dust2, 0f, 0f, 0);
                Main.dust[dust2].velocity *= 0.5f;
                Main.dust[dust2].scale *= 1.3f;
                Main.dust[dust2].fadeIn = 1f;
                Main.dust[dust2].noGravity = true;
            }
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
                        int num935 = Dust.NewDust(npc.position, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("AkumaDust"), 0f, 0f, 100, default, 2f);
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
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 1, npc.frame, new Color(Color.White.R, Color.White.G, Color.White.B, 100), true);
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