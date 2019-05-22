/**
 * Image.cs - To hide SDL image handling
 * 
 * Changes:
 * 0.01, 24-jul-2013: Initial version, based on SdlMuncher 0.02
 */



using System;
using Tao.Sdl;


public class Image
{
    private IntPtr internalPointer;
    public string nombre { get; set; }

    public Image(string fileName)  // Constructor
    {
        nombre = fileName;
        Load(fileName);
    }

    public void Load(string fileName)
    {
        internalPointer = SdlImage.IMG_Load(fileName);
        if (internalPointer == IntPtr.Zero)
            SdlHardware.FatalError("Image not found: " + fileName);
    }


    public IntPtr GetPointer()
    {
        return internalPointer;
    }
}

