Imports System.Data.SqlClient
Public Class Form1
    Dim cmd_gl As SqlCommand    Dim dr_gl As SqlDataReader
    Dim query_gl As String

    Private Sub BtSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtSave.Click
        Dim query As String
        Dim cmd As SqlCommand
        Dim dr As SqlDataReader
        Dim uid As String
        Dim cekStok As Boolean = False
       
        Call OpenKoneksi()
        Try
            Dim query_id As String = "SELECT MAX(NoId)+1 AS MXID FROM pengajuan"
            cmd = New SqlCommand(query_id, conn)
            dr = cmd.ExecuteReader
            dr.Read()
            uid = dr("MXID")
            dr.Close()

            query = "INSERT INTO pengajuan (NoId,NamaPelaksana,Bidang,JumlahPermintaan,BentukDana,Keperluan,Tanggal) VALUES (" & uid & ",'" & TBnama.Text & "','" & Cbbidang.Text & "','" & Tbpermintaan.Text & "','" & Tbbentuk.Text & "','" & TBkeperluan.Text & "','" & DateTimePicker1.Text & "')"
            cmd = New SqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dr.Close()
            LV1.Clear()
            hapus()
            tampil()


            MsgBox("Data pengajuan '" + TBnama.Text + "' Berhasil Tersimpan")
            Call CloseKoneksi()

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        Tbbentuk.Text = "Tunai"
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        Tbbentuk.Text = "Cek Bank"
    End Sub

    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        Tbbentuk.Text = "Giro"
    End Sub

    Private Sub tampil()
        Dim cmd As SqlCommand
        Dim dr As SqlDataReader


        LV1.View = View.Details
        LV1.Columns.Add("No", 50)
        LV1.Columns.Add("Nama Pelaksana", 300)
        LV1.Columns.Add("Bidang", 150)
        LV1.Columns.Add("Jumlah Permintaan", 200)
        LV1.Columns.Add("Bentuk Dana", 200)
        LV1.Columns.Add("Keperluan", 200)



        Call OpenKoneksi()
        Dim query_id As String = "select * from pengajuan"
        cmd = New SqlCommand(query_id, conn)
        dr = cmd.ExecuteReader

        Do While dr.Read
            LV1.Items.Add(dr("NoId"))
            LV1.Items(LV1.Items.Count - 1).SubItems.Add(dr("NamaPelaksana"))
            LV1.Items(LV1.Items.Count - 1).SubItems.Add(dr("Bidang"))
            LV1.Items(LV1.Items.Count - 1).SubItems.Add(dr("JumlahPermintaan"))
            LV1.Items(LV1.Items.Count - 1).SubItems.Add(dr("BentukDana"))
            LV1.Items(LV1.Items.Count - 1).SubItems.Add(dr("Keperluan"))



        Loop
        dr.Close()
        Call CloseKoneksi()

    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        tampil()

        
        


    End Sub

    Private Sub hapus()
        TBnama.Text = ""
        TBkeperluan.Text = ""
        Cbbidang.Text = ""
        Tbpermintaan.Text = ""
        Tbbentuk.Text = ""
        RadioButton1.Checked = False
        RadioButton2.Checked = False
        RadioButton3.Checked = False

    End Sub

  
    
    Private Sub LV1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles LV1.SelectedIndexChanged
        btedit.Enabled = True
    End Sub

    Private Sub btedit_Click(sender As System.Object, e As System.EventArgs) Handles btedit.Click
        Dim cmd As SqlCommand
        Dim dr As SqlDataReader
        Dim idorg As String


        Try
            idorg = LV1.SelectedItems(0).Text
            Call OpenKoneksi()
            Dim query As String = "select * from pengajuan where NoId='" + idorg + "'"

            cmd = New SqlCommand(query, conn)
            dr = cmd.ExecuteReader

            dr.Read()

            tbid.Text = dr("NoId")
            TBnama.Text = dr("NamaPelaksana")
            Cbbidang.Text = dr("Bidang")
            Tbpermintaan.Text = dr("JumlahPermintaan")
            TBkeperluan.Text = dr("Keperluan")
            Tbbentuk.Text = dr("BentukDana")



            If Tbbentuk.Text = "Tunai" Then
                RadioButton1.Checked = True
            ElseIf Tbbentuk.Text = "Cek Bank" Then
                RadioButton2.Checked = True
            ElseIf Tbbentuk.Text = "Giro" Then
                RadioButton3.Checked = True
            End If
            dr.Close()
            Call CloseKoneksi()
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub


    
    Private Sub Btapus_Click(sender As System.Object, e As System.EventArgs) Handles Btapus.Click
        Dim Idorg As String
        Dim cmd As SqlCommand        Dim dr As SqlDataReader

        Try
            Idorg = LV1.SelectedItems(0).Text

            Call OpenKoneksi()
            Dim query As String = "delete from pengajuan where NoId='" + Idorg + "'"            cmd = New SqlCommand(query, conn)            dr = cmd.ExecuteReader
            Call CloseKoneksi()
            dr.Close()
            MsgBox("data berhasil di hapus")
            LV1.Clear()
            tampil()

        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    Private Sub Btupdate_Click(sender As System.Object, e As System.EventArgs) Handles Btupdate.Click

        Try
            Call OpenKoneksi()
            query_gl = "UPDATE pengajuan SET NamaPelaksana = '" & TBnama.Text & "',Bidang ='" & Cbbidang.Text & "',JumlahPermintaan = '" & Tbpermintaan.Text & "',BentukDana = '" & Tbbentuk.Text & "',Keperluan = '" & TBkeperluan.Text & "' WHERE NoId = '" & tbid.Text & "'"
            cmd_gl = New SqlCommand(query_gl, conn)            dr_gl = cmd_gl.ExecuteReader
            dr_gl.Close()
            Call CloseKoneksi()
            hapus()
            LV1.Clear()
            tampil()

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class
