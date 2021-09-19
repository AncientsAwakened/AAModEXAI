using Terraria;
using Terraria.ModLoader;

namespace AAModEXAI.DeBuffs
{
    public class BlazingMadness : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Akuma Battle Ground");
			Description.SetDefault(@"You must keep on moving and attacking
The damage you get will recover Akuma's life");
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