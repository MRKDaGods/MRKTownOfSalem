/*
 * Copyright (c) 2020, Mohamed Ammar <mamar452@gmail.com>
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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
