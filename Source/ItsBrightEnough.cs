using HarmonyLib;
using UnityEngine;
using Verse;

namespace ItsBrightEnough
{
    public class ItsBrightEnough : Mod
    {
        public static BrightEnoughSettings Settings { get; private set; }

        public ItsBrightEnough(ModContentPack contentPack) : base(contentPack)
        {
            Settings = GetSettings<BrightEnoughSettings>();
            
            new Harmony(Content.PackageIdPlayerFacing).PatchAll();
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing = new Listing_Standard();

            listing.Begin(inRect);

            listing.Label($"Job Brightness Threshold: {Settings.JobBrightnessThreshold.ToStringPercent()}");
            Settings.JobBrightnessThreshold = listing.Slider(Settings.JobBrightnessThreshold, 0f, 1f);

            listing.Label($"Trespass Brightness Threshold: {Settings.TrespassBrightnessThreshold.ToStringPercent()}");
            Settings.TrespassBrightnessThreshold = listing.Slider(Settings.TrespassBrightnessThreshold, 0f, 1f);

            listing.End();

            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Bright Enough";
        }
    }
}
