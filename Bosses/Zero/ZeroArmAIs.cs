using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace AAMod.NPCs.Bosses.Zero
{
	// Token: 0x020003D1 RID: 977
	public static class ZeroArmAIs
	{
		// Token: 0x0600165F RID: 5727 RVA: 0x000F4D8C File Offset: 0x000F2F8C
		public static void MeleeWeaponLeft(NPC npc)
		{
			Vector2 vector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector.X;
			float num2 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector.Y;
			float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			if (npc.ai[2] != 99f)
			{
				if (num3 > 800f)
				{
					npc.ai[2] = 99f;
				}
			}
			else if (num3 < 400f)
			{
				npc.ai[2] = 0f;
			}
			npc.spriteDirection = -(int)npc.ai[0];
			if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != 32)
			{
				npc.ai[2] += 10f;
				if (npc.ai[2] > 50f || Main.netMode != 2)
				{
					npc.life = -1;
					npc.HitEffect(0, 10.0);
					npc.active = false;
				}
			}
			if (npc.ai[2] == 99f)
			{
				if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y)
				{
					if (npc.velocity.Y > 0f)
					{
						npc.velocity.Y = npc.velocity.Y * 0.96f;
					}
					npc.velocity.Y = npc.velocity.Y - 0.1f;
					if (npc.velocity.Y > 8f)
					{
						npc.velocity.Y = 8f;
					}
				}
				else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y)
				{
					if (npc.velocity.Y < 0f)
					{
						npc.velocity.Y = npc.velocity.Y * 0.96f;
					}
					npc.velocity.Y = npc.velocity.Y + 0.1f;
					if (npc.velocity.Y < -8f)
					{
						npc.velocity.Y = -8f;
					}
				}
				if (npc.position.X + (float)(npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2))
				{
					if (npc.velocity.X > 0f)
					{
						npc.velocity.X = npc.velocity.X * 0.96f;
					}
					npc.velocity.X = npc.velocity.X - 0.5f;
					if (npc.velocity.X > 12f)
					{
						npc.velocity.X = 12f;
					}
				}
				if (npc.position.X + (float)(npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2))
				{
					if (npc.velocity.X < 0f)
					{
						npc.velocity.X = npc.velocity.X * 0.96f;
					}
					npc.velocity.X = npc.velocity.X + 0.5f;
					if (npc.velocity.X < -12f)
					{
						npc.velocity.X = -12f;
						return;
					}
				}
			}
			else
			{
				if (npc.ai[2] == 0f || npc.ai[2] == 3f)
				{
					if (Main.npc[(int)npc.ai[1]].ai[1] == 3f && npc.timeLeft > 10)
					{
						npc.timeLeft = 10;
					}
					if (Main.npc[(int)npc.ai[1]].ai[1] != 0f)
					{
						npc.TargetClosest(true);
						if (Main.player[npc.target].dead)
						{
							npc.velocity.Y = npc.velocity.Y + 0.1f;
							if (npc.velocity.Y > 16f)
							{
								npc.velocity.Y = 16f;
							}
						}
						else
						{
							float num4 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector.X;
							float num5 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector.Y;
							float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
							num6 = 7f / num6;
							num4 *= num6;
							num5 *= num6;
							npc.rotation = (float)Math.Atan2((double)num5, (double)num4) - 1.57f;
							if (npc.velocity.X > num4)
							{
								if (npc.velocity.X > 0f)
								{
									npc.velocity.X = npc.velocity.X * 0.97f;
								}
								npc.velocity.X = npc.velocity.X - 0.05f;
							}
							if (npc.velocity.X < num4)
							{
								if (npc.velocity.X < 0f)
								{
									npc.velocity.X = npc.velocity.X * 0.97f;
								}
								npc.velocity.X = npc.velocity.X + 0.05f;
							}
							if (npc.velocity.Y > num5)
							{
								if (npc.velocity.Y > 0f)
								{
									npc.velocity.Y = npc.velocity.Y * 0.97f;
								}
								npc.velocity.Y = npc.velocity.Y - 0.05f;
							}
							if (npc.velocity.Y < num5)
							{
								if (npc.velocity.Y < 0f)
								{
									npc.velocity.Y = npc.velocity.Y * 0.97f;
								}
								npc.velocity.Y = npc.velocity.Y + 0.05f;
							}
						}
						npc.ai[3] += 1f;
						if (npc.ai[3] >= 600f)
						{
							npc.ai[2] = 0f;
							npc.ai[3] = 0f;
							npc.netUpdate = true;
						}
					}
					else
					{
						npc.ai[3] += 1f;
						if (npc.ai[3] >= 300f)
						{
							npc.ai[2] += 1f;
							npc.ai[3] = 0f;
							npc.netUpdate = true;
						}
						if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 320f)
						{
							if (npc.velocity.Y > 0f)
							{
								npc.velocity.Y = npc.velocity.Y * 0.96f;
							}
							npc.velocity.Y = npc.velocity.Y - 0.04f;
							if (npc.velocity.Y > 3f)
							{
								npc.velocity.Y = 3f;
							}
						}
						else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 260f)
						{
							if (npc.velocity.Y < 0f)
							{
								npc.velocity.Y = npc.velocity.Y * 0.96f;
							}
							npc.velocity.Y = npc.velocity.Y + 0.04f;
							if (npc.velocity.Y < -3f)
							{
								npc.velocity.Y = -3f;
							}
						}
						if (npc.position.X + (float)(npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2))
						{
							if (npc.velocity.X > 0f)
							{
								npc.velocity.X = npc.velocity.X * 0.96f;
							}
							npc.velocity.X = npc.velocity.X - 0.3f;
							if (npc.velocity.X > 12f)
							{
								npc.velocity.X = 12f;
							}
						}
						if (npc.position.X + (float)(npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 250f)
						{
							if (npc.velocity.X < 0f)
							{
								npc.velocity.X = npc.velocity.X * 0.96f;
							}
							npc.velocity.X = npc.velocity.X + 0.3f;
							if (npc.velocity.X < -12f)
							{
								npc.velocity.X = -12f;
							}
						}
					}
					Vector2 vector2 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					float num7 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector2.X;
					float num8 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector2.Y;
					Math.Sqrt((double)(num7 * num7 + num8 * num8));
					npc.rotation = (float)Math.Atan2((double)num8, (double)num7) + 1.57f;
					return;
				}
				if (npc.ai[2] == 1f)
				{
					Vector2 vector3 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					float num9 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector3.X;
					float num10 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector3.Y;
					npc.rotation = (float)Math.Atan2((double)num10, (double)num9) + 1.57f;
					npc.velocity.X = npc.velocity.X * 0.95f;
					npc.velocity.Y = npc.velocity.Y - 0.1f;
					if (npc.velocity.Y < -8f)
					{
						npc.velocity.Y = -8f;
					}
					if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 200f)
					{
						npc.TargetClosest(true);
						npc.ai[2] = 2f;
						vector3 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
						num9 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector3.X;
						num10 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector3.Y;
						float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
						num11 = 22f / num11;
						npc.velocity.X = num9 * num11;
						npc.velocity.Y = num10 * num11;
						npc.netUpdate = true;
						return;
					}
				}
				else if (npc.ai[2] == 2f)
				{
					if (npc.position.Y > Main.player[npc.target].position.Y || npc.velocity.Y < 0f)
					{
						npc.ai[2] = 3f;
						return;
					}
				}
				else
				{
					if (npc.ai[2] == 4f)
					{
						npc.TargetClosest(true);
						Vector2 vector4 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
						float num12 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector4.X;
						float num13 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector4.Y;
						float num14 = (float)Math.Sqrt((double)(num12 * num12 + num13 * num13));
						num14 = 7f / num14;
						num12 *= num14;
						num13 *= num14;
						if (npc.velocity.X > num12)
						{
							if (npc.velocity.X > 0f)
							{
								npc.velocity.X = npc.velocity.X * 0.97f;
							}
							npc.velocity.X = npc.velocity.X - 0.05f;
						}
						if (npc.velocity.X < num12)
						{
							if (npc.velocity.X < 0f)
							{
								npc.velocity.X = npc.velocity.X * 0.97f;
							}
							npc.velocity.X = npc.velocity.X + 0.05f;
						}
						if (npc.velocity.Y > num13)
						{
							if (npc.velocity.Y > 0f)
							{
								npc.velocity.Y = npc.velocity.Y * 0.97f;
							}
							npc.velocity.Y = npc.velocity.Y - 0.05f;
						}
						if (npc.velocity.Y < num13)
						{
							if (npc.velocity.Y < 0f)
							{
								npc.velocity.Y = npc.velocity.Y * 0.97f;
							}
							npc.velocity.Y = npc.velocity.Y + 0.05f;
						}
						npc.ai[3] += 1f;
						if (npc.ai[3] >= 600f)
						{
							npc.ai[2] = 0f;
							npc.ai[3] = 0f;
							npc.netUpdate = true;
						}
						vector4 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
						num12 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector4.X;
						num13 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector4.Y;
						npc.rotation = (float)Math.Atan2((double)num13, (double)num12) + 1.57f;
						return;
					}
					if (npc.ai[2] == 5f && ((npc.velocity.X > 0f && npc.position.X + (float)(npc.width / 2) > Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2)) || (npc.velocity.X < 0f && npc.position.X + (float)(npc.width / 2) < Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))))
					{
						npc.ai[2] = 0f;
						return;
					}
				}
			}
		}
	}
}
