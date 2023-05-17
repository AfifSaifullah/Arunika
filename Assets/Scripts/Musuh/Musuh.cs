using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Musuh : MonoBehaviour
{
    public float nyawa;
    public float attackVal;

    public abstract void Attacked(float damage);
}
