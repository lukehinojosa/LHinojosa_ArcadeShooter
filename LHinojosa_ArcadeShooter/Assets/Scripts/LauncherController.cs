using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LauncherController : MonoBehaviour
{
    private float clickRadius = 2f;
    private Camera camera;
    public GameObject dragCircle;

    private bool dragging = false;

    public GameObject[] projectiles;

    public Queue<GameObject> projectileQueue;

    [SerializeField] float forceMultiplier = 1f;

    public GameObject dotObject;

    private float dotAmount = 50f;
    private List<GameObject> dots;
    List<Vector3> nodes = new List<Vector3>();
    private float dotSpacing = 0.2f;
    
    void Start()
    {
        camera = Camera.main;
        projectileQueue = new Queue<GameObject>(projectiles);
        
        dots = new List<GameObject>();
        for (int i = 0; i < dotAmount; i++)
        {
            dots.Add(Instantiate(dotObject, new Vector3(0f, 0f, 0f), Quaternion.identity));
            dots[i].SetActive(false);
        }
    }
    
    void Update()
    {
        Controls();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Controls()
    {
        Vector3 camWrldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        
        float mouseDistance = Mathf.Sqrt(Mathf.Pow(camWrldPos.x - transform.position.x, 2) + 
                                         Mathf.Pow(camWrldPos.y - transform.position.y, 2));
        
        if (!dragging && projectileQueue.Count != 0 && mouseDistance <= clickRadius && Input.GetMouseButtonDown(0))
        {
            dragging = true;
            
            projectileQueue.Peek().gameObject.transform.SetParent(dragCircle.transform);
            projectileQueue.Peek().gameObject.transform.localPosition = Vector3.zero;
        }
        
        float adjacent = camWrldPos.x - transform.position.x;
        float opposite = camWrldPos.y - transform.position.y;
        float hypotenuse = Mathf.Sqrt(Mathf.Pow(adjacent, 2) + Mathf.Pow(opposite, 2));
            
        Vector3 direction = new Vector3(adjacent / hypotenuse, opposite / hypotenuse, 0f);
        
        if (mouseDistance > clickRadius)
            mouseDistance = clickRadius;
        
        if (dragging && Input.GetMouseButton(0))
        {
            dragCircle.SetActive(true);
            
            dragCircle.transform.position = transform.position + new Vector3(direction.x * mouseDistance, direction.y * mouseDistance, 0f);
            
            GenerateSamples(dragCircle.transform.position, -direction, mouseDistance * forceMultiplier, dotAmount, dotSpacing);

            for (int i = 0; i < dots.Count; i++)
            {
                dots[i].SetActive(true);
                dots[i].transform.position = nodes[i];
            }
        }
        
        if (dragging && Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < dots.Count; i ++)
                dots[i].SetActive(false);
            
            dragCircle.SetActive(false);
            dragging = false;
            
            projectileQueue.Peek().gameObject.transform.SetParent(null);
            projectileQueue.Peek().gameObject.GetComponent<ProjectileScript>().doGravity = true;
            projectileQueue.Peek().gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * (mouseDistance * forceMultiplier), ForceMode2D.Impulse);
            projectileQueue.Peek().gameObject.GetComponent<Rigidbody2D>().excludeLayers = LayerMask.GetMask();
            
            projectileQueue.Dequeue();
        }
    }
    
    void GenerateSamples(Vector3 origin, Vector3 direction, float strength, float samples, float spacing)
    {
        nodes.Clear();
        
        Vector3 pos = origin + direction * strength * spacing;
        pos += projectileQueue.Peek().gameObject.GetComponent<ProjectileScript>().forceVector;;
        nodes.Add(pos);
        
        for (int i = 0; i < samples; i++)
        {
            if (i != 0)
            {
                Vector3 nextPosition = pos;
                pos += projectileQueue.Peek().gameObject.GetComponent<ProjectileScript>()
                        .UpdateGameobjectForce(nextPosition);
            }
            
            pos *= i;
        }
    }
}
