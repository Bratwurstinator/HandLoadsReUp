using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace HandLoading
{
    class gotobench : JobDriver
    {








        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
        }


        protected override IEnumerable<Toil> MakeNewToils()
        {
            Toil foil = Toils_Goto.Goto(TargetIndex.A, PathEndMode.ClosestTouch);
            foil.AddFinishAction(delegate
            {
                HandLoadingWindow winda = new HandLoading.HandLoadingWindow(GetActor().Position, GetActor().Map, GetActor()) { }; Find.WindowStack.Add(winda);
            });
            yield return foil;


        }
    }
}
