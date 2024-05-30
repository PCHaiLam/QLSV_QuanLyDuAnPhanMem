using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien
{
    internal class Nganh
    {
        public string MaNganh { get; set; }
        public string TenNganh { get; set; }
        public Nganh(string maNganh, string tenNganh)
        {
            MaNganh = maNganh;
            TenNganh = tenNganh;
        }

        public override string ToString()
        {
            return TenNganh; // Hiển thị tên nganh trong ComboBox
        }
    }
}
