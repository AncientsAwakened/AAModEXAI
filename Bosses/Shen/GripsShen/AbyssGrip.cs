
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Shen.GripsShen
{
    [AutoloadBossHead]
    public class AbyssGrip : BaseShenGrips
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grip of Abyssal Wrath");
            Main.npcFrameCount[npc.type] = 14;
        }

	    public override void SetDefaults()
        {
			base.SetDefaults();
			npc.lifeMax = 70000;
			npc.damage = 150;
            npc.defense = 90;
            npc.boss = true;
            npc.buffImmune[BuffID.Poisoned] = true;

			offsetBasePoint = new Vector2(280f, 0f);
        }
		
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0) //this make so when the npc has 0 life(dead) he will spawn this
            {
                npc.position.X = npc.position.X + npc.width / 2;
                npc.position.Y = npc.position.Y + npc.height / 2;
                npc.width = 44;
                npc.height = 78;
                npc.position.X = npc.position.X - npc.width / 2;
                npc.position.Y = npc.position.Y - npc.height / 2;
                int dust1 = ModLoader.GetMod("AAMod").DustType("YamataDust");
                int dust2 = ModLoader.GetMod("AAMod").DustType("YamataDust");
                Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, dust1, 0f, 0f, 0);
                Main.dust[dust1].velocity *= 0.5f;
                Main.dust[dust1].scale *= 1.3f;
                Main.dust[dust1].fadeIn = 1f;
                Main.dust[dust1].noGravity = false;
                Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, dust2, 0f, 0f, 0);
                Main.dust[dust2].velocity *= 0.5f;
                Main.dust[dust2].scale *= 1.3f;
                Main.dust[dust2].fadeIn = 1f;
                Main.dust[dust2].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (npc.alpha > 0)
            {
                return AAColor.Yamata;
            }
            return lightColor;
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D glowTex = ModLoader.GetMod("AAMod").GetTexture("Glowmasks/AbyssGrip_Glow");
            int shader = 0;
            if (npc.ai[0] == 0)
            {
                shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingOceanDye);
            }
            if (npc.ai[0] != 0 || npc.ai[0] != 1 || npc.ai[0] != 5)
            {
                BaseDrawing.DrawAfterimage(spritebatch, Main.npcTexture[npc.type], 0, npc, 2, npc.scale, 7, true, 0, 0, Color.Indigo, npc.frame);
            }

            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], shader, npc, dColor);
            BaseDrawing.DrawTexture(spritebatch, glowTex, shader, npc, Color.White);
            return false;
        }

        public override bool PreNPCLoot()
        {
            return false;
        }


        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("HydraToxin"), 180);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = 0;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
    }
}
