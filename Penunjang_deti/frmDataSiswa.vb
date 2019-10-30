Imports System.Data.OleDb
Public Class frmDataSiswa
    Const WM_NCHITTEST As Integer = &H84
    Const HTCLIENT As Integer = &H1
    Const HTCAPTION As Integer = &H2
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Select Case m.Msg
            Case WM_NCHITTEST
                MyBase.WndProc(m)
                If m.Result = IntPtr.op_Explicit(HTCLIENT) Then m.Result = IntPtr.op_Explicit(HTCAPTION)
            Case Else
                MyBase.WndProc(m)
        End Select
    End Sub
    Private Conn As OleDbConnection = Nothing
    Private cmd As OleDbCommand = Nothing
    Private sql As String = Nothing
    Private reader As OleDbDataReader = Nothing
    Private da As OleDbDataAdapter = Nothing
    Function Connect()
        If Not Conn Is Nothing Then
            Conn.Close()
        End If
        Conn.Open()
        Return Conn
    End Function
    Function Closedd()
        Conn.Close()
        Return Conn
    End Function
    Sub Tampil()
        Connect()
        Try
            Dim dt As New DataTable
            da = New OleDbDataAdapter("select * from tbl_siswa", Conn)
            da.Fill(dt)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
    Sub LoadData()
        Connect()
        sql = "select kd_siswa from tbl_siswa"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader()
        Try
            cboID.Items.Clear()
            While reader.Read
                cboID.Items.Add(reader.GetString(0))
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
    Sub koneksikan()
        Dim ConnString As String
        ConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Application.StartupPath & "\db_spk.accdb"
        Try
            Conn = New OleDbConnection(ConnString)
            Conn.Open()
            LoadData()
            Tampil()
            Conn.Close()
        Catch ex As Exception
            MessageBox.Show("Koneksi Error : " + ex.Message)
        End Try
    End Sub
    Sub nomoran()
        koneksikan()
        Connect()
        cmd = New OleDbCommand("select * from tbl_siswa order by kd_siswa desc", Conn)
        reader = cmd.ExecuteReader
        reader.Read()
        If Not reader.HasRows Then
            lblID.Text = "SW" + "0001"
        Else
            lblID.Text = Val(Microsoft.VisualBasic.Mid(reader.Item("kd_siswa").ToString, 4, 3)) + 1
            If Len(lblID.Text) = 1 Then
                lblID.Text = "SW000" & lblID.Text & ""
            ElseIf Len(lblID.Text) = 2 Then
                lblID.Text = "SW00" & lblID.Text & ""
            ElseIf Len(lblID.Text) = 3 Then
                lblID.Text = "SW0" & lblID.Text & ""
            End If
        End If
        Me.Focus()
    End Sub
    Sub pembersih()
        cboID.Text = "Pilih Kode Siswa :"
        txtNamaSiswa.Clear()
        cboJenisKelamin.Text = "Pilih Jenis Kelamin :"
        txtAlamat.Clear()
        txtEmail.Clear()
        txtNoHP.Clear()
    End Sub
    Private Sub frmDataSiswa_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        nomoran()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Me.Hide()
        frmMain.Show()
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Connect()
        Dim insertquery As String
        Dim Hasil As Integer
        Dim cmd As OleDbCommand
        insertquery = ("insert into tbl_siswa(kd_siswa,nama_siswa,jeniskelamin_siswa,email_siswa,tgllahir_siswa,tempatlahir_siswa,nohp_siswa,alamat_siswa)Values('" & lblID.Text & "','" & txtNamaSiswa.Text & "','" & cboJenisKelamin.Text & "','" & txtEmail.Text & "','" & dtpTglLahir.Text & "','" & txtTempatLahir.Text & "','" & txtNoHP.Text & "','" & txtAlamat.Text & "')")
        Try
            cmd = New OleDbCommand(insertquery, Conn)
            Hasil = cmd.ExecuteNonQuery
            If Hasil > 0 Then
                MessageBox.Show("Data berhasil dimasukan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                nomoran()
                pembersih()
            End If
        Catch ex As OleDbException
            MessageBox.Show("Failed : " & ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Closedd()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Connect()
        Dim hasil As Integer
        sql = "Update tbl_siswa set nama_siswa='" & txtNamaSiswa.Text & "',jeniskelamin_siswa='" & cboJenisKelamin.Text & "', email_siswa='" & txtEmail.Text & "', tgllahir_siswa='" & dtpTglLahir.Text & "', tempatlahir_siswa='" & txtTempatLahir.Text & "', nohp_siswa='" & txtNoHP.Text & "', alamat_siswa='" & txtAlamat.Text & "' where kd_siswa='" & cboID.Text & "'"
        cmd = New OleDbCommand(sql, Conn)
        Try
            hasil = cmd.ExecuteNonQuery
            If (hasil > 0) Then
                MessageBox.Show("Data berhasil diedit.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                nomoran()
                pembersih()
            End If
        Catch ex As OleDbException
            MessageBox.Show("Failed : " & ex.Message)
        End Try
        Closedd()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Connect()
        Dim index As Integer = cboID.SelectedIndex
        Dim hasil As Integer
        Dim pesan As DialogResult
        sql = "delete from tbl_siswa where kd_siswa='" & cboID.Text & "'"
        pesan = MessageBox.Show("Hapus data " & Chr(10) & "dengan Kode Siswa : " & cboID.Text & " ...?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        cmd = New OleDbCommand(sql, Conn)
        Try
            If pesan = DialogResult.Yes = True Then
                hasil = cmd.ExecuteNonQuery
                nomoran()
                pembersih()
            End If
        Catch ex As OleDbException
            MsgBox("Failed : " & ex.Message)
        End Try
        Closedd()
    End Sub
    Private Sub cboID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboID.SelectedIndexChanged
        Connect()
        Dim Kunci As String = cboID.Text
        sql = "select * from tbl_siswa where kd_siswa='" & Kunci & "'"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader
        Try
            reader.Read()
            txtNamaSiswa.Text = reader.Item("nama_siswa")
            cboJenisKelamin.Text = reader.Item("jeniskelamin_siswa")
            txtEmail.Text = reader.Item("email_siswa")
            dtpTglLahir.Text = reader.Item("tgllahir_siswa")
            txtTempatLahir.Text = reader.Item("tempatlahir_siswa")
            txtNoHP.Text = reader.Item("nohp_siswa")
            txtAlamat.Text = reader.Item("alamat_siswa")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
End Class