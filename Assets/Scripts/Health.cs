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
      if( Input.GetMouseButtonDown(0)) { takeDamage(); }
    }

    void takeDamage() { 
        currentHealth -= 1;

        if (currentHealth <= 0) {

            death();
            return;
        } 

        // change dmg level
        if (currentHealth% stepHealt == 0 && currentStep < STEPS-1)
        {
            // animation
            headSpriteR.sprite = crackHeadSprites[currentStep];
            bodySpriteR.sprite = crackBodySprites[currentStep];

            ++currentStep;
        } 
    }

    void death()
    {
        animator.SetTrigger("Death");
        egg.SetActive(false);
      
        //SetTargetInvisible(egg);
        print("death");
      // death animation
      // end screen
    }


    void SetTargetInvisible(Transform theTransform)
    {
        foreach (Transform aaa in theTransform)
        {
            aaa.GetComponent<Renderer>().enabled = false;
            SetTargetInvisible(aaa);
        }
    }

}
