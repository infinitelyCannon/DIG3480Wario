using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DaSiGameManager : MonoBehaviour {

    public float speed;
    //public GameObject Train;
    public Text timeText;

    private Vector3 startingPoint;
    private List<Vector3> keyPoints;
    private GameObject infoText;
    //private List<Vector3> trackPoints;
    //private Vector3 direction = Vector3.right;
    //private SpriteShapeController spriteShapeController;
    private Spline spline;
    private bool isDone = false;
    private float timer = 0f;
    private struct Moving
    {
        public Vector3 direction;
        public Vector3 isMoving;

        public Moving(Vector3 m, Vector3 d)
        {
            isMoving = m;
            direction = d;
        }
    }
    private Moving movement = new Moving(Vector3.zero, Vector3.right);

    private void Awake()
    {
        spline = GetComponent<SpriteShapeController>().spline;
    }

    // Use this for initialization
    void Start () {
        startingPoint = gameObject.transform.position;
        infoText = GameObject.Find("IntroText");
        keyPoints = new List<Vector3>
        {
            spline.GetPosition(0)
        };
        //trackPoints = new List<Vector3>();
	}
	
	// Update is called once per frame
	void Update () {
        int index;
        Vector3 currentPos;

        if (Input.GetKeyDown(KeyCode.UpArrow) && movement.isMoving == Vector3.zero) movement.isMoving = Vector3.up;
        if (Input.GetKeyDown(KeyCode.DownArrow) && movement.isMoving == Vector3.zero) movement.isMoving = Vector3.down;
        if (Input.GetKeyDown(KeyCode.RightArrow) && movement.isMoving == Vector3.zero) movement.isMoving = Vector3.right;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && movement.isMoving == Vector3.zero) movement.isMoving = Vector3.left;

        spline = GetComponent<SpriteShapeController>().spline;
        index = spline.GetPointCount() - 1;
        currentPos = spline.GetPosition(index);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if(movement.direction == Vector3.up && movement.isMoving == Vector3.up)
            {
                spline.SetPosition(index, currentPos + (Vector3.up * speed * Time.deltaTime));
            }

            else if(movement.direction != Vector3.down && movement.isMoving == Vector3.up)
            {
                movement.direction = Vector3.up;
                spline.SetPosition(index, currentPos + Vector3.right);
                spline.InsertPointAt(index, currentPos);
                index = spline.GetPointCount() - 1;
                spline.SetPosition(index, currentPos + movement.direction);
                spline.SetCorner(index - 1, true);
                keyPoints.Add(spline.GetPosition(index));
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if(movement.direction == Vector3.right && movement.isMoving == Vector3.right)
            {
                spline.SetPosition(index, currentPos + (Vector3.right * speed * Time.deltaTime));
            }

            else if(movement.direction != Vector3.left && movement.isMoving == Vector3.right)
            {
                movement.direction = Vector3.right;
                spline.SetPosition(index, currentPos + Vector3.right);
                spline.InsertPointAt(index, currentPos);
                index = spline.GetPointCount() - 1;
                spline.SetPosition(index, currentPos + movement.direction);
                spline.SetCorner(index - 1, true);
                keyPoints.Add(spline.GetPosition(index));
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(movement.direction == Vector3.left && movement.isMoving == Vector3.left)
            {
                spline.SetPosition(index, currentPos + (Vector3.left * speed * Time.deltaTime));
            }

            else if(movement.direction != Vector3.right && movement.isMoving == Vector3.left)
            {
                movement.direction = Vector3.left;
                spline.SetPosition(index, currentPos + Vector3.right);
                spline.InsertPointAt(index, currentPos);
                index = spline.GetPointCount() - 1;
                spline.SetPosition(index, currentPos + movement.direction);
                spline.SetCorner(index - 1, true);
                keyPoints.Add(spline.GetPosition(index));
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {   
            if(movement.direction == Vector3.down && movement.isMoving == Vector3.down)
            {
                spline.SetPosition(index, currentPos + (Vector3.down * speed * Time.deltaTime));
            }

            else if(movement.direction != Vector3.up && movement.isMoving == Vector3.down)
            {
                movement.direction = Vector3.down;
                spline.SetPosition(index, currentPos + Vector3.right);
                spline.InsertPointAt(index, currentPos);
                index = spline.GetPointCount() - 1;
                spline.SetPosition(index, currentPos + movement.direction);
                spline.SetCorner(index - 1, true);
                keyPoints.Add(spline.GetPosition(index));
            }
        }

        if(Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            //PrintSpline();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow))
            movement.isMoving = Vector3.zero;

        if (Input.GetKeyUp(KeyCode.I) && !isDone)
        {
            isDone = true;
            StartCoroutine(DistancesInSecs(GameObject.Find("DaSiTrain"), transform.position, keyPoints));
        }

        if(timer <= 10f && !isDone)
        {
            timer += Time.deltaTime;
            timeText.text = Mathf.Clamp((10f - timer), 0, 10f).ToString();
        }

        if(timer >= 10f && !isDone)
        {
            isDone = true;
            StartCoroutine(DistancesInSecs(GameObject.Find("DaSiTrain"), transform.position, keyPoints));
        }
	}

    float SumDistance(/*List<Vector3> vects*/)
    {
        float result = 0f;
        int count = spline.GetPointCount();

        for (int i = 0; i < count; i++)
        {
            if(i < count - 1)
            {
                result += Mathf.Abs(Vector3.Distance(spline.GetPosition(i), spline.GetPosition(i + 1)));
            }
        }

        return result;
    }

    IEnumerator DistancesInSecs(GameObject obj, Vector3 start, List<Vector3> points)
    {
        float sumDist = SumDistance();
        obj.transform.position = start;
        float subDist = 0f;
        float time = 1.1f;
        int count = spline.GetPointCount();

        obj.GetComponent<DaSiTrainScript>().Run();

        for (int i = 0; i < count; i++)
        {
            if(i < count - 1)
            {
                subDist = Mathf.Abs(Vector3.Distance(spline.GetPosition(i), spline.GetPosition(i + 1)));
                time = (subDist / sumDist) * 1.1f;
            }
            while (Mathf.Abs(Vector3.Distance(obj.transform.position, spline.GetPosition(i) + startingPoint)) > Mathf.Epsilon)
            {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, spline.GetPosition(i) + startingPoint, (subDist / time) * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }

        GameLoader.AddScore((int)Mathf.Clamp(obj.GetComponent<DaSiTrainScript>().NumHit(), 0f, 10f));
        infoText.GetComponent<DaSiIntroScript>().GameOver(obj.GetComponent<DaSiTrainScript>().NumHit() >= 10);
        yield return new WaitForSeconds(0.9f);
        GameLoader.gameOn = false;
    }

    void PrintSpline()
    {
        int i = spline.GetPointCount();

        Debug.Log("Found " + i + " points:");

        for (int j = 0; j < i; j++)
        {
            Debug.Log("Index " + j + " at: " + spline.GetPosition(j).ToString());
        }
    }
}
