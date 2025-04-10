using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class ConsumableItem : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modifierDatas = new List<ModifierData>();
        public string ActionName => "Consume";

        [field: SerializeField]
        public AudioClip actionSFX {  get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            foreach (ModifierData data in modifierDatas)
            {
                data.statModifier.AffectCharacter(character, data.value);
            }
            return true;
        }

    }

}