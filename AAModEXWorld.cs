using System.IO;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader.IO;
using AAModEXAI.Dusts;

namespace AAMod
{
    public class AAModEXAIWorld : ModWorld
    {

        public static bool downedRajahsRevenge;
        public override void Initialize()
        {
            downedRajahsRevenge = false;
        }

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (downedRajahsRevenge) downed.Add("RajahsRevenge");

            return new TagCompound {
                {"downed", downed},
            };
        }

        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            //bosses
            downedRajahsRevenge = downed.Contains("RajahsRevenge");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedRajahsRevenge;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedRajahsRevenge = flags[0];
        }
    }
}
