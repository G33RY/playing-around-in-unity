using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private GameObject StartObject;
    private GameObject EndObject;
    private LineRenderer LineRenderer;
    public TMPro.TextMeshPro Text;
    //public int azimuth = 90;

    // Start is called before the first frame update
    void Start()
    {
        this.StartObject = GetChildWithName(this.gameObject, "Start");
        this.EndObject = GetChildWithName(this.gameObject, "End");
        this.LineRenderer =  this.gameObject.AddComponent<LineRenderer>();
        
        this.LineRenderer.startColor = Color.red;
        this.LineRenderer.endColor = Color.red ;
        this.LineRenderer.positionCount = 90;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = this.gameObject.transform.localScale;
        float r = (scale.x / 2) + 2;


        Vector3 start = this.StartObject.transform.position;
        Vector3 end = this.EndObject.transform.position;



        this.StartObject.transform.position = ProjectPointToSphere(start, r);
        this.EndObject.transform.position = ProjectPointToSphere(end, r);

        List<Vector3> linePoints = LineBetweenTwoPointsOnSphere(start, end, r, 2);
        this.LineRenderer.positionCount = linePoints.Count;
        this.LineRenderer.SetPositions(linePoints.ToArray());


    }


    float D2R(float deg)
    {
        return Mathf.Deg2Rad * deg;
    }
    float R2D(float rad)
    {
        return  Mathf.Rad2Deg * rad;
    }

    float GetAzimuthAngleOfVector(Vector3 vector)
    {
        Vector2 azimuthVector = new Vector2(vector.x, vector.z);
        Vector2 azimuthBaseVector = new Vector2(1, 0);

        float azimuthAngle = Mathf.Acos((azimuthVector.x * azimuthBaseVector.x + azimuthVector.y * azimuthBaseVector.y) / (Mathf.Sqrt(Mathf.Pow(azimuthVector.x, 2) + Mathf.Pow(azimuthVector.y, 2)) * Mathf.Sqrt(Mathf.Pow(azimuthBaseVector.x, 2) + Mathf.Pow(azimuthBaseVector.y, 2))));
        if (vector.z < 0) azimuthAngle *= -1;
        azimuthAngle += Mathf.PI;

        return azimuthAngle;
    }
    


    float GetPolarAngleOfVector(Vector3 vector)
    {
        return Mathf.Acos(vector.y / Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2) + Mathf.Pow(vector.z, 2)));
    }


    List<Vector3> LineBetweenTwoPointsOnSphere(Vector3 p1, Vector3 p2, float radius, float frequency = 1)
    {
        List<Vector3> points = new List<Vector3>();


        Vector3 v = p2 - p1;
        for (int i = 0; i < v.magnitude*frequency; i++)
        {
            Vector3 projected = ProjectPointToSphere(p2 - new Vector3(v.x - (v.x / (v.magnitude * frequency) * i), v.y - (v.y / (v.magnitude * frequency) * i), v.z - (v.z / (v.magnitude * frequency) * i)), radius);
            points.Add(projected);
        }

        return points;
    }

    Vector3 ProjectPointToSphere(Vector3 point, float radius)
    {
        float d = Mathf.Sqrt(Mathf.Pow(point.x, 2) + Mathf.Pow(point.y, 2) + Mathf.Pow(point.z, 2));
        float x = (point.x / d) * radius;
        float y = (point.y / d) * radius;
        float z = (point.z / d) * radius;

        return new Vector3(x, y, z);
    }

    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }


}
