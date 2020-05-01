using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using AAMod;

namespace AAModEXAI.Bosses.Zero
{
	public class ZeroDeactivated : ModNPC
	{
        public static int ZeroShieldStrength = 0;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strange Machine");
        }
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 20000;
			npc.damage = 0;
			npc.defense = 20;
			npc.knockBackResist = 0f;
            npc.width = 206;
            npc.height = 208;
            npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.alpha = 0;
			npc.dontTakeDamage = true;
			npc.boss = false;
            npc.npcSlots = 0;
        }

		public override bool CheckActive()
		{
			return false;
		}		

		public override void AI()
		{
            RingRoatation += .01f;
            if (Main.netMode != 1 && AAWorld.zeroUS == true)
            {
                npc.Transform(mod.NPCType("Zero"));
                return;
            }
            npc.timeLeft = 10;
			if(npc.ai[0] == 0)
			{
				npc.velocity.Y += 0.003f;	
				if(npc.velocity.Y > .3f)
				{
					npc.ai[0] = 1f;
					npc.netUpdate = true;
				}	
			}else
			if(npc.ai[0] == 1)
			{
				npc.velocity.Y -= 0.003f;	
				if(npc.velocity.Y < -.3f)
				{
					npc.ai[0] = 0f;
					npc.netUpdate = true;
				}				
			}
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;
        public bool saythelinezero = false;
        public float ShieldScale = 0.5f;
        public float RingRoatation = 0;

        public Color GetGlowAlpha()
        {
            return AAColor.ZeroShield;
        }


        public override bool PreDraw(SpriteBatch spritebatch, Color drawColor)
        {
            Texture2D Shield = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Zero/ZeroShield");
            Texture2D Ring = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Zero/ZeroShieldRing");
            Texture2D RingGlow = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/ZeroShieldRing_Glow");

            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc, drawColor);
            if (NPC.downedMoonlord)
            {
                BaseDrawing.DrawTexture(spritebatch, ModLoader.GetMod("AAMod").GetTexture("Glowmasks/ZeroDeactivated_Glow"), 0, npc, GetGlowAlpha());
            }
            BaseDrawing.DrawTexture(spritebatch, Shield, 0, npc.position, npc.width, npc.height, ShieldScale, 0, 0, 1, new Rectangle(0, 0, Shield.Width, Shield.Height), GetGlowAlpha(), true);
            BaseDrawing.DrawTexture(spritebatch, Ring, 0, npc.position, npc.width, npc.height, 1, RingRoatation, 0, 1, new Rectangle(0, 0, RingGlow.Width, RingGlow.Height), drawColor, true);
            BaseDrawing.DrawTexture(spritebatch, RingGlow, 0, npc.position, npc.width, npc.height, 1, RingRoatation, 0, 1, new Rectangle(0, 0, RingGlow.Width, RingGlow.Height), AAColor.COLOR_WHITEFADE1, true);
            
            return false;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage = 0;
            return false;
        }
    }

    public class ZeroHandler : ModWorld
    {
        public static int ZX = -1;
        public static int ZY = -1;
        public static int Shield;

        public override void Initialize()
        {
            ZX = -1;
            ZY = -1;
        }

        public override TagCompound Save()
        {
            var tag = new TagCompound();
            if (ZX != -1)
            {
                tag.Add("ZX", ZX);
                tag.Add("ZY", ZY);
            }
            return tag;
        }

        public override void Load(TagCompound tag)
        {
			Reset(); //reset it so it doesn't fuck up between world loads	
            if (tag.ContainsKey("ZX"))
            {
                ZX = tag.GetInt("ZX");
                ZY = tag.GetInt("ZY");
				if(!AAWorld.downedZero)			
					NPC.NewNPC(ZX, ZY, mod.NPCType("ZeroDeactivated"));
            }
        }

        public override void PostUpdate()
        {
            if (Main.netMode != 1 && !AAWorld.downedZero)
            {
                SpawnDeactivatedZero();
            }
        }

		public void Reset()
		{
			ZX = -1;
			ZY = -1;
		}

        public void SpawnDeactivatedZero()
        {
            int VoidHeight = 140;
			
			Point spawnTilePos = new Point((Main.maxTilesX / 15 * 14) + (Main.maxTilesX / 15 / 2) - 100, VoidHeight);				
			Vector2 spawnPos = new Vector2(spawnTilePos.X * 16, spawnTilePos.Y * 16);
			bool anyZerosExist = NPC.AnyNPCs(mod.NPCType("ZeroDeactivated")) || NPC.AnyNPCs(mod.NPCType("Zero")) || NPC.AnyNPCs(mod.NPCType("ZeroProtocol"));			
			if (!anyZerosExist)
			{
                int whoAmI = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, mod.NPCType("ZeroDeactivated"));
                ZX = (int)spawnPos.X;
				ZY = (int)spawnPos.Y;				
				if (Main.netMode == 2 && whoAmI != -1 && whoAmI < 200)
				{					
					NetMessage.SendData(MessageID.SyncNPC, number: whoAmI);
				}			
			}
        }
    }
}
