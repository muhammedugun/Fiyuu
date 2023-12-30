using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Yapý(Bina) objelerini temsil eder
/// </summary>
public class Building : MonoBehaviour, ISmashable
{
    public static event Action<float, BuildingMatter> OnBuildSmashed; // float yapýnýn hacmi, BuildingMatter yapýnýn zýrhý
    public float Durability { get { return _durability; } set { _durability = value; } }

    [Tooltip("Yapýnýn zýrhý")]
    [SerializeField] internal BuildingMatter armor;
    [Tooltip("Bu objenin parçalanabilir örneði")]
    [SerializeField] internal GameObject smashableObjectPrefab;

    private float _durability;
    /// <summary>
    /// Objenin hacmi. Yani objenin uzayda kapladýðý alanýn boyutu.
    /// </summary>
    private float _volumeSize;

    /// <summary>
    /// Zýrh dayanýklýklarý. Zýrhýn neye dayanýklý olup neye dayanýklý olmadýðý. Satýrlar: Bina maddesi(zýrhý), Sütunlar: mühimmat maddesi
    /// </summary>
    private int[,] armorStrengths = new int[4, 8]
    {
        // Mühimmat maddesi türleri (sütunlar).
        // Ahþap=1, taþ=2, demir=3, çelik=4, ateþ=5, buz=6, patlama=7, elektrik=8
        // Bina maddesi türleri (satýrlar)
        { 0,0,0,0,0,0,0,1 }, // Ahþap
        { 1,0,0,0,1,0,0,1 }, // Taþ
        { 1,1,0,0,1,0,0,0 }, // Demir
        { 1,1,1,1,1,1,1,0 }  // Çelik
    };


    internal float[] _matterDurabilitiy = new float[4] { 2000f, 4000f, 6000f, 8000f};

    /// <summary>
    /// Parçalanma gerçekleþti mi?
    /// </summary>
    private bool _isSmash;
    private void Start()
    {
        AssignDurability();
        AssignVolume(gameObject.GetComponent<Renderer>(), ref _volumeSize);
        AssignMass(gameObject.GetComponent<Rigidbody>(), _volumeSize);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isSmash)
        {
            DoDamage(collision);
            if (CheckSmash())
            {
                //StartCoroutine(Smash(smashableObjectPrefab));
                Smash(collision);

            }
        }
    }

    /// <summary>
    /// Objenin hacmini atar
    /// </summary>
    private void AssignVolume(Renderer renderer, ref float volumeSize)
    {
        Bounds bounds = renderer.bounds;
        volumeSize = bounds.size.x * bounds.size.y * bounds.size.z;
    }

    /// <summary>
    /// Objenin aðýrlýðýný atar
    /// </summary>
    private void AssignMass(Rigidbody rb, float volumeSize)
    {
        if (volumeSize <= 0)
        {
            Debug.LogError("Yapýnýn hacmi atanmamýþ ya da hacmi sýfýr");
        }
        else
        {
            rb.mass = (int)armor * (volumeSize);
        }

    }

    /// <summary>
    /// Objenin dayanýklýlýðýný atar
    /// </summary>
    private void AssignDurability()
    {
        _durability = _matterDurabilitiy[(int)armor - 1];
    }

    public void DoDamage(Collision collision)
    {

        if (collision.transform.CompareTag("Ammo"))
        {
            var ammo = collision.gameObject.GetComponent<Ammo>();
            var ammoArmor = ammo.matter;
            if (armorStrengths[(int)armor - 1, (int)ammoArmor - 1] == 0)
            {
                var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
                Debug.Log("collisionForce: " + collisionForce);
                _durability -= collisionForce * ammo.power[(int)ammoArmor - 1];
                Debug.Log("damage: " + collisionForce * ammo.power[(int)ammoArmor - 1]);
            }
        }
        else if(collision.gameObject.layer!=LayerMask.NameToLayer("Node"))
        {
            var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
            _durability -= collisionForce;
        }
    }

    /// <summary>
    /// Parçalamayý gerçekleþtirir
    /// </summary>
    /// <param name="smashableObject">Objenin parçalanabilir halinin örneði</param>
    public IEnumerator Smash(GameObject smashableObject)
    {

        _isSmash = true;


        var initializedSmashableObject = Instantiate(smashableObject, gameObject.transform.position, gameObject.transform.localRotation);
        initializedSmashableObject.transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, transform.localScale.z);
        for (int i = 0; i < initializedSmashableObject.transform.childCount; i++)
        {
            var child = initializedSmashableObject.transform.GetChild(i);
            float volumeSize = 0;
            AssignVolume(child.GetComponent<Renderer>(), ref volumeSize);
            var rb = child.GetComponent<Rigidbody>();
            AssignMass(rb, volumeSize);
            child.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        }



        OnBuildSmashed?.Invoke(_volumeSize, armor);

        yield return null;

        gameObject.SetActive(false);


    }


    public void Smash(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            // Collision force must exceed the minimum force (F = I / T)
            var contact = collision.contacts[0];
            callbackOptions.CallOnFracture(contact.otherCollider, gameObject, contact.point);
            this.ComputeFracture();
        }

        OnBuildSmashed?.Invoke(_volumeSize, armor);
    }

    /// <summary>
    /// Parçalamanýn gerçekleþebilirliðini kontrol eder
    /// </summary>
    /// <param name="collision"></param>
    public bool CheckSmash()
    {
        if (_durability <= 0) return true;
        else return false;
    }


    /// <summary>
    /// Collector object that stores the produced fragments
    /// </summary>
    private GameObject fragmentRoot;


    public FractureOptions fractureOptions;
    public RefractureOptions refractureOptions;
    public CallbackOptions callbackOptions;
    public TriggerOptions triggerOptions;

    /// <summary>
    /// The number of times this fragment has been re-fractured.
    /// </summary>
    [HideInInspector]
    public int currentRefractureCount = 0;

    /// <summary>
    /// Compute the fracture and create the fragments
    /// </summary>
    /// <returns></returns>
    private void ComputeFracture()
    {
        var mesh = this.GetComponent<MeshFilter>().sharedMesh;

        if (mesh != null)
        {
            // If the fragment root object has not yet been created, create it now
            if (this.fragmentRoot == null)
            {
                // Create a game object to contain the fragments
                this.fragmentRoot = new GameObject($"{this.name}Fragments");
                this.fragmentRoot.transform.SetParent(this.transform.parent);

                // Each fragment will handle its own scale
                this.fragmentRoot.transform.position = this.transform.position;
                this.fragmentRoot.transform.rotation = this.transform.rotation;
                this.fragmentRoot.transform.localScale = Vector3.one;
            }

            var fragmentTemplate = CreateFragmentTemplate();

            if (fractureOptions.asynchronous)
            {
                StartCoroutine(Fragmenter.FractureAsync(
                    this.gameObject,
                    this.fractureOptions,
                    fragmentTemplate,
                    this.fragmentRoot.transform,
                    () =>
                    {
                        // Done with template, destroy it
                        GameObject.Destroy(fragmentTemplate);

                        // Deactivate the original object
                        this.gameObject.SetActive(false);

                        // Fire the completion callback
                        if ((this.currentRefractureCount == 0) ||
                            (this.currentRefractureCount > 0 && this.refractureOptions.invokeCallbacks))
                        {
                            if (callbackOptions.onCompleted != null)
                            {
                                callbackOptions.onCompleted.Invoke();
                            }
                        }
                    }
                ));
            }
            else
            {
                Fragmenter.Fracture(this.gameObject,
                                    this.fractureOptions,
                                    fragmentTemplate,
                                    this.fragmentRoot.transform);

                // Done with template, destroy it
                GameObject.Destroy(fragmentTemplate);

                // Deactivate the original object
                this.gameObject.SetActive(false);

                // Fire the completion callback
                if ((this.currentRefractureCount == 0) ||
                    (this.currentRefractureCount > 0 && this.refractureOptions.invokeCallbacks))
                {
                    if (callbackOptions.onCompleted != null)
                    {
                        callbackOptions.onCompleted.Invoke();
                    }
                }
            }
        }
    }


    /// <summary>
    /// Creates a template object which each fragment will derive from
    /// </summary>
    /// <param name="preFracture">True if this object is being pre-fractured. This will freeze all of the fragments.</param>
    /// <returns></returns>
    private GameObject CreateFragmentTemplate()
    {
        // If pre-fracturing, make the fragments children of this object so they can easily be unfrozen later.
        // Otherwise, parent to this object's parent
        GameObject obj = new GameObject();
        obj.name = "Fragment";
        obj.tag = this.tag;

        // Update mesh to the new sliced mesh
        obj.AddComponent<MeshFilter>();

        // Add materials. Normal material goes in slot 1, cut material in slot 2
        var meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterials = new Material[2] {
            this.GetComponent<MeshRenderer>().sharedMaterial,
            this.fractureOptions.insideMaterial
        };

        // Copy collider properties to fragment
        var thisCollider = this.GetComponent<Collider>();
        var fragmentCollider = obj.AddComponent<MeshCollider>();
        fragmentCollider.convex = true;
        fragmentCollider.sharedMaterial = thisCollider.sharedMaterial;
        fragmentCollider.isTrigger = thisCollider.isTrigger;

        // Copy rigid body properties to fragment
        var thisRigidBody = this.GetComponent<Rigidbody>();
        var fragmentRigidBody = obj.AddComponent<Rigidbody>();
        fragmentRigidBody.velocity = thisRigidBody.velocity;
        fragmentRigidBody.angularVelocity = thisRigidBody.angularVelocity;
        fragmentRigidBody.drag = thisRigidBody.drag;
        fragmentRigidBody.angularDrag = thisRigidBody.angularDrag;
        fragmentRigidBody.useGravity = thisRigidBody.useGravity;

        // If refracturing is enabled, create a copy of this component and add it to the template fragment object
        if (refractureOptions.enableRefracturing &&
           (this.currentRefractureCount < refractureOptions.maxRefractureCount))
        {
            CopyFractureComponent(obj);
        }

        return obj;
    }


    /// <summary>
    /// Convenience method for copying this component to another component
    /// </summary>
    /// <param name="obj">The GameObject to copy the component to</param>
    private void CopyFractureComponent(GameObject obj)
    {
        var fractureComponent = obj.AddComponent<Fracture>();

        fractureComponent.triggerOptions = this.triggerOptions;
        fractureComponent.fractureOptions = this.fractureOptions;
        fractureComponent.refractureOptions = this.refractureOptions;
        fractureComponent.callbackOptions = this.callbackOptions;
        fractureComponent.currentRefractureCount = this.currentRefractureCount + 1;
        fractureComponent.fragmentRoot = this.fragmentRoot;
    }



}



/// <summary>
/// Yapýnýn maddesi(zýrhý) 
/// </summary>
public enum BuildingMatter
{
    Wood = 1,
    Stone,
    Iron,
    Steel
}


