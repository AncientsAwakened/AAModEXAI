using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

using AAMod;

namespace AAModEXAI.Items
{
    public class NoodleSword : BaseAAItem
    {
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zero DeathRay test");
            Tooltip.SetDefault(@"Top 10 op weapons in video games");
        }

        public override void SetDefaults()
        {
            item.damage = 10000;  
            item.melee = true; 
            item.width = 64;    
            item.height = 70; 
            item.useTime = 17;
            item.useAnimation = 17;  
            item.useStyle = ItemUseStyleID.SwingThrow;    
            item.knockBack = 4;
            item.value = 0;
            item.rare = ItemRarityID.Purple;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
            item.expert = true; item.expertOnly = true;
			item.shoot = 1;
			item.shootSpeed = 9f;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Main.itemTexture[item.type];
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

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int dirX = player.velocity.X > 0? 1:-1;
            int dirY = player.velocity.Y > 0? 1:-1;

            int xPos = Math.Abs(player.velocity.X) > 4f? -100 * dirX : -300 * dirX;
            int yPos = Math.Abs(player.velocity.Y) > 4f? -100 * dirY : -300 * dirY;

            int a1 = Projectile.NewProjectile(new Vector2(player.Center.X, player.Center.Y), Vector2.Zero, ModContent.ProjectileType<Bosses.Zero.Protocol.Blast>(), damage, 3, Main.myPlayer, 2f, 0f);
            Main.projectile[a1].Center = player.Center + new Vector2(xPos, yPos);

            return false;
        }
    }
}
