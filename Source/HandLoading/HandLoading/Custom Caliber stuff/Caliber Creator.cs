using CombatExtended;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace HandLoading.CustomCalibers
{
    public class CaliberRecreator : WorldComponent
    {


        public Dictionary<ThingWithComps, AmmoSetDef> compams = new Dictionary<ThingWithComps, AmmoSetDef>();
        public void rechangecaliber(ThingWithComps thingwith, AmmoSetDef amset)
        {
            FieldInfo[] something = typeof(CompProperties_AmmoUser).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            CompProperties_AmmoUser amuser = new CompProperties_AmmoUser();
            foreach (FieldInfo field in something)
            {
                field.SetValue(amuser, field.GetValue(thingwith.TryGetComp<CompAmmoUser>().Props));
            }
            amuser.ammoSet = amset;
            thingwith.TryGetComp<CompAmmoUser>().props = amuser;
        }
        public CaliberRecreator(World world) : base(world)
        {
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref compams, "comampssa", LookMode.Reference, LookMode.Def);
            foreach (KeyValuePair<ThingWithComps, AmmoSetDef> valuepear in compams)
            {
                Log.Message(valuepear.Key.Label);
            }
            base.ExposeData();
        }
        public override void FinalizeInit()
        {
            if (compams != null && compams.Count > 0)
            {
                foreach (KeyValuePair<ThingWithComps, AmmoSetDef> valuepear in compams)
                {
                    Log.Message(valuepear.Key.Label + " readding caliber conversion");
                    rechangecaliber(valuepear.Key, valuepear.Value);
                }
            }
            base.FinalizeInit();
        }
    }
    public class CaliberMacher : Window
    {
        private static readonly Vector2 Test = new Vector2(100f, 140f);

        public AmmoDef parentcase;

        public float dimX;

        public float dimY;
        public static string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        public float tloat = 1f;

        public override void DoWindowContents(Rect inRect)
        {
            Rect rect1 = new Rect(inRect);
            rect1.width = Test.x + 16f;
            rect1.height = Test.y + 16f;
            rect1 = rect1.CenteredOnXIn(inRect);
            rect1 = rect1.CenteredOnYIn(inRect);
            rect1.x += 16f;
            rect1.y += 16f;
            Rect position = new Rect(rect1.xMin + (rect1.width - Test.x) / 2f - 10f, rect1.yMin + 20f, Test.x, Test.y);

            Rect trec = new Rect(inRect);
            trec = trec.CenteredOnXIn(inRect);
            trec = trec.CenteredOnYIn(inRect);
            trec.width = 90f;
            trec.height = 90f;

            Rect trec2 = new Rect(inRect);
            trec2 = trec2.CenteredOnXIn(inRect);
            trec2 = trec2.CenteredOnYIn(inRect);
            trec2.width = 90f;
            trec2.height = 90f;
            trec2.x = 200f;

            if (Widgets.ButtonText(trec, "Choose parent case: "))
            {
                List<AmmoDef> ammos = DefDatabase<AmmoDef>.AllDefs.ToList();
                ammos.RemoveAll(C => C.defName != "Ammo_556x45mmNATO_FMJ");
                ammos.Add(DefDatabase<AmmoDef>.AllDefs.ToList().Find(K => K.defName == "Ammo_762x54mmR_FMJ"));
                List<FloatMenuOption> floatmenus = new List<FloatMenuOption>();
                foreach (AmmoDef ammo in ammos)
                {
                    FloatMenuOption flot = new FloatMenuOption(ammo.label, delegate
                    {
                        Log.Message(ammo.defName);
                        string tink = GetNumbers(ammo.defName);
                        string tink2 = GetNumbers(ammo.defName);


                        Log.Message(tink);
                        Log.Message(tink.First().ToString());
                        dimY = Convert.ToInt32(tink2.Substring(3, 2));
                        if (Convert.ToInt32(tink.First().ToString()) < 9)
                        {
                            tink = tink.Remove(3, 2);
                            Log.Message(tink + "afafafa");
                            dimX = Convert.ToInt32(tink);
                        }

                    });
                    floatmenus.Add(flot);


                }
                Find.WindowStack.Add(new FloatMenu(floatmenus));
            }
            Rect trec433 = new Rect(inRect);
            trec433 = trec433.CenteredOnXIn(inRect);
            trec433 = trec433.CenteredOnYIn(inRect);
            trec433.width = 40f;
            trec433.height = 100f;
            trec433.y = 200f;
            trec433.x = 500;
            Vector2 vector = trec433.position;
            tloat = GUI.HorizontalSlider(trec433, tloat, 1f, 50f);
            //GUI.Label(new Rect(vector, vector), "Adjust plus and minus change value (0.01 - 0.5)");

            GUI.TextArea(trec2, Math.Round((dimX / 100), 3).ToString() + "x" + Math.Round(dimY).ToString());



            float somefink3 = 200f;
            Rect trec3 = new Rect(inRect);
            trec3 = trec3.CenteredOnXIn(inRect);
            trec3 = trec3.CenteredOnYIn(inRect);
            trec3.width = 40f;
            trec3.height = 40f;
            trec3.y = 200f;
            trec3.x = somefink3;
            Rect trec4 = new Rect(inRect);
            trec4 = trec4.CenteredOnXIn(inRect);
            trec4 = trec4.CenteredOnYIn(inRect);
            trec4.width = 40f;
            trec4.height = 40f;
            trec4.y = 200f;
            trec4.x = somefink3 + 45f;

            //float idk = 0f;
            if (dimX != 0)
            {
                if (Widgets.ButtonText(trec3, "+"))
                {
                    dimX += tloat;
                }
                if (Widgets.ButtonText(trec4, "-") && dimX > 0)
                {
                    dimX -= tloat;
                }
            }

            Rect trec44 = new Rect(inRect);
            trec44 = trec44.CenteredOnXIn(inRect);
            trec44 = trec44.CenteredOnYIn(inRect);
            trec44.width = 100f;
            trec44.height = 100f;
            trec44.y = 200f;
            trec44.x += 90f;
            Widgets.Label(trec44, "change caliber diamater");
            if (Widgets.ButtonText(trec44, "test"))
            {
                caldamage();
                calpen();
                CreatAmmoDefs.makethedefs(dimage, sharppen, Math.Round((dimX / 100), 3).ToString() + "x" + Math.Round(dimY).ToString(), masss, bulk);
            }
            //Widgets.LongLabel(0f, 200f, "placeholder", ref idk);


            float somefink = 400f;
            Rect trec33 = new Rect(inRect);
            trec33 = trec3.CenteredOnXIn(inRect);
            trec33 = trec3.CenteredOnYIn(inRect);
            trec33.width = 40f;
            trec33.height = 40f;
            trec33.y = 200f;
            trec33.x = somefink + 42f;
            Rect trec43 = new Rect(inRect);
            trec43 = trec43.CenteredOnXIn(inRect);
            trec43 = trec43.CenteredOnYIn(inRect);
            trec43.width = 40f;
            trec43.height = 40f;
            trec43.y = 200f;
            trec43.x = somefink;

            if (dimY != 0)
            {
                if (Widgets.ButtonText(trec33, "+Y"))
                {
                    dimY += tloat;
                }
                if (Widgets.ButtonText(trec43, "-Y") && dimY > 1)
                {
                    dimY -= tloat;
                }
            }



        }
        public float dimage;

        public float ichbinspeed;

        public float sharppen;

        public void caldamage()
        {
            float kek = ((dimX * dimY) / (762f * 51f)) * 20f;
            dimage = kek;
            Log.Message(kek.ToString());
        }

        public void calpen()
        {
            float keker = ((dimX * dimY) / (762f * 51f) * 7f);
            sharppen = (float)Math.Round(keker);
            Log.Message(sharppen.ToString());
        }

        //make this an util later
        public void makeammo(float dmg)
        {
            Log.Message(dmg.ToString());
        }


        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(700f, 600f);
            }
        }

        public void calbulkandmass()
        {
            float massass = ((dimX * dimY) / (762f * 51f) * 0.025f);
            float bulkass = ((dimX * dimY) / (762f * 51f) * 0.03f);
            masss = massass;
            bulk = bulkass;
        }
        public float masss;
        public float bulk;
    }
    public static class CreatAmmoDefs
    {
        public static void makethedefs(float dmg, float armorpen, string dims, float mass, float bulk)
        {
            FieldInfo[] fields = typeof(ThingDef).GetFields(
           BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            AmmoDef amdef = new AmmoDef();
            ThingDef projectile = new ThingDef();
            //var smth = DefDatabase<ThingDef>.AllDefs.ToList().Find(P => P.defName == "Bullet_762x51mmNATO_FMJ");
            //projectile = smth.clone
            foreach (FieldInfo fi in fields)
            {
                fi.SetValue(projectile, fi.GetValue(DefDatabase<ThingDef>.AllDefs.ToList().Find(P => P.defName == "Bullet_556x45mmNATO_FMJ")));
            }
            projectile.defName = Rand.Range(0, 256789) + "e" + Rand.Range(0, 30) + "ee" + Rand.Range(0, 30) + "ammosetdef" + dims;
            projectile.label = dims + "Bullet (FMJ)";
            projectile.thingClass = typeof(HandLoading.Bullet);
            projectile.modExtensions = new List<DefModExtension>();
            projectile.modExtensions.Add(new HandLoading.BulletModExtension { FixedDamage = dmg });
            projectile.graphicData = DefDatabase<ThingDef>.AllDefs.ToList().Find(P => P.defName == "Bullet_762x51mmNATO_FMJ").graphicData;
            projectile.graphic = DefDatabase<ThingDef>.AllDefs.ToList().Find(P => P.defName == "Bullet_762x51mmNATO_FMJ").graphic;
            amdef.drawGUIOverlay = true;


            foreach (FieldInfo fi in fields)
            {
                fi.SetValue(amdef, fi.GetValue(DefDatabase<ThingDef>.AllDefs.ToList().Find(P => P.defName == "Ammo_556x45mmNATO_FMJ")));
            }
            amdef.defName = Rand.Range(0, 256789) + "e" + Rand.Range(0, 30) + "ee" + Rand.Range(0, 30) + "ammosetdef" + dims;
            AmmoSetDef amsetdef = new AmmoSetDef();
            amsetdef.label = dims;
            amsetdef.defName = Rand.Range(0, 256789) + "e" + Rand.Range(0, 30) + "ee" + Rand.Range(0, 30) + "ammosetdef" + dims;
            amsetdef.ammoTypes = new List<AmmoLink>();
            amdef.ammoClass = DefDatabase<AmmoCategoryDef>.AllDefs.ToList().Find(L => L.defName == "FullMetalJacket");
            amsetdef.ammoTypes.Add(new AmmoLink(amdef, projectile));
            amdef.description += "Actual damage  is: " + dmg.ToString();
            amdef.thingClass = typeof(AmmoThing);
            amdef.graphicData = DefDatabase<ThingDef>.AllDefs.ToList().Find(P => P.defName == "Ammo_556x45mmNATO_FMJ").graphicData;
            amdef.graphic = DefDatabase<ThingDef>.AllDefs.ToList().Find(P => P.defName == "Ammo_556x45mmNATO_FMJ").graphic;
            amdef.uiIcon = DefDatabase<ThingDef>.AllDefs.ToList().Find(P => P.defName == "Ammo_556x45mmNATO_FMJ").uiIcon;
            projectile.projectile = new ProjectilePropertiesCE
            {
                speed = 150f,
                armorPenetrationSharp = armorpen,
                armorPenetrationBlunt = armorpen * 5,
                damageDef = DamageDefOf.Bullet,

            };
            foreach (AmmoLink alink in amsetdef.ammoTypes)
            {
                Log.Error(alink.ammo.ToString());
            }
            DefDatabase<AmmoSetDef>.Add(amsetdef);
            //amdef.
            amdef.label = dims + " (FMJ)";

            amdef.statBases.Add(new StatModifier { stat = StatDefOf.Mass, value = mass });
            amdef.statBases.Add(new StatModifier { stat = CE_StatDefOf.Bulk, value = bulk });
            Thing somefink = ThingMaker.MakeThing(amdef);
            Thing m = new Thing();
            somefink.stackCount = 500;
            GenThing.TryDropAndSetForbidden(somefink, Find.CurrentMap.mapPawns.FreeColonists.RandomElement().Position, Find.CurrentMap, ThingPlaceMode.Direct, out m, false);
            Log.Error(m.Position.ToString());


        }

    }
    public static class DebugToolsIan
    {
        [DebugAction("Rechamber", "Rechamber", false, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void adobus()
        {
            foreach (Thing thing in UI.MouseCell().GetThingList(Find.CurrentMap).ToList<Thing>())
            {
                ThingWithComps t = thing as ThingWithComps;
                if (t != null && t.AllComps.Any(L => L is CompAmmoUser))
                {
                    FieldInfo[] something = typeof(CompProperties_AmmoUser).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    CompProperties_AmmoUser amuser = new CompProperties_AmmoUser();
                    foreach (FieldInfo field in something)
                    {
                        field.SetValue(amuser, field.GetValue(t.TryGetComp<CompAmmoUser>().Props));
                    }
                    amuser.ammoSet = DefDatabase<AmmoSetDef>.AllDefs.ToList().FindAll(k => k.defName.Contains("ee")).RandomElement();
                    t.TryGetComp<CompAmmoUser>().props = amuser;
                    if (Find.World.GetComponent<CaliberRecreator>().compams == null)
                    {
                        Find.World.GetComponent<CaliberRecreator>().compams = new Dictionary<ThingWithComps, AmmoSetDef>();
                    }
                    Find.World.GetComponent<CaliberRecreator>().compams.Add(t, amuser.ammoSet);

                }
            }

        }
    }

}
