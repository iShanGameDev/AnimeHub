using UnityEngine;

public class Rot : MonoBehaviour
{
    public float Speed;
    void Update()
    {
        this.transform.Rotate(new Vector3(0,0,1), Time.deltaTime * this.Speed);
    }
}
