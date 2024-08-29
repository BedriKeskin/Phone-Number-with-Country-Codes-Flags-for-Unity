using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhoneNumber
{
    public class CountryItem : MonoBehaviour
    {
        public Country country;

        private void Start()
        {
            transform.Find("Flag").GetComponent<Image>().sprite = Resources.Load<Sprite>("flags/" + country.code.ToLower());
            transform.Find("Name").GetComponent<TextMeshProUGUI>().text = country.name;
            transform.Find("DialCode").GetComponent<TextMeshProUGUI>().text = country.dial_code;

        }
    }
}
