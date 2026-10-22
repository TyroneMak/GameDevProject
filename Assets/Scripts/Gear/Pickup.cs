using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// when the player walks into it, the item goes into their hotbar, gets equipped, gets deleted
/// can be used as a template to pick up different gear
/// </summary>
public class Pickup : MonoBehaviour
{
    [Tooltip("The booster InventoryItem asset. Its id must match Boosting's Item Id (\"booster\").")] // change line 
    public InventoryItem item;
    bool _taken;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || _taken){
            var inv = other.GetComponentInParent<Inventory>();
            if (inv.Add(item)){
                inv.EquipById(item.id);
                Destroy(gameObject);
                 _taken = true;
            }
        }
    }
}
