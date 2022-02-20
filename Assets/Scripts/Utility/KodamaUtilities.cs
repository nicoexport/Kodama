using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public static class KodamaUtilities
{
    public static IEnumerator ActionAfterDelay(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }
}