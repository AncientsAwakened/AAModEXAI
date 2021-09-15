using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    public class SunSummon : ModProjectile
    {
        public override string Texture => "AAModEXAI/BlankTex";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sun Summon");
        }

        public override void SetDefaults()
        {
            projectile.width = 98;
            projectile.height = 98;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, 0, 0, ModContent.DustType<ForsakenDust>(), 0f, 0f, 0, default, 2f);
                Main.dust[num469].noGravity = true;
            }

            projectile.damage = 0;
            projectile.knockBack = 0;

            projectile.ai[1] = projectile.velocity.Length();

            projectile.velocity = projectile.velocity.RotatedBy(projectile.ai[1] / (2 * Math.PI * projectile.ai[0] * ++projectile.localAI[0]));

            projectile.ai[0]++;

            if (projectile.ai[0] > 60)
            {
                projectile.Kill();
            }
        }

        bool HitTile = false;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            HitTile = true;
            return base.OnTileCollide(oldVelocity);
        }

        public override void Kill(int timeLeft)
        {
            int MinionType = mod.NPCType("ForsakenSun");

            if (!HitTile)
            {
                int Minion = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, MinionType, 0);
                Main.npc[Minion].netUpdate = true;
            }
        }
    }
}
