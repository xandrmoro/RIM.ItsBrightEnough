using HarmonyLib;
using UnityEngine;
using Verse;

namespace Template
{
    public class Template : Mod
    {
        public static Settings Settings { get; private set; }

        public Template(ModContentPack contentPack) : base(contentPack)
        {
            Settings = GetSettings<Settings>();

            new Harmony(Content.PackageIdPlayerFacing).PatchAll();
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return nameof(Template);
        }
    }
}
