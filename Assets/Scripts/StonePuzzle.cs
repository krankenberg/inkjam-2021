using System;
using Camera;
using InkFiles;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class StonePuzzle : MonoBehaviour
{
    public float RotationStep = 45F;
    public GameObject[] Stones;
    public bool Active;
    public InkStory InkStory;

    public Vector3 CameraPosition;
    public Vector3 CameraRotationEuler;

    private float[] _initialRotations;
    private bool[] _stoneCorrect;

    private bool _initialized;
    
    private Transform _cameraTransform;
    private FollowObject _followObject;

    private void Start()
    {
        var cameraGO = GameObject.FindWithTag("MainCamera");
        _cameraTransform = cameraGO.transform;
        _followObject = cameraGO.GetComponent<FollowObject>();
    }

    private void Update()
    {
        if (!_initialized)
        {
            _initialRotations = new float[Stones.Length];
            _stoneCorrect = new bool[Stones.Length];
            _initialized = true;
            for (var i = 0; i < Stones.Length; i++)
            {
                var stoneTransform = Stones[i].transform;
                var stoneEulerAngles = stoneTransform.rotation.eulerAngles;
                _initialRotations[i] = stoneEulerAngles.y;

                var newRotationY = _initialRotations[i] + Random.Range(1, 8) * RotationStep;
                stoneTransform.rotation = Quaternion.Euler(stoneEulerAngles.x, newRotationY, stoneEulerAngles.z);
            }
        }
    }

    public void StartPuzzle()
    {
        _followObject.enabled = false;
        _cameraTransform.position = CameraPosition;
        _cameraTransform.rotation = Quaternion.Euler(CameraRotationEuler);
        GetComponent<Collider>().enabled = false;
        Active = true;
    }

    public void StopPuzzle()
    {
        _followObject.enabled = true;
        Active = false;
        GetComponent<Collider>().enabled = true;
    }

    public void ClickOn(Collider clickedCollider, bool backward)
    {
        Debug.Log("turn " + clickedCollider.name);
        for (var i = 0; i < Stones.Length; i++)
        {
            var stone = Stones[i];
            if (stone.name == clickedCollider.name)
            {
                var rotationEulerAngles = stone.transform.rotation.eulerAngles;
                var newYRotation = rotationEulerAngles.y + (backward ? -RotationStep : RotationStep);
                stone.transform.rotation = Quaternion.Euler(rotationEulerAngles.x, newYRotation, rotationEulerAngles.z);

                var stoneCorrect = Math.Abs(((newYRotation + 360) % 360) - ((_initialRotations[i] + 360) % 360)) < 5;
                if (stoneCorrect)
                {
                    Debug.Log(stone.name + " is correct!");
                }
                _stoneCorrect[i] = stoneCorrect;
            }
        }

        var allCorrect = true;
        foreach (var stoneCorrect in _stoneCorrect)
        {
            if (!stoneCorrect)
            {
                allCorrect = false;
            }
        }

        if (allCorrect)
        {
            Debug.Log("all correct");
            StopPuzzle();
            InkStory.StartStitch("entrance_opened");
        }
    }
    
}
