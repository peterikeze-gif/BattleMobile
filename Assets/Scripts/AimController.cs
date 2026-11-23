
using UnityEngine;
using UnityEngine.EventSystems;

public class AimController : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    public WeaponSystem weapon;
    public RectTransform aimArea;

    private int activePointerId = -1;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (activePointerId != -1)
            return;

        activePointerId = eventData.pointerId;
        UpdateAim(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != activePointerId)
            return;

        UpdateAim(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId != activePointerId)
            return;

        activePointerId = -1;
    }

    private void UpdateAim(PointerEventData eventData)
    {
        if (weapon == null)
            return;

        weapon.SetAimScreenPosition(eventData.position);
    }
}
