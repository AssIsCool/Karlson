using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    Renderer renderer;
    level level;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        level = GetComponentInParent<level>();
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.layer == 9)
            ChangeCollarOnWall(coll.gameObject);
    }

    private void ChangeCollarOnWall(GameObject gameObject)
    {
        Color otherColor = renderer.material.color;
        //if (otherColor == Color.red)
        //    renderer.material.SetColor("_Color", Color.gray);

        //if (level.redWallAmount > 0)
        //    level.redWallAmount--;
        //else
        //{
            renderer.material.SetColor("_Color", Color.red);
            level.redWallAmount++;

            if (level.redWallAmount >= level.myWalls.Length)
            {
                Destroy(level.door.gameObject);
                //gameObject.GetComponent<Movement>().levelCompleted = true; 
            }
        //}
            
    }
    
    
}
