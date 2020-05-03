using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController3 : EnemyUnitController
{
    // Start is called before the first frame update
    
    private void Update() {
        
            transform.Translate(Vector3.right * 1.2f * Time.deltaTime);
        
    }
    // Update is called once per frame
    
}
