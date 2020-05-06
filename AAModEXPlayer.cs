using Terraria.ModLoader;
using Terraria;
using AAModEXAI.Buffs;
using AAModEXAI.Bosses.Akuma;
using AAModEXAI.Bosses.Akuma.Awakened;
using AAModEXAI.Bosses.Athena;
using AAModEXAI.Bosses.Athena.Olympian;
using AAModEXAI.Bosses.Shen;
using AAModEXAI.Bosses.Yamata;
using AAModEXAI.Bosses.Yamata.Awakened;
using AAModEXAI.Bosses.Zero;
using AAModEXAI.Bosses.Zero.Protocol;
using AAModEXAI.Bosses.Anubis.Forsaken;

using AAMod;

namespace AAModEXAI
{
	public class AAModEXPlayer : ModPlayer
	{
        public bool AkumaPain = false;
        public bool YamataGravity = false;
        public bool YamataAGravity = false;
        public bool Yanked = false;
        public bool Unstable = false;

        public override void OnEnterWorld(Player player)
		{
			Main.NewText("WARNING: SUPER AAMOD AI Detected. It will influence your playthrough seriously. If you are a new AA player, it is not recommended to experience it. ", 67, 110, 238, false);
            Main.NewText("GLHF.", 67, 110, 238, false);
		}

        public override void ResetEffects()
        {
            AkumaPain = false;
            YamataGravity = false;
            YamataAGravity = false;
            Yanked = false;
            Unstable = false;
        }

        public override void UpdateBadLifeRegen()
        {
            AAMod.AAPlayer modPlayer = player.GetModPlayer<AAMod.AAPlayer>();
            if (AkumaPain)
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;

                if ((player.onFire || player.frostBurn || player.onFire2 || modPlayer.dragonFire || modPlayer.discordInferno) && player.lifeRegen < 0)
                {
                    player.lifeRegen *= 2;
                }
            }
        }

        public override void PostUpdateRunSpeeds()
		{
            if (YamataGravity || YamataAGravity)
            {
                if (player.mount.CanFly)
                {
                    player.mount.Dismount(player);
                }

                if (player.wingTimeMax > 200) player.wingTimeMax = 200;
                player.wingTimeMax /= 2;
                if (player.wingTime > player.wingTimeMax)
                    player.wingTime = player.wingTimeMax;


                if (YamataAGravity)
                {
                    player.moveSpeed *= .58f;
                    player.maxRunSpeed *= .58f;
                    player.accRunSpeed *= .58f;
                }
            }

            if (Unstable)
            {
                bool flag = player.controlLeft;
                bool flag1 = player.controlUp || player.controlJump;
                player.controlLeft = player.controlRight;
                player.controlRight = flag;
                player.controlUp = player.controlDown;
                player.controlJump = player.controlDown;
                player.controlDown = flag1;
                
                player.moveSpeed *= Utils.NextFloat(Main.rand, 0.25f, 2f);
            }
		}

        public override void UpdateBiomes()
        {
            AAMod.AAPlayer modPlayer = player.GetModPlayer<AAMod.AAPlayer>();

            modPlayer.ZoneMire = AAWorld.mireTiles > 100 || BaseAI.GetNPC(player.Center, ModContent.NPCType<Yamata>(), 5000) != -1 || BaseAI.GetNPC(player.Center, ModContent.NPCType<YamataA>(), 5000) != -1;
            modPlayer.ZoneInferno = AAWorld.infernoTiles > 100 || BaseAI.GetNPC(player.Center, ModContent.NPCType<Akuma>(), 5000) != -1 || BaseAI.GetNPC(player.Center, ModContent.NPCType<AkumaA>(), 5000) != -1;
            modPlayer.ZoneVoid = (AAWorld.voidTiles > 20 && player.ZoneSkyHeight) || BaseAI.GetNPC(player.Center, ModContent.NPCType<Zero>(), 5000) != -1 || BaseAI.GetNPC(player.Center, ModContent.NPCType<ZeroProtocol>(), 5000) != -1;
        }

		public override void UpdateBiomeVisuals()
        {
            AAMod.AAPlayer modPlayer = player.GetModPlayer<AAMod.AAPlayer>();

            bool Underground = player.Center.Y > Main.worldSurface * 16;

            bool useAthena = NPC.AnyNPCs(ModContent.NPCType<AthenaA>());
            bool useShenA = NPC.AnyNPCs(ModContent.NPCType<ShenA>());
            bool useShen = NPC.AnyNPCs(ModContent.NPCType<Shen>()) && !useShenA;
            bool useAkuma = NPC.AnyNPCs(ModContent.NPCType<AkumaA>());
            bool useYamata = NPC.AnyNPCs(ModContent.NPCType<YamataAHead>());
            bool useAnu = NPC.AnyNPCs(ModContent.NPCType<ForsakenAnubis>());
            bool useMire = (modPlayer.ZoneMire || modPlayer.MoonAltar) && !useYamata && !useShen && !useShenA && !useAnu;
            bool useInferno = (modPlayer.ZoneInferno || modPlayer.SunAltar) && !useAkuma && !useShen && !useShenA && !useAnu;
            bool useVoid = (modPlayer.ZoneVoid || modPlayer.VoidUnit) && !useShen && !useShenA && !useAnu;

            player.ManageSpecialBiomeVisuals("AAMod:AnubisSky", useAnu);
            player.ManageSpecialBiomeVisuals("AAMod:AthenaSky", useAthena);
            player.ManageSpecialBiomeVisuals("AAMod:ShenSky", useShen);
            player.ManageSpecialBiomeVisuals("AAMod:ShenASky", useShenA);
            player.ManageSpecialBiomeVisuals("AAMod:AkumaSky", useAkuma);
            player.ManageSpecialBiomeVisuals("AAMod:YamataSky", useYamata);

            if (!Underground)
            {
                player.ManageSpecialBiomeVisuals("AAMod:InfernoSky", useInferno);
                player.ManageSpecialBiomeVisuals("AAMod:MireSky", useMire);
            }

            if (Main.UseHeatDistortion)
            {
                player.ManageSpecialBiomeVisuals("HeatDistortion", useAkuma || useInferno);
            }

            player.ManageSpecialBiomeVisuals("AAMod:VoidSky", useVoid);
        }

        public override void PostUpdate()
        {

            if (NPC.AnyNPCs(mod.NPCType("AkumaTransition")))
            {
                int n = BaseAI.GetNPC(player.Center, mod.NPCType("AkumaTransition"), -1);
                NPC akuma = Main.npc[n];

                if (akuma.ai[0] >= 660)
                {
                    player.AddBuff(mod.BuffType("BlazingPain"), 2);
                }
            }
            else if (NPC.AnyNPCs(mod.NPCType("AkumaA")))
            {
                player.AddBuff(mod.BuffType("BlazingPain"), 2);
            }

            if (NPC.AnyNPCs(mod.NPCType("Yamata")))
            {
                player.AddBuff(mod.BuffType("YamataGravity"), 10);
            }

            if (NPC.AnyNPCs(mod.NPCType("YamataA")))
            {
                player.AddBuff(mod.BuffType("YamataAGravity"), 10);
            }

            
            if (ModSupport.GetMod("CalamityMod") != null)
            {
                bool revenge = (bool)ModSupport.GetModWorldConditions("CalamityMod", "CalamityWorld", "revenge", false, true);
                bool Death = (bool)ModSupport.GetModWorldConditions("CalamityMod", "CalamityWorld", "death", false, true);

                if (revenge && (NPC.AnyNPCs(ModContent.NPCType<Shen>()) || NPC.AnyNPCs(ModContent.NPCType<ShenA>())))
                {
                    player.AddBuff(mod.BuffType("YamataAGravity"), 10);
                    player.AddBuff(mod.BuffType("BlazingPain"), 10);
                }
            }
        }
	}
}