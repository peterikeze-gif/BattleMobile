
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    public RectTransform background;
    public RectTransform handle;

    public float handleRange = 80f;

    public Vector2 InputDirection { get; private set; }

    private void Start()
    {
        ResetJoystick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null || handle == null)
            return;

        Vector2 position;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out position))
        {
            Vector2 size = background.rect.size;

            position.x /= size.x;
            position.y /= size.y;

            InputDirection = Vector2.ClampMagnitude(
                new Vector2(position.x * 2f, position.y * 2f),
                1f
            );

            handle.anchoredPosition =
                InputDirection * handleRange;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetJoystick();
    }

    private void ResetJoystick()
    {
        InputDirection = Vector2.zero;

        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }
}
