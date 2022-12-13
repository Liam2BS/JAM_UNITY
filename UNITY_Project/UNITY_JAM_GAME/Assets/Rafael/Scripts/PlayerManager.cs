using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float health = 10;
    public float healthLost = 1;
    public int healthPerChicken = 10;
    public int healthPerGoldenChicken = 50; 

    public int tier = 0;
    public int chicken;

    // Update is called once per frame
    void Update()
    {
        health -= healthLost * Time.deltaTime;
        if(health <= 0) { health = 0; } //GAME OVER
    }

    public void UpgradeTier()
    {
        Debug.Log(health / (tier + 1));
        if(health / (tier+1) >= 100 && tier <= 5)
        {
            tier++;
            transform.parent.localScale = new Vector3(tier+1, tier+1, tier+1);
        }
    }

    public void EatChicken(int moreHealth)
    {
        chicken++;
        health += moreHealth;
        UpgradeTier();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched Smothing");
        if (other.gameObject.tag == "Poulet")
        {
            EatChicken(healthPerChicken);
            transform.parent.GetComponent<PlayerMovement>().anim.SetTrigger("Pick_Up");
            other.GetComponent<Poulet>().Blood();
            other.GetComponent<Poulet>().Respawn();
            //GameObject.Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "GoldenPoulet")
        {
            EatChicken(healthPerGoldenChicken);
            //GameObject.Destroy(other.gameObject);
            transform.parent.GetComponent<PlayerMovement>().anim.SetTrigger("Pick_Up");
            other.GetComponent<Poulet>().Blood();
            other.GetComponent<Poulet>().Respawn();

        }
    }
}
