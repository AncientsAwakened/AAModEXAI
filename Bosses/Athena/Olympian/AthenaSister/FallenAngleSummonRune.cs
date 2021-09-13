using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Athena.Olympian.AthenaSister
{
	public class FallenAngleSummonRune : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fallen Angle Summon Rune");
        }

        public override void SetDefaults()
        {
            npc.alpha = 255;
            npc.dontTakeDamage = true;
            npc.lifeMax = 1;
            npc.aiStyle = 0;
            npc.damage = 0;
            npc.defense = Main.expertMode ? 1 : 1;
            npc.knockBackResist = 0.2f;
            npc.width = 82;
            npc.height = 82;
            npc.value = Item.buyPrice(0, 0, 0, 0);
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
        }

        public override void AI()
        {
            if (npc.localAI[1] == 0f)
            {
                Main.PlaySound(SoundID.Item121, npc.position);
                npc.localAI[1] = 1f;
            }
            if (npc.ai[0] < 360f)
            {
                npc.alpha -= 5;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
            }
            else if(npc.ai[0] == 360f)
            {

                int boss = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("VariaFallenAngel"));
                Main.npc[boss].alpha = 255;
                Main.npc[boss].Center = npc.Center;
                ((VariaFallenAngel)Main.npc[boss].modNPC).AthenaA = npc.ai[1];
            }
            else
            {
                npc.alpha += 5;
                if (npc.alpha > 255)
                {
                    npc.alpha = 255;
                    npc.active = false;
                    return;
                }
            }
            npc.ai[0] += 1f;
            Lighting.AddLight(npc.Center, 0f, 0.85f, 0.9f);
        }

        public override bool PreDraw(SpriteBatch sb, Color drawColor)
        {
            BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 7, npc.frame, npc.GetAlpha(ColorUtils.COLOR_GLOWPULSE), true);
            return false;
        }
    }
}