using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    public class Naddaha : ModNPC
	{
		public override void SetStaticDefaults()
		{
            Main.npcFrameCount[npc.type] = 16;
		}

        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 64;
            npc.value = BaseUtility.CalcValue(0, 0, 0, 0);
            npc.npcSlots = 1;
            npc.aiStyle = -1;
            npc.lifeMax = 400;
            npc.defense = 30;
            npc.damage = 40;
            npc.HitSound = SoundID.NPCHit31;
            npc.DeathSound = SoundID.NPCDeath35;
            npc.knockBackResist = 0.2f;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }
        
        public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server) { return; }
			for (int m = 0; m < (npc.life <= 0 ? 30 : 8); m++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("ForsakenDust"), npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, 1.1f);
			}		
		}

		public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 10)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (Shooty == true)
                {
                    if (npc.frame.Y < frameHeight * 8)
                    {
                        npc.frame.Y = frameHeight * 8;
                    }
                    if (npc.frame.Y > (frameHeight * 15))
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y = 0;
                        Shooty = false;
                    }
                }
                else
                {
                    if (npc.frame.Y > (frameHeight * 7))
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y = 0;
                    }
                }
            }
        }

        public bool Shooty = false;

        public override void AI()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];

            BaseAI.AIEye(npc, ref npc.ai, false, true, 0.2f, 0.16f, 6f, 2f);
            BaseAI.Look(npc, 1);

            if (npc.ai[3] >= 120)
            {
                FireMagic(npc);
                npc.ai[3] = 0;
            }

            if (player.Center.X < npc.Center.X)
            {
                npc.direction = npc.spriteDirection = -1;
            }
            else
            {
                npc.direction = npc.spriteDirection = 1;
            }
        }

        public void FireMagic(NPC npc)
        {
            Player player = Main.player[npc.target];
            Shooty = true;

            BaseAI.FireProjectile(player.Center, npc, mod.ProjectileType("CurseFlame"), npc.damage / 2, 0f, 2f);
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D bodyTex = Main.npcTexture[npc.type];
            Color lightColor = BaseDrawing.GetNPCColor(npc, null);
            BaseDrawing.DrawTexture(sb, bodyTex, 0, npc, lightColor);
            BaseDrawing.DrawTexture(sb, ModLoader.GetMod("AAMod").GetTexture("Glowmasks/Naddaha_Glow"), 0, npc, Color.White, true);
            return false;
		}
	}
}