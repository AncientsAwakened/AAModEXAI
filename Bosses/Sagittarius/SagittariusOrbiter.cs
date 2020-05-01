using System;
using System.IO;
using AAMod.Items.Materials;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Sagittarius
{
	// Token: 0x02000446 RID: 1094
	public class SagittariusOrbiter : ModNPC
	{
		// Token: 0x06001A04 RID: 6660 RVA: 0x0012A98B File Offset: 0x00128B8B
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Orbiter");
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0012A9A0 File Offset: 0x00128BA0
		public override void SetDefaults()
		{
			base.npc.width = 30;
			base.npc.height = 30;
			base.npc.value = (float)BaseUtility.CalcValue(0, 0, 0, 0, false);
			base.npc.npcSlots = 1f;
			base.npc.aiStyle = -1;
			base.npc.lifeMax = 300;
			base.npc.defense = 0;
			base.npc.damage = 20;
			base.npc.HitSound = SoundID.NPCHit4;
			base.npc.DeathSound = SoundID.NPCDeath14;
			base.npc.knockBackResist = 0f;
			base.npc.noTileCollide = true;
			base.npc.defense = 15;
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0012AA6C File Offset: 0x00128C6C
		public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);
			if (Main.netMode == 2 || Main.dedServ)
			{
				writer.Write(this.InternalAI[0]);
				writer.Write(this.InternalAI[1]);
				writer.Write(this.InternalAI[2]);
				writer.Write(this.InternalAI[3]);
			}
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x0012AAC8 File Offset: 0x00128CC8
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);
			if (Main.netMode == 1)
			{
				this.InternalAI[0] = BaseExtensions.ReadFloat(reader);
				this.InternalAI[1] = BaseExtensions.ReadFloat(reader);
				this.InternalAI[2] = BaseExtensions.ReadFloat(reader);
				this.InternalAI[3] = BaseExtensions.ReadFloat(reader);
			}
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x0012AB1C File Offset: 0x00128D1C
		public override void AI()
		{
			if (Main.expertMode)
			{
				this.damage = base.npc.damage / 4;
			}
			else
			{
				this.damage = base.npc.damage / 2;
			}
			if (!NPC.AnyNPCs(ModContent.NPCType<Sagittarius>()))
			{
				base.npc.life = 0;
			}
			base.npc.noGravity = true;
			if (base.npc.alpha > 0)
			{
				base.npc.dontTakeDamage = true;
				base.npc.chaseable = false;
			}
			else
			{
				base.npc.dontTakeDamage = false;
				base.npc.chaseable = true;
			}
			if (this.body == -1)
			{
				int npc = BaseAI.GetNPC(base.npc.Center, base.mod.NPCType("Sagittarius"), 400f, null);
				if (npc >= 0)
				{
					this.body = npc;
				}
			}
			if (this.body == -1)
			{
				return;
			}
			NPC npc2 = Main.npc[this.body];
			if (npc2 == null || npc2.life <= 0 || !npc2.active || npc2.type != base.mod.NPCType("Sagittarius"))
			{
				BaseAI.KillNPCWithLoot(base.npc);
				return;
			}
			Player player = Main.player[npc2.target];
			this.pos = npc2.Center;
			for (int i = base.npc.oldPos.Length - 1; i > 0; i--)
			{
				base.npc.oldPos[i] = base.npc.oldPos[i - 1];
			}
			base.npc.oldPos[0] = base.npc.position;
			int probeCount = ((Sagittarius)npc2.modNPC).ProbeCount;
			if (this.rotValue == -1f)
			{
				this.rotValue = base.npc.ai[0] % (float)probeCount * (6.2831855f / (float)probeCount);
			}
			this.rotValue += 0.04f;
			while (this.rotValue > 6.2831855f)
			{
				this.rotValue -= 6.2831855f;
			}
			base.npc.alpha = (int)Sagittarius.MovementType[1];
			int num = 0;
			this.shootAI[0] += 1f;
			if (base.npc.ai[0] == 1f || base.npc.ai[0] == 4f || base.npc.ai[0] == 7f || base.npc.ai[0] == 10f)
			{
				num = 50;
			}
			if (base.npc.ai[0] == 2f || base.npc.ai[0] == 5f || base.npc.ai[0] == 8f || base.npc.ai[0] == 11f)
			{
				num = 100;
			}
			if (base.npc.ai[0] == 3f || base.npc.ai[0] == 6f || base.npc.ai[0] == 9f || base.npc.ai[0] == 12f)
			{
				num = 150;
			}
			if (this.shootAI[0] >= 150f)
			{
				this.shootAI[0] = 0f;
			}
			if (Sagittarius.MovementType[0] == 0f)
			{
				for (int j = base.npc.oldPos.Length - 1; j > 0; j--)
				{
					base.npc.oldPos[j] = base.npc.oldPos[j - 1];
				}
				base.npc.oldPos[0] = base.npc.position;
				base.npc.Center = BaseUtility.RotateVector(npc2.Center, npc2.Center + new Vector2(140f, 0f), this.rotValue);
				if (this.shootAI[0] == (float)num && Collision.CanHit(base.npc.position, base.npc.width, base.npc.height, player.Center, player.width, player.height))
				{
					Vector2 vector = base.npc.Center;
					float num2 = BaseUtility.RotationTo(base.npc.Center, player.Center);
					vector = BaseUtility.RotateVector(base.npc.Center, vector, num2);
					BaseAI.FireProjectile(player.Center, vector, BaseExtensions.ProjType(base.mod, "SagProj"), this.damage, 0f, 4f, 0, -1, default(Vector2));
				}
			}
			if (Sagittarius.MovementType[0] == 1f)
			{
				BaseAI.AIEye(base.npc, ref base.npc.ai, false, true, 0.2f, 0.1f, 6f, 6f, 0f, 0f);
			}
			else if (Sagittarius.MovementType[0] == 2f)
			{
				for (int k = base.npc.oldPos.Length - 1; k > 0; k--)
				{
					base.npc.oldPos[k] = base.npc.oldPos[k - 1];
				}
				base.npc.oldPos[0] = base.npc.position;
				base.npc.Center = BaseUtility.RotateVector(npc2.Center, npc2.Center + new Vector2(140f, 0f), this.rotValue);
			}
			else if (Sagittarius.MovementType[0] == 3f)
			{
				base.npc.Center = BaseUtility.RotateVector(player.Center, player.Center + new Vector2(260f, 0f), this.rotValue);
				if (this.shootAI[0] == (float)num && Collision.CanHit(base.npc.position, base.npc.width, base.npc.height, player.position, player.width, player.height))
				{
					Vector2 vector2 = base.npc.Center;
					float num3 = BaseUtility.RotationTo(base.npc.Center, player.Center);
					vector2 = BaseUtility.RotateVector(base.npc.Center, vector2, num3);
					BaseAI.FireProjectile(player.Center, vector2, BaseExtensions.ProjType(base.mod, "SagProj"), this.damage, 0f, 4f, 0, -1, default(Vector2));
				}
			}
			else if (Sagittarius.MovementType[0] == 4f)
			{
				for (int l = base.npc.oldPos.Length - 1; l > 0; l--)
				{
					base.npc.oldPos[l] = base.npc.oldPos[l - 1];
				}
				base.npc.oldPos[0] = base.npc.position;
				base.npc.Center = BaseUtility.RotateVector(npc2.Center, npc2.Center + new Vector2(140f, 0f), this.rotValue);
			}
			else if (Sagittarius.MovementType[0] == 5f)
			{
				base.npc.velocity *= 0.8f;
			}
			if (Sagittarius.MovementType[0] != 1f)
			{
				base.npc.rotation = 0f;
			}
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x0012B2CC File Offset: 0x001294CC
		public void MoveToPoint(Vector2 point)
		{
			float num = 8f;
			float scaleFactor = 1f;
			Vector2 vector = point - base.npc.Center;
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

		// Token: 0x06001A0A RID: 6666 RVA: 0x0012B3A8 File Offset: 0x001295A8
		public override void NPCLoot()
		{
			if (NPC.AnyNPCs(ModContent.NPCType<Sagittarius>()))
			{
				Item.NewItem(base.npc.Center, ModContent.ItemType<Doomite>(), Main.rand.Next(2), false, 0, false, false);
			}
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x0012B3DC File Offset: 0x001295DC
		public override bool PreDraw(SpriteBatch sb, Color dColor)
		{
			BaseDrawing.DrawTexture(sb, Main.npcTexture[base.npc.type], 0, base.npc, new Color?(base.npc.GetAlpha(dColor)), false, default(Vector2));
			BaseDrawing.DrawTexture(sb, base.mod.GetTexture("Glowmasks/SagittariusOrbiter_Glow"), 0, base.npc, new Color?(base.npc.GetAlpha(ColorUtils.COLOR_GLOWPULSE)), false, default(Vector2));
			return false;
		}

		// Token: 0x04000510 RID: 1296
		public int damage;

		// Token: 0x04000511 RID: 1297
		public int body = -1;

		// Token: 0x04000512 RID: 1298
		public float rotValue = -1f;

		// Token: 0x04000513 RID: 1299
		public Vector2 pos;

		// Token: 0x04000514 RID: 1300
		public float[] shootAI = new float[1];

		// Token: 0x04000515 RID: 1301
		public float[] InternalAI = new float[4];
	}
}
