using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using AAMod;

namespace AAModEXAI.Bosses.AH.Haruka
{
    [AutoloadBossHead]
    public class HarukaClone : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Haruka Yamata");
        }

        public override void SetDefaults()
        {
            npc.width = 50;
            npc.height = 60;
            npc.friendly = false;
            npc.dontTakeDamage = true;
            npc.damage = 150;
            npc.defense = 9999;
            npc.lifeMax = 150000;
            npc.HitSound = SoundID.NPCHit1;
            npc.knockBackResist = 0f;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.lavaImmune = true;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.9f);
        }

        public override void AI()
        {
            NPC Haruka = Main.npc[(int)npc.ai[0]];
            if(!Main.npc[(int)npc.ai[0]].active || Main.npc[(int)npc.ai[0]].life <= 0)
            {
                npc.life = 0;
                npc.active = false;
            }
            bool despawn = false;
            if(Haruka.type = mod.NPCType("Haruka"))
            {
                despawn = ((Haruka)Haruka.modNPC).internalAI[0] != 4;
            }
            else if(Haruka.type = mod.NPCType("HarukaY"))
            {
                despawn = ((HarukaY)Haruka.modNPC).internalAI[0] != 4;
            }
            if(despawn)
            {
                npc.boss = false;
                npc.life = 0;
                npc.active = false;
            }
        }


        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            if (npc.velocity != Vector2.Zero)
            {
                BaseDrawing.DrawAfterimage(spritebatch, Main.npcTexture[npc.type], 0, npc, 1.5f, 1f, 3, false, 0f, 0f, Color.Navy);
            }
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 1, npc.frame, npc.GetAlpha(dColor), false);
            return false;
        }
    }
}