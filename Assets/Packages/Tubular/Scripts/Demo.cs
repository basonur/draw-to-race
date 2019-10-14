using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curve;

namespace Tubular {

	[RequireComponent (typeof(CurveTester), typeof(MeshFilter), typeof(MeshRenderer))]
	public class Demo : MonoBehaviour {

		[SerializeField] protected int tubularSegments = 20;
		[SerializeField] protected float radius = 0.1f;
		[SerializeField] protected int radialSegments = 6;
		[SerializeField] protected bool closed = false;
        
        private void Awake()
        {
            curveTester = GetComponent<CurveTester>();    
        }

        private CurveTester curveTester;

        public Mesh GenerateMesh(List<Transform> points)
        {
            GetComponent<CurveTester>().Points.Clear();

            for (int i = 0; i < points.Count; i++)
            {
                curveTester.Points.Add(points[i].position);
            }

            var tester = GetComponent<CurveTester>();
            var curve = tester.Build();

            return Tubular.Build(curve, tubularSegments, radius, radialSegments, closed);
        }
       
        
	}
}

