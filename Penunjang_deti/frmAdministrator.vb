Imports System.Data.OleDb
Public Class frmAdministrator
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
            da = New OleDbDataAdapter("select * from tbl_administrator", Conn)
            da.Fill(dt)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
    Sub LoadData()
        Connect()
        sql = "select username_administrator from tbl_administrator"
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
    Sub pembersih()
        cboID.Text = "Pilih ID Admin :"
        txtUsername.Clear()
        txtPassword.Clear()
        txtNamaAdmin.Clear()
        txtEmail.Text = "Pilih Jenis Kelamin :"
    End Sub
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Me.Hide()
        frmMain.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        'Me.Show()
        ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = True
    End Sub
    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Me.WindowState = FormWindowState.Minimized
        e.Cancel = True
    End Sub
    Private Sub frmAdministrator_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        koneksikan()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If txtUsername.Text = "" Then
            MsgBox("Tidak boleh kosong.", vbExclamation, "Error")
        Else
            Connect()
            Dim insertquery As String
            Dim Hasil As Integer
            Dim cmd As OleDbCommand
            insertquery = ("insert into tbl_administrator(username_administrator,password_administrator,nama_administrator,email_administrator)Values('" & txtUsername.Text & "','" & txtPassword.Text & "','" & txtNamaAdmin.Text & "','" & txtEmail.Text & "')")
            Try
                cmd = New OleDbCommand(insertquery, Conn)
                Hasil = cmd.ExecuteNonQuery
                If Hasil > 0 Then
                    MessageBox.Show("Data berhasil dimasukan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    koneksikan()
                    pembersih()
                End If
            Catch ex As OleDbException
                MessageBox.Show("Failed : " & ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Closedd()
        End If
    End Sub

    Private Sub txtEmail_TextChanged(sender As Object, e As EventArgs) Handles txtEmail.TextChanged

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Connect()
        Dim hasil As Integer
        sql = "Update tbl_administrator set password_administrator='" & txtPassword.Text & "',nama_administrator='" & txtNamaAdmin.Text & "', email_administrator='" & txtEmail.Text & "' where username_administrator='" & cboID.Text & "'"
        cmd = New OleDbCommand(sql, Conn)
        Try
            hasil = cmd.ExecuteNonQuery
            If (hasil > 0) Then
                MessageBox.Show("Data berhasil diedit.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                koneksikan()
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
        sql = "delete from tbl_administrator where username_administrator='" & cboID.Text & "'"
        pesan = MessageBox.Show("Hapus data " & Chr(10) & "dengan username : " & cboID.Text & " ...?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        cmd = New OleDbCommand(sql, Conn)
        Try
            If pesan = DialogResult.Yes = True Then
                hasil = cmd.ExecuteNonQuery
                koneksikan()
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
        sql = "select * from tbl_administrator where username_administrator='" & Kunci & "'"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader
        Try
            reader.Read()
            txtUsername.Text = reader.Item("username_administrator")
            txtNamaAdmin.Text = reader.Item("nama_administrator")
            txtPassword.Text = reader.Item("password_administrator")
            txtEmail.Text = reader.Item("email_administrator")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
End Class