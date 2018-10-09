using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script are used to create SplatMaps based on the movement of an object with the tracksShader attached this in turn creates the tracks in the snow behind Gustav
/// It require separate objects attached to vasa to create the tracks as far as I researched therefore Gustav can utilize a pair of shoes
/// separate from his model.
/// </summary>
public class DrawTracks : MonoBehaviour {

    // Use this for initialization
    //[SerializeField]
    //private Camera mainCamera; camera shall no longer be a member of this script
    [SerializeField]
    private Shader drawshader;
    private RenderTexture splatMap;
    private Material snowMaterial;
    private Material drawMaterial;
    [SerializeField]
    private Terrain currentLevel;// this is used instead of a meshrenderer to find the material property(ie the snowshader)
    //[SerializeField]
    //private GameObject terrain;
    [SerializeField]
    private GameObject[] feet; // reference the two skiis or feet/shoes on gustav Vasas model.
    private RaycastHit hit; // hit for the raycast later used with this script
    private int layerMask;// this reference the layer we are on and we only want to draw on objects in the ground layer (no threes and water involved)
    [Range(0,500)]
    public float brushSize;// variable for brush strenght this is public only for the sake of being a range manageable from the inspector
    [Range(0,1)]
    public float brushStrength;// same as above.

	void Start ()
    {
        /*mainCamera = GameManager.managerWasa.mainCamera; // get the main camera from*/ //game manager
        layerMask = LayerMask.GetMask("Ground");// get object that is on the mask of the corresponding name
        drawMaterial = new Material(drawshader);
        //drawMaterial.SetVector("_Color", Color.red); we do not need this anymore
        //snowMaterial = terrain.GetComponent<MeshRenderer>().material;
        snowMaterial = currentLevel.materialTemplate;
        splatMap = new RenderTexture(1024, 1024,0,RenderTextureFormat.ARGBFloat);
        snowMaterial.SetTexture("_Splat", splatMap);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //DrawWithMouseDebug();
        MakeSkitracks();
        
	}
    /// <summary>
    /// This function helps drawing the skitracks to the 
    /// </summary>
    void MakeSkitracks()
    {
        // for loop that iterate through the feet array, since Gustav can both ski and walk it may be appropriate to reach the skiis from the 
        // game manager object and swithc between transforms when nessesary.
        for (int i = 0; i < feet.Length; i++)
        {
            if (Physics.Raycast(feet[i].transform.position, -Vector3.up, out hit,1f,layerMask))
            {
                drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));               
                drawMaterial.SetFloat("_Strength", brushStrength);//translate the value for brushstrenght to the corresponding shader field
                drawMaterial.SetFloat("_Size", brushSize);// translate the value for brush size to the corresponding shader field.
                RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }
    
}
