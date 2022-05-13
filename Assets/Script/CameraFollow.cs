using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float moveSpeed;
    public float turnSpeed;

    Vector3 offset;
    RaycastHit rh;
    float dis;
    Vector3[] currentPoints;

    void Awake() 
    {
        moveSpeed = 3f;
        turnSpeed = 10f;
        currentPoints = new Vector3[5];
    }
    void Start() 
    {
        dis = Vector3.Distance(transform.position, player.position);
        offset = player.position - transform.position;
    }

    void LateUpdate()
    {
        Vector3 startPos = player.position - offset;
        Vector3 endPos = player.position + Vector3.up * dis;
        currentPoints[0] = startPos;
        currentPoints[1] = Vector3.Slerp(startPos, endPos, 0.25f);
        currentPoints[2] = Vector3.Slerp(startPos, endPos, 0.5f);
        currentPoints[3] = Vector3.Slerp(startPos, endPos, 0.75f);
        currentPoints[4] = endPos;
        Vector3 view = currentPoints[0];
        for (int i = 0; i < currentPoints.Length; i++)
        {
            if (CheckPlayer(currentPoints[i])) 
            {
                view = currentPoints[i];
                break;
            }
        }
        transform.position = Vector3.Lerp(transform.position, view, Time.deltaTime * moveSpeed);
        Rotate();
    }
    public bool CheckPlayer(Vector3 pos) 
    {
        Vector3 dir = player.position - pos;
        if (Physics.Raycast(pos, dir, out rh)) 
        {
            if (rh.collider.CompareTag("Player")) 
            {
                return true;
            }
        }
        return false;
    }
    public void Rotate() 
    {
        Vector3 Dir = player.position - transform.position;
        Quaternion qua = Quaternion.LookRotation(Dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, qua, Time.deltaTime * turnSpeed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
    }
}
