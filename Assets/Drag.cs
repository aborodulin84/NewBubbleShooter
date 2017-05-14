using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour {
    #region переменные
    float posX;
    float posY;
    float Impulse;
    int i = 0;
    Vector3 Start;
    Vector3 dist;
    Vector3[] Points = null;
    class Line
    {
        public float a;
            public float b;
       public Line(Vector3 i,Vector3 j)
        {
            a = (i.y - j.y) / (i.x - j.x);
            b = i.y - (i.y - j.y) / (i.x - j.x) * i.x;
        }
    }
    LineRenderer lineRenderer;
    #endregion

    #region функции
    void PosInit()  //подбор координат шарика
    {
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
    }

    void CarryBall() //перенос шарика
    {
        Vector3 curPos =
                  new Vector3(Input.mousePosition.x - posX,
                  Input.mousePosition.y - posY, dist.z);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        transform.position = worldPos;
    }

    float meterImpulse(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
    }

    void RenderLine(Vector3 i, Vector3 j )
    {
        Line tr = new Line(i, j);
        lineRenderer.SetPosition(0, transform.position);
        if (Start.x == transform.position.x) lineRenderer.SetPosition(1, new Vector3(transform.position.x, 6, 1));
        else
        {
            lineRenderer.SetPosition(1, new Vector3((transform.position.x > 0 ? -8.5f : 8.5f), tr.a*(transform.position.x > 0 ? -8.5f : 8.5f) +tr.b, 0));
            lineRenderer.SetPosition(2, new Vector3((transform.position.x < 0 ? -8.5f : 8.5f), tr.a*(lineRenderer.GetPosition(1).x), 0));
        }
    }

    #endregion

   private void OnMouseDown()
    {
        Start = transform.position;
        lineRenderer = GetComponent<LineRenderer>();
        PosInit();
        lineRenderer.enabled = true;
    }

   private void OnMouseDrag()
    {
        CarryBall();
        RenderLine(Start, transform.position);
        Impulse = meterImpulse(Start, transform.position);
    }

    private void OnMouseUp()
    {
        Points = new Vector3[3];
        lineRenderer.GetPositions(Points);
        lineRenderer.enabled = false;
        foreach(Vector3 p in Points)
        {
            Debug.Log(p);
        }
    }

    private void Update()
    {
        if (i == 2) { Points = null; i = 0; }
        if (Points != null)
        {
            Vector3 dir = (Points[i + 1] - transform.position).normalized * Impulse * Time.deltaTime * 10;
            transform.Translate(dir); // Vector3.Lerp(transform.position, Points[i+1], Vector3.Distance(Start,Dash)*Time.deltaTime);
            if (transform.position.x > 8 || transform.position.x < -8) i++;

        }
    }

}
