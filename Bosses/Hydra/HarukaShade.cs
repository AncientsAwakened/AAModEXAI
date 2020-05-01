using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using AAMod;

namespace AAModEXAI.Bosses.Hydra
{ 
    public class HarukaShade : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("...");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.defense = 1;
            npc.knockBackResist = 0f;
            npc.noGravity = false;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.damage = 0;
            npc.value = 0;
            npc.alpha = 255;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/ChaosSissy");
            npc.width = 38;
            npc.height = 58;
        }

        public override void AI()
        {
            npc.dontTakeDamage = true;
            if (Main.netMode != 1)
            {
                if (npc.velocity.Y == 0)
                {
                    npc.ai[1]++;
                    if (npc.ai[1] >= 120 && npc.ai[1] <= 240)
                    {
                        if (npc.alpha > 50)
                        {
                            npc.alpha -= 4;
                        }
                        else
                        {
                            npc.alpha = 50;
                        }
                    }
                    if (npc.ai[1] > 240)
                    {
                        if (npc.alpha < 255)
                        {
                            npc.alpha += 4;
                        }
                        else
                        {
                            npc.active = false;
                        }
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[1] >= 120 && npc.ai[1] <= 240)
            {
                npc.frame.Y = frameHeight;
            }
            else if (npc.ai[1] > 240)
            {
                npc.frame.Y = frameHeight * 2;
            }
            else
            {
                npc.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D tex = Main.npcTexture[npc.type];
            Texture2D tex2 = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Hydra/HarukaShade_Glow");
            BaseDrawing.DrawTexture(sb, tex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 3, npc.frame, npc.GetAlpha(dColor));
            if (npc.ai[1] >= 60 && npc.ai[1] < 240)
            {
                Lighting.AddLight(npc.Center, Color.MediumVioletRed.R / 180, Color.MediumVioletRed.G / 180, Color.MediumVioletRed.B / 180);
                BaseDrawing.DrawTexture(sb, tex2, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 3, npc.frame, Color.White);
            }
            return false;
        }
    }
}