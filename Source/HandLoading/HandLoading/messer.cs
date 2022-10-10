using CombatExtended;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace HandLoading
{
    [HarmonyPatch(typeof(PawnInventoryGenerator), "GenerateInventoryFor")]
    static class HarmonyFuckAmmos
    {
      
        public static void Postfix(Pawn p, PawnGenerationRequest request)
        {
            Func<AmmoLink, float> waterfunc = x => water(x);
            if (p.Faction == null)
            {
                return;
            }
            float water(AmmoLink link)
            {
                if (link.ammo == null | link.projectile == null | link.projectile.projectile == null)
                {
                    return 0f;
                }
                var pear = 1f;
                var tiredness = (ProjectilePropertiesCE)link.projectile.projectile;
                var TT32 = tiredness.GetDamageAmount(1);
                if(TT32 < 1 && link.projectile.HasModExtension<BulletModExtension>())
                {
                    TT32 = (int)link.projectile.GetModExtension<BulletModExtension>().FixedDamage;
                }
                pear = ( (tiredness.armorPenetrationSharp * TT32) / ((link.ammo?.statBases?.Find(pp => pp.stat == StatDefOf.MarketValue)?.value ?? 3f) * (link.ammo?.statBases?.Find(pp => pp.stat == StatDefOf.Mass)?.value ?? 3f)) );
                Log.Message("Weight: " + pear + " for " + (link.ammo?.label ?? "fuck".Colorize(UnityEngine.Color.red)));
                return pear;
            }
            CompInventory inv = p.TryGetComp<CompInventory>();
            //Log.Message(p?.equipment?.Primary?.Label ?? "noequipment");
            if ((p.equipment?.Primary ?? null) != null && inv != null)
            {
                //Log.Message("good morning");
                if (Find.World.GetComponent<IhaveacoldandImustsneeze>().Factionammos.FindAll(tt33 => tt33.Key == p.Faction.ToString()).Any(tt33 => (tt33.Value.AmmoSetDefs.Contains(p.equipment.Primary?.TryGetComp<CompAmmoUser>()?.Props.ammoSet))))
                {
                    inv.container.RemoveAll(t33 => t33 is AmmoThing);

                    
                    var idkanmae = Find.World.GetComponent<IhaveacoldandImustsneeze>().Factionammos.FindAll(tt33 => tt33.Key == p.Faction.ToString()).FindAll(pp33 => (pp33.Value.AmmoSetDefs.Contains(p.equipment.Primary?.TryGetComp<CompAmmoUser>()?.Props.ammoSet)));
                    List<AmmoLink> ammolinkos = new List<AmmoLink>();
                    foreach (KeyValuePair<string, AmmoDef> keyvalue in idkanmae)
                    {
                        AmmoDef amdef = keyvalue.Value;
                        if (amdef != null)
                        {
                            AmmoSetDef caliber = amdef.AmmoSetDefs.First();
                            ammolinkos.Add(caliber.ammoTypes.Find(ps => ps.ammo == amdef));
                        }
                       
                    }
                    AmmoDef finalAmdef = ammolinkos.RandomElementByWeight(waterfunc).ammo;
                    if (finalAmdef == null)
                    {
                        finalAmdef = p.equipment.Primary.TryGetComp<CompAmmoUser>().Props.ammoSet.ammoTypes.RandomElement().ammo;
                    }
                    Log.Message(finalAmdef.label);
                    AmmoThing amthin = (AmmoThing)ThingMaker.MakeThing(finalAmdef);
                    amthin.stackCount = ((int)Math.Round(Rand.Range(p.kindDef.GetModExtension<LoadoutPropertiesExtension>().primaryMagazineCount.min, p.kindDef.GetModExtension<LoadoutPropertiesExtension>().primaryMagazineCount.max)) * p.equipment.Primary.TryGetComp<CompAmmoUser>().Props.magazineSize);
                    inv.container.TryAddOrTransfer(amthin, 20000);
                    Log.Message("es is working");
                    p.equipment.Primary.TryGetComp<CompAmmoUser>().LoadAmmo(amthin);
                }
            }

         


        }
    }
    public class amigus : Mod
    {
        public static Settings settings;

        public amigus(ModContentPack content) : base(content)
        {

            Harmony har = new Harmony("Caulaflower.HandLoading.CustomAmmo");
            har.PatchAll();
            
        }
    }
}
