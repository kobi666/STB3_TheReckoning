using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class CanvasTextController : MonoBehaviour
{
    [Required]
    public TextMeshProUGUI TextMeshProUGUI;

    public float FadeInTime = 2;
    public int StayTimeMS = 3000;
    public float FadeOutTime = 2;

    private Color BaseColor;
    private void Awake()
    {
        BaseColor = TextMeshProUGUI.color;
        TextMeshProUGUI.color = Color.clear;
    }

    public async void FadeTextInAndOut()
    {
        float FIcounter = 0;
        while (FIcounter <= FadeInTime)
        {
            TextMeshProUGUI.color = Color.Lerp(Color.clear, BaseColor, FIcounter / FadeInTime);
            FIcounter += Time.deltaTime;
            await Task.Yield();
        }

        await Task.Delay(StayTimeMS);

        float FOCounter = 0;
        while (FOCounter <= FadeOutTime)
        {
            FOCounter += Time.deltaTime;
            TextMeshProUGUI.color = Color.Lerp(BaseColor,Color.clear, FOCounter / FadeInTime);
            await Task.Yield();
        }
        
        Debug.LogWarning("U DED DONE");

    }
    
    
    
    
}