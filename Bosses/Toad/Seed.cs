/*
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Toad
{
    public class Seed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Waterleaf Seed");
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 200;
            projectile.aiStyle = 1;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        { 
            Collision.HitTiles(projectile.position, oldVelocity, projectile.width, projectile.height);
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1);
            NPC.NewNPC((int)projectile.Top.X, (int)projectile.Top.Y, mod.NPCType("ShroomGlow"), mod.ProjectileType("ShroomGlow"), projectile.damage, 0, projectile.owner, 0, 1);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<ShroomDust>(), 0f, 0f, 100, default, 1.2f);
                Main.dust[dustIndex].velocity *= 1.8f;
            }
        }
    }
}
*/