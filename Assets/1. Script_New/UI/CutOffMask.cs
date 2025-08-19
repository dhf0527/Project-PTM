using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutOffMask : Image
{
    // Inspector에 무조건 노출되게 public으로 선언
    public List<Graphic> holeGraphics = new List<Graphic>();

    [SerializeField] private bool useAlphaOnHole = false;

    public override Material materialForRendering
    {
        get
        {
            var m = new Material(base.materialForRendering);
            m.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return m;
        }
    }
}
