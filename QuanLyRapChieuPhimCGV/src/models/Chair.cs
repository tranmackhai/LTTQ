using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyRapChieuPhimCGV.src.models
{
    public class Chair
    {
        public string id;
        public int row;
        public int column;
        public ChairType type;
        public Room room;

        public Chair() { }
    }
}
