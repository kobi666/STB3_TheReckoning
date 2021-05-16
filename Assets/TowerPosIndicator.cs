using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class TowerPosIndicator : MonoBehaviour
{
    public TextMeshPro text;
    public SpriteRenderer SR;
    [Required]
    public SelectorTest2 ParentSelector;

    public Vector2 tPos;

    private bool movelock;
    public async void MoveToNewTarget(Vector2 TargetPos)
    {
        if (!movelock)
        {
            tPos = TargetPos;
            movelock = true;
            float startTime = Time.time;
            float t;
            while ((Vector2)transform.position != TargetPos)
            {
                t = (Time.time - startTime) / ParentSelector.MovementDuration;
                transform.position = new Vector2(Mathf.SmoothStep(transform.position.x, TargetPos.x, t),Mathf.SmoothStep(transform.position.y, TargetPos.y, t));
                await Task.Yield();
            }

            movelock = false;
        }
        
    }
    
    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }
    
    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        /*SR.enabled = false;*/
    }


    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        text.text = "";
    }
}
