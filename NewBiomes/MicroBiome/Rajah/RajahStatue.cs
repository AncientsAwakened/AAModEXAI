
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;

namespace AAModEXAI.NewBiomes.MicroBiome.Rajah
{
    public class RajahStatue : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            dustType = DustID.Gold;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.Origin = new Point16(0, 4);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 18};
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Vishnu Rajah Statue");
            AddMapEntry(new Color(150, 100, 0), name);
            disableSmartCursor = true;
        }

        public override bool NewRightClick(int i, int j)
        {
            BaseUtility.Chat(Trans.text("Common", "RajahStatueInfo"), Color.Goldenrod);
            return true;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if(Main.SmartInteractTileCoordsSelected.Contains(new Microsoft.Xna.Framework.Point(j, i)))
            {
                Tile tile = Main.tile[i, j];
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                int height = tile.frameY == 36 ? 18 : 16;
                Main.spriteBatch.Draw(mod.GetTexture("NewBiomes/MicroBiome/Rajah/RajahStatue_Highlight"), new Vector2((i * 16) - (int)Main.screenPosition.X, (j * 16) - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);   
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<MicroBiome.Rajah.RajahStatueBlock>());
        }

    }
}