using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PhoneNumber
{
    [Serializable]
    public class Country {  
        public string name;  
        public string flag;
        public string code;
        public string dial_code;  
    }
    
    [Serializable]
    public class Countries
    {
        public List<Country> countries;
    }
    
    public class PhoneNumberPrefab : MonoBehaviour
    {
        public CountryItem countryItem;
        public Toggle toggle;
        public TMP_Text flagText;
        public Image flag, arrow;
        public TMP_InputField number;
        public string dialCode = "+90";
        
        public GameObject _fullScreenObject, countryList;
        
        private void Start()
        {
            StartCoroutine(InitializeFullScreenObject());

            toggle.onValueChanged.AddListener(ShowHideFullScreenObject);
        }
        
        private IEnumerator InitializeFullScreenObject()
        {
            yield return null;
            
            RectTransform rectTransform = _fullScreenObject.GetComponent<RectTransform>();
            rectTransform.SetParent(GetPanelUnderSafeArea(), false);  
            rectTransform.anchorMin = new Vector2(0, 0);  
            rectTransform.anchorMax = new Vector2(1, 1);  
            rectTransform.offsetMin = new Vector2(0, 0);  
            rectTransform.offsetMax = new Vector2(0, 0);
            
            _fullScreenObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                toggle.isOn = false;
                arrow.sprite = Resources.Load<Sprite>("ArrowDown");
                _fullScreenObject.SetActive(false);
            });
            
            //Set CountryList position
            Transform frameLeft = transform.Find("FrameLeft");
            Vector2 positionFrameLeft = frameLeft.transform.position;
            Vector2 countryListShouldBePosition = new(positionFrameLeft.x, positionFrameLeft.y - frameLeft.GetComponent<RectTransform>().rect.width * (480f/645f) * 0.95f); //480f/645f is aspect ratio of the image in FrameLeft 
            countryList.transform.position = countryListShouldBePosition;
            //Set CountryList size
            RectTransform rectCountryList = countryList.GetComponent<RectTransform>();
            rectCountryList.sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, 2 * GetComponent<RectTransform>().rect.height);
            
            TextAsset jsonFile = Resources.Load<TextAsset>("countries");
            List<Country> countries = JsonUtility.FromJson<Countries>("{\"countries\":" + jsonFile.text + "}").countries;
            List<Country> countriesWithFlag = (from country in countries let sprite = Resources.Load<Sprite>("flags/" + country.code.ToLower()) where sprite != null select country).ToList();

            ScrollRect scrollRectCountryList = countryList.GetComponent<ScrollRect>();

            foreach (Country country in countriesWithFlag)
            {
                CountryItem _country = Instantiate(countryItem, scrollRectCountryList.content);
                _country.country = country;
                _country.GetComponent<Button>().onClick.AddListener(() =>
                {
                    dialCode = country.dial_code;
                    flag.sprite = Resources.Load<Sprite>("flags/" + country.code.ToLower());
                    
                    toggle.isOn = false;
                    arrow.sprite = Resources.Load<Sprite>("ArrowDown");
                    _fullScreenObject.SetActive(false);
                });
            }
            
            _fullScreenObject.SetActive(false);
        }
        
        private void ShowHideFullScreenObject(bool isOn)
        {
            if (isOn)
            {
                arrow.sprite = Resources.Load<Sprite>("ArrowUp");
                _fullScreenObject.SetActive(true);
            }
            else
            {
                arrow.sprite = Resources.Load<Sprite>("ArrowDown");
                _fullScreenObject.SetActive(false);
            }
        }
        
        private RectTransform GetPanelUnderSafeArea()
        {
            Transform parent = transform;
            List<Transform> transforms = new List<Transform>();
    
            while(parent != GetComponentInParent<Canvas>().rootCanvas.transform)
            {
                transforms.Add(parent);
                parent = parent.parent;
            }

            return transforms[^2].GetComponent<RectTransform>();
        }
    }
}
