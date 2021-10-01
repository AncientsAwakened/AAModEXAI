using Terraria;
using Terraria.ID;
using AAMod;
using AAModEXAI.Dusts;
namespace AAModEXAI.Items
{
    public class AAUndowner : BaseAAItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("AA Undowner");
            Tooltip.SetDefault(@"Undowns all AA bosses.
Non-Consumable");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(0, 0, 0, 0);
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = ItemUseStyleID.HoldingUp;
        }

        public override bool UseItem(Player player)
        {
            AAModEXAIWorld.downedSisters = false;
            AAModEXAIWorld.downedAkuma = false;
            AAModEXAIWorld.downedAnubis = false;
            AAModEXAIWorld.AnubisAwakened = false;
            AAModEXAIWorld.downedAnubisA = false;
            AAModEXAIWorld.downedRajahsRevenge = false;
            AAModEXAIWorld.CRajahFirst = false;
            return true;
        }
    }
}
