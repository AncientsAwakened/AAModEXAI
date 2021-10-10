using System;
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
using AAModEXAI.Dusts;

namespace AAModEXAI
{
	public class AAModEXAI : Mod
	{
        [BypassAutoUnload] // Don't unload the instance automatically because it is needed until unloading finishes.
        public static AAModEXAI instance;

        [BypassAutoUnload]
        public static Type[] allTypesInAssembly;

        public static List<SoundEffectInstance> activeRumbleSounds;

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
            WeakReferences.PerformModSupport();
        }
	}
}