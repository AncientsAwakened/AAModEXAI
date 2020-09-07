/*
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Toad
{
    public class TinyToad : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Toad");
            Main.npcFrameCount[npc.type] = 7;
        }

        public override void SetDefaults()
        {
            npc.width = 30;
            npc.height = 28;
            npc.aiStyle = -1;
            npc.damage = 20;
            npc.defense = 10;
            npc.lifeMax = 100;
            npc.knockBackResist = 0f;
            npc.npcSlots = 0f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.alpha = 255;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            bool isDead = npc.life <= 0;
            if (isDead) 
            {

            }
            for (int m = 0; m < (isDead ? 35 : 6); m++)
            {
                int dustType = ModContent.DustType<ShroomDust>();
                Dust.NewDust(npc.position, npc.width, npc.height, dustType, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default, isDead ? 2f : 1.5f);
            }
        }
        
        public override void AI()
        {
            npc.TargetClosest(true);
            if (npc.alpha > 0)
            {
                npc.alpha -= 4;
            }
            else
            {
                npc.alpha = 0;
            }
            Player player = Main.player[npc.target];
            if (npc.velocity.Y != 0)
            {
                if (npc.velocity.X < 0)
                {
                    npc.spriteDirection = -1;
                }
                else if (npc.velocity.X > 0)
                {
                    npc.spriteDirection = 1;
                }
            }
            else
            {
                if (player.position.X < npc.position.X)
                {
                    npc.spriteDirection = -1;
                }
                else if (player.position.X > npc.position.X)
                {
                    npc.spriteDirection = 1;
                }
            }
            if (npc.ai[0] < -10) npc.ai[0] = -10; //force rapid jumping
            BaseAI.AISlime(npc, ref npc.ai, false, 30, 6f, -6f, 6f, -8f);
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.velocity.Y < 0)
            {
                npc.frame.Y = frameHeight * 4;
            }
            else if (npc.velocity.Y > 0)
            {
                npc.frame.Y = frameHeight * 5;
            }
            else if (npc.ai[0] < -15f)
            {
                npc.frame.Y = 0;
            }
            else if (npc.ai[0] > -15f)
            {
                npc.frame.Y = frameHeight;
            }
            else if (npc.ai[0] > -10f)
            {
                npc.frame.Y = frameHeight * 2;
            }
            else if (npc.ai[0] > -5f)
            {
                npc.frame.Y = frameHeight * 3;
            }
        }

        public override void PostAI()
        {
            if (NPC.AnyNPCs(mod.NPCType("TruffleToad")))
            {
                if (npc.alpha > 0)
                {
                    npc.alpha -= 5;
                }
                else
                {
                    npc.alpha = 0;
                }
            }
            else
            {
                npc.dontTakeDamage = true;
                if (npc.alpha < 255)
                {
                    npc.alpha += 5;
                }
                else
                {
                    npc.active = false;
                }
            }
        }
    }
}
*/