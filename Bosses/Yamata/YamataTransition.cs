
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Yamata
{
    public class YamataTransition : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit of Wrath");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 100;
            npc.height = 100;
            npc.friendly = false;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.timeLeft = 10;
            npc.alpha = 255;
            npc.scale = .1f;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public int RVal = 125;
        public int BVal = 255;


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 24, npc.frame, npc.GetAlpha(lightColor), true);
            BaseDrawing.DrawTexture(spriteBatch, ModLoader.GetMod("AAMod").GetTexture("Glowmasks/YamataTransition"), 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 24, npc.frame, npc.GetAlpha(new Color(RVal, 0, BVal)), true);
            return false;
        }

        public override void AI()
        {
			npc.TargetClosest();			
            Player player = Main.player[npc.target];
            MoveToPoint(player.Center - new Vector2(0, 300f));

            if (Vector2.Distance(npc.Center, player.Center) > 2000)
            {
                npc.alpha = 255;
                npc.Center = player.Center - new Vector2(0, 300f);
            }
			
			if(Main.netMode != NetmodeID.Server) //clientside stuff
			{
				npc.frameCounter++;
				if (npc.frameCounter >= 7)
				{
					npc.frameCounter = 0;
					npc.frame.Y += Main.npcTexture[npc.type].Height / 4 ;
				}

				if (npc.frame.Y > Main.npcTexture[npc.type].Height / 4 * 3)
				{
					npc.frame.Y = 0 ;
				}
				if (npc.ai[0] > 375)
				{
					if (npc.alpha < 0)
					{
						npc.alpha = 0;
					}
                    else
                    {
                        npc.alpha -= 5;
                    }
                    if (npc.scale < 1)
                    {
                        npc.scale += .02f;
                    }
                    else
                    {
                        npc.scale = 1;
                    }
				}
				if (npc.ai[0] >= 375) //after he says 'nyeh' on the server, change music on the client
				{
					music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Yamata2");
				}
				if (npc.ai[0] >= 900) //after he says 'as if' on the server, transition color
				{
					RVal += 5;
					BVal -= 5;
					if (RVal <= 90)
					{
						BVal = 90;
					}
					if (RVal >= 255)
					{
						RVal = 255;
					}
				}
			}
			if(Main.netMode != NetmodeID.MultiplayerClient)
			{
				npc.ai[0]++;

				if (npc.ai[0] == 375)    
				{
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("YamataTransition1"), new Color(45, 46, 70));
					npc.netUpdate = true;
				}else
				if (npc.ai[0] == 650)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("YamataTransition2"), new Color(45, 46, 70));
				}else
				if (npc.ai[0] == 900)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("YamataTransition3"), new Color(45, 46, 70));
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("YamataTransition7"), Color.PaleVioletRed);
                    npc.netUpdate = true;
				}else
				if (npc.ai[0] == 1100)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("YamataTransition4"), new Color(146, 30, 68));
				}else
				if (npc.ai[0] >= 1455 && !NPC.AnyNPCs(mod.NPCType("YamataA")))
				{
					AAModGlobalNPC.SpawnBoss(player, mod.NPCType("YamataA"), false, npc.Center, "", false);
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("YamataTransition5"), Color.Magenta.R, Color.Magenta.G, Color.Magenta.B);
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("YamataTransition6"), new Color(146, 30, 68));

                    int b = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModLoader.GetMod("AAMod").ProjectileType("ShockwaveBoom"), 0, 1, Main.myPlayer, 0, 0);
                    Main.projectile[b].Center = npc.Center;

                    for(int proj = 0; proj < 1000; proj ++)
                    {
                        if (Main.projectile[proj].active && Main.projectile[proj].friendly && !Main.projectile[proj].hostile)
                        {
                            Main.projectile[proj].hostile = true;
                            Main.projectile[proj].friendly = false;
                            Vector2 vector = Main.projectile[proj].Center - npc.Center;
                            vector.Normalize();
                            Vector2 reflectvelocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                            reflectvelocity.Normalize();
                            reflectvelocity *= vector.Length();
                            reflectvelocity += vector * 20f;
                            reflectvelocity.Normalize();
                            reflectvelocity *= vector.Length();
                            if(reflectvelocity.Length() < 20f)
                            {
                                reflectvelocity.Normalize();
                                reflectvelocity *= 20f;
                            }

                            Main.projectile[proj].penetrate = 1;

                            Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().reflectvelocity = reflectvelocity;
                            Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().isReflecting = true;
                            Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().ReflectConter = 180;
                        }
                    }

                    Main.PlaySound(ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/YamataRoar"), npc.position);
                    Vector2 position = npc.Center + (Vector2.One * -20f);
                    int num84 = 40;
                    int height3 = num84;
                    for (int num85 = 0; num85 < 3; num85++)
                    {
                        int num86 = Dust.NewDust(position, num84, height3, 240, 0f, 0f, 100, default, 1.5f);
                        Main.dust[num86].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                    }
                    for (int num87 = 0; num87 < 15; num87++)
                    {
                        int num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<YamataADust>(), 0f, 0f, 200, default, 3.7f);
                        Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                        Main.dust[num88].noGravity = true;
                        Main.dust[num88].noLight = true;
                        Main.dust[num88].velocity *= 3f;
                        Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                        num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<YamataADust>(), 0f, 0f, 100, default, 1.5f);
                        Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                        Main.dust[num88].velocity *= 2f;
                        Main.dust[num88].noGravity = true;
                        Main.dust[num88].fadeIn = 1f;
                        Main.dust[num88].color = Color.Crimson * 0.5f;
                        Main.dust[num88].noLight = true;
                        Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * 8f;
                    }
                    for (int num89 = 0; num89 < 10; num89++)
                    {
                        int num90 = Dust.NewDust(position, num84, height3, ModContent.DustType<YamataADust>(), 0f, 0f, 0, default, 2.7f);
                        Main.dust[num90].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                        Main.dust[num90].noGravity = true;
                        Main.dust[num90].noLight = true;
                        Main.dust[num90].velocity *= 3f;
                        Main.dust[num90].velocity += npc.DirectionTo(Main.dust[num90].position) * 2f;
                    }
                    for (int num91 = 0; num91 < 30; num91++)
                    {
                        int num92 = Dust.NewDust(position, num84, height3, ModContent.DustType<YamataADust>(), 0f, 0f, 0, default, 1.5f);
                        Main.dust[num92].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                        Main.dust[num92].noGravity = true;
                        Main.dust[num92].velocity *= 3f;
                        Main.dust[num92].velocity += npc.DirectionTo(Main.dust[num92].position) * 3f;
                    }

                    npc.netUpdate = true;
					npc.active = false;				
				}
			}
        }

        public override bool PreAI()
        {
            if (AAConfigClient.Instance.NoBossDialogue)
            {
                npc.TargetClosest();
                Player player = Main.player[npc.target];
                MoveToPoint(player.Center - new Vector2(0, 300f));

                if (Vector2.Distance(npc.Center, player.Center) > 2000)
                {
                    npc.alpha = 255;
                    npc.Center = player.Center - new Vector2(0, 300f);
                }

                if (Main.netMode != NetmodeID.Server) //clientside stuff
                {
                    npc.frameCounter++;
                    if (npc.frameCounter >= 7)
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y += 52;
                    }
                    if (npc.frame.Y > 52 * 5)
                    {
                        npc.frame.Y = 0;
                    }
                    if (npc.ai[0] > 180)
                    {
                        npc.alpha -= 5;
                        if (npc.alpha < 0)
                        {
                            npc.alpha = 0;
                        }
                    }
                    if (npc.ai[0] >= 180) //after he says 'heh' on the server, change music on the client
                    {
                        music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/Akuma2");
                    }
                    if (npc.ai[0] >= 380)
                    {
                        RVal -= 5;
                        BVal += 5;
                        if (RVal <= 0)
                        {
                            RVal = 0;
                        }
                        if (BVal >= 380)
                        {
                            BVal = 255;
                        }
                    }
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.ai[0]++;
                    if (npc.ai[0] == 180)
                    {
                        npc.netUpdate = true;
                    }
                    else
                    if (npc.ai[0] >= 600 && !NPC.AnyNPCs(mod.NPCType("YamataA")))
                    {
                        AAModGlobalNPC.SpawnBoss(player, mod.NPCType("YamataA"), false, npc.Center, "", false);
                        if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(AAMod.Lang.BossChat("YamataTransition5"), Color.Magenta.R, Color.Magenta.G, Color.Magenta.B);
                        for(int proj = 0; proj < 1000; proj ++)
                        {
                            if (Main.projectile[proj].active && Main.projectile[proj].friendly && !Main.projectile[proj].hostile)
                            {
                                Main.projectile[proj].hostile = true;
                                Main.projectile[proj].friendly = false;
                                Vector2 vector = Main.projectile[proj].Center - npc.Center;
                                vector.Normalize();
                                Vector2 reflectvelocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                                reflectvelocity.Normalize();
                                reflectvelocity *= vector.Length();
                                reflectvelocity += vector * 20f;
                                reflectvelocity.Normalize();
                                reflectvelocity *= vector.Length();
                                if(reflectvelocity.Length() < 20f)
                                {
                                    reflectvelocity.Normalize();
                                    reflectvelocity *= 20f;
                                }

                                Main.projectile[proj].penetrate = 1;

                                Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().reflectvelocity = reflectvelocity;
                                Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().isReflecting = true;
                                Main.projectile[proj].GetGlobalProjectile<AAModEXAIGlobalProjectile>().ReflectConter = 180;
                            }
                        }
                        int b = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModLoader.GetMod("AAMod").ProjectileType("ShockwaveBoom"), 0, 1, Main.myPlayer, 0, 0);
                        Main.projectile[b].Center = npc.Center;

                        npc.netUpdate = true;
                        npc.active = false;
                    }
                }
                return false;
            }
            return true;
        }

        public void MoveToPoint(Vector2 point, bool goUpFirst = false)
        {
            float moveSpeed = 14f;
            if (moveSpeed == 0f || npc.Center == point) return; //don't move if you have no move speed
            float velMultiplier = 1f;
            Vector2 dist = point - npc.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < moveSpeed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
            }
            if (length < 200f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 100f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 50f)
            {
                moveSpeed *= 0.5f;
            }
            npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            npc.velocity *= moveSpeed;
            npc.velocity *= velMultiplier;
        }

        public override bool CheckActive()
        {
            if (!NPC.AnyNPCs(mod.NPCType("YamataA")))
            {
                return false;
            }
            return true;
        }
    }
}