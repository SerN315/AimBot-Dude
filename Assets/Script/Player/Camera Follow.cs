
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2.0f;
    public float yOffSet = 1f;
    public float xOffSet = 1f;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x + xOffSet, target.position.y + yOffSet,-10f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed*Time.deltaTime);    
    }
}
