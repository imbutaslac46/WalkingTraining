using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : MonoBehaviour { //to be changed into ObjManager

    //Database for every gameobject
    public ObjDatabase objDatabase;
    //Dictionary to track instantiated objects and to destroy
    private Dictionary<string, GameObject> instantiatedObjs;

    //static proprierty for central manager ops
    public static ObjManager objManager;

    void Awake()
    {
        instantiatedObjs = new Dictionary<string, GameObject>();

        if (objManager == null)
        {
            objManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Other classes can call this method to spawn objects from the supplied object database
    /// </summary>
    /// <param name="assetName"></param>
    public GameObject SpawnFromDatabase(string assetName, string instantiatedObjName)
    {
        if(!objDatabase.objz.ContainsKey(assetName))
        {
            return null;
        }

        //instantiate prefab
        GameObject gameObjectz = Instantiate(objDatabase.objz[assetName]);
        gameObjectz.name = instantiatedObjName;

        //added spawned obj to dictionary in order to be destroyed later thanks to ID
        instantiatedObjs.Add(instantiatedObjName, gameObjectz);

        return gameObjectz;
    }
    
    /// <summary>
    /// Accesses the instantiated version of the object (not the prefab) that has the given key
    /// </summary>
    /// <param name="thingz"></param>
    /// <returns>Instantiated version of the game object that is referred to by the given key.</returns>
    public GameObject GetInstantiatedObject(string thingz)
    {
        GameObject ret = null;
        if( instantiatedObjs.ContainsKey(thingz) )
        {
            ret = instantiatedObjs[thingz];
        }

        return ret;
    }

    public bool Destroyz(string objectId)
    {
        if(!instantiatedObjs.ContainsKey(objectId))
        {
            return false;
        }

        //Destroy instantiated obj
       Destroy(instantiatedObjs[objectId]);

        //Remove from instantiated dictionary the voice about the instantiated one
       if (instantiatedObjs.ContainsKey(objectId))
       {
            instantiatedObjs.Remove(objectId);
       }

        return true;
    }

}
