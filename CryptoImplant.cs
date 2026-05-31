using HarmonyLib;
using RimWorld;
using Verse;

namespace CryptosleepAdaptation
{
    [StaticConstructorOnStartup]
    public static class CryptosleepAdaptationMod
    {
        internal static HediffDef AdaptationImplantDef;

        static CryptosleepAdaptationMod()
        {
            var harmony = new Harmony("com.yourname.cryptosleepadaptation");
            harmony.PatchAll();
            AdaptationImplantDef = HediffDef.Named("CryptosleepAdaptationImplant");
        }
    }

    [HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.AddHediff),
        new[] { typeof(Hediff), typeof(BodyPartRecord), typeof(DamageInfo?), typeof(DamageWorker.DamageResult) })]
    public static class Patch_AddHediff_BlockCryptosleepSickness
    {
        private static readonly HediffDef CryptosleepSicknessDef = HediffDef.Named("CryptosleepSickness");

        static bool Prefix(Pawn_HealthTracker __instance, Hediff hediff)
        {
            if (hediff?.def != CryptosleepSicknessDef)
                return true;

            Pawn pawn = __instance.hediffSet?.pawn;
            if (pawn == null)
                return true;

            return !pawn.health.hediffSet.HasHediff(CryptosleepAdaptationMod.AdaptationImplantDef);
        }
    }
}