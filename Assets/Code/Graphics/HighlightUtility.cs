using UnityEngine;

public class HighlightUtility
{
    private static Material sDefaultHighlightMaterial;
    private static Color sDefaultHighlightColor = Color.green;
    private static Matrix4x4 sReusableMatrix;
    private static Vector3 sReusablePosition = Vector3.zero;
    private static Quaternion sReusableRotation = Quaternion.identity;
    private static Vector3 sReusableScale = Vector3.zero;
    private const string kHighlightShaderName = "Outlined/Silhouette Only";
    private const float kOutlineWidth = 0.05f;

    public static Material DefaultHighlightMaterial
    {
        get
        {
            if (sDefaultHighlightMaterial == null)
            {
                sDefaultHighlightMaterial = new Material(Shader.Find(kHighlightShaderName));
                sDefaultHighlightMaterial.SetColor("_OutlineColor", sDefaultHighlightColor);
                sDefaultHighlightMaterial.SetFloat("_Outline", kOutlineWidth);
            }

            return sDefaultHighlightMaterial;
        }
    }

    public static void HighlightAtPosition(GameObject obj, Material highlightMaterial, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();

        if (meshFilter != null && meshFilter.mesh != null)
        {
            HighlightAtPosition(meshFilter.mesh, highlightMaterial, position, rotation, scale);
        }
    }

    public static void HighlightAtPosition(Mesh mesh, Material higlightMaterial, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);
        Graphics.DrawMesh(mesh, matrix, higlightMaterial, 1);
    }

    public static void HighlightAtPosition(Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        HighlightAtPosition(mesh, DefaultHighlightMaterial, position, rotation, scale);
    }

    public static void HighlightObject(GameObject obj)
    {
        sReusablePosition = obj.transform.position;
        sReusableRotation = obj.transform.rotation;
        sReusableScale = obj.transform.lossyScale;

        HighlightAtPosition(obj, DefaultHighlightMaterial, sReusablePosition, sReusableRotation, sReusableScale);
    }

    public static void HighlightObject(GameObject obj, Vector3 positionOverride)
    {
        sReusablePosition = positionOverride;
        sReusableRotation = obj.transform.rotation;
        sReusableScale = obj.transform.lossyScale;

        HighlightAtPosition(obj, DefaultHighlightMaterial, sReusablePosition, sReusableRotation, sReusableScale);
    }

    public static void HighlightObject(GameObject obj, Vector3 positionOverride, Material materialOverride)
    {
        sReusablePosition = positionOverride;
        sReusableRotation = obj.transform.rotation;
        sReusableScale = obj.transform.lossyScale;

        HighlightAtPosition(obj, materialOverride, sReusablePosition, sReusableRotation, sReusableScale);
    }
}
