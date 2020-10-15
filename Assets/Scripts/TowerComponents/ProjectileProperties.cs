using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectileProperties : SerializedBehaviour
{
    public List<ProjectileBehaviorData> ProjectileTypes = new List<ProjectileBehaviorData>();
    public GenericProjectile ProjectileBase;
}
