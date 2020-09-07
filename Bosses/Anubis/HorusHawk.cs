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

namespace AAModEXAI.Bosses.Anubis
{
    public class HorusHawk : ModNPC
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Horus Hawk");
            Main.npcFrameCount[npc.type] = 4;
		}

        public override void SetDefaults()
        {
            npc.width = 42;
            npc.height = 38;
            npc.value = BaseUtility.CalcValue(0, 0, 0, 0);
            npc.npcSlots = 1;
            npc.aiStyle = -1;
            npc.lifeMax = 500;
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
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldCoin, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, 1.1f);
			}		
		}

		public override void FindFrame(int dummy)
        {
            npc.frameCounter++;
            if (dash)
            {
                npc.frame.Y = dummy;
            }
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
        bool dash = false;
		public override void AI()
		{
            dash = false;

            npc.TargetClosest(true);

			Player player = Main.player[npc.target];

            npc.direction = npc.spriteDirection = npc.velocity.X > 0 ? 1 : -1;

            switch (npc.ai[2])
            {
                case 0:
                    BaseAI.AISkull(npc, ref npc.ai, false, 4, 250, .011f, .22f);
                    break;
                case 1:
                    if (++npc.ai[3] > 30)
                    {
                        Vector2 targetPos = player.Center;
                        targetPos.X += 600 * (npc.Center.X < targetPos.X ? -1 : 1);
                        DashMovement(targetPos, 0.8f);
                        if (npc.ai[3] > 180 || Math.Abs(npc.Center.Y - targetPos.Y) < 50) //initiate dash
                        {
                            npc.ai[2]++;
                            npc.ai[3] = 0;
                            npc.netUpdate = true;
                            npc.velocity.X = -30 * (npc.Center.X < player.Center.X ? -1 : 1);
                            npc.velocity.Y *= 0.1f;
                        }
                    }
                    else
                    {
                        npc.velocity *= 0.9f; //decelerate briefly
                    }
                    npc.rotation = 0;
                    break;

                case 2: //dashing
                    dash = true;
                    if (++npc.ai[3] > 240 || (Math.Sign(npc.velocity.X) > 0 ? npc.Center.X > player.Center.X + 600 : npc.Center.X < player.Center.X - 600))
                    {
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                    break;
                default:
                    npc.ai[2] = 0;
                    goto case 0;
            }
            npc.rotation = 0;
		}

        private void DashMovement(Vector2 targetPos, float speedModifier)
        {
            if (npc.Center.X < targetPos.X)
            {
                npc.velocity.X += speedModifier;
                if (npc.velocity.X < 0)
                    npc.velocity.X += speedModifier * 2;
            }
            else
            {
                npc.velocity.X -= speedModifier;
                if (npc.velocity.X > 0)
                    npc.velocity.X -= speedModifier * 2;
            }
            if (npc.Center.Y < targetPos.Y)
            {
                npc.velocity.Y += speedModifier;
                if (npc.velocity.Y < 0)
                    npc.velocity.Y += speedModifier * 2;
            }
            else
            {
                npc.velocity.Y -= speedModifier;
                if (npc.velocity.Y > 0)
                    npc.velocity.Y -= speedModifier * 2;
            }
            if (Math.Abs(npc.velocity.X) > 30)
                npc.velocity.X = 30 * Math.Sign(npc.velocity.X);
            if (Math.Abs(npc.velocity.Y) > 30)
                npc.velocity.Y = 30 * Math.Sign(npc.velocity.Y);
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D bodyTex = Main.npcTexture[npc.type];
            Color lightColor = BaseDrawing.GetNPCColor(npc, null);
            BaseDrawing.DrawTexture(sb, bodyTex, 0, npc, lightColor);
			return false;
		}
	}
}