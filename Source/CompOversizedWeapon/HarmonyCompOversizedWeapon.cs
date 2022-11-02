using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace CompOversizedWeapon;

[StaticConstructorOnStartup]
internal static class HarmonyCompOversizedWeapon
{
    static HarmonyCompOversizedWeapon()
    {
        var harmony = new Harmony("jecstools.jecrell.comps.oversized");
        var typeFromHandle = typeof(HarmonyCompOversizedWeapon);
        harmony.Patch(AccessTools.Method(typeof(PawnRenderer), "DrawEquipmentAiming"),
            new HarmonyMethod(typeFromHandle, "DrawEquipmentAimingPreFix"));
        harmony.Patch(AccessTools.PropertyGetter(typeof(Thing), "DefaultGraphic"), null,
            new HarmonyMethod(typeFromHandle, "get_DefaultGraphic_PostFix"));
    }

    public static bool DrawEquipmentAimingPreFix(Pawn ___pawn, Thing eq, Vector3 drawLoc, float aimAngle)
    {
        var compOversizedWeapon = eq.TryGetCompOversizedWeapon();
        if (compOversizedWeapon == null)
        {
            return true;
        }

        if (compOversizedWeapon.CompDeflectorIsAnimatingNow)
        {
            return false;
        }

        var flipped = false;
        var west = false;
        var num = aimAngle - 90f;
        switch (aimAngle)
        {
            case > 20f and < 160f:
                num += eq.def.equippedAngleOffset;
                break;
            case > 200f and < 340f:
                west = true;
                num -= 180f;
                num -= eq.def.equippedAngleOffset;
                break;
            default:
                num += eq.def.equippedAngleOffset;
                flipped = true;
                break;
        }

        var props = compOversizedWeapon.Props;
        var rotation = ___pawn.Rotation;
        if (props != null && !___pawn.IsFighting())
        {
            if (flipped && props.verticalFlipOutsideCombat)
            {
                num += 180f;
            }

            if (props.verticalFlipNorth && rotation == Rot4.North)
            {
                num += 180f;
            }

            num += props.NonCombatAngleAdjustment(rotation);
        }

        num %= 360f;
        var material = eq.Graphic is Graphic_StackCount graphic_StackCount
            ? graphic_StackCount.SubGraphicForStackCount(1, eq.def).MatSingle
            : eq.Graphic.MatSingle;
        var s = new Vector3(eq.def.graphicData.drawSize.x, 1f, eq.def.graphicData.drawSize.y);
        var vector = props?.OffsetFromRotation(rotation) ?? Vector3.zero;
        var matrix = Matrix4x4.TRS(drawLoc + vector, Quaternion.AngleAxis(num, Vector3.up), s);
        Graphics.DrawMesh(west ? MeshPool.plane10Flip : MeshPool.plane10, matrix, material, 0);
        if (props is not { isDualWeapon: true })
        {
            return false;
        }

        vector = new Vector3(-1f * vector.x, vector.y, vector.z);
        if (rotation == Rot4.North || rotation == Rot4.South)
        {
            num += 135f;
            num %= 360f;
        }
        else
        {
            vector = new Vector3(vector.x, vector.y - 0.1f, vector.z + 0.15f);
            west = !west;
        }

        matrix.SetTRS(drawLoc + vector, Quaternion.AngleAxis(num, Vector3.up), s);
        Graphics.DrawMesh(west ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, material, 0);

        return false;
    }

    public static void get_DefaultGraphic_PostFix(Thing __instance, Graphic ___graphicInt, ref Graphic __result)
    {
        if (___graphicInt == null || __instance.ParentHolder is Pawn)
        {
            return;
        }

        var compOversizedWeapon = __instance.TryGetCompOversizedWeapon();
        if (compOversizedWeapon == null)
        {
            return;
        }

        var graphicData = compOversizedWeapon.Props?.groundGraphic;
        if (graphicData != null && compOversizedWeapon.IsOnGround)
        {
            var graphic = graphicData.GraphicColoredFor(__instance);
            if (graphic != null)
            {
                graphic.drawSize = graphicData.drawSize;
                __result = graphic;
                return;
            }
        }

        __result.drawSize = __instance.def.graphicData.drawSize;
    }
}