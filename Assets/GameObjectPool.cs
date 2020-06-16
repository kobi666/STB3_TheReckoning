using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public Dictionary<string, PoolObjectQueue<Projectile>> ProjectilesPool = new Dictionary<string, PoolObjectQueue<Projectile>>();

    void CreateNewPool<T> (Dictionary<string,PoolObjectQueue<T>> dict, T prefab) where T : Component,IQueueable<T> {
        GameObject placeholder = Instantiate(new GameObject());
        placeholder.transform.parent = this.transform;
        placeholder.name = "_PlaceHolder_" + prefab.gameObject.name;
        dict.Add(prefab.name, new PoolObjectQueue<T>(prefab, 20,placeholder));
    }

    public PoolObjectQueue<Projectile> GetProjectilePool(Projectile prefab) {
        if (ProjectilesPool.ContainsKey(prefab.name)) {
            return ProjectilesPool[prefab.name];
        }
        else {
            CreateNewPool<Projectile>(ProjectilesPool, prefab);
            return ProjectilesPool[prefab.name];
        }
    }



    public static GameObjectPool Instance {get ; private set;}

    private void Awake() {
        Instance = this;
    }
}
