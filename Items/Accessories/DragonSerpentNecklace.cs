using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AAMod;
using AAModEXAI;
using AAModEXAI.Dusts;

namespace AAModEXAI.Items.Accessories
{
    public class DragonSerpentNecklace : BaseAAItem
    {
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dragon Serpent Necklace");
            Tooltip.SetDefault(@"10% increased damage and damage resistance
Ignores 20 Enemy defense
Cause 30 extra damage to Enemies");
        }
        public override void SetDefaults()
        {
            item.width = 58;
            item.height = 54;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = 2;
            item.accessory = true;
            item.expert = true; 
            item.expertOnly = true;
            item.defense = 3;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<AAMod.Items.Boss.Broodmother.DragonCape>(), 1);
            recipe.AddIngredient(ModContent.ItemType<AAMod.Items.Boss.Hydra.HydraPendant>(), 1);
            recipe.AddIngredient(ItemID.SharkToothNecklace, 1);
            recipe.AddIngredient(ModContent.ItemType<EXSoul>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateEquip(Player player)
        {
            player.endurance += .1f;
            player.allDamage += .1f;
            player.GetModPlayer<AAModEXPlayer>().DragonSerpentNecklace = true;
            player.armorPenetration += 20;
        }
    }
}
