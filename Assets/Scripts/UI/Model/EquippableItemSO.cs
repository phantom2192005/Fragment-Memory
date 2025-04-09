using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        public string ActionName => "Equip";

        public string EquipmentCategory;

        public GameObject Weapon;

        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            WeaponHandler weaponHandler = FindFirstObjectByType<WeaponHandler>();
            if (weaponHandler != null)
            {
                weaponHandler.EquipWeapon(Weapon, this, itemState == null ? DefaultParametersList : itemState);
                return true;
            }
            else
            {
                Debug.Log("WeaponHandler is null");
            }
            return false;
        }
    }
}