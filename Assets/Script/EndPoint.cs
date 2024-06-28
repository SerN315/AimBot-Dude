using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private BoxCollider2D boxCollider2;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
    gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.tag);
        if (hitInfo.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            gameManager.PlayerWon();

}
}
}
