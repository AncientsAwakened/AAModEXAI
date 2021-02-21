using Terraria;
using Terraria.ModLoader;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Buffs
{
    public class YamataGravity : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Abyssal Gravity");
			Description.SetDefault("'YOU AIN'T GETTIN' AWAY FROM ME, PUNK!'");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
		
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<AAModEXPlayer>().YamataGravity = true;
		}
	}
}