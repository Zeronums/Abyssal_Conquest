using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenScript : MonoBehaviour
{


    void Start()
    {
        StartCoroutine(LoadMainScene());
    }

    IEnumerator LoadMainScene(){
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);


    }    
        //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("CavernMain");
        /*while (!asyncLoad.isDone)
        {
            yield return null;
        }*/
    
}
