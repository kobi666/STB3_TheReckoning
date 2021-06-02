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

    
    public TowerSlotController TargetTowerSlot;

    private bool movelock;
    public async void MoveToNewTarget(Vector2 TargetPos, TowerSlotController tsc)
    {
        
        if (!movelock)
        {
            TargetTowerSlot = tsc != null ? tsc : null;
            var tPos = TargetPos;
            movelock = true;
            float startTime = Time.time;
            float t;
            while ((Vector2)transform.position != tPos)
            {
                t = (Time.time - startTime) / ParentSelector.MovementDuration;
                transform.position = new Vector2(Mathf.SmoothStep(transform.position.x, tPos.x, t),Mathf.SmoothStep(transform.position.y, tPos.y, t));
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
