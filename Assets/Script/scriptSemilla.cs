using UnityEngine;

public class scriptSemilla : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        scriptEcobotController robot = collision.GetComponent<scriptEcobotController>();
        if (robot != null)
        {
            robot.RecogerSemilla();
            Destroy(gameObject);
        }
    }
}