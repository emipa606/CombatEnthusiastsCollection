using System;
using HarmonyLib;
using Verse;

namespace CompOversizedWeapon;

public class CompOversizedWeapon : ThingComp
{
    private static readonly Type compDeflectorType = GenTypes.GetTypeInAnyAssembly("CompDeflector.CompDeflector");
    private Func<bool> compDeflectorIsAnimatingNow = AlwaysFalse;

    public CompProperties_OversizedWeapon Props => props as CompProperties_OversizedWeapon;

    public bool CompDeflectorIsAnimatingNow => compDeflectorIsAnimatingNow();

    public bool IsOnGround => ParentHolder is Map;

    private static bool AlwaysFalse()
    {
        return false;
    }

    public override void PostPostMake()
    {
        base.PostPostMake();
        CacheComps();
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        if (Scribe.mode == LoadSaveMode.LoadingVars)
        {
            CacheComps();
        }
    }

    private void CacheComps()
    {
        if (!(compDeflectorType != null))
        {
            return;
        }

        var allComps = parent.AllComps;
        var i = 0;
        for (var count = allComps.Count; i < count; i++)
        {
            var thingComp = allComps[i];
            var type = thingComp.GetType();
            if (!compDeflectorType.IsAssignableFrom(type))
            {
                continue;
            }

            compDeflectorIsAnimatingNow = (Func<bool>)AccessTools.PropertyGetter(type, "IsAnimatingNow")
                .CreateDelegate(typeof(Func<bool>), thingComp);
            break;
        }
    }
}