using UnityEngine;

public class testscript2 : MonoBehaviour
{
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + Vector2.right, 0.05f);
    }
}
