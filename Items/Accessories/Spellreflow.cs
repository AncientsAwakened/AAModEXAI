using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using AAMod;
using AAModEXAI;

namespace AAModEXAI.Items.Accessories
{
    public class Spellreflow : BaseAAItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spell Reflow");
            Tooltip.SetDefault(@"The fatal damage will be offset by your magic.");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = 8;
            item.accessory = true;
            item.expertOnly = true;
            item.expert = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<AAModEXPlayer>().Spellreflow = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.WormScarf, 1);
            recipe.AddIngredient(ModContent.ItemType<EXSoul>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}