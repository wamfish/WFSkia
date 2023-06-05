//  Copyright (C) 2023 - Present John Roscoe Hamilton - All Rights Reserved
//  You may use, distribute and modify this code under the terms of the MIT license.
//  See the file License.txt in the root folder for full license details.

using System.Runtime.InteropServices;
namespace WFSkia;
public class Bitmap : IDisposable
{
    public IntPtr pBitMap { get; private set; }
    public int BytesPerRow => Width * 4;
    public int Width { get; private set; }
    public int Height { get; private set; }

    public void Dispose()
    {
        if (pBitMap != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(pBitMap);
            pBitMap = IntPtr.Zero;
        }
    }
    private unsafe Bitmap(int width, int height)
    {
        Width = width;
        Height = height;
        pBitMap = Marshal.AllocHGlobal(width * height * 4);
        uint color = 0xFF0000FF;
        uint* data = (uint*)pBitMap.ToPointer();
        for (int i = 0; i < width * height; i++)
        {
            *data++ = color;
        }
        data = (uint*)pBitMap.ToPointer();
    }
    public static Bitmap Rent(int width, int height)
    {
        return new Bitmap(width, height);
    }
}
