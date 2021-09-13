using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Zero.ZeroMinion
{
	public class ZeroSummonRune : ModNPC
    {
        public override string Texture => "AAMod/BlankTex";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The digital Portal");
        }

        public override void SetDefaults()
        {
            npc.alpha = 0;
            npc.dontTakeDamage = true;
            npc.lifeMax = 1;
            npc.damage = 0;
            npc.defense = Main.expertMode ? 1 : 1;
            npc.aiStyle = -1;
            npc.knockBackResist = 0.2f;
            npc.width = 10;
            npc.height = 10;
            npc.value = Item.buyPrice(0, 0, 0, 0);
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
        }

        public int timer = 0;

        public override void AI()
        {
            Texture2D tex = Main.npcTexture[(int)npc.ai[1]];
            Texture2D[] Number = new Texture2D[2];
            Number[0] = mod.GetTexture("Bosses/Zero/ZeroMinion/zero");
            Number[1] = mod.GetTexture("Bosses/Zero/ZeroMinion/one");
            if (npc.ai[0] * Number[0].Width > tex.Width && npc.ai[0] * Number[0].Height > tex.Height && timer <= 100)
            {
                if(timer < 100)
                {
                    timer ++;
                    return;
                }
                if(npc.ai[1] == mod.NPCType("ZeroTeslaTurrent") && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int a = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, (int)npc.ai[1]);
                    Main.npc[a].Center = npc.Center;
                    Main.npc[a].ai[1] = npc.Center.X;
                    Main.npc[a].ai[2] = npc.Center.Y;
                    Main.npc[a].ai[3] = npc.ai[3];
                }
                timer ++;
            }
            if(timer++ > 100)
            {
                if (((timer - 100) / 8) * Number[0].Width > tex.Width && ((timer - 100) / 8) * Number[0].Height > tex.Height)
                {
                    npc.active = false;
                    return;
                }
                return;
            }
            else if(timer > 8)
            {
                npc.ai[0] += 1f;
                timer = 0;
            }
            Lighting.AddLight(npc.Center, 0f, 0.85f, 0.9f);
        }

        public override bool PreDraw(SpriteBatch sb, Color drawColor)
        {
            Texture2D tex = Main.npcTexture[(int)npc.ai[1]];
            Texture2D[] Number = new Texture2D[2];
            Number[0] = mod.GetTexture("Bosses/Zero/ZeroMinion/zero");
            Number[1] = mod.GetTexture("Bosses/Zero/ZeroMinion/one");
            Color Alpha = AAColor.Oblivion;
            Alpha.R = (byte)((float)(255 - npc.alpha));
            Alpha.G = (byte)((float)(255 - npc.alpha));
            Alpha.B = (byte)((float)(255 - npc.alpha));
            Alpha.A = (byte)((float)(255 - npc.alpha));
            for(int i = 0 + (int)(timer > 100? (timer - 100) / 8 : 0); i * Number[0].Width < tex.Width && i * Number[0].Width < npc.ai[0] * Number[0].Width ; i++)
            {
                for(int j = 0 + (int)(timer > 100? (timer - 100) / 8 : 0); j * Number[0].Height  < tex.Height  && j * Number[0].Height < npc.ai[0] * Number[0].Height; j++)
                {
                    int n = Main.rand.Next(2);
                    //sb.Draw(Number[n], npc.Center + new Vector2(i * Number[0].Width - tex.Width / 2, j * Number[0].Height - tex.Height / 2) - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Number[n].Width, Number[n].Height)), npc.GetAlpha(drawColor), 0f, default, 0.6f, SpriteEffects.None, 0f);
                    BaseDrawing.DrawTexture(sb, Number[n], 0, npc.Center + new Vector2(i * Number[0].Width - tex.Width / 2, j * Number[0].Height - tex.Height / 2), Number[n].Width, Number[n].Height, 1f, 0f, 0, 1, new Rectangle(0, 0, Number[n].Width, Number[n].Height), Alpha, true);
                }
            }
            return false;
        }
    }
}