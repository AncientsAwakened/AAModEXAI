using Microsoft.Xna.Framework.Audio;

using Terraria.ModLoader;

namespace AAModEXAI.Sounds.Sounds
{
    public class AkumaRoar : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            if (soundInstance.State == SoundState.Playing)
                return null;
            soundInstance.Volume = volume * 1f;
            return soundInstance;
        }
    }
}