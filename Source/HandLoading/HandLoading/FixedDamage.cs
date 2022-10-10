using CombatExtended;
using Verse;

namespace HandLoading
{
    public class BulletModExtension : DefModExtension
    {
        public float FixedDamage = -1;
    }

    public class BootlegExtension : DefModExtension
    {
        public bool isthisreallyneedtbh = false;
    }
    public class Bullet : BulletCE
    {
        public override float DamageAmount
        {
            get
            {
                var result = def.GetModExtension<BulletModExtension>()?.FixedDamage ?? -1;
                return result < 0 ? def.projectile.GetDamageAmount(1) : result;
            }
        }
        
    }

}