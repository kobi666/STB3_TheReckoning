using UnityEngine;
using System.Threading.Tasks;
using MyBox;
using Sirenix.OdinInspector;
//using Sirenix.Utilities;

[System.Serializable]
public class ShootOneProjectile : ProjectileAttackProperties 
{
    [ShowInInspector] 
    public override int ProjectileMultiplier { get; set; } = 1;

    private PoolObjectQueue<GenericProjectile> projectilePool;
    
    public override void InitializeAttack(GenericWeaponController parentWeapon)
    {
        if (!Projectiles.IsNullOrEmpty()) {
        projectilePool = Projectiles[0].CreatePool(parentWeapon.gameObject);
        }
        else
        {
            Debug.LogWarning("Empty Projectile");
        }
    }
    
    
    
    public override async void AttackFunction(Effectable singleTarget,Vector2 SingleTargetPosition)
    {
        if (AsyncAttackInProgress == false)
        { 
            AsyncAttackInProgress = true;
            GenericProjectile proj = projectilePool.Get();
            var transform = proj.transform;
            transform.rotation = ProjectileExitPoints[0].transform.rotation;
            transform.position = ProjectileExitPoints[0].transform.position;
            proj.TargetPosition = ProjectileFinalPoints[0].transform.position; // can also be projectile final point position
            proj.EffectableTarget = singleTarget ?? null;
            proj.Activate();
            AsyncAttackInProgress = false;
        }
        
    }

    

    
}


    
    
    
    [System.Serializable]
    public class ShootMultipleProjectilesOneAfterTheOther : ProjectileAttackProperties
    {
        [ShowInInspector]
        public override int ProjectileMultiplier { get; set; }

        public int NumOfProjectiles = 3;
        public float timebetweenProjectiles = 0.5f;

        private PoolObjectQueue<GenericProjectile> projectilePool;

        public override void InitializeAttack(GenericWeaponController parentWeapon)
        {
            projectilePool = Projectiles[0].CreatePool(parentWeapon.gameObject);
        }

        public override async void AttackFunction(Effectable singleTarget,
            Vector2 SingleTargetPosition)
        {
            int counter = NumOfProjectiles;
            float timeCounter = 0;
            if (AsyncAttackInProgress == false)
            {
                AsyncAttackInProgress = true;
                while (counter >= 0)
                {
                    if (timeCounter <= 0) {
                        GenericProjectile proj = projectilePool.GetInactive();
                        var transform = proj.transform;
                        transform.position = ProjectileExitPoints[0].transform.position;
                        transform.rotation = ProjectileExitPoints[0].transform.rotation;
                        proj.TargetPosition = ProjectileFinalPoints[0].transform.position; //can also be projectile final point position
                        proj.EffectableTarget = singleTarget ?? null;
                        
                        proj.Activate();
                        timeCounter = timebetweenProjectiles;
                        counter -= 1;
                    }
                    timeCounter -= StaticObjects.DeltaGameTime;
                    await Task.Yield();
                }

                AsyncAttackInProgress = false;
            }
        }
    }

