Imports System.Data.OleDb
Public Class frmKriteria
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
            da = New OleDbDataAdapter("select * from tbl_intensitas", Conn)
            da.Fill(dt)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub
    Sub LoadData()
        Connect()
        sql = "select kd_intensitas from tbl_intensitas"
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
        cmd = New OleDbCommand("select * from tbl_intensitas order by kd_intensitas desc", Conn)
        reader = cmd.ExecuteReader
        reader.Read()
        If Not reader.HasRows Then
            lblID.Text = "IN" + "0001"
        Else
            lblID.Text = Val(Microsoft.VisualBasic.Mid(reader.Item("kd_intensitas").ToString, 4, 3)) + 1
            If Len(lblID.Text) = 1 Then
                lblID.Text = "IN000" & lblID.Text & ""
            ElseIf Len(lblID.Text) = 2 Then
                lblID.Text = "IN00" & lblID.Text & ""
            ElseIf Len(lblID.Text) = 3 Then
                lblID.Text = "IN0" & lblID.Text & ""
            End If
        End If
        Me.Focus()
    End Sub
    Sub pembersih()
        cboID.Text = "Pilih Kode Kriteria :"
        txtNamaKriteria.Clear()
        nAkademik.Value = "0"
        nPemahaman.Value = "0"
        nSikap.Value = "0"
        nEkstrakulikuler.Value = "0"
        nAbsen.Value = "0"
    End Sub
    Private Sub frmKriteria_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        nomoran()
    End Sub

    Private Sub cboID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboID.SelectedIndexChanged
        Connect()
        Dim Kunci As String = cboID.Text
        sql = "select * from tbl_intensitas where kd_intensitas='" & Kunci & "'"
        cmd = New OleDbCommand(sql, Conn)
        reader = cmd.ExecuteReader
        Try
            reader.Read()
            txtNamaKriteria.Text = reader.Item("nama_intensitas")
            nAkademik.Value = reader.Item("nilaiakademik_intensitas")
            nPemahaman.Value = reader.Item("nilaipemahaman_intensitas")
            nSikap.Value = reader.Item("nilaisikap_intensitas")
            nEkstrakulikuler.Value = reader.Item("nilaiekstrakulikuler_intensitas")
            nAbsen.Value = reader.Item("nilaiabsen_intensitas")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Closedd()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Connect()
        Dim hasil As Integer
        sql = "Update tbl_intensitas set nama_intensitas='" & txtNamaKriteria.Text & "',nilaiakademik_intensitas='" & nAkademik.Value & "', nilaipemahaman_intensitas='" & nPemahaman.Value & "', nilaisikap_intensitas='" & nSikap.Value & "', nilaiekstrakulikuler_intensitas='" & nEkstrakulikuler.Value & "', nilaiabsen_intensitas='" & nAbsen.Value & "' where kd_intensitas='" & cboID.Text & "'"
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
        sql = "delete from tbl_intensitas where kd_intensitas='" & cboID.Text & "'"
        pesan = MessageBox.Show("Hapus data " & Chr(10) & "dengan Kode Intensitas : " & cboID.Text & " ...?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
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
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'txtNamaKriteria.Text = reader.Item("nama_intensitas")
        'nAkademik.Value = reader.Item("nilaiakademik_intensitas")
        'nPemahaman.Value = reader.Item("nilaipemahaman_intensitas")
        'nSikap.Value = reader.Item("nilaisikap_intensitas")
        'nEkstrakulikuler.Value = reader.Item("nilaiekstrakulikuler_intensitas")
        'nAbsen.Value = reader.Item("nilaiabsen_intensitas")
        Connect()
        Dim insertquery As String
        Dim Hasil As Integer
        Dim cmd As OleDbCommand
        insertquery = ("insert into tbl_intensitas(kd_intensitas,nama_intensitas,nilaiakademik_intensitas,nilaipemahaman_intensitas,nilaisikap_intensitas,nilaiekstrakulikuler_intensitas,nilaiabsen_intensitas)Values('" & lblID.Text & "','" & txtNamaKriteria.Text & "','" & nAkademik.Value & "','" & nPemahaman.Value & "','" & nSikap.Value & "','" & nEkstrakulikuler.Value & "','" & nAbsen.Value & "')")
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
End Class