using System;
using System.IO;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.MushroomMonarch
{
	// Token: 0x02000475 RID: 1141
	[AutoloadBossHead]
	public class FeudalFungus : ModNPC
	{
		// Token: 0x06001AFD RID: 6909 RVA: 0x0013495C File Offset: 0x00132B5C
		public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);
			if (Main.netMode == 2 || Main.dedServ)
			{
				writer.Write(this.internalAI[0]);
				writer.Write(this.internalAI[1]);
				writer.Write(this.internalAI[2]);
				writer.Write(this.internalAI[3]);
			}
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x001349B8 File Offset: 0x00132BB8
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);
			if (Main.netMode == 1)
			{
				this.internalAI[0] = BaseExtensions.ReadFloat(reader);
				this.internalAI[1] = BaseExtensions.ReadFloat(reader);
				this.internalAI[2] = BaseExtensions.ReadFloat(reader);
				this.internalAI[3] = BaseExtensions.ReadFloat(reader);
			}
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x00134A0C File Offset: 0x00132C0C
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Feudal Fungus");
			Main.npcFrameCount[base.npc.type] = 8;
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x00134A30 File Offset: 0x00132C30
		public override void SetDefaults()
		{
			base.npc.lifeMax = 1200;
			base.npc.damage = 12;
			base.npc.defense = 12;
			base.npc.knockBackResist = 0f;
			base.npc.value = (float)Item.sellPrice(0, 0, 50, 0);
			base.npc.aiStyle = 26;
			base.npc.width = 74;
			base.npc.height = 108;
			base.npc.npcSlots = 1f;
			base.npc.boss = true;
			base.npc.lavaImmune = true;
			base.npc.noGravity = false;
			base.npc.buffImmune[46] = true;
			base.npc.buffImmune[47] = true;
			base.npc.netAlways = true;
			base.npc.noGravity = true;
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = SoundID.NPCDeath1;
			this.bossBag = base.mod.ItemType("FungusBag");
			this.music = base.mod.GetSoundSlot(51, "Sounds/Music/Fungus");
			base.npc.alpha = 255;
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x00134B7C File Offset: 0x00132D7C
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.5f;
			return null;
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x00134B9C File Offset: 0x00132D9C
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
			Player player = Main.player[base.npc.target];
			if ((Main.dayTime && (double)player.position.Y < Main.worldSurface) || !player.ZoneGlowshroom)
			{
				base.npc.velocity *= 0f;
				if (base.npc.velocity.X <= 0.1f && base.npc.velocity.X >= -0.1f)
				{
					base.npc.velocity.X = 0f;
				}
				if (base.npc.velocity.Y <= 0.1f && base.npc.velocity.Y >= -0.1f)
				{
					base.npc.velocity.Y = 0f;
				}
				base.npc.alpha += 10;
				if (base.npc.alpha >= 255)
				{
					base.npc.active = false;
				}
				return;
			}
			base.npc.alpha -= 10;
			if (base.npc.alpha < 0)
			{
				base.npc.alpha = 0;
			}
			base.npc.frameCounter += 1.0;
			if (base.npc.frameCounter >= 10.0)
			{
				base.npc.frameCounter = 0.0;
				NPC npc = base.npc;
				npc.frame.Y = npc.frame.Y + 90;
				if (base.npc.frame.Y > 630)
				{
					base.npc.frameCounter = 0.0;
					base.npc.frame.Y = 0;
				}
			}
			base.npc.noTileCollide = true;
			if (Main.netMode != 1 && this.internalAI[1] != (float)FeudalFungus.AISTATE_SHOOT)
			{
				this.internalAI[0] += 1f;
				if (this.internalAI[0] >= 180f)
				{
					this.internalAI[0] = 0f;
					this.internalAI[1] = (float)Main.rand.Next(3);
					base.npc.ai = new float[4];
					base.npc.netUpdate = true;
				}
			}
			if (this.internalAI[1] == (float)FeudalFungus.AISTATE_HOVER)
			{
				BaseAI.AISpaceOctopus(base.npc, ref base.npc.ai, player.Center, 0.15f, 4f, 170f, 56f, new Action<NPC, Vector2>(this.FireMagic));
			}
			else if (this.internalAI[1] == (float)FeudalFungus.AISTATE_FLIER)
			{
				BaseAI.AIFlier(base.npc, ref base.npc.ai, true, 0.1f, 0.04f, 5f, 3f, false, 1);
			}
			else if (this.internalAI[1] == (float)FeudalFungus.AISTATE_SHOOT)
			{
				BaseAI.AISpaceOctopus(base.npc, ref base.npc.ai, player.Center, 0.15f, 4f, 170f, 56f, null);
				if (Main.netMode != 1)
				{
					this.internalAI[0] += 1f;
				}
				if (this.internalAI[0] >= 60f)
				{
					int attack = Main.rand.Next(4);
					this.internalAI[1] = (float)Main.rand.Next(3);
					this.internalAI[0] = 0f;
					this.FungusAttack(attack);
					base.npc.netUpdate = true;
				}
			}
			base.npc.rotation = 0f;
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x00134F84 File Offset: 0x00133184
		public void FireMagic(NPC npc, Vector2 velocity)
		{
			Player player = Main.player[npc.target];
			BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, BaseExtensions.ProjType(base.mod, "Mushshot"), ref this.shootAI[0], 5f, this.damage, 8f, true, new Vector2(20f, 15f));
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x00134FF4 File Offset: 0x001331F4
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = 189;
			AAWorld.downedFungus = true;
			Projectile.NewProjectile(base.npc.Center, base.npc.velocity, base.mod.ProjectileType("FungusIGoNow"), 0, 0f, 255, base.npc.scale, 0f);
			Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("GlowingSporeSac"), Main.rand.Next(30, 35), false, 0, false, false);
			if (Main.rand.Next(10) == 0)
			{
				Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("FungusTrophy"), 1, false, 0, false, false);
			}
			if (Main.expertMode)
			{
				base.npc.DropBossBags();
				return;
			}
			if (Main.rand.Next(7) == 0)
			{
				Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("FungusMask"), 1, false, 0, false, false);
			}
			Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("GlowingMushium"), Main.rand.Next(25, 35), false, 0, false, false);
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x001351E9 File Offset: 0x001333E9
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			base.npc.lifeMax = (int)((float)base.npc.lifeMax * 0.6f * bossLifeScale);
			base.npc.damage = (int)((float)base.npc.damage * 0.6f);
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x0013522C File Offset: 0x0013342C
		public void FungusAttack(int Attack)
		{
			Player player = Main.player[base.npc.target];
			if (Attack == 0)
			{
				if (NPC.CountNPCS(ModContent.NPCType<Mushling>()) < 4)
				{
					for (int i = 0; i < (Main.expertMode ? 3 : 2); i++)
					{
						NPC.NewNPC((int)base.npc.Center.X, (int)base.npc.Center.Y, ModContent.NPCType<Mushling>(), 0, 0f, 0f, 0f, 0f, 255);
					}
					return;
				}
				Attack = 2;
				return;
			}
			else
			{
				if (Attack == 1)
				{
					for (int j = 0; j < 4; j++)
					{
						NPC.NewNPC((int)base.npc.Center.X, (int)base.npc.Center.Y, ModContent.NPCType<FungusFlier>(), 0, 0f, 0f, 0f, 0f, 255);
					}
					return;
				}
				if (Attack == 2)
				{
					float num = 0.2088f;
					double num2 = Math.Atan2((double)base.npc.velocity.X, (double)base.npc.velocity.Y) - (double)(num / 2f);
					double num3 = (double)(num / (float)(Main.expertMode ? 5 : 4));
					for (int k = 0; k < (Main.expertMode ? 5 : 4); k++)
					{
						double num4 = num2 + num3 * (double)(k + k * k) / 2.0 + (double)(32f * (float)k);
						Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, (float)(Math.Sin(num4) * 6.0), (float)(Math.Cos(num4) * 6.0), base.mod.ProjectileType("FungusCloud"), this.damage, 0f, Main.myPlayer, 0f, 0f);
					}
					return;
				}
				for (int l = 0; l < 4; l++)
				{
					NPC.NewNPC((int)base.npc.Center.X, (int)base.npc.Center.Y, ModContent.NPCType<FungusSpore>(), 0, (float)l, 0f, 0f, 0f, 255);
				}
				return;
			}
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x00135474 File Offset: 0x00133674
		public void MoveToPoint(Vector2 point, bool goUpFirst = false)
		{
			float num = 4f;
			if (num == 0f || base.npc.Center == point)
			{
				return;
			}
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

		// Token: 0x06001B08 RID: 6920 RVA: 0x0013556C File Offset: 0x0013376C
		public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
		{
			Texture2D texture = base.mod.GetTexture("Glowmasks/FeudalFungus_Glow");
			BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[base.npc.type], 0, base.npc.position, base.npc.width, base.npc.height, base.npc.scale, base.npc.rotation, 0, 8, base.npc.frame, new Color?(base.npc.GetAlpha(dColor)), true, default(Vector2));
			BaseDrawing.DrawTexture(spritebatch, texture, 0, base.npc.position, base.npc.width, base.npc.height, base.npc.scale, base.npc.rotation, 0, 8, base.npc.frame, new Color?(AAColor.Glow), true, default(Vector2));
			return false;
		}

		// Token: 0x0400052E RID: 1326
		public int damage;

		// Token: 0x0400052F RID: 1327
		public static int AISTATE_HOVER = 0;

		// Token: 0x04000530 RID: 1328
		public static int AISTATE_FLIER = 1;

		// Token: 0x04000531 RID: 1329
		public static int AISTATE_SHOOT = 2;

		// Token: 0x04000532 RID: 1330
		public float[] internalAI = new float[4];

		// Token: 0x04000533 RID: 1331
		public float[] shootAI = new float[4];
	}
}
