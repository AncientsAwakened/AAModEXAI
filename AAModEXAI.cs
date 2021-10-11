using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.Localization;

using AAModEXAI.Loaders;

namespace AAModEXAI
{
	public class AAModEXAI : Mod
	{
        [BypassAutoUnload] // Don't unload the instance automatically because it is needed until unloading finishes.
        public static AAModEXAI instance;

        [BypassAutoUnload]
        public static Type[] allTypesInAssembly;

        public static List<SoundEffectInstance> activeRumbleSounds;

        //SubWorldUI
        internal UserInterface SubWorldInterface;

		public override void Load()
        {
            Loader.Load();
		}

		public override void Unload()
        {
            Unloader.Unload();
		}

        public override void PreSaveAndQuit()
        {
            foreach (SoundEffectInstance sound in activeRumbleSounds)
            {
                sound.Stop();
            }
            activeRumbleSounds.Clear();
        }

        public override void PostSetupContent()
        {
            
        }

        public override void AddRecipes()
		{
			SubworldLibrary.AddRecipes();
		}

        public override void UpdateUI(GameTime gameTime)
		{
			SubworldLibrary.UpdateUI(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			SubworldLibrary.ModifyInterfaceLayers(layers);
		}

        public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
            SubworldLibrary.HandlePacket(reader, whoAmI);
        }

        public override object Call(params object[] args)
		{
            var sub = SubworldLibrary.Call(args);

            if(sub != null) return sub;
            
            return null;
        }
	}
}