using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFinalPoint : MonoBehaviour
{
    WeaponController parentTowerComponent;
    private void Start() {
        parentTowerComponent = transform.parent.GetComponent<WeaponController>() ?? null;
        
        if (parentTowerComponent != null) {
        transform.position = new Vector2(transform.position.x + parentTowerComponent.Data.Radius, transform.position.y);
        }
        else {
            transform.position = new Vector2(transform.position.x + 3, transform.position.y);
        }
    }

    
    
}
