using Verse;

namespace CompOversizedWeapon;

public static class CompOversizedWeaponUtility
{
    public static CompOversizedWeapon GetCompOversizedWeapon(this ThingWithComps thing)
    {
        var allComps = thing.AllComps;
        var i = 0;
        for (var count = allComps.Count; i < count; i++)
        {
            if (allComps[i] is CompOversizedWeapon result)
            {
                return result;
            }
        }

        return null;
    }

    public static CompOversizedWeapon TryGetCompOversizedWeapon(this Thing thing)
    {
        return thing is not ThingWithComps thing2 ? null : thing2.GetCompOversizedWeapon();
    }
}