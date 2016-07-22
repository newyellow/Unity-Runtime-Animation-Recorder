using UnityEngine;
using System.Collections;

public class UnityObjectAnimation {

	public UnityCurveContainer[] curves;
	public Transform observeGameObject;
	public string pathName = "";

	public UnityObjectAnimation( string hierarchyPath, Transform observeObj ) {
		pathName = hierarchyPath;
		observeGameObject = observeObj;

		curves = new UnityCurveContainer[10];

		curves [0] = new UnityCurveContainer( "localPosition.x" );
		curves [1] = new UnityCurveContainer( "localPosition.y" );
		curves [2] = new UnityCurveContainer( "localPosition.z" );

		curves [3] = new UnityCurveContainer( "localRotation.x" );
		curves [4] = new UnityCurveContainer( "localRotation.y" );
		curves [5] = new UnityCurveContainer( "localRotation.z" );
		curves [6] = new UnityCurveContainer( "localRotation.w" );


		curves [7] = new UnityCurveContainer( "localScale.x" );
		curves [8] = new UnityCurveContainer( "localScale.y" );
		curves [9] = new UnityCurveContainer( "localScale.z" );
	}

	public void AddFrame ( float time ) {

		curves [0].AddValue (time, observeGameObject.localPosition.x);
		curves [1].AddValue (time, observeGameObject.localPosition.y);
		curves [2].AddValue (time, observeGameObject.localPosition.z);

		curves [3].AddValue (time, observeGameObject.localRotation.x);
		curves [4].AddValue (time, observeGameObject.localRotation.y);
		curves [5].AddValue (time, observeGameObject.localRotation.z);
		curves [6].AddValue (time, observeGameObject.localRotation.w);

		curves [7].AddValue (time, observeGameObject.localScale.x);
		curves [8].AddValue (time, observeGameObject.localScale.y);
		curves [9].AddValue (time, observeGameObject.localScale.z);

	}
    public void smoothCurves()
    {
        for (int i = 0; i < curves.Length; i++)
        {
            curves[i].SmoothTangents();
        }
    }
}
