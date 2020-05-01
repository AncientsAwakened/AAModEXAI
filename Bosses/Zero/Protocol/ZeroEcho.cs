using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using Terraria.Graphics.Shaders;
using AAMod;

namespace AAModEXAI.Bosses.Zero.Protocol
{
    public class ZeroEcho : ModNPC
    {
        public override string Texture => "AAMod/NPCs/Bosses/Zero/Protocol/ZeroProtocol";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("3CH0");
            Main.npcFrameCount[npc.type] = 7; 
        }
        public override void SetDefaults()
        {
            npc.lifeMax = 6000;
            npc.damage = 110;
            npc.defense = 70;
            npc.knockBackResist = 0f;
            npc.width = 146;
            npc.height = 152;
            npc.friendly = false;
            npc.aiStyle = -1;
            npc.value = Item.sellPrice(0, 0, 0, 0);
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.NPCHit, "Sounds/Sounds/Zerohit");
            npc.DeathSound = ModLoader.GetMod("AAMod").GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/ZeroDeath");
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return AAColor.Oblivion;
        }

        int body = -1;

        public override void AI()
        {
            if(npc.ai[0] ++ == 5)
            {
                SpawnDust();
            }

            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(npc.Center, mod.NPCType("ZeroProtocol"), -1, null);
                if (npcID >= 0) body = npcID;
            }

            if (body == -1) return;

            NPC zero = Main.npc[body];
            if (zero == null || zero.life <= 0 || !zero.active || zero.type != mod.NPCType("ZeroProtocol")) { npc.active = false; return; }

            if (zero.ai[1] == 1f && Main.netMode != 1)
            {
                Projectile.NewProjectile(npc.Center + new Vector2(30, 30), new Vector2(10, 10), mod.ProjectileType("EchoRay"), 70, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center + new Vector2(-30, 30), new Vector2(-10, 10), mod.ProjectileType("EchoRay"), 70, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center + new Vector2(30, -30), new Vector2(10, -10), mod.ProjectileType("EchoRay"), 70, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center + new Vector2(-30, -30), new Vector2(-10, -10), mod.ProjectileType("EchoRay"), 70, 0f, Main.myPlayer, 0, npc.whoAmI);
            }

            if (zero.ai[0] != 4)
            {
                npc.life = 0;
                SpawnDust();
            }
        }

        int Frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 3)
            {
                npc.frameCounter = 0;
                Frame += 1;
            }

            if (Frame > 6)
            {
                Frame = 0;
            }

            npc.frame.Y = frameHeight * Frame;
        }

        public void SpawnDust()
        {
            Vector2 position = npc.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, 226, 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num86].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 7; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num88].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num88].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Black * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * 8f;
            }
            for (int num89 = 0; num89 < 5; num89++)
            {
                int num90 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num90].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num90].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += npc.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 15; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, 226, 0, 0, 100, new Color(), 2f);
                Main.dust[num92].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                Main.dust[num92].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += npc.DirectionTo(Main.dust[num92].position) * 3f;
            }
        }
    }
}
