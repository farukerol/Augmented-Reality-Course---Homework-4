using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;

public class RayLight : MonoBehaviour
{

    LineRenderer Lr;
    public Transform RayEnd;
    private Vector3 PrevPos;
    Transform Hole;
    public float CurveAngle;
    public float CurveWidth;
    public MainLight _mainLight;
    private BGCurve _curve;
    public int RayNum;
    private void Start()
    {
        Lr = GetComponent<LineRenderer>();
        _curve = GetComponent<BGCurve>();
        _mainLight = GetComponentInParent<MainLight>();
        Hole = _mainLight.BlackHole.transform;
        //GenerateMeshCollider();

    }

    public void GenerateMeshCollider()
    {
        MeshCollider collider = GetComponent<MeshCollider>();

        if (collider == null)
        {
            collider = gameObject.AddComponent<MeshCollider>();
        }


        //draw mesh on the Ray
        Mesh mesh = new Mesh();
        Lr.BakeMesh(mesh, true);

        // if you need collisions on both sides of the line, simply duplicate & flip facing the other direction!
        
        int[] meshIndices = mesh.GetIndices(0);
        int[] newIndices = new int[meshIndices.Length * 2];

        int j = meshIndices.Length - 1;
        for (int i = 0; i < meshIndices.Length; i++)
        {
            newIndices[i] = meshIndices[i];
            newIndices[meshIndices.Length + i] = meshIndices[j];
        }
        mesh.SetIndices(newIndices, MeshTopology.Triangles, 0);

        collider.sharedMesh = mesh;

    }
    // This activate when the black hole is disable. 
    void StraightForward()
    {

        PrevPos = RayEnd.position;


        Vector3 midpiont = (this.transform.position + RayEnd.position) * 0.5f;
        _curve.Points[1].PositionWorld = midpiont;
        _curve.Points[1].ControlFirstWorld = midpiont;


        _curve.Points[2].PointTransform = RayEnd;
        _curve.Points[2].PositionWorld = PrevPos;
    }
    // This activate when the black hole is enable. 

    void CurveToTheHole()
    {
        //Set to the Target, piont 2 is the last 
        _curve.Points[2].PointTransform = null;
        _curve.Points[2].PositionWorld = Hole.position;
        //this count the value of the Curve   
        Vector3 BreakPoint = (this.transform.position + _mainLight.BlackHole.transform.position) * 0.5f;
        _curve.Points[1].PositionWorld = new Vector3((_curve.Points[1].PositionWorld.x + BreakPoint.x) * 0.5f, 0,
            (Mathf.Min(Vector3.Distance(this.transform.position, _mainLight.BlackHole.transform.position) / 35f, 1.2f)) * (CurveWidth * (RayNum * (125 / _mainLight.NumOfLightis) - 50)));
        float c = Vector3.Distance(this.transform.position, _mainLight.BlackHole.transform.position);
        _curve.Points[1].ControlFirstLocal = new Vector3(-c * CurveAngle, 0, 0);
    }

    private void Update()
    {

        if (_mainLight._bhm.Active)
        {
            CurveToTheHole();

        }
        else
        {
            StraightForward();
        }
    }
    private Vector3 Pos;
    private GameObject g;

    private void FixedUpdate()
    {
        SetRayCast();
    }
    public Material ON;
    public Material OFF;
    public float H;
    private void SetRayCast()
    {
        for (int i = 0; i < Lr.positionCount - 1; i++)
        {
            Material mat;
            RaycastHit hit;
            Debug.DrawRay(Lr.GetPosition(i), Lr.GetPosition(i + 1), Color.red);
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(Lr.GetPosition(i), Lr.GetPosition(i)-Lr.GetPosition(i + 1), out hit, Vector3.Distance(Lr.GetPosition(i), Lr.GetPosition(i + 1))))
            {
                if (hit.collider.gameObject.CompareTag("Coll"))
                {
    
                    hit.collider.gameObject.GetComponent<ObjectControl>().hit = true;
                    
                }
            }
      
           
     
        }
    }
}
