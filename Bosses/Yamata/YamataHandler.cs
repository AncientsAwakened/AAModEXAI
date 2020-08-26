
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAMod
{
    public abstract class YamataBoss : ParentNPC
	{
		public int frameWidth = 0;

		public int frameHeight = 0;

		public float nextFrameCounter = 0f;

		public int frameCount = 0;

		public bool invertFrames = false;

		public bool showHealthBar = true;

		public bool realLifeHealthBar = false;

		public bool invasionSpawn = false;

		public bool specialBiomeSpawn = false;

		public bool drawCentered = false;

		public bool drawCenteredX = false;

		public Vector2 oldDrawPos = default;

        public override bool CloneNewInstances => true;

        public string name
		{
			get
			{
				return npc.TypeName;
			}
			set
			{
			}
		}

		public string displayName
		{
			get
			{
				return DisplayName.GetDefault();
			}
			set
			{
                DisplayName.SetDefault(value);
			}
		}

		public override Vector4 GetFrameV4()
		{
			return new Vector4(0f, 0f, frameWidth, frameHeight + 2);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);
			SendMaster(writer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);
			RecieveMaster(reader);
		}

		public virtual void SetMaster(params object[] args)
		{
		}

		public virtual void SendMaster(BinaryWriter writer)
		{
		}

		public virtual void RecieveMaster(BinaryReader reader)
		{
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			bool? result;
			if (!showHealthBar)
			{
				npc.position -= npc.visualOffset;
				result = new bool?(false);
			}
			else if (realLifeHealthBar)
			{
				if (npc.realLife == -1)
				{
					result = new bool?(false);
				}
				else
				{
					float alpha = Lighting.Brightness((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f));
					Main.instance.DrawHealthBar(position.X, position.Y, Main.npc[npc.realLife].life, Main.npc[npc.realLife].lifeMax, alpha, scale);
					npc.position -= npc.visualOffset;
					result = new bool?(false);
				}
			}
			else
			{
				if (npc.boss)
				{
					scale = 1.5f;
				}
				result = null;
			}
			return result;
		}

		public override void FindFrame(int dummy)
		{
			if (frameWidth > 0 && frameHeight > 0)
			{
				npc.frame = BaseDrawing.GetFrame(frameCount, frameWidth, frameHeight, 0, 2);
			}
		}

		public override void HitEffect(int hitDir, double damage)
		{
			G_HitEffect(hitDir, damage, npc.life <= 0 || !npc.active);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			float result;
			if (!invasionSpawn && (Main.invasionType > 0 || Main.pumpkinMoon || Main.snowMoon || Main.bloodMoon || Main.eclipse || DD2Event.Ongoing || BaseExtensions.InZone(spawnInfo.player, "TowerAny", null)))
			{
				result = 0f;
			}
			else if (!specialBiomeSpawn && (BaseExtensions.InZone(spawnInfo.player, "TowerAny", null) || BaseExtensions.InZone(spawnInfo.player, "Dungeon", null) || BaseExtensions.InZone(spawnInfo.player, "Meteor", null) || spawnInfo.lihzahrd))
			{
				result = 0f;
			}
			else
			{
				result = G_CanSpawn(spawnInfo.spawnTileX, spawnInfo.spawnTileY, npc.type, spawnInfo.player, spawnInfo) ? 1f : 0f;
			}
			return result;
		}

		public virtual void G_HitEffect(int hitDirection, double damage, bool isDead)
		{
		}

		public virtual bool G_CanSpawn(int x, int y, int type, Player player, NPCSpawnInfo info)
		{
			return G_CanSpawn(x, y, type, player);
		}

		public virtual bool G_CanSpawn(int x, int y, int type, Player player)
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (drawCentered || drawCenteredX)
			{
				oldDrawPos = npc.position;
				if (drawCenteredX)
				{
					NPC expr_48_cp_0 = npc;
					expr_48_cp_0.position.X += npc.Center.X - npc.position.X;
				}
				else
				{
					npc.position += npc.Center - npc.position;
				}
			}
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (drawCentered || drawCenteredX)
			{
				npc.position = oldDrawPos;
			}
		}
	}
}
