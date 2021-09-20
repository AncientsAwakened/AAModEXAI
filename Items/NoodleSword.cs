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
            DisplayName.SetDefault("desire beam test");
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
            int skip = Main.rand.Next(19) - 9;
            int skip2 = Main.rand.Next(19) - 9;
            int skip3 = Main.rand.Next(19) - 9;
            int skip4 = Main.rand.Next(19) - 9;
            for(int k = -9; k<=9; k++)
            {
                if(k == skip || k == skip2 || k == skip3 || k == skip4) continue;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int p = Projectile.NewProjectile(player.Center.X - 1000f, player.Center.Y + k * 70f, 10f, 0, mod.ProjectileType("DesireBeam"), 100, 1);
                    Main.projectile[p].ai[0] = 1f;
                    Main.projectile[p].netUpdate = true;
                }
            }

            return false;
        }
    }
}
