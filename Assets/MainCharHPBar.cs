using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Chracter;
using UnityEngine;
using UnityEngine.UI;


public class MainCharHPBar : MonoBehaviour
{
    [SerializeField]
    private BaseCharacter character = null;
    
    [SerializeField]
    private Slider slider = null;
    
    [SerializeField]
    private Text text = null;
    
    private bool isDead = false;
    
    [SerializeField]
    private Text TxtCountDown = null;
    
    // Start is called before the first frame update
    void Start()
    {
        TxtCountDown.text = "20";
        TxtCountDown.gameObject.SetActive(false);
        slider.maxValue = character.MaxHealth;
        slider.value = character.MaxHealth;
        text.text = character.MaxHealth + "/" + character.MaxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            SetHealth();
        } 
    }
    
    public void SetHealth()
    {
        slider.value = character.CurrentHealth;
        slider.maxValue = character.MaxHealth;
        text.text = character.CurrentHealth + "/" + character.MaxHealth;
        
        if(slider.value <= 0)
        {
            slider.value = 0;
            text.text = "0/" + character.MaxHealth;
            isDead = true;
            TxtCountDown.enabled = true;
            TxtCountDown.gameObject.SetActive(true);
            countDown();
        }
    }
    
    public void countDown()
    {
        StartCoroutine(CCountDown());
    }
    
    private IEnumerator CCountDown()
    {
        yield return new WaitForSeconds(1);
        int count = int.Parse(TxtCountDown.text);
        count--;
        TxtCountDown.text = count.ToString();
        if (count >= 0 && isDead)
        {
            StartCoroutine(CCountDown());
        }

        if (count == 0)
        {
            TxtCountDown.text = "20";
            TxtCountDown.gameObject.SetActive(false);
            character.GetComponent<PlayerMove>().ReSpawn();
            isDead = false;
        }
    }
}
