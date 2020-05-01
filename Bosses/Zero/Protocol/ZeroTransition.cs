
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    public class ZeroTransition : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Rift");
            Main.npcFrameCount[npc.type] = 26;
            Terraria.ID.NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }
        public override void SetDefaults()
        {
            npc.width = 146;
            npc.height = 150;
            npc.friendly = false;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.timeLeft = 10;
            npc.alpha = 255;
            music = ModLoader.GetMod("AAMod").GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 8, npc.frame, npc.GetAlpha(lightColor), true);
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 8, npc.frame, AAColor.Oblivion, true);
            return false;
        }

        public override void AI()
        {
			npc.TargetClosest();			
            Player player = Main.player[npc.target];

            npc.ai[0]++;
            
            if (npc.ai[0] % 5 == 0)
            {
                npc.frame.Y += 152;
            }
            if (npc.ai[0] >= 130)
            {
                npc.frame.Y = 152 * 25;
            }
            if (npc.ai[0] >= 135 && !NPC.AnyNPCs(mod.NPCType("ZeroProtocol")) && Main.netMode != 1)
            {
                AAModGlobalNPC.SpawnBoss(player, mod.NPCType("ZeroProtocol"), false, npc.Center, "", false);

                int b = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("ShockwaveBoom"), 0, 1, Main.myPlayer, 0, 0);
                Main.projectile[b].Center = npc.Center;

                npc.netUpdate = true;
                npc.active = false;
            }
        }

        public override bool CheckActive()
        {
            if (!NPC.AnyNPCs(mod.NPCType("ZeroProtocol")))
            {
                return false;
            }
            return true;
        }

    }
}