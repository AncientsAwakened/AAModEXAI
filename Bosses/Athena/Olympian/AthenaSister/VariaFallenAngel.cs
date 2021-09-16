using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod;

namespace AAModEXAI.Bosses.Athena.Olympian.AthenaSister
{
	[AutoloadBossHead]
	public class VariaFallenAngel : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Fallen Angel");
			NPCID.Sets.TrailCacheLength[npc.type] = 8;
			NPCID.Sets.TrailingMode[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 110000;
			npc.aiStyle = -1;
			npc.damage = 110;
			npc.defense = 90;
			npc.width = 152;
			npc.height = 114;
			npc.noGravity = true;
			npc.knockBackResist = 0f;
			npc.boss = true;
			npc.value = (float)Item.buyPrice(0, 8, 0, 0);
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			Main.npcFrameCount[npc.type] = 5;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)((float)(npc.lifeMax * 0.8f) * bossLifeScale);
		}

		public float timer = 0;

		public float AthenaA = 0f;

		public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(AthenaA);
				writer.Write(timer);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                AthenaA = reader.ReadFloat();
				timer = reader.ReadFloat();
            }
        }

		public override void BossLoot(ref string name, ref int potionType)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("It's the contradiction between you two. I will not disturb you.", Color.AntiqueWhite);
			if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("By the way, Varia is a great mod. Welcome to visit us.", Color.AntiqueWhite);
			npc.netUpdate = true;
			potionType = 499;

		}

		public override void AI()
		{
			if(timer++ < 400)
			{
				if(timer == 120)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("What's the matter?", Color.AntiqueWhite);
					npc.netUpdate = true;
				}
				if(timer == 240)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("I'm busy in Varia Mod. ", Color.AntiqueWhite);
					npc.netUpdate = true;
				}
				if(timer == 300)
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
					int b = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<Effects.ShockwaveBoom>(), 0, 0, Main.myPlayer, 0, 10);
					Main.projectile[b].Center = npc.Center;
				}
				if(timer == 360)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("Oh, the earthwalker! He will be in trouble.", Color.AntiqueWhite);
					npc.netUpdate = true;

					timer = 400f;
				}
				if(npc.alpha-- < 0)
				{
					npc.alpha = 0;
				}
				return;
			}
			

			if (npc.life < npc.lifeMax / 2 && npc.ai[3] < 50f)
			{
				npc.ai[3] = 50f;
				if (Main.netMode != NetmodeID.MultiplayerClient) NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FallenAngleForcefield"), 0, (float)npc.whoAmI, 0f, 0f, 0f, 255);
			}
			if (npc.ai[3] >= 50f)
			{
				npc.ai[3] += 1f;
				if (npc.ai[3] < 250f)
				{
					npc.velocity *= 0.3f;
					npc.ai[0] = 0f;
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
				}
			}
			Player player = Main.player[npc.target];
			if (!Main.player[npc.target].dead && Main.npc[(int)AthenaA].active && Main.npc[(int)AthenaA].type == mod.NPCType("AthenaA"))
			{
				despawn = 0;
				BaseAI.AISpaceOctopus(npc, ref npc.ai, 0.15f, 10f, 300f, 70f, null);
				if (npc.rotation > 0.8f)
				{
					npc.rotation = 0.8f;
				}
				if (npc.rotation < -0.8f)
				{
					npc.rotation = -0.8f;
				}
			}
			else
			{
				npc.velocity.Y = npc.velocity.Y - (float)(despawn / 10);
				despawn++;
				if (despawn > 40)
				{
					if(!Main.npc[(int)AthenaA].active || Main.npc[(int)AthenaA].type != mod.NPCType("AthenaA"))
					{
						if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("It's the contradiction between you two. I will not disturb you.", Color.AntiqueWhite);
					}
					else if(Main.player[npc.target].dead)
					{
						if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("Okey! I'm going back.", Color.AntiqueWhite);
					}
					if (Main.netMode != NetmodeID.MultiplayerClient) BaseUtility.Chat("By the way, Varia is a great mod. Welcome to visit us.", Color.AntiqueWhite);
					npc.netUpdate = true;
					npc.active = false;
				}
			}
			npc.ai[1] += 1f;
			if (npc.ai[0] <= 2f)
			{
				if (npc.ai[1] == 10f)
				{
					npc.ai[2] = 0f;
				}
				if (npc.ai[1] > 180f)
				{
					npc.velocity *= 0.3f;
				}
				if (npc.ai[1] > 240f && npc.ai[1] % 2f == 0f)
				{
					if (npc.ai[2] < 360f)
					{
						npc.ai[2] += 40f;
						if (Main.netMode != 1)
						{
							Main.PlaySound(SoundID.Item75, npc.Center);
							Projectile.NewProjectile(npc.Center + Utils.RotatedBy(new Vector2(0f, -35f), (double)MathHelper.ToRadians(npc.ai[2]), default(Vector2)), Utils.RotatedBy(new Vector2(0f, -10f), (double)MathHelper.ToRadians(npc.ai[2]), default(Vector2)), mod.ProjectileType("UnholyTurretBeam"), Main.expertMode ? 16 : 30, 1f, player.whoAmI, 0f, 0f);
							Projectile.NewProjectile(npc.Center + Utils.RotatedBy(new Vector2(0f, 35f), (double)MathHelper.ToRadians(npc.ai[2]), default(Vector2)), Utils.RotatedBy(new Vector2(0f, 10f), (double)MathHelper.ToRadians(npc.ai[2]), default(Vector2)), mod.ProjectileType("UnholyTurretBeam"), Main.expertMode ? 16 : 30, 1f, player.whoAmI, 0f, 0f);
						}
					}
					else
					{
						npc.ai[2] += 60f;
					}
					if (npc.ai[2] >= 740f)
					{
						npc.ai[0] += 1f;
						npc.ai[1] = 181f;
						if (npc.ai[0] > 2f)
						{
							npc.ai[1] = 0f;
						}
						npc.ai[2] = 0f;
						Main.PlaySound(SoundID.Item68, npc.Center);
						for (int i = 0; i < 50; i++)
						{
							Vector2 center = npc.Center;
							Dust dust = Main.dust[Dust.NewDust(center - new Vector2(30f, 30f), 60, 60, 226, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
							if (i == 25)
							{
								if (npc.life < npc.lifeMax / 2)
								{
									for (int j = 0; j < 9; j++)
									{
										if (Main.netMode != 1)
										{
											Main.PlaySound(SoundID.Item75, npc.Center);
											Projectile.NewProjectile(npc.Center, Utils.RotatedBy(new Vector2(0f, -10f), (double)MathHelper.ToRadians((float)Main.rand.Next(360)), default(Vector2)), mod.ProjectileType("UnholyTurretBeam"), Main.expertMode ? 16 : 30, 1f, player.whoAmI, 0f, 0f);
											Projectile.NewProjectile(npc.Center, Utils.RotatedBy(new Vector2(0f, 10f), (double)MathHelper.ToRadians((float)Main.rand.Next(360)), default(Vector2)), mod.ProjectileType("UnholyTurretBeam"), Main.expertMode ? 16 : 30, 1f, player.whoAmI, 0f, 0f);
										}
									}
								}
								npc.position = player.Center - new Vector2((float)(npc.width / 2), (float)(npc.height / 2)) + Utils.RotatedBy(new Vector2(0f, (float)Main.rand.Next(-325, -225)), (double)MathHelper.ToRadians((float)Main.rand.Next(360)), default(Vector2));
							}
						}
					}
				}
			}
			else if (npc.ai[0] <= 4f)
			{
				if (npc.ai[1] % 40f == 0f && Main.netMode != 1)
				{
					Main.PlaySound(SoundID.Item72, npc.Center);
					int num = 1;
					if (npc.life < npc.lifeMax / 2)
					{
						num = 2;
					}
					for (int k = 0; k < num; k++)
					{
						float degrees = (float)Main.rand.Next(-30, 30);
						float x = (float)Main.rand.Next(-600, 601);
						Projectile.NewProjectile(player.Center + new Vector2(x, (float)(-(float)Main.screenHeight) * 0.7f), Utils.RotatedBy(new Vector2(0f, 8f), (double)MathHelper.ToRadians(degrees), default(Vector2)), mod.ProjectileType("InstantTelegraph"), 0, 1f, player.whoAmI, 0f, 0f);
						Projectile.NewProjectile(player.Center + new Vector2(x, (float)(-(float)Main.screenHeight) * 0.7f), Utils.RotatedBy(new Vector2(0f, 4f), (double)MathHelper.ToRadians(degrees), default(Vector2)), mod.ProjectileType("ChargedOrb"), Main.expertMode ? 20 : 36, 1f, player.whoAmI, 0f, 0f);
					}
				}
				if (npc.ai[1] == 161f)
				{
					npc.ai[0] += 1f;
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
				}
			}
			else if (npc.ai[0] <= 5f)
			{
				if (npc.ai[1] == 30f)
				{
					float num2 = (float)Main.rand.Next(180);
					if (npc.life > npc.lifeMax / 2)
					{
						NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OrbitingTurret"), 0, (float)player.whoAmI, num2 + 60f, 0f, 0f, 255);
						NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OrbitingTurret"), 0, (float)player.whoAmI, num2 + 120f, 0f, 0f, 255);
						NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OrbitingTurret"), 0, (float)player.whoAmI, num2 + 180f, 0f, 0f, 255);
					}
					else
					{
						NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OrbitingTurret"), 0, (float)player.whoAmI, num2 + 36f, 0f, 0f, 255);
						NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OrbitingTurret"), 0, (float)player.whoAmI, num2 + 72f, 0f, 0f, 255);
						NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OrbitingTurret"), 0, (float)player.whoAmI, num2 + 108f, 0f, 0f, 255);
						NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OrbitingTurret"), 0, (float)player.whoAmI, num2 + 144f, 0f, 0f, 255);
						NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OrbitingTurret"), 0, (float)player.whoAmI, num2 + 180f, 0f, 0f, 255);
					}
				}
				if (npc.ai[1] > 30f && NPC.CountNPCS(mod.NPCType("OrbitingTurret")) < 1)
				{
					npc.ai[0] += 1f;
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
				}
			}
			else if (npc.ai[0] <= 7f)
			{
				npc.velocity *= 0.88f;
				if (npc.ai[1] > 50f)
				{
					npc.velocity *= 0.3f;
					if (npc.ai[1] % 20f == 0f)
					{
						for (int l = 0; l < 3; l++)
						{
							Projectile.NewProjectile(npc.Center, Utils.RotatedBy(new Vector2(0f, -10f), (double)MathHelper.ToRadians((float)Main.rand.Next(360)), default(Vector2)), mod.ProjectileType("FallenAngleHomingMissile"), Main.expertMode ? 20 : 36, 1f, player.whoAmI, 0f, 0f);
						}
					}
				}
				if (npc.ai[1] > 110f)
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 0f;
				}
			}
			for (int m = 0; m < 200; m++)
			{
				if (Main.npc[m].type == mod.NPCType("FallenAngleForcefield") && (int)Main.npc[m].ai[0] == npc.whoAmI && Main.npc[m].active)
				{
					hasPersonalForcefield = true;
				}
			}
			if (hasPersonalForcefield)
			{
				npc.dontTakeDamage = true;
			}
			else
			{
				npc.dontTakeDamage = false;
				forcefield = false;
			}
			hasPersonalForcefield = false;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			npc.frameCounter += 1.0;
			if (npc.frameCounter >= 6.0)
			{
				npc.frame.Y = (npc.frame.Y / frameHeight + 1) % Main.npcFrameCount[npc.type] * frameHeight;
				npc.frameCounter = 0.0;
			}
		}

		private Vector2 tPos;

		private int despawn;

		private int turretTime;

		private int stationaryTurretTime;

		private int chargedOrbTime;

		private int forcefieldTime;

		private bool forcefield;

		private bool clonesSpawned;

		private NPC forcefieldNPC;

		private bool hasPersonalForcefield;
	}
}
