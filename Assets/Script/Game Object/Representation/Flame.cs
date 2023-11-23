using UnityEngine;

/// <summary>
/// Ateþ objesini temsil eden sýnýf
/// </summary>
public class Flame : MonoBehaviour
{
    bool isFiring;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Building") && !isFiring && gameObject.transform.parent != collision.transform)
        {
            gameObject.transform.parent = collision.transform;
            isFiring = true;
            CheckForNearbyObjects();
        }
    }

    void CheckForNearbyObjects()
    {
        // Child objenin pozisyonu
        Vector3 childPosition = transform.localPosition;

        // 1 birim sað, sol, ileri ve geri pozisyonlar
        Vector3 rightPosition = childPosition + new Vector3(0.2f, 0, 0);
        Vector3 leftPosition = childPosition + new Vector3(-0.2f, 0, 0);
        Vector3 forwardPosition = childPosition + new Vector3(0, 0, 0.2f);
        Vector3 backwardPosition = childPosition + new Vector3(0, 0, -0.2f);

        // Verilen pozisyondaki objeleri al
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 1f);

        // Çevresindeki objeleri kontrol et
        CheckForObject(rightPosition, hitColliders);
        CheckForObject(leftPosition, hitColliders);
        CheckForObject(forwardPosition, hitColliders);
        CheckForObject(backwardPosition, hitColliders);
    }

    void CheckForObject(Vector3 position, Collider[] hitColliders)
    {
        

        // Objeleri kontrol et
        foreach (Collider collider in hitColliders)
        {
            // Eðer tag ayný ise
            if (collider.gameObject!=gameObject && collider.CompareTag(transform.tag))
            {
                Debug.Log(position);
                return;
            }
        }
        var newFlame = Instantiate(gameObject, gameObject.transform.parent);
        newFlame.transform.localPosition = position;

    }


    /*
    // Update is called once per frame
    void Update()
    {
        if (isFiring)
        {
            bool cocukVar=false;
            var newChildPos = new Vector3(transform.localPosition.x + 0.2f, transform.localPosition.y, transform.localPosition.z);
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.transform.GetChild(i).CompareTag("Flame"))
                {
                    if (transform.parent.transform.GetChild(i).transform.localPosition==newChildPos)
                    {
                        cocukVar = true;
                    }
                }
            }
            if(!cocukVar)
            {
                var newFlame = Instantiate(gameObject, gameObject.transform.parent);
                newFlame.transform.localPosition = newChildPos;
            }
            
        }
    }*/
}
