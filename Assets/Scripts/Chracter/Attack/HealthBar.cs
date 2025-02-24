using UnityEngine;
using UnityEngine.UI;

namespace Chracter
{
    public class HealthBar : MonoBehaviour
    {
        // Start is called before the first frame update
    
        public Slider slider;
        public Color Low;
        public Color High;
        public GameObject Fill;
        public GameObject[] BuffDebuff;

        [SerializeField] Sprite Green;
        [SerializeField] Sprite Red;
        [SerializeField] Sprite Blue;
        void Start()
        {
            slider.value = float.MaxValue;
        }

        // Update is called once per frame
        void Update()
        {
           
        }


        public void SetHealthBarColor(string team)
        {
            if(team=="Team")
            {
                Fill.GetComponent<Image>().sprite = Green;
            }
            else if(team=="Enemy")
            {
                Fill.GetComponent<Image>().sprite = Red;
            }
            else
            {
                Fill.GetComponent<Image>().sprite = Blue;
            }
        }

    
        public void SetHealth(float health, float maxHealth)
        {       
            if (health <= 0)
            {
                slider.gameObject.SetActive(false);
                return;
            }
            
            slider.gameObject.SetActive(true);
            slider.value = health;
            slider.maxValue = maxHealth;
            //Fill.GetComponent<Image>().color = Color.Lerp(Low, High, slider.normalizedValue);
        }

        public void TowerHealth(float health, float maxHealth)
        {
            slider.gameObject.SetActive(true);
            slider.value = health;
            slider.maxValue = maxHealth;
            //Fill.GetComponent<Image>().color = Color.Lerp(Low, High, slider.normalizedValue);
        }

        public void ActiveBuff(int num)
        {
            BuffDebuff[num].SetActive(true);
        }

        public void DeActiveBuff(int num)
        {
            BuffDebuff[num].SetActive(false);
        }
    }
}
