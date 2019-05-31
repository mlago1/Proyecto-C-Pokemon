using System;
using System.Collections.Generic;
using System.IO;

class Pokedex : Menu
{
    Jugador prota;
    Dictionary<int, string> pokedex;
    Dictionary<string, Image> dexImagenes;
    Font select;

    public Pokedex(Jugador prota)
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 25);
        select = new Font("data/Joystix.ttf", 40);
        continuar = true;
        this.prota = prota;
        seleccion = 0;
        pokedex = new Dictionary<int, string>();
        dexImagenes = new Dictionary<string, Image>();
        CargarPokedex();
    }

    private void CargarPokedex()
    {
        try
        {
            string[] leer = File.ReadAllLines("data/pokemons/lista_pokemon.txt");
            for(int i = 0; i < leer.Length; i++)
            {
                pokedex.Add(i,leer[i].Split(';')[0]);
                dexImagenes.Add(leer[i].Split(';')[0], new Image(leer[i].Split(';')[1]));
            }

        }
        catch (Exception e)
        {
            Menu.Error();
        }
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
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        SdlHardware.WriteHiddenText("Pokedex",
            100, 50,
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.WriteHiddenText("Usa ARRIBA O ABAJO para desplazarte",
            100, 100,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText((seleccion + 1)+" de "+pokedex.Count,
            100, 150,
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.WriteHiddenText(pokedex[seleccion],
           100, 250,
           0xC0, 0xC0, 0xC0,
           select);
        SdlHardware.DrawHiddenImage(dexImagenes[pokedex[seleccion]],400, 250);

        SdlHardware.WriteHiddenText("Pulsa <-- para salir",
                100, 600,
                0xC0, 0xC0, 0xC0,
                font24);

        SdlHardware.ShowHiddenScreen();

        if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
        {
            if(seleccion == pokedex.Count - 1)
            {
                seleccion = 0;
            }
            else
            {
                seleccion++;
            }
        }
        if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
        {
            if (seleccion == 0)
            {
                seleccion = pokedex.Count - 1;
            }
            else
            {
                seleccion--;
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
