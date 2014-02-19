﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace endoDB
{
    public partial class ExamList : Form
    {
        public DataTable exam_list = new DataTable();
        private string dateFrom;
        private string dateTo;
        private string pt_id;
        private string department;
        private string operator1;
        private Boolean op1_5;

        public ExamList(string _date_from, string _date_to, string _pt_id, string _department, string _operator1, Boolean _op1_5)
        {
            InitializeComponent();

            dgvExamList.RowHeadersVisible = false;
            dgvExamList.MultiSelect = false;
            dgvExamList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvExamList.Font = new Font(dgvExamList.Font.Name, 12);
            dgvExamList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvExamList.DataSource = exam_list;
            dateFrom = _date_from;
            dateTo = _date_to;
            pt_id = _pt_id;
            department = _department;
            operator1 = _operator1;
            op1_5 = _op1_5;

            searchExam(dateFrom, dateTo, pt_id, department, operator1, op1_5);

            #region Add btSelect
            DataGridViewButtonColumn btSelect = new DataGridViewButtonColumn(); //DataGridViewButtonColumnの作成
            btSelect.Name = "btSelect";  //列の名前を設定
            btSelect.UseColumnTextForButtonValue = true;    //ボタンにテキスト表示
            btSelect.Text = Properties.Resources.ptSelect;  //ボタンの表示テキスト設定
            dgvExamList.Columns.Add(btSelect);           //ボタン追加
            #endregion

            #region Add btImage
            DataGridViewButtonColumn btImage = new DataGridViewButtonColumn(); //DataGridViewButtonColumnの作成
            btImage.Name = "btImage";  //列の名前を設定
            btImage.UseColumnTextForButtonValue = true;    //ボタンにテキスト表示
            btImage.Text = Properties.Resources.Image;  //ボタンの表示テキスト設定
            dgvExamList.Columns.Add(btImage);           //ボタン追加
            #endregion

            #region Add btDelColumn
            DataGridViewButtonColumn btDelColumn = new DataGridViewButtonColumn(); //DataGridViewButtonColumnの作成
            btDelColumn.Name = "btDelColumn";  //列の名前を設定
            btDelColumn.UseColumnTextForButtonValue = true;    //ボタンにテキスト表示
            btDelColumn.Text = Properties.Resources.Delete;  //ボタンの表示テキスト設定
            dgvExamList.Columns.Add(btDelColumn);           //ボタン追加
            #endregion

            #region Add btPrint
            DataGridViewButtonColumn btPrint = new DataGridViewButtonColumn(); //DataGridViewButtonColumnの作成
            btPrint.Name = "btPrint";  //列の名前を設定
            btPrint.UseColumnTextForButtonValue = true;    //ボタンにテキスト表示
            btPrint.Text = Properties.Resources.Print;  //ボタンの表示テキスト設定
            dgvExamList.Columns.Add(btPrint);           //ボタン追加
            #endregion

            #region Design and Settings
            exam_list.DefaultView.RowFilter = "exam_status < 2";  //Show only blank/draft findings.

            this.dgvExamList.Columns["exam_id"].Visible = false;
            this.dgvExamList.Columns["exam_status"].Visible = false;

            //Without below settings, this application will down.
            dgvExamList.Columns["btSelect"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvExamList.Columns["btImage"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvExamList.Columns["btDelColumn"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvExamList.Columns["btPrint"].SortMode = DataGridViewColumnSortMode.NotSortable;

            btDone.Text = Properties.Resources.Done;
            btChecked.Text = Properties.Resources.Checked;
            btCanceled.Text = Properties.Resources.Canceled;
            #endregion

            #region Change columns header text
            this.dgvExamList.Columns["pt_id"].HeaderText = "ID";
            this.dgvExamList.Columns["pt_name"].HeaderText = Properties.Resources.ptName;
            this.dgvExamList.Columns["exam_day"].HeaderText = Properties.Resources.Date;
            this.dgvExamList.Columns["exam_type_name"].HeaderText = Properties.Resources.ExamType;
            this.dgvExamList.Columns["name1"].HeaderText = Properties.Resources.Department;
            this.dgvExamList.Columns["ward"].HeaderText = Properties.Resources.Ward;
            this.dgvExamList.Columns["status_name"].HeaderText = Properties.Resources.Status;
            dgvExamList.Columns["btSelect"].HeaderText = Properties.Resources.ptSelect;
            dgvExamList.Columns["btImage"].HeaderText = Properties.Resources.Image;
            dgvExamList.Columns["btDelColumn"].HeaderText = Properties.Resources.Delete;
            dgvExamList.Columns["btPrint"].HeaderText = Properties.Resources.Print;
            #endregion

            resizeColumns();
        }

        private void searchExam(string _date_from, string _date_to, string _pt_id, string _department, string _operator, Boolean _op1_5)
        {
            #region Npgsql connection
            NpgsqlConnection conn;

            try
            {
                conn = new NpgsqlConnection("Server=" + Settings.DBSrvIP + ";Port=" + Settings.DBSrvPort + ";User Id=" +
                    Settings.DBconnectID + ";Password=" + Settings.DBconnectPw + ";Database=endoDB;");
            }
            catch (ArgumentException)
            {
                MessageBox.Show(Properties.Resources.WrongConnectingString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            string status_name;
            string exam_type_name;
            if (Settings.isJP)
            {
                status_name = "name_jp";
                exam_type_name = "name_jp";
            }
            else
            {
                status_name = "name_eng";
                exam_type_name = "name_eng";
            }
            string sql =
                "SELECT exam_id, exam.pt_id, pt_name, exam_day, exam_type." + exam_type_name + " AS exam_type_name, department.name1, ward, status."
                + status_name
                + " AS status_name, exam_status FROM exam"
                + " INNER JOIN patient ON exam.pt_id = patient.pt_id"
                + " INNER JOIN exam_type ON exam.exam_type = exam_type.type_no"
                + " LEFT JOIN department ON exam.department = department.code"
                + " LEFT JOIN ward ON exam.ward_id = ward.ward_no"
                + " INNER JOIN status ON exam.exam_status = status.status_no"
                + " WHERE exam_visible = true";
            if (_date_from != null)
            { sql += " AND exam_day>='" + _date_from + "' AND exam_day<='" + _date_to + "'"; }
            if (_pt_id != null)
            { sql += " AND exam.pt_id='" + _pt_id + "'"; }
            if (_department != null)
            { sql += " AND exam.department='" + _department + "'"; }
            if (_operator != null)
            {
                if (_op1_5)
                {
                    sql += " AND (exam.operator1='" + _operator
                        + "' OR exam.operator2='" + _operator
                        + "' OR exam.operator3='" + _operator
                        + "' OR exam.operator4='" + _operator
                        + "' OR exam.operator5='" + _operator + "')";
                }
                else
                { sql += " AND exam.operator1='" + _operator + "'"; }
            }

            sql += " ORDER BY exam_id";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
            da.Fill(exam_list);
        }

        private void resizeColumns()
        {
            foreach (DataGridViewColumn dc in dgvExamList.Columns)
            { dc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
        }

        private void btClose_Click(object sender, EventArgs e)
        { this.Close(); }

        private void dgvExamList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            //Open editing form.
            if (dgv.Columns[e.ColumnIndex].Name == "btSelect")
            {
                EditFindings ef = new EditFindings(dgv.Rows[e.RowIndex].Cells["exam_id"].Value.ToString());
                ef.ShowDialog(this);
                ef.Dispose();
                exam_list.Rows.Clear();
                searchExam(dateFrom, dateTo, pt_id, department, operator1, op1_5);
                resizeColumns();
                return;
            }
            else if (dgv.Columns[e.ColumnIndex].Name == "btImage")
            {
                if (System.IO.File.Exists(Application.StartupPath + @"\PtJpgViewer\PtJpgViewer.exe"))
                {
                    System.Diagnostics.Process.Start(Application.StartupPath + @"\PtJpgViewer\PtJpgViewer.exe", dgv.Rows[e.RowIndex].Cells["pt_id"].Value.ToString());
                }
                return;
            }
            else if (dgv.Columns[e.ColumnIndex].Name == "btDelColumn")
            {
                Exam exam = new Exam(dgv.Rows[e.RowIndex].Cells["exam_id"].Value.ToString());

                //If findings was blank, delete exam.
                //If findings was not blank, make exam invisible.
                if (MessageBox.Show(Properties.Resources.ConfirmDel, "Information", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (exam.findings.Length == 0)
                    { exam.delExam(); }
                    else
                    { exam.makeInvisible(); }

                    exam_list.Rows.Clear();
                    searchExam(dateFrom, dateTo, pt_id, department, operator1, op1_5);
                    resizeColumns();
                }
                //exam_visibleがfalseのexamを復活させる管理者用のメニューを作成。
                //日付の範囲指定できるように。
            }
            else if (dgv.Columns[e.ColumnIndex].Name == "btPrint")
            {
                #region Error Check
                if (!System.IO.File.Exists(Application.StartupPath+@"\result.html"))
                {
                    MessageBox.Show("[Result template file]" + Properties.Resources.FileNotExist, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion
                ExamResult er = new ExamResult(dgv.Rows[e.RowIndex].Cells["exam_id"].Value.ToString());
                er.ShowDialog(this);
                er.Dispose();
            }
        }

        private void btBlankDraft_Click(object sender, EventArgs e)
        { exam_list.DefaultView.RowFilter = "exam_status < 2"; }//Show only blank/draft findings.

        private void btDone_Click(object sender, EventArgs e)
        { exam_list.DefaultView.RowFilter = "exam_status = 2"; }

        private void btChecked_Click(object sender, EventArgs e)
        { exam_list.DefaultView.RowFilter = "exam_status = 3"; }

        private void btCanceled_Click(object sender, EventArgs e)
        { exam_list.DefaultView.RowFilter = "exam_status = 9"; }

        private void btShowAll_Click(object sender, EventArgs e)
        { exam_list.DefaultView.RowFilter = "exam_status < 10"; }
    }
}