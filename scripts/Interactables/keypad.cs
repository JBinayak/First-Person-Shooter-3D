using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keypad : interactable
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;
    private GameObject player;
    public bool damageTrigger;
    public bool doorLever;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        if (doorLever)
        {
            doorOpen = !doorOpen;
            door.GetComponent<Animator>().SetBool("isOpen", doorOpen);
        }
        if (damageTrigger)
        {
            player.GetComponent<PlayerHealth>().takeDamage(20);
        }
    }
}
