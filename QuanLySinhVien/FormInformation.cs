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

namespace QuanLySinhVien
{
    public partial class FormInformation : Form
    {
        private string MaSV;
        private bool isEditing = false;

        public FormInformation(ManHinhChinh manHinhChinh, string masv)
        {
            InitializeComponent();
            MaSV = masv;
            LoadCB_Nganh();
            LoadCB_Lop();
            LoadCB_TenLopHocPhan();
            LoadDataFromMaSV();
            LoadDGV_HocPhan();
            LoadDGV_HocPhi();
            LoadDGV_Diem();
        }
        private void LoadDataFromMaSV()
        {
            string query = "SELECT * FROM SinhVien WHERE MaSV = @MaSV";
            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số cho mã sinh viên vào câu lệnh SQL
                        command.Parameters.AddWithValue("@MaSV", MaSV);

                        // Thực thi câu lệnh SQL và đọc dữ liệu
                        SqlDataReader reader = command.ExecuteReader();

                        // Kiểm tra xem có dữ liệu được trả về hay không
                        if (reader.Read())
                        {
                            // Hiển thị thông tin sinh viên lên các điều khiển trên form
                            txtMaSV.Text = reader["MaSV"].ToString();
                            txtName.Text = reader["HoTen"].ToString();
                            dtpBirth.Value = Convert.ToDateTime(reader["NgaySinh"]);
                            txtEmail.Text = reader["Email"].ToString();
                            txtPhone.Text = reader["SDT"].ToString();
                            txtAddress.Text = reader["DiaChi"].ToString();

                            // Đặt giới tính
                            string gender = reader["GioiTinh"].ToString();
                            if (gender == "Nam")
                            {
                                radioGTNam.Checked = true;
                            }
                            else
                            {
                                radioGTNu.Checked = true;
                            }

                            // Lấy thông tin về lớp và ngành của sinh viên
                            string maLop = reader["MaLop"].ToString();

                            string maNganh = GetNganhFromMaLop(maLop);

                            // Chọn ngành và lớp tương ứng trong ComboBox
                            foreach (var item in cbNganh.Items)
                            {
                                if (item is Nganh nganh && nganh.MaNganh == maNganh)
                                {
                                    cbNganh.SelectedItem = item;
                                    break;
                                }
                            }
                            cbLop.SelectedValue = maLop;
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private string GetNganhFromMaLop(string maLop)
        {
            string maNganh = "";

            // Tạo câu lệnh SQL để lấy MaNganh từ MaLop
            string query = "SELECT MaNganh FROM Lop WHERE MaLop = @MaLop";

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số cho mã lớp vào câu lệnh SQL
                        command.Parameters.AddWithValue("@MaLop", maLop);

                        // Thực thi câu lệnh SQL và đọc dữ liệu
                        SqlDataReader reader = command.ExecuteReader();

                        // Kiểm tra xem có dữ liệu được trả về hay không
                        if (reader.Read())
                        {
                            // Lấy MaNganh từ dữ liệu đọc được

                            maNganh = reader["MaNganh"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Mã Ngành cho Mã Lớp " + maLop);
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }

            return maNganh;
        }

        private void btnSaveSV_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo câu lệnh SQL UPDATE để cập nhật dữ liệu của sinh viên
                string query = @"UPDATE SinhVien SET HoTen = @TenSV, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, Email = @Email, DiaChi = @DiaChi, SDT = @DienThoai, MaLop = @MaLop WHERE MaSV = @MaSV";

                using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
                {
                    connection.Open();

                    // Tạo và thực thi command
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Gán giá trị cho các tham số trong câu lệnh SQL
                        command.Parameters.AddWithValue("@MaSV", txtMaSV.Text);
                        command.Parameters.AddWithValue("@TenSV", txtName.Text);
                        command.Parameters.AddWithValue("@NgaySinh", dtpBirth.Value);
                        command.Parameters.AddWithValue("@GioiTinh", radioGTNam.Checked ? "Nam" : "Nữ");
                        command.Parameters.AddWithValue("@Email", txtEmail.Text);
                        command.Parameters.AddWithValue("@DiaChi", txtAddress.Text);
                        command.Parameters.AddWithValue("@DienThoai", txtPhone.Text);
                        command.Parameters.AddWithValue("@MaLop", cbLop.SelectedValue.ToString());

                        // Thực thi câu lệnh SQL UPDATE
                        int rowsAffected = command.ExecuteNonQuery();

                        // Kiểm tra xem có dòng dữ liệu nào bị ảnh hưởng không
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật thông tin sinh viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                // Vô hiệu hóa các điều khiển sau khi cập nhật thành công
                txtEmail.Enabled = false;
                txtAddress.Enabled = false;
                txtName.Enabled = false;
                txtPhone.Enabled = false;

                radioGTNam.Enabled = false;
                radioGTNu.Enabled = false;

                cbLop.Enabled = false;
                cbNganh.Enabled = false;

                dtpBirth.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditSV_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                txtEmail.Enabled = false;
                txtAddress.Enabled = false;
                txtName.Enabled = false;
                txtPhone.Enabled = false;
                radioGTNam.Enabled = false;
                radioGTNu.Enabled = false;
                cbLop.Enabled = false;
                cbNganh.Enabled = false;
                dtpBirth.Enabled = false;

                isEditing = false;
            }
            else
            {
                txtEmail.Enabled = true;
                txtAddress.Enabled = true;
                txtName.Enabled = true;
                txtPhone.Enabled = true;
                radioGTNam.Enabled = true;
                radioGTNu.Enabled = true;
                cbLop.Enabled = true;
                cbNganh.Enabled = true;
                dtpBirth.Enabled = true;

                isEditing = true;
            }
        }

        private void btnDeleteSV_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra xem người dùng đã xác nhận xóa chưa
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
                    {
                        // Mở kết nối
                        connection.Open();

                        // Tạo câu lệnh SQL để xóa sinh viên
                        string query = "DELETE FROM SinhVien WHERE MaSV = @MaSV";

                        // Tạo và thực thi command
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Gán giá trị cho tham số MaSV
                            command.Parameters.AddWithValue("@MaSV", MaSV);

                            // Thực thi câu lệnh SQL
                            int rowsAffected = command.ExecuteNonQuery();

                            // Kiểm tra xem có dòng dữ liệu nào bị ảnh hưởng không
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa sinh viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            cbLop.Items.Add(new Lop(maLop, tenLop));
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

        //=============Đăng kí học phần==========================
        private void LoadCB_TenLopHocPhan()
        {
            // Tạo câu truy vấn SQL để lấy tên môn học từ bảng MonHoc dựa trên MaLopHocPhan
            string query = @"SELECT mh.TenHP, lhp.MaLopHocPhan FROM LopHocPhan lhp INNER JOIN MonHoc mh ON lhp.MaMonHoc = mh.MaHP";

            // Xóa các mục có sẵn trong ComboBox để tránh trùng lặp
            cbTenLopHocPhan.Items.Clear();

            // Sử dụng SqlConnection và SqlCommand để thực hiện truy vấn
            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        // Duyệt qua các dòng kết quả và thêm vào ComboBox
                        while (reader.Read())
                        {
                            string tenMonHoc = reader["TenHP"].ToString();
                            string maMonHoc = reader["MaLopHocPhan"].ToString();
                            string data = maMonHoc + ": "+ tenMonHoc;
                            cbTenLopHocPhan.Items.Add(data);
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
        private void LoadDGV_HocPhan()
        {
            string query = "SELECT SLHP.MaLopHocPhan, LP.DiaDiem, GV.HoTen FROM SinhVien_LopHocPhan SLHP JOIN LopHocPhan LP ON SLHP.MaLopHocPhan = LP.MaLopHocPhan JOIN GiaoVien GV ON LP.MaGV = GV.MaGV WHERE SLHP.MaSV = @MaSV";

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@MaSV", MaSV);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Xóa cột MaSV nếu tồn tại
                        if (dataTable.Columns.Contains("MaSV"))
                        {
                            dataTable.Columns.Remove("MaSV");
                        }

                        dgvHocPhan.DataSource = dataTable;

                        dgvHocPhan.Columns["MaLopHocPhan"].HeaderText = "Mã Lớp Học Phần";
                        dgvHocPhan.Columns["DiaDiem"].HeaderText = "Địa Điểm";
                        dgvHocPhan.Columns["HoTen"].HeaderText = "Giáo Viên";

                        // Thêm cột số thứ tự
                        dataTable.Columns.Add("STT", typeof(int));
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            dataTable.Rows[i]["STT"] = i + 1;
                        }

                        dgvHocPhan.Columns["STT"].DisplayIndex = 0;
                        dgvHocPhan.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private void btnDangKiHocPhan_Click(object sender, EventArgs e)
        {
            if (cbTenLopHocPhan.SelectedItem != null)
            {
                string maLopHocPhan = cbTenLopHocPhan.SelectedItem.ToString().Split(':')[0].Trim();
                // Thêm dữ liệu vào bảng SinhVien_LopHocPhan
                string querySinhVienLopHocPhan = "INSERT INTO SinhVien_LopHocPhan (MaSV, MaLopHocPhan, MaHocKi) VALUES (@MaSV, @MaLopHocPhan, @MaHocKi)";

                // Thêm dữ liệu vào bảng Diem
                string queryDiem = "INSERT INTO Diem (MaSV, MaLopHocPhan, MaHocKi) VALUES (@MaSV, @MaLopHocPhan, @MaHocKi)";

                using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
                {
                    try
                    {
                        connection.Open();
                        // Thêm sinh viên vào bảng SinhVien_LopHocPhan
                        using (SqlCommand commandSVLHP = new SqlCommand(querySinhVienLopHocPhan, connection))
                        {
                            commandSVLHP.Parameters.AddWithValue("@MaSV", MaSV);
                            commandSVLHP.Parameters.AddWithValue("@MaLopHocPhan", maLopHocPhan);
                            commandSVLHP.Parameters.AddWithValue("@MaHocKi", "1");

                            int rowsAffectedSVLHP = commandSVLHP.ExecuteNonQuery();

                            // Thêm điểm cho sinh viên vào bảng Diem
                            if (rowsAffectedSVLHP > 0)
                            {
                                using (SqlCommand commandDiem = new SqlCommand(queryDiem, connection))
                                {
                                    commandDiem.Parameters.AddWithValue("@MaSV", MaSV);
                                    commandDiem.Parameters.AddWithValue("@MaLopHocPhan", maLopHocPhan);
                                    commandDiem.Parameters.AddWithValue("@MaHocKi", "1");


                                    int rowsAffectedDiem = commandDiem.ExecuteNonQuery();

                                    if (rowsAffectedDiem > 0)
                                    {
                                        MessageBox.Show("Đăng ký học phần " + maLopHocPhan + " thành công");
                                        LoadDGV_HocPhan();
                                        LoadDGV_Diem();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Đăng ký học phần thất bại.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn mã lớp học phần.");
            }
        }

        private void btnXoaHocPhan_Click(object sender, EventArgs e)
        {
            if (dgvHocPhan.SelectedRows.Count > 0)
            {
                string maLopHocPhan = dgvHocPhan.SelectedRows[0].Cells["MaLopHocPhan"].Value.ToString();

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM SinhVien_LopHocPhan WHERE MaSV = @MaSV AND MaLopHocPhan = @MaLopHocPhan";

                    using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
                    {
                        try
                        {
                            connection.Open();
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@MaSV", MaSV);
                                command.Parameters.AddWithValue("@MaLopHocPhan", maLopHocPhan);

                                // Thực thi câu lệnh
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Xóa học phần thành công.");
                                    LoadDGV_HocPhan();
                                }
                                else
                                {
                                    MessageBox.Show("Không tìm thấy học phần để xóa.");
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
                MessageBox.Show("Vui lòng chọn học phần cần xóa.");
            }
        }
        //=============Tính toán học phí==========================
        private void LoadDGV_HocPhi()
        {
            string query = @"
        SELECT 
            SinhVien_LopHocPhan.MaLopHocPhan AS [Mã Lớp Học Phần], 
            MonHoc.TenHP AS [Tên Học Phần], 
            MonHoc.SoTinChi AS [Số Tín Chỉ], 
            MonHoc.DonGia AS [Đơn Giá], 
            (MonHoc.SoTinChi * MonHoc.DonGia) AS [Học Phí]
        FROM 
            SinhVien_LopHocPhan
        INNER JOIN 
            LopHocPhan ON SinhVien_LopHocPhan.MaLopHocPhan = LopHocPhan.MaLopHocPhan
        INNER JOIN 
            MonHoc ON LopHocPhan.MaMonHoc = MonHoc.MaHP
        WHERE 
            SinhVien_LopHocPhan.MaSV = @MaSV";

            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.SelectCommand.Parameters.AddWithValue("@MaSV", MaSV);

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Tính tổng học phí
                        decimal totalHocPhi = 0;
                        foreach (DataRow row in dataTable.Rows)
                        {
                            totalHocPhi += Convert.ToDecimal(row["Học Phí"]);
                        }

                        // Hiển thị tổng học phí trong TextBox
                        txtTongHocPhi.Text = totalHocPhi.ToString("N0"); // Định dạng tiền tệ

                        // Gán dữ liệu từ DataTable vào DataGridView
                        dgvHocPhi.DataSource = dataTable;

                        // Thêm cột số thứ tự
                        dataTable.Columns.Add("STT", typeof(int));
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            dataTable.Rows[i]["STT"] = i + 1;
                        }

                        dgvHocPhi.Columns["STT"].DisplayIndex = 0;
                        dgvHocPhi.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnRefresh3_Click(object sender, EventArgs e)
        {
            LoadDGV_HocPhi();
        }
        //=============Điểm==========================
        private void LoadDGV_Diem()
        {
            // Câu truy vấn để lấy dữ liệu điểm sinh viên và các thông tin liên quan
            string query = @"
        SELECT 
            Diem.MaLopHocPhan AS [Mã Lớp Học Phần], 
            MonHoc.TenHP AS [Tên Học Phần], 
            Diem.Diem AS [Điểm]
        FROM 
            Diem
        INNER JOIN 
            LopHocPhan ON Diem.MaLopHocPhan = LopHocPhan.MaLopHocPhan
        INNER JOIN 
            MonHoc ON LopHocPhan.MaMonHoc = MonHoc.MaHP
        WHERE
            Diem.MaSV = @MaSV";

            // Sử dụng SqlConnection và SqlDataAdapter để lấy dữ liệu từ cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.SelectCommand.Parameters.AddWithValue("@MaSV", MaSV);

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Gán DataTable làm DataSource cho DataGridView
                        dgvDiem.DataSource = dataTable;

                        // Thêm cột số thứ tự
                        dataTable.Columns.Add("STT", typeof(int));
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            dataTable.Rows[i]["STT"] = i + 1;
                        }

                        dgvDiem.Columns["STT"].DisplayIndex = 0;
                        dgvDiem.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        dgvDiem.Columns["Điểm"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnEditMark_Click(object sender, EventArgs e)
        {
            dgvDiem.ReadOnly =  false;
            btnSaveMark.Enabled = true;
        }

        private void btnSaveMark_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvDiem.Rows)
            {
                // Kiểm tra xem ô "Mã Lớp Học Phần" và "Điểm" có giá trị null không trước khi sử dụng
                if (row.Cells["Mã Lớp Học Phần"].Value != null && row.Cells["Điểm"].Value != null)
                {
                    string maLopHocPhan = row.Cells["Mã Lớp Học Phần"].Value.ToString();
                    string diem = row.Cells["Điểm"].Value.ToString();

                    // Câu lệnh SQL kiểm tra xem điểm đã tồn tại trong cơ sở dữ liệu chưa
                    string queryCheck = "SELECT COUNT(*) FROM Diem WHERE MaSV = @MaSV AND MaLopHocPhan = @MaLopHocPhan";

                    // Câu lệnh SQL cập nhật điểm nếu đã tồn tại hoặc chèn mới nếu chưa tồn tại
                    string queryUpdate = @"
                            IF EXISTS (SELECT * FROM Diem WHERE MaSV = @MaSV AND MaLopHocPhan = @MaLopHocPhan)
                            UPDATE Diem SET Diem = @Diem WHERE MaSV = @MaSV AND MaLopHocPhan = @MaLopHocPhan
                            ELSE
                            INSERT INTO Diem (MaSV, MaLopHocPhan, Diem) VALUES (@MaSV, @MaLopHocPhan, @Diem)";

                    using (SqlConnection connection = new SqlConnection(ConnectSQL.connectionString))
                    {
                        try
                        {
                            connection.Open();

                            // Kiểm tra xem điểm đã tồn tại trong cơ sở dữ liệu chưa
                            using (SqlCommand commandCheck = new SqlCommand(queryCheck, connection))
                            {
                                commandCheck.Parameters.AddWithValue("@MaSV", MaSV);
                                commandCheck.Parameters.AddWithValue("@MaLopHocPhan", maLopHocPhan);

                                int count = (int)commandCheck.ExecuteScalar();

                                // Thực hiện cập nhật hoặc chèn dữ liệu
                                using (SqlCommand commandUpdate = new SqlCommand(queryUpdate, connection))
                                {
                                    commandUpdate.Parameters.AddWithValue("@MaSV", MaSV);
                                    commandUpdate.Parameters.AddWithValue("@MaLopHocPhan", maLopHocPhan);
                                    commandUpdate.Parameters.AddWithValue("@Diem", diem);

                                    commandUpdate.ExecuteNonQuery();
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
        }
        private void TinhDiemTB()
        {
            double sum = 0;
            int count = 0;

            // Duyệt qua từng hàng trong DataGridView
            foreach (DataGridViewRow row in dgvDiem.Rows)
            {
                // Kiểm tra nếu ô "Điểm" có giá trị và không phải là NULL hoặc chuỗi rỗng
                if (row.Cells["Điểm"].Value != null && row.Cells["Điểm"].Value != DBNull.Value && !string.IsNullOrEmpty(row.Cells["Điểm"].Value.ToString()))
                {
                    // Thực hiện tính toán chỉ khi ô "Điểm" có dữ liệu
                    double mark = Convert.ToDouble(row.Cells["Điểm"].Value);
                    sum += mark;
                    count++;
                }
            }

            if (count > 0)
            {
                double averageMark = sum / count;
                // Hiển thị điểm trung bình
                lbAM10.Text = averageMark.ToString("#0.00");
                lbAM4.Text = (averageMark / 2).ToString("#0.00");
            }
            else
            {
                // Hiển thị 0 nếu không có điểm nào được nhập
                lbAM10.Text = "0";
                lbAM4.Text = "0";
            }
        }

        private void btnRefresh4_Click(object sender, EventArgs e)
        {
            LoadDGV_Diem();
        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            TinhDiemTB();
        }

    }
}
