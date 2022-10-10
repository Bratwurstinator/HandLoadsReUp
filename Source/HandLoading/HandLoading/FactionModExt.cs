using CombatExtended;
using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace HandLoading
{
    public class IhaveacoldandImustsneeze : WorldComponent
    {
        public IhaveacoldandImustsneeze(World world) : base(world)
        {
        }
        public List<KeyValuePair<string, AmmoDef>> Factionammos = new List<KeyValuePair<string, AmmoDef>>();

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref Factionammos, "factionammos", LookMode.Value, LookMode.Def);
          


            base.ExposeData();
        }
    }


}
