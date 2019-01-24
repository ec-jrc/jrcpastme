/*
* JRC-PaStMe © European Union, 2018
* 
* Licensed under the EUPL, Version 1.2 or – as soon they
will be approved by the European Commission - subsequent
versions of the EUPL (the "Licence");
* You may not use this work except in compliance with the Licence.
* You may obtain a copy of the Licence at:
* 
* https://joinup.ec.europa.eu/software/page/eupl
* 
* Unless required by applicable law or agreed to in
writing, software distributed under the Licence is
distributed on an "AS IS" basis,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
either express or implied.
* See the Licence for the specific language governing
permissions and limitations under the Licence.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace JRC_PastMe
{

    public class SMtrx1D
    {
        protected Dictionary<long, double> M;

        public SMtrx1D()
        {
            M = new Dictionary<long, double>();
        }

        public long count()
        {
            return M.Count;
        }
        

        public double get(int x, int y)
        {
            long idx = x * 1000 + y; // Asumes that y < 1000
            double v = 0;
            if (M.TryGetValue(idx, out v)) return v;
            return 0;
        }

        public void set(int x, int y, double v)
        {
            long idx = x * 1000 + y; // Asumes that y < 1000
            M[idx] = v;
        }

        public void inc(int x, int y)
        {
            long idx = x * 1000 + y; // Asumes that y < 1000
            double current_value = 0;
            M.TryGetValue(idx, out current_value); // we get the current value 0 if it does not exist
            M[idx]=current_value +1;
        }

        public void save (string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileInfo fi = new FileInfo(path);
            using (FileStream f = fi.Create())
            {
                bf.Serialize(f, M);
                f.Flush();
            }
        }

        public void save2(string path)
        {
            BinaryWriter bw;
            FileInfo fi = new FileInfo(path);
            using (FileStream f = fi.Create())
            {
                bw = new BinaryWriter(f);
                long count = M.Count;
                bw.Write(count);
                foreach (long key in M.Keys)
                {
                    bw.Write(key);
                    bw.Write(M[key]);
                }
            }
        }

        public void load(string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileInfo fi = new FileInfo(path);
            using (FileStream f = fi.OpenRead())
            {
                M = (Dictionary<long, double>)bf.Deserialize(f);
            }
        }

        public void load2(string path)
        {
            BinaryReader br;
            FileInfo fi = new FileInfo(path);
            using (FileStream f = fi.OpenRead())
            {
                br = new BinaryReader(f);
                long count = br.ReadInt64();
                for(long i = 0; i<count; i++)
                {
                    long rkey = br.ReadInt64();
                    double rval = br.ReadDouble();
                    M.Add(rkey, rval);
                }
            }
        }


    }
}
