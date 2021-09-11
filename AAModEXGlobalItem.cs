using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;

namespace AAModEXAI
{
    public class AAModGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;

        public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
            AAModEXPlayer AAplayer = player.GetModPlayer<AAModEXPlayer>();
            if(item == ItemLoader.GetWing(player) && AAplayer.ForbiddenTele && (player.inventory[player.selectedItem].type == ItemID.RodofDiscord || player.HeldItem.type == ItemID.RodofDiscord))
            {
                ascentWhenFalling += 0.2f;
                ascentWhenRising += 0.2f;
                maxCanAscendMultiplier += 0.2f;
                maxAscentMultiplier += 0.2f;
                constantAscend += 0.2f;
            }
		}

        public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
		{
			AAModEXPlayer AAplayer = player.GetModPlayer<AAModEXPlayer>();
            if(AAplayer.ForbiddenTele && player.wings > 0 && (player.inventory[player.selectedItem].type == ItemID.RodofDiscord || player.HeldItem.type == ItemID.RodofDiscord))
            {
                speed += 3f;
			    acceleration += 0.2f;
            }
		}
    }
}

