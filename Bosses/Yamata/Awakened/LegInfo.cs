using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Yamata.Awakened
{
	// Token: 0x02000400 RID: 1024
	public class LegInfo : LimbInfo
	{
		// Token: 0x060017BE RID: 6078 RVA: 0x00107078 File Offset: 0x00105278
		public LegInfo(int lType, Vector2 initialPos, YamataA m)
		{
			this.yamataA = m;
			this.position = initialPos;
			this.pointToStandOn = this.position;
			this.limbType = lType;
			this.Hitbox = new Rectangle(0, 0, 140, 76);
			this.legOrigin = new Vector2((float)((this.limbType == 1 || this.limbType == 3) ? (this.Hitbox.Width - 12) : 12), 12f);
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0010710C File Offset: 0x0010530C
		public void MoveLegFlying(NPC npc)
		{
			Vector2 vector = this.GetBodyConnector(npc) + new Vector2((this.limbType == 3) ? (-35f - (float)this.Hitbox.Width) : ((this.limbType == 2) ? 35f : ((this.limbType == 1) ? (-15f - (float)this.Hitbox.Width) : 15f)), (this.limbType == 1 || this.limbType == 0) ? 40f : 50f);
			float num = (npc.position - npc.oldPos[1]).Length();
			if (num > 8f)
			{
				this.position = vector;
				this.velocity = default(Vector2);
				return;
			}
			if (Vector2.Distance(vector, this.position) > (float)(40 + (int)npc.velocity.Length()))
			{
				Vector2 vector2 = vector - this.position;
				vector2.Normalize();
				vector2 *= 2f + num * 0.25f;
				this.velocity += vector2;
				float num2 = 4f + num;
				if (this.velocity.Length() > num2)
				{
					this.velocity.Normalize();
					this.velocity *= num2;
				}
				this.position += this.velocity;
				return;
			}
			this.position = vector;
			this.velocity = default(Vector2);
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x00107290 File Offset: 0x00105490
		public void UpdateVelOffsetY()
		{
			this.movementRatio += 0.04f;
			this.movementRatio = Math.Max(0f, Math.Min(1f, this.movementRatio));
			float movementRatio = this.movementRatio;
			float[] array = new float[3];
			array[1] = 30f;
			this.velOffsetY = BaseUtility.MultiLerp(movementRatio, array);
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x001072F0 File Offset: 0x001054F0
		public void MoveLegWalking(NPC npc, Vector2 standOnPoint)
		{
			this.UpdateVelOffsetY();
			if (this.pointToStandOn != default(Vector2))
			{
				Vector2 vector = this.pointToStandOn - this.position;
				vector.Normalize();
				vector *= 1.6f + npc.velocity.Length() * 0.5f;
				this.velocity += vector;
				float num = 4f + npc.velocity.Length();
				if (this.velocity.Length() > num)
				{
					this.velocity.Normalize();
					this.velocity *= num;
				}
				if (Vector2.Distance(this.pointToStandOn, this.position) <= 15f)
				{
					this.position = this.pointToStandOn;
					this.velocity = default(Vector2);
				}
				this.position += this.velocity;
				if (this.position == this.pointToStandOn || Vector2.Distance(standOnPoint, this.position + new Vector2((float)this.Hitbox.Width * 0.5f, 0f)) > this.distanceToMove || Math.Abs(this.position.X - standOnPoint.X) > this.distanceToMoveX)
				{
					this.pointToStandOn = default(Vector2);
				}
			}
			if (this.pointToStandOn == default(Vector2) && (Vector2.Distance(standOnPoint, this.position + new Vector2((float)this.Hitbox.Width * 0.5f, 0f)) > this.distanceToMove || Math.Abs(this.position.X - standOnPoint.X) > this.distanceToMoveX))
			{
				this.movementRatio = 0f;
				this.pointToStandOn = standOnPoint;
			}
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x001074D8 File Offset: 0x001056D8
		public void UpdateLeg(NPC npc)
		{
			this.leftLeg = (this.limbType == 1 || this.limbType == 3);
			if (Vector2.Distance(base.Center, npc.Center) > 499f || YamataA.TeleportMeBitch)
			{
				this.position = npc.Center;
			}
			if (this.overrideAnimation != null)
			{
				if (this.overrideAnimation.movementRatio >= 1f)
				{
					this.overrideAnimation = null;
				}
			}
			else
			{
				this.rotation = 0f;
				Vector2 standOnPoint = this.GetStandOnPoint(npc);
				if (standOnPoint == default(Vector2))
				{
					this.MoveLegFlying(npc);
				}
				else
				{
					this.MoveLegWalking(npc, standOnPoint);
				}
			}
			Vector2 bodyConnector = this.GetBodyConnector(npc);
			this.legJoint = Vector2.Lerp(this.position, bodyConnector, 0.3f) + new Vector2(this.leftLeg ? 30f : 0f, -30f);
			this.oldPosition = this.position;
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x001075D4 File Offset: 0x001057D4
		public Vector2 GetStandOnPoint(NPC npc)
		{
			float num = npc.velocity.Length();
			float num2 = 150f + 0.5f * num;
			float num3 = 120f + 0.5f * num;
			float num4 = npc.Center.X + this.yamataA.topVisualOffset.X + ((this.limbType == 3) ? (-num2 - (float)this.Hitbox.Width) : ((this.limbType == 2) ? (num2 + (float)this.Hitbox.Width) : ((this.limbType == 1) ? (-num3 - (float)this.Hitbox.Width) : (num3 + (float)this.Hitbox.Width))));
			int num5 = (int)(npc.Bottom.Y / 16f);
			int num6 = BaseWorldGen.GetFirstTileFloor((int)(num4 / 16f), (int)(npc.Bottom.Y / 16f), true);
			if (num6 - num5 > YamataA.flyingTileCount)
			{
				return default(Vector2);
			}
			if (!this.flying)
			{
				num6 = (int)((float)num6 * 16f) / 16;
				float num7 = (float)num6 * 16f;
				if (Main.tile[(int)(num4 / 16f), num6] == null || !Main.tile[(int)(num4 / 16f), num6].nactive() || !Main.tileSolid[(int)Main.tile[(int)(num4 / 16f), num6].type])
				{
					num7 += 16f;
				}
				return new Vector2(num4 - (float)this.Hitbox.Width * 0.5f, num7 - (float)this.Hitbox.Height);
			}
			return default(Vector2);
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00107784 File Offset: 0x00105984
		public Vector2 GetBodyConnector(NPC npc)
		{
			return npc.Center + this.yamataA.topVisualOffset + new Vector2((this.limbType == 3 || this.limbType == 1) ? -40f : 40f, 0f);
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x001077D4 File Offset: 0x001059D4
		public void DrawLeg(SpriteBatch sb, NPC npc)
		{
			Mod instance = AAMod.instance;
			if (this.textures == null)
			{
				string str = "NPCs/Bosses/Yamata/Awakened/YamataA";
				this.textures = new Texture2D[5];
				this.textures[0] = instance.GetTexture(str + "LegCap");
				this.textures[1] = instance.GetTexture(str + "LegSegment");
				this.textures[2] = instance.GetTexture(str + "LegCapR");
				this.textures[3] = instance.GetTexture(str + "LegSegmentR");
				this.textures[4] = instance.GetTexture(str + "Foot");
			}
			Vector2 vector = this.position - new Vector2(0f, this.velOffsetY);
			Color alpha = npc.GetAlpha(BaseDrawing.GetLightColor(base.Center));
			if (!this.leftLeg)
			{
				Texture2D[] array = new Texture2D[3];
				array[1] = this.textures[3];
				BaseDrawing.DrawChain(sb, array, 0, vector + new Vector2((float)this.Hitbox.Width * 0.5f, 6f), this.legJoint, 0f, null, 1f, false, null);
				BaseDrawing.DrawChain(sb, new Texture2D[]
				{
					this.textures[2],
					this.textures[3],
					this.textures[2]
				}, 0, this.legJoint, this.GetBodyConnector(npc), 0f, null, 1f, false, null);
			}
			else
			{
				Texture2D[] array2 = new Texture2D[3];
				array2[1] = this.textures[1];
				BaseDrawing.DrawChain(sb, array2, 0, vector + new Vector2((float)this.Hitbox.Width * 0.5f, 6f), this.legJoint, 0f, null, 1f, false, null);
				BaseDrawing.DrawChain(sb, new Texture2D[]
				{
					this.textures[0],
					this.textures[1],
					this.textures[0]
				}, 0, this.legJoint, this.GetBodyConnector(npc), 0f, null, 1f, false, null);
			}
			BaseDrawing.DrawTexture(sb, this.textures[4], 0, vector, this.Hitbox.Width, this.Hitbox.Height, npc.scale, this.rotation, (this.limbType == 1 || this.limbType == 3) ? 1 : -1, 1, this.Hitbox, new Color?(alpha), false, this.legOrigin);
		}

		// Token: 0x04000478 RID: 1144
		private Vector2 velocity;

		// Token: 0x04000479 RID: 1145
		private Vector2 legOrigin;

		// Token: 0x0400047A RID: 1146
		private float velOffsetY;

		// Token: 0x0400047B RID: 1147
		private readonly float distanceToMove = 120f;

		// Token: 0x0400047C RID: 1148
		private readonly float distanceToMoveX = 50f;

		// Token: 0x0400047D RID: 1149
		private readonly bool flying;

		// Token: 0x0400047E RID: 1150
		private bool leftLeg;

		// Token: 0x0400047F RID: 1151
		private Vector2 pointToStandOn;

		// Token: 0x04000480 RID: 1152
		private Vector2 legJoint;

		// Token: 0x04000481 RID: 1153
		public Texture2D[] textures;
	}
}
