using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AAMod;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
    public class YamataSoul : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mire Soul");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.ShadowFlameApparition);
            aiType = NPCID.ShadowFlameApparition;
            npc.aiStyle = NPCID.ShadowFlameApparition;
            animationType = NPCID.ShadowFlameApparition;
            npc.npcSlots = 0;
            npc.value = BaseUtility.CalcValue(0, 0, 0, 0);
            npc.aiStyle = 86;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noGravity = true;
            npc.damage = 200;
            npc.alpha = 255;

        }
        public override void AI()
        {
            Lighting.AddLight(npc.Center, AAColor.YamataA.R / 255, AAColor.YamataA.G / 255, AAColor.YamataA.B / 255);
            AAAI.AIShadowflameGhost(npc, ref npc.ai, false, 660f, 0.3f, 15f, 0.2f, 8f, 5f, 10f, 0.4f, 0.4f, 0.95f, 5f);
            if (!NPC.AnyNPCs(mod.NPCType("YamataA")))
            {
                npc.life = 0;
            }
            if (npc.alpha != 0)
            {
                for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                {
                    int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModLoader.GetMod("AAMod").DustType("YamataAuraDust"), 0f, 0f, 100, default, 2f);
                    Main.dust[num935].noGravity = true;
                    Main.dust[num935].noLight = true;
                }
            }
            npc.alpha -= 12;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
        }

        public Color GetGlowAlpha()
        {
            return new Color(200, 0, 50) * (Main.mouseTextColor / 255f);
        }
        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D glowTex = ModLoader.GetMod("AAMod").GetTexture("NPCs/Bosses/Yamata/Awakened/YamataSoul");
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc, dColor);
            BaseDrawing.DrawTexture(spritebatch, glowTex, 0, npc, GetGlowAlpha());
            BaseDrawing.DrawAfterimage(spritebatch, glowTex, 0, npc, 0.8f, 1f, 4, false, 0f, 0f, Color.White);
            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("HydraToxin"), 600);
            target.AddBuff(mod.BuffType("Yanked"), 120);
        }
    }
}
