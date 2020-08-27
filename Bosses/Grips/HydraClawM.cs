using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Grips
{
    public class HydraClawM : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Claw");
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            aiType = NPCID.DemonEye;  //npc behavior
            animationType = NPCID.DemonEye;
            npc.width = 28;
            npc.height = 24;
            npc.friendly = false;
            npc.damage = 16;
            npc.defense = 3;
            npc.lifeMax = 45;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        public override void AI()
        {
            if (!NPC.AnyNPCs(mod.NPCType("GripOfChaosRed")) && !NPC.AnyNPCs(mod.NPCType("GripOfChaosBlue")))
            {
                npc.alpha += 10;
                if (npc.alpha > 255)
                {
                    npc.active = false;
                }
            }
            AAAI.AIClaw(npc, ref npc.ai, false, true, 0.1f, 0.04f, 5.5f, 2.5f, 1f, 1f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HydraClawGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HydraClawGore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HydraClawGore3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HydraClawGore4"), 1f);
            }
        }
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 180);
        }
    }
}