using CombatExtended;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace HandLoading
{
    public static class calculutils
    {
        public static List<string> shapes()
        {
            List<string> amogus = new List<string>();
            amogus.Add("Hollow point");
            amogus.Add("Armor piercing");
            amogus.Add("Full Metal Jacket");
            amogus.Add("Sabot");
            return amogus;
        }
        public static List<ThingDef> materials()
        {
            List<ThingDef> amogus = new List<ThingDef>();
            amogus = DefDatabase<ThingDef>.AllDefs.ToList().FindAll(L => L.stuffProps?.categories?.Contains(StuffCategoryDefOf.Metallic) is true);
            return amogus;
        }
        public static ThingDef material()
        {
            return calculutils.materials().RandomElement();
        }
        public static List<ThingDef> powders()
        {
            List<ThingDef> amogus = new List<ThingDef>();
            amogus = DefDatabase<ThingDef>.AllDefs.ToList().FindAll(OOPP => OOPP.comps.Any(OO => OO is PowderCompProps));
            return amogus;
        }
        public static ThingDef powder()
        {
            ThingDef result = new ThingDef();
            result = calculutils.powders().RandomElement();
            if (result.defName == "Archotechiumpowdered")
            {
                if (Rand.Chance(0.02f))
                {
                    return result;
                }
                else
                {
                    result = calculutils.powders().First();
                }
            }
            return result;
        }


        public static void CalAP(ThingDef material, ThingDef projbase, ThingDef propelant, ref float PenNN, string shap)
        {

            string ProjectileShape = shap;
            float ShapeDoubleAP = 1f;
            switch (ProjectileShape)
            {
                case "Hollow point":
                    ShapeDoubleAP = 0.5f;
                    break;

                case "Armor piercing":
                    ShapeDoubleAP = 2f;
                    break;

                case "Full Metal Jacket":
                    ShapeDoubleAP = 1f;
                    break;

                case "Sabot":
                    ShapeDoubleAP = 3.5f;
                    break;

            }
            ProjectilePropertiesCE propsCE = projbase?.projectile as ProjectilePropertiesCE;
            float Penmult2 = new float { };
            Penmult2 = propelant.statBases.Find(abc => abc.stat.defName == "PowderPower")?.value ?? 0f;
            if (Penmult2 == 0f)
            {
                Log.Error("propelant most likely has no PowderPower statbase");
            }
            float hardness_material_multiplier = material.statBases.Find(Peepee => Peepee.stat == StatDefOf.StuffPower_Armor_Sharp).value;



            PenNN = (float)((propsCE?.armorPenetrationSharp ?? 1) + (ShapeDoubleAP * hardness_material_multiplier) * Penmult2 * Rand.Range(1f, 3f));
            Log.Message(projbase.ToString());
            Log.Message(PenNN.ToString());
        }

        public static void CalDam(ThingDef propelant, ref int Damm, string shap, ThingDef baseproj)
        {

            string ProjectileShape = shap;
            float DammNult = 1f;
            switch (ProjectileShape)
            {
                case "Hollow point":
                    DammNult = 2f;
                    break;

                case "Armor piercing":
                    DammNult = 0.5f;
                    break;

                case "Full Metal Jacket":
                    DammNult = 1f;
                    break;

                case "Sabot":
                    DammNult = 0.25f;
                    break;

            }
            //ProjectilePropertiesCE propsCE = projbase?.projectile as ProjectilePropertiesCE;
            float Penmult2 = new float { };
            Penmult2 = propelant.statBases.Find(abc => abc.stat.defName == "PowderPower")?.value ?? 0f;
            if (Penmult2 == 0f)
            {
                Log.Error("propelant most likely has no PowderPower statbase");
            }
            //float hardness_material_multiplier = material.statBases.Find(Peepee => Peepee.stat == StatDefOf.StuffPower_Armor_Sharp).value;
            Log.Message(propelant.label);
            //float dmg = new float();

            Damm = (int)Math.Round(DammNult * baseproj.projectile.GetDamageAmount(0.8f) * propelant.statBases.Find(PePe => PePe.stat.defName == "PowderPower").value * (Rand.Range(1, 3)));
            //PenNN = (float)((propsCE?.armorPenetrationSharp ?? 1) + (ShapeDoubleAP * hardness_material_multiplier) * Penmult2 * Rand.Range(1f, 3f));
            //Log.Message(projbase.ToString());
            Log.Message(Damm.ToString());
        }
    }
}
