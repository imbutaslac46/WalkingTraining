using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;


[CreateAssetMenu(fileName = "ObjDatabase.asset", menuName = "Object Database")]
public class ObjDatabase : ScriptableObject
{
    //Serializable Dictionary to store Prefabs to spawn
    //Right click on the script to create the .asset file
    //Drag 'n Drop Prefab to the .asset and assign an ID

    [SerializeField]
    public Objz objz;



    //[SerializeField]
    //public Objz _modelz;


    [System.Serializable]
    public class Objz : SerializableDictionaryBase<string, GameObject>
    {


    }
    


}


