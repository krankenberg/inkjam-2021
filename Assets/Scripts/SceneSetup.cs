using System;
using System.Collections;
using InkFiles;
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
    
    public GameObject[] BurialSceneObjects;
    public Transform BurialScene_DocMarker;
    public Transform BurialScene_StanleyMarker;
    
    public GameObject[] OutsideSceneObjects;
    public Transform OutsideScene_DocMarker;
    public Transform OutsideScene_StanleyMarker;

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
        ActivateSceneObjects(BurialSceneObjects, false);
        ActivateSceneObjects(OutsideSceneObjects, false);
        
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
            Doc.GetComponent<Transform>().position = Scene2_DocMarker.position;
            Doc.GetComponent<StartStitchOnClick>().InteractionPoints = Scene2_DocMarker.GetComponent<Marker>().InteractionPoints;
            Stanley.GetComponent<NavMeshAgent>().Warp(Scene2_StanleyMarker.position);
        }
        if (sceneName == "BURIAL_CHAMBER")
        {
            ActivateScene(BurialSceneObjects);
            DirectionalLight.SetActive(false);
            Doc.GetComponent<Transform>().position = BurialScene_DocMarker.position;
            Doc.GetComponent<StartStitchOnClick>().InteractionPoints = BurialScene_DocMarker.GetComponent<Marker>().InteractionPoints;
            Stanley.GetComponent<NavMeshAgent>().Warp(BurialScene_StanleyMarker.position);
        }
        if (sceneName == "OUTSIDE_GRAVE")
        {
            ActivateScene(OutsideSceneObjects);
            DirectionalLight.SetActive(true);
            var light = DirectionalLight.GetComponent<Light>();
            light.intensity = 0.08F;
            var lightRotation = DirectionalLight.transform.rotation.eulerAngles;
            DirectionalLight.transform.rotation = Quaternion.Euler(lightRotation.x, -lightRotation.y, lightRotation.z);
            Doc.GetComponent<Transform>().position = OutsideScene_DocMarker.position;
            Doc.GetComponent<StartStitchOnClick>().InteractionPoints = OutsideScene_DocMarker.GetComponent<Marker>().InteractionPoints;
            Stanley.GetComponent<NavMeshAgent>().Warp(OutsideScene_StanleyMarker.position);
        }

        return 0;
    }
}
