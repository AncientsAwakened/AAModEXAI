using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.UI;
using Terraria.Utilities;

using static AAModEXAI.AAModEXAI;
using static Terraria.ModLoader.ModContent;

namespace AAModEXAI.Loaders
{
    public class CoreLoadMethods
    {
        [LoadThis]
        public static void LoadMisc()
        {
            activeRumbleSounds = new List<SoundEffectInstance>();

            instance.Logger.InfoFormat("{0} AAModEXAI log", instance.Name);

            if (Main.rand == null)
                Main.rand = new UnifiedRandom();
        }

        [LoadThis(true)]
        public static void LoadSubWorld()
        {
            SubworldLibrary.Load();
        }

        [LoadThis(true)]
        public static void LoadShaders()
        {
            Ref<Effect> screenRef = new Ref<Effect>(instance.GetEffect("Effects/Shockwave"));
            Filters.Scene["AAModEXAI:Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
            Filters.Scene["AAModEXAI:Shockwave"].Load();
        }
    }
}
