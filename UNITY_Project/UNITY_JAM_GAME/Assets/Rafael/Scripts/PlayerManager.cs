using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public float health = 10;
    public float highestHealth = 0;
    public float healthLost = 1;
    public int healthPerChicken = 10;
    public int healthPerGoldenChicken = 50; 

    public int tier = 0;
    public int chicken;

    private bool isGameOver = false;

    //UI
    public Sprite[] rankSprite = new Sprite[7];
    public Image rankImg;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI healthText;
    public GameObject pausePanel;

    public GameObject GOPanel;
    public Image GORankImg;
    public TextMeshProUGUI GOHealthText;
    public TextMeshProUGUI GOChickenText;
    public TextMeshProUGUI GOTierText;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        health -= healthLost * (Mathf.Pow((tier + 1), 1.19f)) * Time.deltaTime;
        if(health <= 0) { health = 0; GameOver(); } //GAME OVER
        healthText.text = ((int)health).ToString();
        staminaText.text = ((int)transform.parent.GetComponent<PlayerMovement>().stamina).ToString();

        if(Input.GetKey(KeyCode.Escape) && !isGameOver)
        {
            PauseGame();
        }

        if( health >= highestHealth) { highestHealth = health; }
    }

    public void UpgradeTier()
    {
        Debug.Log(health / (tier + 1));
        if(health / (tier+1) >= 100 && tier <= 5)
        {
            tier++;
            transform.parent.localScale = new Vector3(tier+1, tier+1, tier+1);
            rankImg.sprite = rankSprite[tier];
            rankText.text = tier.ToString();
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

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        GOPanel.SetActive(false);
    }

    public void RetryGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        isGameOver = true;
        GOPanel.SetActive(true);
        GORankImg.sprite = rankSprite[tier];
        GOHealthText.text = ((int)highestHealth).ToString();
        GOChickenText.text = chicken.ToString();
        GOTierText.text = tier.ToString();
        Time.timeScale = 0;
        //game over
    }
}
