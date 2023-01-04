using System;
using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MainLight : MonoBehaviour
{
    [Range(1, 8)]  public int NumOfLightis;
    public GameObject LightPrefab;
    public List<GameObject> Lights = new List<GameObject>();

    public GameObject BlackHole;
    public Vector3 BreakPoint;
    public BlackHoleManger _bhm;
    
    // Start is called before the first frame update
    void Start()
    {
         _bhm = BlackHole.GetComponent<BlackHoleManger>();
         
    }
    void OnGUI()
    {
        NumOfLightis =  Mathf.RoundToInt(GUILayout.HorizontalSlider(NumOfLightis, 1, 8));
        GUILayout.Label("   Number of Rays   ");
    }   
    // Update is called once per frame
    void Update()
    {
        //Respond again if the count is changed.  
        if (Lights.Count != NumOfLightis)
        {
            foreach (var l in Lights)
            {
                Destroy(l);
            }
            Lights.Clear();
            SetUpLight();
        }
    }
   //Set up the light Positioning and passing important data to the script.  
    private void SetUpLight()
    {
        for (int i = 0; i < NumOfLightis; i++)
        {
            GameObject l = Instantiate(LightPrefab);
            l.transform.SetParent(this.transform);
            l.GetComponent<RayLight>().RayEnd.position = GenRay(i);
            l.GetComponent<RayLight>().RayNum = i;
            l.GetComponent<RayLight>().enabled = true;
            l.GetComponent<RayLight>()._mainLight = this.GetComponent<MainLight>();
            
            l.GetComponent<BGCurve>().Points[0].PositionLocal = this.transform.position;
            l.GetComponent<BGCurve>().Points[0].PointTransform = this.transform;
            
            l.transform.position = this.transform.position;
            
            Lights.Add(l);
        }
    }
    private Vector3 GenRay(int num)
    {
        Vector3 pos = new Vector3(75, 0, num * (125/NumOfLightis)-50);
        return pos;
    }
    
    //mouse Drag Code 
    #region MouseDrag

    private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }
 
    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
 
        Vector3 curPosition = new Vector3(Camera.main.ScreenToWorldPoint(curScreenPoint).x, 0
            ,Camera.main.ScreenToWorldPoint(curScreenPoint).z)+ offset;
        transform.position = curPosition;
        
    }
    #endregion    
}
