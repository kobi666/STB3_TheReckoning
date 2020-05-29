using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTower : MonoBehaviour
{
    // Start is called before the first frame update
    public Projectile TestProj;
    public Projectile proj;
    PoolObjectQueue<Projectile> pool;

    private void Start() {
        //pool = GameObjectPool.Instance.GetProjectilePool(proj);
        //TestProj = pool.Get();
        //TestProj.gameObject.SetActive(true);
        //TestProj.TestTarget = this.gameObject;
    }


}
