using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class KillCounter : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    private int kills;

    public void AddKill()
    {
        kills++;
    }

    private void Update()
    {
        ShowKills();
    }

    private void ShowKills()
    {
        counterText.text = kills.ToString();
    }
}
