using UnityEngine;
using System.Collections;

namespace TeaSoft
{

    public class RotateableObject : MonoBehaviour
    {
        public RollingAxis rollingType = RollingAxis.RollingArroundByX;
        public bool shouldRotate = false;
        public float rotateSpeed = 100f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (shouldRotate)
            {
                float tempSpeed = rotateSpeed * Time.deltaTime;
                switch (rollingType)
                {
                    case RollingAxis.RollingArroundByX:
                        {
                            this.gameObject.transform.Rotate(this.transform.right, tempSpeed, Space.Self);
                        }
                        break;
                    case RollingAxis.RollingArroundByY:
                        {
                            this.gameObject.transform.Rotate(this.transform.up, tempSpeed, Space.Self);
                        }
                        break;
                    case RollingAxis.RollingArroundByZ:
                        {
                            this.gameObject.transform.Rotate(this.transform.forward, tempSpeed, Space.Self);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public enum RollingAxis
    {
        None,
        RollingArroundByX,
        RollingArroundByY,
        RollingArroundByZ,
    }
}