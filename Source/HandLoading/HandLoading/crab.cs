using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using CombatExtended;
using UnityEngine;

namespace HandLoading
{
    public class HaulComp : ThingComp
    {
        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            foreach (Thing spot in Find.CurrentMap.listerBuildings.AllBuildingsColonistOfDef(somedefofidk.HLspot))
            {
               
                foreach(Thing sing in spot.Position.GetThingList(Find.CurrentMap))
                {
                    Log.Message(sing.Label);
                }
                //
                if (spot.Position.GetThingList(Find.CurrentMap).Any(l => l.def == this.parent.def) | !spot.Position.GetThingList(Find.CurrentMap).Any( D => (D is AmmoThing) ))
                {
                    yield return new FloatMenuOption("Haul to spot " + spot.Position.ToString(), delegate
                    {
                        Job job = new Job { def = somedefofidk.gotospot, targetA = spot, targetB = this.parent };
                        job.count = this.parent.stackCount;
                        selPawn.jobs.StartJob(job, JobCondition.InterruptForced);
                    });
                }
               
            }
          
        }
        public override void PostPostMake()
        {
            Log.Message("hello world");
            base.PostPostMake();
        }
    }

    class haul_to_spot : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
        }


        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
            yield return Toils_Haul.CarryHauledThingToCell(TargetIndex.A, PathEndMode.OnCell);
            yield return Toils_General.Do(delegate
            {
                if (TargetA.Thing.Position.GetThingList(Find.CurrentMap).Any(F => F.def == TargetB.Thing.def))
                {
                    TargetA.Thing.Position.GetThingList(Find.CurrentMap).Find(G => G.def == TargetB.Thing.def).stackCount += TargetB.Thing.stackCount;
                    TargetB.Thing.Destroy();

                }
            });


        }
    }
    [DefOf]
    public class somedefofidk : DefOf
    {
        public static JobDef gotospot;

        public static ThingDef HLspot;
    }
}
