using System.Collections.Generic;
using System.IO;
using System;

abstract class Menu
{
    protected Image bg;
    protected Font font24;
    protected int seleccion, maxOpciones;
    protected short posicionFlecha;
    protected List<string> listaNombres;
    protected bool continuar;

    public void SinPartidas()
    {
        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("No hay partidas disponibles",
                50, 100,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa espacio para volver al menu principal",
                50, 300,
                0xC0, 0xC0, 0xC0,
                font24);
            if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
            {
                SdlHardware.Pause(100);
                Pokemon.Run();
            }
        }
        while (true);
    }

    public void CargarNombres(ref List<string> listaNombres)
    {
        try
        {
            listaNombres = new List<string>(File.ReadAllLines("partidas/listaPartidas.txt"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
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
}
