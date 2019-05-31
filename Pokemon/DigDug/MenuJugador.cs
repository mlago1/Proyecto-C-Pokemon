using System;
using System.Collections.Generic;

class MenuJugador : Menu
{
    Jugador prota;
    Juego juego;

    public MenuJugador(Jugador prota, Sprite fondo, Sprite dialogo, Juego juego)
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 30);
        seleccion = 1;
        posicionFlecha = 200;
        maxOpciones = 7;
        continuar = true;
        this.prota = prota;
        this.fondo = fondo;
        this.dialogo = dialogo;
        this.juego = juego;
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
            SdlHardware.WriteHiddenText("Pulsa Espacio para salir de ",
                100, 200,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("la partida",
                100, 250,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa <-- para volver atrás",
                100, 400,
                0xC0, 0xC0, 0xC0,
                font24);

            if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
            {
                SdlHardware.Pause(100);
                Pokemon.Run();
            }

            SdlHardware.ShowHiddenScreen();

        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC) &&
            !SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE));
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
                case 1:;Pokedex p = new Pokedex(prota); p.Run(); break;
                case 2: EquipoJugador ej = new EquipoJugador(prota); ej.Run() ; break;
                case 3:VerMochila(); break;
                case 4: InformacionJugador ij = new InformacionJugador(prota); ij.Run() ; break;
                case 5: prota.guardarJugador("partidas/" + prota.GetNombre() +
                    ".txt", ref fondo, ref dialogo, juego.viejoScrollX, juego.viejoScrollY);
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

    private void VerMochila()
    {
        int actual = 0;
        int maxOpciones = prota.GetMochila().Count - 1;
        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("Mochila",
                100, 50,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa <-- para volver atrás",
                100, 100,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa ARRIBA o ABAJO para ", 
                100, 200,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("pasar objetos", 
                100, 250,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText((actual + 1) + "/" + prota.GetMochila().Count,
                100, 300,
                0xC0, 0xC0, 0xC0,
                font24);
            int i = 0;
            foreach (KeyValuePair<Objeto,int> kp in prota.GetMochila())
            {
                if (i == actual)
                {
                    SdlHardware.WriteHiddenText(kp.Key.Nombre + " x" + kp.Value,
                        100, 350,
                        0xC0, 0xC0, 0xC0,
                        font24);
                }
                i++;
            }
            SdlHardware.ShowHiddenScreen();
            if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
            {
                if (actual == maxOpciones)
                    actual = 0;
                else
                    actual++;
            }
            if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
            {
                if (actual == 0)
                    actual = maxOpciones;
                else
                    actual--;
            }
            SdlHardware.Pause(100);
        } while (!SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE));
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
        SdlHardware.WriteHiddenText("Volver a la partida",
            100, 500,
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.WriteHiddenText("-->",
                30, posicionFlecha,
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
        SdlHardware.ScrollTo(juego.viejoScrollX,juego.viejoScrollY);
    }
}
