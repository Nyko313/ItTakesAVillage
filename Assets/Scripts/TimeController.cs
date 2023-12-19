using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField] private Health health;

    float remainingTime;
    [SerializeField] float NormalMaxTime = 10f;
    [SerializeField] float handoverMaxTime = 10f;
    private float maxTime;
    public Image timerImage;
    float speed;

    //------ MERGING ---
    [SerializeField] private GameHandler gameHandler;


    bool IsHand = false;
    bool stop = false;

    // RICORDARE SE AZIONE ESEGUITE BENE PORTARE TIME A 0
    void Start()
    {
        maxTime = NormalMaxTime;
        remainingTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop) timer();

        
    }

    private void FixedUpdate()
    {
        speed += 0.001f;
    }


    private void timer() 
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime * speed;
            timerImage.fillAmount = remainingTime / maxTime;
            //print(remainingTime / maxTime);
        }
        else
        {
            if (!IsHand) health.takeDamage();
            switchPhase();
            //skip to the next phase
        }
    }

    public void switchPhase()
    {
        if(!IsHand) // go to handover
        {
            gameHandler.TimeFinished();
            maxTime = handoverMaxTime;
            remainingTime = maxTime;
            timerImage.GetComponent<Image>().color = new Color32(255, 92, 12, 255);
            timerImage.fillAmount = 100;
        }
        else // return to normal game
        {
            gameHandler.FinishRound();
            maxTime = NormalMaxTime;
            remainingTime = maxTime;
            timerImage.GetComponent<Image>().color = new Color32(90, 230, 104, 255);
            timerImage.fillAmount = 100;
            gameHandler.StartRound(); // questo va cambiato, deve lanciare il prossimo step del routine handler
        }
        IsHand = !IsHand;
    }

    public void StopTimer() { stop = true; }
}
