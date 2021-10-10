using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace AAModEXAI.Base.AAStuff
{
    public static class ColorUtils
    {
        public static Color COLOR_GLOWPULSE => Color.White * (Main.mouseTextColor / 255f);
    }

    public static class MiscUtils
    {
        public static bool IsBunny(this NPC npc)
        {
            return npc.type == NPCID.Bunny || npc.type == NPCID.GoldBunny || npc.type == NPCID.BunnySlimed || npc.type == NPCID.BunnyXmas || npc.type == NPCID.PartyBunny;
        }

        public static bool IsEclipseEnemy(this NPC npc)
        {
            return npc.type == NPCID.Frankenstein || npc.type == NPCID.Vampire || npc.type == NPCID.VampireBat || npc.type == NPCID.SwampThing ||
                npc.type == NPCID.CreatureFromTheDeep || npc.type == NPCID.Fritz || npc.type == NPCID.Reaper || npc.type == NPCID.ThePossessed ||
                npc.type == NPCID.Mothron || npc.type == NPCID.Butcher || npc.type == NPCID.DeadlySphere || npc.type == NPCID.DrManFly ||
                npc.type == NPCID.Nailhead || npc.type == NPCID.Psycho || npc.type == NPCID.Eyezor;
        }

        public static bool IsPumpkinMoonEnemy(this NPC npc)
        {
            return (npc.type >= NPCID.Scarecrow1 && npc.type <= NPCID.HeadlessHorseman) || (npc.type >= NPCID.MourningWood && npc.type <= NPCID.Poltergeist && npc.type != NPCID.PumpkingBlade);
        }

        public static bool IsFrostMoonEnemy(this NPC npc)
        {
            return npc.type >= NPCID.ZombieElf && npc.type <= NPCID.Krampus;
        }

        public static bool IsMartianEnemy(this NPC npc)
        {
            return npc.type >= NPCID.BrainScrambler && npc.type <= NPCID.MartianSaucer && npc.type != NPCID.ForceBubble;
        }
    }

    public static class ItemUtils
    {
        public static void DropLoot(this Entity ent, int type, int stack = 1)
        {
            Item.NewItem(ent.Hitbox, type, stack);
        }

        public static void DropLoot(this Entity ent, int type, float chance)
        {
            if (Main.rand.NextDouble() < chance)
            {
                Item.NewItem(ent.Hitbox, type);
            }
        }

        public static void DropLoot(this Entity ent, int type, int min, int max)
        {
            Item.NewItem(ent.Hitbox, type, Main.rand.Next(min, max));
        }
    }

    public static class MathUtils
    {
        public static void RefClamp<T>(ref T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(max) > 0)
            {
                value = max;
            }
            else if (value.CompareTo(min) < 0)
            {
                value = min;
            }
        }

        public static float GetLerpValue(float from, float to, float t, bool clamped = false)
        {
            if (clamped)
            {
                if (from < to)
                {
                    if (t < from)
                    {
                        return 0f;
                    }
                    if (t > to)
                    {
                        return 1f;
                    }
                }
                else
                {
                    if (t < to)
                    {
                        return 1f;
                    }
                    if (t > from)
                    {
                        return 0f;
                    }
                }
            }
            return (t - from) / (to - from);
        }

        public static Vector2 MoveTowards(this Vector2 currentPosition, Vector2 targetPosition, float maxAmountAllowedToMove)
        {
            Vector2 v = targetPosition - currentPosition;
            if (v.Length() < maxAmountAllowedToMove)
            {
                return targetPosition;
            }
            return currentPosition + v.SafeNormalize(Vector2.Zero) * maxAmountAllowedToMove;
        }

        public static Vector2 DirectionTo(this Vector2 Origin, Vector2 Target)
        {
            return Vector2.Normalize(Target - Origin);
        }

        public static Vector2 DirectionFrom(this Vector2 Origin, Vector2 Target)
        {
            return Vector2.Normalize(Origin - Target);
        }
    }
}
