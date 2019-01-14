using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform trans;
    [SerializeField] private float minX=-1;
    [SerializeField] private float maxX=1;
    [SerializeField] private float minY=-1;
    [SerializeField] private float maxY=1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float posX = Mathf.Clamp(trans.position.x, minX, maxX);
        float posY = Mathf.Clamp(trans.position.y, minY, maxY);
        transform.position= new Vector2(posX,posY);

    }
}
