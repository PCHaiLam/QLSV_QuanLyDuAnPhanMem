using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace QuanLySinhVien
{
    public partial class ManHinhChinh : Form
    {
        FormInformation openedFormInfo = null;
        public ManHinhChinh()
        {
            InitializeComponent();
            // Gọi phương thức LoadData khi form được tải
            this.Load += ManHinhChinh_Load;
        }
        private void ManHinhChinh_Load(object sender, EventArgs e)
        {
            // Gọi phương thức LoadData để tải dữ liệu vào các ComboBox
            LoadDGV_SinhVien();
            LoadCB_Nganh();
            LoadCB_Lop();
        }
        //---------------------SINHVIEN-------------------------
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu các trường bắt buộc đã được nhập
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên.");
                txtName.Focus();
                return;
            }
            if (dtpBirth.Value == null)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh.");
                dtpBirth.Focus();
                return;
            }
            if (!radioGTNu.Checked && !radioGTNam.Checked)
            {
                MessageBox.Show("Vui lòng chọn giới tính.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email.");
                txtEmail.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.");
                txtPhone.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ.");
                txtAddress.Focus();
                return;
            }
            if (cbNganh.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn ngành.");
                cbNganh.Focus();
                return;
            }
            if (cbLop.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn lớp.");
                cbLop.Focus();
                return;
            }

            // Tạo mã sinh viên
            string namHienTai = DateTime.Now.Year.ToString();
            string namSinhVien = (int.Parse(namHienTai) - 1959).ToString("D2");

            Nganh selectedNganh = (Nganh)cbNganh.SelectedItem;
            string maNganh = int.Parse(selectedNganh.MaNganh).ToString("D2");

            int maSinhVienSo;
            maSinhVienSo = LastIDFromDatabase() + 1;

            string maSinhVien = maSinhVienSo.ToString("D4");

            string maSinhVienFull = namSinhVien + maNganh + maSinhVien;


            // Lấy dữ liệu từ các trường
            string name = txtName.Text;
            DateTime birthDate = dtpBirth.Value;
            string gender = radioGTNu.Checked ? "Nữ" : "Nam";
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string address = txtAddress.Text;
            string maLop = cbLop.SelectedValue.ToString();


            // Câu lệnh SQL để chèn dữ liệu vào bảng SinhVien
            string query = "INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, DiaChi, Email, SoDienThoai, MaLop) " +
                           "VALUES (@MaSV, @HoTen, @NgaySinh, @GioiTinh, @DiaChi, @Email, @SoDienThoai, @MaLop)";

            // Sử dụng SqlConnection và SqlCommand để thực hiện truy vấn
            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm các tham số vào SqlCommand
                    command.Parameters.AddWithValue("@MaSV", maSinhVienFull);
                    command.Parameters.AddWithValue("@HoTen", name);
                    command.Parameters.AddWithValue("@NgaySinh", birthDate);
                    command.Parameters.AddWithValue("@GioiTinh", gender);
                    command.Parameters.AddWithValue("@DiaChi", address);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@SoDienThoai", phone);
                    command.Parameters.AddWithValue("@MaLop", maLop);

                    try
                    {
                        // Mở kết nối và thực hiện truy vấn
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        // Kiểm tra nếu chèn dữ liệu thành công
                        if (result > 0)
                        {
                            MessageBox.Show("Dữ liệu đã được lưu thành công!");
                            LoadDGV_SinhVien();
                        }
                        else
                        {
                            MessageBox.Show("Có lỗi xảy ra trong quá trình lưu dữ liệu. Vui lòng thử lại.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ nếu có lỗi xảy ra
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                    }
                }
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các điều khiển
            string searchID = txtSearchID.Text;
            string searchName = txtSearchName.Text;

            string searchNganh = null;
            if (cbSearchNganh.SelectedIndex != -1 && cbSearchNganh.SelectedItem != null)
            {
                searchNganh = cbSearchNganh.SelectedItem.ToString();
            }

            string searchLop = null;
            if (cbSearchLop.SelectedIndex != -1 && cbSearchLop.SelectedValue != null)
            {
                searchLop = cbSearchLop.SelectedValue.ToString();
            }
            
            // Tạo câu lệnh SQL để tìm kiếm sinh viên
            string query = "SELECT sv.*, l.TenLop FROM SinhVien sv JOIN Lop l ON sv.MaLop = l.MaLop JOIN Nganh n ON l.MaNganh = n.MaNganh WHERE 1=1";
            if (!string.IsNullOrEmpty(searchID))
            {
                query += " AND MaSV = @MaSV";
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                query += " AND sv.HoTen LIKE @HoTen";
            }
            if (!string.IsNullOrEmpty(searchNganh))
            {
                query += " AND n.TenNganh LIKE @TenNganh";
            }
            if (!string.IsNullOrEmpty(searchLop))
            {
                query += " AND l.MaLop LIKE @TenLop";
            }

            // Thực hiện truy vấn và hiển thị kết quả
            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(searchID))
                        {
                            command.Parameters.AddWithValue("@MaSV", searchID);
                        }
                        if (!string.IsNullOrEmpty(searchNganh))
                        {
                            command.Parameters.AddWithValue("@TenNganh", "%" + searchNganh + "%" );
                        }
                        if (!string.IsNullOrEmpty(searchLop))
                        {
                            command.Parameters.AddWithValue("@TenLop", "%" + searchLop + "%");
                        }
                        if (!string.IsNullOrEmpty(searchName))
                        {
                            command.Parameters.AddWithValue("@HoTen", "%" + searchName + "%");
                        }


                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dgvSinhVien.DataSource = dataTable;
                        dgvSinhVien.Columns.Remove("MaLop");

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }

        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Xóa dữ liệu trong các ô tìm kiếm
            txtSearchID.Text = "";
            cbSearchNganh.SelectedIndex = -1;
            cbSearchLop.SelectedIndex = -1;
            txtSearchName.Text = "";
            // Kiểm tra nếu ComboBox cbSearchLop có nguồn dữ liệu
            if (cbSearchLop.DataSource != null)
            {
                // Nếu có, gán nguồn dữ liệu của ComboBox cbSearchLop thành null
                cbSearchLop.DataSource = null;
            }
            else
            {
                // Nếu không có, xóa các mục trực tiếp
                cbSearchLop.Items.Clear();
            }
            // Gọi lại hàm LoadData() để tải lại dữ liệu từ cơ sở dữ liệu và hiển thị nó trên form
            LoadDGV_SinhVien();
            LoadCB_Lop();
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {

        }
        private void LoadDGV_SinhVien()
        {
            // query
            string querySinhVien = "SELECT SinhVien.*, Lop.TenLop FROM SinhVien JOIN Lop ON SinhVien.MaLop = Lop.MaLop";

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(querySinhVien, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dgvSinhVien.DataSource = dataTable;
                        dgvSinhVien.Columns.Remove("MaLop");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private void LoadCB_Nganh()
        {
            // query
            string queryNganh = "SELECT MaNganh, TenNganh FROM Nganh";

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(queryNganh, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        // Duyệt qua các dòng kết quả và thêm vào ComboBox
                        while (reader.Read())
                        {
                            string maNganh = reader["MaNganh"].ToString();
                            string tenNganh = reader["TenNganh"].ToString();
                            cbNganh.Items.Add(new Nganh(maNganh, tenNganh));
                            cbSearchNganh.Items.Add(new Nganh(maNganh, tenNganh));
                        }

                        // Đóng DataReader sau khi sử dụng
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private void LoadCB_Lop()
        {
            // query
            string queryNganh = "SELECT MaLop, TenLop FROM Lop";

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(queryNganh, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        // Duyệt qua các dòng kết quả và thêm vào ComboBox
                        while (reader.Read())
                        {
                            string maLop = reader["MaLop"].ToString();
                            string tenLop = reader["TenLop"].ToString();
                            cbSearchLop.Items.Add(new Lop(maLop, tenLop));
                        }

                        // Đóng DataReader sau khi sử dụng
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private void cbNganh_cbLop(ComboBox comboNganh, ComboBox comboBoxLop)
        {
            // Tạo câu lệnh SQL để lấy các lớp thuộc về ngành đã chọn
            string queryLop = "SELECT MaLop, TenLop FROM Lop WHERE MaNganh = @MaNganh";

            // Kiểm tra nếu đã chọn một ngành
            if (comboNganh.SelectedIndex != -1)
            {
                // Xóa các mục có sẵn trong ComboBox cbLop
                comboBoxLop.DataSource = null;

                // Lấy đối tượng Nganh đã chọn từ ComboBox cbNganh
                Nganh nganhDuocChon = (Nganh)comboNganh.SelectedItem;

                // Kiểm tra nếu đối tượng không null
                if (nganhDuocChon != null)
                {
                    // Lấy MaNganh từ đối tượng Nganh đã chọn
                    string maNganh = nganhDuocChon.MaNganh;

                    // Sử dụng SqlConnection và SqlCommand để thực hiện truy vấn
                    using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(queryLop, connection))
                        {
                            // Thêm tham số cho mã ngành
                            command.Parameters.AddWithValue("@MaNganh", maNganh);

                            // Mở kết nối và thực hiện truy vấn
                            connection.Open();
                            SqlDataAdapter daLop = new SqlDataAdapter(command);
                            DataTable dtLop = new DataTable();
                            daLop.Fill(dtLop);

                            // Gán DataSource cho cbLop
                            comboBoxLop.DataSource = dtLop;
                            comboBoxLop.DisplayMember = "TenLop"; // Hiển thị tên lớp
                            comboBoxLop.ValueMember = "MaLop";     // Sử dụng trường MaLop làm giá trị

                            // Đặt SelectedIndex về -1 để người dùng tự chọn lớp
                            comboBoxLop.SelectedIndex = -1;
                        }
                    }
                }
            }
        }
        private void cbNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbNganh_cbLop(cbNganh, cbLop);
        }
        private void cbSearchNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbNganh_cbLop(cbSearchNganh, cbSearchLop);
        }
        private int LastIDFromDatabase()
        {
            int lastStudentID = 0;

            // Tạo câu lệnh SQL để lấy mã sinh viên cuối cùng từ bảng SinhVien
            string query = "SELECT MAX(CAST(RIGHT(MaSV, 4) AS INT)) AS LastID FROM SinhVien";

            // Sử dụng SqlConnection và SqlCommand để thực hiện truy vấn
            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        // Mở kết nối và thực hiện truy vấn
                        connection.Open();
                        object result = command.ExecuteScalar();

                        // Kiểm tra xem kết quả trả về không
                        if (result != DBNull.Value && result != null)
                        {
                            // Trích xuất mã sinh viên từ kết quả
                            lastStudentID = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ nếu có lỗi xảy ra
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                    }
                }
            }

            return lastStudentID;
        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem người dùng đã click vào một ô dữ liệu (không phải header) chưa
            if (e.RowIndex >= 0)
            {
                // Đóng form cũ nếu có
                if (openedFormInfo != null)
                {
                    openedFormInfo.Close();
                }

                // Lấy dòng đã chọn trong dgv
                DataGridViewRow selectedRow = dgvSinhVien.Rows[e.RowIndex];

                // Lấy thông tin sinh viên từ dòng đã chọn
                string maSinhVien = selectedRow.Cells["MaSV"].Value.ToString();
                string hoTen = selectedRow.Cells["HoTen"].Value.ToString();
                // Lấy thêm các thông tin khác nếu cần

                // Tạo một đối tượng mới của FormInformation và truyền thông tin đã lấy được vào đó
                openedFormInfo = new FormInformation(this,maSinhVien,hoTen);

                // Hiển thị FormInformation
                openedFormInfo.Show();
            }
        }

        //-----------------------Môn Học-----------------------
        //-----------------------ĐIỂM -------------------------
        //-----------------------Học phí -------------------------
    }
}
