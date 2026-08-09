using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image outerStroke, innerStroke;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int nextScene;
    [SerializeField] private bool quit;
    [SerializeField] private Color light, dark;
    [SerializeField] private RectTransform backgroundMask;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(quit)
        {
            Application.Quit();
            return;
        }

        if(backgroundMask != null)
        {
            LeanTween.scale(backgroundMask.gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
            {
                SceneManager.LoadScene(nextScene);
            });
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outerStroke.color = dark;
        innerStroke.color = light;
        text.color = dark;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outerStroke.color = light;
        innerStroke.color = dark;
        text.color = light;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
