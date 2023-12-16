using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{

    float remainingTime;
    [SerializeField] float NormalMaxTime = 10f;
    [SerializeField] float handoverMaxTime = 10f;
    private float maxTime;
    public Image timerImage;

    bool IsHand = false;

    // Start is called before the first frame update
    void Start()
    {
        maxTime = NormalMaxTime;
        remainingTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(remainingTime > 0)
        { 
            remainingTime -= Time.deltaTime;
            timerImage.fillAmount = remainingTime / maxTime;
            //print(remainingTime / maxTime);
        }
        else
        {
            handover();
            //skip to the next phase
        }

    }

    void handover()
    {
        if(!IsHand)
        {
            maxTime = handoverMaxTime;
            remainingTime = maxTime;
            timerImage.GetComponent<Image>().color = new Color32(255, 92, 12, 255);
            timerImage.fillAmount = 100;
        }
        else
        {
            maxTime = NormalMaxTime;
            remainingTime = maxTime;
            timerImage.GetComponent<Image>().color = new Color32(90, 230, 104, 255);
            timerImage.fillAmount = 100;
        }
        IsHand = !IsHand;
    }
}
