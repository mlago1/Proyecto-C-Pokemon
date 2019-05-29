using System;

class EquipoJugador : Menu
{
    Jugador prota;

    public EquipoJugador(Jugador prota)
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 30);
        continuar = true;
        this.prota = prota;
        posicionFlecha = 150;
    }

    public void DetectarTeclas()
    {
        if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE))
        {
            SdlHardware.Pause(100);
            continuar = false;
        }
    }

    public override void DibujarInterfaz()
    {
        SdlHardware.DrawHiddenImage(bg,0,0);
        SdlHardware.WriteHiddenText("Equipo",
            100, 50,
            0xC0, 0xC0, 0xC0,
            font24);
        short altura = 150;
        foreach (Bestia b in prota.GetEquipo())
        {
            SdlHardware.WriteHiddenText(b.GetNombre(),
                        150, altura,
                        0xC0, 0xC0, 0xC0,
                        font24);
            altura += 50;
        }
        altura = 150;
        SdlHardware.WriteHiddenText("-->",
           40, Convert.ToInt16(posicionFlecha),
           0xC0, 0xC0, 0xC0,
           font24);
        SdlHardware.DrawHiddenImage(prota.GetEquipo()[seleccion].image,768,150);
        SdlHardware.WriteHiddenText("Lvl: " + prota.GetEquipo()[seleccion].GetNivel(),
           550, 150,
           0xC0, 0xC0, 0xC0,
           font24);
        SdlHardware.WriteHiddenText("Hp: " + prota.GetEquipo()[seleccion].GetVida(),
           550, 200,
           0xC0, 0xC0, 0xC0,
           font24);

        SdlHardware.WriteHiddenText("Pulsa <-- para salir",
                100, 450,
                0xC0, 0xC0, 0xC0,
                font24);

        SdlHardware.ShowHiddenScreen();

        if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
        {
            if (seleccion == prota.GetEquipo().Count - 1)
            {
                seleccion = 0;
                posicionFlecha = 150;
            }
            else
            {
                seleccion++;
                posicionFlecha += 50;
            }
        }
        if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
        {
            if (seleccion == 0)
            {
                seleccion = prota.GetEquipo().Count - 1;
                posicionFlecha = Convert.ToInt16(150 + ((prota.GetEquipo().Count - 1) * 50));
            }
            else
            {
                seleccion--;
                posicionFlecha -= 50;
            }
        }

        SdlHardware.Pause(40);


    }
    public void Run()
    {
        do
        {
            DibujarInterfaz();
            DetectarTeclas();
        } while (continuar);
    }
}