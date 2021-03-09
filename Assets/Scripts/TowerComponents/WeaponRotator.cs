public class WeaponRotator : Rotator<OrbitalWeaponGeneric>
{
    public OrbitalGunsControllerGeneric parentTowerComponent;
    public TowerComponent ParentTowerComponent {get=> parentTowerComponent;set { ParentTowerComponent = value;}}
    // Start is called before the first frame update
    public override int MaxNumOfOrbitals {
        get => ParentTowerComponent.Data.orbitalData.maxNumberOfOrbitals;
    }
}
