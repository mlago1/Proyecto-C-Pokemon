using System.IO;
using System;

class NuevaPartida : Menu
{
    bool nombreIntroducido;
    bool generoIntroducido;
    int indiceGenero;
    string nombrePJ,genero, errorNombre;
    Image bg2;
    Mapa cargarMapa;

    public NuevaPartida()
    {
        nombreIntroducido = false;
        generoIntroducido = false;
        bg = new Image("data/menu_partidas.png");
        bg2 = new Image("data/menu_seleccion_sprite.jpg");
        font24 = new Font("data/Joystix.ttf", 35);
        string[] leer = File.ReadAllLines("partidas/listaPartidas.txt");
        nombrePJ = " ";
        indiceGenero = 0;
        posicionFlecha = 300;
        genero = "Hombre";
        CargarNombres(ref listaNombres);
        errorNombre = "El nombre no puede estar vacio";
        cargarMapa = new Mapa();
        cargarMapa.CargarMapa("data/mapa.txt");
    }

    public void IntroducirTecla()
    {
        if (SdlHardware.KeyPressed(SdlHardware.KEY_A))
        {
            nombrePJ += "a";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_B))
        {
            nombrePJ += "b";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_C))
        {
            nombrePJ += "c";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_D))
        {
            nombrePJ += "d";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_E))
        {
            nombrePJ += "e";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_F))
        {
            nombrePJ += "f";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_G))
        {
            nombrePJ += "g";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_H))
        {
            nombrePJ += "h";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_I))
        {
            nombrePJ += "i";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_J))
        {
            nombrePJ += "j";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_K))
        {
            nombrePJ += "k";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_L))
        {
            nombrePJ += "l";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_M))
        {
            nombrePJ += "m";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_N))
        {
            nombrePJ += "n";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_O))
        {
            nombrePJ += "o";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_P))
        {
            nombrePJ += "p";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_Q))
        {
            nombrePJ += "q";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_R))
        {
            nombrePJ += "r";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_S))
        {
            nombrePJ += "s";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_T))
        {
            nombrePJ += "t";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_U))
        {
            nombrePJ += "u";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_V))
        {
            nombrePJ += "v";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_W))
        {
            nombrePJ += "w";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_X))
        {
            nombrePJ += "x";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_Y))
        {
            nombrePJ += "y";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_Z))
        {
            nombrePJ += "z";
        }
        else if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE) && nombrePJ.Length > 1)
        {
            nombrePJ = nombrePJ.Substring(0, nombrePJ.Length - 1);
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            if (nombrePJ.Length == 1)
            {
                errorNombre = "El nombre no puede estar vacio";
            }
            else if (listaNombres.Contains(nombrePJ.Trim()))
            {
                errorNombre = "El nombre ya existe";
            }
            else
                nombreIntroducido = true;

            
        }
        SdlHardware.Pause(1);
    }

    public void CrearPartida()
    {
        nombrePJ = nombrePJ.Trim();
        Jugador nuevoJugador = new Jugador(nombrePJ, genero);
        try
        {
            string[] leer = File.ReadAllLines("data/pokemons/lista_pokemon.txt");
            int r = new Random().Next(1, leer.Length);
            nuevoJugador.GetEquipo().Add(new Bestia(leer[r].Split(';')[0], leer[r].Split(';')[1]));
        }
        catch (Exception e)
        {

        }
        nuevoJugador.guardarJugador("partidas/" + nombrePJ + ".txt", ref cargarMapa);
        StreamWriter escribir = new StreamWriter("partidas/listaPartidas.txt", true);
        escribir.WriteLine(nombrePJ);
        escribir.Close();
        //GuardarNpcsPredefinidos y cargarlos -- FUTURO
        Instrucciones i = new Instrucciones();
        i.Run();
        Juego j = new Juego(nuevoJugador, cargarMapa);
        j.Run();
    }

    public void IntroducirNombre()
    {
        do
        {
            if (!nombreIntroducido)
            {
                SdlHardware.ClearScreen();
                SdlHardware.DrawHiddenImage(bg, 0, 0);
                SdlHardware.WriteHiddenText("Introduce tu nombre. ",
                    100, 100,
                    0xC0, 0xC0, 0xC0,
                    font24);
                SdlHardware.WriteHiddenText("Pulsa espacio al terminar.",
                    50, 200,
                    0xC0, 0xC0, 0xC0,
                    font24);
                SdlHardware.WriteHiddenText(nombrePJ,
                    100, 300,
                    0xC0, 0xC0, 0xC0,
                    font24);
                SdlHardware.WriteHiddenText(errorNombre,
                    100, 400,
                    0xC0, 0xC0, 0xC0,
                    font24);
                SdlHardware.ShowHiddenScreen();
                IntroducirTecla();
            }
            SdlHardware.Pause(100);
        } while (!nombreIntroducido);
    }

    public void SeleccionGenero()
    {
        do
        {
            if (!generoIntroducido)
            {
                SdlHardware.ClearScreen();
                SdlHardware.DrawHiddenImage(bg2, 0, 0);
                SdlHardware.WriteHiddenText("<--",
                        500, posicionFlecha,
                        0xC0, 0xC0, 0xC0,
                        font24);
                SdlHardware.ShowHiddenScreen();
                if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
                {
                    if (indiceGenero == 0)
                        indiceGenero++;
                    else
                        indiceGenero = 0;
                    posicionFlecha = posicionFlecha == 300 ? Convert.ToInt16(posicionFlecha + 250) : Convert.ToInt16(300);
                    SdlHardware.Pause(1);
                }
                if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
                {
                    if(indiceGenero == 1)
                        indiceGenero--;
                    else
                        indiceGenero = 1;
                    posicionFlecha = posicionFlecha == 550 ? Convert.ToInt16(posicionFlecha - 250) : Convert.ToInt16(550);
                    SdlHardware.Pause(1);
                }
                if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
                {
                    generoIntroducido = true;
                    SdlHardware.Pause(1);
                }
                genero = indiceGenero == 0 ? "Hombre" : "Mujer";
            }
            SdlHardware.Pause(100);
        } while (!generoIntroducido);
    }

    public void Run()
    {
        IntroducirNombre();
        SeleccionGenero();
        CrearPartida();
    }
}
