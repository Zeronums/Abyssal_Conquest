using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStuff : MonoBehaviour
{
   public GameObject gameOver;
   public void EnableGameOver(){
        gameOver.SetActive(true);
   }

   private void OnEnable(){
        PlayerHealth.OnPlayerDeath += EnableGameOver;
   }

   private void OnDisable(){
        PlayerHealth.OnPlayerDeath -= EnableGameOver;
   }

   public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }

   public void Quit(){
        Application.Quit();
        Debug.Log("Quit the game.");
    }
}
