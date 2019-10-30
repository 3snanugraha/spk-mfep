Imports System.Data.OleDb
Public Class frmLogin
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
    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtUsername.Focus()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If txtUsername.Text = "" And txtPassword.Text = "" Then
            lblStatus.Text = "Username/password tidak boleh kosong."
        Else
            Call Koneksi()
            cmd = New OleDbCommand("select * from tbl_administrator where username_administrator='" & txtUsername.Text & "' and password_administrator='" & txtPassword.Text & "'", Conn)
            rd = cmd.ExecuteReader
            rd.Read()
            If rd.HasRows Then
                Me.Hide()
                frmMain.Show()
                frmMain.lblUser.Text = rd.Item("nama_administrator")
            Else
                lblStatus.Text = "Username/password belum cocok."
            End If
        End If
    End Sub

    Private Sub txtUsername_TextChanged(sender As Object, e As EventArgs) Handles txtUsername.TextChanged

    End Sub
    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(13) Then Call btnLogin_Click(sender, e)
    End Sub
End Class
