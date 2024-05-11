using UnityEngine;

public class Bilboarding : MonoBehaviour
{
    Vector3 gameLocalization = new Vector3(0f, 0f, 0f);
    public bool isplanet = false;

    private void Start()
    {
        Vector3 lookVector = gameLocalization - transform.position;

        transform.rotation = Quaternion.LookRotation(lookVector);

        if (isplanet )
        {
            transform.rotation.eulerAngles.Set(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -50f);
        }
    }

}
