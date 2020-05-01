using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.Audio;using AAMod;

namespace AAModEXAI.Bosses.Shen
{
    public class Shenling : ModNPC
	{
        public override string Texture => "AAMod/NPCs/Bosses/Shen/Shenling";


        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Discordian Serpent");
            Main.npcFrameCount[npc.type] = 2;
        }
        

        public override void SetDefaults()
		{
			npc.noTileCollide = true;
			npc.height = 16;
			npc.width = 16;
			npc.aiStyle = -1;
			npc.netAlways = true;
			npc.knockBackResist = 0f;
            npc.damage = 50;
            npc.defense = 90;
            npc.lifeMax = 8000;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = new LegacySoundStyle(2, 124, Terraria.Audio.SoundType.Sound);
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            npc.buffImmune[103] = false;
            npc.alpha = 255;
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
                    int num2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 228, 0f, 0f, 100, default, 2f);
                    Main.dust[num2].noGravity = true;
                    Main.dust[num2].noLight = true;
                }
            }
            npc.alpha -= 42;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }

            bool flag = false;
            float num4 = 0.2f;
            int num5 = npc.type;
            flag = true;

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
                    npc.velocity.Y = npc.velocity.Y + num4;
                }
            }
            if (Main.netMode != 1)
            {

                if (npc.ai[0] == 0f)
                {
                    npc.ai[3] = npc.whoAmI;
                    npc.realLife = npc.whoAmI;
                    int num9 = npc.whoAmI;
                    for (int l = 0; l < 10; l++)
                    {
                        int Body1 = mod.NPCType("ShenlingBody1");
                        int Body2 = mod.NPCType("ShenlingBody2");

                        int SpawnBody1 = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), Body1, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                        Main.npc[SpawnBody1].ai[3] = npc.whoAmI;
                        Main.npc[SpawnBody1].realLife = npc.whoAmI;
                        Main.npc[SpawnBody1].ai[1] = num9;
                        Main.npc[num9].ai[0] = SpawnBody1;
                        num9 = SpawnBody1;
                        npc.netUpdate = true;

                        int SpawnBody2 = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), Body2, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                        Main.npc[SpawnBody2].ai[3] = npc.whoAmI;
                        Main.npc[SpawnBody2].realLife = npc.whoAmI;
                        Main.npc[SpawnBody2].ai[1] = num9;
                        Main.npc[num9].ai[0] = SpawnBody2;
                        num9 = SpawnBody2;
                        npc.netUpdate = true;
                    }
                    int num10 = mod.NPCType("ShenlingTail");
                    if(Main.netMode != 1)
                    {
                        int num11 = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), num10, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                        if (Main.netMode == 2 && num11 < 200) NetMessage.SendData(23, -1, -1, null, num11);
                        Main.npc[num11].ai[3] = npc.whoAmI;
                        Main.npc[num11].realLife = npc.whoAmI;
                        Main.npc[num11].ai[1] = num9;
                        Main.npc[num9].ai[0] = num11;
                        num9 = num11;
                    }
                    npc.netUpdate = true;
                }
            }
            int num29 = (int)(npc.position.X / 16f) - 1;
            int num30 = (int)((npc.position.X + npc.width) / 16f) + 2;
            int num31 = (int)(npc.position.Y / 16f) - 1;
            int num32 = (int)((npc.position.Y + npc.height) / 16f) + 2;
            if (num29 < 0)
            {
                num29 = 0;
            }
            if (num30 > Main.maxTilesX)
            {
                num30 = Main.maxTilesX;
            }
            if (num31 < 0)
            {
                num31 = 0;
            }
            if (num32 > Main.maxTilesY)
            {
                num32 = Main.maxTilesY;
            }
            if (npc.velocity.X < 0f)
            {
                npc.spriteDirection = 1;
            }
            else if (npc.velocity.X > 0f)
            {
                npc.spriteDirection = -1;
            }
            float num37 = 10f;
            float num38 = 0.07f;
            num37 = 20f;
            num38 = 0.55f;

            Vector2 vector2 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num40 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2;
            float num41 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2;

            num40 = (int)(num40 / 16f) * 16;
            num41 = (int)(num41 / 16f) * 16;
            vector2.X = (int)(vector2.X / 16f) * 16;
            vector2.Y = (int)(vector2.Y / 16f) * 16;
            num40 -= vector2.X;
            num41 -= vector2.Y;

            float num53 = (float)Math.Sqrt(num40 * num40 + num41 * num41);
            if (npc.ai[1] > 0f && npc.ai[1] < Main.npc.Length)
            {
                try
                {
                    vector2 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                    num40 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - vector2.X;
                    num41 = Main.npc[(int)npc.ai[1]].position.Y + Main.npc[(int)npc.ai[1]].height / 2 - vector2.Y;
                }
                catch
                {
                }
                npc.rotation = (float)Math.Atan2(num41, num40) + 1.57f;
                num53 = (float)Math.Sqrt(num40 * num40 + num41 * num41);
                int num54 = npc.width;
                num54 = 42;
                num53 = (num53 - num54) / num53;
                num40 *= num53;
                num41 *= num53;
                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + num40;
                npc.position.Y = npc.position.Y + num41;
                if (num40 < 0f)
                {
                    npc.spriteDirection = 1;
                    return;
                }
                if (num40 > 0f)
                {
                    npc.spriteDirection = -1;
                    return;
                }
            }
            else
            {
                num53 = (float)Math.Sqrt(num40 * num40 + num41 * num41);
                float num56 = Math.Abs(num40);
                float num57 = Math.Abs(num41);
                float num58 = num37 / num53;
                num40 *= num58;
                num41 *= num58;
                bool flag6 = false;
                if (((npc.velocity.X > 0f && num40 < 0f) || (npc.velocity.X < 0f && num40 > 0f) || (npc.velocity.Y > 0f && num41 < 0f) || (npc.velocity.Y < 0f && num41 > 0f)) && Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) > num38 / 2f && num53 < 300f)
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
                            npc.velocity.X = npc.velocity.X - npc.direction;
                        }
                        npc.velocity.X = npc.velocity.X * 1.1f;
                    }
                    else if (npc.velocity.Y > -num37)
                    {
                        npc.velocity.Y = npc.velocity.Y - num38;
                    }
                }
                if (!flag6)
                {
                    if ((npc.velocity.X > 0f && num40 > 0f) || (npc.velocity.X < 0f && num40 < 0f) || (npc.velocity.Y > 0f && num41 > 0f) || (npc.velocity.Y < 0f && num41 < 0f))
                    {
                        if (npc.velocity.X < num40)
                        {
                            npc.velocity.X = npc.velocity.X + num38;
                        }
                        else if (npc.velocity.X > num40)
                        {
                            npc.velocity.X = npc.velocity.X - num38;
                        }
                        if (npc.velocity.Y < num41)
                        {
                            npc.velocity.Y = npc.velocity.Y + num38;
                        }
                        else if (npc.velocity.Y > num41)
                        {
                            npc.velocity.Y = npc.velocity.Y - num38;
                        }
                        if (Math.Abs(num41) < num37 * 0.2 && ((npc.velocity.X > 0f && num40 < 0f) || (npc.velocity.X < 0f && num40 > 0f)))
                        {
                            if (npc.velocity.Y > 0f)
                            {
                                npc.velocity.Y = npc.velocity.Y + num38 * 2f;
                            }
                            else
                            {
                                npc.velocity.Y = npc.velocity.Y - num38 * 2f;
                            }
                        }
                        if (Math.Abs(num40) < num37 * 0.2 && ((npc.velocity.Y > 0f && num41 < 0f) || (npc.velocity.Y < 0f && num41 > 0f)))
                        {
                            if (npc.velocity.X > 0f)
                            {
                                npc.velocity.X = npc.velocity.X + num38 * 2f;
                            }
                            else
                            {
                                npc.velocity.X = npc.velocity.X - num38 * 2f;
                            }
                        }
                    }
                    else if (num56 > num57)
                    {
                        if (npc.velocity.X < num40)
                        {
                            npc.velocity.X = npc.velocity.X + num38 * 1.1f;
                        }
                        else if (npc.velocity.X > num40)
                        {
                            npc.velocity.X = npc.velocity.X - num38 * 1.1f;
                        }
                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num37 * 0.5)
                        {
                            if (npc.velocity.Y > 0f)
                            {
                                npc.velocity.Y = npc.velocity.Y + num38;
                            }
                            else
                            {
                                npc.velocity.Y = npc.velocity.Y - num38;
                            }
                        }
                    }
                    else
                    {
                        if (npc.velocity.Y < num41)
                        {
                            npc.velocity.Y = npc.velocity.Y + num38 * 1.1f;
                        }
                        else if (npc.velocity.Y > num41)
                        {
                            npc.velocity.Y = npc.velocity.Y - num38 * 1.1f;
                        }
                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num37 * 0.5)
                        {
                            if (npc.velocity.X > 0f)
                            {
                                npc.velocity.X = npc.velocity.X + num38;
                            }
                            else
                            {
                                npc.velocity.X = npc.velocity.X - num38;
                            }
                        }
                    }
                }
                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

                float num62 = Vector2.Distance(Main.player[npc.target].Center, npc.Center);
                int num63 = 0;
                if (Vector2.Normalize(Main.player[npc.target].Center - npc.Center).ToRotation().AngleTowards(npc.velocity.ToRotation(), 1.57079637f) == npc.velocity.ToRotation() && num62 < 350f)
                {
                    num63 = 4;
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
                if (npc.frameCounter > 4.0)
                {
                    npc.frameCounter = 4.0;
                }
            }
        }

        public override void NPCLoot()
        {
            for (int num468 = 0; num468 < 3; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y), npc.width, 1, ModLoader.GetMod("AAMod").DustType("DiscordLight"), -npc.velocity.X * 0.2f,
                    -npc.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y), npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("DiscordLight"), -npc.velocity.X * 0.2f,
                    -npc.velocity.Y * 0.2f, 100, default);
                Main.dust[num469].velocity *= 2f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
            for (int k = 0; k < npc.oldPos.Length; k++)
            {
                Texture2D Trail = Main.npcTexture[npc.type];
                Color lightColor = drawColor;
                Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                Color color = npc.GetAlpha(lightColor) * ((npc.oldPos.Length - k) / (float)npc.oldPos.Length);
                spriteBatch.Draw(Trail, drawPos, null, color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }

    public class ShenlingBody1 : Shenling
    {
        public override string Texture => "AAMod/NPCs/Bosses/Shen/ShenlingBody1";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discordian Serpent");
            Main.npcFrameCount[npc.type] = 1;
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

                npc.position.X = npc.position.X + npc.width / 2;
                npc.position.Y = npc.position.Y + npc.height / 2;
                npc.width = 44;
                npc.height = 78;
                npc.position.X = npc.position.X - npc.width / 2;
                npc.position.Y = npc.position.Y - npc.height / 2;
                int dust1 = ModLoader.GetMod("AAMod").DustType("DiscordLight");
                int dust2 = ModLoader.GetMod("AAMod").DustType("DiscordLight");
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

        public override bool PreAI()
        {
            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
                npc.TargetClosest(true);
            if (Main.player[npc.target].dead && npc.timeLeft > 300)
                npc.timeLeft = 300;

            if (Main.netMode != 1)
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
                        int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("DiscordLight"), 0f, 0f, 100, default, 2f);
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
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;
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
            else if (projectile.penetrate >= 1)
            {
                projectile.damage *= (int).2;
            }
        }
    }

    public class ShenlingBody2 : Shenling
    {
        public override string Texture => "AAMod/NPCs/Bosses/Shen/ShenlingBody2";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discordian Serpent");
            Main.npcFrameCount[npc.type] = 1;
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

                npc.position.X = npc.position.X + npc.width / 2;
                npc.position.Y = npc.position.Y + npc.height / 2;
                npc.width = 44;
                npc.height = 78;
                npc.position.X = npc.position.X - npc.width / 2;
                npc.position.Y = npc.position.Y - npc.height / 2;
                int dust1 = ModLoader.GetMod("AAMod").DustType("DiscordLight");
                int dust2 = ModLoader.GetMod("AAMod").DustType("DiscordLight");
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

        public override bool PreAI()
        {
            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
                npc.TargetClosest(true);
            if (Main.player[npc.target].dead && npc.timeLeft > 300)
                npc.timeLeft = 300;

            if (Main.netMode != 1)
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
                        int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("DiscordLight"), 0f, 0f, 100, default, 2f);
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
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;
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
            else if (projectile.penetrate >= 1)
            {
                projectile.damage *= (int).2;
            }
        }
    }

    public class ShenlingTail : Shenling
    {
        public override string Texture => "AAMod/NPCs/Bosses/Shen/ShenlingTail";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discordian Serpent");
            Main.npcFrameCount[npc.type] = 1;
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

                npc.position.X = npc.position.X + npc.width / 2;
                npc.position.Y = npc.position.Y + npc.height / 2;
                npc.width = 44;
                npc.height = 78;
                npc.position.X = npc.position.X - npc.width / 2;
                npc.position.Y = npc.position.Y - npc.height / 2;
                int dust1 = ModLoader.GetMod("AAMod").DustType("DiscordLight");
                int dust2 = ModLoader.GetMod("AAMod").DustType("DiscordLight");
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

        public override bool PreAI()
        {
            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
                npc.TargetClosest(true);
            if (Main.player[npc.target].dead && npc.timeLeft > 300)
                npc.timeLeft = 300;

            if (Main.netMode != 1)
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
                        int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("DiscordLight"), 0f, 0f, 100, default, 2f);
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
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;
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
            else if (projectile.penetrate >= 1)
            {
                projectile.damage *= (int).2;
            }
        }
    }
}