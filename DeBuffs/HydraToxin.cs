using Terraria;
using Terraria.ModLoader;
namespace AAModEXAI.DeBuffs
{
    public class HydraToxin : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Hydra Toxin");
			Description.SetDefault("The longer you have it, the faster it festers and eats you away");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.GetModPlayer<AAModEXPlayer>().hydraToxin = true;
        }
	}
}
