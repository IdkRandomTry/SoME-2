using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGrid : MonoBehaviour
{
//    public GameObject basesqr;
    public float thickness, unit_length; 
    public Color colour;
    private RectTransform baserect;
    private Vector3 lefttop, rightbottom, center, centertop, centerbottom, unit, unit_vec;
    private Vector3 ahead = new Vector3(0, 0, -0.1f);
    private float x_max, y_max;


    void Start()
    {
//        duplicate();
        next_guidelines();
 /*       for (var i = 0; i < 4; i++)
        {
            Debug.Log("World Corner " + i + " : " + allcorners[i]);
            edges.SetPosition(i, allcorners[i]);
        }*/
    }

    private void next_guidelines() 
    {
        baserect = GetComponent<RectTransform>();
        Vector3[] allcorners = new Vector3[4];
        baserect.GetWorldCorners(allcorners);
        lefttop = allcorners[1] + ahead;
        rightbottom = allcorners[3] + ahead;
        center = baserect.localPosition + ahead;
        unit_vec = new Vector3(unit_length, unit_length, unit_length);
        
        GameObject guidelineObj = new GameObject("guidelineObj");
        LineRenderer guideline = guidelineObj.AddComponent<LineRenderer>();
        guideline.positionCount = 2;
        guideline.widthMultiplier = thickness;
        guideline.material = new Material(Shader.Find("Sprites/Default"));
        guideline.material.color = colour;
        
        x_max = Mathf.Floor(rightbottom.x/unit_length);
        unit = Vector3.Scale(unit_vec, Vector3.right);
        guideline.SetPosition(0, new Vector3(center.x - unit_length * x_max ,lefttop.y, center.z));
        guideline.SetPosition(1, new Vector3(center.x - unit_length * x_max ,rightbottom.y, center.z));

        for (var i = -x_max; i<=x_max; i++)
        {
            GameObject inst = GameObject.Instantiate(guidelineObj);
            inst.transform.parent = guidelineObj.transform.parent;
            LineRenderer new_guideline = inst.GetComponent<LineRenderer>();
            new_guideline.SetPosition(0, guideline.GetPosition(0)+unit);
            new_guideline.SetPosition(1, guideline.GetPosition(1)+unit);
            guideline = new_guideline;
            if (i == -1)
            {
                new_guideline.widthMultiplier = thickness*3;
            }
        }

        y_max = Mathf.Floor(lefttop.y/unit_length);
        unit = Vector3.Scale(unit_vec, Vector3.up);
        guideline.SetPosition(0, new Vector3(lefttop.x, center.y - unit_length*y_max , center.z));
        guideline.SetPosition(1, new Vector3(rightbottom.x,center.y - unit_length*y_max, center.z));

        for (var i = -y_max; i<y_max; i++)
        {
            GameObject inst = GameObject.Instantiate(guidelineObj);
            inst.transform.parent = guidelineObj.transform.parent;
            LineRenderer new_guideline = inst.GetComponent<LineRenderer>();
            new_guideline.SetPosition(0, guideline.GetPosition(0)+unit);
            new_guideline.SetPosition(1, guideline.GetPosition(1)+unit);
            guideline = new_guideline;
            if (i == -1)
            {
                new_guideline.widthMultiplier = thickness*3;
            }
        }
    }
}