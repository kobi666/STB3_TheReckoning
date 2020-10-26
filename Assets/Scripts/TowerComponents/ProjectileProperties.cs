using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectileProperties : SerializedBehaviour
{
    public List<ProjectileEffect> ProjectileTypes = new List<ProjectileEffect>();
    public GenericProjectile ProjectileBase;
}
