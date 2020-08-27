using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Grips
{
    public class DragonClawM : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dragon Claw");
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            aiType = NPCID.DemonEye;
            animationType = NPCID.DemonEye;
            npc.width = 28;
            npc.height = 24;
            npc.friendly = false;
            npc.damage = 14;
            npc.defense = 6;
            npc.lifeMax = 45;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 0.6f;
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
            AAAI.AIClaw(npc, ref npc.ai, true, false, 0.1f, 0.04f, 4f, 1.5f, 1f, 1f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DragonClawGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DragonClawGore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DragonClawGore3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DragonClawGore4"), 1f);
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }
    }
}