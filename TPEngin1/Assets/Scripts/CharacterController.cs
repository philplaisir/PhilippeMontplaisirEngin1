using UnityEngine;

public class CharacterController : MonoBehaviour
{

    //[SerializeField]
    private Camera m_camera;

    //private bool m_isKeyDown = false;
    [SerializeField]
    private float m_accelerationValue;
    [SerializeField]
    private float m_maxVelocity = 10.0f;

    private Rigidbody m_rb;



    // Start is called before the first frame update
    void Start()
    {
        // Sans le serailizeField on peut garder la r�f�rence priv�e, et on va chercher directement la cam�ra
        m_camera = Camera.main;
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       






    }

    void FixedUpdate()
    {
        var vectorProjectedOnFLoor = Vector3.ProjectOnPlane(m_camera.transform.forward, Vector3.up);
        vectorProjectedOnFLoor.Normalize();

        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("");

            m_rb.AddForce(vectorProjectedOnFLoor * m_accelerationValue, ForceMode.Acceleration);


        }
        if (Input.GetKey(KeyCode.S))
        {



        }
        if (Input.GetKey(KeyCode.A))
        {



        }
        if (Input.GetKey(KeyCode.D))
        {



        }

        if(m_rb.velocity.magnitude > m_maxVelocity) 
        {
            m_rb.velocity = m_rb.velocity.normalized;
            m_rb.velocity *= m_maxVelocity;
        
        
        }
        // TODO 230831
        // Apliquer les d�placements relatifs � la cam�ra dans les 3 autres directions
        // Avoir des vitesse de d�placement diuff�renetes maximale vers les c�t�s et vers l'arri�re
        // Lorsqu'aucun input est d�tect� d�c�l�rer le personnage rapidement

        // TODO 230831
        // Essayer d'impl�menter d'autres types de d�placements (relatif au personnag, tank control)
        // Essayer d'ajouter contr�le avec manette
        
        Debug.Log(m_rb.velocity.magnitude);



    }


}
