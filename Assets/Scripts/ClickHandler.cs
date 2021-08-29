using System;
using System.Collections;
using System.Collections.Generic;
using InkFiles;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Camera = UnityEngine.Camera;

public class ClickHandler : MonoBehaviour
{
    public ToolTip ToolTip;
    
    private UnityEngine.Camera _camera;
    private Move _playerMove;

    private void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera").GetComponent<UnityEngine.Camera>();
        _playerMove = GameObject.FindWithTag("Player").GetComponent<Move>();
    }

    private void Update()
    {
        ToolTip.Text = "";
        if (GlobalGameState.FreePlay && !EventSystem.current.IsPointerOverGameObject())
        {
            var screenPosition = Input.mousePosition;

            var ray = _camera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100))
            {
                var startStitchOnClick = raycastHit.rigidbody == null ? null : raycastHit.rigidbody.GetComponent<StartStitchOnClick>();

                ToolTip.Text = startStitchOnClick != null ? startStitchOnClick.ToolTip : "";

                if (Input.GetButtonUp("Fire1"))
                {
                    if (startStitchOnClick != null)
                    {
                        startStitchOnClick.Click();
                    }
                    else
                    {
                        _playerMove.Click(raycastHit.point);
                    }
                }
            }
        }
    }
}
