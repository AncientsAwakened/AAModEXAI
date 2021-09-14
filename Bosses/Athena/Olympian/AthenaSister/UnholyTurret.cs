using System;
using AAMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAModEXAI.Bosses.Athena.Olympian.AthenaSister
{
	public class UnholyTurret : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unholy Turret");
			NPCID.Sets.TrailCacheLength[npc.type] = 8;
			NPCID.Sets.TrailingMode[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.scale = 0f;
			npc.lifeMax = (Main.expertMode ? 80 : 120);
			npc.aiStyle = 0;
			npc.damage = (Main.expertMode ? 25 : 42);
			npc.defense = (Main.expertMode ? 2 : 3);
			npc.width = 62;
			npc.height = 62;
			npc.noGravity = true;
			npc.value = (float)Item.buyPrice(0, 0, 0, 0);
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
		}

		public override void AI()
		{
			if (!Main.npc[(int)npc.ai[0]].active)
			{
				npc.life = 0;
				npc.checkDead();
			}
			npc.velocity.X = npc.velocity.X * 3f / 4f;
			npc.velocity.Y = npc.velocity.Y * 3f / 4f;
			Player player = Main.player[npc.target];
			Vector2 vector;
			vector.X = player.Center.X;
			vector.Y = player.Center.Y;
			npc.rotation = npc.AngleTo(vector);
			if (npc.scale < 1f)
			{
				npc.scale += 0.1f;
				npc.width = 62;
				npc.height = 62;
			}
			shootTime++;
			if (shootTime >= 50)
			{
				float num = 18f;
				int num2 = Main.expertMode ? 25 : 42;
				if (Main.netMode != 1)
				{
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Cos((double)npc.rotation) * (double)num), (float)(Math.Sin((double)npc.rotation) * (double)num), mod.ProjectileType("UnholyTurretBeam"), num2, 0f, Main.myPlayer, 0f, 0f);
				}
				shootTime = 0;
			}
			timeAlive++;
			if (timeAlive > 1500)
			{
				npc.life = 0;
				npc.checkDead();
			}
		}

		public override void PostDraw(SpriteBatch spritebatch, Color dColor)
		{
			if (UnholyTurret.glowTex == null)
			{
				UnholyTurret.glowTex = mod.GetTexture("Bosses/Athena/Olympian/AthenaSister/UnholyTurret_GM");
			}
			if (auraDirection)
			{
				auraPercent += 0.1f;
				auraDirection = (auraPercent < 1f);
			}
			else
			{
				auraPercent -= 0.1f;
				auraDirection = (auraPercent <= 0f);
			}
			BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc, new Color?(dColor), false, default(Vector2));
			BaseDrawing.DrawAura(spritebatch, UnholyTurret.glowTex, 0, npc, auraPercent, 1f, 0f, 0f, new Color?(BaseUtility.MultiLerpColor((float)(Main.player[Main.myPlayer].miscCounter % 100) / 100f, new Color[]
			{
				Color.Blue,
				Color.White,
				Color.SkyBlue,
				Color.Blue
			})));
			BaseDrawing.DrawTexture(spritebatch, UnholyTurret.glowTex, 0, npc, new Color?(BaseUtility.MultiLerpColor((float)(Main.player[Main.myPlayer].miscCounter % 100) / 100f, new Color[]
			{
				Color.Blue,
				Color.White,
				Color.SkyBlue,
				Color.Blue
			})), false, default(Vector2));
		}

		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = 1;
		}

		private int shootTime;

		private int timeAlive;

		public static Texture2D glowTex;

		public float auraPercent;

		public bool auraDirection = true;
	}
}
