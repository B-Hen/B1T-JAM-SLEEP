using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class ClockSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clock;
    [SerializeField] private RectTransform backgroundCircleMask, topEyeLid, bottomEyeLid, playerHealthBar;
    [SerializeField] private float totalTime = 240f;

    private float timer = 0.0f;
    private int id;
    private Coroutine closeEyeCoroutine;

    private void Start()
    {
        id = LeanTween.scale(backgroundCircleMask, Vector3.one, totalTime).setEase(LeanTweenType.easeInOutQuad).id;
        CloseEyeAnimation();
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

            if (playerHealthBar.localScale.x > 0) Debug.Log("Player Survived");

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

    private void CloseEyeAnimation()
    {
        if(closeEyeCoroutine != null)
        {
            StopCoroutine(closeEyeCoroutine);
            closeEyeCoroutine = null;
        }

        closeEyeCoroutine = StartCoroutine(CloseEyeCoroutine());
    }

    private IEnumerator CloseEyeCoroutine()
    {
        LeanTween.cancel(topEyeLid.gameObject);
        LeanTween.cancel(bottomEyeLid.gameObject);

        while (true)
        {
            yield return new WaitForSeconds(24f);
            LeanTween.moveLocalY(topEyeLid.gameObject, 45f, 1f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.moveLocalY(bottomEyeLid.gameObject, -45f, 1f).setEase(LeanTweenType.easeInOutQuad);
            yield return new WaitForSeconds(1f);
            LeanTween.moveLocalY(topEyeLid.gameObject, 90f, 1f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.moveLocalY(bottomEyeLid.gameObject, -90f, 1f).setEase(LeanTweenType.easeInOutQuad);
            yield return new WaitForSeconds(1f);
            LeanTween.moveLocalY(topEyeLid.gameObject, 45f, 1f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.moveLocalY(bottomEyeLid.gameObject, -45f, 1f).setEase(LeanTweenType.easeInOutQuad);
            yield return new WaitForSeconds(1f);
            LeanTween.moveLocalY(topEyeLid.gameObject, 180f, 1f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.moveLocalY(bottomEyeLid.gameObject, -180f, 1f).setEase(LeanTweenType.easeInOutQuad);
        }
    }
}
