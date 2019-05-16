using System;

class MenuPartidas : Menu
{
    public MenuPartidas()
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 35);
        seleccion = 0;
        posicionFlecha = 100;
    }

    public void Run()
    {
        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("Nueva Partida",
                100, 100,
                0xC0, 0xC0, 0xC0,
                font24);
            
            SdlHardware.WriteHiddenText("Cargar Partida",
                100, 300,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Borrar Partida",
                100, 500,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("<--",
                600, Convert.ToInt16(posicionFlecha),
                0xC0, 0xC0, 0xC0,
                font24);

            SdlHardware.ShowHiddenScreen();

            if(SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
            {
                if(seleccion == 2)
                {
                    seleccion = 0;
                    posicionFlecha = 100;
                }
                else
                {
                    seleccion++;
                    posicionFlecha += 200;
                }
            }
            if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
            {
                if (seleccion == 0)
                {
                    seleccion = 2;
                    posicionFlecha = 450;
                }
                else
                {
                    seleccion--;
                    posicionFlecha -= 200;
                }
            }
            if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
            {
                SdlHardware.Pause(100);
                switch (seleccion)
                {
                    case 0: NuevaPartida np = new NuevaPartida(); np.Run(); break;
                    case 1: CargarPartida cp = new CargarPartida(); cp.Run()
                         ; break;
                    case 2:BorrarPartida bp = new BorrarPartida(); bp.Run()
                        ; break;
                }
            }

            SdlHardware.Pause(40);
        } while (true);
    }

}

