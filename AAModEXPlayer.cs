using Terraria.ModLoader;
using Terraria;
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
        public override void UpdateBiomes()
        {
            AAMod.AAPlayer modPlayer = player.GetModPlayer<AAMod.AAPlayer>();

            modPlayer.ZoneMire = BaseAI.GetNPC(player.Center, ModContent.NPCType<Yamata>(), 5000) != -1 || BaseAI.GetNPC(player.Center, ModContent.NPCType<YamataA>(), 5000) != -1;
            modPlayer.ZoneInferno = BaseAI.GetNPC(player.Center, ModContent.NPCType<Akuma>(), 5000) != -1 || BaseAI.GetNPC(player.Center, ModContent.NPCType<AkumaA>(), 5000) != -1;
            modPlayer.ZoneVoid = BaseAI.GetNPC(player.Center, ModContent.NPCType<Zero>(), 5000) != -1 || BaseAI.GetNPC(player.Center, ModContent.NPCType<ZeroProtocol>(), 5000) != -1;
        }

		public override void UpdateBiomeVisuals()
        {
            bool useAthena = NPC.AnyNPCs(ModContent.NPCType<AthenaA>());
            bool useShenA = NPC.AnyNPCs(ModContent.NPCType<ShenA>());
            bool useShen = NPC.AnyNPCs(ModContent.NPCType<Shen>()) && !useShenA;
            bool useAkuma = NPC.AnyNPCs(ModContent.NPCType<AkumaA>());
            bool useYamata = NPC.AnyNPCs(ModContent.NPCType<YamataAHead>());
            bool useAnu = NPC.AnyNPCs(ModContent.NPCType<ForsakenAnubis>());

            player.ManageSpecialBiomeVisuals("AAMod:AnubisSky", useAnu);
            player.ManageSpecialBiomeVisuals("AAMod:AthenaSky", useAthena);
            player.ManageSpecialBiomeVisuals("AAMod:ShenSky", useShen);
            player.ManageSpecialBiomeVisuals("AAMod:ShenASky", useShenA);
            player.ManageSpecialBiomeVisuals("AAMod:AkumaSky", useAkuma);
            player.ManageSpecialBiomeVisuals("AAMod:YamataSky", useYamata);
        }
	}
}