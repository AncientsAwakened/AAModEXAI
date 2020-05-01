using System;
using AAMod.Items.Boss.Serpent;
using BaseMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Serpent
{
	// Token: 0x02000442 RID: 1090
	public class SerpentTail : ModNPC
	{
		// Token: 0x060019E3 RID: 6627 RVA: 0x0012735C File Offset: 0x0012555C
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Subzero Serpent");
			Main.npcFrameCount[base.npc.type] = 1;
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00127380 File Offset: 0x00125580
		public override void SetDefaults()
		{
			base.npc.npcSlots = 5f;
			base.npc.width = 38;
			base.npc.height = 38;
			base.npc.damage = 35;
			base.npc.defense = 25;
			base.npc.lifeMax = 6000;
			base.npc.value = (float)Item.buyPrice(0, 0, 0, 0);
			base.npc.knockBackResist = 0f;
			base.npc.aiStyle = -1;
			this.animationType = 10;
			base.npc.behindTiles = true;
			base.npc.noGravity = true;
			base.npc.noTileCollide = true;
			base.npc.HitSound = SoundID.NPCHit5;
			base.npc.DeathSound = SoundID.NPCDeath7;
			base.npc.netAlways = true;
			base.npc.boss = true;
			this.bossBag = ModContent.ItemType<SerpentBag>();
			this.music = base.mod.GetSoundSlot(51, "Sounds/Music/Boss6");
			base.npc.alpha = 50;
			base.npc.dontCountMe = true;
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x001274B4 File Offset: 0x001256B4
		public override void AI()
		{
			int num = (int)(base.npc.position.X / 16f) - 1;
			int num2 = (int)(base.npc.Center.X / 16f) + 2;
			int num3 = (int)(base.npc.position.Y / 16f) - 1;
			int num4 = (int)(base.npc.Center.Y / 16f) + 2;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 > Main.maxTilesX)
			{
				num2 = Main.maxTilesX;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesY)
			{
				num4 = Main.maxTilesY;
			}
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					Tile tileSafely = BaseWorldGen.GetTileSafely(i, j);
					if (tileSafely != null && ((tileSafely.nactive() && (Main.tileSolid[(int)tileSafely.type] || (Main.tileSolidTop[(int)tileSafely.type] && tileSafely.frameY == 0))) || tileSafely.liquid > 64))
					{
						Vector2 vector;
						vector.X = (float)(i * 16);
						vector.Y = (float)(j * 16);
						if (base.npc.position.X + (float)base.npc.width > vector.X && base.npc.position.X < vector.X + 16f && base.npc.position.Y + (float)base.npc.height > vector.Y && base.npc.position.Y < vector.Y + 16f && Main.rand.Next(100) == 0 && tileSafely.nactive())
						{
							WorldGen.KillTile(i, j, true, true, false);
						}
					}
				}
			}
			if (base.npc.ai[3] > 0f)
			{
				base.npc.realLife = (int)base.npc.ai[3];
			}
			if (base.npc.target < 0 || base.npc.target == 255 || Main.player[base.npc.target].dead)
			{
				base.npc.TargetClosest(true);
			}
			base.npc.velocity.Length();
			bool flag = false;
			if (base.npc.ai[1] <= 0f)
			{
				flag = true;
			}
			else if (Main.npc[(int)base.npc.ai[1]].life <= 0)
			{
				flag = true;
			}
			if (flag)
			{
				base.npc.life = 0;
				base.npc.HitEffect(0, 10.0);
				base.npc.checkDead();
			}
			int num5 = (int)(base.npc.position.X / 16f) - 1;
			int num6 = (int)((base.npc.position.X + (float)base.npc.width) / 16f) + 2;
			int num7 = (int)(base.npc.position.Y / 16f) - 1;
			int num8 = (int)((base.npc.position.Y + (float)base.npc.height) / 16f) + 2;
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.maxTilesX)
			{
				num6 = Main.maxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.maxTilesY)
			{
				num8 = Main.maxTilesY;
			}
			bool flag2 = false;
			if (!flag2)
			{
				for (int k = num5; k < num6; k++)
				{
					for (int l = num7; l < num8; l++)
					{
						if (Main.tile[k, l] != null && ((Main.tile[k, l].nactive() && (Main.tileSolid[(int)Main.tile[k, l].type] || (Main.tileSolidTop[(int)Main.tile[k, l].type] && Main.tile[k, l].frameY == 0))) || Main.tile[k, l].liquid > 64))
						{
							Vector2 vector2;
							vector2.X = (float)(k * 16);
							vector2.Y = (float)(l * 16);
							if (base.npc.position.X + (float)base.npc.width > vector2.X && base.npc.position.X < vector2.X + 16f && base.npc.position.Y + (float)base.npc.height > vector2.Y && base.npc.position.Y < vector2.Y + 16f)
							{
								flag2 = true;
								break;
							}
						}
					}
				}
			}
			if (!flag2)
			{
				base.npc.localAI[1] = 1f;
			}
			else
			{
				base.npc.localAI[1] = 0f;
			}
			float num9 = 16f;
			float num10 = 0.1f;
			float num11 = 0.15f;
			Vector2 vector3 = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
			float num12 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2);
			float num13 = Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2);
			num12 = (float)((int)(num12 / 16f) * 16);
			num13 = (float)((int)(num13 / 16f) * 16);
			vector3.X = (float)((int)(vector3.X / 16f) * 16);
			vector3.Y = (float)((int)(vector3.Y / 16f) * 16);
			num12 -= vector3.X;
			num13 -= vector3.Y;
			float num14 = (float)Math.Sqrt((double)(num12 * num12 + num13 * num13));
			if (base.npc.ai[1] > 0f && base.npc.ai[1] < (float)Main.npc.Length)
			{
				try
				{
					vector3 = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
					num12 = Main.npc[(int)base.npc.ai[1]].position.X + (float)(Main.npc[(int)base.npc.ai[1]].width / 2) - vector3.X;
					num13 = Main.npc[(int)base.npc.ai[1]].position.Y + (float)(Main.npc[(int)base.npc.ai[1]].height / 2) - vector3.Y;
				}
				catch
				{
				}
				base.npc.rotation = (float)Math.Atan2((double)num13, (double)num12) + 1.57f;
				num14 = (float)Math.Sqrt((double)(num12 * num12 + num13 * num13));
				int num15 = (int)(44f * base.npc.scale);
				num14 = (num14 - (float)num15) / num14;
				num12 *= num14;
				num13 *= num14;
				base.npc.velocity = Vector2.Zero;
				base.npc.position.X = base.npc.position.X + num12;
				base.npc.position.Y = base.npc.position.Y + num13;
				return;
			}
			if (!flag2)
			{
				base.npc.TargetClosest(true);
				base.npc.velocity.Y = base.npc.velocity.Y + 0.15f;
				if (base.npc.velocity.Y > num9)
				{
					base.npc.velocity.Y = num9;
				}
				if ((double)(Math.Abs(base.npc.velocity.X) + Math.Abs(base.npc.velocity.Y)) < (double)num9 * 0.4)
				{
					if (base.npc.velocity.X < 0f)
					{
						base.npc.velocity.X = base.npc.velocity.X - num10 * 1.1f;
					}
					else
					{
						base.npc.velocity.X = base.npc.velocity.X + num10 * 1.1f;
					}
				}
				else if (base.npc.velocity.Y == num9)
				{
					if (base.npc.velocity.X < num12)
					{
						base.npc.velocity.X = base.npc.velocity.X + num10;
					}
					else if (base.npc.velocity.X > num12)
					{
						base.npc.velocity.X = base.npc.velocity.X - num10;
					}
				}
				else if (base.npc.velocity.Y > 4f)
				{
					if (base.npc.velocity.X < 0f)
					{
						base.npc.velocity.X = base.npc.velocity.X + num10 * 0.9f;
					}
					else
					{
						base.npc.velocity.X = base.npc.velocity.X - num10 * 0.9f;
					}
				}
			}
			else
			{
				if (base.npc.soundDelay == 0)
				{
					float num16 = num14 / 40f;
					if (num16 < 10f)
					{
						num16 = 10f;
					}
					if (num16 > 20f)
					{
						num16 = 20f;
					}
					base.npc.soundDelay = (int)num16;
					Main.PlaySound(15, (int)base.npc.position.X, (int)base.npc.position.Y, 1, 1f, 0f);
				}
				num14 = (float)Math.Sqrt((double)(num12 * num12 + num13 * num13));
				float num17 = Math.Abs(num12);
				float num18 = Math.Abs(num13);
				float num19 = num9 / num14;
				num12 *= num19;
				num13 *= num19;
				if (((base.npc.velocity.X > 0f && num12 > 0f) || (base.npc.velocity.X < 0f && num12 < 0f)) && ((base.npc.velocity.Y > 0f && num13 > 0f) || (base.npc.velocity.Y < 0f && num13 < 0f)))
				{
					if (base.npc.velocity.X < num12)
					{
						base.npc.velocity.X = base.npc.velocity.X + num11;
					}
					else if (base.npc.velocity.X > num12)
					{
						base.npc.velocity.X = base.npc.velocity.X - num11;
					}
					if (base.npc.velocity.Y < num13)
					{
						base.npc.velocity.Y = base.npc.velocity.Y + num11;
					}
					else if (base.npc.velocity.Y > num13)
					{
						base.npc.velocity.Y = base.npc.velocity.Y - num11;
					}
				}
				if ((base.npc.velocity.X > 0f && num12 > 0f) || (base.npc.velocity.X < 0f && num12 < 0f) || (base.npc.velocity.Y > 0f && num13 > 0f) || (base.npc.velocity.Y < 0f && num13 < 0f))
				{
					if (base.npc.velocity.X < num12)
					{
						base.npc.velocity.X = base.npc.velocity.X + num10;
					}
					else if (base.npc.velocity.X > num12)
					{
						base.npc.velocity.X = base.npc.velocity.X - num10;
					}
					if (base.npc.velocity.Y < num13)
					{
						base.npc.velocity.Y = base.npc.velocity.Y + num10;
					}
					else if (base.npc.velocity.Y > num13)
					{
						base.npc.velocity.Y = base.npc.velocity.Y - num10;
					}
					if ((double)Math.Abs(num13) < (double)num9 * 0.2 && ((base.npc.velocity.X > 0f && num12 < 0f) || (base.npc.velocity.X < 0f && num12 > 0f)))
					{
						if (base.npc.velocity.Y > 0f)
						{
							base.npc.velocity.Y = base.npc.velocity.Y + num10 * 2f;
						}
						else
						{
							base.npc.velocity.Y = base.npc.velocity.Y - num10 * 2f;
						}
					}
					if ((double)Math.Abs(num12) < (double)num9 * 0.2 && ((base.npc.velocity.Y > 0f && num13 < 0f) || (base.npc.velocity.Y < 0f && num13 > 0f)))
					{
						if (base.npc.velocity.X > 0f)
						{
							base.npc.velocity.X = base.npc.velocity.X + num10 * 2f;
						}
						else
						{
							base.npc.velocity.X = base.npc.velocity.X - num10 * 2f;
						}
					}
				}
				else if (num17 > num18)
				{
					if (base.npc.velocity.X < num12)
					{
						base.npc.velocity.X = base.npc.velocity.X + num10 * 1.1f;
					}
					else if (base.npc.velocity.X > num12)
					{
						base.npc.velocity.X = base.npc.velocity.X - num10 * 1.1f;
					}
					if ((double)(Math.Abs(base.npc.velocity.X) + Math.Abs(base.npc.velocity.Y)) < (double)num9 * 0.5)
					{
						if (base.npc.velocity.Y > 0f)
						{
							base.npc.velocity.Y = base.npc.velocity.Y + num10;
						}
						else
						{
							base.npc.velocity.Y = base.npc.velocity.Y - num10;
						}
					}
				}
				else
				{
					if (base.npc.velocity.Y < num13)
					{
						base.npc.velocity.Y = base.npc.velocity.Y + num10 * 1.1f;
					}
					else if (base.npc.velocity.Y > num13)
					{
						base.npc.velocity.Y = base.npc.velocity.Y - num10 * 1.1f;
					}
					if ((double)(Math.Abs(base.npc.velocity.X) + Math.Abs(base.npc.velocity.Y)) < (double)num9 * 0.5)
					{
						if (base.npc.velocity.X > 0f)
						{
							base.npc.velocity.X = base.npc.velocity.X + num10;
						}
						else
						{
							base.npc.velocity.X = base.npc.velocity.X - num10;
						}
					}
				}
			}
			base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x0002F024 File Offset: 0x0002D224
		public override bool PreNPCLoot()
		{
			return false;
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x000C72B3 File Offset: 0x000C54B3
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return new bool?(false);
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x0012734B File Offset: 0x0012554B
		public override bool CheckActive()
		{
			return !NPC.AnyNPCs(ModContent.NPCType<SerpentHead>());
		}
	}
}
