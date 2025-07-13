using System.Collections;
using UnityEngine;

namespace HighVoltage.Infrastructure
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
