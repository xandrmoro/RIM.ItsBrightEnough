using LightsOut.Common;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ItsBrightEnough
{
    public class DayNightMapComp : MapComponent
    {
        public static Dictionary<Map, DayNightMapComp> compCache { get; } = new Dictionary<Map, DayNightMapComp>();
        public Dictionary<IntVec3, float> NaturalLight { get; } = new Dictionary<IntVec3, float>();

        public DayNightMapComp(Map map) : base(map)
        {
            compCache[map] = this;
        }

        private int ticks;
        private const int frequency = 360;

        public void SetNaturalLight(IntVec3 cell, float value)
        {
            NaturalLight[cell] = value;
        }

        public override void MapComponentTick()
        {
            ticks++;

            if (ticks % frequency != 0)
            {
                NaturalLight.Clear();
                return;
            }

            var pawns = map.mapPawns.FreeColonistsAndPrisonersSpawned.Concat(map.mapPawns.SlavesOfColonySpawned);

            var workplacesLitEnoughByDaylight = new HashSet<Room>();
            var workplacesNotLitEnoughByDaylight = new HashSet<Room>();

            foreach (var pawn in pawns)
            {
                if (pawn.Downed || pawn.Dead || pawn.Drafted || (pawn.jobs?.curDriver?.asleep ?? false))
                    continue;

                if (pawn.pather.MovingNow)
                    continue;

                var room = pawn.GetRoom();

                if (room is null || room.OutdoorsForWork || room.IsDoorway || !(room.Map?.regionAndRoomUpdater?.Enabled ?? false))
                    continue;

                if (Util.ShouldTurnLightOn(room, pawn))
                {
                    Lights.EnableAllLights(room);
                    continue;
                }

                float value = NaturalLight.GetValueOrDefault(pawn.pather.Destination.Cell, 0);

                if (value < ItsBrightEnough.Settings.JobBrightnessThreshold)
                {
                    workplacesNotLitEnoughByDaylight.Add(room);
                }
                else
                {
                    workplacesLitEnoughByDaylight.Add(room);
                }
            }

            foreach (var room in workplacesLitEnoughByDaylight.Except(workplacesNotLitEnoughByDaylight))
            {
                Lights.DisableAllLights(room, false);
            }

            NaturalLight.Clear();
        }
    }
}
