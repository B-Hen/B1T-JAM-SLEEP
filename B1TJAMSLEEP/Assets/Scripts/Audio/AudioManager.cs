using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManageInstance;

    private void Awake()
    {
        if(audioManageInstance != null && audioManageInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        audioManageInstance = this;
        DontDestroyOnLoad(this);
    }
}
