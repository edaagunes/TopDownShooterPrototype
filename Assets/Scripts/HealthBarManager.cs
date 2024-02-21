using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private Slider enemySlider;
    
    public void UpdateEnemyHealthBar(float currentValue, float maxValue)
    {
        enemySlider.value = currentValue / maxValue;
    }
  
    
}