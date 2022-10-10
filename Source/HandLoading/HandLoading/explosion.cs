using CombatExtended;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace HandLoading
{
    public class exploder : ThingComp
    {
        public bool checl;
        public float faketicker;
        public CompAmmoUser compammo;
        public override void Notify_UsedWeapon(Pawn pawn)
        {
            compammo = this.parent.TryGetComp<CompAmmoUser>();
            if (compammo != null)
            {

                float damchange = 0.1f;
                if (compammo.CurrentAmmo.comps.Any(oof => oof is damagercproprs) && faketicker != 10)
                {
                    damagercproprs damagercproprs = compammo.CurrentAmmo.comps.Find(oof2 => oof2 is damagercproprs) as damagercproprs;

                    faketicker++;
                    damchange = damagercproprs.dammpoints;
                    Log.Message(damchange.ToString());
                    Log.Message("teszt");
                    //AmmoExplodyBitsOrSomethingIdk testrn = ;

                }
                if (compammo.CurrentAmmo.comps.Any(oof => oof is damagercproprs) && faketicker == 10)
                {
                    damagercproprs damagercproprs = compammo.CurrentAmmo.comps.Find(oof2 => oof2 is damagercproprs) as damagercproprs;
                    damchange = damagercproprs.dammpoints;
                    faketicker = 0;
                    //Log.Message(damchange.ToString());
                    //Log.Message("teszt zwei");
                    //AmmoExplodyBitsOrSomethingIdk testrn = ;
                    this.parent.HitPoints -= (int)Math.Round(damchange * 20);
                    if (this.parent.HitPoints < 0)
                    {
                        if (Rand.Chance(0.25f))
                        {
                            GenExplosionCE.DoExplosion(pawn.Position, Find.CurrentMap, Rand.Range(1, 2), DamageDefOf.Burn, this.parent, 20, Rand.Range(8.25f, 20.5f), SoundDefOf.Thunder_OnMap);
                            this.parent.Kill(null);
                        }
                    }

                }

            }
            base.Notify_UsedWeapon(pawn);

        }
        public bool dodl = true;
        public override void Notify_Equipped(Pawn pawn)
        {

            if (pawn.Faction != Faction.OfPlayer && dodl)
            {
                if (pawn.ParentHolder is World)
                {
                    Log.Message("worlt");
                    return;
                }
                if (pawn.Faction == Faction.OfAncients && pawn.Faction == Faction.OfAncientsHostile)
                {
                    Log.Message("ancients");
                    return;
                }
                if ((Find.World.GetComponent<IhaveacoldandImustsneeze>().Factionammos?.ToList().FindAll(tt => tt.Key == pawn.Faction.ToString())?.Count ?? 0) < HandeLoading.settings.factionammos)
                {
                    Log.Message(HandeLoading.settings.factionammos.ToString());
                    //Log.Error("adobe");
                    if (this.parent.TryGetComp<CompAmmoUser>()?.Props.ammoSet.ammoTypes.Any(tt33 => tt33.ammo.ammoClass.defName == "FullMetalJacket") ?? false)
                    {

                        CreateAmmosThenGiveThem(pawn);
                    }
                    else if (this.parent.TryGetComp<CompAmmoUser>()?.Props.ammoSet.ammoTypes.Any(tt33 => tt33.ammo.ammoClass.defName == "GrenadeHE" | tt33.ammo.ammoClass.defName == "BuckShot" | tt33.ammo.ammoClass.defName == "RocketHEAT" | tt33.ammo.ammoClass.defName == "RocketFrag") ?? false)
                    {

                        CreateAmmosThenGiveThem(pawn, true);
                    }
                }

                else
                {



                    Log.Message("adobe");

                }
                dodl = false;
            }






            base.Notify_Equipped(pawn);
        }
        public int s;

        public float checktechlvls(float input, float input2)
        {
            float result = input / input2;
            if (result > 1)
            {
                result = 0.5f;
            }
            //Log.Message("weight: " + result + " source tech level(powder) " + input.ToString() + " source thech level(faction) " + input2.ToString());
            return result;
        }
        public float checktechlvls2(float input, float input2)
        {
            float result = input / input2;
            if (result > 1)
            {
                result = 0.45f;
            }
            //Log.Message("weight: " + result + " source tech level(projectile type) " + input.ToString() + " source thech level(faction) " + input2.ToString());
            return result;
        }
        public float chancemult(ThingDef thingdef, Pawn pawn)
        {
            var result = 1f;
            var Mass = thingdef.statBases.Find(tt33 => tt33.stat == StatDefOf.Mass).value;
            var PenBonus = thingdef.statBases.Find(tt33 => tt33.stat == StatDefOf.StuffPower_Armor_Sharp).value;
            var Cost  = thingdef.statBases.Find(tt33 => tt33.stat == StatDefOf.MarketValue).value;
            result = (Mass + PenBonus) / ((Cost) / (Math.Max(1f, (float)pawn.Faction.def.techLevel / 2)));
            //Log.Message("Chance weight: " + result.ToString() + " for material: " + thingdef.label);
            return result;
        }
        public float charge(Pawn pawn)
        {
            float result = 1f;
            if(pawn.Faction.def.techLevel <= TechLevel.Medieval)
            {
                result = Rand.Range(1.5f, 4.5f);
            }
            if(pawn.Faction.def.techLevel >= TechLevel.Industrial)
            {
                result = Rand.Range(1.3f, 2.1f);
            }
            return result;
        }
        public void CreateAmmosThenGiveThem(Pawn pawn, bool isFrag = false)
        {
            CompAmmoUser comammp = this.parent.TryGetComp<CompAmmoUser>();
         
            if (pawn.Faction != Faction.OfPlayer && comammp != null)
            {
                AmmoDef Adef = new AmmoDef { };
                ThingDef new_projectile = new ThingDef { };
                AmmoDef amder = comammp.Props.ammoSet.ammoTypes.First().ammo;
                ThingDef based_projectile = comammp.Props.ammoSet.ammoTypes.First().projectile;
                //Log.Message(based_projectile.label);
                FieldInfo[] amogus = typeof(AmmoDef).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                foreach (FieldInfo info in amogus)
                {
                    info.SetValue(Adef, info.GetValue(amder));
                }
                //Log.Warning("1a-1");
                FieldInfo[] amoguss = typeof(ThingDef).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                foreach (FieldInfo info in amoguss)
                {
                    info.SetValue(new_projectile, info.GetValue(based_projectile));
                }
                //Log.Warning("2b-2");
                new_projectile.projectile = new ProjectilePropertiesCE { };
                FieldInfo[] projectus = typeof(ProjectilePropertiesCE).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                ProjectilePropertiesCE amo = (ProjectilePropertiesCE)based_projectile.projectile;
                float amoguys = amo.GetDamageAmount(1);
                foreach (FieldInfo info in projectus)
                {
                    // Log.Message(info.Name);

                    info.SetValue(new_projectile.projectile, info.GetValue(amo));
                }
                //Log.Warning("3b-3");


                ///Armor pen calculation and shit

                List<ThingDef> powders = DefDatabase<ThingDef>.AllDefs.ToList().FindAll(tt => tt.comps.Any(k => k is PowderCompProps));
                Func<ThingDef, float> funcer = flt => checktechlvls((float)flt.techLevel, (float)pawn.Faction.def.techLevel);

                Func<RecipeShapeDef, float> shapefunc = flt => checktechlvls2((float)flt.techlevel, (float)pawn.Faction.def.techLevel);

                ThingDef powder = powders.RandomElementByWeight(funcer);
                var dar = (float)Math.Round(charge(pawn), 2);
                float powderpower = powder.statBases.Find(tt33 => tt33.stat == AmmoClassesDefOf.PowderPower).value;
                Func<ThingDef, float> funcermat = flt => chancemult(flt, pawn);
                List<ThingDef> liszt = DefDatabase<ThingDef>.AllDefs.ToList().FindAll(ptrd => ptrd.stuffProps?.categories.Contains(StuffCategoryDefOf.Metallic) ?? false);
                liszt.RemoveAll(tt33 => tt33.defName == "Silver" | tt33.defName == "Gold");
                ThingDef projmat = liszt.RandomElementByWeight(funcermat);
                //Log.Message("projmat: " + projmat.label);
                float matmultpen = projmat.statBases.Find(tt33 => tt33.stat == StatDefOf.StuffPower_Armor_Sharp).value;
                float basepen = amo.armorPenetrationSharp;
                ProjectilePropertiesCE cemogus = (ProjectilePropertiesCE)new_projectile.projectile;
                RecipeShapeDef shapeDef = DefDatabase<RecipeShapeDef>.AllDefs.ToList().FindAll(tt33 => tt33.ismulti == false && tt33.isfrag == false).RandomElementByWeight(shapefunc);
                if (isFrag)
                {
                    shapeDef = DefDatabase<RecipeShapeDef>.AllDefsListForReading.FindAll(o => o.ismulti).RandomElement();
                }
                cemogus.armorPenetrationSharp = (float)Math.Round(basepen + (matmultpen * powderpower * dar) * shapeDef.penmult, 2);
                ///naming
                Adef.label = pawn.Faction.ToString() + " hand loaded " + projmat.label + " " + shapeDef.label + " " + amder.AmmoSetDefs.First().label;
                new_projectile.label = pawn.Faction.ToString() + " hand loaded " + projmat.label + " " + shapeDef.label + " " + amder.AmmoSetDefs.First().label;
                Log.Message(Adef.label + " armor pen: " + cemogus.armorPenetrationSharp);
                // Adef.forceDebugSpawnable = true;
                new_projectile.modExtensions = new List<DefModExtension>();

                float calcul = (shapeDef.damagemult * amoguys * dar);
                //Log.Message(calcul.ToString());
                calcul = (float)Math.Round(calcul);
                new_projectile.modExtensions.Add(new BulletModExtension { FixedDamage = calcul });
                Adef.thingClass = typeof(AmmoThing);
                new_projectile.thingClass = typeof(HandLoading.Bullet);
                DefDatabase<ThingDef>.AllDefs.AddItem(Adef);

                comammp.Props.ammoSet.ammoTypes.Add(new AmmoLink { ammo = Adef, projectile = new_projectile });

                Adef.defName = "a" + Rand.Range(0, 25678) + "add" + Rand.Chance(0.50f).ToString() + "add" + Rand.Range(0, 25678) + Rand.Chance(0.50f).ToString();

                new_projectile.defName = "a" + Rand.Range(0, 25678) + "add" + Rand.Chance(0.50f).ToString() + "add" + Rand.Range(0, 25678) + Rand.Chance(0.50f).ToString();

                Adef.ammoClass = new AmmoCategoryDef { label = Adef.label, labelShort = Adef.label, description = calcul.ToString(), defName = Rand.Range(0, 25678) + "add" + Rand.Chance(0.50f).ToString() };

                Adef.generateCommonality = 0f;

                Adef.ammoClass.description = "actual damage is: " + new_projectile.GetModExtension<BulletModExtension>().FixedDamage.ToString();

                Adef.description = "Powder used: " + powder.label + " Charge amount " + dar.ToString() + " projectile design :" + shapeDef.label + " damage: " + calcul.ToString();

                if (Find.World.GetComponent<IhaveacoldandImustsneeze>().Factionammos != null)
                {

                    Find.World.GetComponent<IhaveacoldandImustsneeze>().Factionammos.Add(new KeyValuePair<string, AmmoDef>(key: pawn.Faction.ToString(), value: Adef));
                }
                else
                {

                    Find.World.GetComponent<IhaveacoldandImustsneeze>().Factionammos = new List<KeyValuePair<string, AmmoDef>>();
                    Find.World.GetComponent<IhaveacoldandImustsneeze>().Factionammos.Add(new KeyValuePair<string, AmmoDef>(key: pawn.Faction.ToString(), value: Adef));
                }
                if (isFrag && shapeDef.isfrag && based_projectile.comps.Any(tt33 => tt33 is CompProperties_Fragments))
                {
                    CompProperties_Fragments frags = new CompProperties_Fragments();
                    FieldInfo[] fragfields = typeof(CompProperties_Fragments).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    
                    foreach (FieldInfo info in fragfields)
                    {
                        info.SetValue(frags, info.GetValue(based_projectile.comps.Find(tt33 => tt33 is CompProperties_Fragments)));
                    }
                    //Log.Warning("4d-4");
                    makefrags(projmat, powder);
                    if (new_projectile.comps.Any(tt33 => tt33 is CompProperties_Fragments))
                    {
                        new_projectile.comps.RemoveAll(tt32 => tt32 is CompProperties_Fragments);
                    }
                    new_projectile.comps.Add(frags);
                    foreach (ThingDefCountClass thingdef in frags.fragments)
                    {
                        thingdef.thingDef = fragc1;
                    }

                }
                var athink = (dar * Rand.Range(1, 3) * powder.statBases.Find(Poof => Poof.stat.defName == "PowderPower").value) / 10;
                Adef.comps.Add(new damagercproprs { dammpoints = athink });
                new_projectile.graphic = based_projectile.graphic;
                var s21s = new_projectile.projectile as ProjectilePropertiesCE;
                s21s.pelletCount = 1;
                if (shapeDef.ThermoBar)
                {
                   
                    s21s.explosionRadius = (float)Math.Round(calcul / 60f);
                    s21s.damageDef = AmmoClassesDefOf.Thermobaric;
                    
                }
                if (shapeDef.ismulti && !shapeDef.isfrag && !shapeDef.ThermoBar)
                {

                    s21s.pelletCount = Rand.Range(1, 12);
                    s21s.spreadMult = s21s.pelletCount * shapeDef.spreadmult;
                    

                }

                Find.World.GetComponent<SaverComp>().SaveAmmodef(projectile: new_projectile, ammo: Adef, ammoset: Adef.AmmoSetDefs.First(), ammoCategory: Adef.ammoClass);
            }
        }
        public ThingDef fragc1;

        public void makefrags(ThingDef material, ThingDef powder)
        {
            fragc1 = new ThingDef { };
            FieldInfo[] fields = typeof(ThingDef).GetFields(
                       BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo dd in fields)
            {

                dd.SetValue(fragc1, dd.GetValue(AmmoClassesDefOf.Fragment_Large));
            }
            //Log.Warning("1a");
            fragc1.projectile = new ProjectilePropertiesCE();
            FieldInfo[] fieldsproj = typeof(ProjectileProperties).GetFields(
                       BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo dd in fieldsproj)
            {
                Log.Message(dd.Name);
                dd.SetValue(fragc1.projectile, dd.GetValue(AmmoClassesDefOf.Fragment_Large.projectile));
            }
            //Log.Warning("2b");
            float powderpower = powder.statBases.Find(oo => oo.stat.defName == "PowderPower").value;
            //Log.Message(powderpower.ToString());
            ProjectilePropertiesCE atak = (ProjectilePropertiesCE)fragc1.projectile;
            atak.armorPenetrationSharp = 1 * material.statBases.Find(pp => pp.stat == StatDefOf.StuffPower_Armor_Sharp).value * powderpower * material.statBases.Find(pp => pp.stat == StatDefOf.Mass).value * Rand.Range(2f, 4.5f);
            atak.armorPenetrationBlunt = 20 * material.statBases.Find(pp => pp.stat == StatDefOf.Mass).value * powderpower;
            //Log.Message(atak.armorPenetrationSharp.ToString() + "ap sharp");
            //Log.Message(atak.armorPenetrationBlunt.ToString() + "ap blunt");
            fragc1.label = material + " fragments";
            fragc1.defName = "truten" + Rand.Range(0, 670).ToString() + "trzmiel" + Rand.Range(0, 670).ToString() + "bak";
           
            Find.World.GetComponent<SaverComp>().SaveAmmodef(projectile: fragc1, amifrag: true);
        }


    }
    public class AmmoExplodyBitsOrSomethingIdk : ThingComp
    {
        public float dampoints => Props.dammpoints;
        int test = 0;

        public override void PostPostMake()
        {
            base.PostPostMake();
            if (this.parent?.def?.tickerType != TickerType.Normal)
            {
                Log.Message("ammo had no ticker. correcting");
                this.parent.def.tickerType = TickerType.Normal;
                Log.Message("corrected");
            }
        }
        public override void CompTick()
        {
            if (test == 0)
            {

                test++;

            }

            if (this.ParentHolder is Pawn_InventoryTracker | this.ParentHolder is CompInventory)
            {
                Pawn_InventoryTracker inbentor = this.ParentHolder as Pawn_InventoryTracker;

                if (inbentor.innerContainer.Any(O => O.def == this.parent.def && O != this.parent))
                {
                    inbentor.innerContainer.ToList().Find(O => O.def == this.parent.def && O != this.parent).stackCount += this.parent.stackCount;

                    this.parent.Destroy();
                }
            }
            if (!(this.ParentHolder is Pawn_InventoryTracker) && !(this.ParentHolder is Map))
            {

                Log.Message(this.ParentHolder.ToString());
            }
            base.CompTick();
        }


        public damagercproprs Props => (damagercproprs)this.props;
    }
    public class damagercproprs : CompProperties
    {
        public float dammpoints = 0.1f;

        public damagercproprs()
        {
            this.compClass = typeof(AmmoExplodyBitsOrSomethingIdk);
        }

        public damagercproprs(Type compClass) : base(compClass)
        {
            this.compClass = compClass;
        }
    }
    [DefOf]
    public class shapdefof : DefOf
    {
        public static RecipeShapeDef frag;
    }
}
