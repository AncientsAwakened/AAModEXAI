using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAModEXAI.Items.Accessories
{
    public class Energy_Conduit : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 24;
			item.value = Item.sellPrice(0, 6, 0, 0);
			item.rare = 8;
			item.accessory = true;
            
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Glowmasks/" + GetType().Name + "_Glow");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.moveSpeed += 0.5f;
            player.GetModPlayer<AAPlayer>().MaxMovespeedboost += 0.5f;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Conduit");
			Tooltip.SetDefault("50% increased movement speed");
			
		}
	}
}
