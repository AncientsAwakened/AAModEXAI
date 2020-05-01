using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Athena
{
	// Token: 0x020004B0 RID: 1200
	public class AthenaFlee : ModNPC
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x0014EB22 File Offset: 0x0014CD22
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Athena/Athena";
			}
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x0014EB29 File Offset: 0x0014CD29
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Athena");
			Main.npcFrameCount[base.npc.type] = 7;
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x0014EB50 File Offset: 0x0014CD50
		public override void SetDefaults()
		{
			base.npc.width = 152;
			base.npc.height = 114;
			base.npc.npcSlots = 1000f;
			base.npc.aiStyle = -1;
			base.npc.defense = 1;
			base.npc.knockBackResist = 0f;
			base.npc.noGravity = true;
			base.npc.lifeMax = 1;
			base.npc.dontTakeDamage = true;
			base.npc.noTileCollide = true;
			base.npc.damage = 0;
			base.npc.value = 0f;
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x0014EC00 File Offset: 0x0014CE00
		public override void AI()
		{
			if (Main.netMode != 1)
			{
				float[] ai = base.npc.ai;
				int num = 0;
				float num2 = ai[num];
				ai[num] = num2 + 1f;
				if (num2 >= 120f)
				{
					if (base.npc.ai[0] >= 120f && base.npc.ai[0] < 130f)
					{
						NPC npc = base.npc;
						npc.velocity.Y = npc.velocity.Y + 1f;
						base.npc.netUpdate = true;
					}
					else if (base.npc.ai[0] == 130f)
					{
						base.npc.velocity.Y = -6f;
						base.npc.netUpdate = true;
					}
					else if (base.npc.ai[0] > 130f)
					{
						base.npc.velocity.Y = -6f;
					}
					if (base.npc.position.Y + base.npc.velocity.Y <= 0f && Main.netMode != 1)
					{
						BaseAI.KillNPC(base.npc);
						base.npc.netUpdate = true;
					}
				}
			}
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x0014ED38 File Offset: 0x0014CF38
		public override void FindFrame(int frameHeight)
		{
			base.npc.frameCounter += 1.0;
			if (base.npc.frameCounter >= 6.0)
			{
				base.npc.frame.Y = base.npc.frame.Y + frameHeight;
				base.npc.frameCounter = 0.0;
			}
			if (base.npc.frame.Y >= frameHeight * Main.npcFrameCount[base.npc.type])
			{
				base.npc.frame.Y = 0;
			}
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x0014EDE4 File Offset: 0x0014CFE4
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			BaseDrawing.DrawAfterimage(spriteBatch, Main.npcTexture[base.npc.type], 0, base.npc.position, base.npc.width, base.npc.height, base.npc.oldPos, base.npc.scale, base.npc.rotation, base.npc.direction, 7, base.npc.frame, 1f, 1f, 5, false, 0f, 0f, new Color?(Color.CornflowerBlue));
			BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[base.npc.type], 0, base.npc.position, base.npc.width, base.npc.height, base.npc.scale, base.npc.rotation, base.npc.direction, 7, base.npc.frame, new Color?(base.npc.GetAlpha(lightColor)), false, default(Vector2));
			return false;
		}
	}
}
