using System.IO;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader.IO;
using AAModEXAI.Dusts;

namespace AAModEXAI
{
    public class AAModEXAIWorld : ModWorld
    {
        public static bool downedSisters;
        public static bool downedAkuma;
        public static bool downedAnubis;
        public static bool downedAnubisA;
        public static bool AnubisAwakened;
        public static bool downedRajahsRevenge;
        public static bool CRajahFirst;

        public override void Initialize()
        {
            downedSisters = false;
            downedAkuma = false;
            downedAnubis = false;
            downedAnubisA = false;
            AnubisAwakened = false;
            downedRajahsRevenge = false;
            CRajahFirst = false;
        }

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (downedSisters) downed.Add("downedSisters");
            if (downedAkuma) downed.Add("downedAkuma");
            if (downedAnubis) downed.Add("downedAnubis");
            if (AnubisAwakened) downed.Add("AnubisAwakened");
            if (downedAnubisA) downed.Add("downedAnubisA");
            if (downedRajahsRevenge) downed.Add("RajahsRevenge");
            if (CRajahFirst) downed.Add("CRajahFirst");

            return new TagCompound {
                {"AAModEXAIWorlddowned", downed},
            };
        }

        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("AAModEXAIWorlddowned");
            //bosses
            downedSisters = downed.Contains("downedSisters");
            downedAkuma = downed.Contains("downedAkuma");
            downedAnubis = downed.Contains("downedAnubis");
            AnubisAwakened = downed.Contains("AnubisAwakened");
            downedAnubisA = downed.Contains("downedAnubisA");
            downedRajahsRevenge = downed.Contains("RajahsRevenge");
            CRajahFirst = downed.Contains("CRajahFirst");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedSisters;
            flags[1] = downedAkuma;
            flags[2] = downedAnubis;
            flags[3] = AnubisAwakened;
            flags[4] = downedAnubisA;
            
            BitsByte flags2 = new BitsByte();
            flags2[0] = downedRajahsRevenge;
            flags2[1] = CRajahFirst;
            writer.Write(flags2);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedSisters = flags[0];
            downedAkuma = flags[1];
            downedAnubis = flags[2];
            AnubisAwakened = flags[3];
            downedAnubisA = flags[4];

            BitsByte flags2 = reader.ReadByte();
            downedRajahsRevenge = flags2[0];
            CRajahFirst = flags2[1];
        }
    }
}
