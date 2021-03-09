using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Shaker : MonoBehaviour
{
    [Header("Info")]
    private Vector3 _startPos;
    private float _timer;
    private Vector3 _randomPos;

    [Header("Settings")]
    [Range(0f, 2f)]
    public float _time = 0.2f;
    [Range(0f, 2f)]
    public float _distance = 0.1f;
    [Range(0f, 0.1f)]
    public float _delayBetweenShakes = 0f;

    public bool ShakeInProgress = false;

    private void Awake()
    {
        
    }

    private void OnValidate()
    {
        if (_delayBetweenShakes > _time)
            _delayBetweenShakes = _time;
    }
    
    public void BeginShaking()
    {
        if (ShakeInProgress == false) {
        StopAllCoroutines();
        StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        if (ShakeInProgress == false) {
        _startPos = transform.position;
        }
        else
        {
            yield break;
        }
        _timer = 0f;
        ShakeInProgress = true;
        while (_timer < _time)
        {
            _timer += Time.deltaTime;

            _randomPos = _startPos + (Random.insideUnitSphere * _distance);

            transform.position = _randomPos;

            if (_delayBetweenShakes > 0f)
            {
                yield return new WaitForSeconds(_delayBetweenShakes);
            }
            else
            {
                yield return null;
            }
        }

        while (transform.position != _startPos)
            {
                transform.position = Vector2.MoveTowards(transform.position, _startPos, _distance);
                yield return null;
            }

        ShakeInProgress = false;

    }

}