using System.Collections.Generic;
using System;

public class TestOrbitalGunsController : OrbitalGunsControllerGeneric
{
    // Start is called before the first frame update
    public override WeaponRotator Rotator {get; set;}
    public override List<OrbitalWeaponGeneric> OrbitalGuns {get; set;}


    public override void InitComponent()
    {
        throw new NotImplementedException();
    }

    public override void PostAwake() {
        
        Rotator = GetComponent<WeaponRotator>() ?? null;
        Rotator.parentTowerComponent = this;
        OrbitalGuns =  new List<OrbitalWeaponGeneric>();
        
    }

    private void Start() {
        for (int i = 0; i < Data.orbitalData.initialNumberOfOrbitals; i++)
        {
            AddOrbitalGun();
        }
        
    }

    public override List<Effect> GetEffectList()
    {
        throw new NotImplementedException();
    }

    public override void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        throw new NotImplementedException();
    }

    public override List<TagDetector> GetTagDetectors()
    {
        throw new NotImplementedException();
    }

    public override void UpdateRange(float RangeSizeDelta, List<TagDetector> detectors)
    {
        throw new NotImplementedException();
    }
}
