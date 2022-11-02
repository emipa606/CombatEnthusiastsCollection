using UnityEngine;
using Verse;

namespace CompOversizedWeapon;

public class CompProperties_OversizedWeapon : CompProperties
{
    public float angleAdjustmentEast;

    public float angleAdjustmentNorth;

    public float angleAdjustmentSouth;

    public float angleAdjustmentWest;

    public Vector3 eastOffset = new Vector3(0f, 0f, 0f);

    public GraphicData groundGraphic;

    public bool isDualWeapon;
    public Vector3 northOffset = new Vector3(0f, 0f, 0f);

    public Vector3 southOffset = new Vector3(0f, 0f, 0f);

    public bool verticalFlipNorth;

    public bool verticalFlipOutsideCombat;

    public Vector3 westOffset = new Vector3(0f, 0f, 0f);

    public CompProperties_OversizedWeapon()
    {
        compClass = typeof(CompOversizedWeapon);
    }

    public float NonCombatAngleAdjustment(Rot4 rotation)
    {
        if (rotation == Rot4.North)
        {
            return angleAdjustmentNorth;
        }

        if (rotation == Rot4.East)
        {
            return angleAdjustmentEast;
        }

        return rotation == Rot4.West ? angleAdjustmentWest : angleAdjustmentSouth;
    }

    public Vector3 OffsetFromRotation(Rot4 rotation)
    {
        if (rotation == Rot4.North)
        {
            return northOffset;
        }

        if (rotation == Rot4.East)
        {
            return eastOffset;
        }

        if (rotation == Rot4.West)
        {
            return westOffset;
        }

        return southOffset;
    }
}