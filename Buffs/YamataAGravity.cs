﻿using Terraria;
using Terraria.ModLoader;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Buffs
{
    public class YamataAGravity : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("True Abyssal Gravity");
			Description.SetDefault("'REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE'");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
		
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<AAModEXPlayer>().YamataAGravity = true;
		}
	}
}