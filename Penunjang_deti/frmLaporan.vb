Imports System.Data.OleDb
Public Class frmLaporan
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
            da = New OleDbDataAdapter("select * from tbl_mfep", Conn)
            da.Fill(dt)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
    Sub LoadData_mfep()
        Connect()
        sql = "select kd_mfep from tbl_mfep"
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
            LoadData_mfep()
            Tampil()
            Conn.Close()
        Catch ex As Exception
            MessageBox.Show("Koneksi Error : " + ex.Message)
        End Try
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
    Private Sub frmLaporan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        koneksikan()
    End Sub
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Me.Hide()
        frmMain.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        frmLaporanCetak.Show()
    End Sub
End Class