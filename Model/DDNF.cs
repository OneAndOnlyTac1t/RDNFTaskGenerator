using System;
using System.Collections.Generic;

namespace TDNFGenerator.Model
{
    public class DDNF
    {
        public static string GenerateDDNF(int arguments, int dnfs, int maxDnfs)
        {
            var _enum = GetEnum(arguments);
            string[] dnf;
            var resultList = new List<string[]>();
            var result = "";
            Random random = new Random();
            int i = 0;
            do
            {
                var value = random.Next(0, dnfs);
                dnf = _enum[value];
                if (resultList.Contains(dnf))
                {
                    continue;
                }
                else
                {
                    resultList.Add(dnf);
                    i++;
                }
            }
            while (i < dnfs);

            foreach (var value in resultList)
            {
                string parsedArgs = null;
                foreach (string arg in value)
                {
                    parsedArgs += arg + " ";
                }
                result += parsedArgs + "V ";
            }
            result = result.Remove(result.Length - 3, 3);
            return result;
        }
        public static string[][] GetEnum(int arguments)
        {
            switch (arguments)
            {
                case 2:
                    return Enums.Enums.Two;
                case 3:
                    return Enums.Enums.Three;
                case 4:
                    return Enums.Enums.Four;
                default:
                    return null;
            }
        }
    }
}
