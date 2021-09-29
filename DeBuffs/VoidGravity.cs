using Terraria;
using Terraria.ModLoader;

namespace AAModEXAI.DeBuffs
{
    public class VoidGravity : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Strange Gravity");
			Description.SetDefault("You can't feel gravity in this strange field");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
		
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<AAModEXPlayer>().VoidGravity = true;
			player.gravity = 0f;
		}
	}
}