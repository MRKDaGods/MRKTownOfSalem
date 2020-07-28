using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MRK {
    public enum MRKToSNameGenType {
        Stock
    }

    public class MRKToSNameGen {
        static readonly string[] ms_StockNames;

        static MRKToSNameGen() {
            ms_StockNames = new string[] {
                "Giles Corey",
                "Jonathan Corwin",
                "Deodat Lawson",
                "Betty Parris",
                "Martha Corey",
                "Abigail Hobbs",
                "Sarah Bishop",
                "John Hathorne"
            };
        }

        public static string[] GenerateNames(int count, MRKToSNameGenType type) {
            string[] mainArr = null;
            if (type == MRKToSNameGenType.Stock) {
                mainArr = new string[count];
                Array.Copy(ms_StockNames, mainArr, count);
            }

            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = count;

            while (n > 1) {
                byte[] box = new byte[1];

                do provider.GetBytes(box);
                while (!(box[0] < n * (byte.MaxValue / n)));

                int k = (box[0] % n);
                n--;

                string value = mainArr[k];
                mainArr[k] = mainArr[n];
                mainArr[n] = value;
            }

            return mainArr;
        }
    }
}
