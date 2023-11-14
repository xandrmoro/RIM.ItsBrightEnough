using HarmonyLib;
using LightsOut.Patches.Lights;
using System.Collections.Generic;
using System.Reflection.Emit;
using LightsOut.Common;

namespace ItsBrightEnough
{
    [HarmonyPatch(typeof(DetectPawnRoomChange), nameof(DetectPawnRoomChange.Postfix))]
    public class LightsOut_RoomChange_PatchPostfix
    {
        [HarmonyBefore("LightsOut")]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction lastBrfalse = null;

            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Brfalse_S)
                {
                    lastBrfalse = instruction;
                }

                if (instruction.Calls(AccessTools.Method(typeof(Lights), nameof(Lights.EnableAllLights))))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Util), nameof(Util.ShouldTurnLightOn)));
                    yield return new CodeInstruction(OpCodes.Brfalse_S, lastBrfalse.operand);
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                }

                yield return instruction;
            }
        }
    }
}
