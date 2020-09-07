using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Broodmother
{
    [AutoloadBossHead]
    public class BroodEgg : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dragon Egg");
        }
        public override void SetDefaults()
        {
            npc.width = 34;
            npc.height = 34;
            npc.aiStyle = -1;
            npc.damage = 0;
            npc.defense = 20;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.lifeMax = 50;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = .2f;
            npc.npcSlots = 0f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
			bool isDead = npc.life <= 0;
            if (isDead)
            {
				for(int m = 0; m < 4; m++)
				{
					Vector2 offset = new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height));
					Gore.NewGore(npc.position + offset, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/BroodGore3"), 1f); //reused brood gore, it looks right for the egg
				}
            }
			for (int m = 0; m < (isDead ? 20 : 5); m++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, 1.3f);
			}
        }
        
        public override void AI()
        {
            if (npc.velocity.Y == 0f)
            {
                npc.velocity.X = npc.velocity.X * 0.9f;
                npc.rotation += npc.velocity.X * 0.02f;
            }
            else
            {
                npc.velocity.X = npc.velocity.X * 0.99f;
                npc.rotation += npc.velocity.X * 0.04f;
            }
            int hatchTimer = 900;
            if (Main.expertMode)
            {
                hatchTimer = 700;
            }
            if (npc.justHit)
            {
                npc.ai[3] -= Main.rand.Next(10, 21);
                if (!Main.expertMode)
                {
                    npc.ai[3] -= Main.rand.Next(10, 21);
                }
            }
            npc.ai[3] += 1f;
            if (npc.ai[3] >= hatchTimer)
            {
                npc.Transform(mod.NPCType("Broodmini"));
            }
            if (Main.netMode != NetmodeID.MultiplayerClient && npc.velocity.Y == 0f && Math.Abs(npc.velocity.X) < 0.2 && npc.ai[3] >= hatchTimer * 0.75)
            {
                float wiggleAmount = npc.ai[3] - (hatchTimer * 0.75f);
                wiggleAmount /= hatchTimer * 0.25f;
                if (Main.rand.Next(-10, 120) < wiggleAmount * 100f)
                {
                    npc.velocity.Y = npc.velocity.Y - (Main.rand.Next(20, 40) * 0.025f);
                    npc.velocity.X = npc.velocity.X + (Main.rand.Next(-20, 20) * 0.025f);
                    npc.velocity *= 1f + (wiggleAmount * 2f);
                    npc.netUpdate = true;
                    return;
                }
            }
        }

		public Color GetGlowAlpha()
		{
			return ColorUtils.COLOR_GLOWPULSE;// new Color(255, 255, 255) * ((float)Main.mouseTextColor / 255f);
		}

        public override void PostDraw(SpriteBatch sb, Color dColor)
        {
			BaseDrawing.DrawTexture(sb, ModLoader.GetMod("AAMod").GetTexture("Glowmasks/BroodEgg_Glow"), 0, npc, GetGlowAlpha());
        }		
    }
}