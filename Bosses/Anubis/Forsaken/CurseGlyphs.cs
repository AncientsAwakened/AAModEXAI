using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
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
                int npcID = BaseAI.GetNPC(projectile.Center, mod.NPCType("ForsakenAnubis"), 400f, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;
            NPC anubis = Main.npc[body];
            if (anubis == null || anubis.life <= 0 || !anubis.active || anubis.type != mod.NPCType("ForsakenAnubis")) { projectile.active = false; return; }

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
        }

		public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 9, 0, 0);
            BaseDrawing.DrawAfterimage(sb, Main.projectileTexture[projectile.type], 0, projectile, 3f, 0.9f, 6, true, 0f, 0f, Color.White, frame, 9);
            return false;
		}		
	}
}