using UnityEngine;

public class MagooCatAnimation : MonoBehaviour
{
    [SerializeField] private Transform magooCat;

    private void Start()
    {
        LeanTween.moveLocalX(magooCat.gameObject, -12f, 10f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong();
    }
}
