using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ItsBrightEnough
{
    public static class Util
    {
        public static bool ShouldTurnLightOn(Room room, Pawn pawn)
        {
            if (room.Cells.Contains(pawn.pather.Destination.Cell))
            {
                if (room.Map.glowGrid.GameGlowAt(pawn.pather.Destination.Cell) < ItsBrightEnough.Settings.JobBrightnessThreshold)
                {
                    return true;
                }
            }

            if (!pawn.pather.MovingNow)
                return false;

            var cells = new HashSet<IntVec3>();

            for (var i = 0; i < pawn.pather.curPath.NodesLeftCount - 1; i++)
            {
                var from = pawn.pather.curPath.Peek(i).ToVector3Shifted().ToIntVec3();
                var to = pawn.pather.curPath.Peek(i + 1).ToVector3Shifted().ToIntVec3();

                foreach (var cell in GenSight.PointsOnLineOfSight(from, to))
                {
                    if (room.ContainsCell(cell))
                        cells.Add(cell);
                    else
                        goto exit;
                }
            }

        exit:

            var average = cells.Any() ? cells.Average(c => room.Map.glowGrid.GameGlowAt(c)) : 0;

            return average < ItsBrightEnough.Settings.TrespassBrightnessThreshold;
        }
    }
}
