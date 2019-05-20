using System;

class MenuJugador : Menu
{
    bool continuar;
    Jugador prota;
    Mapa mapa;

    public MenuJugador(Jugador prota, Mapa mapa)
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 30);
        seleccion = 1;
        posicionFlecha = 200;
        maxOpciones = 7;
        continuar = true;
        this.prota = prota;
        this.mapa = mapa;
    }

    private void GuardadoCompletado()
    {
        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("La partida se ha guardado",
                100, 50,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("satisfactoriamente",
                100, 100,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa Espacio para",
                100, 150,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("volver",
                100, 200,
                0xC0, 0xC0, 0xC0,
                font24);

            if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
            {
                SdlHardware.Pause(100);
            }

        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
    }

    public void DetectarTeclas()
    {
        if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
        {
            if (seleccion == maxOpciones)
            {
                seleccion = 1;
                posicionFlecha = 200;
            }
            else
            {
                seleccion++;
                posicionFlecha += 50;
            }
        }
        if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
        {
            if (seleccion == 1)
            {
                seleccion = maxOpciones;
                posicionFlecha = 500;
            }
            else
            {
                seleccion--;
                posicionFlecha -= 50;
            }
        }
        SdlHardware.ShowHiddenScreen();
        if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            
            switch(seleccion)
            {
                case 1:;break;
                case 2:; break;
                case 3:; break;
                case 4: InformacionJugador ij = new InformacionJugador(prota); ij.Run() ; break;
                case 5: prota.guardarJugador("partidas/" + prota.GetNombre() + ".txt",ref mapa);
                    GuardadoCompletado(); break;
                case 6: Instrucciones i = new Instrucciones() ; i.Run() ; break;
                case 7: continuar = false; ; break;
            }  
            SdlHardware.Pause(100);
        }
        if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE))
        {
            SdlHardware.Pause(100);
            continuar = false;
        }
    }

    public override void DibujarInterfaz()
    {

        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        SdlHardware.WriteHiddenText("Menú",
            100, 50,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Pulsa <-- para volver",
            100, 100,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Pokedex",
            100, 200,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Equipo",
            100, 250,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Mochila",
            100, 300,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Entrenador",
            100, 350,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Guardar Partida",
            100, 400,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Instrucciones",
            100, 450,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Salir",
            100, 500,
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.WriteHiddenText("<--",
                500, posicionFlecha,
                0xC0, 0xC0, 0xC0,
                font24);

        SdlHardware.ShowHiddenScreen();

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
