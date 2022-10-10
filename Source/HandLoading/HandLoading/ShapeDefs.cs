using Verse;
using Verse.AI;
using RimWorld;
namespace HandLoading
{
    public class RecipeShapeDef : Def
    {
        public ResearchProjectDef needed;

        public float damagemult;

        public float penmult;

        public int pelletcount = 1;

        public bool ischarge = false;

        public bool ismulti = false;

        public float spreadmult = 0f;

        public bool isfrag = false;

        public float workmult = 1f;

        public bool isbootleq = false;

        public TechLevel techlevel = TechLevel.Industrial;

        public bool dmgusestatpawn = false;

        public bool penusestatpawn = true;

        public bool isjacketed = false;

        public bool ThermoBar = false;
    }
}
