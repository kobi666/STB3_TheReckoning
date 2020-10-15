using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectileAttack : SerializedBehaviour
{
    
    
    [HideLabel]
    [BoxGroup] public List<ProjectileProperties> ProjectilePropertieses = new List<ProjectileProperties>();
}
