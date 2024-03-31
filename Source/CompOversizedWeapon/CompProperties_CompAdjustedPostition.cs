using UnityEngine;
using Verse;

namespace CompAdjustedPostition;

public class CompProperties_CompAdjustedPostition : CompProperties
{
    public Vector3 eastOffset = Vector3.zero;

    public Vector3 westOffset = Vector3.zero;

    public CompProperties_CompAdjustedPostition()
    {
        compClass = typeof(CompAdjustedPostition);
    }
}