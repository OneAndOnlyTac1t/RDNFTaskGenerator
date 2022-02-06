namespace TDNFGenerator.Enums
{
    public class Enums
    {
        public static string[][] Two = GenerateTwo();
        public static string[][] Three = GenerateThree();
        public static string[][] Four = GenerateFour();
        private static string[][] GenerateTwo()
        {
            string[][] two = new string[4][];
            two[0] = new string[] { "!x", "!y" };
            two[1] = new string[] { "!x", "y" };
            two[2] = new string[] { "x", "!y" };
            two[3] = new string[] { "x", "y" };
            return two;
        }

        private static string[][] GenerateThree()
        {
            string[][] three = new string[8][];
            three[0] = new string[] { "!x", "!y", "!z" };
            three[1] = new string[] { "!x", "!y", "z" };
            three[2] = new string[] { "!x", "y", "!z" };
            three[3] = new string[] { "!x", "y", "z" };
            three[4] = new string[] { "x", "!y", "!z" };
            three[5] = new string[] { "x", "!y", "z" };
            three[6] = new string[] { "x", "y", "!z" };
            three[7] = new string[] { "x", "y", "z" };
            return three;
        }
        private static string[][] GenerateFour()
        {
            string[][] four = new string[16][];
            four[0] = new string[] { "!t", "!x", "!y", "!z" };
            four[1] = new string[] { "!t", "!x", "!y", "z" };
            four[2] = new string[] { "!t", "!x", "y", "!z" };
            four[3] = new string[] { "!t", "!x", "y", "z" };
            four[4] = new string[] { "!t", "x", "!y", "!z" };
            four[5] = new string[] { "!t", "x", "!y", "z" };
            four[6] = new string[] { "!t", "x", "y", "!z" };
            four[7] = new string[] { "!t", "x", "y", "z" };
            four[8] = new string[] { "t", "!x", "!y", "!z" };
            four[9] = new string[] { "t", "!x", "!y", "z" };
            four[10] = new string[] { "t", "!x", "y", "!z" };
            four[11] = new string[] { "t", "!x", "y", "z" };
            four[12] = new string[] { "t", "x", "!y", "!z" };
            four[13] = new string[] { "t", "x", "!y", "z" };
            four[14] = new string[] { "t", "x", "y", "!z" };
            four[15] = new string[] { "t", "x", "y", "z" };
            return four;
        }       
    }
}
