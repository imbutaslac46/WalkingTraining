using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Receivers;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.InputModule.Utilities.Interactions;


public class PanelButtonReceiver : InteractionReceiver
{
    ///Add object to interact with on the side panel in unity and then add to switch-case
    
    private float time;

    //used for state machine behaviour of 2handmanipulation
    public bool holding;
    public TwoHandManipulatable twoHandz;
    

    void Start()
    {
        time = Time.time;
        holding = false;
        twoHandz = (TwoHandManipulatable)GetComponent<TwoHandManipulatable>();
    }

    private void Update()
    { 
       
    }

    protected override void FocusEnter(GameObject obj, PointerSpecificEventData eventData)
    {
        holding = false;
        twoHandz.enabled = true;
    }

    protected override void FocusExit(GameObject obj, PointerSpecificEventData eventData)
    {
        if (!holding)
        {
            twoHandz.enabled = false;
        }
    }

    protected override void InputDown(GameObject obj, InputEventData eventData)
    {

        holding = true;

        //for not giving more than 1 input on buttons
        if (Time.time >= time + 0.1f)
        {
            time = Time.time;

            switch (obj.name)
            {

                case "CloseButton":
                    // Do something when button is pressed
                    Destroy(gameObject);
                    break;

                default:
                    break;
            }

        }
        
    }

    protected override void InputUp(GameObject obj, InputEventData eventData)
    {
      
        holding = false;
        twoHandz.enabled = false;

    }

}
