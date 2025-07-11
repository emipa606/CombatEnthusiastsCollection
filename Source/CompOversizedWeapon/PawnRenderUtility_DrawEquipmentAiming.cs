using HarmonyLib;
using UnityEngine;
using Verse;

namespace CompAdjustedPostition;

[HarmonyPatch(typeof(PawnRenderUtility), nameof(PawnRenderUtility.DrawEquipmentAiming))]
public static class PawnRenderUtility_DrawEquipmentAiming
{
    public static void Prefix(Thing eq, ref Vector3 drawLoc, float aimAngle)
    {
        var compOversizedWeapon = eq.TryGetComp<CompAdjustedPostition>();
        if (compOversizedWeapon == null)
        {
            return;
        }

        switch (aimAngle)
        {
            case > 20f and < 160f when compOversizedWeapon.Props.eastOffset != Vector3.zero:
                drawLoc += compOversizedWeapon.Props.eastOffset;
                return;
            case > 200f and < 340f when compOversizedWeapon.Props.westOffset != Vector3.zero:
                drawLoc += compOversizedWeapon.Props.westOffset;
                break;
        }
    }
}