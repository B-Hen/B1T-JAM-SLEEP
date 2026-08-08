using System;
using UnityEngine;

public class MaskFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform mask;

    void Start()
    {

    }

    void LateUpdate()
    {
        if (Camera.main == null) return;

        // Convert the player's 3D position + offset into 2D screen space pixels
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(player.position);

        // Assign the calculated coordinates to the UI element
        mask.position = screenPosition;
    }
}
