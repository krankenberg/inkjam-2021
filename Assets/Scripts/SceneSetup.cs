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
    public Animator EntranceAnimator;
    private static readonly int Opened = Animator.StringToHash("opened");

    public GameObject[] BurialSceneObjects;
    public Transform BurialScene_DocMarker;
    public Transform BurialScene_StanleyMarker;

    public GameObject[] Room1_SceneObjects;
    public Transform Room1__DocMarker;
    public Transform Room1__StanleyMarker;

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
        ActivateSceneObjects(Room1_SceneObjects, false);

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

        yield return new WaitForSeconds(0.5F);

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
            return 1.5F;
        }

        if (sceneName == "BRIEFING")
        {
            BlackScreen.SetActive(false);
            DocLookLeft();
            StanleyLookAtDoc();
        }

        if (sceneName == "FRONT_OF_GRAVE")
        {
            ActivateScene(Scene2Objects);
            DirectionalLight.SetActive(true);

            DocLookRight();
            Warp(Scene2_DocMarker, Scene2_StanleyMarker);
            StanleyLookAtDoc();
        }

        if (sceneName == "ROOM_1")
        {
            ActivateScene(Room1_SceneObjects);
            DirectionalLight.SetActive(false);

            Warp(Room1__DocMarker, Room1__StanleyMarker);
            DocLookRight();
            StanleyLookAtDoc();
        }

        if (sceneName == "BURIAL_CHAMBER")
        {
            ActivateScene(BurialSceneObjects);
            DirectionalLight.SetActive(false);

            Warp(BurialScene_DocMarker, BurialScene_StanleyMarker);
            DocLookLeft();
            StanleyLookAtDoc();
        }

        if (sceneName == "OUTSIDE_GRAVE")
        {
            ActivateScene(OutsideSceneObjects);
            EntranceAnimator.SetBool(Opened, true);
            DirectionalLight.SetActive(true);
            var directionalLight = DirectionalLight.GetComponent<Light>();
            directionalLight.intensity = 0.08F;
            var lightRotation = DirectionalLight.transform.rotation.eulerAngles;
            DirectionalLight.transform.rotation = Quaternion.Euler(lightRotation.x, -lightRotation.y, lightRotation.z);

            Warp(OutsideScene_DocMarker, OutsideScene_StanleyMarker);
            DocLookLeft();
            StanleyLookAtDoc();
        }

        if (sceneName == "THE_END")
        {  
            BlackScreen.SetActive(true);
            BlackScreenTitle.SetActive(false);
        }

        return 0;
    }

    private void Warp(Transform docPosition, Transform stanleyPosition)
    {
        Doc.GetComponent<Transform>().position = docPosition.position;
        Doc.GetComponent<StartStitchOnClick>().InteractionPoints = docPosition.GetComponent<Marker>().InteractionPoints;
        Stanley.GetComponent<NavMeshAgent>().Warp(stanleyPosition.position);
    }

    private void StanleyLookAtDoc()
    {
        Stanley.GetComponent<Move>().LookAt = Doc.transform;
    }

    private void DocLookLeft()
    {
        Doc.GetComponentInChildren<SpriteRenderer>().flipX = true;
    }

    private void DocLookRight()
    {
        Doc.GetComponentInChildren<SpriteRenderer>().flipX = false;
    }
}
