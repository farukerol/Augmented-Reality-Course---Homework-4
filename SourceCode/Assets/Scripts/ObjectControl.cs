using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    // Start is called before the first frame update
    public bool  hit = false;
    
    public Material ON;
    public Material OFF;
    private void OnTriggerEnter(Collider other)
    {
        gameObject.GetComponent<Renderer>().material = ON;
    }
    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<Renderer>().material = OFF;
    }
    private void FixedUpdate()
    {
        if (hit)
        {
            gameObject.GetComponent<Renderer>().material = ON;
            hit = false;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = OFF;

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
