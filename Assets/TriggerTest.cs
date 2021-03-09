using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class TriggerTest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogWarning("CollisionHappened");
    }

    IEnumerator flickerCollider()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        yield return null;
        collider.enabled = true;
        yield break;
    }

[Button]
    public void jjdasj()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    
}
