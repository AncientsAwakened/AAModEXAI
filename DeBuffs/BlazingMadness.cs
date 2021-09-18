using Terraria;
using Terraria.ModLoader;

namespace AAModEXAI.DeBuffs
{
    public class BlazingMadness : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Blazing Madness");
			Description.SetDefault("You must keep moving and attacking");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            AAModEXPlayer mp = player.GetModPlayer<AAModEXPlayer>();
            mp.BlazingMadness = true;
        }
	}
}