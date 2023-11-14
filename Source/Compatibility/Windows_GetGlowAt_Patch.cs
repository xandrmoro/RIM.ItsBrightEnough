using HarmonyLib;
using ItsBrightEnough;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace Compatibility
{
    [HarmonyPatch]
    public class Windows_GetGlowAt_Patch
    {
        public static bool Prepare()
        {
            return ModLister.GetActiveModWithIdentifier("Owlchemist.Windows") != null;
        }

        public static MethodBase TargetMethod()
        {
            Log.Message(AccessTools.Method("OpenTheWindows.GlowGrid_GameGlowAt:Postfix").DeclaringType.Assembly.Location);
            return AccessTools.Method("OpenTheWindows.GlowGrid_GameGlowAt:Postfix");
        }

        [HarmonyBefore("JPT.OpenTheWindows")]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                yield return instruction;

                if (instruction.opcode == OpCodes.Stloc_0)
                {
                    // get current DayNightComp
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(DayNightMapComp), nameof(DayNightMapComp.compCache)));
                    yield return new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field("OpenTheWindows.LightingOverlay_Regenerate:map"));
                    yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(Dictionary<Map, DayNightMapComp>), "get_Item"));

                    // store natural light value for current cell
                    yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(DayNightMapComp), nameof(DayNightMapComp.NaturalLight)));
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(Dictionary<IntVec3, float>), "set_Item"));
                }
            }
        }
    }
}
