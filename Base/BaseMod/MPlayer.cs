using Terraria.ModLoader;
namespace AAModEXAI
{
    public class MPlayer : ModPlayer
    {
		public static bool useItem = false;

		public override void SetControls() 
		{
			if(useItem)
			{
				useItem = false;
				player.delayUseItem = false;
				player.controlUseItem = true;
			}
		}
    }
}