using System.Collections;
using UnityEngine;
using TMPro;



public class TowerPosIndicator : MonoBehaviour
{
    public TextMeshPro text;
    public SpriteRenderer SR;

    private IEnumerator moveCoroutine;
    public void MoveToNewTarget(Vector2 TargetPos)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = SelectorUtils.SmoothMove(transform, TargetPos, 0.15f, null, null);
        StartCoroutine(moveCoroutine);
    }
    
    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }
    
    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        SR.enabled = false;
    }


    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        text.text = "";
    }
}
