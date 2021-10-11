using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAModEXAI.NewBiomes.SmallWorld
{
    public class EnterVoid : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Gate Creator");
            Tooltip.SetDefault("Sends all players to the Void");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.noUseGraphic = true;
            item.useAnimation = 45;
            item.useTime = 45;
            item.UseSound = SoundID.NPCDeath52;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = false;
        }
        public override bool UseItem(Player player)
        {
            if (!Subworld.AnyActive<AAModEXAI>())
            {
                Subworld.Enter<VoidSub>();
            }
            else
            {
                Subworld.Exit();
            }
            return true;
        }
    }
}