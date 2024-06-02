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
        private bool isInitializing = true;
        private bool isEditing = false;

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
            LoadDGV_MonHoc();
            LoadCB_Nganh();
            LoadCB_Lop();
            LoadCB_MaMonHoc();
            LoadDGV_LopHocPhan();
            LoadCB_GiaoVien();
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

                        dataTable.Columns["MaSV"].ColumnName = "Mã Sinh viên";
                        dataTable.Columns["HoTen"].ColumnName = "Họ và tên";
                        dataTable.Columns["NgaySinh"].ColumnName = "Ngày sinh";
                        dataTable.Columns["GioiTinh"].ColumnName = "Giới tính";
                        dataTable.Columns["Email"].ColumnName = "Email";
                        dataTable.Columns["DiaChi"].ColumnName = "Địa chỉ";
                        dataTable.Columns["SDT"].ColumnName = "Số điện thoại";
                        dataTable.Columns["TenLop"].ColumnName = "Lớp";

                        // Thêm cột số thứ tự
                        dataTable.Columns.Add("STT", typeof(int));
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            dataTable.Rows[i]["STT"] = i + 1;
                        }

                        dgvSinhVien.Columns["STT"].DisplayIndex = 0;
                        dgvSinhVien.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        dgvSinhVien.Columns["Lớp"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvSinhVien.Columns["Họ và tên"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvSinhVien.Columns["Giới tính"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        dgvSinhVien.Columns["Số điện thoại"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

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
                string maSinhVien = selectedRow.Cells["Mã Sinh Viên"].Value.ToString();
                // Lấy thêm các thông tin khác nếu cần

                // Tạo một đối tượng mới của FormInformation và truyền thông tin đã lấy được vào đó
                openedFormInfo = new FormInformation(this,maSinhVien);

                // Hiển thị FormInformation
                openedFormInfo.Show();
            }
        }

        //-----------------------Môn Học-----------------------
        private void btnAddNewMH_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu các trường bắt buộc đã được nhập
            if (string.IsNullOrWhiteSpace(txtTenHocPhan.Text))
            {
                MessageBox.Show("Vui lòng nhập tên môn học.");
                txtTenHocPhan.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSoTinChi.Text))
            {
                MessageBox.Show("Vui lòng nhập số tín chỉ.");
                txtSoTinChi.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDonGiaHocPhi.Text))
            {
                MessageBox.Show("Vui lòng nhập đơn giá tín chỉ.");
                txtDonGiaHocPhi.Focus();
                return;
            }

            // Lấy dữ liệu từ các trường
            string tenHocPhan = txtTenHocPhan.Text;
            string soTinChi = txtSoTinChi.Text;
            string donGia = txtDonGiaHocPhi.Text;
            string maHP = MaHPDuyNhat(txtMaHocPhan.Text);

            // Chèn dữ liệu vào bảng MonHoc
            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                string query = "INSERT INTO MonHoc (MaHP, TenHP, SoTinChi, DonGia) VALUES (@MaHP, @TenHP, @SoTinChi, @DonGia)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaHP", maHP);
                    command.Parameters.AddWithValue("@TenHP", tenHocPhan);
                    command.Parameters.AddWithValue("@SoTinChi", soTinChi);
                    command.Parameters.AddWithValue("@DonGia", donGia);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Thêm môn học thành công!");
                        }
                        else
                        {
                            MessageBox.Show("Thêm môn học thất bại!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
            LoadDGV_MonHoc();
            txtMaHocPhan.Text = " ";
            txtTenHocPhan.Text = " ";
            txtSoTinChi.Text = " ";
        }
        private void btnEditMH_Click(object sender, EventArgs e)
        {
                // Lấy dữ liệu từ ô được chọn
                int selectedRowIndex = dgvMonHoc.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dgvMonHoc.Rows[selectedRowIndex];

                string maHP = txtMaHocPhan.Text;
                string tenHP = txtTenHocPhan.Text;
                int soTinChi;

                if (!int.TryParse(txtSoTinChi.Text, out soTinChi))
                {
                    MessageBox.Show("Số tín chỉ phải là số nguyên.");
                    txtSoTinChi.Focus();
                    return;
                }

                // Tạo câu lệnh SQL để cập nhật dữ liệu
                string query = "UPDATE MonHoc SET TenHP = @TenHP, SoTinChi = @SoTinChi WHERE MaHP = @MaHP";

                using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
                {
                    try
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Thêm các tham số vào câu lệnh SQL
                            command.Parameters.AddWithValue("@MaHP", maHP);
                            command.Parameters.AddWithValue("@TenHP", tenHP);
                            command.Parameters.AddWithValue("@SoTinChi", soTinChi);

                            // Thực thi câu lệnh SQL
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Cập nhật môn học thành công.");
                                LoadDGV_MonHoc(); // Tải lại dữ liệu lên DataGridView
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy môn học để cập nhật.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
        }
        private void btnDeleteMH_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có hàng nào được chọn hay không
            if (dgvMonHoc.SelectedRows.Count > 0)
            {
                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow selectedRow = dgvMonHoc.SelectedRows[0];
                string maHP = selectedRow.Cells["MaHP"].Value.ToString();

                // Xác nhận việc xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa môn học này?", "Xác nhận xóa", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    // Tạo câu lệnh SQL để xóa dữ liệu
                    string query = "DELETE FROM MonHoc WHERE MaHP = @MaHP";

                    using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
                    {
                        try
                        {
                            connection.Open();
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                // Thêm tham số vào câu lệnh SQL
                                command.Parameters.AddWithValue("@MaHP", maHP);

                                // Thực thi câu lệnh SQL
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Xóa môn học thành công.");
                                    LoadDGV_MonHoc(); // Tải lại dữ liệu lên DataGridView
                                }
                                else
                                {
                                    MessageBox.Show("Không tìm thấy môn học để xóa.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một môn học để xóa.");
            }
        }
        private bool KiemTraMaHocPhan(string maHP)
        {
            bool exist = false;

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                string query = "SELECT COUNT(*) FROM MonHoc WHERE MaHP = @MaHP";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaHP", maHP);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    exist = count > 0;
                }
            }

            return exist;
        }
        private string MaHPDuyNhat(string maHP)
        {
            string unique = maHP;
            int count = 1;
            while(KiemTraMaHocPhan(unique))
            {
                unique = maHP + "-" + count;
                count++;
            }
            txtMaHocPhan.Text = unique;
            return unique;
        }
        private void txtTenHocPhan_TextChanged(object sender, EventArgs e)
        {
            // Lấy nội dung của txtTenHocPhan
            string tenHocPhan = txtTenHocPhan.Text;

            // Tách các từ trong chuỗi tên học phần
            string[] words = tenHocPhan.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Lấy chữ cái đầu tiên của mỗi từ và chuyển thành chữ hoa
            string maHP = string.Concat(words.Select(word => char.ToUpper(word[0])));

            // Hiển thị mã học phần trong txtMaHP
            txtMaHocPhan.Text = maHP;
        }
        private void LoadDGV_MonHoc()
        {
            string query = "SELECT * FROM MonHoc";

            // Sử dụng SqlConnection và SqlDataAdapter để lấy dữ liệu từ cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();

                        adapter.Fill(dataTable);

                        dgvMonHoc.DataSource = dataTable;

                        // Đặt tên cho các header
                        dgvMonHoc.Columns["MaHP"].HeaderText = "Mã học phần";
                        dgvMonHoc.Columns["TenHP"].HeaderText = "Tên học phần";
                        dgvMonHoc.Columns["SoTinChi"].HeaderText = "Số tín chỉ";

                        // Thêm cột số thứ tự
                        dataTable.Columns.Add("STT", typeof(int));
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            dataTable.Rows[i]["STT"] = i + 1;
                        }

                        dgvMonHoc.Columns["STT"].DisplayIndex = 0;
                        dgvMonHoc.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private void dgvMonHoc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu hàng được click là hợp lệ
            if (e.RowIndex >= 0)
            {
                // Lấy hàng được chọn
                DataGridViewRow selectedRow = dgvMonHoc.Rows[e.RowIndex];

                // Hiển thị dữ liệu từ hàng được chọn lên các TextBox
                txtMaHocPhan.Text = selectedRow.Cells["MaHP"].Value.ToString();
                txtTenHocPhan.Text = selectedRow.Cells["TenHP"].Value.ToString();
                txtSoTinChi.Text = selectedRow.Cells["SoTinChi"].Value.ToString();
            }
        }
        //-----------------------Học phần -------------------------
        private string GenerateNewMaLopHocPhan(string maMonHoc)
        {
            // Truy vấn để lấy các MaLopHocPhan hiện có tương ứng với MaMonHoc đã chọn
            string query = "SELECT MaLopHocPhan FROM LopHocPhan WHERE MaMonHoc = @MaMonHoc";
            List<string> existingMaLopHocPhanList = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaMonHoc", maMonHoc);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            existingMaLopHocPhanList.Add(reader["MaLopHocPhan"].ToString());
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                    return null;
                }
            }

            // Tìm giá trị lớn nhất và tăng dần chữ số phía sau
            int maxNumber = 0;
            foreach (var maLopHocPhan in existingMaLopHocPhanList)
            {
                string numberPart = maLopHocPhan.Substring(maMonHoc.Length);
                if (int.TryParse(numberPart, out int number))
                {
                    if (number > maxNumber)
                    {
                        maxNumber = number;
                    }
                }
            }

            // Tạo mã lớp học phần mới
            int newNumber = maxNumber + 1;
            string newMaLopHocPhan = $"{maMonHoc}{newNumber:D3}";
            return newMaLopHocPhan;
        }
        private void LoadCB_MaMonHoc()
        {
            string query = "SELECT MaHP, TenHP FROM MonHoc";

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        cbMaMonHoc.DataSource = dataTable;
                        cbMaMonHoc.DisplayMember = "TenHP"; // Hiển thị tên môn học
                        cbMaMonHoc.ValueMember = "MaHP";    // Sử dụng MaMonHoc làm giá trị

                        cbMaMonHoc.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
            //isInitializing = false; // Đặt biến trạng thái sau khi khởi tạo xong
        }
        private void LoadDGV_LopHocPhan()
        {
            string query = @"
        SELECT 
            lhp.MaLopHocPhan, 
            mh.TenHP, 
            lhp.DiaDiem, 
            gv.HoTen
        FROM 
            LopHocPhan lhp
        INNER JOIN 
            MonHoc mh ON lhp.MaMonHoc = mh.MaHP
        INNER JOIN 
            GiaoVien gv ON lhp.MaGV = gv.MaGV";

            // Sử dụng SqlConnection và SqlDataAdapter để lấy dữ liệu từ cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dgvLopHocPhan.DataSource = dataTable;

                        // Đặt tên các cột nếu cần
                        dgvLopHocPhan.Columns["MaLopHocPhan"].HeaderText = "Mã Lớp Học Phần";
                        dgvLopHocPhan.Columns["TenHP"].HeaderText = "Tên Môn Học";
                        dgvLopHocPhan.Columns["DiaDiem"].HeaderText = "Địa Điểm";
                        dgvLopHocPhan.Columns["HoTen"].HeaderText = "Tên Giáo Viên";

                        // Thêm cột số thứ tự
                        dataTable.Columns.Add("STT", typeof(int));
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            dataTable.Rows[i]["STT"] = i + 1;
                        }

                        dgvLopHocPhan.Columns["STT"].DisplayIndex = 0;
                        dgvLopHocPhan.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        dgvLopHocPhan.Columns["DiaDiem"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private void LoadCB_GiaoVien()
        {
            string query = "SELECT MaGV, HoTen FROM GiaoVien";

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        cbGiaoVienLopHocPhan.DataSource = dataTable;
                        cbGiaoVienLopHocPhan.DisplayMember = "Hoten"; // Hiển thị tên giáo viên
                        cbGiaoVienLopHocPhan.ValueMember = "MaGV"; // Giá trị là mã giáo viên

                        cbGiaoVienLopHocPhan.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnRefresh2_Click(object sender, EventArgs e)
        {
            txtMaLopHocPhan.Text = "";
            txtDiaDiem.Text = "";
            cbMaMonHoc.SelectedIndex = -1;
            cbGiaoVienLopHocPhan.SelectedIndex = -1;
            isEditing = false;
            EnableControls(true);
            btnThemLopHocPhan.Enabled = true;
            btnSuaLopHocPhan.Text = "Sửa";
            LoadDGV_LopHocPhan();
        }

        private void btnThemLopHocPhan_Click(object sender, EventArgs e)
        {
            // Kiểm tra các trường đầu vào
            if (string.IsNullOrWhiteSpace(cbMaMonHoc.SelectedValue.ToString()) ||
                string.IsNullOrWhiteSpace(txtDiaDiem.Text) ||
                string.IsNullOrWhiteSpace(cbGiaoVienLopHocPhan.SelectedValue.ToString()))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            // Lấy dữ liệu từ các trường
            string maMonHoc = cbMaMonHoc.SelectedValue.ToString();
            string diaDiem = txtDiaDiem.Text;
            string maGiaoVien = cbGiaoVienLopHocPhan.SelectedValue.ToString();
            string maLopHocPhan = GenerateNewMaLopHocPhan(maMonHoc);

            // Câu lệnh SQL để thêm lớp học phần
            string query = "INSERT INTO LopHocPhan (MaLopHocPhan, MaMonHoc, DiaDiem, MaGV) VALUES (@MaLopHocPhan, @MaMonHoc, @DiaDiem, @MaGV)";

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số vào câu lệnh SQL
                        command.Parameters.AddWithValue("@MaLopHocPhan", maLopHocPhan);
                        command.Parameters.AddWithValue("@MaMonHoc", maMonHoc);
                        command.Parameters.AddWithValue("@DiaDiem", diaDiem);
                        command.Parameters.AddWithValue("@MaGV", maGiaoVien);

                        // Thực thi câu lệnh SQL
                        command.ExecuteNonQuery();
                        MessageBox.Show("Thêm lớp học phần thành công.");
                        LoadDGV_LopHocPhan(); // Tải lại dữ liệu lên DataGridView
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private void btnSuaLopHocPhan_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                // Chuyển sang chế độ chỉnh sửa
                EnableControls(true);
                btnSuaLopHocPhan.Text = "Lưu";
                isEditing = true;
            }
            else
            {
                // Thực hiện chức năng lưu
                SaveLopHocPhan();

                // Chuyển trở lại chế độ xem
                EnableControls(false);
                btnSuaLopHocPhan.Text = "Sửa";
                isEditing = false;
            }
        }
        private void SaveLopHocPhan()
        {
            // Lấy dữ liệu từ dòng được chọn
            DataGridViewRow selectedRow = dgvLopHocPhan.SelectedRows[0];
            string maLopHocPhan = selectedRow.Cells["MaLopHocPhan"].Value.ToString();

            // Lấy dữ liệu từ các trường
            string maMonHoc = cbMaMonHoc.SelectedValue.ToString();
            string diaDiem = txtDiaDiem.Text;
            string maGiaoVien = cbGiaoVienLopHocPhan.SelectedValue.ToString();

            // Câu lệnh SQL để cập nhật lớp học phần
            string query = "UPDATE LopHocPhan SET MaMonHoc = @MaMonHoc, DiaDiem = @DiaDiem, MaGV = @MaGV WHERE MaLopHocPhan = @MaLopHocPhan";

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số vào câu lệnh SQL
                        command.Parameters.AddWithValue("@MaLopHocPhan", maLopHocPhan);
                        command.Parameters.AddWithValue("@MaMonHoc", maMonHoc);
                        command.Parameters.AddWithValue("@DiaDiem", diaDiem);
                        command.Parameters.AddWithValue("@MaGV", maGiaoVien);

                        // Thực thi câu lệnh SQL
                        command.ExecuteNonQuery();
                        MessageBox.Show("Cập nhật lớp học phần thành công.");
                        LoadDGV_LopHocPhan(); // Tải lại dữ liệu lên DataGridView
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }

            // Hiển thị thông báo thành công
            MessageBox.Show("Cập nhật lớp học phần thành công.");
            LoadDGV_LopHocPhan();
        }
        private void btnXoaLopHocPhan_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ dòng được chọn
            string maLopHocPhan = txtMaLopHocPhan.Text.ToString();

            // Xác nhận việc xóa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa lớp học phần này?", "Xác nhận xóa", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                // Câu lệnh SQL để xóa lớp học phần
                string query = "DELETE FROM LopHocPhan WHERE MaLopHocPhan = @MaLopHocPhan";

                using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
                {
                    try
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Thêm tham số vào câu lệnh SQL
                            command.Parameters.AddWithValue("@MaLopHocPhan", maLopHocPhan);

                            // Thực thi câu lệnh SQL
                            command.ExecuteNonQuery();
                            MessageBox.Show("Xóa lớp học phần thành công.");
                            LoadDGV_LopHocPhan(); // Tải lại dữ liệu lên DataGridView
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
        }
        private void dgvLopHocPhan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLopHocPhan.Rows[e.RowIndex];

                // Hiển thị dữ liệu lên các TextBox và ComboBox
                txtMaLopHocPhan.Text = row.Cells["MaLopHocPhan"].Value.ToString();

                // Lấy giá trị TenHP từ DataGridView
                string tenHP = row.Cells["TenHP"].Value.ToString();

                string maMonHoc = GetMaMonHocFromTenHP(tenHP);
                if (!string.IsNullOrEmpty(maMonHoc))
                {
                    cbMaMonHoc.SelectedValue = maMonHoc;
                }

                txtDiaDiem.Text = row.Cells["DiaDiem"].Value.ToString();

                // Lấy giá trị HoTen từ DataGridView
                string hoTenGiaoVien = row.Cells["HoTen"].Value.ToString();

                // Gọi phương thức để lấy MaGiaoVien từ HoTen và gán giá trị cho ComboBox
                string maGiaoVien = GetMaGiaoVienFromHoTen(hoTenGiaoVien);
                if (!string.IsNullOrEmpty(maGiaoVien))
                {
                    cbGiaoVienLopHocPhan.SelectedValue = maGiaoVien;
                }
            }
            btnThemLopHocPhan.Enabled = false;
            cbGiaoVienLopHocPhan.Enabled = false;
            cbMaMonHoc.Enabled = false;
            txtDiaDiem.Enabled = false;
        }
        private string GetMaMonHocFromTenHP(string tenHP)
        {
            string maMonHoc = string.Empty;
            string query = "SELECT MaHP FROM MonHoc WHERE TenHP = @TenHP";

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenHP", tenHP);
                    try
                    {
                        connection.Open();
                        maMonHoc = command.ExecuteScalar()?.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }

            return maMonHoc;
        }
        private string GetMaGiaoVienFromHoTen(string hoTen)
        {
            string maGiaoVien = string.Empty;
            string query = "SELECT MaGV FROM GiaoVien WHERE HoTen = @HoTen";

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@HoTen", hoTen);
                    try
                    {
                        connection.Open();
                        maGiaoVien = command.ExecuteScalar()?.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }

            return maGiaoVien;
        }
        private void EnableControls(bool enable)
        {
            // Thiết lập trạng thái của các điều khiển
            txtDiaDiem.Enabled = enable;
            cbMaMonHoc.Enabled = enable;
            cbGiaoVienLopHocPhan.Enabled = enable;
        }

        //-----------------------ĐIỂM -----------------------------

        //-----------------------Học phí --------------------------
    }
}
