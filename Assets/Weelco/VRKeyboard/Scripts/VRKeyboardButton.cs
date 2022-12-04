using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Weelco.VRKeyboard {

    public class VRKeyboardButton : VRKeyboardBase {
        
        private Button button;
        private TextMeshProUGUI label;
        private Image icon;

        void OnDestroy() {
            if (Initialized) {
                button.onClick.RemoveListener(HandleClick);
            }
        }

        public void Init() {
            if (!Initialized) {
                Transform iconTransform = transform.Find("Image");
                if (iconTransform != null)
                    icon = iconTransform.GetComponent<Image>();

                label = transform.Find("Text").GetComponent<TextMeshProUGUI>();
                label.enabled = (icon == null);

                button = transform.GetComponent<Button>();
                button.onClick.AddListener(HandleClick);

                Initialized = true;
            }
        }

        public void SetKeyText(string value, bool ignoreIcon = false) {
            label.text = value;
            if (icon != null) {
                label.enabled = ignoreIcon;
                icon.enabled = !ignoreIcon;
            }
        }        

        private void HandleClick() {
            if (OnVRKeyboardBtnClick != null) {
                OnVRKeyboardBtnClick(label.text);
            }                
        }
    }
}