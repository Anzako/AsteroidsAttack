using UnityEngine;

public class Bilboarding : MonoBehaviour
{
    Vector3 gameLocalization = new Vector3(0f, 0f, 0f);

    private void Start()
    {
        Vector3 lookVector = gameLocalization - transform.position;

        transform.rotation = Quaternion.LookRotation(lookVector);
    }

}
