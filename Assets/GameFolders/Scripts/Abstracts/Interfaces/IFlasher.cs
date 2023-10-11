using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IFlasher 
{
     public Task StartFlashing();
    void SetMaterial();
}
