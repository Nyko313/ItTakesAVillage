using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Instruction
{
    public Sprite sprite;
    public String text;
}
public class Instructions : MonoBehaviour
{
    public Image imageContainer;

    public TextMeshProUGUI textField;

    public Instruction[] instructions;

    public int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        ShowCurrent();
    }

    void ShowCurrent()
    {
        if (instructions.Length > 0)
        {
            this.imageContainer.sprite = instructions[currentIndex].sprite;
            this.textField.text = instructions[currentIndex].text;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShowCurrent();
    }

    public void Forward()
    {
        currentIndex += 1;
        currentIndex = currentIndex % instructions.Length;
    }
    
    public void Backward()
    {
        currentIndex -= 1;
        if (currentIndex < 0)
        {
            currentIndex = Math.Max(0, instructions.Length - 1);
        }
        
    }
}
