using UnityEngine;
using UnityEngine.UI;
using Weelco.VRKeyboard;
using TMPro;
using OhmsLibraries.Utilities.Events;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace Weelco {
    public class KeyboardUtil : MonoBehaviour {

        public int maxOutputChars = 30;

        public TextMeshProUGUI inputFieldLabel;
        public VRKeyboardFull keyboard;

        public UnityStringEvent _onKeyboardTextUpdated;

        void Start () {
            if ( keyboard ) {
                keyboard.OnVRKeyboardBtnClick += HandleClick;
                keyboard.Init();
            }
        }

        void OnDestroy () {
            if ( keyboard ) {
                keyboard.OnVRKeyboardBtnClick -= HandleClick;
            }
        }

        private void OnEnable () {
            Clean();
            
        }

        private void OnDisable () {
            
        }

        public void Clean () {
            inputFieldLabel.text = "";
        }

        private void HandleClick ( string value ) {
            if ( value.Equals( VRKeyboardData.BACK ) ) {
                BackspaceKey();
            }
            else if ( value.Equals( VRKeyboardData.ENTER ) ) {
                EnterKey();
            }
            else {
                TypeKey( value );
            }
        }

        private void BackspaceKey () {
            if ( inputFieldLabel.text.Length >= 1 ) {
                inputFieldLabel.text = inputFieldLabel.text.Remove( inputFieldLabel.text.Length - 1, 1 );
                _onKeyboardTextUpdated.Invoke( inputFieldLabel.text );
            }
        }

        private void EnterKey () {
            // Add enter key handler
            
            Clean();
        }

        private void TypeKey ( string value ) {
            char[] letters = value.ToCharArray();
            for ( int i = 0; i < letters.Length; i++ ) {
                TypeKey( letters[i] );
            }
        }

        private void TypeKey ( char key ) {
            if ( inputFieldLabel.text.Length < maxOutputChars ) {
                inputFieldLabel.text += key.ToString();
                _onKeyboardTextUpdated.Invoke( inputFieldLabel.text );
            }
        }
    }
}