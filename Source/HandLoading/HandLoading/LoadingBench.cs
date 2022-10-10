using HandLoading.CustomCalibers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;


namespace HandLoading
{
    // Token: 0x0200004F RID: 79
    public class LoadingBench : Building, IThingHolder
    {
        public ThingOwner<Thing> ingredients = new ThingOwner<Thing>();


        public bool StorageTabVisible
        {
            get
            {
                return true;
            }
        }

        public void doob()
        {
            //Find.WindowStack.Add(new HandLoading.HandLoadingWindow(this.Position, this.Map));
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return this.ingredients;
        }
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            //if (!respawningAfterLoad)
            //doob();
        }
    }
    public class BenchComp : ThingComp
    {
        public bool dee = false;
        public BenchCompProps Props => (BenchCompProps)this.props;
        public override void Initialize(CompProperties props)
        {





        }
        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            yield return new FloatMenuOption("hand load ammo", delegate { selPawn.jobs.StartJob(new Verse.AI.Job { def = DefDatabase<JobDef>.AllDefs.ToList().Find(TT => TT.defName == "gotobench"), targetA = this.parent }, Verse.AI.JobCondition.InterruptForced); });
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            bool a = false;

            if (a)
            {
                yield return new Command_Action()
                {
                    defaultLabel = "design custom ammunition",
                    defaultDesc = "design your own new ammunition",
                    icon = ContentFinder<Texture2D>.Get("Things/Ammo/Rifle/AP/AP_c", true),

                    //action = delegate { Log.Error(this.parent.Position.ToString()); Log.Message(ParentHolder.ToString()); Find.WindowStack.Add(winda);}


                };
                yield return new Command_Action()
                {
                    defaultLabel = "design custom caliber",
                    defaultDesc = "design your own new caliber",
                    icon = ContentFinder<Texture2D>.Get("Things/Ammo/Rifle/AP/AP_c", true),

                    action = delegate { Log.Error(this.parent.Position.ToString()); Log.Message(ParentHolder.ToString()); Find.WindowStack.Add(new CaliberMacher()); }


                };


            }



        }
    }
    public class BenchCompProps : CompProperties
    {

        public BenchCompProps()
        {
            this.compClass = typeof(BenchComp);
        }

        public BenchCompProps(Type compClass) : base(compClass)
        {
            this.compClass = compClass;
        }
    }
}
