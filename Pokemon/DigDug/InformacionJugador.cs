class InformacionJugador : Menu
{
    Jugador prota;
    bool continuar;

    public InformacionJugador(Jugador prota)
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 30);
        continuar = true;
        this.prota = prota;
    }

    public void DetectarTeclas()
    {
        SdlHardware.ShowHiddenScreen();
        if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            SdlHardware.Pause(100);
            continuar = false;
        }
    }

    public override void DibujarInterfaz()
    {

        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        SdlHardware.WriteHiddenText("Nombre: " + prota.GetNombre(),
            100, 50,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Genero: " + prota.GetGenero(),
            100, 100,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Dinero: " + prota.GetDinero(),
            100, 150,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Nº Pokemons diferentes ",
            100, 200,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("atrapados: 0", //PROVISIONAL
            100, 250,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Medallas: 0", //PROVISIONAL
           100, 300,
           0xC0, 0xC0, 0xC0,
           font24);
        SdlHardware.WriteHiddenText("Tiempo Jugado: " + prota.GetTiempoJugado(),
           100, 350,
           0xC0, 0xC0, 0xC0,
           font24);

        SdlHardware.WriteHiddenText("Pulsa Espacio para salir",
                100, 450,
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
