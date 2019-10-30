Imports System.Data.OleDb
Public Class frmMFEP
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
    Sub LoadData_intensitas()
        Connect()
        sql = "select kd_intensitas from tbl_intensitas"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader()
        Try
            cboIDKriteria.Items.Clear()
            While reader.Read
                cboIDKriteria.Items.Add(reader.GetString(0))
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
    Sub LoadData_siswa()
        Connect()
        sql = "select kd_siswa from tbl_siswa"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader()
        Try
            cboIDSiswa.Items.Clear()
            While reader.Read
                cboIDSiswa.Items.Add(reader.GetString(0))
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
            LoadData_intensitas()
            LoadData_siswa()
            LoadData_mfep()
            Tampil()
            Conn.Close()
        Catch ex As Exception
            MessageBox.Show("Koneksi Error : " + ex.Message)
        End Try
    End Sub
    Sub nomoran()
        Connect()
        cmd = New OleDbCommand("select * from tbl_mfep order by kd_mfep desc", Conn)
        reader = cmd.ExecuteReader
        reader.Read()
        If Not reader.HasRows Then
            lblID.Text = "MF" + "0001"
        Else
            lblID.Text = Val(Microsoft.VisualBasic.Mid(reader.Item("kd_mfep").ToString, 4, 3)) + 1
            If Len(lblID.Text) = 1 Then
                lblID.Text = "MF000" & lblID.Text & ""
            ElseIf Len(lblID.Text) = 2 Then
                lblID.Text = "MF00" & lblID.Text & ""
            ElseIf Len(lblID.Text) = 3 Then
                lblID.Text = "MF0" & lblID.Text & ""
            End If
        End If
        Me.Focus()
    End Sub
    Private Sub frmMFEP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        koneksikan()
        nomoran()
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
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Me.Close()
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub lblNamaSiswa_Click(sender As Object, e As EventArgs) Handles lblNamaSiswa.Click

    End Sub

    Private Sub Label28_Click(sender As Object, e As EventArgs) Handles Label28.Click

    End Sub
    Private Sub Label27_Click(sender As Object, e As EventArgs) Handles Label27.Click

    End Sub
    Private Sub cboIDKriteria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboIDKriteria.SelectedIndexChanged
        Connect()
        Dim Kunci As String = cboIDKriteria.Text
        sql = "select * from tbl_intensitas where kd_intensitas='" & Kunci & "'"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader
        Try
            reader.Read()
            lblNamaIntensitas.Text = reader.Item("nama_intensitas")
            lblIntenAkademik.Text = reader.Item("nilaiakademik_intensitas")
            lblIntenPemahaman.Text = reader.Item("nilaipemahaman_intensitas")
            lblIntenSikap.Text = reader.Item("nilaisikap_intensitas")
            lblIntenAbsen.Text = reader.Item("nilaiabsen_intensitas")
            lblIntenEkstrakulikuler.Text = reader.Item("nilaiekstrakulikuler_intensitas")
            lblJumlahNilaiIntens.Text = Val(lblIntenAkademik.Text) + Val(lblIntenPemahaman.Text) + Val(lblIntenSikap.Text) + Val(lblIntenEkstrakulikuler.Text) + Val(lblIntenAbsen.Text)
            lblTotalIntens.Text = lblJumlahNilaiIntens.Text

            Dim x, y, z, a, b, c As Double
            x = Val(lblIntenAkademik.Text) / Val(lblTotalIntens.Text)
            y = Val(lblIntenPemahaman.Text) / Val(lblTotalIntens.Text)
            z = Val(lblIntenSikap.Text) / Val(lblTotalIntens.Text)
            a = Val(lblIntenEkstrakulikuler.Text) / Val(lblTotalIntens.Text)
            b = Val(lblIntenAbsen.Text) / Val(lblTotalIntens.Text)
            'c = Val(efAkademik.Text) + Val(efPemahaman.Text) + Val(efSikap.Text) + Val(efEkstrakulikuler.Text) + Val(efNilaiAbsen.Text)
            c = x + y + z + a + b
            lblTotalIntens2.Text = c.ToString
            efAkademik.Text = x.ToString
            efPemahaman.Text = y.ToString
            efSikap.Text = z.ToString
            efEkstrakulikuler.Text = a.ToString
            efNilaiAbsen.Text = b.ToString

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
    Private Sub cboIDSiswa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboIDSiswa.SelectedIndexChanged
        Connect()
        Dim Kunci As String = cboIDSiswa.Text
        sql = "select * from tbl_siswa where kd_siswa='" & Kunci & "'"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader
        Try
            reader.Read()
            lblNamaSiswa.Text = reader.Item("nama_siswa")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
    Private Sub cboID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboID.SelectedIndexChanged
        Connect()
        Dim Kunci As String = cboID.Text
        sql = "select * from tbl_mfep where kd_mfep='" & Kunci & "'"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader
        Try
            reader.Read()
            cboIDKriteria.Text = reader.Item("kd_intensitas")
            cboIDSiswa.Text = reader.Item("kd_siswa")
            lblNamaMFEP.Text = reader.Item("nama_mfep")
            nAkhir.Text = reader.Item("nilai_akhir")
        Catch ex As Exception
            'messagebox.show(ex.message)
        End Try
        Closedd()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Connect()
        Dim insertquery As String
        Dim Hasil As Integer
        Dim cmd As OleDbCommand
        insertquery = ("insert into tbl_mfep(kd_mfep,kd_siswa,kd_intensitas,nama_mfep,nama_siswa,nilai_akhir)Values('" & lblID.Text & "','" & cboIDSiswa.Text & "','" & cboIDKriteria.Text & "','" & lblNamaMFEP.Text & "','" & lblNamaSiswa.Text & "','" & nAkhir.Text & "')")
        Try
            cmd = New OleDbCommand(insertquery, Conn)
            Hasil = cmd.ExecuteNonQuery
            If Hasil > 0 Then
                MessageBox.Show("Data berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                koneksikan()
                nomoran()
            End If
        Catch ex As OleDbException
            MessageBox.Show("Failed : " & ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Closedd()
    End Sub

    Private Sub lblNamaMFEP_TextChanged(sender As Object, e As EventArgs) Handles lblNamaMFEP.TextChanged

    End Sub

    Private Sub nAkademik_ValueChanged(sender As Object, e As EventArgs) Handles nAkademik.ValueChanged
        Dim x, y, z, a, b, c, varx, vary, varz, vara, varb, varc As Double
        x = efAkademik.Text
        y = efPemahaman.Text
        z = efSikap.Text
        a = efEkstrakulikuler.Text
        b = efNilaiAbsen.Text
        varx = nAkademik.Value * x
        vary = nPemahaman.Value * y
        varz = nSikap.Value * z
        vara = nEkstrakulikuler.Value * a
        varb = nAbsen.Value * b
        varc = varx + vary + varz + vara + varb
        nAkhir.Text = varc
        lblNilaiAkademik_hasil.Text = varx
        lblNilaiPemahaman_hasil.Text = vary
        lblNilaiSikap_hasil.Text = varz
        lblEkstrakulikuler_hasil.Text = vara
        lblNilaiAbsen_hasil.Text = varb
    End Sub

    Private Sub nPemahaman_ValueChanged(sender As Object, e As EventArgs) Handles nPemahaman.ValueChanged
        Dim x, y, z, a, b, c, varx, vary, varz, vara, varb, varc As Double
        x = efAkademik.Text
        y = efPemahaman.Text
        z = efSikap.Text
        a = efEkstrakulikuler.Text
        b = efNilaiAbsen.Text
        varx = nAkademik.Value * x
        vary = nPemahaman.Value * y
        varz = nSikap.Value * z
        vara = nEkstrakulikuler.Value * a
        varb = nAbsen.Value * b
        varc = varx + vary + varz + vara + varb
        nAkhir.Text = varc
        lblNilaiAkademik_hasil.Text = varx
        lblNilaiPemahaman_hasil.Text = vary
        lblNilaiSikap_hasil.Text = varz
        lblEkstrakulikuler_hasil.Text = vara
        lblNilaiAbsen_hasil.Text = varb
    End Sub

    Private Sub nSikap_ValueChanged(sender As Object, e As EventArgs) Handles nSikap.ValueChanged
        Dim x, y, z, a, b, c, varx, vary, varz, vara, varb, varc As Double
        x = efAkademik.Text
        y = efPemahaman.Text
        z = efSikap.Text
        a = efEkstrakulikuler.Text
        b = efNilaiAbsen.Text
        varx = nAkademik.Value * x
        vary = nPemahaman.Value * y
        varz = nSikap.Value * z
        vara = nEkstrakulikuler.Value * a
        varb = nAbsen.Value * b
        varc = varx + vary + varz + vara + varb
        nAkhir.Text = varc
        lblNilaiAkademik_hasil.Text = varx
        lblNilaiPemahaman_hasil.Text = vary
        lblNilaiSikap_hasil.Text = varz
        lblEkstrakulikuler_hasil.Text = vara
        lblNilaiAbsen_hasil.Text = varb
    End Sub

    Private Sub nEkstrakulikuler_ValueChanged(sender As Object, e As EventArgs) Handles nEkstrakulikuler.ValueChanged
        Dim x, y, z, a, b, c, varx, vary, varz, vara, varb, varc As Double
        x = efAkademik.Text
        y = efPemahaman.Text
        z = efSikap.Text
        a = efEkstrakulikuler.Text
        b = efNilaiAbsen.Text
        varx = nAkademik.Value * x
        vary = nPemahaman.Value * y
        varz = nSikap.Value * z
        vara = nEkstrakulikuler.Value * a
        varb = nAbsen.Value * b
        varc = varx + vary + varz + vara + varb
        nAkhir.Text = varc
        lblNilaiAkademik_hasil.Text = varx
        lblNilaiPemahaman_hasil.Text = vary
        lblNilaiSikap_hasil.Text = varz
        lblEkstrakulikuler_hasil.Text = vara
        lblNilaiAbsen_hasil.Text = varb
    End Sub

    Private Sub nAbsen_ValueChanged(sender As Object, e As EventArgs) Handles nAbsen.ValueChanged
        Dim x, y, z, a, b, c, varx, vary, varz, vara, varb, varc As Double
        x = efAkademik.Text
        y = efPemahaman.Text
        z = efSikap.Text
        a = efEkstrakulikuler.Text
        b = efNilaiAbsen.Text
        varx = nAkademik.Value * x
        vary = nPemahaman.Value * y
        varz = nSikap.Value * z
        vara = nEkstrakulikuler.Value * a
        varb = nAbsen.Value * b
        varc = varx + vary + varz + vara + varb
        nAkhir.Text = varc
        lblNilaiAkademik_hasil.Text = varx
        lblNilaiPemahaman_hasil.Text = vary
        lblNilaiSikap_hasil.Text = varz
        lblEkstrakulikuler_hasil.Text = vara
        lblNilaiAbsen_hasil.Text = varb
    End Sub

    Private Sub nAbsen_TextChanged(sender As Object, e As EventArgs) Handles nAbsen.TextChanged
        Dim x, y, z, a, b, c, varx, vary, varz, vara, varb, varc As Double
        x = efAkademik.Text
        y = efPemahaman.Text
        z = efSikap.Text
        a = efEkstrakulikuler.Text
        b = efNilaiAbsen.Text
        varx = nAkademik.Value * x
        vary = nPemahaman.Value * y
        varz = nSikap.Value * z
        vara = nEkstrakulikuler.Value * a
        varb = nAbsen.Value * b
        varc = varx + vary + varz + vara + varb
        nAkhir.Text = varc
        lblNilaiAkademik_hasil.Text = varx
        lblNilaiPemahaman_hasil.Text = vary
        lblNilaiSikap_hasil.Text = varz
        lblEkstrakulikuler_hasil.Text = vara
        lblNilaiAbsen_hasil.Text = varb
    End Sub

    Private Sub nEkstrakulikuler_TextChanged(sender As Object, e As EventArgs) Handles nEkstrakulikuler.TextChanged
        Dim x, y, z, a, b, c, varx, vary, varz, vara, varb, varc As Double
        x = efAkademik.Text
        y = efPemahaman.Text
        z = efSikap.Text
        a = efEkstrakulikuler.Text
        b = efNilaiAbsen.Text
        varx = nAkademik.Value * x
        vary = nPemahaman.Value * y
        varz = nSikap.Value * z
        vara = nEkstrakulikuler.Value * a
        varb = nAbsen.Value * b
        varc = varx + vary + varz + vara + varb
        nAkhir.Text = varc
        lblNilaiAkademik_hasil.Text = varx
        lblNilaiPemahaman_hasil.Text = vary
        lblNilaiSikap_hasil.Text = varz
        lblEkstrakulikuler_hasil.Text = vara
        lblNilaiAbsen_hasil.Text = varb
    End Sub

    Private Sub nSikap_TextChanged(sender As Object, e As EventArgs) Handles nSikap.TextChanged
        Dim x, y, z, a, b, c, varx, vary, varz, vara, varb, varc As Double
        x = efAkademik.Text
        y = efPemahaman.Text
        z = efSikap.Text
        a = efEkstrakulikuler.Text
        b = efNilaiAbsen.Text
        varx = nAkademik.Value * x
        vary = nPemahaman.Value * y
        varz = nSikap.Value * z
        vara = nEkstrakulikuler.Value * a
        varb = nAbsen.Value * b
        varc = varx + vary + varz + vara + varb
        nAkhir.Text = varc
        lblNilaiAkademik_hasil.Text = varx
        lblNilaiPemahaman_hasil.Text = vary
        lblNilaiSikap_hasil.Text = varz
        lblEkstrakulikuler_hasil.Text = vara
        lblNilaiAbsen_hasil.Text = varb
    End Sub

    Private Sub nPemahaman_TextChanged(sender As Object, e As EventArgs) Handles nPemahaman.TextChanged
        Dim x, y, z, a, b, c, varx, vary, varz, vara, varb, varc As Double
        x = efAkademik.Text
        y = efPemahaman.Text
        z = efSikap.Text
        a = efEkstrakulikuler.Text
        b = efNilaiAbsen.Text
        varx = nAkademik.Value * x
        vary = nPemahaman.Value * y
        varz = nSikap.Value * z
        vara = nEkstrakulikuler.Value * a
        varb = nAbsen.Value * b
        varc = varx + vary + varz + vara + varb
        nAkhir.Text = varc
        lblNilaiAkademik_hasil.Text = varx
        lblNilaiPemahaman_hasil.Text = vary
        lblNilaiSikap_hasil.Text = varz
        lblEkstrakulikuler_hasil.Text = vara
        lblNilaiAbsen_hasil.Text = varb
    End Sub

    Private Sub nAkademik_TextChanged(sender As Object, e As EventArgs) Handles nAkademik.TextChanged
        Dim x, y, z, a, b, c, varx, vary, varz, vara, varb, varc As Double
        x = efAkademik.Text
        y = efPemahaman.Text
        z = efSikap.Text
        a = efEkstrakulikuler.Text
        b = efNilaiAbsen.Text
        varx = nAkademik.Value * x
        vary = nPemahaman.Value * y
        varz = nSikap.Value * z
        vara = nEkstrakulikuler.Value * a
        varb = nAbsen.Value * b
        varc = varx + vary + varz + vara + varb
        nAkhir.Text = varc
        lblNilaiAkademik_hasil.Text = varx
        lblNilaiPemahaman_hasil.Text = vary
        lblNilaiSikap_hasil.Text = varz
        lblEkstrakulikuler_hasil.Text = vara
        lblNilaiAbsen_hasil.Text = varb
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Connect()
        Dim index As Integer = cboID.SelectedIndex
        Dim hasil As Integer
        Dim pesan As DialogResult
        sql = "delete from tbl_mfep where kd_mfep='" & cboID.Text & "'"
        pesan = MessageBox.Show("Hapus data " & Chr(10) & "dengan Kode MFEP : " & cboID.Text & " ...?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        cmd = New OleDbCommand(sql, Conn)
        Try
            If pesan = DialogResult.Yes = True Then
                hasil = cmd.ExecuteNonQuery
                koneksikan()
                nomoran()
            End If
        Catch ex As OleDbException
            MsgBox("Failed : " & ex.Message)
        End Try
        Closedd()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Connect()
        Dim hasil As Integer
        sql = "Update tbl_mfep set nama_mfep='" & lblNamaMFEP.Text & "',kd_siswa='" & cboIDSiswa.Text & "', kd_intensitas='" & cboIDKriteria.Text & "', nama_siswa='" & lblNamaSiswa.Text & "', nilai_akhir='" & nAkhir.Text & "' where kd_mfep='" & cboID.Text & "'"
        cmd = New OleDbCommand(sql, Conn)
        Try
            hasil = cmd.ExecuteNonQuery
            If (hasil > 0) Then
                MessageBox.Show("Data berhasil diedit.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                koneksikan()
                nomoran()
            End If
        Catch ex As OleDbException
            MessageBox.Show("Failed : " & ex.Message)
        End Try
        Closedd()
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        frmLaporan.Show()
    End Sub
End Class