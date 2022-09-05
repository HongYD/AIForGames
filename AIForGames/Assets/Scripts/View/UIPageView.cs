using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPageView : MonoBehaviour, ISubject
{
    private List<GameObject> _observers = new List<GameObject>();
    private GameObject formationManager;
    FormationPatternEnum patternEnum;
    public Button circleButton;
    public Button squareButton;
    public Button triangleButton;

    void Start()
    {
        formationManager = GameObject.Find("FormationManager");
        if (formationManager != null)
        {
            Attach(formationManager);
        }
        circleButton.onClick.AddListener(OnClickCircleButton);
        squareButton.onClick.AddListener(OnClickSquareButton);
        triangleButton.onClick.AddListener(OnClickTriangleButton);
    }

    void Update()
    {
        
    }

    public void Attach(GameObject observer) 
    {
        _observers.Add(observer);
    }

    public void Detach(GameObject observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var o in _observers)
        {
            o.GetComponent<FormationManager>().ChangeFormation(patternEnum);
        }
    }

    private void OnClickCircleButton()
    {
        patternEnum = FormationPatternEnum.DefensiveCircle;
        Notify();
    }

    private void OnClickSquareButton()
    {
        patternEnum = FormationPatternEnum.Square;
        Notify();
    }

    private void OnClickTriangleButton()
    {
        patternEnum = FormationPatternEnum.Triangle;
        Notify();
    }
}
