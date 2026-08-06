using UnityEngine;
using TMPro;
using System;

public class ClockSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clock;
    [SerializeField] private RectTransform backgroundCircleMask;
    [SerializeField] private float totalTime = 240f;

    private float timer = 0.0f;
    private int id;

    private void Start()
    {
        id = LeanTween.scale(backgroundCircleMask, Vector3.one, totalTime).setEase(LeanTweenType.easeInOutQuad).id;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        timer += deltaTime;
        
        if(timer >= 180f)
        {
            if(clock.text != "03:00 AM")
            {
                clock.text = "03:00 AM";
            }

            LeanTween.cancel(id);

            return;
        }

        DisplayTime();
    }

    private void DisplayTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(timer);

        clock.text = string.Format("0{0}:{1:00} AM", (int)time.TotalMinutes, time.Seconds);
    }
}
