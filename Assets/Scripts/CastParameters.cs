using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastParameters : MonoBehaviour
{
    public static CastParameters singleton;

    public CastParameters()
    {
        singleton = this;
    }



    public static object CastParameter(string parameter)
    {
        object result;

        bool parsed;

        int intParsed;
        parsed = int.TryParse(parameter, out intParsed);
        if(parsed)
        {
            result = intParsed;
            return result;
        }
        float floatParsed;
        parsed = float.TryParse(parameter, out floatParsed);
        if(parsed)
        {
            result = floatParsed;
            return result;
        }
        bool boolParsed;
        parsed = bool.TryParse(parameter, out boolParsed);
        if(parsed)
        {
            result = boolParsed;
            return result;
        }


        result = parameter;


        return result;
    }
}
