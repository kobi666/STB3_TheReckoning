using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class testloop : MonoBehaviour
{
    private List<float> timnes = new List<float>()
    {
        1f,5f,8f
    };

    private float counter;
    async void test()
    {
        foreach (var VARIABLE in timnes)
        {
            while (counter < VARIABLE)
            {
                counter += Time.deltaTime;
                Debug.LogWarning(VARIABLE);
                await Task.Yield();
            }
        }
    }
    
    

    
}
