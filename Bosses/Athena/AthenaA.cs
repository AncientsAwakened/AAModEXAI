using System;
using System.IO;
using AAMod.Items.Boss.Athena;
using AAMod.NPCs.Enemies.Sky;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Athena
{
	// Token: 0x020004B1 RID: 1201
	[AutoloadBossHead]
	public class AthenaA : ModNPC
	{
		// Token: 0x06001CC4 RID: 7364 RVA: 0x0014EF04 File Offset: 0x0014D104
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Olympian Athena");
			Main.npcFrameCount[base.npc.type] = 7;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x0014EF28 File Offset: 0x0014D128
		public override void SetDefaults()
		{
			base.npc.width = 152;
			base.npc.height = 114;
			base.npc.value = (float)BaseUtility.CalcValue(0, 10, 0, 0, false);
			base.npc.npcSlots = 1000f;
			base.npc.aiStyle = -1;
			base.npc.lifeMax = 110000;
			base.npc.defense = 70;
			base.npc.damage = 110;
			base.npc.knockBackResist = 0f;
			base.npc.noGravity = true;
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = SoundID.NPCDeath1;
			base.npc.boss = true;
			this.music = base.mod.GetSoundSlot(51, "Sounds/Music/AthenaA");
			this.bossBag = ModContent.ItemType<AthenaBag>();
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x001351E9 File Offset: 0x001333E9
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			base.npc.lifeMax = (int)((float)base.npc.lifeMax * 0.6f * bossLifeScale);
			base.npc.damage = (int)((float)base.npc.damage * 0.6f);
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x0014F01C File Offset: 0x0014D21C
		public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);
			if (Main.netMode == 2 || Main.dedServ)
			{
				writer.Write(this.internalAI[0]);
				writer.Write(this.internalAI[1]);
				writer.Write(this.internalAI[2]);
				writer.Write(this.internalAI[3]);
				writer.Write(this.FlyAI[0]);
				writer.Write(this.FlyAI[1]);
			}
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x0014F094 File Offset: 0x0014D294
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);
			if (Main.netMode == 1)
			{
				this.internalAI[0] = BaseExtensions.ReadFloat(reader);
				this.internalAI[1] = BaseExtensions.ReadFloat(reader);
				this.internalAI[2] = BaseExtensions.ReadFloat(reader);
				this.internalAI[3] = BaseExtensions.ReadFloat(reader);
				this.FlyAI[0] = BaseExtensions.ReadFloat(reader);
				this.FlyAI[1] = BaseExtensions.ReadFloat(reader);
			}
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x0014F104 File Offset: 0x0014D304
		public override void AI()
		{
			base.npc.TargetClosest(true);
			Player player = Main.player[base.npc.target];
			AAPlayer modPlayer = player.GetModPlayer<AAPlayer>();
			Vector2 vector = new Vector2(this.Origin.X + 1216f, this.Origin.Y + 1152f);
			if (player.dead || !player.active || Vector2.Distance(base.npc.position, player.position) > 5000f || !modPlayer.ZoneAcropolis)
			{
				base.npc.TargetClosest(true);
				if (player.dead || !player.active || Math.Abs(Vector2.Distance(base.npc.position, player.position)) > 5000f || !modPlayer.ZoneAcropolis)
				{
					if (Main.netMode != 1)
					{
						BaseUtility.Chat(Lang.BossChat("AthenaA1"), Color.CornflowerBlue, true);
					}
					int num = NPC.NewNPC((int)base.npc.position.X, (int)base.npc.position.Y, ModContent.NPCType<AthenaFlee>(), 0, 0f, 0f, 0f, 0f, 255);
					Main.npc[num].Center = base.npc.Center;
					base.npc.active = false;
					base.npc.netUpdate = true;
				}
			}
			if (this.internalAI[0] == 0f && base.npc.life < base.npc.lifeMax / 3 && Main.netMode != 1)
			{
				AAModGlobalNPC.SpawnBoss(Main.player[base.npc.target], ModContent.NPCType<AthenaDark>(), false, base.npc.Center, "", false);
				AAModGlobalNPC.SpawnBoss(Main.player[base.npc.target], ModContent.NPCType<AthenaLight>(), false, base.npc.Center, "", false);
				this.internalAI[0] = 1f;
				base.npc.netUpdate = true;
			}
			float[] array = this.internalAI;
			int num2 = 2;
			float num3 = array[num2];
			array[num2] = num3 + 1f;
			if (num3 > 300f && Main.netMode != 1)
			{
				int num4 = Main.rand.Next(3);
				if (num4 == 0)
				{
					NPC.NewNPC((int)base.npc.position.X, (int)base.npc.position.Y, ModContent.NPCType<OwlRuneCharged>(), 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (num4 == 1)
				{
					int num5 = ModContent.ProjectileType<RazorGust>();
					double num6 = (double)0.522f;
					Vector2 vector2 = Vector2.Normalize(player.Center - base.npc.Center);
					vector2 *= 14f;
					float num7 = (float)Math.Sqrt((double)(vector2.X * vector2.X + vector2.Y * vector2.Y));
					double num8 = Math.Atan2((double)vector2.X, (double)vector2.Y) - 0.1;
					double num9 = num6 / (double)6f;
					for (int i = 0; i < 3; i++)
					{
						double num10 = num8 + num9 * (double)i;
						int num11 = Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, num7 * (float)Math.Sin(num10), num7 * (float)Math.Cos(num10), num5, this.damage, 2f, Main.myPlayer, 0f, 0f);
						Main.projectile[num11].tileCollide = false;
					}
				}
				else
				{
					for (int j = 0; j < 3; j++)
					{
						Projectile.NewProjectile(player.Center.X + (float)Main.rand.Next(-100, 100), player.Center.Y, 0f, 0f, ModContent.ProjectileType<Hurricane>(), this.damage, 12f, Main.myPlayer, 0f, 0f);
					}
				}
				this.internalAI[2] = 0f;
				base.npc.netUpdate = true;
			}
			if (this.internalAI[1] == 0f)
			{
				if (Main.netMode != 1)
				{
					base.npc.ai[3] += 1f;
				}
				if (Vector2.Distance(player.Center, vector) > 480f)
				{
					if (base.npc.ai[2] == 0f && Main.netMode != 1)
					{
						base.npc.ai[2] = 1f;
						base.npc.netUpdate = true;
					}
					this.MoveToVector2(vector);
				}
				else
				{
					if (base.npc.ai[2] == 1f && Main.netMode != 1)
					{
						base.npc.ai[2] = 0f;
						base.npc.netUpdate = true;
					}
					BaseAI.AISpaceOctopus(base.npc, ref this.FlyAI, Main.player[base.npc.target].Center, 0.1f, 8f, 220f, 70f, new Action<NPC, Vector2>(this.ShootFeather));
				}
				if (base.npc.ai[3] > 400f)
				{
					this.internalAI[1] = 1f;
					base.npc.ai[0] = 0f;
					base.npc.ai[1] = 0f;
					base.npc.ai[2] = 0f;
					base.npc.ai[3] = 0f;
					this.MoveVector2 = this.CloudPick();
				}
			}
			else
			{
				if (Main.netMode != 1)
				{
					base.npc.ai[1] += 1f;
					if (base.npc.ai[1] == 200f)
					{
						if (Main.rand.Next(5) == 0)
						{
							this.internalAI[1] = 0f;
							base.npc.ai[0] = 0f;
							base.npc.ai[1] = 0f;
							base.npc.ai[2] = 0f;
							base.npc.ai[3] = 0f;
							base.npc.netUpdate = true;
							return;
						}
						base.npc.ai[0] = 0f;
						this.MoveVector2 = this.CloudPick();
						base.npc.netUpdate = true;
					}
				}
				if (Vector2.Distance(base.npc.Center, this.MoveVector2) < 10f)
				{
					if (base.npc.ai[2] == 1f && Main.netMode != 1)
					{
						base.npc.ai[1] = 0f;
						base.npc.ai[2] = 0f;
						base.npc.netUpdate = true;
					}
					base.npc.velocity *= 0f;
					if (base.npc.ai[1] % 200f == 0f && Main.netMode != 1)
					{
						if (Main.rand.Next(2) == 0)
						{
							NPC.NewNPC((int)base.npc.Center.X + 100, (int)base.npc.Center.Y, ModContent.NPCType<OlympianDragon>(), 0, 0f, 0f, 0f, 0f, 255);
							NPC.NewNPC((int)base.npc.Center.X - 100, (int)base.npc.Center.Y, ModContent.NPCType<OlympianDragon>(), 0, 0f, 0f, 0f, 0f, 255);
						}
						else
						{
							NPC npc = Main.npc[NPC.NewNPC((int)base.npc.Center.X, (int)base.npc.Center.Y + 100, ModContent.NPCType<SeraphA>(), 0, 0f, 0f, 0f, 0f, 255)];
							for (int k = 0; k < 3; k++)
							{
								Dust dust = Main.dust[Dust.NewDust(npc.position, npc.height, npc.width, ModContent.DustType<Feather>(), (float)Main.rand.Next(-1, 2), 1f, 0, default(Color), 1f)];
							}
							NPC npc2 = Main.npc[NPC.NewNPC((int)base.npc.Center.X + 100, (int)base.npc.Center.Y - 50, ModContent.NPCType<SeraphA>(), 0, 0f, 0f, 0f, 0f, 255)];
							for (int l = 0; l < 3; l++)
							{
								Dust dust2 = Main.dust[Dust.NewDust(npc2.position, npc2.height, npc2.width, ModContent.DustType<Feather>(), (float)Main.rand.Next(-1, 2), 1f, 0, default(Color), 1f)];
							}
							NPC npc3 = Main.npc[NPC.NewNPC((int)base.npc.Center.X + 100, (int)base.npc.Center.Y - 50, ModContent.NPCType<SeraphA>(), 0, 0f, 0f, 0f, 0f, 255)];
							for (int m = 0; m < 3; m++)
							{
								Dust dust3 = Main.dust[Dust.NewDust(npc3.position, npc3.height, npc3.width, ModContent.DustType<Feather>(), (float)Main.rand.Next(-1, 2), 1f, 0, default(Color), 1f)];
							}
						}
						base.npc.netUpdate = true;
					}
					if (base.npc.ai[1] % 60f == 0f && Vector2.Distance(player.Center, base.npc.Center) < 900f)
					{
						this.ShootFeather(base.npc, base.npc.velocity);
					}
				}
				else
				{
					if (base.npc.ai[2] == 0f && Main.netMode != 1)
					{
						base.npc.ai[2] = 1f;
						base.npc.netUpdate = true;
					}
					this.MoveToVector2(this.MoveVector2);
				}
			}
			if (base.npc.ai[2] == 1f)
			{
				base.npc.noTileCollide = true;
			}
			else
			{
				base.npc.noTileCollide = false;
			}
			if (player.Center.X < base.npc.Center.X)
			{
				base.npc.direction = -1;
			}
			else
			{
				base.npc.direction = 1;
			}
			base.npc.rotation = 0f;
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x0014FC0C File Offset: 0x0014DE0C
		public Vector2 CloudPick()
		{
			int num = Main.rand.Next(12);
			Vector2 result = new Vector2(this.Origin.X + 1168f, this.Origin.Y + 128f);
			Vector2 result2 = new Vector2(this.Origin.X + 688f, this.Origin.Y + 304f);
			Vector2 result3 = new Vector2(this.Origin.X + 400f, this.Origin.Y + 624f);
			Vector2 result4 = new Vector2(this.Origin.X + 224f, this.Origin.Y + 976f);
			Vector2 result5 = new Vector2(this.Origin.X + 320f, this.Origin.Y + 1488f);
			Vector2 result6 = new Vector2(this.Origin.X + 720f, this.Origin.Y + 1824f);
			Vector2 result7 = new Vector2(this.Origin.X + 1168f, this.Origin.Y + 1952f);
			Vector2 result8 = new Vector2(this.Origin.X + 1760f, this.Origin.Y + 1792f);
			Vector2 result9 = new Vector2(this.Origin.X + 2048f, this.Origin.Y + 1472f);
			Vector2 result10 = new Vector2(this.Origin.X + 2160f, this.Origin.Y + 1008f);
			Vector2 result11 = new Vector2(this.Origin.X + 1952f, this.Origin.Y + 608f);
			Vector2 result12 = new Vector2(this.Origin.X + 1616f, this.Origin.Y + 288f);
			if (num == 1)
			{
				return result2;
			}
			if (num == 2)
			{
				return result3;
			}
			if (num == 3)
			{
				return result4;
			}
			if (num == 4)
			{
				return result5;
			}
			if (num == 5)
			{
				return result6;
			}
			if (num == 6)
			{
				return result7;
			}
			if (num == 7)
			{
				return result8;
			}
			if (num == 8)
			{
				return result9;
			}
			if (num == 9)
			{
				return result10;
			}
			if (num == 10)
			{
				return result11;
			}
			if (num == 11)
			{
				return result12;
			}
			return result;
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x0014FE64 File Offset: 0x0014E064
		public void ShootFeather(NPC npc, Vector2 velocity)
		{
			Player player = Main.player[npc.target];
			if (Main.rand.Next(2) == 0)
			{
				int num = ModContent.ProjectileType<SeraphFeather>();
				double num2 = (double)0.522f;
				Vector2 vector = Vector2.Normalize(player.Center - npc.Center);
				vector *= 14f;
				float num3 = (float)Math.Sqrt((double)(vector.X * vector.X + vector.Y * vector.Y));
				double num4 = Math.Atan2((double)vector.X, (double)vector.Y) - 0.1;
				double num5 = num2 / (double)6f;
				for (int i = 0; i < 3; i++)
				{
					double num6 = num4 + num5 * (double)i;
					int num7 = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, num3 * (float)Math.Sin(num6), num3 * (float)Math.Cos(num6), num, this.damage / 2, 2f, Main.myPlayer, 0f, 0f);
					Main.projectile[num7].tileCollide = false;
				}
				return;
			}
			BaseAI.FireProjectile(player.position, npc.position, ModContent.ProjectileType<AthenaMagic>(), this.damage / 2, 5f, 12f, -1, Main.myPlayer, default(Vector2));
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x0014FFB8 File Offset: 0x0014E1B8
		public override void FindFrame(int frameHeight)
		{
			base.npc.frameCounter += 1.0;
			if (base.npc.frameCounter >= 6.0)
			{
				NPC npc = base.npc;
				npc.frame.Y = npc.frame.Y + frameHeight;
				base.npc.frameCounter = 0.0;
			}
			if (base.npc.frame.Y >= frameHeight * 7)
			{
				base.npc.frame.Y = 0;
			}
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x00150048 File Offset: 0x0014E248
		public void MoveToVector2(Vector2 p)
		{
			float num = 25f;
			float scaleFactor = 1f;
			Vector2 vector = p - base.npc.Center;
			float num2 = (vector == Vector2.Zero) ? 0f : vector.Length();
			if (num2 < num)
			{
				scaleFactor = MathHelper.Lerp(0f, 1f, num2 / num);
			}
			if (num2 < 200f)
			{
				num *= 0.5f;
			}
			if (num2 < 100f)
			{
				num *= 0.5f;
			}
			if (num2 < 50f)
			{
				num *= 0.5f;
			}
			base.npc.velocity = ((num2 == 0f) ? Vector2.Zero : Vector2.Normalize(vector));
			base.npc.velocity *= num;
			base.npc.velocity *= scaleFactor;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x0014E6A1 File Offset: 0x0014C8A1
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = 499;
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x00150124 File Offset: 0x0014E324
		public override void NPCLoot()
		{
			if (!AAWorld.downedAthenaA)
			{
				int num = NPC.NewNPC((int)base.npc.position.X, (int)base.npc.position.Y, ModContent.NPCType<AthenaDefeat>(), 0, 0f, 0f, 1f, 0f, 255);
				Main.npc[num].Center = base.npc.Center;
			}
			else
			{
				if (Main.netMode != 1)
				{
					BaseUtility.Chat(Lang.BossChat("AthenaA2"), Color.CornflowerBlue, true);
				}
				int num2 = NPC.NewNPC((int)base.npc.position.X, (int)base.npc.position.Y, ModContent.NPCType<AthenaFlee>(), 0, 0f, 0f, 0f, 0f, 255);
				Main.npc[num2].Center = base.npc.Center;
			}
			if (Main.expertMode)
			{
				base.npc.DropBossBags();
			}
			else
			{
				base.npc.DropLoot(base.mod.ItemType("GoddessFeather"), Main.rand.Next(20, 25));
				string[] array = new string[]
				{
					"HurricaneStone",
					"Olympia",
					"Windfury",
					"GaleForce"
				};
				int num3 = Main.rand.Next(array.Length);
				base.npc.DropLoot(base.mod.ItemType(array[num3]), 1);
			}
			AAWorld.downedAthenaA = true;
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x001502A8 File Offset: 0x0014E4A8
		public override bool PreDraw(SpriteBatch sb, Color dColor)
		{
			Texture2D texture2D = Main.npcTexture[base.npc.type];
			Color lightColor = BaseDrawing.GetLightColor(base.npc.Center);
			BaseDrawing.DrawAfterimage(sb, texture2D, 0, base.npc.position, base.npc.width, base.npc.height, base.npc.oldPos, base.npc.scale, base.npc.rotation, base.npc.direction, 7, base.npc.frame, 1f, 1f, 5, false, 0f, 0f, new Color?(Color.CornflowerBlue));
			BaseDrawing.DrawTexture(sb, texture2D, 0, base.npc.position, base.npc.width, base.npc.height, base.npc.scale, base.npc.rotation, base.npc.direction, 7, base.npc.frame, new Color?(lightColor), false, default(Vector2));
			return false;
		}

		// Token: 0x0400059A RID: 1434
		public int damage;

		// Token: 0x0400059B RID: 1435
		public static Point CloudPoint = new Point((int)((float)Main.maxTilesX * 0.65f), 100);

		// Token: 0x0400059C RID: 1436
		public Vector2 Origin = new Vector2((float)((int)((float)Main.maxTilesX * 0.65f)), 100f) * 16f;

		// Token: 0x0400059D RID: 1437
		public float[] internalAI = new float[4];

		// Token: 0x0400059E RID: 1438
		public float[] FlyAI = new float[2];

		// Token: 0x0400059F RID: 1439
		public Vector2 MoveVector2;
	}
}
