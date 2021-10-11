using Microsoft.Xna.Framework;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.World.Generation;

namespace AAModEXAI
{
    public class VoidSub : Subworld
    {
        public override int width => 500;
        public override int height => 940;
        public override ModWorld modWorld => ModContent.GetInstance<AAModEXAIWorld>();
        public override bool noWorldUpdate => true;
        public override bool saveSubworld => true;
        public override List<GenPass> tasks => new List<GenPass>()
        {
            new SubworldGenPass(1f, progress =>
            {
                progress.Message = "Transporting...";
                Main.spawnTileX = 296;
                Main.spawnTileY = 177;
                Main.worldSurface = 5999; //Hides the underground layer just out of bounds
			    Main.rockLayer = 5999;
                VoidBiome();
            })
        };

        public void VoidBiome()
        {
            Mod mod = AAModEXAI.instance;
            Dictionary<Color, int> colorToTile = new Dictionary<Color, int>
            {
                [new Color(0, 0, 255)] = ModContent.TileType<NewBiomes.SmallWorld.Void.VoidTile.Doomstone>(),
                [new Color(0, 255, 0)] = ModContent.TileType<NewBiomes.SmallWorld.Void.VoidTile.DoomitePlate>(),
                [new Color(255, 255, 0)] = ModContent.TileType<NewBiomes.SmallWorld.Void.VoidTile.OroborosWoodNatural>(),
                //[new Color(255, 0, 0)] = ModContent.TileType<SmallWorld.Void.VoidTile.Apocalyptite>(),
                [new Color(255, 0, 255)] = ModContent.TileType<NewBiomes.SmallWorld.Void.VoidTile.DoomstoneBrick>(),
                [new Color(150, 150, 150)] = -2, //turn into air
                [Color.Black] = -1 //don't touch when genning
            };

            Dictionary<Color, int> colorToWall = new Dictionary<Color, int>
            {
                //[new Color(255, 255, 0)] = ModContent.WallType<Walls.OroborosWallTile>(),
                //[new Color(0, 255, 0)] = ModContent.WallType<Walls.Bricks.DoomiteWallTile>(),
                //[new Color(255, 0, 255)] = ModContent.WallType<Walls.Bricks.DoomstoneBrickWallTile>(),
                [Color.Black] = -1 //don't touch when genning
            };

            TexGen gen = BaseWorldGenTex.GetTexGenerator(mod.GetTexture("NewBiomes/SmallWorld/Void/VoidGen/VoidFull"), colorToTile, mod.GetTexture("NewBiomes/SmallWorld/Void/VoidGen/VoidFullWalls"), colorToWall, mod.GetTexture("NewBiomes/SmallWorld/Void/VoidGen/VoidFullWater"), null);
            gen.Generate(0, 0, true, true);


            /*
            Items = new List<int>
            {
                ModContent.ItemType<Voidsaber>(),
                ModContent.ItemType<DoomGun>(),
                ModContent.ItemType<DoomStaff>(),
                ModContent.ItemType<ProbeControlUnit>()
            };
            */
            /*
            VoidChest(349, 138, ModContent.TileType<OroborosChest>());
            VoidChest(216, 208, ModContent.TileType<OroborosChest>());
            VoidChest(145, 272, ModContent.TileType<OroborosChest>());
            VoidChest(348, 276, ModContent.TileType<OroborosChest>());
            */
        }

        #region Chest Contents

        public List<int> Items;

        public void VoidChest(int x, int y, int chestID)
        {
            /*
            int PlacementSuccess = WorldGen.PlaceChest(x, y, (ushort)ModContent.TileType<OroborosChest>(), true);
            if (PlacementSuccess >= 0)
            {
                Chest chest = Main.chest[PlacementSuccess];

                int choice = Main.rand.Next(Items.Count);

                int Item = Items[choice];

                VoidLoot(Item, chest);

                Items.Remove(choice);
            }
            */
        }

        public void VoidLoot(int Item, Chest chest)
        {
            /*
            chest.item[0].SetDefaults(Item, false);
            chest.item[1].SetDefaults(ModContent.ItemType<Items.Materials.DoomiteScrap>(), false);
            chest.item[1].stack = WorldGen.genRand.Next(4, 6);
            Item item = chest.item[2];
            UnifiedRandom genRand = WorldGen.genRand;
            int[] array2 = new int[]
            { 302, 2327, 2351, 304, 2329 };
            item.SetDefaults(Utils.Next(genRand, array2), false);
            chest.item[2].stack = WorldGen.genRand.Next(1, 3);
            chest.item[3].SetDefaults(Utils.Next(WorldGen.genRand, new int[]
            { 282, 286 }), false);
            chest.item[3].stack = WorldGen.genRand.Next(15, 31);
            chest.item[4].SetDefaults(73, false);
            chest.item[4].stack = WorldGen.genRand.Next(1, 3);
            */
        }

        #endregion

        public override void Load()
        {
            Main.dayTime = true;
            Main.time = 40000;
        }

        public override void Unload()
        {
            //SubworldCache.AddCache("AAMod", "AAWorld", "downedSag", AAWorld.downedSag, null);
            //SubworldCache.AddCache("AAMod", "AAWorld", "downedZero", AAWorld.downedZero, null);
        }
    }

    public class NotInSubworlds : ModWorld
    {
        public override void PostUpdate()
        {
            if (!SLWorld.subworld)
            {
                SubworldCache.UpdateCache();
            }
        }
    }

    public class GenUtils
    {
        public static void ObjectPlace(Point Origin, int x, int y, int TileType)
        {
            WorldGen.PlaceObject(Origin.X + x, Origin.Y + y, TileType);
            NetMessage.SendObjectPlacment(-1, Origin.X + x, Origin.Y + y, TileType, 0, 0, -1, -1);
        }
        public static void ObjectPlace(int x, int y, int TileType)
        {
            WorldGen.PlaceObject(x, y, TileType);
            NetMessage.SendObjectPlacment(-1, x, y, TileType, 0, 0, -1, -1);
        }
        public static void ObjectPlaceRand1(Point Origin, int x, int y, int TileType)
        {
            WorldGen.PlaceObject(Origin.X + x, Origin.Y + y, TileType, false, WorldGen.genRand.Next(3));
            NetMessage.SendObjectPlacment(-1, Origin.X + x, Origin.Y + y, TileType, WorldGen.genRand.Next(3), 0, -1, -1);
        }
        public static void ObjectPlaceRand1(int x, int y, int TileType)
        {
            WorldGen.PlaceObject(x, y, TileType, false, WorldGen.genRand.Next(3));
            NetMessage.SendObjectPlacment(-1, x, y, TileType, WorldGen.genRand.Next(3), 0, -1, -1);
        }
    }
}