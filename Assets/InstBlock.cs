using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml.Linq;

public class InstBlock : MonoBehaviour {
    #region переменные
    //игровой объект
    public GameObject Circle;
    public GameObject Cue;
    public bool Void=false;
    //структура цвет
    struct RGBcolor
    {
       public float R;
       public float G;
       public float B;
       public float A;
    }
    #endregion

    #region функции
    //функция парсинга цвета
    public Color parseColor(string s)
    {
        RGBcolor c;
        c.R = float.Parse(s.Substring(0, 5));
        c.G = float.Parse(s.Substring(7, 5));
        c.B = float.Parse(s.Substring(14, 5));
        c.A = 1f;
        return new Color(c.R, c.G, c.B, c.A);
    }
    // парсинг координат
    public Vector3 parseVector(string v)
    {
        string[] coord = v.Split(',');
        string q = coord[0] + coord[1];
        coord = q.Split();
        return new Vector3(float.Parse(coord[0]), float.Parse(coord[1]),0);
    }
    // построение игрового поля
    public void BildPole()
    {
        var pole = XDocument.Parse(System.IO.File.ReadAllText("block.xml")).Descendants("Ball").ToList<XElement>();

        foreach (var e in pole)
        {
            Circle.GetComponent<SpriteRenderer>().color = parseColor((e.Attribute("color").Value).Substring(5, 26));
            Instantiate(Circle, parseVector((e.Attribute("transform").Value).Substring(1, 9)), Quaternion.identity);
        }
    }
    // позиционирование шара-биты
    public void CueBall()
    {
        var pole = XDocument.Parse(System.IO.File.ReadAllText("block.xml")).Descendants("Ball").ToList<XElement>();
        Cue.GetComponent<SpriteRenderer>().color = parseColor((pole[Random.Range(0, pole.Count)].Attribute("color").Value).Substring(5, 26));
        Instantiate(Cue,new Vector3(0,-5,0),Quaternion.identity);
        Void = true;
    }

    #endregion

    void Start() {

        BildPole();
    }

    private void Update()
    {
        if (Void == false) CueBall();
        
    }


}
	
	

