using Terraria.ModLoader;
using Terraria.ID;

namespace AAModEXAI.NewBiomes.MicroBiome.Rajah
{
    public class RajahStatueBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vishnu Rajah Statue");
        }

        public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 50000;
			item.rare = ItemRarityID.White;
			item.createTile = ModContent.TileType<RajahStatue>();
			item.placeStyle = 0;
        }
    }
}