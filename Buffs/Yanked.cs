using Terraria;
using Terraria.ModLoader;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Buffs
{
    public class Yanked : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Yanked");
            Description.SetDefault("'YOU AREN'T GOING ANYWHERE, YOU LITTLE SISSY!'");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.wingTime = 0;
            player.velocity.Y += 10;
            player.GetModPlayer<AAModEXPlayer>().Yanked = true;
        }
    }
}
