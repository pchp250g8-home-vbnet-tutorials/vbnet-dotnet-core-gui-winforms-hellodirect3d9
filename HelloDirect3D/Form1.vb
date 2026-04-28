Imports SlimDX
Imports SlimDX.Direct3D9
Public Class Form1
    Private m_oD3D As Direct3D
    Private m_oD3D_Device As Device
    Private m_d3dpp As PresentParameters
    Private nRedColor As Byte
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        nRedColor = 0
        If (Not InitializeDirect3D()) Then
            MessageBox.Show("Failed Loading DirectX", "Error")
            Application.Exit()
            Return
        End If
        SetStyle(ControlStyles.Opaque, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        AddHandler Application.Idle, AddressOf Application_Idle
    End Sub
    Private Sub Application_Idle(sender As Object, e As EventArgs)
        Invalidate()
        nRedColor += 1
    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        DestroyDirect3D()
    End Sub
    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        RenderScene()
    End Sub
    Private Function InitializeDirect3D() As Boolean
        Dim bResult As Boolean = False
        m_d3dpp = New PresentParameters()
        m_d3dpp.SwapEffect = SwapEffect.Discard
        m_d3dpp.Windowed = True
        m_d3dpp.EnableAutoDepthStencil = False
        Try
            m_oD3D = New Direct3D()
            Dim nAdapter = m_oD3D.Adapters.DefaultAdapter.Adapter
            Dim d3ddm = m_oD3D.GetAdapterDisplayMode(nAdapter)
            m_d3dpp.BackBufferFormat = d3ddm.Format
            m_oD3D_Device = New Device _
            (
                m_oD3D, nAdapter,
                DeviceType.Hardware,
                Handle,
                CreateFlags.SoftwareVertexProcessing,
                m_d3dpp
            )
            bResult = Not (m_oD3D_Device Is Nothing)
        Catch
            bResult = False
        End Try
        Return bResult
    End Function
    Private Sub DestroyDirect3D()
        If Not (m_oD3D_Device Is Nothing) Then
            m_oD3D_Device.Dispose()
        End If
        If Not (m_oD3D! Is Nothing) Then
            m_oD3D.Dispose()
        End If
    End Sub
    Private Sub RenderScene()
        If (m_oD3D Is Nothing) Then Return
        If (m_oD3D_Device Is Nothing) Then Return
        Dim gdipColor As Color = Color.FromArgb(nRedColor, 0, 0)
        Dim d3dColor As Color4 = New Color4(gdipColor)
        m_oD3D_Device.Clear(ClearFlags.Target, d3dColor, 0.0F, 0)
        m_oD3D_Device.BeginScene()
        m_oD3D_Device.EndScene()
        m_oD3D_Device.Present()
    End Sub
End Class
