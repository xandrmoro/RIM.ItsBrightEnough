using HarmonyLib;
using Verse;
using LightsOut.Common;

namespace ItsBrightEnough
{
    [HarmonyPatch(typeof(Resources), nameof(Resources.SetTicksRemaining))]
    public class LightsOut_Resources_SetTicksRemaining
    {
        public static void Prefix(ThingWithComps thing, ref int? ticksRemaining)
        {
            if (thing != null && ticksRemaining > 0 && Resources.BuildingStatus[thing] == false)
            {
                ticksRemaining = 0;
            }
        }
    }
}
