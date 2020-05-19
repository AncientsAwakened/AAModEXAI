using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Toad
{
    public class ShroomGlow : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glowing Mushroom");
            Main.npcFrameCount[npc.type] = 7;
        }

        public override void SetDefaults()
        {
            npc.width = 48;
            npc.height = 40;
            npc.aiStyle = -1;
            npc.damage = 0;
            npc.defense = 12;
            npc.lifeMax = 100;
            npc.knockBackResist = 0f;
            npc.npcSlots = 0f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.aiStyle = -1;
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
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, ModLoader.GetMod("AAMod").DustType("ShroomDust"), default, isDead ? 2f : 1.5f);
            }
        }

        public int body = -1;

        public override void AI()
        {
            npc.TargetClosest(false);
            if (npc.alpha > 0)
            {
                npc.alpha -= 4;
            }
            else
            {
                npc.alpha = 0;
            }
            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(npc.Center, mod.NPCType("TruffleToad"), 1000, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;
            NPC toad = Main.npc[body];
            if (toad == null || toad.life <= 0 || !toad.active || toad.type != mod.NPCType("TruffleToad")) { BaseAI.KillNPCWithLoot(npc); return; }

        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 5)
            {
                npc.frame.Y += frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y > frameHeight * 6)
            {
                npc.frame.Y = frameHeight * 6;
            }
        }

        public override bool PreNPCLoot()
        {
            return false;
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