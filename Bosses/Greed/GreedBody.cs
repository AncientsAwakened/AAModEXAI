using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Greed
{
	// Token: 0x0200048D RID: 1165
	[AutoloadBossHead]
	public class GreedBody : Greed
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06001BB0 RID: 7088 RVA: 0x0013DD82 File Offset: 0x0013BF82
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Greed/GreedBody";
			}
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x0013DD89 File Offset: 0x0013BF89
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Greed");
			NPCID.Sets.TechnicallyABoss[base.npc.type] = true;
			Main.npcFrameCount[base.npc.type] = 22;
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x0013DDC0 File Offset: 0x0013BFC0
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.dontCountMe = true;
			base.npc.alpha = 255;
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x000C72B3 File Offset: 0x000C54B3
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return new bool?(false);
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x0002F024 File Offset: 0x0002D224
		public override bool PreNPCLoot()
		{
			return false;
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x0013DDE4 File Offset: 0x0013BFE4
		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			damage *= 0.05000000074505806;
			return true;
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x0013DDF8 File Offset: 0x0013BFF8
		public override bool PreAI()
		{
			base.npc.defense = this.Def();
			Vector2 vector = Main.npc[(int)base.npc.ai[1]].Center - base.npc.Center;
			base.npc.spriteDirection = ((vector.X > 0f) ? 1 : -1);
			if (base.npc.ai[3] > 0f)
			{
				base.npc.realLife = (int)base.npc.ai[3];
			}
			if (base.npc.target < 0 || base.npc.target == 255 || Main.player[base.npc.target].dead)
			{
				base.npc.TargetClosest(true);
			}
			if (Main.player[base.npc.target].dead && base.npc.timeLeft > 300)
			{
				base.npc.timeLeft = 300;
			}
			if (Main.netMode != 1 && (!Main.npc[(int)base.npc.ai[3]].active || Main.npc[(int)base.npc.ai[3]].type != base.mod.NPCType("Greed")))
			{
				base.npc.life = 0;
				base.npc.HitEffect(0, 10.0);
				base.npc.active = false;
				NetMessage.SendData(28, -1, -1, null, base.npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
			}
			if ((double)base.npc.ai[1] < (double)Main.npc.Length)
			{
				Vector2 vector2 = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
				float num = Main.npc[(int)base.npc.ai[1]].position.X + (float)(Main.npc[(int)base.npc.ai[1]].width / 2) - vector2.X;
				float num2 = Main.npc[(int)base.npc.ai[1]].position.Y + (float)(Main.npc[(int)base.npc.ai[1]].height / 2) - vector2.Y;
				base.npc.rotation = (float)Math.Atan2((double)num2, (double)num) + 1.57f;
				float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
				float num4 = (num3 - (float)base.npc.width) / num3;
				float num5 = num * num4;
				float num6 = num2 * num4;
				if (num < 0f)
				{
					base.npc.spriteDirection = 1;
				}
				else
				{
					base.npc.spriteDirection = -1;
				}
				base.npc.velocity = Vector2.Zero;
				base.npc.position.X = base.npc.position.X + num5;
				base.npc.position.Y = base.npc.position.Y + num6;
			}
			int num7 = (int)(base.npc.position.X / 16f) - 1;
			int num8 = (int)(base.npc.Center.X / 16f) + 2;
			int num9 = (int)(base.npc.position.Y / 16f) - 1;
			int num10 = (int)(base.npc.Center.Y / 16f) + 2;
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.maxTilesX)
			{
				num8 = Main.maxTilesX;
			}
			if (num9 < 0)
			{
				num9 = 0;
			}
			if (num10 > Main.maxTilesY)
			{
				num10 = Main.maxTilesY;
			}
			for (int i = num7; i < num8; i++)
			{
				for (int j = num9; j < num10; j++)
				{
					Tile tileSafely = BaseWorldGen.GetTileSafely(i, j);
					if (tileSafely != null && ((tileSafely.nactive() && (Main.tileSolid[(int)tileSafely.type] || (Main.tileSolidTop[(int)tileSafely.type] && tileSafely.frameY == 0))) || tileSafely.liquid > 64))
					{
						Vector2 vector3;
						vector3.X = (float)(i * 16);
						vector3.Y = (float)(j * 16);
						if (base.npc.position.X + (float)base.npc.width > vector3.X && base.npc.position.X < vector3.X + 16f && base.npc.position.Y + (float)base.npc.height > vector3.Y && base.npc.position.Y < vector3.Y + 16f && Main.rand.Next(100) == 0 && tileSafely.nactive())
						{
							WorldGen.KillTile(i, j, true, true, false);
						}
					}
				}
			}
			if (base.npc.target < 0 || base.npc.target == 255 || Main.player[base.npc.target].dead || !Main.player[base.npc.target].active)
			{
				base.npc.TargetClosest(true);
			}
			if (base.npc.alpha <= 0)
			{
				base.npc.alpha = 0;
				return false;
			}
			for (int k = 0; k < 4; k++)
			{
				int num11 = Dust.NewDust(new Vector2(base.npc.position.X, base.npc.position.Y), base.npc.width, base.npc.height, 246, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num11].noGravity = true;
				Main.dust[num11].noLight = true;
			}
			base.npc.alpha -= 3;
			return false;
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x000E0983 File Offset: 0x000DEB83
		public override void BossHeadRotation(ref float rotation)
		{
			rotation = base.npc.rotation;
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x0013E464 File Offset: 0x0013C664
		public override bool CheckActive()
		{
			if (NPC.AnyNPCs(ModContent.NPCType<Greed>()))
			{
				return false;
			}
			base.npc.active = false;
			return true;
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x0013E484 File Offset: 0x0013C684
		public int Def()
		{
			switch ((int)base.npc.ai[2])
			{
			case 0:
				return base.npc.defense = 6;
			case 1:
				return base.npc.defense = 7;
			case 2:
				return base.npc.defense = 9;
			case 3:
				return base.npc.defense = 11;
			case 4:
				return base.npc.defense = 13;
			case 5:
				return base.npc.defense = 15;
			case 6:
				return base.npc.defense = 16;
			case 7:
				return base.npc.defense = 20;
			case 8:
				return base.npc.defense = 19;
			case 9:
				return base.npc.defense = 19;
			case 10:
				return base.npc.defense = 15;
			case 11:
				return base.npc.defense = 21;
			case 12:
				return base.npc.defense = 25;
			case 13:
				return base.npc.defense = 26;
			case 14:
				return base.npc.defense = 32;
			case 15:
				return base.npc.defense = 37;
			case 16:
				return base.npc.defense = 42;
			case 17:
				return base.npc.defense = 50;
			case 18:
				return base.npc.defense = 49;
			case 19:
				return base.npc.defense = 50;
			case 20:
				return base.npc.defense = 56;
			default:
				return base.npc.defense = 30;
			}
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x0013E674 File Offset: 0x0013C874
		public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
		{
			Texture2D texture2D = Main.npcTexture[base.npc.type];
			Texture2D texture = base.mod.GetTexture("Glowmasks/GreedBody_Glow");
			NPC npc = base.npc;
			npc.position.Y = npc.position.Y + (float)base.npc.height * 0.5f;
			BaseDrawing.DrawTexture(spritebatch, texture2D, 0, base.npc, new Color?(dColor), false, default(Vector2));
			if (Main.LocalPlayer.findTreasure)
			{
				Color value = dColor;
				byte b = 200;
				byte b2 = 170;
				if (value.R < b)
				{
					value.R = b;
				}
				if (value.G < b2)
				{
					value.G = b2;
				}
				value.A = Main.mouseTextColor;
				BaseDrawing.DrawTexture(spritebatch, texture, 0, base.npc, new Color?(value), false, default(Vector2));
			}
			NPC npc2 = base.npc;
			npc2.position.Y = npc2.position.Y - (float)base.npc.height * 0.5f;
			return false;
		}
	}
}
