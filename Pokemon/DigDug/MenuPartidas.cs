using System;

class MenuPartidas : Menu
{
    public MenuPartidas()
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 35);
        seleccion = 0;
        posicionFlecha = 100;
        maxOpciones = 2;
        continuar = true;
    }

    public override void DibujarInterfaz()
    {
        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        SdlHardware.WriteHiddenText("Nueva Partida",
            100, 100,
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.WriteHiddenText("Cargar Partida",
            100, 200,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Borrar Partida",
            100, 300,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("<--",
            600, Convert.ToInt16(posicionFlecha),
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.ShowHiddenScreen();
    }

    private void DetectarTeclas()
    {
        if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
        {
            if (seleccion == maxOpciones)
            {
                seleccion = 0;
                posicionFlecha = 100;
            }
            else
            {
                seleccion++;
                posicionFlecha += 100;
            }
        }
        if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
        {
            if (seleccion == 0)
            {
                seleccion = maxOpciones;
                posicionFlecha = Convert.ToInt16(100 + (maxOpciones * 100));
            }
            else
            {
                seleccion--;
                posicionFlecha -= 100;
            }
        }
        if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            SdlHardware.Pause(100);
            switch (seleccion)
            {
                case 0: NuevaPartida np = new NuevaPartida(); np.Run(); break;
                case 1:
                    CargarPartida cp = new CargarPartida(); cp.Run()
                 ; break;
                case 2:
                    BorrarPartida bp = new BorrarPartida(); bp.Run()
                 ; break;
            }
        }
    }

    public void Run()
    {
        do
        {
            DibujarInterfaz();
            DetectarTeclas();
            SdlHardware.Pause(100);
        } while (continuar);
    }

}

