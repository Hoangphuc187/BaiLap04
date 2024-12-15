using bt.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace bt
{
    public partial class Quanlysinhvien : Form
    {
        private Model1 dbContext;

        public Quanlysinhvien()
        {
            InitializeComponent();
            dbContext = new Model1();
        }

        private void Quanlysinhvien_Load(object sender, EventArgs e)
        {
            try
            {
                List<Student> students = dbContext.Student.ToList();
                List<Faculty> faculties = dbContext.Faculty.ToList();
                FillFacultyCombobox(faculties);
                BindGrid(students);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillFacultyCombobox(List<Faculty> faculties)
        {
            cmbFaculty.DataSource = faculties;
            cmbFaculty.DisplayMember = "FacultyName";
            cmbFaculty.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> students)
        {
            dgvStudent.Rows.Clear();
            foreach (var student in students)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = student.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = student.FullName;
                dgvStudent.Rows[index].Cells[2].Value = student.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = student.AverageScore;
            }
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            try
            {
                string studentID = txtMSSV.Text;
                string fullName = txtHoten.Text;
                double averageScore = double.Parse(txtDTB.Text);
                int facultyID = int.Parse(cmbFaculty.SelectedValue.ToString());

                if (dbContext.Student.Any(s => s.StudentID == studentID))
                {
                    MessageBox.Show("Mã sinh viên đã tồn tại. Vui lòng nhập mã khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newStudent = new Student
                {
                    StudentID = studentID,
                    FullName = fullName,
                    FacultyID = facultyID,
                    AverageScore = averageScore
                };

                dbContext.Student.Add(newStudent);
                dbContext.SaveChanges();

                BindGrid(dbContext.Student.ToList());

                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            try
            {
                string studentID = txtMSSV.Text;
                var student = dbContext.Student.FirstOrDefault(s => s.StudentID == studentID);

                if (student != null)
                {
                    student.FullName = txtHoten.Text;
                    student.AverageScore = double.Parse(txtDTB.Text);
                    student.FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString());

                    dbContext.SaveChanges();

                    BindGrid(dbContext.Student.ToList());

                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sinh viên không tìm thấy!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            try
            {
                string studentID = txtMSSV.Text;
                var student = dbContext.Student.FirstOrDefault(s => s.StudentID == studentID);

                if (student != null)
                {
                    dbContext.Student.Remove(student);
                    dbContext.SaveChanges();

                    BindGrid(dbContext.Student.ToList());

                    MessageBox.Show("Sinh viên đã được xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sinh viên không tìm thấy!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvStudent.Rows[e.RowIndex];

                txtMSSV.Text = selectedRow.Cells[0].Value.ToString();
                txtHoten.Text = selectedRow.Cells[1].Value.ToString();
                txtDTB.Text = selectedRow.Cells[3].Value.ToString();
                cmbFaculty.Text = selectedRow.Cells[2].Value.ToString();
            }
        }
    }
}
