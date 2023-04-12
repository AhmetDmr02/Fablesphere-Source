using System.Collections.Generic;
using UnityEngine;
using System;

namespace HelperOfDmr
{
    public class RotationHelper
    {
        public static Quaternion GiveRandomRotation(bool RandomZ)
        {
            if (!RandomZ)
            {
                Quaternion randomQua = Quaternion.Euler(UnityEngine.Random.Range(0, -45), UnityEngine.Random.Range(0, 365), 0);
                return randomQua;
            }
            else
            {
                Quaternion randomQuaZ = Quaternion.Euler(UnityEngine.Random.Range(0, -45), UnityEngine.Random.Range(0, 365), UnityEngine.Random.Range(-50,50));
                return randomQuaZ;
            }
        }
        public static Vector3 GiveVec3WithOne(float X_Y_Z)
        {
            Vector3 returnVec = new Vector3(X_Y_Z, X_Y_Z, X_Y_Z);
            return returnVec;
        }
    }
    public static class UtilitesOfDmr
    {
        private static System.Random rng = new System.Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static SerializationInfoClass CreateDefaultSIC(GameObject GO,MonoBehaviour MB)
        {
            SerializationInfoClass SIC = new SerializationInfoClass();
            SIC.instanceID = GO.name + GO.transform.position.x + GO.transform.position.y + GO.transform.position.z + GO.transform.rotation.x + GO.transform.rotation.y + GO.transform.rotation.z + GO.transform.rotation.w;
            SIC.scriptName = MB.GetType().Name;
            SIC.dontDeleteThis = false;
            SIC.generatedBySomething = false;
            return SIC;
        }
        public static bool RandomChanceForPercentageBool(int chanceToWin)
        {
            int random = UnityEngine.Random.Range(0, 100);
            if (random < chanceToWin)
            {
                return true;
            }else
            {
                return false;
            }
        }
        public static int RandomChanceForPercentageInt(int chanceToWin)
        {
            int random = UnityEngine.Random.Range(0, 100);
            if (random < chanceToWin)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
