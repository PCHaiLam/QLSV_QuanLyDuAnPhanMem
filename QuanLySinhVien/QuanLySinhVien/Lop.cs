using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien
{
    internal class Lop
    {
        public string MaLop {  get; set; }
        public string TenLop { get; set;}
        public Lop(string maLop, string tenLop)
        {
            MaLop = maLop;
            TenLop = tenLop;
        }

        public override string ToString()
        {
            return TenLop; // Hiển thị tên nganh trong ComboBox
        }
    }
}
