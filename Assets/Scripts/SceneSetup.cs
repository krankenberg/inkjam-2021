using System.Collections;
using UI;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public Transitioner Transitioner;
    
    public void Setup(string sceneName)
    {
        Debug.Log("setup " + sceneName);
        GlobalGameState.HideUi = true;
        
        StartCoroutine(DoIt(sceneName));
    }
    
    private IEnumerator DoIt(string sceneName)
    {
        Transitioner.FadeIn = true;

        while (Transitioner.FadeIn)
        {
            yield return null;
        }

        RealSetup(sceneName);
        
        Transitioner.FadeOut = true;

        while (Transitioner.FadeOut)
        {
            yield return null;
        }
        
        GlobalGameState.HideUi = false;
    }

    private void RealSetup(string sceneName)
    {
        Debug.Log("real setup " + sceneName);
    }
}
