using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using CombatExtended;
using UnityEngine;

namespace HandLoading
{
   public class Tab_FactionAmmos : MainTabWindow
    {
        public float RectFloater;

        public IhaveacoldandImustsneeze sneeze
        {
            get
            {
                return Find.World.GetComponent<IhaveacoldandImustsneeze>();
            }
        }
        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard standard = new Listing_Standard();
            standard.Begin(inRect);
            foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
            {
                var avariable = (sneeze.Factionammos.ToList().FindAll(t33 => t33.Key == faction.ToString()));
                foreach (KeyValuePair<string, AmmoDef> amderf in (sneeze.Factionammos.ToList().FindAll(t33 => t33.Key == faction.ToString())))
                {
                    var amderfers = (sneeze.Factionammos.ToList().FindAll(t33 => t33.Key == faction.ToString())).FindAll(x => x.Value.AmmoSetDefs.Contains(amderf.Value.AmmoSetDefs.First()));
                    if(amderfers.Count > avariable.Count)
                    {
                        Log.Message(amderfers.First().Value.ToString());
                        avariable = amderfers;
                    }
                    
                }
                
                standard.Label(faction.Name + " amount of handloads: " + (sneeze.Factionammos?.ToList().FindAll(t33 => t33.Key == faction.ToString())?.Count ?? 0) + " Caliber with most hand loads: " + (avariable?.First().Value.AmmoSetDefs.First()?.label ?? "no info"));
                standard.GapLine(12f);
                //standard.EndSection(standard);
            }
            standard.End();
        }
    }
}
