using UnityEngine;

public class testplayerunit : MonoBehaviour
{
    private GenericUnitController gnc;

    private void Start()
    {
        gnc = GetComponent<GenericUnitController>();
        gnc.Data.DynamicData.BasePosition = transform.position;
    }
}
