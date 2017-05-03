using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catopus
{
    public class Orbits : MonoBehaviour
    {

        // When added to an object, draws colored rays from the
        // transform position.
        #region Graphic parameters
        public float[] rads;

        [SerializeField]
        float averageLineLength = 0.1f,
            averageBetweenLineLength = 0.2f;

        static Material lineMaterial;
        #endregion
        SphereCollider myCollider;
        static void CreateLineMaterial()
        {
            if (!lineMaterial)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                lineMaterial.SetInt("_ZWrite", 0);
            }
        }

        void Awake()
        {
            SetColliderRadius();
        }

        void SetColliderRadius()
        {
            myCollider = GetComponent<SphereCollider>();
            if (!myCollider)
                return;
            //get max orbit radius
            if (rads == null || rads.Length == 0)
                return;
            float rMax = rads[0];
            for (int i = 0; i < rads.Length; i++)
                if (rMax < rads[i])
                    rMax = rads[i];
            myCollider.radius = rMax;

            var p = GetComponent<Planet>();
            if (p)
                p.SetRadius(rMax);
        }

        // Will be called after all regular rendering is done
        public void OnRenderObject()
        {
            if (rads == null && rads.Length == 0)
                return;

            CreateLineMaterial();
            // Apply the line material
            lineMaterial.SetPass(0);

            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            GL.MultMatrix(transform.localToWorldMatrix);

            // Draw lines
            GL.Begin(GL.LINES);
            GL.Color(Color.white);

            foreach (var r in rads)
                DrawCircle(r);

            GL.End();
            GL.PopMatrix();
        }


        void DrawCircle(float radius)
        {
            float cl = 2 * Mathf.PI * radius;
            float sum = averageBetweenLineLength + averageLineLength;
            float proportion = averageLineLength / sum;

            int count = (int)(cl / sum);

            float circlePiece = 1.0f / count;
            float circleLinePiece = circlePiece * proportion;
            float angleStep = circlePiece * 2 * Mathf.PI;
            float angleLineStep = circleLinePiece * 2 * Mathf.PI;

            for (int i = 0; i < count; ++i)
            {
                //float a = i * circlePiece;
                float angleStart = i * angleStep;
                float angleEnd = angleStart + angleLineStep;


                // Vertex colors change from red to green
                //GL.Color (new Color (a, 1-a, 0, 0.8F));
                // One vertex at transform position
                GL.Vertex3(Mathf.Cos(angleStart) * radius, Mathf.Sin(angleStart) * radius, 0);
                // Another vertex at edge of circle
                GL.Vertex3(Mathf.Cos(angleEnd) * radius, Mathf.Sin(angleEnd) * radius, 0);


            }
        }
    }
}