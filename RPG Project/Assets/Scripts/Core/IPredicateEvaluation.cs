using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public interface IPredicateEvaluation
    {
        bool? Evaluate(string predicate, string[] parameters);
    }
}
