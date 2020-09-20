using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using AAMod;
using AAModEXAI;
using System.Collections.Generic;

namespace AAModEXAI.Items.Accessories
{
    public class ForbiddenTeleCharm : BaseAAItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Space Charm");
            Tooltip.SetDefault(@"Press the accessory ability key to teleport you to the position of the mouse
Holding on the Rod of Discord will increase your movement stat");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = 8;
            item.defense = 0;
            item.accessory = true;
            item.expertOnly = true;
            item.expert = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<AAModEXPlayer>().ForbiddenTele = true;

            if(player.inventory[player.selectedItem].type == ItemID.RodofDiscord)
            {
                player.moveSpeed += .4f;
                player.GetModPlayer<AAPlayer>().MaxMovespeedboost += 0.4f;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<AAMod.Items.Boss.Greed.StoneShell>(), 20);
            recipe.AddIngredient(ModContent.ItemType<AAMod.Items.Boss.Athena.Olympian.StormSphere>(), 20);
            recipe.AddIngredient(ModContent.ItemType<AAMod.Items.Boss.Anubis.Forsaken.SoulFragment>(), 20);
            recipe.AddIngredient(ModContent.ItemType<EXSoul>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}