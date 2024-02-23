// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("mCMlSIm+Dxe0I6cuyBIrGJ179dABxwKE5gXfkTMuMhhuZvsDLggCP4RZ1MiW2tNWO2pv1a8pOltoU9AxvD8xPg68PzQ8vD8/PpEekhSqsOp+2lcsptPRzZFDfZ8NHT+Mja7EKt5ObRMWFH2Vj7lrbhvq1hrRt4PxAyngaHcDE8GL5LF4rrIVmIJBa+3eUIbbr2sp3VXJ0kvPzmVW5AMI3Z3WncaZ4/RcGrlrnPZGjh4vrKmUqk8+ZxTXQz10VgtfozMfHp81sJ4Dq7bK9O4DgaQsndrvdmPZR73T+g68PxwOMzg3FLh2uMkzPz8/Oz49yoz4OJHBje83BSJ7tRAA7k+a+ciUI9sCf5CR3/01oLmbzPenj0cG351+K5H7YOnsvTw9Pz4/");
        private static int[] order = new int[] { 9,13,3,13,13,10,11,8,12,13,12,13,12,13,14 };
        private static int key = 62;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
