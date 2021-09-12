using System;
using AAMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAModEXAI.Bosses.Athena.Olympian.AthenaSister
{
	public class OrbitingTurret : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orbiting Turret");
			NPCID.Sets.TrailCacheLength[npc.type] = 8;
			NPCID.Sets.TrailingMode[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.scale = 0f;
			npc.dontTakeDamage = true;
			npc.lifeMax = (Main.expertMode ? 100 : 150);
			npc.aiStyle = 0;
			npc.damage = (Main.expertMode ? 25 : 42);
			npc.defense = (Main.expertMode ? 2 : 3);
			npc.width = 40;
			npc.height = 40;
			npc.noGravity = true;
			npc.value = (float)Item.buyPrice(0, 0, 0, 0);
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
		}

		public override bool PreAI()
		{
			parent = Main.player[(int)npc.ai[0]];
			if (!parent.active)
			{
				npc.active = false;
			}
			if (start)
			{
				start = false;
			}
			npc.TargetClosest(true);
			double num = (double)npc.ai[1] * 2.0 * 0.017453292519943295;
			double num2 = 200.0;
			npc.velocity.X = npc.velocity.X * 3f / 4f;
			npc.velocity.Y = npc.velocity.Y * 3f / 4f;
			Vector2 vector;
			vector.X = parent.Center.X;
			vector.Y = parent.Center.Y;
			if (npc.scale < 1f)
			{
				npc.scale += 0.1f;
			}
			shootTime++;
			if (shootTime >= 120)
			{
				npc.velocity = Vector2.Zero;
			}
			else
			{
				npc.position.X = parent.Center.X - (float)((int)(Math.Cos(num) * num2)) - (float)(npc.width / 2);
				npc.position.Y = parent.Center.Y - (float)((int)(Math.Sin(num) * num2)) - (float)(npc.height / 2);
				npc.ai[1] += 2f;
				npc.rotation = npc.AngleTo(vector);
			}
			if (shootTime == 140)
			{
				int num3 = Main.expertMode ? 25 : 42;
				if (Main.netMode != 1)
				{
					Main.PlaySound(SoundID.Item12, npc.Center);
					Projectile.NewProjectile(npc.Center, Utils.RotatedBy(new Vector2(10f, 0f), (double)npc.rotation, default(Vector2)) * 2f, mod.ProjectileType("UnholyTurretBeam"), num3, 0f, Main.myPlayer, 0f, 0f);
				}
			}
			if (shootTime == 170)
			{
				for (int i = 0; i < 50; i++)
				{
					Vector2 center = npc.Center;
					Dust dust = Main.dust[Dust.NewDust(center - new Vector2(15f, 15f), 30, 30, 226, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
				}
				npc.active = false;
				Main.PlaySound(SoundID.Item70, npc.Center);
			}
			return false;
		}

		public override void PostDraw(SpriteBatch spritebatch, Color dColor)
		{
			if (OrbitingTurret.glowTex == null)
			{
				OrbitingTurret.glowTex = mod.GetTexture("Bosses/Athena/Olympian/AthenaSister/UnholyTurret_GM");
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
			BaseDrawing.DrawAura(spritebatch, OrbitingTurret.glowTex, 0, npc, auraPercent, 1f, 0f, 0f, new Color?(BaseUtility.MultiLerpColor((float)(Main.player[Main.myPlayer].miscCounter % 100) / 100f, new Color[]
			{
				Color.Blue,
				Color.White,
				Color.SkyBlue,
				Color.Blue
			})));
			BaseDrawing.DrawTexture(spritebatch, OrbitingTurret.glowTex, 0, npc, new Color?(BaseUtility.MultiLerpColor((float)(Main.player[Main.myPlayer].miscCounter % 100) / 100f, new Color[]
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

		private bool start = true;

		private Player parent;

		public static Texture2D glowTex;

		public float auraPercent;

		public bool auraDirection = true;
	}
}
