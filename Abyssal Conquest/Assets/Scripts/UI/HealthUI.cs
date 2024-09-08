using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider hpSlider;
   
    
    public void SetHP(float amount){
        hpSlider.value = amount;

    }

    public void SetHPMax(float amount){
        hpSlider.maxValue = amount;
        SetHP(amount);
    }
}
