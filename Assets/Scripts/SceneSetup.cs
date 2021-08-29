using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.AI;

public class SceneSetup : MonoBehaviour
{
    public Transitioner Transitioner;
    public GameObject Doc;
    public GameObject Stanley;
    public GameObject DirectionalLight;
    public GameObject BlackScreen;
    public GameObject BlackScreenTitle;
    
    public GameObject[] Scene1Objects;

    public GameObject[] Scene2Objects;
    public Transform Scene2_DocMarker;
    public Transform Scene2_StanleyMarker;

    private void Start()
    {
        BlackScreen.SetActive(true);
        DirectionalLight.SetActive(false);
        ActivateScene(Scene1Objects);
    }

    private void ActivateScene(GameObject[] sceneObjects)
    {
        ActivateSceneObjects(Scene1Objects, false);
        ActivateSceneObjects(Scene2Objects, false);
        
        ActivateSceneObjects(sceneObjects, true);
    }

    private void ActivateSceneObjects(GameObject[] sceneObjects, bool active)
    {
        foreach (var sceneObject in sceneObjects)
        {
            sceneObject.SetActive(active);
        }
    }

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

        float waitTime = RealSetup(sceneName);
        
        Transitioner.FadeOut = true;

        while (Transitioner.FadeOut)
        {
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);
        
        GlobalGameState.HideUi = false;
    }

    private float RealSetup(string sceneName)
    {
        if (sceneName == "START")
        {
            BlackScreenTitle.SetActive(true);
            return 2.5F;
        }
        if (sceneName == "BRIEFING")
        {
            BlackScreen.SetActive(false);
            Stanley.GetComponent<Move>().LookAt = Doc.transform;
        }
        if (sceneName == "FRONT_OF_GRAVE")
        {
            ActivateScene(Scene2Objects);
            DirectionalLight.SetActive(true);
            Doc.GetComponent<Rigidbody>().position = Scene2_DocMarker.position;
            Stanley.GetComponent<NavMeshAgent>().Warp(Scene2_StanleyMarker.position);
        }

        return 0;
    }
}
