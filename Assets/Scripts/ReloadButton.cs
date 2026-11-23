
using UnityEngine;
using UnityEngine.EventSystems;

public class ReloadButton : MonoBehaviour,
    IPointerDownHandler
{
    public WeaponSystem weapon;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (weapon != null)
        {
            weapon.ReloadServerRpc();
            Debug.Log("RELOADING WEAPON!");
        }
    }
}
