using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.FeudalFungus
{
    public class FungusSpore : ModNPC
    {
        public float[] internalAI = new float[4];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fungal Spore");
        }

        public override void SetDefaults()
        {
            npc.width = 14;
            npc.height = 14;
            npc.value = BaseUtility.CalcValue(0, 0, 0, 0);
            npc.npcSlots = 1;
            npc.aiStyle = -1;
            npc.lifeMax = 1;
            npc.defense = 0;
            npc.damage = 15;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = null;
            npc.knockBackResist = 0f;
            NPCID.Sets.NeedsExpertScaling[npc.type] = false;
        }

        public override void AI()
        {
            if (npc.ai[0] == 0 && npc.ai[1] == 0)
            {
                npc.velocity.X = 5;
            }
            else if (npc.ai[0] == 1 && npc.ai[1] == 0)
            {
                npc.velocity.X = -5;
            }
            else if (npc.ai[0] == 2 && npc.ai[1] == 0)
            {
                npc.velocity.X = 4;
                npc.velocity.Y = 2.5F;

            }
            else if (npc.ai[0] == 3 && npc.ai[1] == 0)
            {
                npc.velocity.X = -4;
                npc.velocity.Y = 2.5f;
            }
            npc.ai[1] = 1;
            
            BaseAI.AISpore(npc, ref internalAI, 0.1f, 0.02f, 5f, 1f);
            
            if (Collision.SolidCollision(npc.position, npc.width, npc.height))
            {
                npc.velocity *= .96f;
                npc.scale -= .5f;
                if (npc.scale <= 0)
                {
                    npc.active = false;
                    npc.netUpdate = true;
                }
            }
        }
    }
}


