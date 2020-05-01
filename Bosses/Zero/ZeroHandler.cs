using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AAMod.NPCs.Bosses.Zero
{
	// Token: 0x020003D5 RID: 981
	public class ZeroHandler : ModWorld
	{
		// Token: 0x06001679 RID: 5753 RVA: 0x000F7020 File Offset: 0x000F5220
		public override void Initialize()
		{
			ZeroHandler.ZX = -1;
			ZeroHandler.ZY = -1;
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x000F7030 File Offset: 0x000F5230
		public override TagCompound Save()
		{
			TagCompound tagCompound = new TagCompound();
			if (ZeroHandler.ZX != -1)
			{
				tagCompound.Add("ZX", ZeroHandler.ZX);
				tagCompound.Add("ZY", ZeroHandler.ZY);
			}
			return tagCompound;
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x000F7078 File Offset: 0x000F5278
		public override void Load(TagCompound tag)
		{
			this.Reset();
			if (tag.ContainsKey("ZX"))
			{
				ZeroHandler.ZX = tag.GetInt("ZX");
				ZeroHandler.ZY = tag.GetInt("ZY");
				if (!AAWorld.downedZero)
				{
					NPC.NewNPC(ZeroHandler.ZX, ZeroHandler.ZY, base.mod.NPCType("ZeroDeactivated"), 0, 0f, 0f, 0f, 0f, 255);
				}
			}
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x000F70F9 File Offset: 0x000F52F9
		public override void PostUpdate()
		{
			if (Main.netMode != 1 && !AAWorld.downedZero)
			{
				this.SpawnDeactivatedZero();
			}
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x000F7020 File Offset: 0x000F5220
		public void Reset()
		{
			ZeroHandler.ZX = -1;
			ZeroHandler.ZY = -1;
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x000F7110 File Offset: 0x000F5310
		public void SpawnDeactivatedZero()
		{
			int y = 140;
			Point point = new Point(Main.maxTilesX / 15 * 14 + Main.maxTilesX / 15 / 2 - 100, y);
			Vector2 vector = new Vector2((float)(point.X * 16), (float)(point.Y * 16));
			if (!NPC.AnyNPCs(base.mod.NPCType("ZeroDeactivated")) && !NPC.AnyNPCs(base.mod.NPCType("Zero")) && !NPC.AnyNPCs(base.mod.NPCType("ZeroProtocol")))
			{
				int num = NPC.NewNPC((int)vector.X, (int)vector.Y, ModContent.NPCType<ZeroDeactivated>(), 0, 0f, 0f, 0f, 0f, 255);
				ZeroHandler.ZX = (int)vector.X;
				ZeroHandler.ZY = (int)vector.Y;
				if (Main.netMode == 2 && num != -1 && num < 200)
				{
					NetMessage.SendData(23, -1, -1, null, num, 0f, 0f, 0f, 0, 0, 0);
				}
			}
		}

		// Token: 0x040003B1 RID: 945
		public static int ZX = -1;

		// Token: 0x040003B2 RID: 946
		public static int ZY = -1;

		// Token: 0x040003B3 RID: 947
		public static int Shield;
	}
}
