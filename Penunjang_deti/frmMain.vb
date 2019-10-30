Imports System.Data.OleDb
Public Class frmMain
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
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub PictureBox6_Click(sender As Object, e As EventArgs) Handles PictureBox6.Click
        frmLogin.Show()
        frmLogin.txtUsername.Clear()
        frmLogin.txtPassword.Clear()
        frmLogin.lblStatus.Text = "..."
        frmLogin.txtUsername.Focus()
        Me.Close()
    End Sub
    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click
        frmLogin.Show()
        frmLogin.txtUsername.Clear()
        frmLogin.txtPassword.Clear()
        frmLogin.lblStatus.Text = "..."
        frmLogin.txtUsername.Focus()
        Me.Close()
    End Sub
    Private Sub Label13_Click(sender As Object, e As EventArgs) Handles Label13.Click
        frmLogin.Show()
        frmLogin.txtUsername.Clear()
        frmLogin.txtPassword.Clear()
        frmLogin.lblStatus.Text = "..."
        frmLogin.txtUsername.Focus()
        Me.Close()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click

    End Sub

    Private Sub KeluarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KeluarToolStripMenuItem.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click
        frmLogin.Show()
        frmLogin.txtUsername.Clear()
        frmLogin.txtPassword.Clear()
        frmLogin.lblStatus.Text = "..."
        frmLogin.txtUsername.Focus()
        Me.Close()
    End Sub

    Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            NotifyIcon1.Visible = True
            NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
            NotifyIcon1.BalloonTipTitle = "Aplikasi MFEP"
            NotifyIcon1.BalloonTipText = "Double-click untuk membuka"
            NotifyIcon1.ShowBalloonTip(50000)
            'Me.Hide()
            ShowInTaskbar = False
        End If
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

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        frmAdministrator.Show()
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        frmAdministrator.Show()
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click
        frmAdministrator.Show()
    End Sub

    Private Sub Label20_Click(sender As Object, e As EventArgs) Handles lbl1.Click, lbl3.Click, lbl2.Click

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If lbl1.ForeColor = Color.Black Then

        End If
    End Sub

    Private Sub PictureBox9_Click(sender As Object, e As EventArgs) Handles PictureBox9.Click
        frmLaporan.Show()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        frmDataSiswa.Show()
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click
        frmDataSiswa.Show()
    End Sub
    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        frmDataSiswa.Show()
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        frmMFEP.Show()
    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click
        frmMFEP.Show()
    End Sub
    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        frmMFEP.Show()
    End Sub

    Private Sub PictureBox8_Click(sender As Object, e As EventArgs) Handles PictureBox8.Click
        frmKriteria.Show()
    End Sub
End Class