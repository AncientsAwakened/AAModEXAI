
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using AAModEXAI.Dusts;
using AAModEXAI.Localization;
using AAModEXAI.Bosses;
using AAModEXAI.Bosses.Akuma.Awakened;

namespace AAModEXAI.Bosses.Akuma
{
    public class AkumaTransition : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Of Fury");
            Main.npcFrameCount[npc.type] = 8;
            Terraria.ID.NPCID.Sets.TechnicallyABoss[npc.type] = true;
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
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public int RVal = 255;
        public int BVal = 0;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, Main.npcFrameCount[npc.type], npc.frame, npc.GetAlpha(new Color(RVal, 125, BVal)), true);
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
					npc.frame.Y += 42;
				}
				if (npc.frame.Y > 42 * 7)
				{
					npc.frame.Y = 0;
				}
				if (npc.ai[0] > 300)
				{
					npc.alpha -= 5;
					if (npc.alpha < 0)
					{
						npc.alpha = 0;
					}
				}		
				if (npc.ai[0] >= 660) //after 660 on the server, transition color
				{
					RVal -= 5;
					BVal += 5;
					if (RVal <= 0)
					{
						RVal = 0;
					}
					if (BVal >= 255)
					{
						BVal = 255;
					}
				}
			}
			if(Main.netMode != NetmodeID.MultiplayerClient)
			{
				npc.ai[0]++;	
				if(npc.ai[0] == 300)
				{
					npc.netUpdate = true;
				}else
				if (npc.ai[0] == 300)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("Akuma","AkumaTransition1"), new Color(180, 41, 32));
					npc.netUpdate = true;
				}else
				if (npc.ai[0] == 525)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("Akuma","AkumaTransition2"), new Color(180, 41, 32));
				}else
				if(npc.ai[0] == 750) //sync so the color transition occurs
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("Akuma","AkumaTransition3"), new Color(175, 75, 255));
                    npc.netUpdate = true;
				}else
				if (npc.ai[0] == 976)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("Akuma","AkumaTransition4"), Color.DeepSkyBlue);
				}else
				if (npc.ai[0] >= 1200 && !NPC.AnyNPCs(ModContent.NPCType<AkumaA>()))
				{
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
					SpawnBossMethod.SpawnBoss(player, ModContent.NPCType<AkumaA>(), false, npc.Center, "", false);
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("Akuma","AkumaTransition5"), Color.Magenta.R, Color.Magenta.G, Color.Magenta.B);
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat(Trans.text("Akuma","AkumaTransition6"), Color.DeepSkyBlue.R, Color.DeepSkyBlue.G, Color.DeepSkyBlue.B);

                    int b = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<Effects.ShockwaveBoom>(), 0, 1, Main.myPlayer, 0, 0);
                    Main.projectile[b].Center = npc.Center;

                    npc.netUpdate = true;
					npc.active = false;
				}
			}
        }

        public void MoveToPoint(Vector2 point)
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
            if (!NPC.AnyNPCs(ModContent.NPCType<AkumaA>()))
            {
                return false;
            }
            return true;
        }

    }
}