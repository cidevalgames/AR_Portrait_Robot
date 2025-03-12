using UnityEngine;
using Vuforia;

public class AdjustImageTargetSize : MonoBehaviour
{
    private void Start()
    {
        ImageTargetBehaviour imageTarget = GetComponent<ImageTargetBehaviour>();

        if (imageTarget != null)
        {
            Vector2 targetSize = imageTarget.GetSize();
            Debug.Log($"Taille correcte de l’image : {targetSize.x} m x {targetSize.y} m");

            // Met à jour la scale
            transform.localScale = new Vector3(targetSize.x, targetSize.y, 1);
        }
        else
        {
            Debug.LogError("L’Image Target n’a pas été trouvée !");
        }
    }

    void Update()
    {
        // Vérification en continu pour voir si le scale est bien mis à jour
        Debug.Log($"Scale Actuel : {transform.localScale}");
    }
}
