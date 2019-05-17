using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Bienvenida
{
    Image bg;
    Font font24;

    public Bienvenida()
    {
        bg = new Image("data/pantalla_principal.jpg");
        font24 = new Font("data/Joystix.ttf", 24);
    }

    public void Run()
    {
        do
        {
            SdlHardware.ClearScreen();

            SdlHardware.DrawHiddenImage(bg, 0, 0);

            SdlHardware.ShowHiddenScreen();

            if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
                {
                MenuPartidas m = new MenuPartidas();
                m.Run();
            }

            SdlHardware.Pause(1);

        } while (true);
    }
}


