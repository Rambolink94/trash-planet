using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemWheelController : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Transform outerHandle;
    [SerializeField] Transform innerHandle;

    // Update is called once per frame
    void Update()
    {
        RotateHandle(outerHandle);
        RotateHandle(innerHandle);
    }

    private void RotateHandle(Transform handle)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3? screenPosition = CalculateWorldToScreenPos(handle.position);
        if (screenPosition == null) return;

        Vector2 offset = mousePosition - (Vector2)screenPosition;
        if (offset != Vector2.zero)
        {
            handle.rotation = Quaternion.FromToRotation(Vector3.down, offset);
        }
    }

    private Vector3? CalculateWorldToScreenPos(Vector3 worldPos)
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            return canvas.worldCamera.WorldToScreenPoint(worldPos);
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Vector3 screenPos = canvas.transform.InverseTransformPoint(worldPos);
            var rectTransform = canvas.transform as RectTransform;

            screenPos.x += rectTransform.rect.width * 0.5f * rectTransform.localScale.x;
            screenPos.y += rectTransform.rect.height * 0.5f * rectTransform.localScale.y;

            return screenPos;
        }

        return null;
    }
}
