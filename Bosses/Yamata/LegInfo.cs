using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Yamata
{
	// Token: 0x020003E8 RID: 1000
	public class LegInfo : LimbInfo
	{
		// Token: 0x0600170A RID: 5898 RVA: 0x000FDDCC File Offset: 0x000FBFCC
		public LegInfo(int lType, Vector2 initialPos, Yamata m)
		{
			this.yamata = m;
			this.position = initialPos;
			this.pointToStandOn = this.position;
			this.limbType = lType;
			this.Hitbox = new Rectangle(0, 0, 70, 38);
			this.legOrigin = new Vector2((float)((this.limbType == 1 || this.limbType == 3) ? (this.Hitbox.Width - 12) : 12), 12f);
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x000FDE5C File Offset: 0x000FC05C
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

		// Token: 0x0600170C RID: 5900 RVA: 0x000FDFE0 File Offset: 0x000FC1E0
		public void UpdateVelOffsetY()
		{
			this.movementRatio += 0.04f;
			this.movementRatio = Math.Max(0f, Math.Min(1f, this.movementRatio));
			float movementRatio = this.movementRatio;
			float[] array = new float[3];
			array[1] = 30f;
			this.velOffsetY = BaseUtility.MultiLerp(movementRatio, array);
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x000FE040 File Offset: 0x000FC240
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

		// Token: 0x0600170E RID: 5902 RVA: 0x000FE228 File Offset: 0x000FC428
		public void UpdateLeg(NPC npc)
		{
			this.leftLeg = (this.limbType == 1 || this.limbType == 3);
			if (Vector2.Distance(base.Center, npc.Center) > 499f || Yamata.TeleportMeBitch)
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

		// Token: 0x0600170F RID: 5903 RVA: 0x000FE324 File Offset: 0x000FC524
		public Vector2 GetStandOnPoint(NPC npc)
		{
			float num = npc.velocity.Length();
			float num2 = 70f + 0.5f * num;
			float num3 = 50f + 0.5f * num;
			float num4 = npc.Center.X + this.yamata.topVisualOffset.X + ((this.limbType == 3) ? (-num2 - (float)this.Hitbox.Width) : ((this.limbType == 2) ? (num2 + (float)this.Hitbox.Width) : ((this.limbType == 1) ? (-num3 - (float)this.Hitbox.Width) : (num3 + (float)this.Hitbox.Width))));
			int num5 = (int)(npc.Bottom.Y / 16f);
			int num6 = BaseWorldGen.GetFirstTileFloor((int)(num4 / 16f), (int)(npc.Bottom.Y / 16f), true);
			if (num6 - num5 > Yamata.flyingTileCount)
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

		// Token: 0x06001710 RID: 5904 RVA: 0x000FE4D4 File Offset: 0x000FC6D4
		public Vector2 GetBodyConnector(NPC npc)
		{
			return npc.Center + this.yamata.topVisualOffset + new Vector2((this.limbType == 3 || this.limbType == 1) ? -40f : 40f, 0f);
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x000FE524 File Offset: 0x000FC724
		public void DrawLeg(SpriteBatch sb, NPC npc)
		{
			Mod instance = AAMod.instance;
			if (this.textures == null)
			{
				bool flag = npc.type == instance.NPCType("YamataA");
				string str = "NPCs/Bosses/Yamata/Yamata";
				if (flag)
				{
					str = "NPCs/Bosses/Yamata/Awakened/YamataA";
				}
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

		// Token: 0x04000407 RID: 1031
		private Vector2 velocity;

		// Token: 0x04000408 RID: 1032
		private Vector2 legOrigin;

		// Token: 0x04000409 RID: 1033
		private float velOffsetY;

		// Token: 0x0400040A RID: 1034
		private readonly float distanceToMove = 120f;

		// Token: 0x0400040B RID: 1035
		private readonly float distanceToMoveX = 50f;

		// Token: 0x0400040C RID: 1036
		private readonly bool flying;

		// Token: 0x0400040D RID: 1037
		private bool leftLeg;

		// Token: 0x0400040E RID: 1038
		private Vector2 pointToStandOn;

		// Token: 0x0400040F RID: 1039
		private Vector2 legJoint;

		// Token: 0x04000410 RID: 1040
		public Texture2D[] textures;
	}
}
