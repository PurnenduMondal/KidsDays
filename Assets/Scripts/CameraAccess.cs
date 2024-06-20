using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NativeCameraNamespace;
public class CameraAccess : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void GetProfilePic()
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
        {
            if( path != null )
            {
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath( path, 1024 );
                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }
                GameObject.Find("ProfilePic").GetComponent<Image>().sprite =  Sprite.Create(texture, new Rect(0, 0, texture.width, texture.width), new Vector2(0.5f, 0.5f));
                GameObject.Find("ProfilePic").GetComponent<Image>().preserveAspect = true;
            }
        }, 1024,true, NativeCamera.PreferredCamera.Front);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
