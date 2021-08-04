using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse.AI;
using Verse.Sound;
using HarmonyLib;
using Verse;
using UnityEngine;
using AlienRace;

namespace HalfDragons
{
    public class HalfDragonsMod : Mod
    {
        ToggleSettings settings;
        public HalfDragonsMod(ModContentPack content) : base(content)
        {
          
          


            this.settings = GetSettings<ToggleSettings>();
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            this.settings = GetSettings<ToggleSettings>();
            if (settings.eyes)
            {
                listingStandard.CheckboxLabeled("Use vanilla eyes (requires reloading the save to take effect)", ref settings.eyes, "Use vanilla eyes (requires reloading the save)");
                DefDatabase<AlienRace.ThingDef_AlienRace>.AllDefs.ToList().Find(A => A.defName == "HalfDragon").alienRace.graphicPaths.First().head = "pawn/headwithneweyes/";
                //Log.Message(DefDatabase<AlienRace.ThingDef_AlienRace>.AllDefs.ToList().Find(A => A.defName == "HalfDragon").alienRace.graphicPaths.First().head);
            }
            else
            {
                listingStandard.CheckboxLabeled("Use modded eyes (requires reloading the save to take effect)", ref settings.eyes, "Use modded eyes (requires reloading the save)");
               
                DefDatabase<AlienRace.ThingDef_AlienRace>.AllDefs.ToList().Find(A => A.defName == "HalfDragon").alienRace.graphicPaths.First().head = "pawn/head/";
                //Log.Message(DefDatabase<AlienRace.ThingDef_AlienRace>.AllDefs.ToList().Find(A => A.defName == "HalfDragon").alienRace.graphicPaths.First().head);

            }
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);

        }
        public override string SettingsCategory()
        {
            return "Half Dragons";
        }
    }
    public class ToggleSettings : ModSettings
    {

        public bool eyes;
       




        public override void ExposeData()
        {
            Scribe_Values.Look(ref eyes, "eyes");
        



            base.ExposeData();
        }
    }

}
