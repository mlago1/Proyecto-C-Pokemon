using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Pokemon
{
    public static void Main()
    {
        bool fullScreen = false;
        SdlHardware.Init(1024, 768, 24, fullScreen);
        Bienvenida b = new Bienvenida();
        b.Run();
    }
}


