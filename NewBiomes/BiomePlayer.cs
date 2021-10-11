using System;
using System.IO;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace AAModEXAI
{
	public class BiomePlayer : ModPlayer
	{
        public bool ZoneRajahStatue = false;

		public override void Initialize()
        {
            ZoneRajahStatue = false;
		}

		public override void UpdateBiomes()
        {
            ZoneRajahStatue = MicroBiomeWorld.RajahStatueNumber >= 1;
		}

		public override void UpdateBiomeVisuals()
        {
		}

		public override bool CustomBiomesMatch(Player other)
        {
            BiomePlayer modOther = other.GetModPlayer<BiomePlayer>();
            return ZoneRajahStatue == modOther.ZoneRajahStatue;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            BiomePlayer modOther = other.GetModPlayer<BiomePlayer>();
            modOther.ZoneRajahStatue = ZoneRajahStatue;
        }

		public override void SendCustomBiomes(BinaryWriter bb)
        {
            BitsByte zoneByte = 0;
            zoneByte[0] = ZoneRajahStatue;
            bb.Write(zoneByte);
        }

		public override void ReceiveCustomBiomes(BinaryReader bb)
        {
            BitsByte zoneByte = bb.ReadByte();
            ZoneRajahStatue = zoneByte[0];
        }
	}
}