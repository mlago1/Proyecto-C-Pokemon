using System.IO;
using System;
using System.Collections.Generic;

class BorrarPartida : Menu
{ 
    protected bool partidaElegida;
    protected string nombrePartida;

    public BorrarPartida()
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 20);
        seleccion = 1;
        posicionFlecha = 200;
        partidaElegida = false;
        CargarNombres(ref listaNombres);
        try
        {
            maxOpciones = listaNombres.Count;
        }
        catch (Exception e)
        {
            maxOpciones = 0;
        }
    }

    public void BorrarPartidaSeleccionada()
    {
        File.Delete("partidas/" + nombrePartida + ".txt");
        List<string> leerPartidas = new List<string>(File.ReadAllLines("partidas/listaPartidas.txt"));
        leerPartidas.Remove(nombrePartida);
        File.WriteAllLines("partidas/listaPartidas.txt", leerPartidas);
        SdlHardware.Pause(100);
        MenuPartidas m = new MenuPartidas();
        m.Run();
    }

    public override void DibujarInterfaz()
    {
        if (maxOpciones == 0)
        {
            SinPartidas();
        }
        else
        {
            do
            {
                SdlHardware.ClearScreen();
                SdlHardware.DrawHiddenImage(bg, 0, 0);
                SdlHardware.WriteHiddenText("Elige una partida",
                    100, 50,
                    0xC0, 0xC0, 0xC0,
                    font24);
                SdlHardware.WriteHiddenText("Pulsa <-- para volver",
                    100, 100,
                    0xC0, 0xC0, 0xC0,
                    font24);

                short altura = 200;
                foreach (string s in listaNombres)
                {
                    if (s.Length > 0)
                    {
                        SdlHardware.WriteHiddenText(s,
                        100, altura,
                        0xC0, 0xC0, 0xC0,
                        font24);
                        altura += 100;
                    }
                    else
                        SinPartidas();
                }

                SdlHardware.WriteHiddenText("<--",
                        500, posicionFlecha,
                        0xC0, 0xC0, 0xC0,
                        font24);

                SdlHardware.ShowHiddenScreen();

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
                        posicionFlecha += 100;
                    }
                }
                if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
                {
                    if (seleccion == 1)
                    {
                        seleccion = maxOpciones;
                        posicionFlecha = Convert.ToInt16(50 * maxOpciones);
                    }
                    else
                    {
                        seleccion--;
                        posicionFlecha -= 100;
                    }
                }
                SdlHardware.ShowHiddenScreen();
                if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
                {
                    partidaElegida = true;
                    nombrePartida = listaNombres[seleccion - 1];
                    SdlHardware.Pause(100);
                }
                if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE))
                {
                    SdlHardware.Pause(100);
                    Pokemon.Run();
                }
                SdlHardware.Pause(40);
            } while (!partidaElegida);
        }
    }

    public void Run()
    {
            DibujarInterfaz();
            BorrarPartidaSeleccionada();
    }
}
