using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    public class NullZP : ModNPC
	{
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Null");
            Main.npcFrameCount[npc.type] = 4;
        }
		
		public override void SetDefaults()
		{
            npc.CloneDefaults(NPCID.Poltergeist);
            npc.noGravity = true;
            npc.noTileCollide = true;
			npc.aiStyle = -1;
            npc.width = 24;
            npc.height = 40;
            npc.damage = 50;
            npc.defense = 9999999;
            npc.lifeMax = 10;
            npc.HitSound = ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.NPCHit, "Sounds/Sounds/Glitch");
            npc.DeathSound = SoundID.NPCDeath6;
            npc.alpha = 70;
            npc.value = 7000f;
            npc.knockBackResist = 0.1f;
            npc.noGravity = true;
        }

		public int frameCount = 0;
		public int frameCounter = 0;
		public override void PostAI()
		{
            if (!NPC.AnyNPCs(mod.NPCType("ZeroProtocol")))
            {
                npc.alpha++;

                if (npc.alpha >= 255)
                {
                    npc.active = false;
                }
            }
			npc.frame = new Rectangle(0, frameCount * 40, 36, 38);
			npc.spriteDirection = npc.velocity.X > 0 ? -1 : 1;
			npc.rotation = npc.velocity.X * 0.25f;
		}

        public override void AI()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            for (int m = 0; m < 2; m++)
            {
                BaseAI.AIEye(npc, ref npc.ai, false, true, 0.13f, 0.08f, 2f, 1.1f, 1.2f, 1.2f);
                BaseAI.Look(npc, 1);
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (Main.rand.Next(7) == 0)
			{
				target.AddBuff(mod.BuffType("Unstable"), 180, true);
			}
		}
    }
}