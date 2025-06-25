using System.Reflection;
using HarmonyLib;
using Verse;

namespace CompAdjustedPostition;

[StaticConstructorOnStartup]
internal static class HarmonyCompAdjustedPostition
{
    static HarmonyCompAdjustedPostition()
    {
        new Harmony("Mlie.CompAdjustedPostition").PatchAll(Assembly.GetExecutingAssembly());
    }
}