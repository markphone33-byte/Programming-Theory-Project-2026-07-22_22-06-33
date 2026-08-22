using UnityEngine;

public class FogOfWarController : MonoBehaviour
{
    [SerializeField] private Renderer[] fogObjects;
    private float collidersPlayerIsIn;

    void Awake()
    {
        fogObjects = GetComponentsInChildren<Renderer>();
    }

    public void FogDisappear()
    {
        foreach (Renderer fogObject in fogObjects)
        {
            fogObject.material.color = Color.clear;
        }
    }

    public void PlayerExitsCollider()
    {
        collidersPlayerIsIn--;
        if (collidersPlayerIsIn <= 0)
        {
            foreach (Renderer fogObject in fogObjects)
            {
                fogObject.material.color = Color.black;
            }
            collidersPlayerIsIn = 0;
        }
    }

    public void PlayerEntersCollider()
    {
        collidersPlayerIsIn++;
    }
}
