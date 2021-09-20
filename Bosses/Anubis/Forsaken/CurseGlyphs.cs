using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Anubis.Forsaken
{
    public class CurseGlyphs : ModProjectile
	{				
		public override void SetStaticDefaults()
		{
            Main.projFrames[projectile.type] = 9;
		}

        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
        }

        public int body = -1;
		public float rotValue = -1f;
		public bool spawnedDust = false;

		public override void AI()
        {
            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(projectile.Center, ModContent.NPCType<ForsakenAnubis>(), 400f, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;
            NPC anubis = Main.npc[body];
            if (anubis == null || anubis.life <= 0 || !anubis.active || anubis.type != ModContent.NPCType<ForsakenAnubis>()) { projectile.active = false; return; }

            projectile.rotation += .1f;

            int glyph = ((ForsakenAnubis)anubis.modNPC).RuneCount;

            if (rotValue == -1f) rotValue = projectile.ai[0] % glyph * ((float)Math.PI * 2f / glyph);
            rotValue += 0.04f;
            while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;

            projectile.Center = BaseUtility.RotateVector(anubis.Center, anubis.Center + new Vector2(130, 0f), rotValue);

            projectile.rotation = 0;

            for (int m = projectile.oldPos.Length - 1; m > 0; m--)
            {
                projectile.oldPos[m] = projectile.oldPos[m - 1];
            }
            projectile.oldPos[0] = projectile.position;
            ReflectProjectiles(projectile.Hitbox);
        }

        public void ReflectProjectiles(Rectangle myRect)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].friendly && !Main.projectile[i].hostile)
                {
                    Rectangle hitbox = Main.projectile[i].Hitbox;
                    if (myRect.Intersects(hitbox) && Main.rand.Next(10) == 0)
                    {
                        Main.PlaySound(SoundID.Dig, Main.projectile[i].position);
                        for (int j = 0; j < 3; j++)
                        {
                            int num = Dust.NewDust(Main.projectile[i].position, Main.projectile[i].width, Main.projectile[i].height, ModContent.DustType<ForsakenDust>(), 0f, 0f, 0, default, 1f);
                            Main.dust[num].velocity *= 0.3f;
                        }
                        Main.projectile[i].hostile = true;
                        Main.projectile[i].friendly = false;
                        Vector2 vector = Main.player[Main.projectile[i].owner].Center - Main.projectile[i].Center;
                        vector.Normalize();
                        vector *= Main.projectile[i].oldVelocity.Length();
                        Vector2 reflectvelocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        reflectvelocity.Normalize();
                        reflectvelocity *= vector.Length();
                        reflectvelocity += vector * 20f;
                        reflectvelocity.Normalize();
                        reflectvelocity *= 25f;
                        Main.projectile[i].penetrate = 1;
                        Main.projectile[i].GetGlobalProjectile<AAModEXAIGlobalProjectile>().reflectvelocity = reflectvelocity;
                        Main.projectile[i].GetGlobalProjectile<AAModEXAIGlobalProjectile>().isReflecting = true;
                        Main.projectile[i].GetGlobalProjectile<AAModEXAIGlobalProjectile>().ReflectConter = 180;
                    }
                }
            }
        }

		public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 9, 0, 0);
            BaseDrawing.DrawAfterimage(sb, Main.projectileTexture[projectile.type], 0, projectile, 3f, 0.9f, 6, true, 0f, 0f, Color.White, frame, 9);
            return false;
		}		
	}
}