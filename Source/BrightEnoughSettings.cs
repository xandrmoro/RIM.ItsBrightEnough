using Verse;

namespace ItsBrightEnough
{
    public class BrightEnoughSettings : ModSettings
    {
        public float JobBrightnessThreshold = 0.45f;
        public float TrespassBrightnessThreshold = 0.20f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref JobBrightnessThreshold, nameof(JobBrightnessThreshold), 0.45f);
            Scribe_Values.Look(ref TrespassBrightnessThreshold, nameof(TrespassBrightnessThreshold), 0.20f);

            base.ExposeData();
        }
    }
}
