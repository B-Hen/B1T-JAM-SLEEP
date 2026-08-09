using UnityEngine;

public class MaskBackground : MonoBehaviour
{
    [SerializeField] private RectTransform backgroundMask;

    private void Start()
    {
        LeanTween.scale(backgroundMask.gameObject, new Vector3(40f, 40f, 40f), 1f).setEase(LeanTweenType.easeInOutQuad);
    }
}
