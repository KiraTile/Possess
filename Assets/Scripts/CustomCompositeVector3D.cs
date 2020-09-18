using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Layouts;

#if UNITY_EDITOR
public class CustomCompositeVector3D : InputParameterEditor<Custom3DVectorComposite>
{
    public override void OnGUI()
    {
        //EditorGUILayout.LabelField("3D Vector Composite");
        //target.floatParameter = EditorGUILayout.FloatField("Some Parameter", target.floatParameter);
        target.modeParameter = (Custom3DVectorComposite.Mode)EditorGUILayout.EnumPopup("Mode", target.modeParameter);
    }
}
#endif


// Use InputBindingComposite<TValue> as a base class for a composite that returns
// values of type TValue.
// NOTE: It is possible to define a composite that returns different kinds of values
//       but doing so requires deriving directly from InputBindingComposite.
#if UNITY_EDITOR
[InitializeOnLoad] // Automatically register in editor.
#endif
// Determine how GetBindingDisplayString() formats the composite by applying
// the  DisplayStringFormat attribute.
[DisplayStringFormat("{firstPart}+{secondPart}")]
public class Custom3DVectorComposite : InputBindingComposite<Vector3>
{
    // Each part binding is represented as a field of type int and annotated with
    // InputControlAttribute. Setting "layout" restricts the controls that
    // are made available for picking in the UI.
    //
    // On creation, the int value is set to an integer identifier for the binding
    // part. This identifier can read values from InputBindingCompositeContext.
    // See ReadValue() below.
    [InputControl(layout = "Value")]
    public int left;

    [InputControl(layout = "Value")]
    public int right;

    [InputControl(layout = "Value")]
    public int down;

    [InputControl(layout = "Value")]
    public int up;

    [InputControl(layout = "Value")]
    public int backward;

    [InputControl(layout = "Value")]
    public int forward;


    // Any public field that is not annotated with InputControlAttribute is considered
    // a parameter of the composite. This can be set graphically in the UI and also
    // in the data (e.g. "custom(floatParameter=2.0)").

    public enum Mode
    {
        Analog = 0,             // As-is
        DigitalNormalized = 1,  // Normalized
        Digital = 2             // Non-Normalized 1, 0, -1 values
    }
    public Mode modeParameter = Mode.DigitalNormalized;
    //public float floatParameter;
    //public bool boolParameter;

    // This method computes the resulting input value of the composite based
    // on the input from its part bindings.
    public override Vector3 ReadValue(ref InputBindingCompositeContext context)
    {
        var leftValue = context.ReadValue<float>(left);
        var rightValue = context.ReadValue<float>(right);
        var downValue = context.ReadValue<float>(down);
        var upValue = context.ReadValue<float>(up);
        var backwardValue = context.ReadValue<float>(backward);
        var forwardValue = context.ReadValue<float>(forward);

        Vector3 returnVector = new Vector3(rightValue - leftValue, upValue - downValue, forwardValue - backwardValue);

        switch (modeParameter)
        {
            case Mode.Analog:
                break;
            case Mode.DigitalNormalized:
                returnVector.Normalize();
                break;
            case Mode.Digital:
                returnVector = MakeDigital(returnVector);
                break;
            default:
                Debug.LogError($"Custom3DVectorComposite.EvaluateMagnitude: Unrecognized Mode[{modeParameter}]");
                throw new ArgumentOutOfRangeException($"Custom3DVectorComposite.EvaluateMagnitude: Unrecognized Mode[{modeParameter}]");
        }

        //... do some processing and return value
        return returnVector;
    }

    // This method computes the current actuation of the binding as a whole.
    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        // Compute normalized [0..1] magnitude value for current actuation level.

        return -1;
    }

    Vector3 MakeDigital(Vector3 vector)
    {
        var returnVector = Vector3.zero;
        if (vector.x > 0)
            returnVector.x = 1;
        else if (vector.x < 0)
            returnVector.x = -1;
        else
            returnVector.x = 0;

        if (vector.y > 0)
            returnVector.y = 1;
        else if (vector.y < 0)
            returnVector.y = -1;
        else
            returnVector.y = 0;

        if (vector.z > 0)
            returnVector.z = 1;
        else if (vector.z < 0)
            returnVector.z = -1;
        else
            returnVector.z = 0;

        return returnVector;
    }

    static Custom3DVectorComposite()
    {
        // Can give custom name or use default (type name with "Composite" clipped off).
        // Same composite can be registered multiple times with different names to introduce
        // aliases.
        //
        // NOTE: Registering from the static constructor using InitializeOnLoad and
        //       RuntimeInitializeOnLoadMethod is only one way. You can register the
        //       composite from wherever it works best for you. Note, however, that
        //       the registration has to take place before the composite is first used
        //       in a binding. Also, for the composite to show in the editor, it has
        //       to be registered from code that runs in edit mode.
        InputSystem.RegisterBindingComposite<Custom3DVectorComposite>();
    }

    [RuntimeInitializeOnLoadMethod]
    static void Init() { } // Trigger static constructor.
}
