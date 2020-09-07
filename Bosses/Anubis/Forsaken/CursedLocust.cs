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

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    public class CursedLocust : ModNPC
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Cursed Locust");
            Main.npcFrameCount[npc.type] = 4;
		}

        public override void SetDefaults()
        {
            npc.width = 42;
            npc.height = 38;
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
        }
        
        public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server) { return; }
			for (int m = 0; m < (npc.life <= 0 ? 30 : 8); m++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<ForsakenDust>(), npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, 1.1f);
			}		
		}

		public override void FindFrame(int dummy)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 2)
            {
                npc.frameCounter = 0;
                npc.frame.Y += dummy;
                if (npc.frame.Y > dummy * 3)
                {
                    npc.frame.Y = 0;
                }
            }
        }

		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			for (int m = npc.oldPos.Length - 1; m > 0; m--)
			{
				npc.oldPos[m] = npc.oldPos[m - 1];
			}
			npc.oldPos[0] = npc.position;

            BaseAI.AISkull(npc, ref npc.ai, true, 4, 250, .2f, .26f);

            if (npc.ai[1] <= 600f)
            {
                BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, mod.ProjectileType("CurseFlame"), ref npc.ai[3], 120, npc.damage / 2, 12, false);
            }

            if (player.Center.X < npc.Center.X)
            {
                npc.direction = npc.spriteDirection = -1;
            }
            else
            {
                npc.direction = npc.spriteDirection  = 1;
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D bodyTex = Main.npcTexture[npc.type];
            Color lightColor = BaseDrawing.GetNPCColor(npc, null);
		    BaseDrawing.DrawAfterimage(sb, bodyTex, 0, npc, 3f, 0.9f, 4, true, 0f, 0f, Color.MediumPurple);
            BaseDrawing.DrawTexture(sb, bodyTex, 0, npc, lightColor);
			return false;
		}
	}
}