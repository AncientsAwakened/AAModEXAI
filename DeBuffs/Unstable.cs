using Terraria;
using Terraria.ModLoader;

using AAModEXAI.Dusts;

namespace AAModEXAI.DeBuffs
{
    public class Unstable : ModBuff
	  {
      public override void SetDefaults()
      {
          DisplayName.SetDefault("Unstable");
          Description.SetDefault("Your character's code has become destablized");
          Main.debuff[Type] = true;
          Main.pvpBuff[Type] = true;
          Main.buffNoSave[Type] = true;
      }

      public override void Update(Player player, ref int buffIndex)
      {
          player.GetModPlayer<AAModEXPlayer>().Unstable = true;
      }
	}
}