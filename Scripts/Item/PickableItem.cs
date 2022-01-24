using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    public class PickableItem : MonoBehaviour
    {
        [Header("Item")]
        public int amount = 1;

        protected Character _triggerCharacter = null;

        /// <summary>
        /// 檢查是否為玩家
        /// </summary>
        protected virtual bool CheckIsPlayer(GameObject triggerObject)
        {
            _triggerCharacter = triggerObject.GetComponent<Character>();
            if (_triggerCharacter == null)
            {
                return false;
            }
            if (_triggerCharacter.characterType != Character.CharacterTypes.Player)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 撿取
        /// </summary>
        protected virtual void PickItem()
        {
            CharacterInventory inventory = _triggerCharacter.GetComponent<CharacterInventory>();
            if (inventory == null)
                return;

            if (inventory.CanPickStock(amount) == false)
                return;

            inventory.AddStock(amount);
            Destroy(gameObject);
        }

        /// <summary>
        /// 觸發進入
        /// </summary>
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (CheckIsPlayer(other.gameObject) == false)
                return;

            PickItem();
        }

    }
}

