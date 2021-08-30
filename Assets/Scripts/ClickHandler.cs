using InkFiles;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour
{
    public ToolTip ToolTip;
    public StonePuzzle StonePuzzle;

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
                var startStitchOnClick = raycastHit.collider == null ? null : raycastHit.collider.GetComponent<StartStitchOnClick>();
                var stonePuzzleStone = raycastHit.collider != null && raycastHit.collider.CompareTag("StonePuzzleStone") ? raycastHit.collider : null;

                if (StonePuzzle.Active)
                {
                    var leftMouseButton = Input.GetButtonUp("Fire1");
                    var rightMouseButton = Input.GetButtonUp("Fire2");
                    ToolTip.Text = stonePuzzleStone != null ? "Stone" : "Go back";
                    if (stonePuzzleStone != null)
                    {
                        if (leftMouseButton || rightMouseButton)
                        {
                            StonePuzzle.ClickOn(stonePuzzleStone, rightMouseButton);
                        }
                    }
                    else if (leftMouseButton || rightMouseButton)
                    {
                        StonePuzzle.StopPuzzle();
                    }

                    return;
                }

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
