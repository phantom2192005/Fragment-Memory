using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class ItemActionPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonPrefab;
        public void AddButton(string name, Action onPointerDownAction, Action onPointerUpAction, Action performAction)
        {
            GameObject button = Instantiate(buttonPrefab, transform);

            // Tìm image con nằm trong Border (dùng true để tìm cả khi đang bị tắt)
            Image childImage = button.transform.Find("Border")?.GetComponentInChildren<Image>(true);
            if (childImage != null)
                childImage.enabled = false; // Ẩn ban đầu

            // Thêm EventTrigger nếu chưa có
            EventTrigger trigger = button.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = button.AddComponent<EventTrigger>();

            // PointerDown
            EventTrigger.Entry entryDown = new EventTrigger.Entry();
            entryDown.eventID = EventTriggerType.PointerDown;
            entryDown.callback.AddListener((eventData) =>
            {
                if (childImage != null)
                    childImage.enabled = true; // Hiện hình ảnh khi nhấn xuống

                onPointerDownAction?.Invoke();
            });
            trigger.triggers.Add(entryDown);

            // PointerUp
            EventTrigger.Entry entryUp = new EventTrigger.Entry();
            entryUp.eventID = EventTriggerType.PointerUp;
            entryUp.callback.AddListener((eventData) =>
            {
                if (childImage != null)
                    childImage.enabled = false; // Ẩn hình ảnh khi thả chuột

                onPointerUpAction?.Invoke(); // callback tùy chỉnh nếu cần
                performAction?.Invoke();     // Thực hiện hành động chính ở đây
            });
            trigger.triggers.Add(entryUp);

            // Đặt tên cho button
            button.GetComponentInChildren<TMPro.TMP_Text>().text = name;
        }




        public void Toggle(bool val)
        {
            if (val)
                RemoveOldButtons();
            gameObject.SetActive(val);
        }

        public void RemoveOldButtons()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
