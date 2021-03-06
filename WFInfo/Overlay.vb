﻿Imports System.Runtime.InteropServices
Imports System.Drawing.Text
Imports System.Text

Public Class Overlay
    Private InitialStyle As Integer
    Dim PercentVisible As Decimal
    Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
    Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height
    Dim pSize As Point
    Dim pLoc As Point
    Protected Overrides ReadOnly Property CreateParams As System.Windows.Forms.CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H80
            Return cp
        End Get
    End Property
    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UpdateColors(Me)
        PictureBox1.Image = Tint(PictureBox1.Image, My.Settings.cTray, 0.25)
        Me.Location = pLoc
        Me.Size = pSize
    End Sub

    Public Enum GWL As Integer
        ExStyle = -20
    End Enum

    Public Enum WS_EX As Integer
        Transparent = &H20
        Layered = &H80000
    End Enum

    Public Enum LWA As Integer
        ColorKey = &H1
        Alpha = &H2
    End Enum

    <DllImport("user32.dll", EntryPoint:="GetWindowLong")>
    Public Shared Function GetWindowLong(
        ByVal hWnd As IntPtr,
        ByVal nIndex As GWL
            ) As Integer
    End Function

    <DllImport("user32.dll", EntryPoint:="SetWindowLong")>
    Public Shared Function SetWindowLong(
        ByVal hWnd As IntPtr,
        ByVal nIndex As GWL,
        ByVal dwNewLong As WS_EX
            ) As Integer
    End Function

    Private Declare Function GetForegroundWindow Lib "user32" () As Long

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Private Shared Function GetWindowText(hWnd As IntPtr, text As StringBuilder, count As Integer) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Private Shared Function GetWindowTextLength(hWnd As IntPtr) As Integer
    End Function

    <DllImport("user32.dll", EntryPoint:="GetWindowRect")>
    Private Shared Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As Rectangle) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function



    <DllImport("user32.dll",
      EntryPoint:="SetLayeredWindowAttributes")>
    Public Shared Function SetLayeredWindowAttributes(
        ByVal hWnd As IntPtr,
        ByVal crKey As Integer,
        ByVal alpha As Byte,
        ByVal dwFlags As LWA
            ) As Boolean
    End Function

    Private Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Integer,
      ByVal dx As Integer, ByVal dy As Integer, ByVal cButtons As Integer,
      ByVal dwExtraInfo As Integer)

    Private Sub Overlay_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        InitialStyle = GetWindowLong(Me.Handle, GWL.ExStyle)
        PercentVisible = 0.5

        SetWindowLong(Me.Handle, GWL.ExStyle, InitialStyle Or WS_EX.Layered Or WS_EX.Transparent)
        'SetLayeredWindowAttributes(Me.Handle, 0, 255 * PercentVisible, LWA.Alpha)
        Me.BackColor = Color.Black
        Me.TopMost = True
        Me.Refresh()
    End Sub

    Public Sub Display(x As Integer, y As Integer, p As String, d As Integer, Optional v As Boolean = False)
        If v Then
            PictureBox1.Image = My.Resources.Panel_V
        End If
        pLoc = New Point(x, y)
        pSize = New Point(125, 70)
        Dim fontSize As Integer = 0.26 * pSize.Y

        'Platinum Label
        lbPlat.Location = New Point(-2, 0)
        lbPlat.Font = New Font(lbPlat.Font.FontFamily, fontSize, FontStyle.Bold)
        lbPlat.BackColor = Color.Transparent
        lbPlat.Parent = lbPDropShadow
        lbPlat.Text = p

        'Platinum Label Drop Shadow
        lbPDropShadow.Location = New Point((pSize.X / 2.58) + 2, (pSize.Y / 27))
        lbPDropShadow.Font = New Font(lbPDropShadow.Font.FontFamily, fontSize, FontStyle.Bold)
        lbPDropShadow.BackColor = Color.Transparent
        lbPDropShadow.Parent = PictureBox1
        lbPDropShadow.Text = p

        'Ducat Label
        lbDucats.Location = New Point(-1, 0)
        lbDucats.Font = New Font(lbDucats.Font.FontFamily, fontSize, FontStyle.Bold)
        lbDucats.BackColor = Color.Transparent
        lbDucats.Parent = lbDDropShadow
        lbDucats.Text = d

        'Ducat Label Drop Shadow
        lbDDropShadow.Location = New Point((pSize.X / 2.58) + 2, (pSize.Y / 2.15) + (pSize.Y / 27))
        lbDDropShadow.Font = New Font(lbDDropShadow.Font.FontFamily, fontSize, FontStyle.Bold)
        lbDDropShadow.BackColor = Color.Transparent
        lbDDropShadow.Parent = PictureBox1
        lbDDropShadow.Text = d

        Me.Show()
        Me.Refresh()
    End Sub

    Private Sub tHide_Tick(sender As Object, e As EventArgs) Handles tHide.Tick
        Me.Close()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub
End Class
