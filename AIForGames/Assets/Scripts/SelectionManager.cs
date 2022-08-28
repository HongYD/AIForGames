using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    #region variables

    public static SelectionManager instance;
    //float
    public float boxWidth;
    public float boxHeight;
    public float boxTop;
    public float boxLeft;
    //vector2
    public Vector2 boxStart;
    public Vector2 boxFinish;
    public Vector2 mouseDragStartPosition;
    //vector3
    public Vector3 currentMousePoint;
    public Vector3 mouseDownPoint;
    //gui
    public GUIStyle mouseDragSkin;
    //list and arrays
    public List<GameObject> currentlySelectedPlayers = new List<GameObject>();
    //bool
    public bool mouseDragging;
    //gameobjects
    public GameObject selectedPlayer;
    public List<GameObject> players = new List<GameObject>();
    //FSM 
    public enum SelectFSM
    {
        clickOrDrag,
        clickSelect,
        clickDeselect
    }
    public SelectFSM selectFSM;

    #endregion
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for(int i=0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
        {
            players.Add(GameObject.FindGameObjectsWithTag("Player")[i]);
        }
    }

    private void Update()
    {
        SelectUnitsFSM();
    }

    private void OnGUI()
    {
        if (mouseDragging)
            GUI.Box(new Rect(boxLeft, boxTop, boxWidth, boxHeight), "", mouseDragSkin);
    }

    private void SelectUnitsFSM()
    {
        switch (selectFSM)
        {
            case SelectFSM.clickOrDrag:
                ClickOrDrag();
                break;
            case SelectFSM.clickSelect:
                SelectSingleUnit();
                break;
            case SelectFSM.clickDeselect:
                DeselectAll();
                break;
        }
    }

    #region helper functions

    private void ClickOrDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)/* && !EventSystem.current.IsPointerOverGameObject()*/)
        {
            currentMousePoint = hit.point;
            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                mouseDownPoint = hit.point;

                mouseDragStartPosition = Input.mousePosition;

                //click to select unit, or click the ground to deselect all
                if (hit.collider.gameObject.tag == "Player")
                {
                    selectedPlayer = hit.collider.gameObject;
                    selectFSM = SelectFSM.clickSelect;
                }
                else if (hit.collider.gameObject.tag == "Ground")
                    selectFSM = SelectFSM.clickDeselect;
            }
            else if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
            {
                //holding shift, click to select units or click selected units to deselect
                if (hit.collider.gameObject.tag == "Player" && !currentlySelectedPlayers.Contains(hit.collider.gameObject))
                    AddToCurrentlySelectedUnits(hit.collider.gameObject);
                else if (hit.collider.gameObject.tag == "Player" && currentlySelectedPlayers.Contains(hit.collider.gameObject))
                    RemoveFromCurrentlySelectedUnits(hit.collider.gameObject);
            }
            else if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                if (UserDraggingByPosition(mouseDragStartPosition, Input.mousePosition))
                {
                    mouseDragging = true;
                    DrawDragBox();
                    SelectUnitsInDrag();
                }
            }
            else if (Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                mouseDragging = false;
            }
        }
    }

    private void SelectSingleUnit()
    {
        if (selectedPlayer != null)
        {
            if (currentlySelectedPlayers.Count > 0)
            {
                for (int i = 0; i < currentlySelectedPlayers.Count; i++)
                {
                    currentlySelectedPlayers[i].transform.Find("Selection").gameObject.SetActive(false);
                    currentlySelectedPlayers.Remove(currentlySelectedPlayers[i]);
                }
            }
            else if (currentlySelectedPlayers.Count == 0)
            {
                AddToCurrentlySelectedUnits(selectedPlayer);
                selectFSM = SelectFSM.clickOrDrag;
            }
        }
        else
        {
            Debug.Log("Error!");
        }
    }

    private void DrawDragBox()
    {
        boxWidth = Camera.main.WorldToScreenPoint(mouseDownPoint).x - Camera.main.WorldToScreenPoint(currentMousePoint).x;
        boxHeight = Camera.main.WorldToScreenPoint(mouseDownPoint).y - Camera.main.WorldToScreenPoint(currentMousePoint).y;
        boxLeft = Input.mousePosition.x;
        boxTop = (Screen.height - Input.mousePosition.y) - boxHeight; //need to invert y as GUI space has 0,0 at top left, but Screen space has 0,0 at bottom left. x is the same. 

        if (boxWidth > 0 && boxHeight < 0f)
            boxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        else if (boxWidth > 0 && boxHeight > 0f)
            boxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y + boxHeight);
        else if (boxWidth < 0 && boxHeight < 0f)
            boxStart = new Vector2(Input.mousePosition.x + boxWidth, Input.mousePosition.y);
        else if (boxWidth < 0 && boxHeight > 0f)
            boxStart = new Vector2(Input.mousePosition.x + boxWidth, Input.mousePosition.y + boxHeight);

        boxFinish = new Vector2(boxStart.x + Mathf.Abs(boxWidth), boxStart.y - Mathf.Abs(boxHeight));
    }

    private bool UserDraggingByPosition(Vector2 dragStartPoint, Vector2 newPoint)
    {
        if ((newPoint.x > dragStartPoint.x || newPoint.x < dragStartPoint.x) || (newPoint.y > dragStartPoint.y || newPoint.y < dragStartPoint.y))
            return true;
        else
            return false;
    }

    private void SelectUnitsInDrag()
    {
        for (int i = 0; i < players.Count; i++)
        {
            Vector2 unitScreenPosition = Camera.main.WorldToScreenPoint(players[i].transform.position);

            if (unitScreenPosition.x < boxFinish.x && unitScreenPosition.y > boxFinish.y && unitScreenPosition.x > boxStart.x && unitScreenPosition.y < boxStart.y)
                AddToCurrentlySelectedUnits(players[i]);
            else
            {
                RemoveFromCurrentlySelectedUnits(players[i]);
            }
        }
    }

    private void AddToCurrentlySelectedUnits(GameObject unitToAdd)
    {
        if (!currentlySelectedPlayers.Contains(unitToAdd))
        {
            currentlySelectedPlayers.Add(unitToAdd);
            unitToAdd.transform.Find("Selection").gameObject.SetActive(true);
        }
    }

    private void RemoveFromCurrentlySelectedUnits(GameObject unitToRemove)
    {
        if (currentlySelectedPlayers.Count > 0)
        {
            unitToRemove.transform.Find("Selection").gameObject.SetActive(false);
            currentlySelectedPlayers.Remove(unitToRemove);
        }
    }

    private void DeselectAll()
    {
        if (currentlySelectedPlayers.Count > 0)
        {
            for (int i = 0; i < currentlySelectedPlayers.Count; i++)
            {
                currentlySelectedPlayers[i].transform.Find("Selection").gameObject.SetActive(false);
                currentlySelectedPlayers.Remove(currentlySelectedPlayers[i]);
            }
        }
        else if (currentlySelectedPlayers.Count == 0)
        {
            selectFSM = SelectFSM.clickOrDrag;
        }
    }

    #endregion

}
