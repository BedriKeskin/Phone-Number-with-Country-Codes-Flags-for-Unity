using TMPro;
using UnityEngine;

namespace PhoneNumber
{
    public class PhoneNumberValidator : MonoBehaviour
    {
        private TMP_InputField _inputField;

        private void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
            _inputField.onValueChanged.AddListener(FormatPhoneNumber);
        }

        private void FormatPhoneNumber(string text)
        {
            string textFormatted = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9]", string.Empty);
            if (textFormatted.Length > 10) textFormatted = textFormatted[..10];

            if (textFormatted.Length < 3)
            { }
            else if (textFormatted.Length == 3)
            { }
            else if (textFormatted.Length == 4)
            {
                textFormatted = $"({textFormatted.Substring(0, 3)}) {textFormatted.Substring(3, 1)}";
            }
            else if (textFormatted.Length <= 6)
            {
                textFormatted = $"({textFormatted.Substring(0, 3)}) {textFormatted.Substring(3, textFormatted.Length - 3)}";
            }
            else
            {
                textFormatted = $"({textFormatted.Substring(0, 3)}) {textFormatted.Substring(3, 3)} - {textFormatted.Substring(6, textFormatted.Length - 6)}";
            }
            
            // if (textFormatted.Length != text.Length)
            // {
            //     number.ForceLabelUpdate();
            //     number.MoveTextEnd(false);
            // }
            
            _inputField.text = textFormatted;
        }
    }
}
