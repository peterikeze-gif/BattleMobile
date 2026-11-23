
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    public WeaponSystem weapon;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (weapon != null)
        {
            weapon.StartFiring();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (weapon != null)
        {
            weapon.StopFiring();
        }
    }

    public void OnDisable()
    {
        if (weapon != null)
        {
            weapon.StopFiring();
        }
    }
}
