using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;

    private int sceneToLoad;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //General Scene-changer
        if (Input.GetKeyDown(KeyCode.T))
        {
            FadeToNextScene();
        }

        if (GoalBox.finished == true)
            FadeToNextScene();
    }

    public void FadeToNextScene()
    {
        FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FadeToPreviousScene()
    {
        FadeToScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void FadeToScene(int sceneIndex)
    {
        sceneToLoad = sceneIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
