using System.Collections.Generic;
using System.IO;
using System;

abstract class Menu
{
    protected Image bg;
    protected Font font24;
    protected int seleccion, maxOpciones, scrollX, scrollY;
    protected short posicionFlecha;
    protected List<string> listaNombres;
    protected bool continuar;
    protected Sprite fondo, dialogo;

    public Menu()
    {
        fondo = new Sprite("data/fondo_juego.jpg");
        dialogo = new Sprite("data/fondo_dialogo.png");
        scrollX = scrollY = 0;
        dialogo.MoveTo(0,550);
    }

    protected void SinPartidas()
    {
        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("No hay partidas disponibles",
                50, 100,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa <-- para volver al menu principal",
                50, 300,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.ShowHiddenScreen();
        }
        while (!SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE));
        SdlHardware.Pause(100);
        Pokemon.Run();
    }

    protected void CargarNombres(ref List<string> listaNombres)
    {
        try
        {
            listaNombres = new List<string>(File.ReadAllLines("partidas/listaPartidas.txt"));
        }
        catch (Exception e)
        {
            Menu.Error();
        }

        for(int i = 0; i < listaNombres.Count; i++)
        {
            if (listaNombres[i].Length < 0)
                listaNombres.RemoveAt(i);
        }
    }

    public virtual void DibujarInterfaz()
    {

    }

    public static void Error()
    {
        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(new Image("data/menu_partidas.png"), 0, 0);
            SdlHardware.WriteHiddenText("Error Fatal",
                    100, 50,
                    0xC0, 0xC0, 0xC0,
                    new Font("data/Joystix.ttf", 24));
            SdlHardware.WriteHiddenText("Pulsa Espacio para volver",
                    100, 100,
                    0xC0, 0xC0, 0xC0,
                    new Font("data/Joystix.ttf", 24));
            SdlHardware.WriteHiddenText("a la pantalla principal",
                    100, 150,
                    0xC0, 0xC0, 0xC0,
                    new Font("data/Joystix.ttf", 24));
            SdlHardware.ShowHiddenScreen();

        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        SdlHardware.Pause(100);
        Pokemon.Run();
    }
}
