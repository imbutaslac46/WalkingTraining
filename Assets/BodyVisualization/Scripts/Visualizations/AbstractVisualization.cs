using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Not sure if this should be an abstract class or not...
// But at the very least, we need a common class for all visualizations
public abstract class AbstractVisualization : MonoBehaviour
{
    // Since we won't know what each kind of visualization will have as parameters(variables),
    // let's provide a way to modify parameters(variables) of the visualization using a string-object pair.
    // The way of interpreting this string-object pair is up to the visualization itself.
    //
    // For example, for the BarGraph3D, we can associate the propertyName "value" to the variable "value" in BarGraph3D.
    // Behind the scenes, BarGraph3D just checks if the property name is "value", and then assigns the actual value to the variable "value".
    // For outside classes, however, we just say something like barGraph.UpdateProperty( "value", 2.0 ) or something
    // One caveat: we need to know the property names provided by the visualization beforehand.
    public abstract void UpdateProperty(string propertyName, object value);
}
