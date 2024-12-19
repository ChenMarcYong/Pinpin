using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public static PlayerShield singleton;

    [Header("References")]
    [SerializeField] private GameObject shieldRight;
    [SerializeField] private GameObject shieldLeft;
    private bool isShielding = false;
    private bool getsDamaged = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() 
    {
        if (singleton == null) singleton = this;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        isShielding = PlayerController.singleton.getIsShielding();
        //UnityEngine.Debug.Log(health);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isShielding)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnnemiProjectile"))
            {
                Destroy(collision.gameObject);
                //UnityEngine.Debug.Log("Projectile détruit par le bouclier.");
            }
        }
    }

    public bool getGetsDamaged() 
    {
        return getsDamaged;
    }
}
