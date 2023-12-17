using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] GameObject egg;
    [SerializeField] Transform deathSprite;
    [SerializeField] private int maxHealth;
    private int currentHealth;
    private int stepHealt;
    private int currentStep = 0;

    private const int STEPS = 6;

    public SpriteRenderer headSpriteR;
    public SpriteRenderer bodySpriteR;
    [SerializeField] List<Sprite> crackHeadSprites = new List<Sprite>();
    [SerializeField] List<Sprite> crackBodySprites = new List<Sprite>();

    [SerializeField] GameObject Chicken;
    [SerializeField] Sprite scaredChicken;

    [SerializeField] private TimeController timeController;

    Animator animator;

    //public event Action OutTime; 
    void Start()
    {
        currentHealth = maxHealth;
        stepHealt = maxHealth / STEPS;
        animator = deathSprite.GetComponent<Animator>();
    }

    //just for debug
    private void Update()
    {
      //if( Input.GetMouseButtonDown(0)) { takeDamage(); }
    }

     public void takeDamage() { 
        currentHealth -= 1;

        if (currentHealth <= 0) {
            death();
            return;
        } 

        // change dmg level
        if (currentHealth% stepHealt == 0 && currentStep < STEPS-1)
        {
            // animation
            animator.SetTrigger("Damage");
            headSpriteR.sprite = crackHeadSprites[currentStep];
            bodySpriteR.sprite = crackBodySprites[currentStep];

            ++currentStep;
        } 
    }

    void death()
    {

        // death animation
        timeController.StopTimer();
        deathSprite.GetComponent<SpriteRenderer>().sortingOrder = 5;
        Chicken.GetComponent<SpriteRenderer>().sprite = scaredChicken;
        animator.SetTrigger("Death");
        egg.SetActive(false);
        print("death");
      // end screen
    }

}
