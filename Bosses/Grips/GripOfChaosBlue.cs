using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Grips
{
    [AutoloadBossHead]
    public class GripOfChaosBlue : BaseGripOfChaos
    {
        public override void SetDefaults()
        {
			base.SetDefaults();
			npc.lifeMax = 1400;
            npc.damage = 30;
            npc.defense = 10;		
            npc.buffImmune[BuffID.Poisoned] = true;	

			offsetBasePoint = new Vector2(240f, 0f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0) //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MireGripGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MireGripGore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MireGripGore3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MireGripGore4"), 1f);
            }
        }

        public override void NPCLoot()
        {
            int redGripExists = NPC.CountNPCS(mod.NPCType("GripOfChaosRed"));
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("GripTrophyBlue"));
            }
            if (redGripExists == 0)
            {
                if (Main.rand.Next(4) == 0 && !Main.expertMode)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("ClawBaton"));
                }

                AAWorld.downedGrips = true;
                if (Main.expertMode)
                {
                    npc.DropBossBags();
                }
            }
            else
            {
                if (Main.rand.Next(10) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("GripMaskBlue"));
                }
            }
            if (!Main.expertMode)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModLoader.GetMod("AAMod").ItemType("Abyssium"), Main.rand.Next(30, 44));
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if (Main.rand.Next(2) == 0 || (Main.expertMode && Main.rand.Next(0) == 0))       //Chances for it to inflict the debuff
            {
                target.AddBuff(BuffID.Poisoned, Main.rand.Next(180, 250));       //Main.rand.Next part is the length of the buff, so 8.3 seconds to 16.6 seconds
            }
        }		
    }
}
