using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public partial class FormInformation : Form
    {
        private string maSinhVien;
        private string hoTen;

        public FormInformation(ManHinhChinh manHinhChinh, string masv, string hoten)
        {
            InitializeComponent();
            label3.Text = masv;
            label4.Text = hoten;
        }
    }
}
