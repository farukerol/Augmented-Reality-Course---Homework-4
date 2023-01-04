using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleManger : MonoBehaviour
{
    #region GUI
    private void OnGUI()
    {
        if (GUI.Button(new Rect(7, 35, 200, 30), "Active Black Hole Or Press [B]"))
        {
            OnBlackHole?.Invoke(this,EventArgs.Empty);
        }
        /*if (GUI.Button(new Rect(10, 60, 300, 40), "Generate Collider"))
        {
            GenerateMeshCollider();
        }
        if (GUI.Button(new Rect(10, 110, 300, 40), "Simplify Mesh"))
        {
            Line.Simplify(0.1f);
        }*/
    }
    

  #endregion
    public event EventHandler OnBlackHole;
    
    private bool active = false;

    public bool Active
    {
        get
        {
          return active;
        }
    }

    public void HitActive(object sender,EventArgs e)
    {
        if (active)
        {
            active = false;
        }
        else
        {
            active = true;
        }
        
    }

    void Start()
    {
        OnBlackHole += HitActive;
    }

    void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            OnBlackHole?.Invoke(this,EventArgs.Empty);
        }
    }
    
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
