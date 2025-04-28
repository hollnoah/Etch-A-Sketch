'Noah Holloway
'RCET 2265
'Spring 2025
'Etch-A-Sketch
'

Option Strict On
Option Explicit On
Option Compare Text

Public Class EtchASketch
    Function ForeGroundColor(Optional newColor As Color = Nothing) As Color
        Static _foreColor As Color = Color.Black
        If newColor <> Nothing Then _foreColor = newColor
        Return _foreColor
    End Function

    Function PenWidth(Optional newWidth As Integer = -1) As Integer
        Static _penWidth As Integer = 2
        If newWidth > 0 AndAlso newWidth <= 100 Then _penWidth = newWidth
        Return _penWidth
    End Function

    'Draw line
    Sub DrawWithMouse(oldX As Integer, oldY As Integer, newX As Integer, newY As Integer)
        Dim g As Graphics = DisplayPictureBox.CreateGraphics()
        Dim pen As New Pen(ForeGroundColor(), PenWidth())
        g.DrawLine(pen, oldX, oldY, newX, newY)
        g.Dispose()
    End Sub

    Sub DrawGraticule()
        Dim g As Graphics = DisplayPictureBox.CreateGraphics()
        Dim pen As New Pen(Color.LightGray)

        Dim spacingX As Double = DisplayPictureBox.Width / 10.0
        Dim spacingY As Double = DisplayPictureBox.Height / 10.0

        For i As Integer = 1 To 9
            g.DrawLine(pen, CInt(i * spacingX), 0, CInt(i * spacingX), DisplayPictureBox.Height)
            g.DrawLine(pen, 0, CInt(i * spacingY), DisplayPictureBox.Width, CInt(i * spacingY))
        Next

        g.Dispose()
    End Sub

    'draw waveforms
    Sub DrawWaveforms()
        Dim g As Graphics = DisplayPictureBox.CreateGraphics()
        Dim width = DisplayPictureBox.Width
        Dim height = DisplayPictureBox.Height
        Dim centerY = height \ 2
        Dim degreesPerPixel = 360 / width

        Dim oldX As Integer = 0
        Dim oldSinY As Integer = centerY
        Dim oldCosY As Integer = centerY
        Dim oldTanY As Integer = centerY

        Dim sinPen As New Pen(Color.Red, 2)
        Dim cosPen As New Pen(Color.Blue, 2)
        Dim tanPen As New Pen(Color.Green, 2)

        For x = 0 To width
            Dim radians = Math.PI * x * degreesPerPixel / 180
            Dim sinY = centerY - CInt((height / 2 - 10) * Math.Sin(radians))
            Dim cosY = centerY - CInt((height / 2 - 10) * Math.Cos(radians))
            Dim tanValue = Math.Tan(radians)
            Dim tanY As Integer
            If Math.Abs(tanValue) > 10 Then
                tanY = If(tanValue > 0, 0, height)
            Else
                tanY = centerY - CInt((height / 4) * tanValue)
            End If

            g.DrawLine(sinPen, oldX, oldSinY, x, sinY)
            g.DrawLine(cosPen, oldX, oldCosY, x, cosY)
            g.DrawLine(tanPen, oldX, oldTanY, x, tanY)

            oldX = x
            oldSinY = sinY
            oldCosY = cosY
            oldTanY = tanY
        Next

        g.Dispose()
    End Sub

    'mouse move
    Private Sub DisplayPictureBox_MouseMove(sender As Object, e As MouseEventArgs) Handles DisplayPictureBox.MouseMove, DisplayPictureBox.MouseDown
        Static oldX, oldY As Integer

        Select Case e.Button
            Case MouseButtons.Left
                DrawWithMouse(oldX, oldY, e.X, e.Y)
            Case MouseButtons.Middle
                SelectColor()
        End Select

        oldX = e.X
        oldY = e.Y
    End Sub

    'select color 
    Sub SelectColor()
        Dim cd As New ColorDialog()
        If cd.ShowDialog() = DialogResult.OK Then
            ForeGroundColor(cd.Color)
        End If
    End Sub

    'Clear screen and shake
    Sub ClearScreen()
        Try
            For i = 1 To 10
                Me.Top += If(i Mod 2 = 0, 10, -10)
                Me.Left += If(i Mod 2 = 0, 10, -10)
                Threading.Thread.Sleep(50)
            Next
        Catch
        End Try
        DisplayPictureBox.Refresh()
    End Sub

    'menu and button clicks*******************************************************************************************************************************
    Private Sub SelectColorButton_Click(sender As Object, e As EventArgs) Handles SelectColorButton.Click, SelectColorContextItem.Click, SelectColorContextMenuItem.Click
        SelectColor()
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click, ClearContextItem.Click, ClearContextMenuItem.Click
        ClearScreen()
    End Sub

    Private Sub DrawWaveformsButton_Click(sender As Object, e As EventArgs) Handles DrawWaveformsButton.Click, DrawWaveformsContextItem.Click
        DisplayPictureBox.Refresh()
        DrawGraticule()
        DrawWaveforms()
    End Sub

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        MessageBox.Show("Etch-A-Sketch Program Version 1.0" & vbCrLf & "Created by Noah Holloway", "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    'tooltips
    Private Sub EtchASketchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ToolTip1.SetToolTip(DisplayPictureBox, "Draw by dragging with Left Mouse Button. Middle-click to select color.")
        ToolTip1.SetToolTip(SelectColorButton, "Select Drawing Color")
        ToolTip1.SetToolTip(DrawWaveformsButton, "Draw Graticule and Waveforms")
        ToolTip1.SetToolTip(ClearButton, "Clear Drawing Area")
        ToolTip1.SetToolTip(ExitButton, "Exit the Program")

        'Tab Order
        SelectColorButton.TabIndex = 0
        DrawWaveformsButton.TabIndex = 1
        ClearButton.TabIndex = 2
        ExitButton.TabIndex = 3

        'Accept (Enter) = DrawWaveforms
        Me.AcceptButton = DrawWaveformsButton

        'Cancel (Esc) = Clear
        Me.CancelButton = ClearButton
    End Sub


End Class
